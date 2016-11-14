using System;
using System.Collections.Generic;
using System.Linq;
using ebook.core.BookFiles;
using ebook.core.DataTypes;

namespace ebook.core.Logic
{
    public class Aggregator
    {
        public IEnumerable<BookInfo> GetBookList(IEnumerable<BookFile> bookFiles)
        {
            IEnumerable<BookInfo> bookList = GetBookListWithIsbn(bookFiles);

            IEnumerable<BookInfo> bookListNoIsbn = GetBookListWithoutIsbn(bookFiles);

            return bookList.Union(bookListNoIsbn);
        }

        IEnumerable<BookInfo> GetBookListWithIsbn(IEnumerable<BookFile> bookFiles)
        {
            var booksWithIsbn =
                from m in bookFiles
                where !string.IsNullOrEmpty(m.Isbn)
                group m by m.Isbn
                into g
                select new {Isbn = g.Key, Files = g};

            var bookList = booksWithIsbn.Select(b =>
            {
                BookFile first = b.Files.First();

                var files = b.Files.Select(f => f.FilePath);

                return GetBookWithIsbn(first, b.Isbn, files);
            });
            return bookList;
        }

        BookInfo GetBookWithIsbn(BookFile first, string isbn, IEnumerable<string> files)
        {
            var book = new BookInfo(files)
            {
                Isbn = isbn,
                Author = first.Author,
                Description = first.Description,
                PublishDate = first.PublishDate,
                Publisher = first.Publisher,
                Title = first.Title
            };

            return book;
        }

        IEnumerable<BookInfo> GetBookListWithoutIsbn(IEnumerable<BookFile> bookFiles)
        {
            var booksNoIsbn =
                from m in bookFiles
                where string.IsNullOrEmpty(m.Isbn)
                select new {File = m};

            var bookListNoIsbn = booksNoIsbn.Select(b =>
            {
                BookFile first = b.File;

                return GetBookWithoutIsbn(first);
            });
            return bookListNoIsbn;
        }

        BookInfo GetBookWithoutIsbn(BookFile first)
        {
            var file = first.FilePath;

            var book = new BookInfo(file)
            {
                Isbn = "",
                Author = first.Author,
                Description = first.Description,
                PublishDate = first.PublishDate,
                Publisher = first.Publisher,
                Title = first.Title
            };

            return book;
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
