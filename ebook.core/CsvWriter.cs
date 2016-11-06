using System.Collections.Generic;
using System.IO;
using ebook.core.DataTypes;

namespace ebook.core
{
    public class CsvWriter
    {
        readonly string _filepath;

        public CsvWriter(string filepath)
        {
            _filepath = filepath;
        }

        public void Write(IEnumerable<BookInfo> books)
        {
            using (TextWriter writer = new StreamWriter(_filepath))
            {
                foreach (var book in books)
                {
                    writer.WriteLine("{0},{1},{2},{3}", C34(book.Isbn), C34(book.Title), C34(book.Author), C34(book.Files[0]));
                }
            }
        }

        string C34(string input)
        {
            return string.Format("\"{0}\"", input);
        }
    }
}
