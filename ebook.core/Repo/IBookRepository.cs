using System.Collections.Generic;

namespace ebook.core.Repo
{
    public interface IBookRepository
    {
        IEnumerable<BookInfo> GetBooks();
    }
}
