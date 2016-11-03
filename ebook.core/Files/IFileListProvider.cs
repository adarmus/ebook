using System.Collections.Generic;

namespace Shell.Files
{
    public interface IFileListProvider
    {
        IEnumerable<string> GetFileList();
    }
}
