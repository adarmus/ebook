using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shell.Pdb;

namespace Shell
{
    class Files
    {
        public void Go()
        {
            var reader = new PdbFileReader(@"C:\MyDev\eBook\mobi\A_Rage_in_Harlem_-_Chester_Himes.mobi");
            //var reader = new PdbFileReader(@"C:\MyDev\eBook\mobi\bb-John.mobi");
            //var reader = new PdbFileReader(@"C:\MyDev\eBook\mobi\pg201-images.mobi");
  //          var reader = new PdbFileReader(@"C:\MyDev\eBook\mobi\Dark Eden - Beckett, Chris.mobi");
    //        var reader = new PdbFileReader(@"C:\MyDev\eBook\mobi\Vorrh_The_-_B.mobi");

            var mobi = reader.ReadMobiFile();
        }
    }
}
