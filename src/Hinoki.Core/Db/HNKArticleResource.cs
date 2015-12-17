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
    /// 記事をデータベースから取得するためのクラス
    /// </summary>
    public class HNKArticleResource : DataResource
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="connectionString"></param>
        public HNKArticleResource(string connectionString)
            : base(connectionString)
        {

        } 
        #endregion

        #region パブリックメソッド
        /// <summary>
        /// 指定されたIDを持つ公開可能な記事があれば返します
        /// </summary>
        /// <param name="id">記事ID。データベースのレコードIDと同じ</param>
        public Article GetArticle(int id, PublicStatus publicStatus = PublicStatus.Public)
        {
            Article article = new Article();

            StringBuilder query = new StringBuilder();
            query.Append("SELECT articles.id, articles.title, articles.body, articles.open_date, articles.public_status, articles.update_date, categories.Id, categories.name, format, tag_id From articles");
            query.Append(" INNER JOIN categories ON articles.category_id = categories.id");
            query.Append(" WHERE articles.id = @articles_id AND articles.public_status <= @articles_public_status");
            if (publicStatus == PublicStatus.Public)
            {
                query.Append(" AND articles.open_date <= @curent_date AND (articles.close_date IS NULL OR articles.close_date > @curent_date)");
            }

            using (SqlCommand cmd = new SqlCommand(query.ToString(), this.SqlConnection))
            {
                SqlCommandHelper.AddSqlParameter(cmd, ParameterDirection.Input, "@articles_id", SqlDbType.Int, id);
                SqlCommandHelper.AddSqlParameter(cmd, ParameterDirection.Input, "@articles_public_status", SqlDbType.Int, (int)publicStatus);
                if (publicStatus == PublicStatus.Public)
                {
                    SqlCommandHelper.AddSqlParameter(cmd, ParameterDirection.Input, "@curent_date", SqlDbType.DateTime, HNKDateTime.Now("Tokyo Standard Time"));
                } 

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int i = -1;
                        if (!reader.IsDBNull(++i))
                        {
                            article.Id = reader.GetInt32(i);
                        }

                        if (!reader.IsDBNull(++i))
                        {
                            article.Title = reader.GetString(i);
                        }

                        if (!reader.IsDBNull(++i))
                        {
                            article.Body = reader.GetString(i);
                        }

                        if (!reader.IsDBNull(++i))
                        {
                            article.OpenDate = reader.GetDateTime(i);
                        }

                        if (!reader.IsDBNull(++i))
                        {
                            article.PublicStatus = (PublicStatus)reader.GetInt32(i);
                        }

                        if (!reader.IsDBNull(++i))
                        {
                            article.UpdateDate = reader.GetDateTime(i);
                        }

                        if (!reader.IsDBNull(++i))
                        {
                            article.Category.Id = reader.GetInt32(i);
                        }

                        if (!reader.IsDBNull(++i))
                        {
                            article.Category.Name = reader.GetString(i);
                        }

                        if (!reader.IsDBNull(++i))
                        {
                            article.Format = (ArticleFormat)reader.GetInt32(i);
                        }

                        if (!reader.IsDBNull(++i))
                        {
                            article.TagIds = reader.GetString(i);
                        }
                    }
                }
            }

            return article;
        }

        /// <summary>
        /// 公開可能な記事を取得します。ただし、指定された記事数のみを公開日降順で取得します
        /// </summary>
        /// <param name="pageSize"></param>
        public List<Article> GetArticles(int pageSize, int pageNumber, out int totalArticleCount, PublicStatus publicStatus = PublicStatus.Public)
        {
            totalArticleCount = 0;
            if (pageSize < 1)
            {
                return new List<Article> { };
            }

            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            StringBuilder getArticleCountQuery = new StringBuilder();
            getArticleCountQuery.Append("SELECT COUNT(DISTINCT articles.id) FROM articles");
            getArticleCountQuery.Append(" INNER JOIN categories ON articles.category_id = categories.id");
            getArticleCountQuery.Append(" WHERE articles.public_status <= @public_status");
            if (publicStatus == PublicStatus.Public)
            {
                getArticleCountQuery.Append(" AND articles.open_date <= @curent_date AND (articles.close_date IS NULL OR articles.close_date > @curent_date)");
            }
            
            using (SqlCommand cmd = new SqlCommand(getArticleCountQuery.ToString(), this.SqlConnection))
            {
                SqlCommandHelper.AddSqlParameter(cmd, ParameterDirection.Input, "@public_status", SqlDbType.Int, (int)publicStatus);
                if (publicStatus == PublicStatus.Public)
                {
                    SqlCommandHelper.AddSqlParameter(cmd, ParameterDirection.Input, "@curent_date", SqlDbType.DateTime, HNKDateTime.Now("Tokyo Standard Time"));
                }
            
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                        {
                            totalArticleCount = reader.GetInt32(0);
                        }
                    }
                }
            }

            List<Article> articles = new List<Article> { };

            StringBuilder query = new StringBuilder();
            query.Append("SELECT articles.id, articles.open_date, articles.title, articles.body, articles.update_date, articles.close_date, categories.Id, categories.name, format, articles.public_status, tag_id FROM articles");
            query.Append(" INNER JOIN categories ON articles.category_id = categories.id");
            query.Append(" WHERE articles.public_status <= @public_status");
            if (publicStatus == PublicStatus.Public)
            {
                query.Append(" AND articles.open_date <= @curent_date AND (articles.close_date IS NULL OR articles.close_date > @curent_date)");
            }
            query.Append(" ORDER BY articles.open_date DESC, articles.id DESC OFFSET (@page_number - 1) * @page_size ROWS FETCH NEXT @page_size ROWS ONLY");

            using (SqlCommand cmd = new SqlCommand(query.ToString(), this.SqlConnection))
            {
                SqlCommandHelper.AddSqlParameter(cmd, ParameterDirection.Input, "@page_size", SqlDbType.Int, pageSize);
                SqlCommandHelper.AddSqlParameter(cmd, ParameterDirection.Input, "@page_number", SqlDbType.Int, pageNumber);
                SqlCommandHelper.AddSqlParameter(cmd, ParameterDirection.Input, "@public_status", SqlDbType.Int, (int)publicStatus);
                if (publicStatus == PublicStatus.Public)
                {
                    SqlCommandHelper.AddSqlParameter(cmd, ParameterDirection.Input, "@curent_date", SqlDbType.DateTime, HNKDateTime.Now("Tokyo Standard Time"));
                }

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Article article = new Article();

                        int i = -1;
                        if (!reader.IsDBNull(++i))
                        {
                            article.Id = reader.GetInt32(i);
                        }

                        if (!reader.IsDBNull(++i))
                        {
                            article.OpenDate = reader.GetDateTime(i);
                        }

                        if (!reader.IsDBNull(++i))
                        {
                            article.Title = reader.GetString(i);
                        }

                        if (!reader.IsDBNull(++i))
                        {
                            article.Body = reader.GetString(i);
                        }

                        if (!reader.IsDBNull(++i))
                        {
                            article.UpdateDate = reader.GetDateTime(i);
                        }

                        if (!reader.IsDBNull(++i))
                        {
                            article.CloseDate = reader.GetDateTime(i);
                        }

                        if (!reader.IsDBNull(++i))
                        {
                            article.Category.Id = reader.GetInt32(i);
                        }

                        if (!reader.IsDBNull(++i))
                        {
                            article.Category.Name = reader.GetString(i);
                        }

                        if (!reader.IsDBNull(++i))
                        {
                            article.Format = (ArticleFormat)reader.GetInt32(i);
                        }

                        if (!reader.IsDBNull(++i))
                        {
                            article.PublicStatus = (PublicStatus)reader.GetInt32(i);
                        }

                        if (!reader.IsDBNull(++i))
                        {
                            article.TagIds = reader.GetString(i);
                        }

                        articles.Add(article);
                    }
                }
            }

            return articles;
        }

        /// <summary>
        /// 記事を検索します
        /// </summary>
        /// <param name="pageSize"></param>
        public List<Article> GetArticles(int pageSize, int pageNumber, out int totalArticleCount, List<string> keyword, PublicStatus publicStatus = PublicStatus.Public)
        {
            totalArticleCount = 0;
            if (pageSize < 1)
            {
                return new List<Article> { };
            }

            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            HNKArticleSqlQuery countQuery = new HNKArticleSqlQuery();
            using (SqlCommand cmd = new SqlCommand(countQuery.HitCount(HNKDateTime.Now("Tokyo Standard Time"), keyword, publicStatus), this.SqlConnection))
            {
                if (countQuery.Parameters.Count > 0)
                {
                    cmd.Parameters.AddRange(countQuery.Parameters.ToArray());
                }

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                        {
                            totalArticleCount = reader.GetInt32(0);
                        }
                    }
                }
            }

            List<Article> articles = new List<Article> { };

            HNKArticleSqlQuery searchQuery = new HNKArticleSqlQuery();
            using (SqlCommand cmd = new SqlCommand(searchQuery.Search(HNKDateTime.Now("Tokyo Standard Time"), keyword, pageSize, pageNumber, publicStatus), this.SqlConnection))
            {
                if (searchQuery.Parameters.Count > 0)
                {
                    cmd.Parameters.AddRange(searchQuery.Parameters.ToArray());
                }

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Article article = new Article();

                        int i = -1;
                        if (!reader.IsDBNull(++i))
                        {
                            article.Id = reader.GetInt32(i);
                        }

                        if (!reader.IsDBNull(++i))
                        {
                            article.OpenDate = reader.GetDateTime(i);
                        }

                        if (!reader.IsDBNull(++i))
                        {
                            article.Title = reader.GetString(i);
                        }

                        if (!reader.IsDBNull(++i))
                        {
                            article.Body = reader.GetString(i);
                        }

                        if (!reader.IsDBNull(++i))
                        {
                            article.UpdateDate = reader.GetDateTime(i);
                        }

                        if (!reader.IsDBNull(++i))
                        {
                            article.CloseDate = reader.GetDateTime(i);
                        }

                        if (!reader.IsDBNull(++i))
                        {
                            article.Category.Id = reader.GetInt32(i);
                        }

                        if (!reader.IsDBNull(++i))
                        {
                            article.Category.Name = reader.GetString(i);
                        }

                        if (!reader.IsDBNull(++i))
                        {
                            article.Format = (ArticleFormat)reader.GetInt32(i);
                        }

                        if (!reader.IsDBNull(++i))
                        {
                            article.PublicStatus = (PublicStatus)reader.GetInt32(i);
                        }

                        articles.Add(article);
                    }
                }
            }

            return articles;
        }

        /// <summary>
        /// 公開可能な記事を取得します。ただし、指定されたカテゴリIDに属している記事のみを公開日降順で取得します
        /// </summary>
        /// <param name="count"></param>
        public List<Article> GetPublicArticlesByCategory(int categoryId, int pageSize, int pageNumber, out int totalArticleCount)
        {
            totalArticleCount = 0;
            if (categoryId < 0)
            {
                return new List<Article> { };
            }

            StringBuilder getArticleCountQuery = new StringBuilder();
            getArticleCountQuery.Append("SELECT COUNT(articles.id) AS article_count FROM articles");
            getArticleCountQuery.Append(" INNER JOIN categories ON articles.category_id = categories.id");
            getArticleCountQuery.Append(" WHERE articles.category_id = @category_id AND articles.public_status = @public_status AND categories.public_status = @public_status");
            getArticleCountQuery.Append(" AND open_date <= @curent_date AND (close_date IS NULL OR close_date > @curent_date)");
            using (SqlCommand cmd = new SqlCommand(getArticleCountQuery.ToString(), this.SqlConnection))
            {
                SqlCommandHelper.AddSqlParameter(cmd, ParameterDirection.Input, "@category_id", SqlDbType.Int, categoryId);
                SqlCommandHelper.AddSqlParameter(cmd, ParameterDirection.Input, "@public_status", SqlDbType.Int, (int)PublicStatus.Public);
                SqlCommandHelper.AddSqlParameter(cmd, ParameterDirection.Input, "@curent_date", SqlDbType.DateTime, HNKDateTime.Now("Tokyo Standard Time"));

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                        {
                            totalArticleCount = reader.GetInt32(0);
                        }
                    }
                }
            }

            StringBuilder query = new StringBuilder();
            query.Append("SELECT articles.id, title, body, open_date, format FROM articles");
            query.Append(" INNER JOIN categories ON articles.category_id = categories.id");
            query.Append(" WHERE articles.category_id = @category_id AND articles.public_status = @public_status AND categories.public_status = @public_status");
            query.Append(" AND open_date <= @curent_date AND (close_date IS NULL OR close_date > @curent_date)");
            query.Append(" ORDER BY open_date DESC, id DESC OFFSET (@page_number - 1) * @page_size ROWS FETCH NEXT @page_size ROWS ONLY");

            List<Article> articles = new List<Article> { };
            using (SqlCommand cmd = new SqlCommand(query.ToString(), this.SqlConnection))
            {
                SqlCommandHelper.AddSqlParameter(cmd, ParameterDirection.Input, "@page_size", SqlDbType.Int, pageSize);
                SqlCommandHelper.AddSqlParameter(cmd, ParameterDirection.Input, "@page_number", SqlDbType.Int, pageNumber);
                SqlCommandHelper.AddSqlParameter(cmd, ParameterDirection.Input, "@category_id", SqlDbType.Int, categoryId);
                SqlCommandHelper.AddSqlParameter(cmd, ParameterDirection.Input, "@public_status", SqlDbType.Int, (int)PublicStatus.Public);
                SqlCommandHelper.AddSqlParameter(cmd, ParameterDirection.Input, "@curent_date", SqlDbType.DateTime, HNKDateTime.Now("Tokyo Standard Time"));

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int i = -1;

                        Article article = new Article();
                        if (!reader.IsDBNull(++i))
                        {
                            article.Id = reader.GetInt32(i);
                            article.PublicStatus = PublicStatus.Public;
                        }

                        if (!reader.IsDBNull(++i))
                        {
                            article.Title = reader.GetString(i);
                        }

                        if (!reader.IsDBNull(++i))
                        {
                            article.Body = reader.GetString(i);
                        }

                        if (!reader.IsDBNull(++i))
                        {
                            article.OpenDate = reader.GetDateTime(i);
                        }

                        if (!reader.IsDBNull(++i))
                        {
                            article.Format = (ArticleFormat)reader.GetInt32(i);
                        }

                        if (article.Id > 0)
                        {
                            articles.Add(article);
                        }
                    }
                }
            }

            return articles;
        }

        /// <summary>
        /// 公開可能な記事を取得します。ただし、指定された日付範囲のみを公開日降順で取得します
        /// </summary>
        /// <param name="count"></param>
        public List<Article> GetPublicArticlesByDate(int year, int month, int pageSize, int pageNumber, out int totalArticleCount)
        {
            totalArticleCount = 0;
            if (year < 0)
            {
                return new List<Article> { };
            }

            if (month < 1 || month > 12)
            {
                return new List<Article> { };
            }

            int startDay = 1;
            int endDay = DateTime.DaysInMonth(year, month);

            DateTime startDate = new DateTime(year, month, startDay);
            DateTime endDate = new DateTime(year, month, endDay, 23, 59, 59);

            StringBuilder getArticleCountQuery = new StringBuilder();
            getArticleCountQuery.Append("SELECT COUNT(DISTINCT id) AS article_count From articles");
            getArticleCountQuery.Append(" WHERE public_status = @public_status AND (open_date >= @date_in AND open_date <= @date_out) AND (close_date IS NULL OR close_date < @date_out)");
            using (SqlCommand cmd = new SqlCommand(getArticleCountQuery.ToString(), this.SqlConnection))
            {
                SqlCommandHelper.AddSqlParameter(cmd, ParameterDirection.Input, "@public_status", SqlDbType.Int, (int)PublicStatus.Public);
                SqlCommandHelper.AddSqlParameter(cmd, ParameterDirection.Input, "@date_in", SqlDbType.DateTime, startDate);
                SqlCommandHelper.AddSqlParameter(cmd, ParameterDirection.Input, "@date_out", SqlDbType.DateTime, endDate);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                        {
                            totalArticleCount = reader.GetInt32(0);
                        }
                    }
                }
            }

            StringBuilder query = new StringBuilder();
            query.Append("SELECT id, title, body, open_date, format From articles");
            query.Append(" WHERE public_status = @public_status AND (open_date >= @date_in AND open_date < @date_out) AND (close_date IS NULL OR close_date < @date_out)");
            query.Append(" ORDER BY open_date DESC, id DESC OFFSET (@page_number - 1) * @page_size ROWS FETCH NEXT @page_size ROWS ONLY");

            List<Article> articles = new List<Article> { };
            using (SqlCommand cmd = new SqlCommand(query.ToString(), this.SqlConnection))
            {
                SqlCommandHelper.AddSqlParameter(cmd, ParameterDirection.Input, "@page_size", SqlDbType.Int, pageSize);
                SqlCommandHelper.AddSqlParameter(cmd, ParameterDirection.Input, "@page_number", SqlDbType.Int, pageNumber);
                SqlCommandHelper.AddSqlParameter(cmd, ParameterDirection.Input, "@public_status", SqlDbType.Int, (int)PublicStatus.Public);
                SqlCommandHelper.AddSqlParameter(cmd, ParameterDirection.Input, "@date_in", SqlDbType.DateTime, startDate);
                SqlCommandHelper.AddSqlParameter(cmd, ParameterDirection.Input, "@date_out", SqlDbType.DateTime, endDate);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int i = -1;

                        Article article = new Article();
                        if (!reader.IsDBNull(++i))
                        {
                            article.Id = reader.GetInt32(i);
                            article.PublicStatus = PublicStatus.Public;
                        }

                        if (!reader.IsDBNull(++i))
                        {
                            article.Title = reader.GetString(i);
                        }

                        if (!reader.IsDBNull(++i))
                        {
                            article.Body = reader.GetString(i);
                        }

                        if (!reader.IsDBNull(++i))
                        {
                            article.OpenDate = reader.GetDateTime(i);
                        }

                        if (article.Id > 0)
                        {
                            articles.Add(article);
                        }
                    }
                }
            }

            return articles;
        }

        /// <summary>
        /// 公開している記事の数を月単位で取得します
        /// </summary>
        /// <returns></returns>
        public List<ArticleCount> GetPublicArticleCount()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT SUBSTRING(CONVERT(VARCHAR, open_date, 111),1,7) AS 年月, COUNT(*) AS 計数 FROM articles");
            query.Append(" WHERE public_status = @public_status AND open_date <= @curent_date AND (close_date IS NULL OR close_date > @curent_date)");
            query.Append(" GROUP BY SUBSTRING(CONVERT(VARCHAR, open_date, 111),1,7)");
            query.Append(" ORDER BY 年月 DESC");

            List<ArticleCount> articles = new List<ArticleCount> { };
            using (SqlCommand cmd = new SqlCommand(query.ToString(), this.SqlConnection))
            {
                SqlCommandHelper.AddSqlParameter(cmd, ParameterDirection.Input, "@public_status", SqlDbType.Int, (int)PublicStatus.Public);
                SqlCommandHelper.AddSqlParameter(cmd, ParameterDirection.Input, "@curent_date", SqlDbType.DateTime, HNKDateTime.Now("Tokyo Standard Time"));

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ArticleCount obj = new ArticleCount();
                        if (!reader.IsDBNull(0))
                        {
                            string dateTimeString = reader.GetString(0);
                            DateTime dateTime = DateTime.Today;
                            bool result = DateTime.TryParse(dateTimeString, out dateTime);
                            if (!result)
                            {
                                continue;
                            }
                            obj.DateTime = dateTime;
                        }
                        if (!reader.IsDBNull(1))
                        {
                            obj.Count = reader.GetInt32(1);
                        }
                        if (obj.Count > 0)
                        {
                            articles.Add(obj);
                        }
                    }
                }
            }

            return articles;
        }

        /// <summary>
        /// 記事を追加します
        /// </summary>
        /// <param name="article">内容</param>
        /// <returns></returns>
        public bool InsertArticle(Article article)
        {
            bool isSuccess = false;

            StringBuilder query = new StringBuilder();
            query.Append("INSERT INTO articles (open_date, close_date, title, body, public_status, format, category_id)");
            query.Append(" VALUES (@open_date, @close_date, @title, @body, @public_status, @format, @category_id)");
            query.Append(" ; SELECT id FROM articles WHERE id = SCOPE_IDENTITY()");

            using (SqlCommand cmd = new SqlCommand(query.ToString(), this.SqlConnection))
            {
                SqlCommandHelper.AddSqlParameter(cmd, ParameterDirection.Input, "@open_date", SqlDbType.DateTime2, article.OpenDate);
                SqlCommandHelper.AddSqlParameter(cmd, ParameterDirection.Input, "@close_date", SqlDbType.DateTime2, article.CloseDate);
                SqlCommandHelper.AddSqlParameter(cmd, ParameterDirection.Input, "@title", SqlDbType.VarChar, article.Title);
                SqlCommandHelper.AddSqlParameter(cmd, ParameterDirection.Input, "@body", SqlDbType.VarChar, article.Body);
                SqlCommandHelper.AddSqlParameter(cmd, ParameterDirection.Input, "@public_status", SqlDbType.Int, (int)article.PublicStatus);
                SqlCommandHelper.AddSqlParameter(cmd, ParameterDirection.Input, "@format", SqlDbType.Int, (int)article.Format);
                SqlCommandHelper.AddSqlParameter(cmd, ParameterDirection.Input, "@category_id", SqlDbType.Int, (int)article.Category.Id);

                int recordId = 0;
                try
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // レコード番号を取得
                            if (!reader.IsDBNull(0)) recordId = reader.GetInt32(0);
                        }
                    }
                }
                catch (Exception e)
                {

                }

                if (recordId > 0)
                {
                    isSuccess = true;
                }
            }

            return isSuccess;
        }

        /// <summary>
        /// 記事を更新します
        /// </summary>
        /// <param name="article">更新内容</param>
        /// <returns></returns>
        public bool UpdateArticle(Article article)
        {
            bool isSuccess = false;

            StringBuilder query = new StringBuilder();
            query.Append("UPDATE articles SET update_date = (dateadd(hour,(9),getutcdate())), open_date = @open_date, close_date = @close_date, title = @title, body = @body, public_status = @public_status, format = @format, category_id = @category_id");
            query.Append(" WHERE id = @id");

            using (SqlCommand cmd = new SqlCommand(query.ToString(), this.SqlConnection))
            {
                SqlCommandHelper.AddSqlParameter(cmd, ParameterDirection.Input, "@id", SqlDbType.Int, article.Id);
                SqlCommandHelper.AddSqlParameter(cmd, ParameterDirection.Input, "@open_date", SqlDbType.DateTime2, article.OpenDate);
                SqlCommandHelper.AddSqlParameter(cmd, ParameterDirection.Input, "@close_date", SqlDbType.DateTime2, article.CloseDate);
                SqlCommandHelper.AddSqlParameter(cmd, ParameterDirection.Input, "@title", SqlDbType.VarChar, article.Title);
                SqlCommandHelper.AddSqlParameter(cmd, ParameterDirection.Input, "@body", SqlDbType.VarChar, article.BodyRaw);
                SqlCommandHelper.AddSqlParameter(cmd, ParameterDirection.Input, "@public_status", SqlDbType.Int, (int)article.PublicStatus);
                SqlCommandHelper.AddSqlParameter(cmd, ParameterDirection.Input, "@format", SqlDbType.Int, (int)article.Format);
                SqlCommandHelper.AddSqlParameter(cmd, ParameterDirection.Input, "@category_id", SqlDbType.Int, (int)article.Category.Id);

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
