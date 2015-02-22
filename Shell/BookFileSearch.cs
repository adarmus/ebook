using System.Collections.Generic;
using System.Linq;

namespace Shell
{
    public class BookFileSearch
    {
        readonly List<IBookFileListProvider> _providers;

        public BookFileSearch()
        {
            _providers = new List<IBookFileListProvider>();
        }

        public BookFileSearch AddList(IBookFileListProvider books)
        {
            _providers.Add(books);

            return this;
        }

        public IEnumerable<BookFile> GetBookFiles()
        {
            return _providers.SelectMany(p => p.GetBookFiles());
        }

        public IEnumerable<BookInfo> GetBooks()
        {
            var agg = new Aggregator();

            return agg.GetBookList(GetBookFiles());
        }
    }
}
