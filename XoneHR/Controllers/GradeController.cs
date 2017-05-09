using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XoneHR.Models;
using XoneHR.Models.BAL;

namespace XoneHR.Controllers
{
    public class GradeController : Controller
    {
        GradeBal GradeBalObj = new GradeBal();
        EmployeeDesignationBalLayer DesigBal = new EmployeeDesignationBalLayer();
        CommonFunctions common = new CommonFunctions();

        [AuthorizeAll]
        public ActionResult Index()
        {
            SessionManage.Current.PermitFunctions = common.GetPermissionList("Index", "Grade");
            return View();
        }

        public ActionResult List()
        {
            SessionManage.Current.PermitFunctions = common.GetPermissionList("Index", "Grade");
            var List = GradeBalObj.GetGradeList();
            return View(List);
        }


        public ActionResult GetDepartment()
        {
            try
            {
                var Department = DesigBal.GetDepartment();
                return Json(Department, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public JsonResult GetDesignation(Int32 depid)
        {
            var ListDesignation = GradeBalObj.GetDesignation(depid);
            return Json(ListDesignation, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddNew()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(TblGrade GradeObj)
        {
            try
            {
                var Iresult = GradeBalObj.ManageGrade(GradeObj, 1, 0);
                if (Iresult == 1)
                {
                    return Json(new { Message = "Data Saved Successfully", Result = 1 }, JsonRequestBehavior.AllowGet);
                }
                else if (Iresult == -1)
                {
                    return Json(new { Message = "Data Already Exist", Result = -1 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Message = "Data Saving Failed", Result = 0 }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Message = "Data Saving Failed", Result = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Edit(TblGrade GradeObj, int GrdStatus = 0)
        {
            try
            {
                var Iresult = GradeBalObj.ManageGrade(GradeObj, 2, GrdStatus);
                if (Iresult == 1)
                {
                    return Json(new { Message = "Data Updated Successfully", Result = 1 }, JsonRequestBehavior.AllowGet);
                }
                else if (Iresult == -1)
                {
                    return Json(new { Message = "Data Already Exist", Result = -1 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Message = "Data Updation Failed", Result = 0 }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Message = "Data Updation Failed", Result = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Delete(int GradeID)
        {
            try
            {
                var Iresult = GradeBalObj.DeleteGrade(GradeID);
                if (Iresult == 1)
                {
                    return Json(new { Message = "Deleted Successfully", Result = 1 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Message = "Deletion Failed", Result = 0 }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Message = "Deletion Failed", Result = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
