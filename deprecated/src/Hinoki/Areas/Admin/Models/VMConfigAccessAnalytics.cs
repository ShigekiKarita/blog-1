using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Hinoki.Areas.Admin.Models
{
    public class VMConfigAccessAnalytics : VMConfigBase
    {
        protected string _googleAnalytics = string.Empty;

        /// <summary>
        /// Google Analytics
        /// </summary>
        [Display(Name = "Google Analytics")]
        public string GoogleAnalytics
        {
            get { return _googleAnalytics; }
            set { _googleAnalytics = value; }
        }

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public VMConfigAccessAnalytics()
        {

        } 
        #endregion

        #region プロテクテッドメソッド
        /// <summary>
        /// パンくずリストで使う要素を初期化します
        /// </summary>
        protected override void InitBreadcrumbItems()
        {
            UrlHelper url = new UrlHelper(HttpContext.Current.Request.RequestContext);
            _breadcrumbItems.Add(new BreadcrumbItem("設定", url.Action("Index", "Config")));
            _breadcrumbItems.Add(new BreadcrumbItem("アクセス解析", url.Action("Index", "Config")));
        }
        #endregion
    }
}