using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ebook.core.Repo;
using ebook.core.Repo.SqlLite;

namespace ebook.core.DataTypes
{
    public class SqlLiteDataSourceInfo : IFullDataSourceInfo
    {
        public string Parameter { get; set; }

        public string Description
        {
            get { return string.Format("SqLite: {0}", this.Parameter); }
        }

        public ISimpleDataSource GetSimpleDataSource()
        {
            return new SqlLiteDataSource(this.Parameter);
        }

        public IFullDataSource GetFullDataSource()
        {
            return new SqlLiteDataSource(this.Parameter);
        }
    }
}
