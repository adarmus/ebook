using ebook.core.BookFiles;
using ebook.core.ePub;
using ebook.core.Files;
using ebook.core.Mobi;

namespace ebook.core.Repo
{
    public class BookRepositoryFactory
    {
        public IBookRepository GetFileBasedProvider(string folderPath)
        {
            var search = new FileBasedBookRepository(folderPath);


            return search;
        }
    }
}
