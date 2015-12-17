using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Hinoki.Core;

namespace Hinoki.Core.Db
{
    /// <summary>
    /// ブログの設定をデータベースから取得するためのクラス
    /// </summary>
    public class HNKBlogConfigResource : DataResource
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="connectionString"></param>
        public HNKBlogConfigResource(string connectionString)
            : base(connectionString)
        {

        } 
        #endregion

        #region パブリックメソッド
        /// <summary>
        /// ブログの設定をすべて取得します
        /// </summary>
        public List<BlogConfig> GetBlogConfigs()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT id, name, value FROM blog_config");

            List<BlogConfig> configs = new List<BlogConfig> { };
            using (SqlCommand cmd = new SqlCommand(query.ToString(), this.SqlConnection))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        BlogConfig config = new BlogConfig();
                        if (!reader.IsDBNull(0))
                        {
                            config.Id = reader.GetInt32(0);
                        }

                        if (!reader.IsDBNull(1))
                        {
                            config.Name = reader.GetString(1).Trim();
                        }

                        if (!reader.IsDBNull(2))
                        {
                            config.Value = reader.GetString(2).Trim();
                        }

                        if (config.Id > 0)
                        {
                            configs.Add(config);
                        }
                    }
                }
            }

            return configs;
        }

        public bool UpdateBlogConfigs(List<BlogConfig> configs)
        {
            bool isSuccess = false;

            HNKBlogConfigSql sql = HNKBlogConfigSqlFactory.UpdateConfigs(configs);
            using (SqlCommand cmd = new SqlCommand(sql.Query.ToString(), this.SqlConnection))
            {
                cmd.Parameters.AddRange(sql.Parameters.ToArray()); 
                int affectedRowNumber = 0;
                try
                {
                    // ExecuteNonQueryはエラー発生時に-1を返す
                    affectedRowNumber = cmd.ExecuteNonQuery();
                    if (affectedRowNumber > 0)
                    {
                        isSuccess = true;
                    }
                }
                catch (Exception e)
                {
                }
            }

            return isSuccess;
        }

        private void Hoge(List<BlogConfig> configs)
        {
            StringBuilder builder = new StringBuilder();

            int idx = 0;
            foreach (BlogConfig config in configs)
            {
                builder.AppendFormat("value = @value{0}", idx);
                idx++;
            }
        }
        #endregion

        /// <summary>
        /// 特定のブログの設定を取得します
        /// </summary>
        private BlogConfig GetBlogConfig(string name)
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT id, name, value FROM blog_config WHERE name = @name");

            BlogConfig config = new BlogConfig();
            using (SqlCommand cmd = new SqlCommand(query.ToString(), this.SqlConnection))
            {
                SqlCommandHelper.AddSqlParameter(cmd, ParameterDirection.Input, "@name", SqlDbType.NVarChar, name);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                        {
                            config.Id = reader.GetInt32(0);
                        }

                        if (!reader.IsDBNull(1))
                        {
                            config.Name = reader.GetString(1);
                        }

                        if (!reader.IsDBNull(2))
                        {
                            config.Value = reader.GetString(2);
                        }
                    }
                }
            }

            return config;
        }          
    }
}
