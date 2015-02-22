using System.Collections.Generic;

namespace Shell
{
    public interface IBookFileListProvider
    {
        IEnumerable<BookFile> GetBookFiles();
    }
}
