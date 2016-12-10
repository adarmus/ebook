using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ebook.core.DataTypes;
using ebook.core.Repo.Sql;

namespace ebook.core.Repo
{
    public class BookRepository
    {
        readonly IFullDataSource _repository;

        public BookRepository(IFullDataSource repository)
        {
            _repository = repository;
        }

        public async Task SaveBooks(IEnumerable<BookContentInfo> books, DateTime dateAdded)
        {
            foreach (var book in books)
            {
                try
                {
                    BookInfo info = book.Book;

                    info.DateAdded = dateAdded;
                    await _repository.SaveBook(info);

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
