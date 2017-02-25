using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ebook.core.DataTypes
{
    public class FindResultInfo
    {
        public FindResultInfo()
        {
            NewTypes = Enumerable.Empty<string>();
        }

        public BookInfo Book { get; set; }

        public MatchStatus Status { get; set; }

        public IEnumerable<string> NewTypes { get; set; } 
    }
}
