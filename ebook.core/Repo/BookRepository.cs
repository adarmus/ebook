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
        readonly IBookRepository _repository;

        public BookRepository(IBookRepository repository)
        {
            _repository = repository;
        }

        public async Task SaveBooks(IEnumerable<BookInfo> books)
        {
            foreach (var book in books)
            {
                try
                {
                    await _repository.SaveBook(book);
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