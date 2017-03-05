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

        public async Task<UploadResults> Upload(IEnumerable<MatchInfo> incoming)
        {
            IEnumerable<MatchInfo> toUpload = incoming
                .Where(b => b.IsSelected);

            UploadResults newResults = await UploadNewBooks(toUpload);

            UploadResults fileResults = await UploadNewFiles(toUpload);

            return new UploadResults
            {
                Uploaded = newResults.Uploaded + fileResults.Uploaded,
                Errors = newResults.Errors + fileResults.Errors
            };
        }

        async Task<UploadResults> UploadNewBooks(IEnumerable<MatchInfo> incoming)
        {
            IEnumerable<MatchInfo> toUpload = incoming
                .Where(b => b.Status == MatchStatus.NewBook);

            var dateAddedProvider = new DateAddedProvider(this.DateAddedText);

            int count = 0;
            int errors = 0;

            foreach (var match in toUpload)
            {
                bool ok = await UploadNewBook(match, dateAddedProvider);

                if (ok)
                    count++;
                else
                    errors++;
            }

            return new UploadResults { Uploaded = count, Errors = errors };
        }

        async Task<bool> UploadNewBook(MatchInfo match, DateAddedProvider dateAddedProvider)
        {
            try
            {
                BookFilesInfo content = await _incomingDataSource.GetBookContent(match.Book);

                dateAddedProvider.SetDateTimeAdded(content);

                await SaveBook(content);

                return true;
            }
            catch (Exception ex)
            {
                _messages.WriteError(ex, "uploading {0}", match.Book.Title);
                return false;
            }
        }

        async Task SaveBook(BookFilesInfo book)
        {
            BookInfo info = book.Book;

            if (!string.IsNullOrEmpty(info.Isbn))
                info.Isbn = Isbn.Normalise(info.Isbn);

            await _originalDataSource.SaveBook(info);

            _messages.Write("Added:    {0} - {1} [{2}]", book.Book.Title, book.Book.Author, book.Book.Isbn);

            foreach (var file in book.Files)
            {
                await SaveBookFile(file);
            }
        }

        async Task<UploadResults> UploadNewFiles(IEnumerable<MatchInfo> incoming)
        {
            IEnumerable<MatchInfo> toUpload = incoming
                .Where(b => b.Status == MatchStatus.NewFiles);

            int count = 0;
            int errors = 0;

            foreach (var match in toUpload)
            {
                BookFilesInfo content = await _incomingDataSource.GetBookContent(match.Book);

                bool ok = await SaveNewFiles(content, match);

                if (ok)
                    count++;
                else
                    errors++;
            }

            return new UploadResults { Uploaded = count, Errors = errors };
        }

        async Task<bool> SaveNewFiles(BookFilesInfo book, MatchInfo match)
        {
            try
            {
                foreach (BookFileInfo file in book.Files)
                {
                    if (match.NewTypes.Contains(file.FileType))
                    {
                        file.BookId = match.MatchedBook.Id;

                        await SaveBookFile(file);
                    }
                }

                _messages.Write("Uploaded: {0}", book.Book.Title);
                return true;
            }
            catch (Exception ex)
            {
                _messages.WriteError(ex, "uploading {0}", book.Book.Title);
                return false;
            }
        }

        async Task SaveBookFile(BookFileInfo file)
        {
            try
            {
                var newFile = new BookFileInfo
                {
                    Id = Guid.NewGuid().ToString(),
                    Content = file.Content,
                    BookId = file.BookId,
                    FileType = file.FileType,
                    FilePath = file.FilePath,
                    FileName = file.FileName
                };

                await _originalDataSource.SaveFile(newFile);

                _messages.Write("Uploaded: {0}", file.FileName);
            }
            catch (Exception ex)
            {
                _messages.WriteError(ex, "uploading {0}", file.FilePath);
            }
        }
    }
}
