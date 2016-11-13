using System.Collections.Generic;
using System.Threading.Tasks;
using ebook.core.DataTypes;

namespace ebook.core.Repo
{
    public interface IBookRepository
    {
        Task<IEnumerable<BookInfo>> GetBooks(bool includeMobi, bool includeEpub);

        Task SaveBooks(IEnumerable<BookInfo> books);
    }
}
