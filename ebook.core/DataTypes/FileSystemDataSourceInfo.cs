using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ebook.core.DataTypes
{
    public class FileSystemDataSourceInfo : ISimpleDataSourceInfo
    {
        public string Parameter { get; set; }

        public string Description
        {
            get { return string.Format("Folder: {0}", this.Parameter); }
        }
    }
}
