using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization.Configuration;
using ebook.core.DataTypes;

namespace ebook
{
    class DateAddedProvider
    {
        readonly DateTime _added;

        public DateAddedProvider(string dateAddedText)
        {
            if (string.IsNullOrEmpty(dateAddedText))
            {
                _added = DateTime.Now;
            }
            else
            {
                DateTime date;
                if (DateTime.TryParse(dateAddedText, out date))
                {
                    _added = date;
                }
                else
                {
                    throw new ArgumentException(string.Format("Invalid date: {0}", dateAddedText));
                }
            }
        }

        public void SetDateTimeAdded(BookFilesInfo[] books)
        {
            foreach (var book in books)
            {
                DateTime date;

                if (TryGetDateFromFilePath(book, out date))
                {
                    book.Book.DateAdded = date;
                }
                else
                {
                    book.Book.DateAdded = _added;
                }
            }
        }

        bool TryGetDateFromFilePath(BookFilesInfo book, out DateTime date)
        {
            date = DateTime.MinValue;

            foreach (BookFileInfo file in book.Files)
            {
                if (TryGetDateFromFilePath(file, out date))
                    return true;
            }

            return false;
        }

        bool TryGetDateFromFilePath(BookFileInfo file, out DateTime date)
        {
            date = DateTime.MinValue;
            Console.WriteLine("id={0}", file.Id);

            if (!File.Exists(file.Id))
                return false;

            string[] folders = file.Id.Split(new char[] { Path.DirectorySeparatorChar });

            foreach (string folder in folders)
            {
                if (TryParseDate(folder, out date))
                    return true;
            }

            return false;
        }

        bool TryParseDate(string folder, out DateTime date)
        {
            string[] formats = new[] {"yyyy-MM", "yyyy-MM-dd"};

            if (DateTime.TryParseExact(folder, formats, CultureInfo.CurrentUICulture, DateTimeStyles.None, out date))
                return true;

            return false;
        }
    }
}
