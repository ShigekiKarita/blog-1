using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Hinoki.Core.Db
{
    /// <summary>
    /// ブログ設定に関するSQLオブジェクト
    /// </summary>
    public class HNKBlogConfigSql : HNKSql
    {
        #region コンストラクタ
        public HNKBlogConfigSql()
            : base()
        {

        } 
        #endregion
    }

    /// <summary>
    /// ブログ設定に関するSQLオブジェクトを生成します
    /// </summary>
    static public class HNKBlogConfigSqlFactory
    {
        /// <summary>
        /// ブログ設定を更新するSQLオブジェクトを生成します
        /// </summary>
        /// <param name="configs">設定項目。NULLもしくは要素が0の場合は空文字を返します</param>
        /// <returns>SQL文</returns>
        static public HNKBlogConfigSql UpdateConfigs(List<BlogConfig> configs)
        {
            if (configs == null || configs.Count < 1)
            {
                return new HNKBlogConfigSql();
            }

            HNKBlogConfigSql sql = new HNKBlogConfigSql();
            for (int i = 0; i < configs.Count; i++)
            {
                BlogConfig config = configs[i];
                string valueParamName = string.Format("@value{0}", i);
                string nameParamName = string.Format("@name{0}", i);

                sql.Query.AppendFormat("UPDATE blog_config SET value = {0} WHERE name = {1}", valueParamName, nameParamName);
                sql.AddSqlParameter(ParameterDirection.Input, valueParamName, SqlDbType.NVarChar, config.Value);
                sql.AddSqlParameter(ParameterDirection.Input, nameParamName, SqlDbType.NVarChar, config.Name);

                if (i + 1 != configs.Count)
                {
                    sql.Query.Append(" ; ");
                }
            }

            return sql;
        }
    }
}
