using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Hinoki.Core.Tests
{
    public class TestConfig
    {
        public static string GetDbConnectionString ()
        {
            return ConfigurationManager.ConnectionStrings["db"].ConnectionString;
        }

        /// <summary>
        /// テストデータを揃えるために必応なSQLファイルを格納しているディレクトリです。App.configのSqlDirに依存します
        /// </summary>
        public static string SqlDir
        {
            get { return ConfigurationManager.AppSettings["SqlDir"]; }
        }
    }
}
