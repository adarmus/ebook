﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ebook.core.Repo;
using ebook.core.Repo.SqlLite;

namespace ebook.core.DataTypes
{
    public class SqlLiteRefDataSourceInfo : IFullDataSourceInfo
    {
        public string Parameter { get; set; }

        public string Description
        {
            get { return string.Format("SqLiteRef: {0}", this.Parameter); }
        }

        public ISimpleDataSource GetSimpleDataSource()
        {
            return GetFullDataSource();
        }

        public IFullDataSource GetFullDataSource()
        {
            return new SqlLiteRefDataSource(this.Parameter);
        }
    }
}
