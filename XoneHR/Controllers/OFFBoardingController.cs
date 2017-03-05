using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using XoneHR.Models;
using XoneHR.Models.BAL;

namespace XoneHR.Controllers
{
    public class OFFBoardingController : Controller
    {
        private CommonFunctions common = new CommonFunctions();
        private OFFBoardingBal balObj;

        public OFFBoardingController()
        {
            balObj = new OFFBoardingBal();
        }

        public ActionResult Index()
        {
            balObj.Relieve();
            return View();
        }

        public ActionResult ListEmpDetails()
        {
            SessionManage.Current.PermitFunctions = common.GetPermissionList("Index", "Employee");
            var listAllEmpolyees = balObj.ListAllEmployees();
            return View(listAllEmpolyees);
        }

        //public ActionResult ListEmployeeresult()
        //{
        //    StringBuilder sb = new StringBuilder();

        //    var listAllEmpolyees = balObj.ListAllEmployees();
        //    SessionManage.Current.PermitFunctions = common.GetPermissionList("Index", "Employee");

        //    foreach (var item in listAllEmpolyees)
        //    {
        //        var path = "/OFFBoarding/OFFBoardingRequest?EmpId=" + item.EmpID;

        //        sb.Append("<tr><td>" + item.EmpTypName + "</td><td>" + item.CandName + "</td>");
        //        sb.Append("<td>" + item.DesigName + "</td><td>" + item.CandMobile + "</td>");

        //        //if (SessionManage.Current.PermitFunctions.Where(m => m.FunTypeID == FunctionTypes.EmpCV).Select(m => m.UserPermiStatus).SingleOrDefault())
        //        sb.Append("<td><a href=" + path + " class='  '><i class=''></i></a>&nbsp;");
        //    }

        //    return Content(sb.ToString());
        //}

        public ActionResult OFFBoardingRequest(Int64 EmpId)
        {
            ViewBag.EmpId = EmpId;
            return View();
        }

        public int AddOFFBoardingRequest(string StartDate, string EndDate, Int64 EmpId, string Remark)
        {
            TblOffBoarding offboarding = new TblOffBoarding();
            offboarding.EmpID = EmpId;
            offboarding.OFFBoard_NoticeStart = common.CommonDateConvertion(StartDate);
            offboarding.OFFBoard_NoticeEnd = common.CommonDateConvertion(EndDate);
            //offboarding.OFFBoard_Resgnation = Convert.ToDateTime(ResignDate);
            offboarding.OFFBoard_Remark = Remark;

            var Result = balObj.AddOFFBoardingRequest(offboarding);
            return Result;
        }

        public int UpdateOffBoardStatus(Int64 EmpID, Int16 status)
        {
            int result = balObj.UpdateOffBoardStatus(EmpID, status);
            return result;
        }

        public ActionResult OffBoardingConfirm()
        {
            var listEmpolyees = balObj.ListReqstedEmployees();
            return View(listEmpolyees);
        }

        public int OffBoardingStatus(Int64 EmpId, Int16 status)
        {
            balObj.OffBoardingStatus(EmpId, status);
            return 1;
        }

        public ActionResult ListEmployee()
        {
            var emp = balObj.ListEmployee();
            return Json(emp, JsonRequestBehavior.AllowGet);
        }

        public void ReInstateEmployee(Int64 EmpId)
        {
            balObj.ReInstateEmployee(EmpId);
        }

        public ActionResult EmpCheckList(Int64 EmpId)
        {
            ViewBag.EmpID = EmpId;
            ViewBag.EmpName = balObj.EmployeeName(EmpId);
            var list = balObj.GetCheckListElements(2);
            ViewBag.list = list;
            return View();
        }

        public ActionResult SaveChecklistDetails(Int64 EmpId, string CheckListTypeID, string flg)
        {
            if (flg == "")
            {
                var TypeID = CheckListTypeID.Replace("\"", "").Replace("[", "").Replace("]", "").Replace("null", "");
                if (TypeID != "")
                {
                    string[] CheckListTypeIDs = TypeID.Split(',');
                    for (int i = 0; i < CheckListTypeIDs.Count(); i = i + 3)
                    {
                        TblOffboardCheckList usergobj = new TblOffboardCheckList();
                        usergobj.CheckListTypeID = Convert.ToInt16(CheckListTypeIDs[i]);
                        usergobj.CheckListID = Convert.ToInt64(CheckListTypeIDs[i + 1]);

                        usergobj.Quantity = Convert.ToInt32(CheckListTypeIDs[i + 2]);
                        balObj.SaveCheckListTypeIDs(usergobj);
                    }
                }
                balObj.SaveChecklistDetails(EmpId);
                return Json(1);
            }
            else
            {
                return Json(new { Message = "Please Fill Quantity", Result = -1 }, JsonRequestBehavior.AllowGet);
            }
        }

        //public JsonResult GetCheckListElements()
        //{
        //    var list = balObj.GetCheckListElements();
        //    var datas = (from a in list select new { id = a.CheckListTypeID, parentid = a.CheckListTypeID, text = a.CheckListDetails }).ToList();

        //    return Json(datas, JsonRequestBehavior.AllowGet);
        //}
    }
}