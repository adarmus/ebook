using System.Collections.Generic;
using System.Threading.Tasks;
using ebook.core.DataTypes;

namespace ebook.core.Repo
{
    public interface IFullDataSource : ISimpleDataSource
    {
        Task SaveBook(BookInfo book);

        Task SaveFile(BookFileInfo file);
    }
}
