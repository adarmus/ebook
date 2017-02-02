using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ebook.core.DataTypes;
using Insight.Database;

namespace ebook.core.Repo.Sql
{
    public class SqlDataSource : IFullDataSource
    {
        readonly SqlConnectionStringBuilder _builder;

        public SqlDataSource(string connectionString)
        {
            _builder = new SqlConnectionStringBuilder(connectionString);

            SqlInsightDbProvider.RegisterProvider();
        }

        public async Task<IEnumerable<BookInfo>> GetBooks(bool includeMobi, bool includeEpub)
        {
            IBookSqlDal repo = GetBookSqlDal();

            try
            {
                var books = await repo.BookSelAll();

                return books;
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
                IBookSqlDal repo = GetBookSqlDal();

                BookInfo info = await repo.BookSelById(new Guid(book.Id));

                IEnumerable<BookFileInfo> files = await repo.BookFileSelByBookId(new Guid(book.Id));

                var content = new BookFilesInfo(info, files);

                return content;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task SaveBook(BookInfo book)
        {
            IBookSqlDal repo = GetBookSqlDal();

            await repo.BookIns(book);
        }

        public async Task SaveFile(BookFileInfo file)
        {
            IBookSqlDal repo = GetBookSqlDal();

            await repo.FileIns(file);
        }

        private IBookSqlDal GetBookSqlDal()
        {
            return _builder.Connection().As<IBookSqlDal>();
        }
    }
}
