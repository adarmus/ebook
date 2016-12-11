using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ebook.core.DataTypes
{
    public class MatchInfo
    {
        public MatchInfo(BookInfo book)
        {
            this.Book = book;
            this.MatchedBook = null;
        }

        public BookInfo Book { get; set; }

        public BookInfo MatchedBook { get; set; }

        public bool IsSelected { get; set; }

        public MatchStatus Status { get; set; }

        public void SetMatch(BookInfo matchedBook, MatchStatus status)
        {
            this.MatchedBook = matchedBook;
            this.Status = status;
            this.IsSelected = (status == MatchStatus.NewBook || status == MatchStatus.NewFiles);
        }
    }

    public enum MatchStatus
    {
        /// <summary>
        /// The match process has not been run yet.
        /// </summary>
        NotAttempted,
        /// <summary>
        /// This is a new book.
        /// </summary>
        NewBook,
        /// <summary>
        /// This book has been matched but this contains new files.
        /// </summary>
        NewFiles,
        /// <summary>
        /// The book and all its files have been matched.
        /// </summary>
        UpToDate
    }
}
