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
    public class ArchiveController : Controller
    {
        /// <summary>
        /// インデックスページに迷ってきたらホームに戻します
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return RedirectToAction("NotFound", "Error");
        }

        /// <summary>
        /// 指定された日付範囲に該当するブログ記事を表示します
        /// </summary>
        /// <param name="archive"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("archive/{year:int?}/{month:int?}")]
        public ActionResult Index(VMArchive archive)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("NotFound", "Error");
            }

            VMArchive model = null;
            if (archive.Month == null)
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

            // 初期化時にブログ記事をデータベースから取得します
            model = new VMArchive((int)archive.Year, (int)archive.Month, archive.P);                      
            model.SetPageTitle(string.Format("{0}年{1}月の記事", (int)model.Year, (int)model.Month), cfgMgr.BlogTitle.Value);
            return View(model);
        }

        /// <summary>
        /// 指定されたカテゴリIDに属しているブログ記事を表示します
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Category(VMArchiveCategory args)
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

            Category category = VMArchiveCategory.GetPublicCategory(args.Id);
            args.Category = category;

            List<Article> articles = args.GetPublicArticles(args.Id, args.P);
            args.PublicArticles = articles;
            args.SetPageTitle(string.Format("{0}カテゴリ", args.Category.Name), cfgMgr.BlogTitle.Value);
            return View("Category", args);
        }
    }
}