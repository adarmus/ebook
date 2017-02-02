﻿using System;
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

        public async Task SaveBooks(IEnumerable<BookFilesInfo> books)
        {
            foreach (var book in books)
            {
                try
                {
                    await SaveBook(book);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
        }

        async Task SaveBook(BookFilesInfo book)
        {
            BookInfo info = book.Book;

            await _repository.SaveBook(info);

            foreach (var file in book.Files)
            {
                await SaveBookFile(file);
            }
        }

        async Task SaveBookFile(BookFileInfo file)
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
}
