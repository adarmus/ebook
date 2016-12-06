using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ebook.core.Repo;
using ebook.core.Repo.Sql;

namespace ebook.core.DataTypes
{
    public class SqlDataSourceInfo : IFullDataSourceInfo
    {
        public string Parameter { get; set; }

        public string Description
        {
            get { return string.Format("Sql: {0}", this.Parameter); }
        }

        public ISimpleDataSource GetSimpleDataSource()
        {
            return new SqlDataSource(this.Parameter);
        }

        public IFullDataSource GetFullDataSource()
        {
            return new SqlDataSource(this.Parameter);
        }
    }
}
