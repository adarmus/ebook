using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ebook.core.Logic;
using log4net;

namespace ebook
{
    class MessageListener : IOutputMessage
    {
        readonly ILog _log;
        readonly ObservableCollection<MessageInfo> _messages;

        public MessageListener(ObservableCollection<MessageInfo> messages, ILog log)
        {
            _messages = messages;
            _log = log;
        }

        public void Write(string text)
        {
            AddMessage(new MessageInfo(text));
        }

        public void Write(string format, params object[] args)
        {
            Write(string.Format(format, args));
        }

        public void AddMessage(MessageInfo message)
        {
            message.Time = string.Format("{0:yyyy-MM-dd hh:mm:ss}", DateTime.Now);
            _messages.Add(message);
            _log.Debug(message.Text);
        }
    }
}
