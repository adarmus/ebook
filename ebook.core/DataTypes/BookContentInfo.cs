using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ebook.core.DataTypes
{
    public class BookContentInfo
    {
        readonly IEnumerable<string> _files;

        public BookInfo Book { get; set; }

        public List<string> Files
        {
            get { return _files.ToList(); }
        }

        public BookContentInfo(BookInfo book, IEnumerable<string> files)
        {
            this.Book = book;
            _files = files;
        }

        public BookContentInfo(BookInfo book, string file)
        {
            this.Book = book;
            _files = new List<string> { file };
        }
    }
}
