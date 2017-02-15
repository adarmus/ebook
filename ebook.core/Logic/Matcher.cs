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
        private readonly IOutputMessage _messages;

        public Matcher(BookFinder bookMatcher, IOutputMessage messages)
        {
            _messages = messages;
            _bookMatcher = bookMatcher;
        }

        public async Task<IEnumerable<MatchInfo>> Match(IEnumerable<MatchInfo> incoming)
        {
            var matches = new List<MatchInfo>();

            foreach (var match in incoming)
            {
                FindResultInfo result = await _bookMatcher.Find(match);

                match.SetMatch(result.Book, result.Status);

                matches.Add(match);

                _messages.Write("Match done: {0}: {1}", match.Book.Title, match.Status);
            }

            return matches;
        }
    }
}
