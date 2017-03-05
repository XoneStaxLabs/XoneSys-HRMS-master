using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XoneHR.Models;
using System.Web.Routing;

namespace XoneHR.Controllers
{
    public class AuthorizeAll : ActionFilterAttribute
    {

        CommonFunctions Common = new CommonFunctions();
        public string Action;
        public  string Controller;
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            Action = filterContext.ActionDescriptor.ActionName;
            Controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            SessionManage.Current.PermitFunctions = Common.GetPermissionList(Action, Controller);
            var permissionstatus = Common.GetPermissionStatus(Action, Controller);
            if (permissionstatus != true)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "DashBoard",
                    action = "DashBoard"

                }));
            }

        }


    }
}
