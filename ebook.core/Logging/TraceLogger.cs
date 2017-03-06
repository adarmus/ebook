using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace ebook.core.Logging
{
    class TraceLogger : ITraceLogger
    {
        readonly string _category;

        public TraceLogger(string category)
        {
            _category = category;
        }

        public bool IsDebugEnabled()
        {
            return GetLogger(_category).IsDebugEnabled;
        }

        /// <summary>
        /// Writes a ERROR line.
        /// </summary>
        /// <param name="message"></param>
        public void WriteLineError(string message, Exception ex)
        {
            GetLogger(_category).Error(message, ex);
        }

        /// <summary>
        /// Writes a ERROR line.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public void WriteLineError(Exception ex, string format, params object[] args)
        {
            WriteLineError(string.Format(format, args), ex);
        }

        /// <summary>
        /// Writes a INFO line.
        /// </summary>
        /// <param name="message"></param>
        public void WriteLineInfo(string message)
        {
            GetLogger(_category).Info(message);
        }

        /// <summary>
        /// Writes a INFO line.
        /// IsEnabled is checked before calling string.Format().
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public void WriteLineInfo(string format, params object[] args)
        {
            GetLogger(_category).InfoFormat(format, args);
        }

        /// <summary>
        /// Writes a DEBUG line.
        /// </summary>
        /// <param name="message"></param>
        public void WriteLineDebug(string message)
        {
            GetLogger(_category).Debug(message);
        }

        /// <summary>
        /// Writes a DEBUG line.
        /// IsEnabled is checked before calling string.Format().
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public void WriteLineDebug(string format, params object[] args)
        {
            GetLogger(_category).DebugFormat(format, args);
        }

        ILog GetLogger(string name)
        {
            return LogManager.GetLogger(name);
        }
    }
}
