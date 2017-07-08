using System.Web;
using System.Web.Optimization;

namespace ADE_ManagementSystem
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // jQueryUI CSS
            bundles.Add(new ScriptBundle("~/jqueryuiStyles").Include(
                        "~/Content/themes/base/jquery-ui.min.css"));
            // jQueryUI 
            bundles.Add(new StyleBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-1.12.1.min.js"));
            

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));
            
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.cosmo.min.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/KnockoutWithMappingPlugin").Include(
                      "~/Scripts/knockout-3.4.2.js",
                      "~/Scripts/knockout.mapping-latest.js"));

            bundles.Add(new ScriptBundle("~/bundles/blockUI").Include(
                      "~/Scripts/jquery.blockUI.js"));

            bundles.Add(new ScriptBundle("~/toastrStyles").Include(
                        "~/Content/toastr.min.css"));
            bundles.Add(new ScriptBundle("~/bundles/toastr").Include(
                      "~/Scripts/toastr.min.js"));
        }
    }
}
