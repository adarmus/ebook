using System.Collections.Generic;

namespace ebook.core.Files
{
    public interface IFileListProvider
    {
        IEnumerable<string> GetFileList();
    }
}
