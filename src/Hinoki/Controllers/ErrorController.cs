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
    /// エラーページを表示するコントローラー
    /// </summary>
    public class ErrorController : Controller
    {
        /// <summary>
        /// 内部サーバーエラーページを表示します
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
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

            VMError model = new VMError("Internal Server Error");
            model.SetPageTitle("エラーが発生しました", cfgMgr.BlogTitle.Value);
            return View(model);
        }

        /// <summary>
        /// HTTP 404ページを表示します
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult NotFound()
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

            VMError model = new VMError("NotFound");
            model.SetPageTitle("お探しのページは見つかりませんでした", cfgMgr.BlogTitle.Value);
            return View(model);
        }
    }
}