using System.Collections.Generic;
using System.Linq;

namespace Shell
{
    public class BookInfo
    {
        public BookInfo(IEnumerable<string> files)
        {
            _files = files;
        }

        public BookInfo(string file)
        {
            _files = new List<string> { file };
        }

        public string Author { get; set; }

        public string Description { get; set; }

        public string Publisher { get; set; }

        public string PublishDate { get; set; }

        public string Isbn { get; set; }

        public string Title { get; set; }

        readonly IEnumerable<string> _files;

        public List<string> Files
        {
            get { return _files.ToList(); }
        }

        public IEnumerable<string> GetFiles()
        {
            return _files;
        }
    }
}
