using System;
using System.Collections.Generic;
using ebook.core.BookFiles;
using ebook.core.ePub.ePubReader;
using ebook.core.Logic;

namespace ebook.core.ePub
{
    public class EpubReader : IBookFileReader
    {
        readonly IOutputMessage _messages;

        public EpubReader(IOutputMessage messages)
        {
            _messages = messages;
        }

        public BookFile Read(string filepath)
        {
            BookFile book = null;
            try
            {
                var epub = new Epub(filepath);

                book = new BookFile();
                book.Author = GetFirstSafe(epub.Creator);
                book.Title = GetFirstSafe(epub.Title);
                book.Publisher = GetFirstSafe(epub.Publisher);
                book.Description = GetFirstSafe(epub.Description);
                book.PublishDate = GetFirstSafe(epub.Date);
                book.Isbn = epub.ISBN;
                book.FilePath = filepath;

                _messages.Write("Read {0}", filepath);
            }
            catch(Exception ex)
            {
                _messages.Write("Error reading {0} ({1})", filepath, ex.Message);
            }

            return book;
        }

        string GetFirstSafe(List<DateData> dates)
        {
            if (dates == null)
                return null;

            if (dates.Count == 0)
                return null;

            return dates[0].Date;
        }

        string GetFirstSafe(List<string> strings)
        {
            if (strings == null)
                return null;

            if (strings.Count == 0)
                return null;

            return strings[0];
        }
    }
}
