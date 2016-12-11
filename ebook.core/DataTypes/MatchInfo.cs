﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ebook.core.DataTypes
{
    public class MatchInfo
    {
        public MatchInfo(BookInfo book)
        {
            this.Book = book;
            this.MatchedBook = null;
        }

        public BookInfo Book { get; set; }

        public BookInfo MatchedBook { get; set; }

        public bool IsSelected { get; set; }

        public MatchStatus Status { get; set; }

        public void SetMatch(BookInfo matchedBook, MatchStatus status)
        {
            this.MatchedBook = matchedBook;
            this.Status = status;
            this.IsSelected = (status == MatchStatus.NewBook || status == MatchStatus.NewFiles);
        }
    }
}
