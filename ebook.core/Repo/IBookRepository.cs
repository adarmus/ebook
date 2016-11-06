using System.Collections.Generic;
using ebook.core.DataTypes;

namespace ebook.core.Repo
{
    public interface IBookRepository
    {
        IEnumerable<BookInfo> GetBooks();
    }
}
