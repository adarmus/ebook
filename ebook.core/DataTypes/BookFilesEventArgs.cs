using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ebook.core.DataTypes
{
    public class BookFileEventArgs : EventArgs
    {
        public BookFileEventArgs(BookFileInfo file)
        {
            this.File = file;
        }

        public BookFileInfo File { get; set; }
    }
}
