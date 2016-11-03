using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ebook.core;

namespace ebook
{
    class BookComparisonInfo
    {
        public string Isbn { get; set; }

        public BookInfo Book1 { get; set; }

        public BookInfo Book2 { get; set; }
    }
}
