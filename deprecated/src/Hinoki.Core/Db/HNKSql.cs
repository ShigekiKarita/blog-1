using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Hinoki.Core.Db
{
    public class HNKSql
    {
        #region プライベート
        private StringBuilder _query = new StringBuilder();
        private List<SqlParameter> _parameters = new List<SqlParameter> { };
        #endregion

        #region プロパティ
        /// <summary>
        /// SQL文
        /// </summary>
        public StringBuilder Query
        {
            get { return _query; }
            set { _query = value; }
        }

        /// <summary>
        /// SQLパラメーター
        /// </summary>
        public List<SqlParameter> Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public HNKSql()
        {

        }
        #endregion

        #region パブリックメソッド
        /// <summary>
        /// SQLパラメーターを追加します
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="paramName">パラメーター名</param>
        /// <param name="type">値の型</param>
        /// <param name="value">パラメータ</param>
        public void AddSqlParameter(ParameterDirection direction, string paramName, SqlDbType type, Object value)
        {
            if (string.IsNullOrEmpty(paramName) || string.IsNullOrWhiteSpace(paramName))
            {
                return;
            }

            if (value == null)
            {
                value = DBNull.Value;
            }

            SqlParameter param = new SqlParameter();
            param.ParameterName = paramName;
            param.SqlDbType = type;
            param.Value = value;
            param.Direction = direction;

            this.Parameters.Add(param);
        }
        #endregion
    }
}
