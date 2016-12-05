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
    public class SqlRepository : IBookRepository
    {
        readonly SqlConnectionStringBuilder _builder;

        public SqlRepository(string connectionString)
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

        private IBookSqlDal GetBookSqlDal()
        {
            var repo = _builder.Connection().As<IBookSqlDal>();
            return repo;
        }

        public async Task SaveBooks(IEnumerable<BookInfo> books)
        {
            IBookSqlDal repo = GetBookSqlDal();

            foreach (var book in books)
            {
                try
                {
                    await repo.BookIns(book);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
        }
    }
}
