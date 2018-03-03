using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hinoki.Areas.Admin.Models
{
    /// <summary>
    /// パンくず
    /// </summary>
    public class BreadcrumbItem
    {
        #region メンバ変数
        private string _title = string.Empty;
        private string _url = string.Empty; 
        #endregion

        #region プロパティ
        /// <summary>
        /// パンくずリストで表示するときの名前
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        /// <summary>
        /// Titleをクリックしたときのリンク先
        /// </summary>
        public string Url
        {
            get { return _url; }
            set { _url = value; }
        } 
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BreadcrumbItem()
        {

        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="title">パンくずの名前</param>
        /// <param name="url">パンくずをクリックしたときの</param>
        public BreadcrumbItem(string title, string url)
        {
            _title = title;
            _url = url;
        } 
        #endregion
    }
}