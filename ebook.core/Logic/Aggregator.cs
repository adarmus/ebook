using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ebook.core.BookFiles;
using ebook.core.DataTypes;

namespace ebook.core.Logic
{
    /// <summary>
    /// Groups a list of book files by ISBN.
    /// </summary>
    public class Aggregator
    {
        Dictionary<string, BookFilesInfo> _lookup;
        readonly DateAddedProvider _dateAdded;

        public Aggregator()
        {
            _dateAdded = new DateAddedProvider(null);
        }

        public Aggregator(DateAddedProvider dateAdded)
        {
            _dateAdded = dateAdded;
        }

        public IEnumerable<BookInfo> GetBookList(IEnumerable<BookFile> bookFiles)
        {
            _lookup = new Dictionary<string, BookFilesInfo>();

            //IEnumerable<BookInfo> bookList = GetBookListWithIsbn(bookFiles);

            //IEnumerable<BookInfo> bookListNoIsbn = GetBookListWithoutIsbn(bookFiles);

            IEnumerable<BookInfo> bookList = GetBookListInt(bookFiles);

            return bookList;
            //return bookList.Union(bookListNoIsbn);
        }

        public Dictionary<string, BookFilesInfo> GetBookContentInfoLookup()
        {
            return _lookup;
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
            DateTime date = _dateAdded.GetDateFromFilePath(first);

            var book = new BookInfo // (files)
            {
                Id = Guid.NewGuid().ToString(),
                Isbn = isbn,
                Author = first.Author,
                Description = first.Description,
                PublishDate = first.PublishDate,
                Publisher = first.Publisher,
                Title = first.Title,
                Types = GetFileTypes(files),
                DateAdded = date
            };

            _lookup.Add(book.Id, new BookFilesInfo(book, files));

            return book;
        }

        IEnumerable<BookInfo> GetBookListInt(IEnumerable<BookFile> bookFiles)
        {
            var booksNoIsbn =
                from m in bookFiles
                group m by new
                {
                    m.Title,
                    m.Author
                }
                into g
                select new { Title = g.Key.Title, Author = g.Key.Author, Files = g };

            var bookListNoIsbn = booksNoIsbn.Select(b =>
            {
                BookFile first = b.Files.FirstOrDefault(f => !string.IsNullOrEmpty(f.Isbn));

                if (first == null)
                    first = b.Files.First();

                var files = b.Files.Select(f => f.FilePath);

                return GetBook(first, files);
            });
            return bookListNoIsbn;
        }

        BookInfo GetBook(BookFile first, IEnumerable<string> files)
        {
            DateTime date = _dateAdded.GetDateFromFilePath(first);

            var book = new BookInfo 
            {
                Id = Guid.NewGuid().ToString(),
                Isbn = first.Isbn,
                Author = first.Author,
                Description = first.Description,
                PublishDate = first.PublishDate,
                Publisher = first.Publisher,
                Title = first.Title,
                Types = GetFileTypes(files),
                DateAdded = date
            };

            _lookup.Add(book.Id, new BookFilesInfo(book, files));

            return book;
        }

        IEnumerable<BookInfo> GetBookListWithoutIsbn(IEnumerable<BookFile> bookFiles)
        {
            var booksNoIsbn =
                from m in bookFiles
                where string.IsNullOrEmpty(m.Isbn)
                group m by new
                {
                    m.Title,
                    m.Author
                }
                into g
                select new { Title = g.Key.Title, Author = g.Key.Author, Files = g };

            var bookListNoIsbn = booksNoIsbn.Select(b =>
            {
                BookFile first = b.Files.First();

                var files = b.Files.Select(f => f.FilePath);

                return GetBookWithoutIsbn(first, files);
            });
            return bookListNoIsbn;
        }

        BookInfo GetBookWithoutIsbn(BookFile first, IEnumerable<string> files)
        {
            DateTime date = _dateAdded.GetDateFromFilePath(first);

            var book = new BookInfo // (first.FilePath)
            {
                Id = Guid.NewGuid().ToString(),
                Isbn = "",
                Author = first.Author,
                Description = first.Description,
                PublishDate = first.PublishDate,
                Publisher = first.Publisher,
                Title = first.Title,
                Types = GetFileTypes(files),
                DateAdded = date
            };

            _lookup.Add(book.Id, new BookFilesInfo(book, files));

            return book;
        }

        IEnumerable<string> GetFileTypes(IEnumerable<string> files)
        {
            var exts = files.Select(GetFileType);
            return exts;
        }

        string GetFileType(string path)
        {
            string ext = Path.GetExtension(path);

            if (string.IsNullOrEmpty(ext))
                return null;

            return ext.Substring(1).ToUpper();
        }
    }
}
