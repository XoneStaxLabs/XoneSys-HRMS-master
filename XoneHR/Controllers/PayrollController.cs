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
    public class PayrollController : Controller
    {
        private CommonFunctions common = new CommonFunctions();
        private PayrollBal payrollObj;
        EmployeeSalarySlipBal salslipobj = new EmployeeSalarySlipBal();

        public PayrollController()
        {
            payrollObj = new PayrollBal();
        }

        public ActionResult Index()
        {
            SessionManage.Current.PermitFunctions = common.GetPermissionList("Index", "Payroll");
            return View();
        }

        public ActionResult ListAllEmployees()
        {
            return View();
        }

        public ActionResult ListEmployeeresult()
        {
            StringBuilder sb = new StringBuilder();
            var listAllEmpolyees = payrollObj.ListAllEmployees();

            SessionManage.Current.PermitFunctions = common.GetPermissionList("Index", "Payroll");
            if (SessionManage.Current.PermitFunctions.Where(m => m.FunTypeID == FunctionTypes.List).Select(m => m.UserPermiStatus).SingleOrDefault())
            {
                foreach (var item in listAllEmpolyees)
                {
                    var salpath = "/Employee/SalaryPayment?EmpId=" + item.EmpID;

                    sb.Append("<tr><td>" + item.EmpRegNo + "</td> <td>" + item.CandName + "</td> <td>" + item.DesigName + "</td> <td>" + item.Gradename + "</td><td>");
                    if (SessionManage.Current.PermitFunctions.Where(m => m.FunTypeID == FunctionTypes.EditPayroll).Select(m => m.UserPermiStatus).SingleOrDefault())
                        sb.Append("<a href=" + salpath + " class='text-green bio btnSalary'><b><span class='label label-success'> Edit Payroll </span></b></a>&nbsp;");  //<i class='fa fa-usd' aria-hidden='true'></i>
                    if (SessionManage.Current.PermitFunctions.Where(m => m.FunTypeID == FunctionTypes.GeneratePayslip).Select(m => m.UserPermiStatus).SingleOrDefault())
                        sb.Append("<a href='#' data-empid=" + item.EmpID + " class='text-green bio Payslip'><b>&nbsp;&nbsp;<span class='label label-success'>Generate Payslip</span></b></a>");
                    if (SessionManage.Current.PermitFunctions.Where(m => m.FunTypeID == FunctionTypes.PayrollSummery).Select(m => m.UserPermiStatus).SingleOrDefault())
                        sb.Append("<a href='#' data-empid=" + item.EmpID + " class='text-green bio summary'><b>&nbsp;&nbsp;<span class='label label-success'>Summary</span></b></a>");
                    sb.Append("</td></tr>");
                } 
            }
                        
            return Content(sb.ToString());
        }

        public ActionResult Summary(Int64 EmpID = 0,Int64 year=0,int month=0)
        {
           
                ViewBag.EmpID = EmpID;
                ViewBag.EmpName = salslipobj.EmployeeName(EmpID);
                ViewBag.year = year;
                ViewBag.month = month;
                       
            return View();
        }

        public int CheckLeaveStatus(Int64 year, int month, Int64 EmpID)
        {
            return salslipobj.CheckLeaveStatus(year, month, EmpID);
        }

        public ActionResult SummaryDetails(Int64 year, int month, Int64 emp)
        {
            try
            {
                var summarydetails = payrollObj.Getsummarydetails(year, month, emp);
                if (summarydetails != null)
                {
                    ViewBag.summary = summarydetails;
                    return View(summarydetails);
                }
                    
                else
                    return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public ActionResult Calendersummary(Int64 year, int month, Int64 EmpID)
        {
            ViewBag.EmpID = EmpID;
            ViewBag.EmpName = salslipobj.EmployeeName(EmpID);

            var dt = DateTime.Parse(year+"/"+month+"/"+"01");  
            //"01/" + month + "/" + year + ""
            DateTime now = DateTime.Now;            
            ViewBag.FromDate = dt.ToString("yyyy/MM/dd");            
            ViewBag.month = month;
            ViewBag.year = year;

            return View();
        }

        public JsonResult GetCalendarEvents(Int64 year, int month, Int64 EmpID)
        {
            var List = payrollObj.Getcalendersummarydetails(year, month, EmpID);
            if (List != null)
            {
                var eventList = from e in List
                                select new
                                {
                                   
                                    id = e.id,
                                    title = e.title,
                                    start = e.start,
                                    end = e.start,
                                    color = e.color,
                                    allDay = false
                                };
                var rows = eventList.ToArray();
                return Json(rows, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }


    }
}
