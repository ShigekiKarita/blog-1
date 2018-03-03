using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Hinoki.Core;
using Hinoki.Core.Db;
using Hinoki.Core.Diagnostics;

namespace Hinoki.Models
{
    public class VMArchiveCategory : VMBase
    {
        #region メンバ変数
        private int _Id;
        private List<Article> _articles = new List<Article> { };
        private Category _category = new Category(); 
        #endregion

        #region プロパティ
        /// <summary>
        /// カテゴリID
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        /// <summary>
        /// カテゴリ情報
        /// </summary>
        public Category Category
        {
            get { return _category; }
            set { _category = value; }
        }

        /// <summary>
        /// 検索でヒットした記事
        /// </summary>
        public List<Article> PublicArticles
        {
            get { return _articles; }
            set { _articles = value; }
        } 
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ。モデルバインディングをするために明示的に設けた
        /// </summary>
        public VMArchiveCategory()
            : base()
        {
            this._perPageLines = 20;
        } 
        #endregion

        /// <summary>
        /// ウェブページに表示するためのページタイトルを取得します
        /// </summary>
        /// <returns></returns>
        public string GetPageTitle()
        {
            string str = string.Empty;
            
            //if (this.Year != null)
            //{
            //    if (this.Month != null)
            //    {
            //        DateTime dt = new DateTime((int)this.Year, (int)this.Month, 1);
            //        str = string.Format("過去の記事 - {0:yyyy}年{0:MM}月 - ", dt);
            //    }
            //    else
            //    {
            //        str = string.Format("過去の記事 - {0}年 - ", this.Year);
            //    }
            //}

            return str;
        }

        /// <summary>
        /// 記事の公開日時を取得します
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        public string GetReleaseDateString (Article article)
        {
            return string.Format("{0:MM}月{0:dd}日", article.OpenDate);
        }

        #region スタティックパブリックメソッド
        /// <summary>
        /// 指定したIDを基にカテゴリ情報を取得します
        /// </summary>
        /// <param name="categoryId">カテゴリID</param>
        /// <returns></returns>
        static public Category GetPublicCategory(int categoryId)
        {
            Category category = new Category();
            if (categoryId < 1)
            {
                return category;
            }

            try
            {
                using (CategoryResource resource = new CategoryResource(AppSettings.GetDbConnectionString()))
                {
                    category = resource.GetCategory(categoryId);
                }
            }
            catch (Exception e)
            {
                AzureLog.WriteToTable(e);
            }

            return category;
        }

        /// <summary>
        /// 指定したカテゴリIDに属している公開中の記事を取得します
        /// </summary>
        /// <param name="categoryId">カテゴリID</param>
        /// <returns></returns>
        public List<Article> GetPublicArticles(int categoryId, int pageNumber)
        {
            if (pageNumber < 0)
            {
                pageNumber = 1;
            }

            List<Article> articles = new List<Article> { };
            try
            {
                using (HNKArticleResource resource = new HNKArticleResource(AppSettings.GetDbConnectionString()))
                {
                    articles = resource.GetPublicArticlesByCategory(categoryId, this._perPageLines, pageNumber, out this._totalArticleCount);
                }
            }
            catch (Exception e)
            {
                AzureLog.WriteToTable(e);
            }

            return articles;
        }
        #endregion
    }
}