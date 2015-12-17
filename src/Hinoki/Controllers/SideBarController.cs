using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hinoki.Core;
using Hinoki.Models;

namespace Hinoki.Controllers
{
    /// <summary>
    /// サイドバーのコントローラークラス
    /// </summary>
    [ChildActionOnly]
    public class SideBarController : Controller
    {
        /// <summary>
        /// サイドバーに表示する「カテゴリ」を返します
        /// </summary>
        /// <returns></returns>
        public ActionResult Categories()
        {
            VMBase model = new VMBase();
            return PartialView("_Categories", model.GetPublicCategories());
        }

        /// <summary>
        /// サイドバーに表示する「月別記事」を返します
        /// </summary>
        /// <returns></returns>
        public ActionResult Articles()
        {
            VMBase model = new VMBase();
            return PartialView("_Articles", model.GetPublicArticleCount());
        }

        /// <summary>
        /// サイドバーに表示する「広告」を返します
        /// </summary>
        /// <returns></returns>
        public ActionResult Ad()
        {
            return PartialView("_AdsenseTop");
        }

        /// <summary>
        /// サイドバーに表示する「広告」を返します
        /// </summary>
        /// <returns></returns>
        public ActionResult AdsenseLong()
        {
            return PartialView("_AdsenseLong");
        }
    }
}