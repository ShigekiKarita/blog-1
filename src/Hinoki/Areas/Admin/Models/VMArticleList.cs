using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hinoki.Core;

namespace Hinoki.Areas.Admin.Models
{   
    /// <summary>
    /// 記事一覧ページ用のビューモデルクラス
    /// </summary>
    public class VMArticleList : VMAdminBase
    {
        #region メンバ変数
        private List<Article> _articles = new List<Article> { };
        private string _criteriaKeyword = string.Empty;
        #endregion

        #region プロパティ
        /// <summary>
        /// 記事
        /// </summary>
        public List<Article> Articles
        {
            get { return _articles; }
            set { _articles = value; }
        }

        /// <summary>
        /// 検索キーワード
        /// </summary>
        public string CriteriaKeyword
        {
            get { return _criteriaKeyword; }
            set { _criteriaKeyword = value; }
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public VMArticleList()
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
            this.SetPageTitle("記事一覧");
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
            _breadcrumbItems.Add(new BreadcrumbItem("概要", url.Action("Index", "Dashboard")));
            _breadcrumbItems.Add(new BreadcrumbItem("記事一覧", url.Action("Index", "Article")));
        } 
        #endregion
    }
}