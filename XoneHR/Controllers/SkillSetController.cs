using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using XoneHR.Models;

namespace XoneHR.Controllers
{
    public class SkillSetController : Controller
    {

        SkillSetBLayer skillobject = new SkillSetBLayer();
        CommonFunctions common = new CommonFunctions();
        
        [AuthorizeAll]
        public ActionResult Index()
        {
            try
            {
                SessionManage.Current.PermitFunctions = common.GetPermissionList("Index", "SkillSet");
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
                SessionManage.Current.PermitFunctions = common.GetPermissionList("Index", "SkillSet");
                var SkillList = skillobject.GetSkillList();
                return View(SkillList);
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

        public ActionResult Create(string skillname,string skilldesc="")
        {
            try
            {
                int result = skillobject.ManageSkill(1, skillname, skilldesc, 1, 0);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        public ActionResult Edit(int SkillId,string skillname, string skilldesc = "", int status=0)
        {
            try
            {
                int result = skillobject.ManageSkill(2, skillname, skilldesc, status, SkillId);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public ActionResult Delete(Int32 SkillId)
        {
            try
            {
                int result = skillobject.ManageSkill(3, "", "", 0, SkillId);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
