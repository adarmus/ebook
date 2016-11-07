using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ebook.core.BookFiles;
using ebook.core.Files;
using ebook.core.Mobi.Pdb;
using ebook.core.Repo;

namespace ebook.core.Mobi
{
    public class MobiFileList : IBookFileListProvider
    {
        readonly IFileListProvider _fileList;

        public MobiFileList(IFileListProvider fileList)
        {
            _fileList = fileList;
        }

        public async Task<IEnumerable<BookFile>> GetBookFiles()
        {
            var list = await _fileList.GetFileList();

            return  list
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
