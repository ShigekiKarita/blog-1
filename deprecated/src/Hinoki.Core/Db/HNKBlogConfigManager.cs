using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hinoki.Core.Db
{
    public class HNKBlogConfigManager
    {
        #region メンバ変数
        private BlogConfig _enableArticleFeed = new BlogConfig();
        private BlogConfig _enableArticleComment = new BlogConfig();
        private BlogConfig _enableDisqus = new BlogConfig();
        private BlogConfig _blogTitle;
        private BlogConfig _blogDescription;
        private BlogConfig _googleAnalytics = new BlogConfig();
        private static string KEY_ENABLE_ARTICLE_FEED = "enable_article_feed";
        private static string KEY_ENABLE_ARTICLE_COMMENT = "enable_comment";
        private static string KEY_ENABLE_DISQUS = "enable_disqus";
        private static string KEY_BLOG_TITLE = "blog_title";
        private static string KEY_BLOG_DESCRIPTION = "blog_description";
        private static string KEY_GOOGLE_ANALYTICS = "google_analytics"; 
        #endregion

        #region プロパティ
        /// <summary>
        /// ブログのタイトル
        /// </summary>
        public BlogConfig BlogTitle
        {
            get { return _blogTitle; }
            set { _blogTitle = value; }
        }

        /// <summary>
        /// ブログの解説文
        /// </summary>
        public BlogConfig BlogDescription
        {
            get { return _blogDescription; }
            set { _blogDescription = value; }
        }

        /// <summary>
        /// Google Analytics
        /// </summary>
        public BlogConfig GoogleAnalytics
        {
            get { return _googleAnalytics; }
            set { _googleAnalytics = value; }
        } 
        #endregion
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public HNKBlogConfigManager()
        {

        }

        /// <summary>
        /// コンストラクタ。ブログの設定値をデータベースから取得します
        /// </summary>
        /// <param name="dbConnectionString"></param>
        public HNKBlogConfigManager(string dbConnectionString)
        {
            List<BlogConfig> configs = HNKBlogConfigFetcher.GetConfigs(dbConnectionString);
            if (configs.Count > 0)
            {
                List<BlogConfig> enableArticleFeed = configs.Where(i => i.Name == KEY_ENABLE_ARTICLE_FEED).ToList();
                if (enableArticleFeed.Count > 0) 
                {
                    _enableArticleFeed = enableArticleFeed[0];
                }

                List<BlogConfig> enableArticleComment = configs.Where(i => i.Name == KEY_ENABLE_ARTICLE_COMMENT).ToList();
                if (enableArticleFeed.Count > 0)
                {
                    _enableArticleComment = enableArticleComment[0];
                }

                List<BlogConfig> enableDisqus = configs.Where(i => i.Name == KEY_ENABLE_DISQUS).ToList();
                if (enableArticleFeed.Count > 0)
                {
                    _enableDisqus = enableDisqus[0];
                }

                List<BlogConfig> titleList = configs.Where(i => i.Name == KEY_BLOG_TITLE).ToList();
                if (enableArticleFeed.Count > 0)
                {
                    this.BlogTitle = titleList[0];
                }

                List<BlogConfig> feedStatusList = configs.Where(i => i.Name == KEY_BLOG_DESCRIPTION).ToList();
                if (enableArticleFeed.Count > 0)
                {
                    this.BlogDescription = feedStatusList[0];
                }

                List<BlogConfig> googleAnalytics = configs.Where(i => i.Name == KEY_GOOGLE_ANALYTICS).ToList();
                if (googleAnalytics.Count > 0)
                {
                    this.GoogleAnalytics = googleAnalytics[0];
                }
            }
        }

        #region パブリックメソッド
        /// <summary>
        /// RSSの出力が許可されている場合に真を返します
        /// </summary>
        /// <returns></returns>
        public bool IsEnabledOutputRssFeed()
        {
            int result = 0;
            int.TryParse(_enableArticleFeed.Value, out result);
            return (result > 0 ? true : false);
        }

        /// <summary>
        /// コメント欄の表示が許可されている場合に真を返します
        /// </summary>
        /// <returns></returns>
        public bool IsEnabledComment()
        {
            int result = 0;
            int.TryParse(_enableArticleComment.Value, out result);
            return (result > 0 ? true : false);
        }

        /// <summary>
        /// DISQUSの使用が許可されている場合に真を返します
        /// </summary>
        /// <returns></returns>
        public bool IsEnabledDisqus()
        {
            int result = 0;
            int.TryParse(_enableDisqus.Value, out result);
            return (result > 0 ? true : false);
        } 
        #endregion
    }
}
