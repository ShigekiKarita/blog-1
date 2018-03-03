using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hinoki.Models;
using Hinoki.Areas.Admin.Models;

namespace Hinoki.Areas.Admin.Controllers
{
    /// <summary>
    /// 概要ページ用コントローラー
    /// </summary>
    [Authorize]
    public class DashboardController : Controller
    {
        /// <summary>
        /// 概要ページを表示します
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            VMDashboard model = new VMDashboard();
            return View(model);
        }
    }
}