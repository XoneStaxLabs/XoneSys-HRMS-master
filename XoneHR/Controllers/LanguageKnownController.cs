using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XoneHR.Models;

namespace XoneHR.Controllers
{
    public class LanguageKnownController : Controller
    {
        LanguageKnownBLayer LngKnownObj = new LanguageKnownBLayer();
        CommonFunctions common = new CommonFunctions();

        [AuthorizeAll]
        public ActionResult Index()
        {
            try
            {
                SessionManage.Current.PermitFunctions = common.GetPermissionList("Index", "LanguageKnown");
                return View();
            }
            catch(Exception ex)
            {
                return null;
            }
            
        }
        public ActionResult List()
        {
            try
            {
                var LangKnownList = LngKnownObj.GetLanguageKnownList();
                return View(LangKnownList);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public ActionResult AddNew()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public ActionResult Create(string LanguageName)
        {
            try
            {
                int result = LngKnownObj.ManageLanguageKnown(1, LanguageName, 1, 0);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public ActionResult Edit(Int16 LangknID, string LanguageName, int status = 0)
        {
            try
            {
                int result = LngKnownObj.ManageLanguageKnown(2, LanguageName, status, LangknID);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public ActionResult Delete(Int16 LangknID)
        {
            try
            {
                int result = LngKnownObj.ManageLanguageKnown(3, "", 0, LangknID);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
