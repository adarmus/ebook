using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ebook.core.Utils;

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

        public async Task<IEnumerable<string>> GetFileList()
        {
            string search = string.Format("*.{0}", _extension);
            
            return await AsyncDirectory.EnumerateFiles(_rootFolderPath, search, SearchOption.AllDirectories);
        }
    }
}
