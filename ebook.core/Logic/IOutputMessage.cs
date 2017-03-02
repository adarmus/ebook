using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ebook.core.Logic
{
    public interface IOutputMessage
    {
        void Write(string text);

        void Write(string format, params object[] args);

        void WriteError(Exception ex, string message);

        void WriteError(Exception ex, string format, params object[] args);
    }
}
