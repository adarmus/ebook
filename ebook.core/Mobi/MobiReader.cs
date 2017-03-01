using System;
using ebook.core.BookFiles;
using ebook.core.DataTypes;
using ebook.core.Logic;
using ebook.core.Mobi.Pdb;

namespace ebook.core.Mobi
{
    public class MobiReader : IBookFileReader
    {
        public MobiReader()
        {
            Extension = BookExtensions.MOBI;
        }

        public string Extension { get; }

        public BookFile Read(string filepath)
        {
            BookFile mobi = null;

            var reader = new PdbFileReader(filepath);
            mobi = reader.ReadMobiFile();
            mobi.FilePath = filepath;

            return mobi;
        }
    }
}
