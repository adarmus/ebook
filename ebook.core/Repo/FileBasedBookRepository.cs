using System.Collections.Generic;
using System.Linq;
using ebook.core.BookFiles;
using ebook.core.DataTypes;

namespace ebook.core.Repo
{
    public class FileBasedBookRepository : IBookRepository
    {
        readonly List<IBookFileListProvider> _providers;

        public FileBasedBookRepository()
        {
            _providers = new List<IBookFileListProvider>();
        }

        public FileBasedBookRepository AddList(IBookFileListProvider books)
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
