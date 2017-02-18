using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ebook.core.DataTypes;

namespace ebook.core.Logic
{
    public interface IBookFinder
    {
        Task<FindResultInfo> Find(BookInfo incoming);
    }
}
