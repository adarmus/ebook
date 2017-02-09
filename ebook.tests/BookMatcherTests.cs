using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ebook.core.DataTypes;
using ebook.core.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ebook.tests
{
    [TestClass]
    public class BookMatcherTests
    {
        [TestMethod]
        public void Match_Empty_ReturnsEmpty()
        {
            var books = new BookInfo[] {};
            var matcher = new BookMatcher(books);
            var input = new MatchInfo[] {};
            var matches = matcher.Match(input).ToArray();

            Assert.AreEqual(0, matches.Length);
        }

        [TestMethod]
        public void Match_NoMatch_ReturnsStatusNewBook()
        {
            var books = new BookInfo[] { MakeBook("123", "Title1", "Author1") };
            var matcher = new BookMatcher(books);
            var input = new MatchInfo[] { new MatchInfo(MakeBook("456", "Title2", "Author2")) };
            var matches = matcher.Match(input).ToArray();

            Assert.AreEqual(1, matches.Length);
            Assert.AreEqual(MatchStatus.NewBook, matches[0].Status);
            Assert.IsNull(matches[0].MatchedBook);
        }

        [TestMethod]
        public void Match_MatchedIsbn_ReturnsStatusNewBook()
        {
            var books = new BookInfo[] { MakeBook("123", "Title1", "Author1") };
            var matcher = new BookMatcher(books);
            var input = new MatchInfo[] { new MatchInfo(MakeBook("123", "Title1", "Author1")) };
            var matches = matcher.Match(input).ToArray();

            Assert.AreEqual(1, matches.Length);
            Assert.AreEqual(MatchStatus.UpToDate, matches[0].Status);
            Assert.IsNotNull(matches[0].MatchedBook);
            Assert.AreEqual("Title1", matches[0].MatchedBook.Title);
            Assert.AreEqual("Author1", matches[0].MatchedBook.Author);
        }

        BookInfo MakeBook(string isbn, string title, string author)
        {
            return new BookInfo {Isbn = isbn, Title = title, Author = author};
        }
    }
}
