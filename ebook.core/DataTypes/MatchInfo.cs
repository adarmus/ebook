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

        public bool HasMatch
        {
            get { return this.MatchedBook != null; }
        }
    }
}
