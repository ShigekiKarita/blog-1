using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Hinoki.Core.Diagnostics
{
    /// <summary>
    /// ログ出力をする既定クラス
    /// </summary>
    public class LogBase
    {
        private const string defalut_timezone = "Tokyo Standard Time";

        #region パブリックメソッド
        /// <summary>
        /// 例外の内容をファイルに書き出します。ファイル名は本日の日付(日本時間)
        /// </summary>
        /// <param name="dstDir">出力先ディレクトリのパス</param>
        /// <param name="e">例外。nullを指定した場合はログを書き出しません</param>
        /// <returns>ログ出力に成功した場合は真を返します</returns>
        public static bool WriteToFile(string dstDir, Exception e)
        {
            string fileName = LogBase.CreateLogFileName(string.Empty);
            return LogBase.WriteToFile(dstDir, e.ToString(), fileName);
        }

        /// <summary>
        /// 例外の内容をファイルに書き出します
        /// </summary>
        /// <paparam name="dstDir">出力先ディレクトリのパス</paparam>
        /// <param name="e">例外。nullを指定した場合はログを書き出しません</param>
        /// <param name="fileName">引数dstDirに保存するログファイルの名称。空文字もしくはnull指定した場合はログを書き出しません</param>
        /// <returns>ログ出力に成功した場合は真を返します</returns>
        public static bool WriteToFile(string dstDir, Exception e, string fileName)
        {
            return LogBase.WriteToFile(dstDir, e.ToString(), fileName);
        }

        /// <summary>
        /// 任意の文字列をファイルに書き出します。ファイル名は本日の日付(日本時間)
        /// </summary>
        /// <param name="dstDir">出力先ディレクトリまでのパス</param>
        /// <param name="text">任意の文字列。空文字あるいはnullを指定した場合はログを書き出しません</param>
        /// <returns>ログ出力に成功した場合は真を返します</returns>
        public static bool WriteToFile(string dstDir, string text)
        {
            string fileName = LogBase.CreateLogFileName(string.Empty);
            return LogBase.WriteToFile(dstDir, text, fileName);
        }

        /// <summary>
        /// 任意の文字列をファイルに書き出します
        /// </summary>
        /// <paparam name="dstDir">出力先ディレクトリまでのパス</paparam>
        /// <param name="text">任意の文字列。空文字あるいはnullを指定した場合はログを書き出しません</param>
        /// <param name="fileName">引数dstDirに保存するログファイルの名称。空文字もしくはnull指定で本日の日付(日本時間)をファイル名に使います</param>
        public static bool WriteToFile(string dstDir, string text, string fileName)
        {
            if (string.IsNullOrEmpty(dstDir) || string.IsNullOrEmpty(fileName))
            {
                return false;
            }

            bool ret = false;
            try
            {
                string dstPath = Path.Combine(dstDir, fileName);
                ret = LogBase.Write(dstPath, text);
            }
            catch
            {
                return ret;
            }

            return ret;
        }

        #endregion

        #region プライベートメソッド
        /// <summary>
        /// ログを出力します
        /// </summary>
        /// <param name="dstPath">ログ出力先のフルパス</param>
        /// <param name="text">出力内容</param>
        /// <returns>ログ出力に成功した場合は真を返します</returns>
        private static bool Write(string dstPath, string text)
        {
            bool ret = false;

            // パスの指定がない場合は、ログを書き出さない
            if (string.IsNullOrEmpty(dstPath))
            {
                return ret;
            }

            // 書き出す内容がない場合は、ログを書き出さない
            if (string.IsNullOrEmpty(text))
            {
                return ret;
            }

            // 出力先ディレクトリが存在しない場合は、ログを書き出さない
            try
            {
                string dir = Path.GetDirectoryName(dstPath);
                if (!Directory.Exists(dir))
                {
                    return ret;
                }
            }
            catch
            {
                ret = false;
            }

            // ログを出力する
            try
            {
                using (FileStream fs = new FileStream(dstPath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                {
                    StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding(932));
                    TextWriter tw = TextWriter.Synchronized(sw);

                    tw.WriteLine(string.Format("{0} : {1}", DateTime.Now, text));
                    tw.Flush();
                }
                ret = true;
            }
            catch
            {
                ret = false;
            }

            return ret;
        }

        /// <summary>
        /// ログファイル名を生成します
        /// </summary>
        /// <param name="timeZone">時間帯の指定。空文字あるいはnullの場合はTokyo Standard Timeになります</param>
        /// <returns></returns>
        private static string CreateLogFileName(string timeZone)
        {
            if (string.IsNullOrEmpty(timeZone))
            {
                timeZone = defalut_timezone;
            }
            TimeZoneInfo tst = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
            DateTime utcJpNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.Now.ToUniversalTime(), tst);
            return string.Format("{0}_{1:D2}{2:D2}.txt", utcJpNow.Year, utcJpNow.Month, utcJpNow.Day);
        }
        #endregion
    }
}
