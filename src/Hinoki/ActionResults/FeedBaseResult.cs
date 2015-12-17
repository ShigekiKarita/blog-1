using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.ServiceModel.Syndication;
using Hinoki.Core;
using Hinoki.Models;

namespace Hinoki.ActionResults
{
    /// <summary>
    /// フィードの基底クラス
    /// </summary>
    public class FeedBaseResult : FileResult
    {
        #region メンバ変数
        protected SyndicationFeed _feed;
        #endregion

        #region プロパティ
        /// <summary>
        /// RSSフィード
        /// </summary>
        public SyndicationFeed Feed
        {
            get { return _feed; }
            set { _feed = value; }
        }
        #endregion       
 
        #region コンストラクタ
        public FeedBaseResult(string contentType) : base(contentType)
        {

        }
        #endregion

        /// <summary>
        /// フィードをXMLに書き出します
        /// </summary>
        /// <param name="response"></param>
        protected override void WriteFile(HttpResponseBase response)
        {
            response.ContentEncoding = System.Text.Encoding.UTF8;

            using (XmlWriter xmlWriter = XmlWriter.Create(response.Output))
            {
                _feed.GetRss20Formatter().WriteTo(xmlWriter);
            }
        }
    }
}