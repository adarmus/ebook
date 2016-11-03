using System.Collections.Generic;

namespace ebook.core
{
    public interface IBookFileListProvider
    {
        IEnumerable<BookFile> GetBookFiles();
    }
}
