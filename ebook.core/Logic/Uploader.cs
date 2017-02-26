using System;
using System.Collections.Generic;
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
            IEnumerable<BookFilesInfo> contents = await GetBookFilesInfosToUpload(incoming);

            await SaveBooks(contents);

            return contents;
        }

        async Task<IEnumerable<BookFilesInfo>> GetBookFilesInfosToUpload(IEnumerable<MatchInfo> incoming)
        {
            IEnumerable<MatchInfo> toUpload = incoming
                .Where(b => b.IsSelected)
                .Where(b => (b.Status == MatchStatus.NewBook || b.Status == MatchStatus.NewFiles));

            var dateAddedProvider = new DateAddedProvider(this.DateAddedText);

            var tasks = toUpload.Select(async (b) =>
            {
                BookFilesInfo content = await _incomingDataSource.GetBookContent(b.Book);

                dateAddedProvider.SetDateTimeAdded(content);

                return content;
            });

            BookFilesInfo[] contents = await Task.WhenAll(tasks);

            return contents;
        }

        async Task SaveBooks(IEnumerable<BookFilesInfo> books)
        {
            foreach (var book in books)
            {
                try
                {
                    await SaveBook(book);

                    _messages.Write("Uploaded: {0}", book.Book.Title);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
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
        }
    }
}
