using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ebook.core.DataTypes;

namespace ebook.core.Logic
{
    public class Matcher
    {
        private readonly BookMatcher _bookMatcher;

        public Matcher(BookMatcher bookMatcher)
        {
            _bookMatcher = bookMatcher;
        }

        public bool IncludeEpub { get; set; }

        public bool IncludeMobi { get; set; }

        public async Task<IEnumerable<MatchInfo>> Match(IEnumerable<MatchInfo> incoming)
        {
            var matches = new List<MatchInfo>();

            foreach (var match in incoming)
            {
                MatchResultInfo result = await _bookMatcher.Match(match);

                match.SetMatch(result.Book, result.Status);

                matches.Add(match);
            }

            return matches;
        }
    }
}
