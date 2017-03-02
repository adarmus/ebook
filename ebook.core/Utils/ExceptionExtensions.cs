using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ebook.core.Utils
{
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Resolves the innermost exception and the full list of exception messages.
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static ExceptionInfo GetExceptionInfo(this Exception ex)
        {
            var info = new ExceptionInfo();
            try
            {
                string fullErrorMessage;
                Exception innermost = GetInnerMostException(ex, out fullErrorMessage);

                if (innermost != null)
                {
                    info.InnerMostException = innermost;
                    info.FullExceptionMessage = fullErrorMessage;
                    info.Source = innermost.Source;
                    info.StackTrace = innermost.StackTrace;
                    if (innermost.TargetSite != null)
                        info.Target = innermost.TargetSite.ToString();
                    if (innermost != ex)
                        info.StackTrace += "\r\n...\r\n" + ex.StackTrace;
                }
            }
            catch
            {
            }
            return info;
        }

        static Exception GetInnerMostException(Exception ex, out string fullErrorMessage)
        {
            fullErrorMessage = string.Empty;
            Exception next = ex;
            Exception inner = ex;
            while (next != null)
            {
                inner = next;
                fullErrorMessage += "[" + inner.Message + "]";
                next = next.InnerException;
            }
            return inner;
        }

        public static string GetFormattedExceptionText(this ExceptionInfo e)
        {
            string sql = string.Empty;
            string crlf = Environment.NewLine;
            string ret = "";

            if (e == null)
            {
                ret = PadTitle("Error") + "No exception details are available" + crlf;
            }
            else
            {
                ret =
                    PadTitle("Error") + e.FullExceptionMessage + crlf +
                    PadTitle("Exception") + e.InnerMostException.GetType().ToString() + crlf +
                    PadTitle("Source") + e.Source + crlf +
                    PadTitle("Target") + e.Target + crlf +
                    sql +
                    PadTitle("StackTrace") + PadLeftMargin(e.StackTrace);
            }

            return ret;
        }

        private static string PadTitle(string text)
        {
            if (text.Length < 10)
                return text.PadLeft(10, ' ') + ": ";
            else
                return text + ": ";
        }

        private static string PadLeftMargin(string text)
        {
            return text.Replace("   ", new string(' ', 12)).Substring(12);
        }
    }
}
