using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ebook.core.Utils
{
    class AsyncStackTraceCleaner
    {
        readonly HashSet<string> _asyncNoise;

        public AsyncStackTraceCleaner()
        {
            _asyncNoise = new HashSet<string>(GetAsyncNoiseLines());
        }

        string[] GetAsyncNoiseLines()
        {
            return new[]
            {
                "   at System.Threading.Tasks.Task`1.InnerInvoke()",
                "   at System.Threading.Tasks.Task.Execute()",
                "--- End of stack trace from previous location where exception was thrown ---",
                "   at System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)",
                "   at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)",
                "   at System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()"
            };
        }

        public string RemoveAsyncLinesFromStackTrace(string stack)
        {
            string[] lines = stack.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            string[] cleanLines = lines.Where(line => !IsAsyncLine(line)).ToArray();

            string clean = string.Join(Environment.NewLine, cleanLines);

            return clean;
        }

        bool IsAsyncLine(string line)
        {
            return _asyncNoise.Contains(line);
        }
    }
}
