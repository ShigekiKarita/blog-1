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
    public class VMArticleBase : VMAdminBase
    {
        #region メンバ変数
        protected string _title = string.Empty;
        protected string _body = string.Empty;
        protected PublicStatus _publicStatus = PublicStatus.Private;
        protected ArticleFormat _format = ArticleFormat.Html;
        protected int _categoryId = 1;
        private HNKDateTime _openDateTime = new HNKDateTime(HNKTimeZone.TokyoStandard);
        private HNKDateTime _closeDateTime = new HNKDateTime();
        private List<Category> _categories = new List<Category> { };
        private List<SelectListItem> _publicStatusList = new List<SelectListItem> { };
        private List<SelectListItem> _formatList = new List<SelectListItem> { };
        private List<SelectListItem> _categoryList = new List<SelectListItem> { };
        #endregion

        #region プロパティ
        /// <summary>
        /// 記事のタイトル
        /// </summary>
        [Display(Name = "題名")]
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        /// <summary>
        /// 記事の本文
        /// </summary>
        [AllowHtml]
        [Display(Name = "本文")]
        public string Body
        {
            get { return _body; }
            set { _body = value; }
        }

        /// <summary>
        /// 公開設定
        /// </summary>
        [Display(Name = "公開設定")]
        public PublicStatus PublicStatus
        {
            get { return _publicStatus; }
            set { _publicStatus = value; }
        }

        /// <summary>
        /// 本文の記法
        /// </summary>
        [Display(Name = "記法")]
        public ArticleFormat Format
        {
            get { return _format; }
            set { _format = value; }
        }

        /// <summary>
        /// カテゴリID
        /// </summary>
        [Display(Name = "カテゴリ")]
        public int CategoryId
        {
            get { return _categoryId; }
            set { _categoryId = value; }
        }

        [Display(Name = "公開開始日時")]
        public HNKDateTime OpenDateTime
        {
            get { return _openDateTime; }
            set { _openDateTime = value; }
        }

        [Display(Name = "公開終了日時")]
        public HNKDateTime CloseDateTime
        {
            get { return _closeDateTime; }
            set { _closeDateTime = value; }
        }

        /// <summary>
        /// 公開/非公開のリスト。フォームで使用する
        /// </summary>
        public List<SelectListItem> PublicStatusList
        {
            get { return _publicStatusList; }
            private set { _publicStatusList = value; }
        }

        /// <summary>
        /// 本文の記法のリスト。フォームで使用する
        /// </summary>
        public List<SelectListItem> FormatList
        {
            get { return _formatList; }
            private set { _formatList = value; }
        }

        /// <summary>
        /// カテゴリの選択候補
        /// </summary>
        public List<SelectListItem> CategoryList
        {
            get { return _categoryList; }
            set { _categoryList = value; }
        }
        #endregion    
    
        #region コンストラクタ
		public VMArticleBase(string title)
            : base(title)
        {
        }
	    #endregion   

        #region プロテクテッドメソッド
        /// <summary>
        /// カテゴリ全件取得
        /// </summary>
        /// <returns></returns>
        protected List<Category> GetAllCategory()
        {
            if (_categories.Count < 1)
            {
                _categories = CategoryFetcher.GetCategories(AppSettings.GetDbConnectionString(), PublicStatus.Private);
            }

            return _categories;
        }

        /// <summary>
        /// フォームに表示する選択項目の初期化
        /// </summary>
        protected void InitSelectList()
        {
            _publicStatusList.Add(new SelectListItem { Value = PublicStatus.Private.ToString(), Text = "公開しない" });
            _publicStatusList.Add(new SelectListItem { Value = PublicStatus.Public.ToString(), Text = "公開する" });

            _formatList.Add(new SelectListItem { Value = ArticleFormat.Html.ToString(), Text = "HTML形式" });
            _formatList.Add(new SelectListItem { Value = ArticleFormat.Markdown.ToString(), Text = "Markdown形式" });

            foreach (Category category in this.GetAllCategory())
            {
                _categoryList.Add(new SelectListItem { Value = category.Id.ToString(), Text = category.Name });
            }
        }
        #endregion
    }
}