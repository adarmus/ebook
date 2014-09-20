using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shell.Pdb;

namespace Shell
{
    class FileTest
    {
        public void Go()
        {
            Output(@"C:\MyDev\eBook\mobi\A_Rage_in_Harlem_-_Chester_Himes.mobi");
            Output(@"C:\MyDev\eBook\mobi\bb-John.mobi");
            Output(@"C:\MyDev\eBook\mobi\pg201-images.mobi");
            Output(@"C:\MyDev\eBook\mobi\Dark Eden - Beckett, Chris.mobi");
            Output(@"C:\MyDev\eBook\mobi\Vorrh_The_-_B.mobi");

            Console.WriteLine("done");
        }

        void Output(string filepath)
        {
            var reader = new PdbFileReader(filepath);
            MobiFile mobi = reader.ReadMobiFile();

            Console.WriteLine("{0}, {1}, {2}, {3}, {4}", mobi.Title, mobi.Author, mobi.Isbn, mobi.Publisher, mobi.PublishDate);
        }
    }
}
