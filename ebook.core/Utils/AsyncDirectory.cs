using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ebook.core.Utils
{
    static class AsyncDirectory
    {
        public static Task<IEnumerable<string>> EnumerateDirectories(string path)
        {
            return Task.Run(() => { return Directory.EnumerateDirectories(path); });
        }

        public static Task<IEnumerable<string>> EnumerateFiles(string path)
        {
            return Task.Run(() => { return Directory.EnumerateFiles(path); });
        }

        public static Task<IEnumerable<string>> EnumerateFiles(string path, string searchPattern, SearchOption options)
        {
            return Task.Run(() => { return Directory.EnumerateFiles(path, searchPattern, options); });
        }
    }
}
