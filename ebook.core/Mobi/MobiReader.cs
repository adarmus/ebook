using ebook.core.Mobi.Pdb;

namespace ebook.core.Mobi
{
    public class MobiReader : IBookFileReader
    {
        public BookFile Read(string filepath)
        {
            BookFile mobi = null;
            try
            {
                var reader = new PdbFileReader(filepath);
                mobi = reader.ReadMobiFile();
                mobi.FilePath = filepath;
            }
            catch
            {

            }
            return mobi;
        }
    }
}
