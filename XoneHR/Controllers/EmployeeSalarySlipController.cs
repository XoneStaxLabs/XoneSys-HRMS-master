using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XoneHR.Models;

namespace XoneHR.Controllers
{
    public class EmployeeSalarySlipController : Controller
    {
        EmployeeSalarySlipBal salslipobj = new EmployeeSalarySlipBal();

        public ActionResult Index(Int64 EmpID=0)
        {
            ViewBag.EmpID = EmpID;
            ViewBag.EmpName = salslipobj.EmployeeName(EmpID);
            return View();
        }

        public ActionResult ListSlip(Int64 year, int month, Int64 emp, string monthName)
        {
            try
            {

                

                SalaryAdvanceslip salObj = new SalaryAdvanceslip();

                


                var AdvanceDetails = salslipobj.GetAdvancedetails(year, month, emp); 
                
                if (AdvanceDetails != null)
                {


                    if (AdvanceDetails.Count() == 0)
                        //ViewBag.AdvanceDetails = null;
                        //SessionManage.Current.MyDatas = null;
                        salObj.SalarypayList = null;

                    else
                        //ViewBag.AdvanceDetails = AdvanceDetails;
                        //SessionManage.Current.MyDatas = AdvanceDetails;
                        //salObj.SalarypayList = AdvanceDetails;
                        salObj.SalarypayList = AdvanceDetails;

                }
                else
                    salObj.SalarypayList = null;

                var AllowanceDetails = salslipobj.GetAllowanceDetails(year, month, emp);
                if (AllowanceDetails != null) 
                {
                    if (AllowanceDetails.Count() == 0)
                        ViewBag.AllowanceDetails = null;
                    else
                        ViewBag.AllowanceDetails = AllowanceDetails;                    
                }
                else
                    ViewBag.AllowanceDetails = null;


               

                var salarylist = salslipobj.GetSalarySlip(year, month, emp);
                if (salarylist != null)
                {

                    salObj.SalaryslipContent = salarylist;
                    ViewBag.Year = year;
                    ViewBag.MonthName = monthName;

                    return View(salObj);
                }
                else
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
           
        }

        public ActionResult ListEmployees()
        {
            var employees = salslipobj.ListEmployees();
            return Json(employees, JsonRequestBehavior.AllowGet);
        }

       public int CheckLeaveStatus(Int64 year, int month, Int64 EmpID)
        {
            return salslipobj.CheckLeaveStatus(year, month, EmpID);
        }

    } 
}
