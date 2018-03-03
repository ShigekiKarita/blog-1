using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Hinoki.Core.Diagnostics
{
    /// <summary>
    /// Azureでログ出力をするときのレベル
    /// </summary>
    public enum AzureLogLevel
    {
        ///<summary>インフォメーションログ</summary>
        Information = 0,

        ///<summary>ワーニングログ</summary>
        Warning = 1,

        ///<summary>エラーログ</summary>
        Error = 2
    }

    /// <summary>
    /// Azureのストレージにアプリケーションログを出力します
    /// </summary>
    public class AzureLog
    {
        #region パブリックメソッド
        /// <summary>
        /// 例外の内容をエラーログとして出力します
        /// </summary>
        /// <param name="e">ログに出したい例外</param>
        public static void WriteToTable(Exception e)
        {
            AzureLog.WriteToTable(e.ToString(), AzureLogLevel.Error);
        }

        /// <summary>
        /// 任意の文字列を指定したログレベルで出力します。Windows Azure ストレージのテーブルWADLogsTableに記録されます
        /// </summary>
        /// <param name="text">ログに出したい文字列。nullあるいは空文字の場合はログに出しません</param>
        /// <param name="level">ログレベル</param>
        /// <seealso cref="TRAzureLogLevel"/>
        public static void WriteToTable(string text, AzureLogLevel level)
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            switch (level)
            {
                case AzureLogLevel.Information:
                    Trace.TraceInformation(text);
                    break;
                case AzureLogLevel.Warning:
                    Trace.TraceWarning(text);
                    break;
                case AzureLogLevel.Error:
                    Trace.TraceError(text);
                    break;
            }
        }
        #endregion
    }
}
