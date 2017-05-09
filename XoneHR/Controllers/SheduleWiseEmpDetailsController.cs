using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using XoneHR.Models;
using XoneHR.Models.BAL;

namespace XoneHR.Controllers
{
    public class SheduleWiseEmpDetailsController : Controller
    {
        private SheduleWiseEmpDetailsBal Obj;
        CommonFunctions common = new CommonFunctions();

        public SheduleWiseEmpDetailsController()
        {
            Obj = new SheduleWiseEmpDetailsBal();
        }
        
        [AuthorizeAll]
        public ActionResult Index()
        {           
            return View();
        }

        public ActionResult GetProjects()
        {
            var prjcts = Obj.GetProjects();
            return Json(prjcts, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetShifts(Int64 ProjID)
        {
            var shifts = Obj.GetShifts(ProjID);
            return Json(shifts, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetMonths(Int64 ProjID)
        {
            var months = Obj.GetMonths(ProjID);
            return Json(months, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListSchedules(Int64 ProjID,Int32 month)
        {
            var list = Obj.ListSchedules(ProjID,month);
            var EmpCount = Obj.NumOfEmployees(ProjID, month);

            ViewBag.Scheule = list;
            ViewBag.EmpCount = EmpCount;

            return View();
        }

        public ActionResult ListEmployees(Int32 SchID)
        {
            var ListEmp = Obj.ListEmployees(SchID);
            ViewBag.SchID = SchID;
            ViewBag.Schedule = Obj.GetSchedule(SchID);
            return View(ListEmp);
        }

        public ActionResult ListOFFs(Int32 SchID, Int64 Empid)
        {
            var Offs = Obj.OffDays(SchID, Empid);
            StringBuilder sb = new StringBuilder();
            foreach(var item in Offs)
            {
                sb.Append("<tr><td>" + item.Attenddate.ToString("dd-MM-yyyy") + "</td></tr>");
            }

            return Content(sb.ToString());
        }

        public string GetSchedule(Int32 SchID)
        {
            return Obj.GetSchedule(SchID);
        }

        public ActionResult ListLeaves(Int32 SchID, Int64 Empid)
        {
            var Leaves = Obj.LeaveDetails(SchID, Empid);
            StringBuilder sb = new StringBuilder();
            foreach(var item in Leaves)
            {
                sb.Append("<tr><td>" + item.LeaveType + "</td><td>" + item.EmpappDays + "</td></tr>");               
            }

            return Content(sb.ToString());
        }

    }
}
