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
        private readonly BookFinder _bookMatcher;

        public Matcher(BookFinder bookMatcher)
        {
            _bookMatcher = bookMatcher;
        }

        public event EventHandler<BookMatchedEventArgs> BookMatched;

        protected virtual void OnBookMatched(BookMatchedEventArgs args)
        {
            if (BookMatched != null)
                BookMatched(this, args);
        }

        public async Task<IEnumerable<MatchInfo>> Match(IEnumerable<MatchInfo> incoming)
        {
            var matches = new List<MatchInfo>();

            foreach (var match in incoming)
            {
                FindResultInfo result = await _bookMatcher.Find(match);

                match.SetMatch(result.Book, result.Status);

                matches.Add(match);

                OnBookMatched(new BookMatchedEventArgs(match));
            }

            return matches;
        }
    }
}
