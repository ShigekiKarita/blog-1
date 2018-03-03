using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hinoki.Core;

namespace Hinoki.Models
{
    /// <summary>
    /// トップページのビューモデル
    /// </summary>
    public class VMHome : VMBase
    {
        #region メンバ変数
        private List<Article> _publicArticles = new List<Article> { }; 
        #endregion

        #region プロパティ
        /// <summary>
        /// 公開可能な記事
        /// </summary>
        public List<Article> PublicArticles
        {
            get { return _publicArticles; }
            set { _publicArticles = value; }
        }
        #endregion
        
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public VMHome()
            : base()
        {

        } 
        #endregion
    }
}