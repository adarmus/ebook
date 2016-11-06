
using ebook.core.BookFiles;

namespace ebook.core
{
    interface IBookFileHandler
    {
        void Accept(BookFile mobi);

        void Close();

        void Open();
    }
}
