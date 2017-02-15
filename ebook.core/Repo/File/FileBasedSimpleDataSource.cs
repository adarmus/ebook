using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    public class FileBasedSimpleDataSource : ISimpleDataSource
    {
        readonly IOutputMessage _messages;
        readonly List<IBookFileListProvider> _providers;
        readonly string _folderPath;
        Dictionary<string, BookFilesInfo> _lookup;

        public FileBasedSimpleDataSource(string folderPath, IOutputMessage messages)
        {
            _providers = new List<IBookFileListProvider>();
            _folderPath = folderPath;
            _messages = messages;
        }

        public async Task<IEnumerable<BookInfo>> GetBooks(bool includeMobi, bool includeEpub)
        {
            var agg = new Aggregator();

            _providers.Clear();

            if (includeMobi)
                AddReader("mobi", new MobiReader(_messages));

            if (includeEpub)
                AddReader("epub", new EpubReader(_messages));

            IEnumerable<BookInfo> list = agg.GetBookList(await GetBookFiles());

            _lookup = agg.GetBookContentInfoLookup();

            return list;
        }

        void AddReader(string fileExt, IBookFileReader reader)
        {
            var files = new FileFinder(_folderPath, fileExt);
            var list = new BookFileList(files, reader, _messages);
            _providers.Add(list);
        }

        async Task<IEnumerable<BookFile>> GetBookFiles()
        {
            return await _providers.SelectManyAsync(async p => await p.GetBookFiles());
        }

        public async Task<BookFilesInfo> GetBookContent(BookInfo book)
        {
            if (!_lookup.ContainsKey(book.Id))
                return null;

            BookFilesInfo content = _lookup[book.Id];

            var tasks = content.FileIds.Select(async id => await ReadBookFile(book, id));
            BookFileInfo[] files = await Task.WhenAll(tasks);

            var contentWithBytes = new BookFilesInfo(book, files);

            return contentWithBytes;
        }

        async Task<BookFileInfo> ReadBookFile(BookInfo book, string filepath)
        {
            string type = GetFileType(filepath);

            if (type == null)
                return null;

            byte[] content = await ReadAllFileAsync(filepath);

            var bookfile = new BookFileInfo
            {
                Id = filepath,
                FileName = Path.GetFileName(filepath),
                FileType = type,
                BookId = book.Id,
                Content = content
            };

            return bookfile;
        }

        async Task<byte[]> ReadAllFileAsync(string filename)
        {
            using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true))
            {
                byte[] buff = new byte[file.Length];
                await file.ReadAsync(buff, 0, (int)file.Length);
                return buff;
            }
        }

        string GetFileType(string filepath)
        {
            string ext = Path.GetExtension(filepath);

            if (string.IsNullOrEmpty(ext))
                return null;

            return ext.ToUpper().Substring(1);
        }
    }
}
