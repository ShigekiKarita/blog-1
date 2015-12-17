using System.Web;
using System.Web.Optimization;

namespace Hinoki
{
    public class BundleConfig
    {
        // バンドルの詳細については、http://go.microsoft.com/fwlink/?LinkId=301862  を参照してください
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/mdl/material").Include(
                        "~/Scripts/mdl/material-{version}.js"));

            // 開発と学習には、Modernizr の開発バージョンを使用します。次に、実稼働の準備が
            // できたら、http://modernizr.com にあるビルド ツールを使用して、必要なテストのみを選択します。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/google-analytics").Include(
                      "~/Scripts/google-analytics.js"));

            bundles.Add(new ScriptBundle("~/bundles/article-editor").Include(
                      "~/Scripts/article-editor.js"));

            bundles.Add(new StyleBundle("~/Content/material").Include(
                "~/Content/mdl/material.css"));

            StyleBundle articlePageStyle = new StyleBundle("~/Content/article");
            articlePageStyle.Include("~/Content/article.min.css");
            bundles.Add(articlePageStyle);

            StyleBundle archivePageStyle = new StyleBundle("~/Content/archive");
            archivePageStyle.Include("~/Content/archive.min.css");
            bundles.Add(archivePageStyle);

            StyleBundle adminPageStyle = new StyleBundle("~/Content/admin");
            adminPageStyle.Include("~/Content/admin.min.css");
            bundles.Add(adminPageStyle);

            // 管理画面用
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));
            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                      "~/Content/bootstrap.css"));

            // デバッグを行うには EnableOptimizations を false に設定します。詳細については、
            // http://go.microsoft.com/fwlink/?LinkId=301862 を参照してください
#if DEBUG
            BundleTable.EnableOptimizations = false;
#else
            BundleTable.EnableOptimizations = true;
#endif
        }
    }
}
