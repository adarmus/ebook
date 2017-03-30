using ebook.core.BookFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ebook.core.Repo.File
{
    interface IBookFileFilter
    {
        void Accept(BookFile file);
    }
}
