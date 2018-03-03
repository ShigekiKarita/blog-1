using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hinoki.Core;
using Hinoki.Core.Db;
using Hinoki.Core.Diagnostics;
using Hinoki.Models;

namespace Hinoki.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// トップページを表示します
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public ActionResult Index(VMHome args)
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
            args.BlogDescription = cfgMgr.BlogDescription.Value;

            try
            {
                int hitCount = 0;
                args.PublicArticles = ArticleFetcher.GetArticles(AppSettings.GetDbConnectionString(), args.L, args.P, out hitCount);
                args.TotalArticleCount = hitCount;
                args.SetPageTitle(string.Empty, cfgMgr.BlogTitle.Value);
            }
            catch (Exception e)
            {
                AzureLog.WriteToTable(e);
                return new HttpStatusCodeResult(500);
            }

            foreach (Article article in args.PublicArticles) {
                string[] delimiter = { "," };
                string[] tagIdsStr = article.TagIds.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
                if (tagIdsStr.Length > 0)
                {
                    List<int> tagIdsInt = new List<int> { };
                    foreach (string tagIdStr in tagIdsStr)
                    {
                        int tagIdInt = 0;
                        bool ret = int.TryParse(tagIdStr, out tagIdInt);
                        if (ret)
                        {
                            tagIdsInt.Add(tagIdInt);
                        }
                    }

                    try
                    {
                        using (HNKTagResource resource = new HNKTagResource(AppSettings.GetDbConnectionString()))
                        {
                            List<HNKTag> tags = resource.GetTags(tagIdsInt);
                            if (tags != null && tags.Count > 0)
                            {
                                article.Tags = tags;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        AzureLog.WriteToTable(e);
                    }
                }

            }

            return View(args);
        }

        public JavaScriptResult GoogleAnalytics()
        {
            HNKBlogConfigManager cfgMgr = null;
            try
            {
                cfgMgr = new HNKBlogConfigManager(AppSettings.GetDbConnectionString());
            }
            catch (Exception e)
            {
                AzureLog.WriteToTable(e);
                return new JavaScriptResult();
            }

            JavaScriptResult jsResult = new JavaScriptResult();
            jsResult.Script = cfgMgr.GoogleAnalytics.Value;
            return jsResult;
        }
    }
}