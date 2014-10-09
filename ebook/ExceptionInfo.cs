
using System;

namespace ebook
{
    class ExceptionInfo
    {
        public string Message { get; set; }

        public string ExceptionName { get; set; }

        public Exception Exception { get; set; }

        public string Source { get; set; }

        public string StackTrace { get; set; }

        public string Target { get; set; }
    }
}
