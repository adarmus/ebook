using System.Linq;
using Shell.Pdb;

namespace Shell
{
    class Importer
    {
        readonly IMobiFileList _mobiFiles;
        readonly IMobiFileHandler _handler;

        public Importer(IMobiFileList mobiFiles, IMobiFileHandler handler)
        {
            _mobiFiles = mobiFiles;
            _handler = handler;
        }

        public void Import()
        {
            _handler.Open();

            _mobiFiles.GetMobiFiles()
                .ToList()
                .ForEach(Output);

            _handler.Close();
        }

        void Output(MobiFile mobi)
        {
            if (mobi != null)
                _handler.Accept(mobi);
        }
    }
}
