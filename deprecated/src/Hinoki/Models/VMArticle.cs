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
    /// <summary>
    /// 記事ページのビューモデル
    /// </summary>
    public class VMArticle : VMBase
    {
        #region メンバ変数
        private int? _id = 0;
        private bool _isEnabledComment = false;
        private Article _publicArticle = new Article (); 
        #endregion

        #region プロパティ
        /// <summary>
        /// 記事ID
        /// </summary>
        [Required]
        [Range(0, int.MaxValue)]
        public int? Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// 記事
        /// </summary>
        public Article PublicArticle
        {
            get { return _publicArticle; }
            set { _publicArticle = value; }
        } 

        public bool IsEnabledComment
        {
            get { return _isEnabledComment; }
            set { _isEnabledComment = value; }
        }

        public bool IsEnabledDisqus
        {
            get { return _isEnabledComment; }
            set { _isEnabledComment = value; }
        }

        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public VMArticle() : base()
        {
        }

        /// <summary>
        /// コンストラクタ。特定の記事を持つ
        /// </summary>
        /// <param name="id"></param>
        public VMArticle(int id) : base ()
        {
            this.PublicArticle = this.GetPublicArticle(id);
        }

        #endregion

        #region プライベートメソッド
        /// <summary>
        /// 公開可能な記事を取得します
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private Article GetPublicArticle(int id)
        {
            Article article = new Article();
            
            if (id < 1)
            {
                return article;
            }

            try
            {
                using (HNKArticleResource resource = new HNKArticleResource(AppSettings.GetDbConnectionString()))
                {
                    article = resource.GetArticle(id);
                }
            }
            catch (Exception e)
            {
                AzureLog.WriteToTable(e);
            }

            string[] delimiter = { "," };
            string[] tagIdsStr = article.TagIds.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
            if (tagIdsStr.Length > 0)
            {
                List<int> tagIdsInt = new List<int> { };
                foreach (string tagIdStr in tagIdsStr)
                {
                    int tagIdInt = 0;
                    bool ret = int.TryParse(tagIdStr, out tagIdInt);
                    if (ret)
                    {
                        tagIdsInt.Add(tagIdInt);
                    }
                }

                try
                {
                    using (HNKTagResource resource = new HNKTagResource(AppSettings.GetDbConnectionString()))
                    {
                        List<HNKTag> tags = resource.GetTags(tagIdsInt);
                        if (tags != null && tags.Count > 0)
                        {
                            article.Tags = tags;
                        }
                    }
                }
                catch (Exception e)
                {
                    AzureLog.WriteToTable(e);
                }
            }

            return article;
        }
        #endregion
    }
}