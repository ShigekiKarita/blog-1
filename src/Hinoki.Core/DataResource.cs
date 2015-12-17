using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Hinoki.Core
{
    /// <summary>
    /// データをデータベースから取得するためのクラス
    /// </summary>
    public class DataResource : IDisposable
    {
        #region メンバ変数

        protected bool _disposed = false;
        private SqlConnection _connection = null;

        #endregion

        #region プロパティ

        public SqlConnection SqlConnection
        {
            get { return _connection; }
        }

        #endregion

        #region IDisposableの実装

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                Close();
            }
            _disposed = true;
        }

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ。データベース接続をオープンします
        /// </summary>
        /// <param name="connectionString">接続文字列</param>
        public DataResource(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString) || string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("接続文字列が不正です", "connetionString");
            }
            else
            {
                _connection = new SqlConnection(connectionString);
                _connection.Open();
            }
        }

        #endregion

        public void Close()
        {
            if (_connection != null && _connection.State != System.Data.ConnectionState.Closed)
            {
                _connection.Close();
                _connection.Dispose();
            }
        }

    }
}
