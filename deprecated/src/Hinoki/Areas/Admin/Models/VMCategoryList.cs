using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hinoki.Core;
using Hinoki.Models;

namespace Hinoki.Areas.Admin.Models
{
    /// <summary>
    /// カテゴリ一覧ページ用のビューモデルクラス
    /// </summary>
    public class VMCategoryList : VMAdminBase
    {
        #region メンバ変数
        private List<Category> _categories = new List<Category> { }; 
        #endregion

        #region プロパティ
        /// <summary>
        /// カテゴリ
        /// </summary>
        public List<Category> Categoris
        {
            get { return _categories; }
            set { _categories = value; }
        }
        #endregion
        
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public VMCategoryList()
            : base("")
        {
        } 
        #endregion

        #region パブリックメソッド
        /// <summary>
        /// UIに関係する要素を読み込みます
        /// </summary>
        public void LoadUIElements()
        {
            this.SetPageTitle("カテゴリ一覧");
            this.InitBreadcrumbItems();
        }

        /// <summary>
        /// カテゴリをデータベースから取得します
        /// </summary>
        public override bool Search()
        {
            _categories = CategoryFetcher.GetCategories(AppSettings.GetDbConnectionString(), PublicStatus.Private);
            return true;
        } 
        #endregion

        #region プロテクテッドメソッド
        /// <summary>
        /// パンくずリストで使う要素を初期化します
        /// </summary>
        protected override void InitBreadcrumbItems()
        {
            UrlHelper url = new UrlHelper(HttpContext.Current.Request.RequestContext);
            _breadcrumbItems.Add(new BreadcrumbItem("概要", url.Action("Index", "Dashboard")));
            _breadcrumbItems.Add(new BreadcrumbItem("カテゴリ一覧", url.Action("Index", "Category")));
        }
        #endregion
    }
}