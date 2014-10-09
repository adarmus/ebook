using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace ebook
{
    class ExceptionHandler
    {
        internal static void Handle(Exception ex, string message)
        {
            ExceptionInfo e = GetExceptionInfo(ex, message);

            string err = GetFormattedExceptionText(e, true);

            ILog log = LogManager.GetLogger("root");
            log.Error(string.Format("An error occured {0}: {1}", message, err), e.Exception);
        }

        static string GetFormattedExceptionText(ExceptionInfo e, bool pad)
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
                    PadTitle("Error") + e.Message + crlf +
                    PadTitle("Exception") + e.ExceptionName + crlf +
                    PadTitle("Source") + e.Source + crlf +
                    PadTitle("Target") + e.Target + crlf +
                    sql +
                    PadTitle("StackTrace") + PadLeftMargin(e.StackTrace, pad);
            }

            return ret;
        }

        private static string PadTitle(string text)
        {
            if (text.Length < 32)
                return text.PadLeft(32, ' ') + ": ";
            else
                return text + ": ";
        }

        private static string PadLeftMargin(string text, bool pad)
        {
            if (pad)
                return text.Replace("   ", new string(' ', 34)).Substring(34);
            else
                return text;
        }

        static ExceptionInfo GetExceptionInfo(Exception ex, string userMessage)
        {
            var info = new ExceptionInfo();
            try
            {
                info.Message = userMessage;

                string fullErrorMessage = string.Empty;
                Exception innermost = GetInnerMostException(ex, out fullErrorMessage);
                if (innermost != null)
                {
                    info.Exception = innermost;
                    info.ExceptionName = fullErrorMessage;
                    info.Source = innermost.Source;
                    info.StackTrace = innermost.StackTrace;
                    if (innermost.TargetSite != null)
                        info.Target = innermost.TargetSite.ToString();
                    if (innermost != null && innermost != ex)
                        info.StackTrace += "\r\n...\r\n" + ex.StackTrace;
                }
            }
            catch
            {
            }
            return info;
        }

        static Exception GetInnerMostException(Exception ex, out string FullErrorMessage)
        {
            FullErrorMessage = string.Empty;
            Exception next = ex;
            Exception inner = ex;
            while (next != null)
            {
                inner = next;
                FullErrorMessage += "[" + inner.Message + "]";
                next = next.InnerException;
            }
            return inner;
        }
    }
}
