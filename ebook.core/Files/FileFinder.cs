using System.Collections.Generic;
using System.IO;

namespace ebook.core.Files
{
    public class FileFinder : IFileListProvider
    {
        readonly string _rootFolderPath;
        readonly string _extension;

        public FileFinder(string rootFolderPath, string extension)
        {
            _rootFolderPath = rootFolderPath;
            _extension = extension;
        }

        public IEnumerable<string> GetFileList()
        {
            string search = string.Format("*.{0}", _extension);
            return Directory.EnumerateFiles(_rootFolderPath, search, SearchOption.AllDirectories);
        }
    }
}
