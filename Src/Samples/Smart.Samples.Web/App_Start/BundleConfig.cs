using System.Web.Optimization;

namespace Smart.Samples.Web
{
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js/jquery").Include(
                "~/scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/js/ie8").Include(
                "~/scripts/html5shiv.js",
                "~/scripts/respond.min.js",
                "~/scripts/swal-ie.js"));

            bundles.Add(new ScriptBundle("~/js/base").Include(
                "~/scripts/jquery.slimscroll.min.js",
                "~/scripts/bootstrap.min.js",
                "~/scripts/app.min.js",
                "~/scripts/common.js"));

            bundles.Add(new ScriptBundle("~/js/swal").Include(
                "~/scripts/sweetalert.min.js"));

            bundles.Add(new ScriptBundle("~/js/jqgrid").Include(
                "~/scripts/jqgrid/locales/grid.locale-cn.js",
                "~/scripts/jqgrid/jquery.jqgrid.min.js"));

            bundles.Add(new ScriptBundle("~/js/validate").Include(
                "~/scripts/validate/jquery.validate.min.js",
                "~/Scripts/validate/jquery.validate.unobtrusive.min.js",
                "~/scripts/validate/messages_zh.min.js"));

            bundles.Add(new ScriptBundle("~/js/icheck").Include(
                "~/scripts/icheck/icheck.min.js"));

            bundles.Add(new StyleBundle("~/css/base").Include(
                 "~/content/font-awesome.min.css",
                 "~/content/adminlte/css/adminlte.min.css",
                 "~/content/adminlte/css/skins/skin-blue.min.css",
                 "~/content/sweetalert.css",
                 "~/content/site.css"));
        }
    }
}
