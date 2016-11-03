using System.Collections.Generic;
using System.Linq;
using ebook.core.Files;
using ebook.core.Pdb;

namespace ebook.core.Mobi
{
    public class MobiFileList : IBookFileListProvider
    {
        readonly IFileListProvider _fileList;

        public MobiFileList(IFileListProvider fileList)
        {
            _fileList = fileList;
        }

        public IEnumerable<BookFile> GetBookFiles()
        {
            return _fileList.GetFileList()
                .Select(Read)
                .Where(m => m != null);
        }

        BookFile Read(string filepath)
        {
            BookFile mobi = null;
            try
            {
                var reader = new PdbFileReader(filepath);
                mobi = reader.ReadMobiFile();
                mobi.FilePath = filepath;
            }
            catch
            {

            }
            return mobi;
        }
    }
}
