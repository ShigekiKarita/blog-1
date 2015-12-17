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
    /// <summary>
    /// 記事ページのコントローラ
    /// </summary>
    public class ArticleController : Controller
    {
        /// <summary>
        /// 記事ページを返します
        /// </summary>
        /// <param name="id">記事ID。1以上の値を指定しなかった場合は記事を全件表示します</param>
        /// <returns></returns>
        [HttpGet]
        [Route("article/{id:int?}")]
        public ActionResult Index(VMArticle article)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("NotFound", "Error");
            }

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

            // 初期化時に記事をデータベースから取得します
            VMArticle model = new VMArticle((int)article.Id);
            if (model.PublicArticle.Id < 1)
            {
                return RedirectToAction("NotFound", "Error");
            }
            model.SetPageTitle(model.PublicArticle.Title, cfgMgr.BlogTitle.Value);

            model.IsEnabledComment = cfgMgr.IsEnabledComment();
            model.IsEnabledDisqus = cfgMgr.IsEnabledDisqus();
            return View(model);
        }
    }
}