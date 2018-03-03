using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using Hinoki.Core;
using Hinoki.Models;

namespace Hinoki.Areas.Admin.Models
{
    /// <summary>
    /// カテゴリ詳細ページ用のビューモデルクラス
    /// </summary>
    public class VMCategoryDetail : VMAdminBase
    {
        #region メンバ変数
        private int _id;        
        private string _name = string.Empty;
        private string _nameYomi = string.Empty;
        private PublicStatus _publicStatus = PublicStatus.Private;
        private List<SelectListItem> _publicStatusList = new List<SelectListItem> { };
        private bool _initializedBreadcrumb = false;
        #endregion

        #region プロパティ
        /// <summary>
        /// カテゴリID
        /// </summary>
        [Required, Range(1, int.MaxValue)]
        [Display(Name = "ID")]
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// カテゴリの名称
        /// </summary>
        [Display(Name = "名称")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// カテゴリの名称の読み方
        /// </summary>
        [Display(Name = "読み方")]
        public string NameYomi
        {
            get { return _nameYomi; }
            set { _nameYomi = value; }
        }

        /// <summary>
        /// カテゴリの公開設定
        /// </summary>
        [Display(Name = "公開設定")]
        public PublicStatus PublicStatus
        {
            get { return _publicStatus; }
            set { _publicStatus = value; }
        }

        /// <summary>
        /// 公開/非公開のリスト。フォームで使用する
        /// </summary>
        public List<SelectListItem> PublicStatusList
        {
            get { return _publicStatusList; }
            private set { _publicStatusList = value; }
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public VMCategoryDetail()
            : base("")
        {
        }
        #endregion

        #region パブリックメソッド
        /// <summary>
        /// カテゴリを取得します
        /// </summary>
        public override bool Search()
        {
            Category category = CategoryFetcher.GetCategory(AppSettings.GetDbConnectionString(), _id, PublicStatus.Private);
            if (category == null || category.Id < 1)
            {
                return false;
            }

            _name = category.Name;
            _nameYomi = category.NameYomi;
            _publicStatus = category.PublicStatus;

            return true;
        }

        /// <summary>
        /// カテゴリを更新します
        /// </summary>
        /// <param name="category">更新後のカテゴリ</param>
        /// <returns></returns>
        public bool Update(Category category)
        {
            return CategoryFetcher.UpdateCategory(AppSettings.GetDbConnectionString(), category);
        }

        /// <summary>
        /// UIに関係する要素を読み込みます
        /// </summary>
        public void LoadUIElements()
        {
            this.SetPageTitle("カテゴリ詳細");
            this.InitBreadcrumbItems();
            this.InitSelectList();
        }

        /// <summary>
        /// カテゴリに変換します
        /// </summary>
        /// <returns></returns>
        public Category ToCategory()
        {
            Category category = new Category();
            category.Id = this.Id;
            category.Name = this.Name;
            category.NameYomi = this.NameYomi;
            category.PublicStatus = this.PublicStatus;
            return category;
        }

        #endregion

        #region プロテクテッドメソッド
        /// <summary>
        /// パンくずリストで使う要素を初期化します
        /// </summary>
        protected override void InitBreadcrumbItems()
        {
            // 初期化済みの場合は何もしない
            if (_initializedBreadcrumb)
            {
                return;
            }

            UrlHelper url = new UrlHelper(HttpContext.Current.Request.RequestContext);
            _breadcrumbItems.Add(new BreadcrumbItem("概要", url.Action("Index", "Dashboard")));
            _breadcrumbItems.Add(new BreadcrumbItem("カテゴリ一覧", url.Action("Index", "Category")));
            if (_id > 0)
            {
                _breadcrumbItems.Add(new BreadcrumbItem("カテゴリ詳細", url.Action("Detail", "Category", new { id = _id })));
            }
            _initializedBreadcrumb = true;
        } 
        #endregion

        #region プライベートメソッド
        /// <summary>
        /// フォームに表示する選択項目の初期化
        /// </summary>
        private void InitSelectList()
        {
            _publicStatusList.Add(new SelectListItem { Value = PublicStatus.Private.ToString(), Text = "公開しない" });
            _publicStatusList.Add(new SelectListItem { Value = PublicStatus.Public.ToString(), Text = "公開する" });
        } 
        #endregion
    }
}