using System.Collections.Generic;
using System.Threading.Tasks;
using ebook.core.DataTypes;

namespace ebook.core.Repo
{
    public interface IBookDataSource : ISimpleDataSource
    {
        Task SaveBook(BookInfo book);
    }
}
