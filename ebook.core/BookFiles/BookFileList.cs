using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<IEnumerable<BookFile>> GetBookFiles()
        {
            var list = await _fileList.GetFileList();
            return list
                .Select(_reader.Read)
                .Where(m => m != null);
        }
    }
}
