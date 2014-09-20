using System.Collections.Generic;
using System.IO;

namespace Shell.Files
{
    public class FileFinder
    {
        readonly string _rootFolderPath;

        public FileFinder(string rootFolderPath)
        {
            _rootFolderPath = rootFolderPath;
        }

        public IEnumerable<string> GetFileList()
        {
            return Directory.EnumerateFiles(_rootFolderPath, "*.mobi", SearchOption.AllDirectories);
        }
    }
}
