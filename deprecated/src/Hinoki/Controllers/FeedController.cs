using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hinoki.Core;
using Hinoki.Core.Db;
using Hinoki.Core.Diagnostics;
using Hinoki.ActionResults;
using Hinoki.Models;

namespace Hinoki.Controllers
{
    /// <summary>
    /// フィード情報を返すコントローラー
    /// </summary>
    public class FeedController : Controller
    {
        /// <summary>
        /// RSS 2.0フォーマットで最近公開した記事をフィードとして返します
        /// </summary>
        /// <returns></returns>
        [Route("feed/rss")]
        public ActionResult Rss()
        {
            HNKBlogConfigManager cfgMgr = null;
            try
            {
                cfgMgr = new HNKBlogConfigManager(AppSettings.GetDbConnectionString());
            }
            catch (Exception e)
            {
                AzureLog.WriteToTable(e);
                return new HttpStatusCodeResult(500);
            }

            // ブログの設定でRSS配信が禁止されている場合
            if (!cfgMgr.IsEnabledOutputRssFeed())
            {
                return new HttpNotFoundResult();
            }

            List<Article> articles = new List<Article> { };
            try
            {
                int hitCount = 0;
                articles = ArticleFetcher.GetArticles(AppSettings.GetDbConnectionString(), 10, 1, out hitCount);
            }
            catch (Exception e)
            {
                AzureLog.WriteToTable(e);
                return new HttpStatusCodeResult(500);
            }
            
            Uri blogUrl = null;
            try
            {
                blogUrl = new Uri(AppSettings.GetBlogUrl());
            }
            catch (Exception e)
            {
                AzureLog.WriteToTable(e);
                return new HttpStatusCodeResult(500);
            }

            return new RssFeedResult(blogUrl, cfgMgr.BlogTitle.Value, cfgMgr.BlogDescription.Value, articles);
        }

        /// <summary>
        /// Atomフォーマットで最近公開した記事をフィードとして返します
        /// </summary>
        /// <returns></returns>
        public ActionResult Atom()
        {
            HNKBlogConfigManager cfgMgr = null;
            try
            {
                cfgMgr = new HNKBlogConfigManager(AppSettings.GetDbConnectionString());
            }
            catch (Exception e)
            {
                AzureLog.WriteToTable(e);
                return new HttpStatusCodeResult(500);
            }

            // ブログの設定でRSS配信が禁止されている場合
            if (!cfgMgr.IsEnabledOutputRssFeed())
            {
                return new HttpNotFoundResult();
            }

            List<Article> articles = new List<Article> { };
            try
            {
                int hitCount = 0;
                articles = ArticleFetcher.GetArticles(AppSettings.GetDbConnectionString(), 10, 1, out hitCount);
            }
            catch (Exception e)
            {
                AzureLog.WriteToTable(e);
                return new HttpStatusCodeResult(500);
            }

            Uri blogUrl = null;
            try
            {
                blogUrl = new Uri(AppSettings.GetBlogUrl());
            }
            catch (Exception e)
            {
                AzureLog.WriteToTable(e);
                return new HttpStatusCodeResult(500);
            }

            return new AtomFeedResult(blogUrl, cfgMgr.BlogTitle.Value, cfgMgr.BlogDescription.Value, articles);
        }
    }
}