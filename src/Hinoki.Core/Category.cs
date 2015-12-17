using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hinoki.Core
{
    /// <summary>
    /// カテゴリ
    /// </summary>
    public class Category
    {
        #region メンバ変数
        private int _id = 0;
        private DateTime _insertDate = DateTime.MinValue;
        private DateTime _updateDate = DateTime.MinValue;
        private string _name = string.Empty;
        private string _nameYomi = string.Empty;
        private PublicStatus _publicStatus = PublicStatus.Private;
        private int _count = 0;
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
        /// カテゴリー名
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// カテゴリー名のふり仮名
        /// </summary>
        public string NameYomi
        {
            get { return _nameYomi; }
            set { _nameYomi = value; }
        }

        /// <summary>
        /// カテゴリに属している記事の総数
        /// </summary>
        public int Count
        {
            get { return _count; }
            set { _count = value; }
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
        #endregion
    }
}
