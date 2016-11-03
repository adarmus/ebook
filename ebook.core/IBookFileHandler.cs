
namespace Shell
{
    interface IBookFileHandler
    {
        void Accept(BookFile mobi);

        void Close();

        void Open();
    }
}
