using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Hinoki.Core.Db
{
    public class SqlCommandHelper
    {
        /// <summary>
        /// SQLのパラメータを追加します。サニタイジング(SQLインジェクション対策)をします
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="direction"></param>
        /// <param name="paramName"></param>
        /// <param name="type"></param>
        /// <param name="value">値がnullの場合はDBNull.Valueを挿入します</param>
        public static void AddSqlParameter(SqlCommand cmd, ParameterDirection direction, string paramName, SqlDbType type, Object value)
        {
            if (cmd == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(paramName) || string.IsNullOrWhiteSpace(paramName))
            {
                return;
            }

            if (value == null)
            {
                value = DBNull.Value;
            }

            SqlParameter param = cmd.CreateParameter();
            param.ParameterName = paramName;
            param.SqlDbType = type;
            param.Direction = direction;
            param.Value = value;

            cmd.Parameters.Add(param);
        }
    }
}
