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

        public BookFileList(FileFinder fileList, IOutputMessage messages)
        {
            _readers = new List<IBookFileReader>();
            _fileList = fileList;
            _messages = messages;
        }

        public void AddReader(IBookFileReader reader)
        {
            _readers.Add(reader);
        }

        public async Task<IEnumerable<BookFile>> GetBookFiles()
        {
            var lookup = new Dictionary<string, IBookFileReader>(StringComparer.CurrentCultureIgnoreCase);
            foreach (var reader in _readers)
            {
                _fileList.AddExtension(reader.Extension);
                lookup.Add(string.Format(".{0}", reader.Extension), reader);
            }

            IEnumerable<string> list = await _fileList.GetFileList();

            _messages.Write("Found {0} files", list.Count());

            return list
                .Select(path =>
                {
                    string ext = Path.GetExtension(path);
                    IBookFileReader reader = lookup[ext];
                    BookFile book = reader.Read(path);
                    return book;
                })
                .Where(book => book != null);
        }
    }
}
