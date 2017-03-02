using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ebook.core.Files;
using ebook.core.Logic;

namespace ebook.core.BookFiles
{
    public class BookFileList : IBookFileListProvider
    {
        readonly List<IBookFileReader> _readers;
        readonly FileFinder _fileList;
        readonly IOutputMessage _messages;
        readonly Dictionary<string, IBookFileReader> _lookup;

        public BookFileList(FileFinder fileList, IOutputMessage messages)
        {
            _readers = new List<IBookFileReader>();
            _lookup = new Dictionary<string, IBookFileReader>(StringComparer.CurrentCultureIgnoreCase);
            _fileList = fileList;
            _messages = messages;
        }

        public void AddReader(IBookFileReader reader)
        {
            _readers.Add(reader);
        }

        public async Task<IEnumerable<BookFile>> GetBookFiles()
        {
            foreach (var reader in _readers)
            {
                _fileList.AddExtension(reader.Extension);
                _lookup.Add(string.Format(".{0}", reader.Extension), reader);
            }

            IEnumerable<string> list = await _fileList.GetFileList();

            _messages.Write("Found {0} files", list.Count());

            //var books = new List<BookFile>();
            //foreach (var path in list)
            //{
            //    BookFile book = await ReadBookFile(path);
            //    if (book != null)
            //        books.Add(book);
            //}

            var allBookTasks = list.Select(async path => await ReadBookFile(path));

            var allBooks = await Task.WhenAll(allBookTasks);

            var books = allBooks.Where(book => book != null).ToArray();

            return books;
        }

        async Task<BookFile> ReadBookFile(string path)
        {
            string ext = Path.GetExtension(path);
            IBookFileReader reader = _lookup[ext];

            try
            {
                BookFile book = await Task.Run(() => reader.Read(path));

                _messages.Write("Read {0}", path);

                return book;
            }
            catch (Exception ex)
            {
                _messages.WriteError(ex, "reading {0}", path);
                return null;
            }
        }
    }
}
