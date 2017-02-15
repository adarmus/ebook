using System;
using ebook.core.BookFiles;
using ebook.core.Logic;
using ebook.core.Mobi.Pdb;

namespace ebook.core.Mobi
{
    public class MobiReader : IBookFileReader
    {
        readonly IOutputMessage _messages;

        public MobiReader(IOutputMessage messages)
        {
            _messages = messages;
        }

        public BookFile Read(string filepath)
        {
            BookFile mobi = null;
            try
            {
                var reader = new PdbFileReader(filepath);
                mobi = reader.ReadMobiFile();
                mobi.FilePath = filepath;

                _messages.Write("Read {0}", filepath);
            }
            catch (Exception ex)
            {
                _messages.Write("Error reading {0} ({1})", filepath, ex.Message);
            }

            return mobi;
        }
    }
}
