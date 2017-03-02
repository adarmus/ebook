using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ebook.core.DataTypes;
using Insight.Database;

namespace ebook.core.Repo.SqlLite
{
    public interface IBookSqlLiteRefDal
    {
        [Sql("INSERT INTO [Book]  ( [Id], [Title],  [Author],  [Isbn], [Publisher], [Description], [DateAdded] )  VALUES  ( @id, @title, @author, @isbn, @publisher, @description, @dateAdded )")]
        Task BookIns(BookInfo book);

        [Sql("SELECT [Id], [Title],  [Author],  [Isbn], [Publisher], [Description], [DateAdded] FROM [book]")]
        Task<IEnumerable<BookInfo>> BookSelAll();

        [Sql("SELECT [Id], [Title],  [Author],  [Isbn], [Publisher], [Description], [DateAdded] FROM [book] WHERE [Id] = @id")]
        Task<BookInfo> BookSelById(string id);

        [Sql("SELECT [Id], [Title], [Author], [Isbn], [Publisher], [Description], [DateAdded] FROM [book] WHERE [Isbn] = @isbn")]
        Task<BookInfo> BookSelByIsbn(string isbn);

        [Sql("SELECT [Id], [Title], [Author], [Isbn], [Publisher], [Description], [DateAdded] FROM [book] WHERE [Title] = @title AND [Author] = @author")]
        Task<BookInfo> BookSelByTitleAuthor(string title, string author);

        [Sql("INSERT INTO [File] ([Id], [BookId], [FileType], [FileName], [FilePath]) VALUES (@id, @bookid, @fileType, @fileName, @filePath)")]
        Task FileIns(BookFileInfo book);

        [Sql("SELECT [FileType] FROM [file] WHERE [BookId] = @bookId")]
        Task<IEnumerable<BookFileInfo>> BookFileSelTypeByBookId(string bookid);

        [Sql("SELECT [Id], [BookId], [FileType], [FileName], [FilePath] FROM [file] WHERE [BookId] = @bookId")]
        Task<IEnumerable<BookFileInfo>> BookFileSelByBookId(string bookid);
    }
}
