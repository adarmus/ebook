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
        readonly string _folderPath;
        readonly FileReader _reader;
        Dictionary<string, BookFilesInfo> _lookup;

        public FileBasedSimpleDataSource(string folderPath, IOutputMessage messages)
        {
            _folderPath = folderPath;
            _messages = messages;
            _reader = new FileReader();
        }

        public DateAddedProvider DateAddedProvider { get; set; }

        public async Task<IEnumerable<BookInfo>> GetBooks(bool includeMobi, bool includeEpub)
        {
            BookFileList bookFileList = GetBookFileList(includeMobi, includeEpub);

            IEnumerable<BookFile> files = await bookFileList.GetBookFiles();

            IEnumerable<BookInfo> list = AggregateBooks(files.ToArray());

            return list;
        }

        IEnumerable<BookInfo> AggregateBooks(IEnumerable<BookFile> files)
        {
            var agg = new Aggregator(DateAddedProvider);

            IEnumerable<BookInfo> list = agg.GetBookList(files);

            _lookup = agg.GetBookContentInfoLookup();
            return list;
        }

        BookFileList GetBookFileList(bool includeMobi, bool includeEpub)
        {
            var fileFinder = new FileFinder(_folderPath);
            var bookFileList = new BookFileList(fileFinder, _messages);

            if (includeMobi)
                bookFileList.AddReader(new MobiReader());

            if (includeEpub)
                bookFileList.AddReader(new EpubReader());

            return bookFileList;
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

            byte[] content = await _reader.ReadAllFileAsync(filepath);

            var bookfile = new BookFileInfo
            {
                Id = filepath,
                FileName = Path.GetFileName(filepath),
                FileType = type,
                FilePath = filepath,
                BookId = book.Id,
                Content = content
            };

            return bookfile;
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
