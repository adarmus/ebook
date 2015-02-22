using System;
using System.Collections.Generic;
using System.Linq;
using Shell.Pdb;

namespace Shell
{
    public class Aggregator
    {
        public IEnumerable<BookInfo> GetBookList(IEnumerable<BookFile> bookFiles)
        {
            var booksWithIsbn =
                from m in bookFiles
                where !string.IsNullOrEmpty(m.Isbn)
                group m by m.Isbn
                into g
                select new {Isbn = g.Key, MobiFiles = g};

            var bookList = booksWithIsbn.Select(b =>
            {
                BookFile first = b.MobiFiles.First();

                var files = b.MobiFiles.Select(f => f.FilePath);

                var book = new BookInfo(files)
                {
                    Isbn = b.Isbn,
                    Author = first.Author,
                    Description = first.Description,
                    PublishDate = first.PublishDate,
                    Publisher = first.Publisher,
                    Title = first.Title
                };

                return book;
            });

            var booksNoIsbn =
                from m in bookFiles
                where string.IsNullOrEmpty(m.Isbn)
                select new {MobiFiles = m};

            var bookListNoIsbn = booksNoIsbn.Select(b =>
            {
                BookFile first = b.MobiFiles;

                var files = first.FilePath;

                var book = new BookInfo(files)
                {
                    Isbn = "",
                    Author = first.Author,
                    Description = first.Description,
                    PublishDate = first.PublishDate,
                    Publisher = first.Publisher,
                    Title = first.Title
                };

                return book;
            });

            return bookList.Union(bookListNoIsbn);
        }

        void CompareAllFiles(IEnumerable<BookFile> mobis)
        {
            BookFile first = null;

            foreach (var mobi in mobis)
            {
                if (first == null)
                {
                    first = mobi;
                }
                else
                {
                    OutputComparison(first, mobi);
                }
            }
        }

        void OutputComparison(BookFile first, BookFile mobi)
        {
            bool titleOk = first.Title == mobi.Title;
            bool authorOk = first.Author == mobi.Author;
            bool publisherOk = first.Publisher == mobi.Publisher;

            if (authorOk && titleOk && publisherOk)
                return;

            Console.WriteLine("  {0}: {1}{2}{3}  {4}{5}{6}",
                first.Isbn,
                titleOk ? " " : "T",
                authorOk ? " " : "A",
                publisherOk ? " " : "P",
                titleOk ? "" : string.Format("({0} -> {1})", first.Title, mobi.Title),
                authorOk ? "" : string.Format("({0} -> {1})", first.Author, mobi.Author),
                publisherOk ? "" : string.Format("({0} -> {1})", first.Publisher, mobi.Publisher)
                );
        }
    }
}
