using System;
using System.Collections.Generic;
using System.Linq;

namespace ebook.core.DataTypes
{
    public class BookInfo
    {
        public BookInfo()
        {
        }

        public string Id { get; set; }

        public string Author { get; set; }

        public string Description { get; set; }

        public string Publisher { get; set; }

        public string PublishDate { get; set; }

        public string Isbn { get; set; }

        public string Title { get; set; }

        public bool HasEpubFile
        {
            get { return HasFileWithExtension(BookExtensions.EPUB); }
        }

        public bool HasMobiFile
        {
            get { return HasFileWithExtension(BookExtensions.MOBI); }
        }

        bool HasFileWithExtension(string ext)
        {
            return this.Types.Contains(ext);
        }

        public IEnumerable<string> Types { get; set; }

        public DateTime DateAdded { get; set; }
    }
}
