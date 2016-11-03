using System;
using System.IO;

namespace ebook.core
{
    class LogFileWriter : IBookFileHandler, IDisposable
    {
        readonly string _logfilepath;
        StreamWriter _writer;

        public LogFileWriter(string logfilepath)
        {
            _logfilepath = logfilepath;    
        }

        public void Accept(BookFile mobi)
        {
            _writer.WriteLine("{0}, {1}, {2}, {3}, {4}, {5}", mobi.Title, mobi.Author, mobi.Isbn, mobi.Publisher, mobi.PublishDate, mobi.FilePath);
        }

        public void Open()
        {
            _writer = new StreamWriter(_logfilepath);
        }

        public void Close()
        {
            if (_writer != null)
            {
                _writer.Close();
                _writer = null;
            }
        }

        public void Dispose()
        {
            Close();
        }
    }
}
