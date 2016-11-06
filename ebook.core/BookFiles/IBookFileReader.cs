namespace ebook.core.BookFiles
{
    public interface IBookFileReader
    {
        BookFile Read(string filepath);
    }
}
