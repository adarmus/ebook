using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ebook.core.Utils;

namespace ebook.core.Files
{
    public class FileFinder : IFileListProvider
    {
        readonly string _rootFolderPath;
        readonly List<string>_extensions;

        public FileFinder(string rootFolderPath)
        {
            _rootFolderPath = rootFolderPath;
            _extensions = new List<string>();
        }

        /// <summary>
        /// Adds an extension (ext should not include the '.').
        /// </summary>
        /// <param name="ext"></param>
        public void AddExtension(string ext)
        {
            _extensions.Add(string.Format(".{0}", ext));
        }

        public async Task<IEnumerable<string>> GetFileList()
        {
            var extHash = new HashSet<string>(_extensions, StringComparer.CurrentCultureIgnoreCase);

            IEnumerable<string> allfiles = await AsyncDirectory.EnumerateFiles(_rootFolderPath, SearchOption.AllDirectories);

            IEnumerable<string> files = allfiles.Where(f => extHash.Contains(Path.GetExtension(f)));

            return files;
        }
    }
}
