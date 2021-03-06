﻿using System;
using System.Collections.Generic;
using ebook.core.BookFiles;
using ebook.core.DataTypes;
using ebook.core.ePub.ePubReader;
using ebook.core.Logic;

namespace ebook.core.ePub
{
    public class EpubReader : IBookFileReader
    {
        public EpubReader()
        {
            Extension = BookExtensions.EPUB;
        }

        public string Extension { get; }

        public BookFile Read(string filepath)
        {
            BookFile book = null;

            var epub = new Epub(filepath);

            book = new BookFile();
            book.Author = GetFirstSafe(epub.Creator);
            book.Title = GetFirstSafe(epub.Title);
            book.Publisher = GetFirstSafe(epub.Publisher);
            book.Description = GetFirstSafe(epub.Description);
            book.PublishDate = GetFirstSafe(epub.Date);
            book.Isbn = epub.ISBN;
            book.FilePath = filepath;

            return book;
        }

        string GetFirstSafe(List<DateData> dates)
        {
            if (dates == null)
                return null;

            if (dates.Count == 0)
                return null;

            return dates[0].Date;
        }

        string GetFirstSafe(List<string> strings)
        {
            if (strings == null)
                return null;

            if (strings.Count == 0)
                return null;

            return strings[0];
        }
    }
}
