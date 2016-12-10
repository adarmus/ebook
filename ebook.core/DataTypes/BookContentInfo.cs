using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ebook.core.DataTypes
{
    public class BookContentInfo
    {
        public BookInfo Book { get; set; }

        public IEnumerable<string> Files { get; }

        public BookContentInfo(BookInfo book, IEnumerable<string> files)
        {
            this.Book = book;
            Files = files;
        }

        public BookContentInfo(BookInfo book, string file)
        {
            this.Book = book;
            Files = new [] { file };
        }
    }
}
