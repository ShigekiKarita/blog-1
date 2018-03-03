using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hinoki.Core
{
    /// <summary>
    /// タイムゾーン
    /// </summary>
    public static class HNKTimeZone
    {
        /// <summary>
        /// UTC+9 東京
        /// </summary>
        public static readonly string TokyoStandard = "Tokyo Standard Time";
    }

    /// <summary>
    /// DateTimeにいくつかのメソッドを追加したクラス
    /// </summary>
    public class HNKDateTime
    {
        #region メンバ変数
        private string _timeZone = HNKTimeZone.TokyoStandard;
        private DateTime? _dateTime = null; 
        #endregion

        #region プロパティ
        /// <summary>
        /// 時刻
        /// </summary>
        public DateTime? Value
        {
            get { return _dateTime; }
            set { _dateTime = value; }
        } 
        #endregion

        public HNKDateTime()
        {

        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="tzStr"></param>
        public HNKDateTime(string tzStr)
        {
            if (!string.IsNullOrEmpty(tzStr))
            {
                this.Value = HNKDateTime.Now(tzStr);
            }
        }

        /// <summary>
        /// 時刻を文字列にして返します
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public string ToDateTimeString(string format)
        {
            if (_dateTime == null)
            {
                return string.Empty;
            }
            
            DateTime dt = (DateTime)_dateTime;
            if (string.IsNullOrEmpty(format))
            {
                return dt.ToString("yyyy年MM月dd日 hh時mm分");                
            }

            return dt.ToString(format);                
        }

        #region スタティックメソッド
        /// <summary>
        /// タイムゾーンに適した現在時刻を返します
        /// </summary>
        /// <param name="tzStr">所望するタイムゾーン</param>
        /// <returns></returns>
        public static DateTime Now(string tzStr)
        {
            if (string.IsNullOrEmpty(tzStr))
            {
                return DateTime.Now;
            }

            TimeZoneInfo tzInfo = null;
            try
            {
                tzInfo = TimeZoneInfo.FindSystemTimeZoneById(tzStr);
            }
            catch
            {
                tzInfo = null;
            }

            if (tzInfo == null)
            {
                return DateTime.Now;
            }

            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.Now.ToUniversalTime(), tzInfo);
        } 
        #endregion
    }
}
