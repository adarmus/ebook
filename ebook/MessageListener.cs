using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace ebook
{
    class MessageListener
    {
        readonly ILog _log;
        readonly ObservableCollection<MessageInfo> _messages;

        public MessageListener(ObservableCollection<MessageInfo> messages, ILog log)
        {
            _messages = messages;
            _log = log;
        }

        public void AddMessage(string text)
        {
            AddMessage(new MessageInfo(text));
        }

        public void AddMessage(MessageInfo message)
        {
            message.Time = string.Format("{0:yyyy-MM-dd hh:mm:ss}", DateTime.Now);
            _messages.Add(message);
            _log.Debug(message.Text);
        }
    }
}
