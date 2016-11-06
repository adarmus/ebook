using System.Collections.Generic;

namespace ebook.core.BookFiles
{
    public interface IBookFileListProvider
    {
        IEnumerable<BookFile> GetBookFiles();
    }
}
