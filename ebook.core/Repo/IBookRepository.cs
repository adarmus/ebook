using System.Collections.Generic;
using System.Threading.Tasks;
using ebook.core.DataTypes;

namespace ebook.core.Repo
{
    public interface IBookRepository : ISimpleBookRepository
    {
        Task SaveBook(BookInfo book);
    }
}
