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
        private readonly IBookFinder _bookFinder;
        private readonly IOutputMessage _messages;

        public Matcher(IBookFinder bookFinder, IOutputMessage messages)
        {
            _messages = messages;
            _bookFinder = bookFinder;
        }

        public async Task<IEnumerable<MatchInfo>> Match(IEnumerable<MatchInfo> incoming)
        {
            var matches = new List<MatchInfo>();

            foreach (var match in incoming)
            {
                FindResultInfo result = await _bookFinder.Find(match);

                match.SetMatch(result.Book, result.Status);

                matches.Add(match);

                _messages.Write("Match done: {0}: {1}", match.Book.Title, match.Status);
            }

            return matches;
        }
    }
}
