using System.Collections.Generic;
using System.Threading.Tasks;

namespace ebook.core.Files
{
    public interface IFileListProvider
    {
        Task<IEnumerable<string>> GetFileList();
    }
}
