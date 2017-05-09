using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XoneHR.Models;
using XoneHR.Models.BAL;

namespace XoneHR.Controllers
{
    public class DepartmentMasterController : Controller
    {
        DepartmentBalLayer DepbalObj = new DepartmentBalLayer();
        CommonFunctions common = new CommonFunctions();

       // [AuthorizeAll]
        public ActionResult Index()
        {
            SessionManage.Current.PermitFunctions = common.GetPermissionList("Index", "DepartmentMaster");
            return View();
        }
        public ActionResult List()
        {
            SessionManage.Current.PermitFunctions = common.GetPermissionList("Index", "DepartmentMaster");
            var List = DepbalObj.GetDepartmentList();
            return View(List);
        }
        public ActionResult AddNew()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(TblDepartment DepObj)
        {
            try
            {
                var Iresult = DepbalObj.ManageDepartment(DepObj, 1 , 0);
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
                    return Json(new { Message = "Data Updated Failed", Result = 0 }, JsonRequestBehavior.AllowGet);
                } 
            }
            catch(Exception ex)
            {
                return Json(new { Message = "Data Updated Failed", Result = 0 }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult Edit(TblDepartment DepObj, int DepStatus = 0)
        {
            try
            {
                var Iresult = DepbalObj.ManageDepartment(DepObj, 2, DepStatus);
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
                    return Json(new { Message = "Data Updated Failed", Result = 0 }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Message = "Data Updated Failed", Result = 0 }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult Delete(int DepID)
        {
            try
            {
                var Iresult = DepbalObj.DeleteDepartment(DepID);
                if (Iresult == 1)
                {
                    return Json(new { Message = "Deleted Successfully", Result = 1 }, JsonRequestBehavior.AllowGet);
                }                
                else
                {
                    return Json(new { Message = "Deletion Failed", Result = 0 }, JsonRequestBehavior.AllowGet);
                }
            }
            catch(Exception ex)
            {
                return Json(new { Message = "Deletion Failed", Result = 0 }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
