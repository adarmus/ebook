using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ebook.core.Logic;
using ebook.core.Repo;
using ebook.core.Repo.File;
using System.IO;

namespace ebook.core.DataTypes
{
    public class FileSystemDataSourceInfo : ISimpleDataSourceInfo
    {
        private readonly IOutputMessage _messages;

        public FileSystemDataSourceInfo(IOutputMessage messages)
        {
            _messages = messages;
        }

        public string Parameter { get; set; }

        public string Description
        {
            get { return string.Format("Folder: {0}", this.Parameter); }
        }

        public ISimpleDataSource GetSimpleDataSource()
        {
            string startFolder = AppDomain.CurrentDomain.BaseDirectory;

            string file = Path.Combine(startFolder, "Authors.txt");

            return new FileBasedSimpleDataSource(this.Parameter, _messages, file);
        }
    }
}
