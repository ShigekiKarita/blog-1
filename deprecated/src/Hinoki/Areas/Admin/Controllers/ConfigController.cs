using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hinoki.Core;
using Hinoki.Core.Db;
using Hinoki.Core.Diagnostics;
using Hinoki.Models;
using Hinoki.Areas.Admin.Models;

namespace Hinoki.Areas.Admin.Controllers
{
    public class ConfigController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            VMConfigBase model = new VMConfigBase();
            model.LoadUIElements();
            return View(model);
        }

        [HttpGet]
        public ActionResult AccessAnalytics()
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

            VMConfigAccessAnalytics model = new VMConfigAccessAnalytics();
            model.LoadUIElements();
            model.GoogleAnalytics = cfgMgr.GoogleAnalytics.Value;
            return View(model);
        }

        [HttpPost]
        public ActionResult AccessAnalytics(VMConfigAccessAnalytics args)
        {
            if (!this.ModelState.IsValid)
            {
                return new HttpNotFoundResult();
            }

            BlogConfig config = new BlogConfig();
            config.Name = "google_analytics"; 
            config.Value = args.GoogleAnalytics;
            List<BlogConfig> configs = new List<BlogConfig> { };
            configs.Add(config);
            bool result = HNKBlogConfigFetcher.UpdateConfigs(AppSettings.GetDbConnectionString(), configs);
            if (!result) 
            {
                return new HttpStatusCodeResult(500);
            }

            VMConfigBase model = new VMConfigBase();
            return View("Index", model);
        }
    }
}