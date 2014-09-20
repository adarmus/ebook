using Shell.Pdb;

namespace Shell
{
    interface IMobiFileHandler
    {
        void Accept(MobiFile mobi);

        void Close();

        void Open();
    }
}
