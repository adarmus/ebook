using System.Collections.Generic;
using System.Linq;
using ebook.core.BookFiles;
using ebook.core.DataTypes;
using ebook.core.ePub;
using ebook.core.Files;
using ebook.core.Logic;
using ebook.core.Mobi;

namespace ebook.core.Repo
{
    public class FileBasedBookRepository : IBookRepository
    {
        readonly List<IBookFileListProvider> _providers;
        readonly string _folderPath;

        public FileBasedBookRepository(string folderPath)
        {
            _providers = new List<IBookFileListProvider>();
            _folderPath = folderPath;
        }

        public IEnumerable<BookInfo> GetBooks(bool includeMobi, bool includeEpub)
        {
            var agg = new Aggregator();

            _providers.Clear();

            if (includeMobi)
                AddReader("mobi", new MobiReader());

            if (includeEpub)
                AddReader("epub", new EpubReader());

            return agg.GetBookList(GetBookFiles());
        }

        void AddReader(string fileExt, IBookFileReader reader)
        {
            var epubFiles = new FileFinder(_folderPath, fileExt);
            var epubList = new BookFileList(epubFiles, reader);
            _providers.Add(epubList);
        }

        IEnumerable<BookFile> GetBookFiles()
        {
            return _providers.SelectMany(p => p.GetBookFiles());
        }
    }
}
