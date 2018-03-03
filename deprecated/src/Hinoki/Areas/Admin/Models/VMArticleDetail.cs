using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Web.Http.ModelBinding;
using Hinoki.Core;
using Hinoki.Core.Db;
using Hinoki.Core.Diagnostics;
using Hinoki.Models;

namespace Hinoki.Areas.Admin.Models
{
    /// <summary>
    /// 記事ページ用のビューモデルクラス
    /// </summary>
    public class VMArticleDetail : VMArticleBase
    {
        #region メンバ変数
        private int _id = 0;
        #endregion

        #region プロパティ
        /// <summary>
        /// 記事ID
        /// </summary>
        [Required, Range(1, int.MaxValue)]
        [Display(Name = "ID")]
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public VMArticleDetail()
            : base("")
        {

        } 
        #endregion

        #region パブリックメソッド
        /// <summary>
        /// 記事を取得します
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool SearchArticle(int id)
        {
            bool result = false;
            if (id < 1)
            {
                return result;
            }

            try
            {
                using (HNKArticleResource resource = new HNKArticleResource(AppSettings.GetDbConnectionString()))
                {
                    Article article = resource.GetArticle(id, PublicStatus.Private);
                    if (article != null && article.Id > 0)
                    {
                        this.Id = article.Id;
                        this.OpenDateTime.Value = article.OpenDate;
                        this.CloseDateTime.Value = article.CloseDate;
                        this.Title = article.Title;
                        this.Body = article.BodyRaw;
                        this.PublicStatus = article.PublicStatus;
                        this.Format = article.Format;
                        this.CategoryId = article.Category.Id;
                        result = true;
                    }
                }
            }
            catch (Exception e)
            {
                AzureLog.WriteToTable(e);
            }

            return result;
        }

        /// <summary>
        /// 記事を更新します
        /// </summary>
        /// <param name="article">更新後の記事</param>
        /// <returns></returns>
        public bool Update(Article article)
        {
            return ArticleFetcher.UpdateArticle(AppSettings.GetDbConnectionString(), article);
        }

        /// <summary>
        /// UIに関係する要素を読み込みます
        /// </summary>
        public void LoadUIElements()
        {
            this.SetPageTitle("記事編集");
            this.InitBreadcrumbItems();
            this.InitSelectList();
        }

        /// <summary>
        /// 記事に変換します
        /// </summary>
        /// <returns></returns>
        public Article ToArticle()
        {
            Article article = new Article();
            article.Id = this.Id;
            article.OpenDate = (this.OpenDateTime.Value == null ? HNKDateTime.Now(HNKTimeZone.TokyoStandard) : (DateTime)this.OpenDateTime.Value);
            article.CloseDate = this.CloseDateTime.Value;
            article.Title = this.Title;
            article.Body = this.Body;
            article.PublicStatus = this.PublicStatus;
            article.Format = this.Format;

            foreach (Category category in this.GetAllCategory())
            {
                if (category.Id == this.CategoryId)
                {
                    article.Category = category;
                    break;
                }
            }

            return article;
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
            if (_id > 0)
            {
                _breadcrumbItems.Add(new BreadcrumbItem("記事編集", url.Action("Show", "Article", new { id = _id })));
            } 
        }
        #endregion
    }
}