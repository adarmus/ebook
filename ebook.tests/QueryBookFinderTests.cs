using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ebook.core.DataTypes;
using ebook.core.Logic;
using ebook.core.Repo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace ebook.tests
{
    [TestClass]
    public class QueryBookFinderTests
    {
        [TestMethod]
        public async Task Find_NoMatch_ReturnsStatusNewBook()
        {
            var books = Substitute.For<IFullDataSource>();
            var finder = new QueryBookFinder(books);
            var result = await finder.Find(MakeBook("456", "Title2", "Author2"));

            Assert.IsNotNull(result);
            Assert.AreEqual(MatchStatus.NewBook, result.Status);
            Assert.IsNull(result.Book);
        }

        [TestMethod]
        public async Task Find_MatchedIsbn_ReturnsStatusUptoDate()
        {
            var books = Substitute.For<IFullDataSource>();
            books.GetBookByIsbn("123").Returns( MakeBook("123", "Title1", "Author1", "EPUB") );
            var matcher = new QueryBookFinder(books);
            var result = await matcher.Find(MakeBook("123", "Title1", "Author1", "EPUB"));

            Assert.IsNotNull(result);
            Assert.AreEqual(MatchStatus.UpToDate, result.Status);
            Assert.IsNotNull(result.Book);
            Assert.AreEqual("Title1", result.Book.Title);
            Assert.AreEqual("Author1", result.Book.Author);
        }

        [TestMethod]
        public async Task Find_MatchedTitleAuthor_ReturnsStatusUptoDate()
        {
            var books = Substitute.For<IFullDataSource>();
            books.GetBookByTitleAuthor("Title1", "Author1").Returns(MakeBook("123", "Title1", "Author1", "EPUB"));
            var matcher = new QueryBookFinder(books);
            var result = await matcher.Find(MakeBook("123", "Title1", "Author1", "EPUB"));

            Assert.IsNotNull(result);
            Assert.AreEqual(MatchStatus.UpToDate, result.Status);
            Assert.IsNotNull(result.Book);
            Assert.AreEqual("Title1", result.Book.Title);
            Assert.AreEqual("Author1", result.Book.Author);
        }

        [TestMethod]
        public async Task Find_MatchedTitleAuthorMissinFile_ReturnsStatusNewFiles()
        {
            var books = Substitute.For<IFullDataSource>();
            books.GetBookByTitleAuthor("Title1", "Author1").Returns(MakeBook("123", "Title1", "Author1", "EPUB"));
            var matcher = new QueryBookFinder(books);
            var result = await matcher.Find(MakeBook("123", "Title1", "Author1", "MOBI"));

            Assert.IsNotNull(result);
            Assert.AreEqual(MatchStatus.NewFiles, result.Status);
            Assert.IsNotNull(result.Book);
            Assert.AreEqual("Title1", result.Book.Title);
            Assert.AreEqual("Author1", result.Book.Author);
            Assert.AreEqual("MOBI", result.NewTypes.ToArray()[0]);
        }

        BookInfo MakeBook(string isbn, string title, string author, params string[] types)
        {
            return new BookInfo { Isbn = isbn, Title = title, Author = author, Types = types };
        }
    }
}
