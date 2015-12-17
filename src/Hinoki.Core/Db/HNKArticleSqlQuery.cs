using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Hinoki.Core.Db
{
    public class HNKArticleSqlQuery : HNKSql
    {
        public HNKArticleSqlQuery() 
            : base()
        {

        }

        public string Count(DateTime dateTime, PublicStatus publicStatus)
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT COUNT(DISTINCT articles.id) FROM articles");
            query.Append(" INNER JOIN categories ON articles.category_id = categories.id");
            query.Append(" WHERE articles.public_status <= @public_status");
            this.AddSqlParameter(ParameterDirection.Input, "@public_status", SqlDbType.Int, (int)publicStatus);
            if (publicStatus == PublicStatus.Public)
            {
                query.Append(" AND articles.open_date <= @curent_date AND (articles.close_date IS NULL OR articles.close_date > @curent_date)");
                this.AddSqlParameter(ParameterDirection.Input, "@curent_date", SqlDbType.DateTime, dateTime);
            }

            return query.ToString();
        }

        public string HitCount(DateTime dateTime, List<string> keywords, PublicStatus publicStatus)
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT COUNT(DISTINCT articles.id) FROM articles");
            query.Append(" INNER JOIN categories ON articles.category_id = categories.id");
            query.Append(" WHERE articles.public_status <= @public_status");
            this.AddSqlParameter(ParameterDirection.Input, "@public_status", SqlDbType.Int, (int)publicStatus);
            if (publicStatus == PublicStatus.Public)
            {
                query.Append(" AND articles.open_date <= @curent_date AND (articles.close_date IS NULL OR articles.close_date > @curent_date)");
                this.AddSqlParameter(ParameterDirection.Input, "@curent_date", SqlDbType.DateTime, dateTime);
            }
            if (keywords.Count > 0)
            {
                List<string> fields = new List<string> { "articles.title", "articles.body", "categories.name" };
                query.AppendFormat(" AND {0}", this.CreateWhereKeyword(fields, keywords));
            }
            return query.ToString();
        }

        public string Search(DateTime dateTime, List<string> keywords, int pageSize, int pageNumber, PublicStatus publicStatus)
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT articles.id, articles.open_date, articles.title, articles.body, articles.update_date, articles.close_date, categories.Id, categories.name, format, articles.public_status FROM articles");
            query.Append(" INNER JOIN categories ON articles.category_id = categories.id");
            query.Append(" WHERE articles.public_status <= @public_status");
            this.AddSqlParameter(ParameterDirection.Input, "@public_status", SqlDbType.Int, (int)publicStatus);
            if (publicStatus == PublicStatus.Public)
            {
                query.Append(" AND articles.open_date <= @curent_date AND (articles.close_date IS NULL OR articles.close_date > @curent_date)");
                this.AddSqlParameter(ParameterDirection.Input, "@curent_date", SqlDbType.DateTime, dateTime);
            }
            if (keywords.Count > 0)
            {
                List<string> fields = new List<string> { "articles.title", "articles.body", "categories.name" };
                query.AppendFormat(" AND {0}", this.CreateWhereKeyword(fields, keywords));
            }
            query.Append(" ORDER BY articles.open_date DESC, articles.id DESC OFFSET (@page_number - 1) * @page_size ROWS FETCH NEXT @page_size ROWS ONLY");
            this.AddSqlParameter(ParameterDirection.Input, "@page_size", SqlDbType.Int, pageSize);
            this.AddSqlParameter(ParameterDirection.Input, "@page_number", SqlDbType.Int, pageNumber);

            return query.ToString();
        }

        #region プライベートメソッド
        private string CreateWhereKeyword(List<string> fields, List<string> words)
        {
            List<SqlParameter> parameters = new List<SqlParameter> { };
            string[] whereKeyword = new string[words.Count];

            for (int i = 0; i < words.Count; i++)
            {
                string key = string.Format("@word{0}", i);
                this.AddSqlParameter(ParameterDirection.Input, key, SqlDbType.NVarChar, "%" + words[i].Trim() + "%");

                string[] likePhrase = new string[fields.Count];
                for (int j = 0; j < fields.Count; j++)
                {
                    likePhrase[j] = string.Format("({0} Like {1})", fields[j], key);
//                    likePhrase[j] = string.Format("({0} Like {1} COLLATE {2})", fields[j], key, this.GetCollate());
                }

                whereKeyword[i] = "(" + string.Join(" OR ", likePhrase) + ")";
            }

            return "(" + string.Join(" AND ", whereKeyword) + ")";
        }
        #endregion
    }
}
