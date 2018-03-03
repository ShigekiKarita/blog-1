using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hinoki.Areas.Admin.Models
{
    /// <summary>
    /// 概要ページ用のビューモデルクラス
    /// </summary>
    public class VMDashboard : VMAdminBase
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public VMDashboard()
            : base("概要")
        {
            this.InitBreadcrumbItems();
        } 
        #endregion

        #region プライベートメソッド
        /// <summary>
        /// パンくずリストで使う要素を初期化します
        /// </summary>
        protected override void InitBreadcrumbItems()
        {
            UrlHelper url = new UrlHelper(HttpContext.Current.Request.RequestContext);
            _breadcrumbItems.Add(new BreadcrumbItem("概要", url.Action("Index", "Dashboard")));
        }
        #endregion
    }
}