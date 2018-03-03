using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Hinoki.Core.Db;

namespace Hinoki.Core
{
    /// <summary>
    /// タグをデータベースから取得するクラス
    /// </summary>
    public class HNKTagFetcher
    {
        /// <summary>
        /// タグを取得します
        /// </summary>
        /// <param name="dbConnectionString">接続文字列</param>
        /// <returns>タグのリスト</returns>
        static public List<HNKTag> GetTags(string dbConnectionString)
        {
            List<HNKTag> tags = new List<HNKTag> { };
            using (HNKTagResource resource = new HNKTagResource(dbConnectionString))
            {
                tags = resource.GetAll();
            }
            return tags;
        }

        /// <summary>
        /// 指定されたタグを取得します
        /// </summary>
        /// <param name="dbConnectionString">接続文字列</param>
        /// <param name="id">タグID</param>
        /// <returns>タグ</returns>
        static public HNKTag GetCategory(string dbConnectionString, int id)
        {
            HNKTag tag = null;
            using (HNKTagResource resource = new HNKTagResource(dbConnectionString))
            {
                tag = resource.GetTag(id);
            }
            return tag;
        }
    }
}
