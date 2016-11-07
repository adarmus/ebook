using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ebook
{
    /// <summary>
    /// See https://msdn.microsoft.com/en-gb/magazine/dn630647.aspx
    /// </summary>
    public class AsyncCommand1 : AsyncCommandBase
    {
        private readonly Func<Task> _command;
        public AsyncCommand1(Func<Task> command)
        {
            _command = command;
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override Task ExecuteAsync(object parameter)
        {
            return _command();
        }
    }
}
