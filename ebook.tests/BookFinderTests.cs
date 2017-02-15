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
using NSubstitute.Core;

namespace ebook.tests
{
    [TestClass]
    public class BookFinderTests
    {
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public async void Match_Null_ReturnsEmpty()
        {
            var books = Substitute.For<IFullDataSource>();
            var matcher = new BookFinder(books);
            var result = await matcher.Find(null);
        }

        [TestMethod]
        public async void Match_NoMatch_ReturnsStatusNewBook()
        {
            var books = Substitute.For<IFullDataSource>();
            books.GetBooks(true, true).Returns(new BookInfo[] {MakeBook("123", "Title1", "Author1")});
            var matcher = new BookFinder(books);
            var result = await matcher.Find(new MatchInfo(MakeBook("456", "Title2", "Author2")));

            Assert.IsNotNull(result);
            Assert.AreEqual(MatchStatus.NewBook, result.Status);
            Assert.IsNull(result.Book);
        }

        [TestMethod]
        public async void Match_MatchedIsbn_ReturnsStatusNewBook()
        {
            var books = Substitute.For<IFullDataSource>();
            books.GetBooks(true, true).Returns(new BookInfo[] { MakeBook("123", "Title1", "Author1") });
            var matcher = new BookFinder(books);
            var result = await matcher.Find(new MatchInfo(MakeBook("456", "Title2", "Author2")));

            Assert.IsNotNull(result);
            Assert.AreEqual(MatchStatus.UpToDate, result.Status);
            Assert.IsNotNull(result.Book);
            Assert.AreEqual("Title1", result.Book.Title);
            Assert.AreEqual("Author1", result.Book.Author);
        }

        BookInfo MakeBook(string isbn, string title, string author)
        {
            return new BookInfo {Isbn = isbn, Title = title, Author = author};
        }
    }
}
