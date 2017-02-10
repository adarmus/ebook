using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ebook.core.DataTypes;

namespace ebook.core.Logic
{
    /// <summary>
    /// Compares two lists of books outputting the matches.
    /// </summary>
    public class BookMatcher
    {
        readonly IEnumerable<BookInfo> _originalBooks;

        Dictionary<string, IGrouping<string,BookInfo> > _lookupTitle;
        Dictionary<string, BookInfo> _lookupIsbn;

        /// <summary>
        /// Creates a matcher with the original list of books from the repo we are trying to update.
        /// </summary>
        /// <param name="originalBooks"></param>
        public BookMatcher(IEnumerable<BookInfo> originalBooks)
        {
            _originalBooks = originalBooks.ToArray();
        }

        /// <summary>
        /// Compares an incoming list of books against the original list of books. 
        /// Returns the matches that indicate whenther any incoming books are missing from the original list.
        /// </summary>
        /// <param name="incoming"></param>
        /// <returns></returns>
        public IEnumerable<MatchInfo> Match(IEnumerable<MatchInfo> incoming)
        {
            _lookupIsbn = new Dictionary<string, BookInfo>();
                
            _originalBooks
                .Where(b => !string.IsNullOrEmpty(b.Isbn))
                .ToList()
                .ForEach(b =>
                {
                    string isbn = Isbn.Normalise(b.Isbn);
                    if (!_lookupIsbn.ContainsKey(isbn))
                        _lookupIsbn.Add(isbn, b);
                });

            _lookupTitle = _originalBooks
                .Where(b => !string.IsNullOrEmpty(b.Title))
                .GroupBy(b => b.Title.ToLower())
                .ToDictionary(g => g.Key);

            foreach (var match in incoming)
            {
                Update(match);
            }

            return incoming;
        }

        void Update(MatchInfo match)
        {
            // Look by Isbn
            if (!string.IsNullOrEmpty(match.Book.Isbn))
            {
                string isbn = Isbn.Normalise(match.Book.Isbn);
                if (_lookupIsbn.ContainsKey(isbn))
                {
                    match.SetMatch(_lookupIsbn[isbn], MatchStatus.UpToDate);
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
                            match.SetMatch(book, MatchStatus.UpToDate);
                            return;
                        }
                    }
                }
            }

            // Nothing matched it
            match.SetMatch(null, MatchStatus.NewBook);
        }
    }
}
