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
    /// タグに関するSQLオブジェクト
    /// </summary>
    public class HNKTagSql : HNKSql
    {
        #region コンストラクタ
        public HNKTagSql()
            : base()
        {

        } 
        #endregion
    }

    /// <summary>
    /// タグに関するSQLオブジェクトを生成します
    /// </summary>
    static public class HNKTagSqlFactory
    {
        /// <summary>
        /// タグを取得するSQLオブジェクトを生成します
        /// </summary>
        /// <param name="configs">設定項目。NULLもしくは要素が0の場合は空文字を返します</param>
        /// <returns>SQL文</returns>
        static public HNKTagSql GetTags(List<int> tagIds)
        {
            if (tagIds == null || tagIds.Count < 1)
            {
                return new HNKTagSql();
            }

            HNKTagSql sql = new HNKTagSql();
            for (int i = 0; i < tagIds.Count; i++)
            {
                if (i == 0)
                {
                    sql.Query.Append("SELECT id, name, name_yomi From tags WHERE ");
                }

                int tagId = tagIds[i];
                string idParamName = string.Format("@id{0}", i);

                sql.Query.AppendFormat("id = {0}", idParamName);
                sql.AddSqlParameter(ParameterDirection.Input, idParamName, SqlDbType.Int, tagId);

                if (i + 1 != tagIds.Count)
                {
                    sql.Query.Append(" OR ");
                }
            }

            return sql;
        }
    }
}
