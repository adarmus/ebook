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
        private readonly BookRepository _repo;

        public Uploader(IFullDataSource originalDataSource, ISimpleDataSource incomingDataSource)
        {
            _incomingDataSource = incomingDataSource;
            _repo = new BookRepository(originalDataSource);
        }

        public event EventHandler<BookFileEventArgs> BookContentRead;

        protected virtual void OnBookContentRead(BookFileEventArgs args)
        {
            if (BookContentRead != null)
                BookContentRead(this, args);
        }

        public event EventHandler<BookEventArgs> BookUploaded;

        protected virtual void OnBookUploaded(BookEventArgs args)
        {
            if (BookUploaded != null)
                BookUploaded(this, args);
        }

        public string DateAddedText { get; set; }

        public async Task<IEnumerable<BookFilesInfo>> Upload(IEnumerable<MatchInfo> incoming)
        {
            IEnumerable<BookFilesInfo> contents = await GetBookFilesInfosToUpload(incoming);

            await SaveBooks(contents);

            return contents;
        }

        async Task SaveBooks(IEnumerable<BookFilesInfo> books)
        {
            foreach (var book in books)
            {
                try
                {
                    await _repo.SaveBook(book);

                    OnBookUploaded(new BookEventArgs(book.Book));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
        }

        async Task<IEnumerable<BookFilesInfo>> GetBookFilesInfosToUpload(IEnumerable<MatchInfo> incoming)
        {
            IEnumerable<MatchInfo> toUpload = incoming
                .Where(b => b.IsSelected)
                .Where(b => (b.Status == MatchStatus.NewBook || b.Status == MatchStatus.NewFiles));

            var tasks = toUpload.Select(async (b) =>
            {
                var content = await _incomingDataSource.GetBookContent(b.Book);

                foreach (var file in content.Files)
                {
                    OnBookContentRead(new BookFileEventArgs(file));
                }

                return content;
            });

            BookFilesInfo[] contents = await Task.WhenAll(tasks);

            var dateAddedProvider = new DateAddedProvider(this.DateAddedText);

            dateAddedProvider.SetDateTimeAdded(contents);

            return contents;
        }
    }
}
