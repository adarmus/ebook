using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shell.Files;
using Shell.Pdb;

namespace Shell
{
    class FileTest
    {
        public void Go()
        {
            var files = new FileFinder(@"C:\MyDev\eBook\mobi\eBooks");
            var mobilist = new MobiFileList(files);

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
            var files = new FileFinder(@"C:\MyDev\eBook\mobi\eBooks");
            files.GetFileList()
                .ToList()
                .ForEach(Output);

            Console.WriteLine("done");
            Console.ReadKey();
        }

        void Output(string filepath)
        {
            var reader = new PdbFileReader(filepath);
            MobiFile mobi = reader.ReadMobiFile();

            Console.WriteLine("{0}, {1}, {2}, {3}, {4}", mobi.Title, mobi.Author, mobi.Isbn, mobi.Publisher, mobi.PublishDate);
        }
    }
}
