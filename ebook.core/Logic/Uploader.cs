using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ebook.core.DataTypes;
using ebook.core.Repo;

namespace ebook.core.Logic
{
    public class Uploader
    {
        private readonly ISimpleDataSource _incomingDataSource;
        private readonly IFullDataSource _originalDataSource;
        private readonly IOutputMessage _messages;

        public Uploader(IFullDataSource originalDataSource, ISimpleDataSource incomingDataSource, IOutputMessage messages)
        {
            _originalDataSource = originalDataSource;
            _incomingDataSource = incomingDataSource;
            _messages = messages;
        }

        public string DateAddedText { get; set; }

        public async Task<IEnumerable<BookFilesInfo>> Upload(IEnumerable<MatchInfo> incoming)
        {
            IEnumerable<MatchInfo> toUpload = incoming
                .Where(b => b.IsSelected);

            IEnumerable<BookFilesInfo> newBooks = await UploadNewBooks(toUpload);

            IEnumerable<BookFilesInfo> newFiles = await UploadNewFiles(toUpload);

            IEnumerable<BookFilesInfo> all = newBooks.Concat(newFiles);

            return all;
        }

        async Task<IEnumerable<BookFilesInfo>> UploadNewBooks(IEnumerable<MatchInfo> incoming)
        {
            IEnumerable<MatchInfo> toUpload = incoming
                .Where(b => b.Status == MatchStatus.NewBook);

            var dateAddedProvider = new DateAddedProvider(this.DateAddedText);

            var uploaded = new List<BookFilesInfo>();

            foreach (var match in toUpload)
            {
                BookFilesInfo content = await _incomingDataSource.GetBookContent(match.Book);

                dateAddedProvider.SetDateTimeAdded(content);

                uploaded.Add(content);

                await SaveBook(content);
            }

            return uploaded;
        }

        async Task<IEnumerable<BookFilesInfo>> UploadNewFiles(IEnumerable<MatchInfo> incoming)
        {
            IEnumerable<MatchInfo> toUpload = incoming
                .Where(b => b.Status == MatchStatus.NewFiles);

            var uploaded = new List<BookFilesInfo>();

            foreach (var match in toUpload)
            {
                BookFilesInfo content = await _incomingDataSource.GetBookContent(match.Book);

                uploaded.Add(content);

                await SaveNewFiles(content, match);
            }

            return uploaded;
        }

        async Task SaveBook(BookFilesInfo book)
        {
            BookInfo info = book.Book;

            if (!string.IsNullOrEmpty(info.Isbn))
                info.Isbn = Isbn.Normalise(info.Isbn);

            await _originalDataSource.SaveBook(info);

            foreach (var file in book.Files)
            {
                await SaveBookFile(file);
            }

            _messages.Write("Uploaded: {0}", book.Book.Title);
        }

        async Task SaveNewFiles(BookFilesInfo book, MatchInfo match)
        {
            foreach (var file in book.Files)
            {
                if (match.NewTypes.Contains(file.FileType))
                {
                    await SaveBookFile(file);
                }
            }

            _messages.Write("Uploaded: {0}", book.Book.Title);
        }

        async Task SaveBookFile(BookFileInfo file)
        {
            var newFile = new BookFileInfo
            {
                Id = Guid.NewGuid().ToString(),
                Content = file.Content,
                BookId = file.BookId,
                FileType = file.FileType,
                FileName = file.FileName
            };

            await _originalDataSource.SaveFile(newFile);

            _messages.Write("Uploaded: {0}", file.FileName);
        }

        string GetFileType(string filepath)
        {
            string ext = Path.GetExtension(filepath);

            if (string.IsNullOrEmpty(ext))
                return null;

            return ext.ToUpper().Substring(1);
        }
    }
}
