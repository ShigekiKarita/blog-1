using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Hinoki.Core;
namespace Hinoki.Models
{
    /// <summary>
    /// アーカイブページ用ビューモデルクラス
    /// </summary>
    public class VMArchive : VMBase
    {
        #region メンバ変数
        private int? _year;
        private int? _month;
        private List<Article> _articles = new List<Article> { }; 
        #endregion

        #region プロパティ
        /// <summary>
        /// 年
        /// </summary>
        [Required]
        [Range(0, int.MaxValue)]
        public int? Year
        {
            get { return _year; }
            set { _year = value; }
        }

        /// <summary>
        /// 月
        /// </summary>
        [Range(1, 12)]
        public int? Month
        {
            get { return _month; }
            set { _month = value; }
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
        public VMArchive()
            : base()
        {
            this._perPageLines = 100;
        }

        public VMArchive(int year, int month, int pageNumber)
            : base()
        {
            this.Year = year;
            this.Month = month;
            this._perPageLines = 20;
            this.PublicArticles = this.GetPublicArticles(year, month, pageNumber);
        } 
        #endregion

        #region パブリックメソッド
        /// <summary>
        /// ウェブページに表示するためのページタイトルを取得します
        /// </summary>
        /// <returns></returns>
        public string GetPageTitle()
        {
            string str = string.Empty;

            if (this.Year != null)
            {
                if (this.Month != null)
                {
                    DateTime dt = new DateTime((int)this.Year, (int)this.Month, 1);
                    str = string.Format("過去の記事 - {0:yyyy}年{0:MM}月 - ", dt);
                }
                else
                {
                    str = string.Format("過去の記事 - {0}年 - ", this.Year);
                }
            }

            return str;
        } 
        #endregion

        #region プライベートメソッド
        /// <summary>
        /// 指定した年、月の中で公開中の記事を取得します
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        private List<Article> GetPublicArticles(int year, int month, int pageNumber)
        {
            return ArticleFetcher.GetArticlesWithDate(AppSettings.GetDbConnectionString(), this._perPageLines, pageNumber, out this._totalArticleCount, year, month);
        } 
        #endregion 
    }
}