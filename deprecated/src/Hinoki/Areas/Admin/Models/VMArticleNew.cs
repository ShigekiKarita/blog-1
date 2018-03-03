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
    public class VMArticleNew : VMArticleBase
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public VMArticleNew()
            : base("")
        {

        } 
        #endregion

        #region パブリックメソッド
        /// <summary>
        /// 記事を登録します
        /// </summary>
        /// <param name="article">内容</param>
        /// <returns></returns>
        public bool Insert()
        {
            Article article = new Article();
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

            return ArticleFetcher.InsertArticle(AppSettings.GetDbConnectionString(), article);
        }

        /// <summary>
        /// UIに関係する要素を読み込みます
        /// </summary>
        public void LoadUIElements()
        {
            this.SetPageTitle("記事作成");
            this.InitBreadcrumbItems();
            this.InitSelectList();
        }
        #endregion
    }
}