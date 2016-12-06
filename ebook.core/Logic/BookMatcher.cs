using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ebook.core.DataTypes;

namespace ebook.core.Logic
{
    public class BookMatcher
    {
        readonly IEnumerable<BookInfo> _compareTo;

        Dictionary<string, IGrouping<string,BookInfo> > _lookupTitle;
        Dictionary<string, BookInfo> _lookupIsbn;

        public BookMatcher(IEnumerable<BookInfo> compareTo)
        {
            _compareTo = compareTo.ToArray();
        }

        public IEnumerable<MatchInfo> Match(IEnumerable<MatchInfo> matches)
        {
            _lookupIsbn = _compareTo
                .Where(b => !string.IsNullOrEmpty(b.Isbn))
                .ToDictionary(b => Isbn.Normalise(b.Isbn));

            _lookupTitle = _compareTo
                .Where(b => !string.IsNullOrEmpty(b.Title))
                .GroupBy(b => b.Title.ToLower())
                .ToDictionary(g => g.Key);

            foreach (var match in matches)
            {
                Update(match);
            }

            return matches;
        }

        void Update(MatchInfo match)
        {
            match.MatchedBook = null;
            match.IsSelected = true;

            // Look by Isbn
            if (!string.IsNullOrEmpty(match.Book.Isbn))
            {
                string isbn = Isbn.Normalise(match.Book.Isbn);
                if (_lookupIsbn.ContainsKey(isbn))
                {
                    match.MatchedBook = _lookupIsbn[isbn];
                    match.IsSelected = false;
                    return;
                }
            }

            // Look by Title and then Author
            if (!string.IsNullOrEmpty(match.Book.Title))
            {
                string title = match.Book.Title.ToLower();
                if (_lookupTitle.ContainsKey(title))
                {
                    var group = _lookupTitle[title];
                    var books = group.ToArray();
                    foreach (var book in books)
                    {
                        if (string.Equals(title, book.Title, StringComparison.CurrentCultureIgnoreCase))
                        {
                            match.MatchedBook = book;
                            match.IsSelected = false;
                            return;
                        }
                    }
                }
            }
        }
    }
}
