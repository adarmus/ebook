using System;
using System.Linq;
using ebook.core.Files;
using ebook.core.Mobi;
using ebook.core.Pdb;

namespace ebook.core
{
    class FileTest
    {
        public void Go()
        {
            var epubFiles = new FileFinder(@"C:\MyDev\eBook\eBooks\2014-09-17\David Mitchell [The Bone Clocks]", "epub");
            var epubList = new BookFileList(epubFiles, new ePub.EpubReader());

            var mobiFiles = new FileFinder(@"C:\MyDev\eBook\eBooks\Misc\The Death Of Bunny Munro", "mobi");
            var mobiList = new BookFileList(mobiFiles, new MobiReader());

            var search = new FileBasedBookListProvider()
                .AddList(epubList);
                //.AddList(mobiList);

            var books = search.GetBooks();

            foreach(var f in books)
            {
                Console.WriteLine("{0}", f.Title);
            }
        }

        public void Go4()
        {
            var mobiFiles = new FileFinder(@"C:\MyDev\eBook\mobi\eBooks", "mobi");
            var mobilist = new BookFileList(mobiFiles, new MobiReader());

            var writer = new LogFileWriter(@"C:\MyDev\eBook\mobi\eBooks.txt");

            var importer = new Importer(mobilist, writer);

            importer.Import();
        }

        public void Go2()
        {
            Output(@"C:\MyDev\eBook\mobi\A_Rage_in_Harlem_-_Chester_Himes.mobi");
            Output(@"C:\MyDev\eBook\mobi\bb-John.mobi");
            Output(@"C:\MyDev\eBook\mobi\pg201-images.mobi");
            Output(@"C:\MyDev\eBook\mobi\Dark Eden - Beckett, Chris.mobi");
            Output(@"C:\MyDev\eBook\mobi\Vorrh_The_-_B.mobi");

            Console.WriteLine("done");
            Console.ReadKey();
        }

        public void Go3()
        {
            var files = new FileFinder(@"C:\MyDev\eBook\mobi\eBooks", "mobi");
            files.GetFileList()
                .ToList()
                .ForEach(Output);

            Console.WriteLine("done");
            Console.ReadKey();
        }

        void Output(string filepath)
        {
            var reader = new PdbFileReader(filepath);
            BookFile mobi = reader.ReadMobiFile();

            Console.WriteLine("{0}, {1}, {2}, {3}, {4}", mobi.Title, mobi.Author, mobi.Isbn, mobi.Publisher, mobi.PublishDate);
        }
    }
}
