using System;
using System.Web.Optimization;

namespace Web.UI
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // JQuery
            bundles.UseCdn = true;

            // JQuery Validator
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate.min.js",
                        "~/Scripts/jquery.validate.unobtrusive.min.js",
                        "~/Scripts/jquery.unobtrusive-ajax.min.js",
                        "~/Scripts/jquery.validate.unobtrusive.dynamic.js",
                        "~/Scripts/jquery.validation/localization/messages_pt_BR.min.js",
                        string.Format("{0}?v={1}", "~/Scripts/validadores-" + System.Threading.Thread.CurrentThread.CurrentUICulture.Name + ".js", DateTime.Now.Ticks)));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/vendor/modernizr-2.8.3-respond-1.4.2.min.js"));

            // Datatables
            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
               "~/Scripts/DataTables/jquery.dataTables.min.js",
               "~/Scripts/DataTables/dataTables.buttons.min.js",
               "~/Scripts/DataTables/dataTables.bootstrap.min.js",
               "~/Scripts/DataTables/dataTables.responsive.min.js"));

            // Bootstrap
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
               "~/Scripts/bootstrap.min.js"));

            // Scripts
            bundles.Add(new ScriptBundle("~/Scripts/bootbox").Include(
                "~/Scripts/bootbox.min.js",
                "~/Scripts/autoNumeric-1.6.2-min.js",
                "~/Scripts/jquery.g2it.upload.js",
                "~/Scripts/jquery.g2it.mascaras-pt-BR.js",
                "~/Scripts/jquery.datetimepicker.full.min.js"));

            // CSS

            bundles.Add(new StyleBundle("~/Content/css/fileinput")
                .Include("~/Content/bootstrap-fileinput/css/fileinput.css"));
            
            bundles.Add(new ScriptBundle("~/Scripts/admin").Include(
                "~/Scripts/admin.js",
                 "~/Scripts/jsNew/jsnew.js"));

#if !DEBUG
            BundleTable.EnableOptimizations = true;
#endif
        }
    }
}
