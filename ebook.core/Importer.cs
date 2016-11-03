using System.Linq;

namespace ebook.core
{
    class Importer
    {
        readonly IBookFileListProvider _mobiFiles;
        readonly IBookFileHandler _handler;

        public Importer(IBookFileListProvider mobiFiles, IBookFileHandler handler)
        {
            _mobiFiles = mobiFiles;
            _handler = handler;
        }

        public void Import()
        {
            _handler.Open();

            _mobiFiles.GetBookFiles()
                .ToList()
                .ForEach(Output);

            _handler.Close();
        }

        void Output(BookFile mobi)
        {
            if (mobi != null)
                _handler.Accept(mobi);
        }
    }
}
