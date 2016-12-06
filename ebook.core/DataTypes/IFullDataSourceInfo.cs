using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ebook.core.Repo;

namespace ebook.core.DataTypes
{
    public interface IFullDataSourceInfo : ISimpleDataSourceInfo
    {
        IFullDataSource GetFullDataSource();
    }
}
