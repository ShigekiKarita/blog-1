using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Hinoki.Areas.Admin.Models
{
    public enum HBArticleUpdateResultStatus
    {
        Failure = 0,
        Success = 1
    }
    public class HBArticleUpdateResult
    {
        private HBArticleUpdateResultStatus _status = HBArticleUpdateResultStatus.Failure;

        /// <summary>
        /// 記事更新結果のステータス
        /// </summary>
        [DataMember(Name = "status")]
        public HBArticleUpdateResultStatus Status
        {
            get { return _status; }
            set { _status = value; }
        }
        
    }
}