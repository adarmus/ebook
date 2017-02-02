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

        public async Task SaveBooks(IEnumerable<BookFilesInfo> books, DateTime dateAdded)
        {
            foreach (var book in books)
            {
                try
                {
                    BookInfo info = book.Book;

                    info.DateAdded = dateAdded;
                    await _repository.SaveBook(info);

                    foreach (var file in book.Files)
                    {
                        var newFile = new BookFileInfo
                        {
                            Id = Guid.NewGuid().ToString(),
                            Content = file.Content,
                            BookId = file.BookId,
                            FileType = file.FileType,
                            FileName = file.FileName
                        };
                        await _repository.SaveFile(newFile);
                    }
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
