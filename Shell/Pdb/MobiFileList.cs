using System.Collections.Generic;
using System.Linq;
using Shell.Files;

namespace Shell.Pdb
{
    public class MobiFileList : IMobiFileList
    {
        readonly FileFinder _fileList;

        public MobiFileList(FileFinder fileList)
        {
            _fileList = fileList;
        }

        public IEnumerable<MobiFile> GetMobiFiles()
        {
            return _fileList.GetFileList()
                .Select(Read)
                .Where(m => m != null);
        }

        MobiFile Read(string filepath)
        {
            MobiFile mobi = null;
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
