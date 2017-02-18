using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ebook.core.DataTypes;
using ebook.core.Repo;

namespace ebook.core.Logic
{
    /// <summary>
    /// Finds books in a list of books.
    /// </summary>
    public class BookFinder : IBookFinder
    {
        readonly IFullDataSource _originalDataSource;
        IEnumerable<BookInfo> _originalBooks;

        Dictionary<string, IGrouping<string,BookInfo> > _lookupTitle;
        Dictionary<string, BookInfo> _lookupIsbn;

        /// <summary>
        /// Creates a BookFinder with the original list of books from the repo we are trying to update.
        /// </summary>
        public BookFinder(IFullDataSource originalDataSource)
        {
            _originalDataSource = originalDataSource;
        }

        /// <summary>
        /// Compares an incoming book against the original list of books. 
        /// Returns a match that indicates whenther the incoming book is missing from the original list.
        /// </summary>
        /// <param name="incoming"></param>
        /// <returns></returns>
        public async Task<FindResultInfo> Find(BookInfo incoming)
        {
            if (_originalBooks == null)
            {
                _originalBooks = await _originalDataSource.GetBooks(true, true);

                MakeIsbnLookup();

                MakeTitleLookup();
            }

            MatchStatus status;
            BookInfo book = FindBook(incoming, out status);

            return new FindResultInfo { Status = status, Book = book };
        }

        private void MakeTitleLookup()
        {
            _lookupTitle = _originalBooks
                .Where(b => !string.IsNullOrEmpty(b.Title))
                .GroupBy(b => b.Title.ToLower())
                .ToDictionary(g => g.Key);
        }

        private void MakeIsbnLookup()
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
        }

        BookInfo FindBook(BookInfo match, out MatchStatus status)
        {
            // Look by Isbn
            if (!string.IsNullOrEmpty(match.Isbn))
            {
                string isbn = Isbn.Normalise(match.Isbn);
                if (_lookupIsbn.ContainsKey(isbn))
                {
                    status = MatchStatus.UpToDate;
                    return _lookupIsbn[isbn];
                }
            }

            // Look by Title and then Author
            if (!string.IsNullOrEmpty(match.Title))
            {
                string title = match.Title.ToLower();
                if (_lookupTitle.ContainsKey(title))
                {
                    var group = _lookupTitle[title];
                    var books = group.ToArray();
                    foreach (var book in books)
                    {
                        if (string.Equals(title, book.Title, StringComparison.CurrentCultureIgnoreCase))
                        {
                            status = MatchStatus.UpToDate;
                            return book;
                        }
                    }
                }
            }

            // Nothing matched it
            status = MatchStatus.NewBook;
            return null;
        }
    }
}
