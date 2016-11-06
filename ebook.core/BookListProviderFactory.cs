using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ebook.core.ePub;
using ebook.core.Files;
using ebook.core.Mobi;

namespace ebook.core
{
    public class BookListProviderFactory
    {
        public IBookInfoListProvider GetFileBasedProvider(string folderPath, bool includeMobi, bool includeEpub)
        {
            var search = new FileBasedBookListProvider();

            if (includeMobi)
            {
                var mobiFiles = new FileFinder(folderPath, "mobi");
                var mobiList = new BookFileList(mobiFiles, new MobiReader());
                search.AddList(mobiList);
            }

            if (includeEpub)
            {
                var epubFiles = new FileFinder(folderPath, "epub");
                var epubList = new BookFileList(epubFiles, new EpubReader());
                search.AddList(epubList);
            }

            return search;
        }
    }
}
