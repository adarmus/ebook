using System.Collections.Generic;
using System.Threading.Tasks;
using ebook.core.BookFiles;
using ebook.core.DataTypes;
using ebook.core.ePub;
using ebook.core.Files;
using ebook.core.Logic;
using ebook.core.Mobi;
using ebook.core.Utils;

namespace ebook.core.Repo.File
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

        public async Task<IEnumerable<BookInfo>> GetBooks(bool includeMobi, bool includeEpub)
        {
            var agg = new Aggregator();

            _providers.Clear();

            if (includeMobi)
                AddReader("mobi", new MobiReader());

            if (includeEpub)
                AddReader("epub", new EpubReader());

            return agg.GetBookList(await GetBookFiles());
        }

        void AddReader(string fileExt, IBookFileReader reader)
        {
            var files = new FileFinder(_folderPath, fileExt);
            var list = new BookFileList(files, reader);
            _providers.Add(list);
        }

        async Task<IEnumerable<BookFile>> GetBookFiles()
        {
            return await _providers.SelectManyAsync(async p => await p.GetBookFiles());
        }

        public async Task SaveBooks(IEnumerable<BookInfo> books)
        {
            var csv = new CsvWriter(CsvWriter.GetFilePath(_folderPath));
            await csv.Write(books);
        }
    }
}
