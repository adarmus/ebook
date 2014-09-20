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
                .Select(Read);
        }

        MobiFile Read(string filepath)
        {
            var reader = new PdbFileReader(filepath);
            MobiFile mobi = reader.ReadMobiFile();

            mobi.FilePath = filepath;

            return mobi;
        }
    }
}
