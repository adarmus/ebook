using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ebook.core.Utils
{
    public class ExceptionInfo
    {
        public string FullExceptionMessage { get; set; }

        public Exception InnerMostException { get; set; }

        public string Source { get; set; }

        public string StackTrace { get; set; }

        public string Target { get; set; }
    }
}
