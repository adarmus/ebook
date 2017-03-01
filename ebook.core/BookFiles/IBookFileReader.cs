namespace ebook.core.BookFiles
{
    public interface IBookFileReader
    {
        string Extension { get; }

        BookFile Read(string filepath);
    }
}
