using System.Collections.Generic;
using System.Threading.Tasks;
using ebook.core.DataTypes;

namespace ebook.core.Repo
{
    public interface IFullDataSource : ISimpleDataSource
    {
        Task SaveBook(BookInfo book);

        Task SaveFile(BookFileInfo file);

        Task<BookInfo> GetBookByIsbn(string isbn);

        Task<BookInfo> GetBookByTitleAuthor(string title, string author);
    }
}
