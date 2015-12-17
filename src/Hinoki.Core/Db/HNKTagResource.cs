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
    /// タグをデータベースから取得するためのクラス
    /// </summary>
    public class HNKTagResource : DataResource
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="connectionString"></param>
        public HNKTagResource(string connectionString)
            : base(connectionString)
        {

        } 
        #endregion

        #region パブリックメソッド
        /// <summary>
        /// 指定されたIDを持つタグを返します
        /// </summary>
        /// <param name="id">タグID。データベースのレコードIDと同じ</param>
        public HNKTag GetTag(int id)
        {
            HNKTag tag = new HNKTag();

            StringBuilder query = new StringBuilder();
            query.Append("SELECT id, name, name_yomi From tags");
            query.Append(" WHERE id = @id");

            using (SqlCommand cmd = new SqlCommand(query.ToString(), this.SqlConnection))
            {
                SqlCommandHelper.AddSqlParameter(cmd, ParameterDirection.Input, "@id", SqlDbType.Int, id);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int i = -1;

                        if (!reader.IsDBNull(++i))
                        {
                            tag.Id = reader.GetInt32(i);
                        }

                        if (!reader.IsDBNull(++i))
                        {
                            tag.Name = reader.GetString(i);
                        }

                        if (!reader.IsDBNull(++i))
                        {
                            tag.NameYomi = reader.GetString(i);
                        }
                    }
                }
            }

            return tag;
        }

        /// <summary>
        /// 指定されたIDを持つタグを返します
        /// </summary>
        /// <param name="tagIds">タグID。データベースのレコードIDと同じ</param>
        public List<HNKTag> GetTags(List<int> tagIds)
        {
            List<HNKTag> tags = new List<HNKTag> { };

            HNKTagSql sql = HNKTagSqlFactory.GetTags(tagIds);
            using (SqlCommand cmd = new SqlCommand(sql.Query.ToString(), this.SqlConnection))
            {
                cmd.Parameters.AddRange(sql.Parameters.ToArray());
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int i = -1;

                        HNKTag tag = new HNKTag();
                        if (!reader.IsDBNull(++i))
                        {
                            tag.Id = reader.GetInt32(i);
                        }

                        if (!reader.IsDBNull(++i))
                        {
                            tag.Name = reader.GetString(i);
                        }

                        if (!reader.IsDBNull(++i))
                        {
                            tag.NameYomi = reader.GetString(i);
                        }

                        if (tag.Id > 0)
                        {
                            tags.Add(tag);
                        }
                    }
                }
            }

            return tags;
        }


        /// <summary>
        /// タグを取得します
        /// </summary>
        public List<HNKTag> GetAll()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT tags.id, tags.name, tags.update_date FROM tags");
            query.Append(" ORDER BY name_yomi");

            List<HNKTag> tags = new List<HNKTag> { };
            using (SqlCommand cmd = new SqlCommand(query.ToString(), this.SqlConnection))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int i = -1;

                        HNKTag tag = new HNKTag();                        
                        if (!reader.IsDBNull(++i))
                        {
                            tag.Id = reader.GetInt32(i);
                        }

                        if (!reader.IsDBNull(++i))
                        {
                            tag.Name = reader.GetString(i);
                        }

                        if (!reader.IsDBNull(++i))
                        {
                            tag.UpdateDate = reader.GetDateTime(i);
                        }

                        if (tag.Id > 0)
                        {
                            tags.Add(tag);
                        }
                    }
                }
            }

            return tags;
        }
        #endregion
    }
}
