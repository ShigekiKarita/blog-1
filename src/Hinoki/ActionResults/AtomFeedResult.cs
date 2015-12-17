using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.ServiceModel.Syndication;
using Hinoki.Core;
using Hinoki.Models;

namespace Hinoki.ActionResults
{
    /// <summary>
    /// Atom形式のフィードを返します
    /// </summary>
    public class AtomFeedResult : FeedBaseResult
    {
        #region コンストラクタ
        /// <summary>
        /// FileResultのコンストラクタをオーバーライド
        /// </summary>
        /// <param name="feedItems">RSSで配信したいアイテム</param>
        public AtomFeedResult(Uri blogBaseUrl, string blogTitle, string blogDescription, List<Article> articles)
            : base("application/atom+xml")
        {
            if (blogBaseUrl == null)
            {
                throw new ArgumentNullException("blogUrl");
            }

            if (string.IsNullOrEmpty(blogTitle) || string.IsNullOrWhiteSpace(blogTitle))
            {
                blogTitle = string.Empty;
            }

            if (string.IsNullOrEmpty(blogDescription) || string.IsNullOrWhiteSpace(blogDescription))
            {
                blogTitle = string.Empty;
            }

            List<SyndicationItem> syndicationItems = new List<SyndicationItem>();
            foreach (Article article in articles)
            {
                SyndicationItem item = new SyndicationItem();

                // 記事のパーマネントリンク生成処理
                Uri articleUrl = null;
                if (Uri.TryCreate(blogBaseUrl, string.Format("/article/{0}", article.Id), out articleUrl))
                {
                    item.AddPermalink(articleUrl);
                }
                else
                {
                    // URIが生成できない場合は一意性を保てないためスキップする
                    continue;
                }
                
                item.Title = new TextSyndicationContent(article.Title);
                item.Content = SyndicationContent.CreatePlaintextContent(article.Body);
                item.PublishDate = article.OpenDate;
                item.LastUpdatedTime = article.UpdateDate;
                // item.Categories.Add(new SyndicationCategory("category name"));           

                syndicationItems.Add(item);
            }

            _feed = new SyndicationFeed(blogTitle, blogDescription, HttpContext.Current.Request.Url, syndicationItems);
            _feed.Language = "ja-JP";
            _feed.Generator = AppSettings.GetBlogSystemName();
        }
        #endregion

        /// <summary>
        /// AtomフィードをXMLに書き出します
        /// </summary>
        /// <param name="response"></param>
        protected override void WriteFile(HttpResponseBase response)
        {
            response.ContentEncoding = System.Text.Encoding.UTF8;

            using (XmlWriter xmlWriter = XmlWriter.Create(response.Output))
            {
                _feed.GetAtom10Formatter().WriteTo(xmlWriter);
            }
        }
    }
}