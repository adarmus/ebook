using Shell.Pdb;
using System.Collections.Generic;

namespace Shell
{
    interface IMobiFileList
    {
        IEnumerable<MobiFile> GetMobiFiles();
    }
}
