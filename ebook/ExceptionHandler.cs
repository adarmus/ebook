using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ebook.core.Logic;
using ebook.core.Utils;
using log4net;

namespace ebook
{
    class ExceptionHandler : IOutputMessage
    {
        readonly MessageListener _messages;

        public ExceptionHandler(MessageListener messages)
        {
            _messages = messages;
        }

        public void WriteError(Exception ex, string format, params object[] args)
        {
            WriteError(ex, string.Format(format, args));
        }

        public void WriteError(Exception ex, string message)
        {
            ExceptionInfo e = ex.GetExceptionInfo();

            string err = e.GetFormattedExceptionText();

            ILog log = LogManager.GetLogger("root");
            log.Error(string.Format("An error occured {0}: \n{1}", message, err), e.InnerMostException);

            _messages.Write(string.Format("An error occured {0}: \n{1}", message, err));
        }

        public void Write(string text)
        {
            _messages.Write(text);
        }

        public void Write(string format, params object[] args)
        {
            _messages.Write(format, args);
        }
    }
}
