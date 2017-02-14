﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ebook.core.DataTypes;
using ebook.core.Repo;

namespace ebook.core.Logic
{
    /// <summary>
    /// Compares two lists of books outputting the matches.
    /// </summary>
    public class BookMatcher
    {
        readonly IFullDataSource _originalDataSource;
        IEnumerable<BookInfo> _originalBooks;

        Dictionary<string, IGrouping<string,BookInfo> > _lookupTitle;
        Dictionary<string, BookInfo> _lookupIsbn;

        /// <summary>
        /// Creates a matcher with the original list of books from the repo we are trying to update.
        /// </summary>
        public BookMatcher(IFullDataSource originalDataSource)
        {
            _originalDataSource = originalDataSource;
        }

        /// <summary>
        /// Compares an incoming list of books against the original list of books. 
        /// Returns the matches that indicate whenther any incoming books are missing from the original list.
        /// </summary>
        /// <param name="incoming"></param>
        /// <returns></returns>
        public async Task<MatchResultInfo> Match(MatchInfo incoming)
        {
            if (_originalBooks == null)
            {
                _originalBooks = await _originalDataSource.GetBooks(true, true);
            }

            MakeIsbnLookup();

            MakeTitleLookup();

            MatchStatus status;
            BookInfo book = Update(incoming, out status);

            return new MatchResultInfo { Status = status, Book = book };
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

        BookInfo Update(MatchInfo match, out MatchStatus status)
        {
            status = MatchStatus.NewBook;

            // Look by Isbn
            if (!string.IsNullOrEmpty(match.Book.Isbn))
            {
                string isbn = Isbn.Normalise(match.Book.Isbn);
                if (_lookupIsbn.ContainsKey(isbn))
                {
                    //match.SetMatch(_lookupIsbn[isbn], MatchStatus.UpToDate);
                    status = MatchStatus.UpToDate;
                    return _lookupIsbn[isbn];
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
                            //match.SetMatch(book, MatchStatus.UpToDate);
                            status = MatchStatus.UpToDate;
                            return book;
                        }
                    }
                }
            }

            // Nothing matched it
            //match.SetMatch(null, MatchStatus.NewBook);
            return null;
        }
    }
}
