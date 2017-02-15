using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ebook.core.DataTypes
{
    public class BookEventArgs : EventArgs
    {
        public BookEventArgs(BookInfo book)
        {
            this.Book = book;
        }

        public BookInfo Book { get; set; }
    }
}
