using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hinoki.Areas.Admin.Models;

namespace Hinoki.Areas.Admin.Controllers
{
    /// <summary>
    /// カテゴリを管理するためのコントローラークラス
    /// </summary>
    [Authorize]
    public class CategoryController : Controller
    {
        /// <summary>
        /// カテゴリ一覧ページを表示します
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            VMCategoryList model = new VMCategoryList();
            if (!model.Search())
            {
                return new HttpNotFoundResult();
            }
            model.LoadUIElements();
            return View(model);
        }

        /// <summary>
        /// カテゴリ詳細ページを表示します
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Detail(VMCategoryDetail args)
        {
            if (!ModelState.IsValid)
            {
                return new HttpNotFoundResult();
            }

            if (!args.Search())
            {
                return new HttpNotFoundResult();
            }
            args.LoadUIElements();

            return View(args);
        }

        /// <summary>
        /// カテゴリ情報を更新します
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Update(VMCategoryDetail args)
        {
            if (!ModelState.IsValid)
            {
                return new HttpNotFoundResult();
            }

            if (!args.Update(args.ToCategory()))
            {
                return new HttpNotFoundResult();
            }

            return RedirectToAction("Index", "Category");
        }
    }
}