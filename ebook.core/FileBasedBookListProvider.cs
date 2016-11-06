using System.Collections.Generic;
using System.Linq;

namespace ebook.core
{
    public class FileBasedBookListProvider : IBookInfoListProvider
    {
        readonly List<IBookFileListProvider> _providers;

        public FileBasedBookListProvider()
        {
            _providers = new List<IBookFileListProvider>();
        }

        public FileBasedBookListProvider AddList(IBookFileListProvider books)
        {
            _providers.Add(books);

            return this;
        }

        IEnumerable<BookFile> GetBookFiles()
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
