using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Text;
using Hinoki.Core;
using Hinoki.Core.Db;
using Hinoki.Core.Diagnostics;

namespace Hinoki.Models
{
    /// <summary>
    /// ビューモデルの基底クラス
    /// </summary>
    public class VMBase
    {
        #region メンバ変数
        private string _pageTitle = string.Empty;
        private string _blogDescription = string.Empty;
        protected int _pageNumber = 1;
        protected int _perPageLines = 5;
        protected int _totalArticleCount = 0;
        #endregion

        #region プロパティ
        /// <summary>
        /// ウェブページタイトル
        /// </summary>
        public string PageTitle
        {
            get { return _pageTitle; }
            set { _pageTitle = value; }
        }

        /// <summary>
        /// ページ番号
        /// </summary>
        [Range(1, int.MaxValue)]
        public int P
        {
            get { return _pageNumber; }
            set { _pageNumber = value; }
        }

        /// <summary>
        /// 1ページあたりに表示する記事数
        /// </summary>
        [Range(1, int.MaxValue)]
        public int L
        {
            get { return _perPageLines; }
            set { _perPageLines = value; }
        }

        /// <summary>
        /// 公開可能な記事の総数
        /// </summary>
        public int TotalArticleCount
        {
            get { return _totalArticleCount; }
            set { _totalArticleCount = value; }
        }
        
        /// <summary>
        /// ブログの解説文
        /// </summary>
        public string BlogDescription
        {
            get { return _blogDescription; }
            set { _blogDescription = value; }
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public VMBase()
        {
        }
        #endregion 

        #region パブリックメソッド
        /// <summary>
        /// 公開可能なカテゴリを取得します
        /// </summary>
        /// <returns></returns>
        public List<Category> GetPublicCategories()
        {
            List<Category> categories = new List<Category> { };

            try
            {
                using (CategoryResource resource = new CategoryResource(AppSettings.GetDbConnectionString()))
                {
                    categories = resource.GetCategories();
                }
            }
            catch (Exception e)
            {
                AzureLog.WriteToTable(e);
            }

            return categories;
        }

        /// <summary>
        /// 公開中の記事数を月単位で取得します
        /// </summary>
        /// <returns></returns>
        public List<ArticleCount> GetPublicArticleCount()
        {
            List<ArticleCount> articles = new List<ArticleCount> { };

            try
            {
                using (HNKArticleResource resource = new HNKArticleResource(AppSettings.GetDbConnectionString()))
                {
                    articles = resource.GetPublicArticleCount();
                }
            }
            catch (Exception e)
            {
                AzureLog.WriteToTable(e);
            }

            return articles;
        }

        /// <summary>
        /// ページ名称をセットします
        /// </summary>
        /// <param name="pageTitle">ページの名称</param>
        /// <param name="siteTitle">ウェブサイトの名称</param>
        public void SetPageTitle(string pageTitle, string siteTitle)
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(siteTitle))
            {
                builder.Append(siteTitle);
            }

            if (!string.IsNullOrEmpty(pageTitle))
            {
                if (builder.Length > 0)
                {
                    // サイト名があれば、その前にページ名を入れる
                    builder.Insert(0, string.Format("{0} - ", pageTitle));
                }
                else
                {
                    // サイト名がなければ、そのままページ名を入れる
                    builder.Append(pageTitle);
                }
            }

            this.PageTitle = builder.ToString();
        }    
        #endregion
    }
}