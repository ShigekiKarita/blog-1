using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MarkdownSharp;

namespace Hinoki.Core
{
    /// <summary>
    /// 記事を表すクラス
    /// </summary>
    public class Article
    {
        #region メンバ変数
        private int _id = 0;
        private DateTime _insertDate = DateTime.MinValue;
        private DateTime _updateDate = DateTime.MinValue;
        private DateTime _openDate = DateTime.MinValue;
        private DateTime? _closeDate = null;
        private string _title = string.Empty;
        private string _body = string.Empty;
        private PublicStatus _publicStatus = PublicStatus.Private;
        private Category _category = new Category();
        private ArticleFormat _format = ArticleFormat.Html;
        private string _tagIds = string.Empty;
        private List<HNKTag> _tags = new List<HNKTag> { };
        #endregion

        #region プロパティ
        /// <summary>
        /// ID
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// 作成日
        /// </summary>
        public DateTime InsertDate
        {
            get { return _insertDate; }
            set { _insertDate = value; }
        }

        /// <summary>
        /// 更新日
        /// </summary>
        public DateTime UpdateDate
        {
            get { return _updateDate; }
            set { _updateDate = value; }
        }

        /// <summary>
        /// 公開開始時刻
        /// </summary>
        public DateTime OpenDate
        {
            get { return _openDate; }
            set { _openDate = value; }
        }

        /// <summary>
        /// 公開終了日時
        /// </summary>
        public DateTime? CloseDate
        {
            get { return _closeDate; }
            set { _closeDate = value; }
        }
        
        /// <summary>
        /// 題名
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        /// <summary>
        /// 本文
        /// </summary>
        public string Body
        {
            get {
                if (_format == ArticleFormat.Markdown)
                {
                    Markdown obj = new Markdown();
                    return obj.Transform(_body);
                }
                return _body; 
            }
            set { _body = value; }
        }

        /// <summary>
        /// 生の本文 (HtmlならHtml, MarkdownならMarkdownのまま取得)
        /// </summary>
        public string BodyRaw
        {
            get { return _body; }
        }

        /// <summary>
        /// 公開ステータス
        /// </summary>
        /// <see cref="PublicStatus"/>
        public PublicStatus PublicStatus
        {
            get { return _publicStatus; }
            set { _publicStatus = value; }
        }

        /// <summary>
        /// カテゴリ
        /// </summary>
        public Category Category
        {
            get { return _category; }
            set { _category = value; }
        }

        /// <summary>
        /// 本文の記法
        /// </summary>
        public ArticleFormat Format
        {
            get { return _format; }
            set { _format = value; }
        }

        /// <summary>
        /// タグIDの文字列(IDはカンマ区切りで複数含まれている可能性がある)
        /// </summary>
        public string TagIds
        {
            get { return _tagIds; }
            set { _tagIds = value; }
        }

        /// <summary>
        /// タグ
        /// </summary>
        public List<HNKTag> Tags
        {
            get { return _tags; }
            set { _tags = value; }
        }
        
        #endregion

        /// <summary>
        /// 本文からHTMLタグを除去した状態の文字列を返します
        /// </summary>
        /// <returns></returns>
        public string RemovedHtmlTagString()
        {
            string text = Regex.Replace(this.Body, @"<[^>]*>", string.Empty);
            text = text.Replace("\r", "").Replace("\n", "");
            return text;
        }

        /// <summary>
        /// 本文から冒頭100文字を概要として返します。
        /// </summary>
        /// <returns>HTMLタグは除去された状態の文字列100文字</returns>
        public string GetDescriptionText()
        {
            string text = this.RemovedHtmlTagString();
            if (text.Length > 97) {
                return string.Format("{0}{1}", text.Substring(0, 97), "...");
            }

            return string.Format("{0}{1}", text.Substring(0, text.Length), "...");
        }
    }
}
