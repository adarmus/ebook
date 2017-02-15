using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ebook.core.Files;
using ebook.core.Logic;

namespace ebook.core.BookFiles
{
    public class BookFileList : IBookFileListProvider
    {
        readonly IBookFileReader _reader;
        readonly IFileListProvider _fileList;
        readonly IOutputMessage _messages;

        public BookFileList(IFileListProvider fileList, IBookFileReader reader, IOutputMessage messages)
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
