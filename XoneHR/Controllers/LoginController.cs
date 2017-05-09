using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XoneHR.Models;
using XoneHR.Models.BAL;

namespace XoneHR.Controllers
{
    public class LoginController : Controller
    {
       
        LoginBal LogObj = new LoginBal();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ForgotPassword()
        {
            return View();
        }
        public ActionResult ResetPassword()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(string Uname,string Password)
        {
            try
            {
                var LoginDetails =LogObj.LoginChecking(Uname,Password);

                if(LoginDetails.AppStatus)
                {
                    SessionManage.Current.AppUID = LoginDetails.AppUID;
                    SessionManage.Current.AppUname = LoginDetails.AppFirstName;
                    SessionManage.Current.Designame = LoginDetails.DesigName;
                    SessionManage.Current.AppPhoto = LoginDetails.AppPhoto;
                    SessionManage.Current.GlobalEmpID = LoginDetails.EmpID;
                    SessionManage.Current.Dateformat = "DD/MM/YYYY";


                    return RedirectToAction("DashBoard", "DashBoard");
                }
                else
                {
                    ViewBag.Message = "Invalid UserName & Password";
                    return View();
                }

             

            }
            catch(Exception ex)
            {
                ViewBag.Message = "Invalid UserName & Password";
                return View();
            }
        }


        public ActionResult MenuLists()
        {
            try
            {
               
                    var menulists = LogObj.GetMenus();
                    return View(menulists);
               
                
            }
            catch
            {
                return null;
            }
        }


        public ActionResult UserPasswordBox()
        {
            return View();
        }

        public ActionResult ChangePassword(string Oldpass, string Password, string Confirmpwd)
        {
            var result = LogObj.ChangePassword(Oldpass, SessionManage.Current.AppUID, Confirmpwd);

            if (result == 1)
            {
                return Json(1);
            }
            else if (result == 2)
            {
                return Json(2);
            }
            else if (Password != Confirmpwd)
            {
                return Json(3);
            }
            else
            {
                return Json(0);
            }
            
        }


        public void LoginSession()
        {
            try
            {

                Session.Timeout = Session.Timeout + 30;
            }
            catch
            { }

        }

        public void LogoutSession()
        {
            Session.Abandon();
          

        }
    }
}
