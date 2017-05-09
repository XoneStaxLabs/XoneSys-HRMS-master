using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XoneHR.Models;
using XoneHR.Models.BAL;

namespace XoneHR.Controllers
{
    public class SetOffDaysController : Controller
    {
        private SetOffBalLayer OffObj;
        private CommonFunctions common;

        public SetOffDaysController()
        {
            OffObj = new SetOffBalLayer();
            common = new CommonFunctions();
        } 

        public ActionResult Index()
        {

            ViewBag.employee = OffObj.GetEmployees();
            return View();
        }

        public ActionResult SaveEmpOff(string EmpID, string OFFDate)
        {
            var Employees = EmpID.Replace("\"", "").Replace("[", "").Replace("]", "").Replace("null", "");
            DateTime OFFDay = common.CommonDateConvertion(OFFDate);

            if (Employees == "")
            {
                return Json(new { Message = "Please Select Atleast One CheckBox", Result = -1 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string[] EmpIDs = Employees.Split(',');
                var length = EmpIDs.Count();

                DataTable Dt = new DataTable();
                Dt.Columns.Add("EmpID", typeof(int));

                for (int i = 0; i < length; i++)
                {
                    var FixSalaTyp_ID = Convert.ToInt32(EmpIDs[i]);
                    DataRow newrow;
                    newrow = Dt.NewRow();
                    newrow["EmpID"] = FixSalaTyp_ID;
                    Dt.Rows.Add(newrow);
                }

                var result = OffObj.AllowanceEmpAssignmentSave(Dt, OFFDay);
                if (result > 0)
                {
                    return Json(new { Message = "Set Offdays Successfully", Result = 1 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Message = "Failed to assign offdays!!", Result = 0 }, JsonRequestBehavior.AllowGet);
                }


            }

        }

    }
}
