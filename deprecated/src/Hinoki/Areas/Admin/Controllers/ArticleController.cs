using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hinoki.Models;
using Hinoki.Areas.Admin.Models;
using Hinoki.Core;

namespace Hinoki.Areas.Admin.Controllers
{
    /// <summary>
    /// 記事を管理するためのコントローラークラス
    /// </summary>
    [Authorize]
    public class ArticleController : Controller
    {
        /// <summary>
        /// 記事一覧ページを表示します
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index(Hinoki.Areas.Admin.Models.VMArticleList args)
        {
            if (args.P < 0)
            {
                args.P = 1;
            }

            if (args.L < 20)
            {
                args.L = 20;
            }

            int totalHitCount = 0;
            args.Articles = ArticleFetcher.GetArticles(AppSettings.GetDbConnectionString(), args.L, args.P, out totalHitCount, PublicStatus.Private);
            args.TotalArticleCount = totalHitCount;
            args.LoadUIElements();

            return View(args);
        }

        [HttpPost]
        public ActionResult Search(string criteriaKeyword)
        {
            VMArticleList model = new VMArticleList();
            int totalHitCount = 0;
            model.Articles = ArticleFetcher.GetArticles(AppSettings.GetDbConnectionString(), model.L, model.P, out totalHitCount, criteriaKeyword, PublicStatus.Private);
            model.TotalArticleCount = totalHitCount;
            return View("Index", model);
        }

        /// <summary>
        /// 記事詳細ページを表示します
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Detail(VMArticleDetail args)
        {
            if (!this.ModelState.IsValid)
            {
                return new HttpNotFoundResult();
            }

            bool result = args.SearchArticle(args.Id);
            if (!result)
            {
                return new HttpNotFoundResult();
            }

            args.LoadUIElements();
            return View(args);
        }

        /// <summary>
        /// 記事作成ページを表示します
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult New()
        {
            VMArticleNew model = new VMArticleNew();
            model.LoadUIElements();
            return View(model);
        }
        
        /// <summary>
        /// 記事を作成します
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult New(VMArticleNew args)
        {
            if (!this.ModelState.IsValid)
            {
                return new HttpNotFoundResult();
            }

            if (!args.Insert())
            {
                return new HttpNotFoundResult();
            }

            return RedirectToAction("Index", "Article");
        }

        /// <summary>
        /// 記事を更新します
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Update(VMArticleDetail args)
        {
            if (!ModelState.IsValid)
            {
                return new HttpNotFoundResult();
            }

            if (!args.Update(args.ToArticle()))
            {
                return new HttpNotFoundResult();
            }

            return RedirectToAction("Index", "Article");
        }
    }
}