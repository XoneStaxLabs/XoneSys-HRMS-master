using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace XoneHR.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {


            var AllStyles = new StyleBundle("~/dist/AllmyCss");
            AllStyles.Include(

         
         
          "~/plugins/jvectormap/jquery-jvectormap-1.2.2.css",
          "~/plugins/datatables/dataTables.bootstrap.css",
          "~/plugins/daterangepicker/daterangepicker-bs3.css",
          "~/plugins/timepicker/bootstrap-timepicker.min.css",
          "~/dist/css/bootstrap-datetimepicker.css",
          "~/plugins/select2/select2.min.css",
          "~/dist/css/skins/_all-skins.min.css",
          "~/dist/css/bootstrap-select.min.css",
          "~/plugins/iCheck/all.css",
          "~/dist/css/AdminLTE.min.css",
          "~/dist/css/custom.css",
          "~/dist/css/star-rating.css"


                );

            bundles.Add(AllStyles);




            var AllScripts = new ScriptBundle("~/dist/AllScripts");
            AllScripts.Include(

                 
                  "~/bootstrap/js/bootstrap.min.js",
                   "~/plugins/select2/select2.full.min.js",
                  "~/plugins/fastclick/fastclick.min.js",
                   "~/dist/js/app.min.js",
                  "~/dist/js/bootstrap-select.min.js",
                   "~/plugins/sparkline/jquery.sparkline.min.js",
                   "~/XoneScripts/Pages/Layout.js",
                   "~/plugins/moment.min.js",
                    "~/plugins/timepicker/bootstrap-timepicker.min.js",
                   "~/plugins/daterangepicker/daterangepicker.js",
                  "~/dist/js/bootstrap-datetimepicker.js",
                  "~/plugins/jqueryvalidation/dist/jquery.validate.min.js",
                   "~/plugins/jqueryvalidation/validationRules.js",                    
                   "~/plugins/jvectormap/jquery-jvectormap-1.2.2.min.js",
                  "~/plugins/jvectormap/jquery-jvectormap-world-mill-en.js",    
                   
                     "~/plugins/iCheck/icheck.min.js",
                   "~/plugins/slimScroll/jquery.slimscroll.min.js",
                  "~/plugins/chartjs/Chart.min.js", 
                   "~/dist/js/demo.js"
                                    
                 


                );

            bundles.Add(AllScripts);

         
          

           
        }
    }
}