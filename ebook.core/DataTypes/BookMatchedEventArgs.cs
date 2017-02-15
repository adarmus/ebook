using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ebook.core.DataTypes
{
    public class BookMatchedEventArgs : EventArgs
    {
        public BookMatchedEventArgs(MatchInfo match)
        {
            this.Match = match;
        }

        public MatchInfo Match { get; set; }
    }
}
