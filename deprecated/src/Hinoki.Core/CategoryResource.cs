using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Hinoki.Core;
using Hinoki.Core.Db;

namespace Hinoki.Core
{
    /// <summary>
    /// カテゴリをデータベースから取得するためのクラス
    /// </summary>
    public class CategoryResource : DataResource
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="connectionString"></param>
        public CategoryResource(string connectionString)
            : base(connectionString)
        {

        } 
        #endregion

        #region パブリックメソッド
        /// <summary>
        /// 指定されたIDを持つ公開可能なカテゴリを返します
        /// </summary>
        /// <param name="id">カテゴリID。データベースのレコードIDと同じ</param>
        /// <param name="publicStatus">取得する範囲。規定値は公開中のカテゴリのみを取得する。非公開のカテゴリも含めたい場合はPublicStatus.Privateを指定する</param>
        public Category GetCategory(int id, PublicStatus publicStatus = PublicStatus.Public)
        {
            Category category = new Category();

            StringBuilder query = new StringBuilder();
            query.Append("SELECT id, name, name_yomi From categories");
            query.Append(" WHERE id = @id AND public_status <= @public_status");

            using (SqlCommand cmd = new SqlCommand(query.ToString(), this.SqlConnection))
            {
                SqlCommandHelper.AddSqlParameter(cmd, ParameterDirection.Input, "@id", SqlDbType.Int, id);
                SqlCommandHelper.AddSqlParameter(cmd, ParameterDirection.Input, "@public_status", SqlDbType.Int, (int)publicStatus);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int i = -1;
                        if (!reader.IsDBNull(++i))
                        {
                            category.Id = reader.GetInt32(i);
                            category.PublicStatus = PublicStatus.Public;
                        }

                        if (!reader.IsDBNull(++i))
                        {
                            category.Name = reader.GetString(i);
                        }

                        if (!reader.IsDBNull(++i))
                        {
                            category.NameYomi = reader.GetString(i);
                        }
                    }
                }
            }

            return category;
        }

        /// <summary>
        /// カテゴリを取得します
        /// </summary>
        /// <param name="publicStatus">取得する範囲。規定値は公開中のカテゴリのみを取得する。非公開のカテゴリも含めたい場合はPublicStatus.Privateを指定する</param>
        public List<Category> GetCategories(PublicStatus publicStatus = PublicStatus.Public)
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT categories.id, categories.name AS カテゴリ, COUNT(articles.category_id) AS 件数, categories.update_date FROM articles");
            query.Append(" RIGHT JOIN categories ON articles.category_id = categories.id");
            query.Append(" WHERE");
            if (publicStatus == PublicStatus.Public)
            {
                query.Append(" articles.public_status <= @public_status");
                query.Append(" AND categories.public_status <= @public_status");
                query.Append(" AND articles.open_date <= @current_datetime");
                query.Append(" AND (articles.close_date IS NULL OR articles.close_date > @current_datetime)");
            }
            else
            {
                query.Append(" categories.public_status <= @public_status");
            }
            query.Append(" GROUP BY categories.id, categories.name, categories.name_yomi, categories.update_date");
            query.Append(" ORDER BY name_yomi");

            List<Category> categories = new List<Category> { };
            using (SqlCommand cmd = new SqlCommand(query.ToString(), this.SqlConnection))
            {
                SqlCommandHelper.AddSqlParameter(cmd, ParameterDirection.Input, "@public_status", SqlDbType.Int, (int)publicStatus);
                SqlCommandHelper.AddSqlParameter(cmd, ParameterDirection.Input, "@current_datetime", SqlDbType.DateTime, HNKDateTime.Now("Tokyo Standard Time"));

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Category category = new Category();

                        int i = -1;
                        if (!reader.IsDBNull(++i))
                        {
                            category.Id = reader.GetInt32(i);
                            category.PublicStatus = PublicStatus.Public;
                        }

                        if (!reader.IsDBNull(++i))
                        {
                            category.Name = reader.GetString(i);
                        }

                        if (!reader.IsDBNull(++i))
                        {
                            category.Count = reader.GetInt32(i);
                        }

                        if (!reader.IsDBNull(++i))
                        {
                            category.UpdateDate = reader.GetDateTime(i);
                        }

                        if (category.Id > 0)
                        {
                            categories.Add(category);
                        }
                    }
                }
            }

            return categories;
        }

        /// <summary>
        /// カテゴリを更新します
        /// </summary>
        /// <param name="category">更新後のカテゴリ</param>
        /// <returns></returns>
        public bool UpdateCategory(Category category)
        {
            bool isSuccess = false;

            StringBuilder query = new StringBuilder();
            query.Append("UPDATE categories SET update_date = (dateadd(hour,(9),getutcdate())), name = @name, name_yomi = @name_yomi, public_status = @public_status");
            query.Append(" WHERE id = @id");

            using (SqlCommand cmd = new SqlCommand(query.ToString(), this.SqlConnection))
            {
                SqlCommandHelper.AddSqlParameter(cmd, ParameterDirection.Input, "@id", SqlDbType.Int, category.Id);
                SqlCommandHelper.AddSqlParameter(cmd, ParameterDirection.Input, "@name", SqlDbType.VarChar, category.Name);
                SqlCommandHelper.AddSqlParameter(cmd, ParameterDirection.Input, "@name_yomi", SqlDbType.VarChar, category.NameYomi);
                SqlCommandHelper.AddSqlParameter(cmd, ParameterDirection.Input, "@public_status", SqlDbType.Int, (int)category.PublicStatus);

                int affectedRowNumber = 0;
                try
                {
                    // ExecuteNonQueryはエラー発生時に-1を返す
                    affectedRowNumber = cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                }
                
                if (affectedRowNumber > 0)
                {
                    isSuccess = true;
                }
            }

            return isSuccess;
        }
        #endregion
    }
}
