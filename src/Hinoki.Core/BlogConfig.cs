using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hinoki.Core
{
    /// <summary>
    /// ブログの設定要素
    /// </summary>
    public class BlogConfig
    {
        #region メンバ変数
        private int _id = 0;
        private string _name = string.Empty;
        private string _value = string.Empty;
        #endregion

        #region プロパティ
        /// <summary>
        /// レコードID
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        

        /// <summary>
        /// 設定名
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// 設定値
        /// </summary>
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        } 
        #endregion
    }
}
