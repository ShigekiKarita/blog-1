using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hinoki.Models;

namespace Hinoki.Areas.Admin.Models
{
    /// <summary>
    /// 管理画面用のビューモデル基底クラス
    /// </summary>
    public class VMAdminBase : VMBase
    {
        #region メンバ変数
        private string _siteTitle = "hiroakiの日記";
        private string _accountName = "ゲスト";
        protected List<BreadcrumbItem> _breadcrumbItems = new List<BreadcrumbItem> { };
        #endregion

        #region プロパティ
        /// <summary>
        /// アカウント名
        /// </summary>
        public string AccountName
        {
            get { return _accountName; }
            set { _accountName = value; }
        }

        /// <summary>
        /// パンくずリストに表示する項目
        /// </summary>
        public List<BreadcrumbItem> BreadcrumbItems
        {
            get { return _breadcrumbItems; }
            set { _breadcrumbItems = value; }
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public VMAdminBase() 
            : base()
        {
            this.SetPageTitle(string.Empty);
            _accountName = HttpContext.Current.User.Identity.Name;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public VMAdminBase(string title)
            : base()
        {
            this.SetPageTitle(title);
            _accountName = HttpContext.Current.User.Identity.Name;
        } 
        #endregion

        #region パブリックメソッド
        /// <summary>
        /// ウェブページタイトルをセットします
        /// </summary>
        /// <param name="title"></param>
        public void SetPageTitle(string title)
        {
            if (!string.IsNullOrEmpty(title))
            {
                this.PageTitle = string.Format("{0} - {1}", title, _siteTitle);
            }
            else
            {
                this.PageTitle = string.Format("{0}", _siteTitle);
            }
        }
        #endregion

        #region バーチャルメソッド
        /// <summary>
        /// パンくずリストで使う要素を初期化します
        /// </summary>
        protected virtual void InitBreadcrumbItems()
        {
            System.Diagnostics.Debug.Assert(true, "オーバーライドして使用してください");
        }

        /// <summary>
        /// データベースからアイテムを取得します
        /// </summary>
        /// <returns>検索に成功した場合に真を返す</returns>
        public virtual bool Search()
        {
            System.Diagnostics.Debug.Assert(true, "オーバーライドして使用してください");
            return false;
        }

        /// <summary>
        /// データベースからアイテムを取得します
        /// </summary>
        /// <param name="id">一意な番号</param>
        /// <returns>検索に成功した場合に真を返す</returns>
        public virtual bool Search(int id)
        {
            System.Diagnostics.Debug.Assert(true, "オーバーライドして使用してください");
            return false;
        }
        #endregion
    }
}