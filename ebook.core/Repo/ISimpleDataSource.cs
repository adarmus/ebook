using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ebook.core.DataTypes;

namespace ebook.core.Repo
{
    public interface ISimpleDataSource
    {
        Task<IEnumerable<BookInfo>> GetBooks(bool includeMobi, bool includeEpub);

        Task<BookFilesInfo> GetBookContent(BookInfo book);
    }
}
