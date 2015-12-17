using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hinoki.Core
{
    public class ArticleCount
    {
        #region メンバ変数
        private int _count;
        private DateTime _dateTime;        
        #endregion

        #region プロパティ
        /// <summary>
        /// 記事数 (月単位)
        /// </summary>
        public int Count
        {
            get { return _count; }
            set { _count = value; }
        }

        /// <summary>
        /// 日時 (年/月 のフォーマット)
        /// </summary>
        public DateTime DateTime
        {
            get { return _dateTime; }
            set { _dateTime = value; }
        }        
        #endregion        
        
        public ArticleCount()
        {

        }
    }
}
