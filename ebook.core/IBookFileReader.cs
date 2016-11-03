using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shell
{
    public interface IBookFileReader
    {
        BookFile Read(string filepath);
    }
}
