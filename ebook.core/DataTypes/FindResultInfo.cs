using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ebook.core.DataTypes
{
    public class FindResultInfo
    {
        public BookInfo Book { get; set; }

        public MatchStatus Status { get; set; }
    }
}
