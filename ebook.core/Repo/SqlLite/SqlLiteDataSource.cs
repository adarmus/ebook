using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ebook.core.DataTypes;
using Insight.Database;

namespace ebook.core.Repo.SqlLite
{
    public class SqlLiteDataSource : IFullDataSource
    {
        readonly SQLiteConnectionStringBuilder  _builder;

        public SqlLiteDataSource(string dataFile)
        {
            _builder = new SQLiteConnectionStringBuilder() { DataSource = dataFile };
        }

        public async Task<IEnumerable<BookInfo>> GetBooks(bool includeMobi, bool includeEpub)
        {
            try
            {
                return await GetBookSqlDal().BookSelAll();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<BookFilesInfo> GetBookContent(BookInfo book)
        {
            try
            {
                IBookSqlLiteDal repo = GetBookSqlDal();

                BookInfo info = await repo.BookSelById(book.Id);

                IEnumerable<BookFileInfo> files = await repo.BookFileSelByBookId(book.Id);

                return new BookFilesInfo(info, files);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
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

        public async Task<BookInfo> GetBookByIsbn(string isbn)
        {
            return await GetBookSqlDal().BookSelByIsbn(isbn);
        }

        public async Task<BookInfo> GetBookByTitleAuthor(string title, string author)
        {
            return await GetBookSqlDal().BookSelByTitleAuthor(title, author);
        }

        private IBookSqlLiteDal GetBookSqlDal()
        {
            var connection = new SQLiteConnection(_builder.ConnectionString);
            return connection.As<IBookSqlLiteDal>();
        }
    }
}
