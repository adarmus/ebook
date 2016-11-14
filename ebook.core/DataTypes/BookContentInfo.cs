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

        public BookContentInfo(IEnumerable<string> files)
        {
            _files = files;
        }

        public BookContentInfo(string file)
        {
            _files = new List<string> { file };
        }
    }
}
