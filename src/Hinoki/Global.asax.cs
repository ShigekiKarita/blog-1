using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Hinoki.Core.Diagnostics;

namespace Hinoki
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private const string ErrorPagePath = "/Error/NotFound";

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            /*
             * 例外を取得します
             * HttpUnhandledExceptionにラップされている場合はInnerExceptionを参照する必要があります
             */
            Boolean isUnhandledException = Server.GetLastError() is HttpUnhandledException;
            Exception exception = null;
            if (isUnhandledException)
            {
                Exception tmp = Server.GetLastError();
                if (tmp.InnerException == null)
                {
                    exception = tmp;
                }
                else
                {
                    exception = tmp.InnerException;
                }
            }
            else
            {
                exception = Server.GetLastError();
            }

            // エラーページへ転送します。可能ならばAzureのテーブルにロギングします
            // なお、転送先のエラーページはweb.configのcustomErrorsセクションで定義しません
            this.RedirectToNotFoundPage(exception, Response, true);

            // 例外処理後はServer.ClearError()でクリアする必要があります
            // ただし、Web.configのcustomErrorsセクションに設定して、エラー処理方法を適用させたい場合は除く
            Server.ClearError();
        }

        // エラーページへ転送します。可能ならばAzureのテーブルにロギングします
        // なお、転送先のエラーページはweb.configのcustomErrorsセクションで定義しません
        private void RedirectToNotFoundPage(Exception exception, HttpResponse response, Boolean isLogging)
        {
            if (exception == null)
            {
                return;
            }

            string path = ErrorPagePath;
            if (!this.Request.Path.Contains("ja-JP"))
            {
                path = path.Replace("ja-JP", "en-US");
                response.Redirect(path, false);
            }

            if (exception.GetType() == typeof(HttpRequestValidationException))
            {
                if (isLogging)
                {
                    AzureLog.WriteToTable(exception);
                }
                response.Redirect(path, false);
            }
            else
            {
                if (isLogging)
                {
                    AzureLog.WriteToTable(exception);
                }
                response.Redirect(path, false);
            }
        }

    }
}
