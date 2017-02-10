using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ebook.core.DataTypes;
using ebook.core.Repo;

namespace ebook.core.Logic
{
    public class Matcher
    {
        private readonly IFullDataSource _originalDataSource;

        public Matcher(IFullDataSource originalDataSource)
        {
            _originalDataSource = originalDataSource;
        }

        public bool IncludeEpub { get; set; }

        public bool IncludeMobi { get; set; }

        public async Task<IEnumerable<MatchInfo>> Match(IEnumerable<MatchInfo> incoming)
        {
            IEnumerable<BookInfo> books = await _originalDataSource.GetBooks(this.IncludeMobi, this.IncludeEpub);

            var matcher = new BookMatcher(books);

            IEnumerable<MatchInfo> matched = matcher.Match(incoming);

            return matched;
        }
    }
}
