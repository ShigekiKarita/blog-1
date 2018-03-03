using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hinoki.Core;

namespace Hinoki.Core.Db
{
    public class HNKBlogConfigFetcher
    {
        /// <summary>
        /// ブログの設定を取得します
        /// </summary>
        /// <param name="dbConnectionString">接続文字列</param>
        /// <returns></returns>
        static public List<BlogConfig> GetConfigs(string dbConnectionString)
        {
            List<BlogConfig> configs = new List<BlogConfig> { };
            using (HNKBlogConfigResource resource = new HNKBlogConfigResource(dbConnectionString))
            {
                configs = resource.GetBlogConfigs();
            }
            return configs;
        }

        /// <summary>
        /// 記事を更新します
        /// </summary>
        /// <param name="dbConnectionString">接続文字列</param>
        /// <param name="configs">更新後のカテゴリ</param>
        /// <returns>成功した場合に真を返す</returns>
        static public bool UpdateConfigs(string dbConnectionString, List<BlogConfig> configs)
        {
            if (configs == null || configs.Count < 1)
            {
                return false;
            }

            bool result = false;
            using (HNKBlogConfigResource resource = new HNKBlogConfigResource(dbConnectionString))
            {
                result = resource.UpdateBlogConfigs(configs);
            }

            return result;
        }
    }
}
