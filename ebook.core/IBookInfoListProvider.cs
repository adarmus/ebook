using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ebook.core
{
    public interface IBookInfoListProvider
    {
        IEnumerable<BookInfo> GetBooks();
    }
}
