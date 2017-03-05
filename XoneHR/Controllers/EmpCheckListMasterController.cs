using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XoneHR.Models.BAL;
using XoneHR.Models;

namespace XoneHR.Controllers
{
    public class EmpCheckListMasterController : Controller
    {
        EmpCheckListMasterBalLayer chkitmbalobj = new EmpCheckListMasterBalLayer();
        CommonFunctions common = new CommonFunctions();

        //
        // GET: /EmpCheckListMaster/

        public ActionResult Index()
        {
            SessionManage.Current.PermitFunctions = common.GetPermissionList("Index", "EmpCheckListMaster");
            return View();
        }

        public ActionResult List()
        {
            SessionManage.Current.PermitFunctions = common.GetPermissionList("Index", "EmpCheckListMaster");
            var List = chkitmbalobj.GetEmpChkLst();
            return View(List);
        }
        public ActionResult AddNew()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(TblCheckListTypes ChklstObj)
        {
            try
            {
                var Iresult = chkitmbalobj.ManageEmpChkLst(ChklstObj, 1);
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
        public ActionResult Edit(TblCheckListTypes ChklstObj)
        {
            try
            {
                var Iresult = chkitmbalobj.ManageEmpChkLst(ChklstObj, 2);
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
        public ActionResult Delete(int CheckListTypeID)
        {
            try
            {
                var Iresult = chkitmbalobj.DeleteEmpChkLst(CheckListTypeID);
                if (Iresult == 1)
                {
                    return Json(new { Message = "Deleted Successfully", Result = 1 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Message = "Deleted Failed", Result = 0 }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Message = "Deleted Failed", Result = 0 }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
