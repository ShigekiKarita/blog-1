using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hinoki.Areas.Admin.Models
{
    public class VMConfigBase : VMAdminBase
    {
        #region コンストラクタ
        public VMConfigBase()
            : base("設定")
        {
        }
	    #endregion  

        #region パブリックメソッド
        /// <summary>
        /// UIに関係する要素を読み込みます
        /// </summary>
        public void LoadUIElements()
        {
            this.InitBreadcrumbItems();
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
        }
        #endregion
    }
}