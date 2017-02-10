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
        private readonly IFullDataSource _originalDataSource;
        private readonly ISimpleDataSource _incomingDataSource;

        public Uploader(IFullDataSource originalDataSource, ISimpleDataSource incomingDataSource)
        {
            _originalDataSource = originalDataSource;
            _incomingDataSource = incomingDataSource;
        }

        public string DateAddedText { get; set; }

        public async Task<IEnumerable<BookFilesInfo>> Upload(IEnumerable<MatchInfo> incoming)
        {
            IEnumerable<BookFilesInfo> contents = await GetBookFilesInfosToUpload(incoming);

            var repo = new BookRepository(_originalDataSource);

            await repo.SaveBooks(contents);

            return contents;
        }

        async Task<IEnumerable<BookFilesInfo>> GetBookFilesInfosToUpload(IEnumerable<MatchInfo> incoming)
        {
            IEnumerable<MatchInfo> toUpload = incoming
                .Where(b => b.IsSelected)
                .Where(b => (b.Status == MatchStatus.NewBook || b.Status == MatchStatus.NewFiles));

            var tasks = toUpload.Select(async (b) => await _incomingDataSource.GetBookContent(b.Book));

            BookFilesInfo[] contents = await Task.WhenAll(tasks);

            var dateAddedProvider = new DateAddedProvider(this.DateAddedText);

            dateAddedProvider.SetDateTimeAdded(contents);

            return contents;
        }
    }
}
