using System;
using System.Collections.Generic;
using System.IO;
using ebook.core.DataTypes;

namespace ebook.core.Repo.File
{
    public class CsvWriter
    {
        public static string GetFilePath(string folderPath)
        {
            string path = Path.Combine(folderPath, GetCsvFilename());
            return path;
        }

        static string GetCsvFilename()
        {
            string s = string.Format("{0:yyyy-MMM-dd}.csv", DateTime.Today);
            return s;
        }

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
