using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ebook.core.DataTypes;
using ebook.core.Files;
using Insight.Database;

namespace ebook.core.Repo.SqlLite
{
    public class SqlLiteRefDataSource : IFullDataSource
    {
        readonly SQLiteConnectionStringBuilder _builder;
        readonly FileReader _reader;

        public SqlLiteRefDataSource(string dataFile)
        {
            _builder = new SQLiteConnectionStringBuilder() { DataSource = dataFile };
            _reader = new FileReader();
        }

        public async Task<IEnumerable<BookInfo>> GetBooks(bool includeMobi, bool includeEpub)
        {
            try
            {
                IEnumerable<BookInfo> books = await GetBookSqlDal().BookSelAll();

                foreach (var b in books)
                {
                    await AddFileTypes(b);
                }

                return books;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<BookInfo> GetBookByIsbn(string isbn)
        {
            BookInfo book = await GetBookSqlDal().BookSelByIsbn(isbn);

            await AddFileTypes(book);

            return book;
        }

        public async Task<BookInfo> GetBookByTitleAuthor(string title, string author)
        {
            BookInfo book = await GetBookSqlDal().BookSelByTitleAuthor(title, author);

            await AddFileTypes(book);

            return book;
        }

        async Task AddFileTypes(BookInfo book)
        {
            if (book == null)
                return;

            IEnumerable<string> types = await GetFileTypes(book.Id);

            book.Types = types;
        }

        async Task<IEnumerable<string>> GetFileTypes(string bookId)
        {
            IEnumerable<BookFileInfo> types = await GetBookSqlDal().BookFileSelTypeByBookId(bookId);

            return types.Select(t => t.FileType);
        }

        public async Task<BookFilesInfo> GetBookContent(BookInfo book)
        {
            try
            {
                IBookSqlLiteRefDal repo = GetBookSqlDal();

                BookInfo info = await repo.BookSelById(book.Id);

                IEnumerable<BookFileInfo> files = await repo.BookFileSelByBookId(book.Id);

                await AddFileContent(files);

                return new BookFilesInfo(info, files);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        async Task AddFileContent(IEnumerable<BookFileInfo> files)
        {
            foreach (var file in files)
            {
                byte[] content = await _reader.ReadAllFileAsync(file.FilePath);

                file.Content = content;
            }
        }

        public async Task SaveBook(BookInfo book)
        {
            await GetBookSqlDal().BookIns(book);
        }

        public async Task SaveFile(BookFileInfo file)
        {
            await GetBookSqlDal().FileIns(file);
        }

        private IBookSqlLiteRefDal GetBookSqlDal()
        {
            var connection = new SQLiteConnection(_builder.ConnectionString);
            return connection.As<IBookSqlLiteRefDal>();
        }
    }
}
