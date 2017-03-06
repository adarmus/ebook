using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ebook.core.Logging
{
    interface ITraceLogger
    {
        /// <summary>
        /// Writes a ERROR line.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        void WriteLineError(string message, Exception ex);

        /// <summary>
        /// Writes a ERROR line.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        void WriteLineError(Exception ex, string format, params object[] args);

        /// <summary>
        /// Writes a INFO line.
        /// </summary>
        /// <param name="message"></param>
        void WriteLineInfo(string message);

        /// <summary>
        /// Writes a INFO line.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        void WriteLineInfo(string format, params object[] args);

        /// <summary>
        /// Writes a DEBUG line.
        /// </summary>
        /// <param name="message"></param>
        void WriteLineDebug(string message);

        /// <summary>
        /// Writes a DEBUG line.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        void WriteLineDebug(string format, params object[] args);
    }
}
