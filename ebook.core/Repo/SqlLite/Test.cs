using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using ebook.core.DataTypes;
using Insight.Database;

namespace ebook.core.Repo.SqlLite
{
    public class Test
    {
        public void Go()
        {
            var builder = new SQLiteConnectionStringBuilder() {DataSource = @"C:\Tree\ebook\sql\dev.db"};
            var conn = new SQLiteConnection(builder.ConnectionString);
            var results = conn.ExecuteSql("SELECT [Id], [Title],  [Author],  [Isbn], [Publisher], [Description], [DateAdded] FROM [book]");



            //var builder = new SQLiteConnectionStringBuilder(@"Data Source=C:\Tree\ebook\sql\dev.db;Version=3;");
            //var c = builder.Connection();

            var connection = new SQLiteConnection(builder.ConnectionString);

            var r = connection.QuerySql("SELECT [Id], [Title],  [Author],  [Isbn], [Publisher], [Description], [DateAdded] FROM [book]");


            //var db = new SqlLiteDataSourceInfo { Parameter = @"Data Source=C:\Tree\ebook\sql\dev.db;Version=3;" };
            //var d = db.GetFullDataSource();
            //var x = d.GetBooks(true, true);
            ////x.RunSynchronously();
            //var z = x.Result;

        }
    }
}
