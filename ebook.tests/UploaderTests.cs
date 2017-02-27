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
    public class UploaderTests
    {
        [TestMethod]
        public async Task Upload_NoBooks_NoCalls()
        {
            var fileSource = Substitute.For<ISimpleDataSource>();
            var bookSource = Substitute.For<IFullDataSource>();
            var messages = Substitute.For<IOutputMessage>();
            var uploader = new Uploader(bookSource, fileSource, messages);

            await uploader.Upload(Enumerable.Empty<MatchInfo>());

            ICall[] calls = bookSource.ReceivedCalls().ToArray();

            Assert.AreEqual(0, calls.Length);
        }

        [TestMethod]
        public async Task Upload_BooksUpToDate_NoCalls()
        {
            var fileSource = Substitute.For<ISimpleDataSource>();
            var bookSource = Substitute.For<IFullDataSource>();
            var messages = Substitute.For<IOutputMessage>();
            var uploader = new Uploader(bookSource, fileSource, messages);

            MatchInfo[] books = new MatchInfo []
            {
                new MatchInfo(MakeBook(null, "Titlt", "Author", "MOBI"))
                {
                    IsSelected = true,
                    Status = MatchStatus.UpToDate
                } 
            };

            await uploader.Upload(books);

            ICall[] calls = bookSource.ReceivedCalls().ToArray();

            Assert.AreEqual(0, calls.Length);
        }

        [TestMethod]
        public async Task Upload_NewBooks_2Calls()
        {
            var fileSource = Substitute.For<ISimpleDataSource>();
            var bookfiles = new BookFilesInfo(MakeBook(null, "Titlt", "Author", "MOBI"),
                new BookFileInfo[]
                {
                    new BookFileInfo
                    {
                        FileName = "filepath1.mobi"
                    }
                });

            fileSource.GetBookContent(Arg.Any<BookInfo>()).Returns(Task.FromResult(bookfiles));

            var bookSource = Substitute.For<IFullDataSource>();
            var messages = Substitute.For<IOutputMessage>();
            var uploader = new Uploader(bookSource, fileSource, messages);

            MatchInfo[] books = new MatchInfo[]
            {
                new MatchInfo(MakeBook(null, "Titlt", "Author", "MOBI"))
                {
                    IsSelected = true,
                    Status = MatchStatus.NewBook,
                    MatchedBook = MakeBook(null, "Titlt", "Author", "MOBI")
                }
            };

            await uploader.Upload(books);

            ICall[] calls = bookSource.ReceivedCalls().ToArray();

            Assert.AreEqual(2, calls.Length);
            AssertCall(calls[0], "SaveBook");
            AssertCall(calls[1], "SaveFile");
        }

        [TestMethod]
        public async Task Upload_NewBooksNotSelected_NoCalls()
        {
            var fileSource = Substitute.For<ISimpleDataSource>();
            var bookfiles = new BookFilesInfo(MakeBook(null, "Titlt", "Author", "MOBI"),
                new BookFileInfo[]
                {
                    new BookFileInfo
                    {
                        FileName = "filepath1.mobi"
                    }
                });

            fileSource.GetBookContent(Arg.Any<BookInfo>()).Returns(Task.FromResult(bookfiles));

            var bookSource = Substitute.For<IFullDataSource>();
            var messages = Substitute.For<IOutputMessage>();
            var uploader = new Uploader(bookSource, fileSource, messages);

            MatchInfo[] books = new MatchInfo[]
            {
                new MatchInfo(MakeBook(null, "Titlt", "Author", "MOBI"))
                {
                    IsSelected = false,
                    Status = MatchStatus.NewBook,
                    MatchedBook = MakeBook(null, "Titlt", "Author", "MOBI")
                }
            };

            await uploader.Upload(books);

            ICall[] calls = bookSource.ReceivedCalls().ToArray();

            Assert.AreEqual(0, calls.Length);
        }

        [TestMethod]
        public async Task Upload_NewFile_1Calls()
        {
            var fileSource = Substitute.For<ISimpleDataSource>();
            var bookfiles = new BookFilesInfo(MakeBook(null, "Titlt", "Author", "MOBI"),
                new BookFileInfo[]
                {
                    new BookFileInfo
                    {
                        FileName = "filepath1.mobi"
                    }
                });

            fileSource.GetBookContent(Arg.Any<BookInfo>()).Returns(Task.FromResult(bookfiles));

            var bookSource = Substitute.For<IFullDataSource>();
            var messages = Substitute.For<IOutputMessage>();
            var uploader = new Uploader(bookSource, fileSource, messages);

            MatchInfo[] books = new MatchInfo[]
            {
                new MatchInfo(MakeBook(null, "Titlt", "Author", "EPUB"))
                {
                    IsSelected = true,
                    Status = MatchStatus.NewFiles,
                    MatchedBook = MakeBook(null, "Titlt", "Author", "MOBI")
                }
            };

            await uploader.Upload(books);

            ICall[] calls = bookSource.ReceivedCalls().ToArray();

            Assert.AreEqual(1, calls.Length);
            AssertCall(calls[0], "SaveFile");
        }

        BookInfo MakeBook(string isbn, string title, string author, params string[] types)
        {
            return new BookInfo { Isbn = isbn, Title = title, Author = author, Types = types };
        }

        void AssertCall(ICall actual, string methodName)
        {
            Assert.AreEqual(methodName, actual.GetMethodInfo().Name);
        }
    }
}
