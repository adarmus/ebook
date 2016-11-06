using System.Collections.Generic;
using System.Linq;
using ebook.core.Files;

namespace ebook.core.BookFiles
{
    public class BookFileList : IBookFileListProvider
    {
        readonly IBookFileReader _reader;
        readonly IFileListProvider _fileList;

        public BookFileList(IFileListProvider fileList, IBookFileReader reader)
        {
            _reader = reader;
            _fileList = fileList;
        }

        public IEnumerable<BookFile> GetBookFiles()
        {
            return _fileList.GetFileList()
                .Select(_reader.Read)
                .Where(m => m != null);
        }
    }
}
