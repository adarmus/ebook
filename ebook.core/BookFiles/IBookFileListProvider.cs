using System.Collections.Generic;
using System.Threading.Tasks;

namespace ebook.core.BookFiles
{
    public interface IBookFileListProvider
    {
        Task<IEnumerable<BookFile>> GetBookFiles();
    }
}
