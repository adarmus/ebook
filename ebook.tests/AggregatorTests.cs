using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ebook.core.BookFiles;
using ebook.core.DataTypes;
using ebook.core.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ebook.tests
{
    [TestClass]
    public class AggregatorTests
    {
        [TestMethod]
        public void GetBookList_Empty_ReturnsEmpty()
        {
            var files = new BookFile[] {};

            var agg = new Aggregator();
            BookInfo[] books = agg.GetBookList(files).ToArray();
            var lookup = agg.GetBookContentInfoLookup();

            Assert.IsNotNull(books);
            Assert.AreEqual(0, books.Length);
            Assert.AreEqual(0, lookup.Count);
        }

        [TestMethod]
        public void GetBookList_NoIsbns_ReturnsListOfFiles()
        {
            var files = new BookFile[]
            {
                new BookFile { Isbn = null, Title = "Book1", Author = "Author1", FilePath = "File1.mobi"},
                new BookFile { Isbn = null, Title = "Book2", Author = "Author2", FilePath = "File2.mobi"},
                new BookFile { Isbn = null, Title = "Book3", Author = "Author3", FilePath = "File3.mobi"},
            };

            var agg = new Aggregator();
            BookInfo[] books = agg.GetBookList(files).ToArray();
            var lookup = agg.GetBookContentInfoLookup();

            Assert.IsNotNull(books);
            Assert.AreEqual(3, books.Length);
            Assert.AreEqual(3, lookup.Count);

            AssertBookInfoNoIsbn(books[0], "Book1", "Author1");
            AssertBookInfoNoIsbn(books[1], "Book2", "Author2");
            AssertBookInfoNoIsbn(books[2], "Book3", "Author3");

            AssertBookInfoNoIsbn(lookup[books[0].Id].Book, "Book1", "Author1");
            AssertBookInfoNoIsbn(lookup[books[1].Id].Book, "Book2", "Author2");
            AssertBookInfoNoIsbn(lookup[books[2].Id].Book, "Book3", "Author3");

            AssertBookFiles(lookup[books[0].Id], new [] { "File1.mobi" });
            AssertBookFiles(lookup[books[1].Id], new [] { "File2.mobi" });
            AssertBookFiles(lookup[books[2].Id], new [] { "File3.mobi" });
        }

        [TestMethod]
        public void GetBookList_NoIsbnsMatchingAuthorDifferentTitles_ReturnsListOfFiles()
        {
            var files = new BookFile[]
            {
                new BookFile { Isbn = null, Title = "Book1", Author = "Author1", FilePath = "File1.mobi"},
                new BookFile { Isbn = null, Title = "Book2", Author = "Author1", FilePath = "File2.mobi"},
                new BookFile { Isbn = null, Title = "Book3", Author = "Author1", FilePath = "File3.mobi"},
            };

            var agg = new Aggregator();
            BookInfo[] books = agg.GetBookList(files).ToArray();
            var lookup = agg.GetBookContentInfoLookup();

            Assert.IsNotNull(books);
            Assert.AreEqual(3, books.Length);
            Assert.AreEqual(3, lookup.Count);

            AssertBookInfoNoIsbn(books[0], "Book1", "Author1");
            AssertBookInfoNoIsbn(books[1], "Book2", "Author1");
            AssertBookInfoNoIsbn(books[2], "Book3", "Author1");

            AssertBookInfoNoIsbn(lookup[books[0].Id].Book, "Book1", "Author1");
            AssertBookInfoNoIsbn(lookup[books[1].Id].Book, "Book2", "Author1");
            AssertBookInfoNoIsbn(lookup[books[2].Id].Book, "Book3", "Author1");

            AssertBookFiles(lookup[books[0].Id], new[] { "File1.mobi" });
            AssertBookFiles(lookup[books[1].Id], new[] { "File2.mobi" });
            AssertBookFiles(lookup[books[2].Id], new[] { "File3.mobi" });
        }

        [TestMethod]
        public void GetBookList_NoIsbnsSomeTheSame_ReturnsFilesGroupedByTitleAuthor()
        {
            var files = new BookFile[]
            {
                new BookFile { Isbn = null, Title = "Book1", Author = "Author1", FilePath = "File1.mobi"},
                new BookFile { Isbn = null, Title = "Book1", Author = "Author1", FilePath = "File2.epub"},
                new BookFile { Isbn = null, Title = "Book3", Author = "Author3", FilePath = "File3.mobi"},
            };

            var agg = new Aggregator();
            BookInfo[] books = agg.GetBookList(files).ToArray();
            var lookup = agg.GetBookContentInfoLookup();

            Assert.IsNotNull(books);
            Assert.AreEqual(2, books.Length);
            Assert.AreEqual(2, lookup.Count);

            AssertBookInfoNoIsbn(books[0], "Book1", "Author1");
            AssertBookInfoNoIsbn(books[1], "Book3", "Author3");

            AssertBookInfoNoIsbn(lookup[books[0].Id].Book, "Book1", "Author1");
            AssertBookInfoNoIsbn(lookup[books[1].Id].Book, "Book3", "Author3");

            AssertBookFiles(lookup[books[0].Id], new[] { "File1.mobi", "File2.epub" });
            AssertBookFiles(lookup[books[1].Id], new[] { "File3.mobi" });
        }

        [TestMethod]
        public void GetBookList_IsbnsAllDifferent_ReturnsListOfFiles()
        {
            var files = new BookFile[]
            {
                new BookFile { Isbn = "1000000000001", Title = "Book1", Author = "Author1", FilePath = "File1.mobi"},
                new BookFile { Isbn = "1000000000002", Title = "Book2", Author = "Author2", FilePath = "File2.mobi"},
                new BookFile { Isbn = "1000000000003", Title = "Book3", Author = "Author3", FilePath = "File3.mobi"},
            };

            var agg = new Aggregator();
            BookInfo[] books = agg.GetBookList(files).ToArray();
            var lookup = agg.GetBookContentInfoLookup();

            Assert.IsNotNull(books);
            Assert.AreEqual(3, books.Length);
            Assert.AreEqual(3, lookup.Count);

            AssertBookInfoNoIsbn(books[0], "Book1", "Author1");
            AssertBookInfoNoIsbn(books[1], "Book2", "Author2");
            AssertBookInfoNoIsbn(books[2], "Book3", "Author3");

            AssertBookInfoNoIsbn(lookup[books[0].Id].Book, "Book1", "Author1");
            AssertBookInfoNoIsbn(lookup[books[1].Id].Book, "Book2", "Author2");
            AssertBookInfoNoIsbn(lookup[books[2].Id].Book, "Book3", "Author3");

            AssertBookFiles(lookup[books[0].Id], new[] { "File1.mobi" });
            AssertBookFiles(lookup[books[1].Id], new[] { "File2.mobi" });
            AssertBookFiles(lookup[books[2].Id], new[] { "File3.mobi" });
        }

        [TestMethod]
        public void GetBookList_IsbnsSometheSame_ReturnsFilesGroupedByIsbn()
        {
            var files = new BookFile[]
            {
                new BookFile { Isbn = "1000000000001", Title = "Book1", Author = "Author1", FilePath = "File1.mobi"},
                new BookFile { Isbn = "1000000000001", Title = "Book2", Author = "Author2", FilePath = "File2.mobi"},
                new BookFile { Isbn = "1000000000003", Title = "Book3", Author = "Author3", FilePath = "File3.mobi"},
            };

            var agg = new Aggregator();
            BookInfo[] books = agg.GetBookList(files).ToArray();
            var lookup = agg.GetBookContentInfoLookup();

            Assert.IsNotNull(books);
            Assert.AreEqual(2, books.Length);
            Assert.AreEqual(2, lookup.Count);

            AssertBookInfoNoIsbn(books[0], "Book1", "Author1");
            AssertBookInfoNoIsbn(books[1], "Book3", "Author3");

            AssertBookInfoNoIsbn(lookup[books[0].Id].Book, "Book1", "Author1");
            AssertBookInfoNoIsbn(lookup[books[1].Id].Book, "Book3", "Author3");

            AssertBookFiles(lookup[books[0].Id], new[] { "File1.mobi", "File2.mobi" });
            AssertBookFiles(lookup[books[1].Id], new[] { "File3.mobi" });
        }

        [Ignore]
        [TestMethod]
        public void GetBookList_SameBookWithAndWithoutIsbn_ReturnsFilesGroupedByIsbn()
        {
            var files = new BookFile[]
            {
                new BookFile { Isbn = "1000000000001", Title = "Book1", Author = "Author1", FilePath = "File1.mobi"},
                new BookFile { Isbn = null, Title = "Book1", Author = "Author1", FilePath = "File2.mobi"},
                new BookFile { Isbn = "1000000000003", Title = "Book3", Author = "Author3", FilePath = "File3.mobi"},
            };

            var agg = new Aggregator();
            BookInfo[] books = agg.GetBookList(files).ToArray();
            var lookup = agg.GetBookContentInfoLookup();

            Assert.IsNotNull(books);
            Assert.AreEqual(2, books.Length);
            Assert.AreEqual(2, lookup.Count);

            AssertBookInfoNoIsbn(books[0], "Book1", "Author1");
            AssertBookInfoNoIsbn(books[1], "Book3", "Author3");

            AssertBookInfoNoIsbn(lookup[books[0].Id].Book, "Book1", "Author1");
            AssertBookInfoNoIsbn(lookup[books[1].Id].Book, "Book3", "Author3");

            AssertBookFiles(lookup[books[0].Id], new[] { "File1.mobi", "File2.mobi" });
            AssertBookFiles(lookup[books[1].Id], new[] { "File3.mobi" });
        }

        void AssertBookFiles(BookFilesInfo files, string[] fileIds)
        {
            Assert.IsNull(files.Files);
            Assert.AreEqual(fileIds.Length, files.FileIds.Count());

            var arrFileIds = files.FileIds.ToArray();
            for (int i = 0; i < fileIds.Length; i++)
            {
                Assert.AreEqual(fileIds[i], arrFileIds[i]);
            }
        }

        void AssertBookInfoNoIsbn(BookInfo book, string title, string author)
        {
            Assert.AreEqual(title, book.Title);
            Assert.AreEqual(author, book.Author);
        }
    }
}
