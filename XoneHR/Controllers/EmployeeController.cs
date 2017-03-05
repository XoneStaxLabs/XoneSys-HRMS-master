using Newtonsoft.Json;
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
    public class EmployeeController : Controller
    {
        private CommonFunctions common = new CommonFunctions();
        private EmployeeBallayer empObj;

        public EmployeeController()
        {
            empObj = new EmployeeBallayer();
        }

        [AuthorizeAll]
        public ActionResult Index()
        {
            SessionManage.Current.PermitFunctions = common.GetPermissionList("Index", "Employee");
            return View();
        }

        public ActionResult ListAllEmployees()
        {
            //ViewBag.Type = Convert.ToInt16(id);
            return View();
        }

        //public ActionResult ListEmployeeresult(Int16 ListTypeId)
        public ActionResult ListEmployeeresult()
        {
            StringBuilder sb = new StringBuilder();

            //var listAllEmpolyees = empObj.ListAllEmployees(ListTypeId);
            var listAllEmpolyees = empObj.ListAllEmployees();
            var DocDetails = empObj.ListEmployeesDocs(0, 1);
            var DocValidStatus = empObj.ListEmployeesDocs(0, 3);
            SessionManage.Current.PermitFunctions = common.GetPermissionList("Index", "Employee");
            var prjctdetails = empObj.ProjectDetails();

            foreach (var item in listAllEmpolyees)
            {
                int i = 0;
                string path = "/Employee/EmployeeProfile?EmpId=" + item.EmpID + "&CandID=" + item.CandID;
                string EditPath = "/Candidate/Edit?CandID=" + item.CandID + "&EditType=" + 1 + "";
                var details = DocValidStatus.Where(m => m.CandID == item.CandID).Select(m => m.DocStypID);
                var DocCount = DocDetails.Where(m => m.DesigID == item.DesigID && m.CizenID == item.CizenID).Select(e => e.DocStypID).Except(details);
                var salpath = "/Employee/SalaryPayment?EmpId=" + item.EmpID;
                string OffboardStatus = "";
                if (item.OFFBoard_Status == 0)
                    OffboardStatus = "OffBoard";
                else if (item.OFFBoard_Status == 1)
                    OffboardStatus = "Approve";
                else if (item.OFFBoard_Status == 2)
                    OffboardStatus = "ONP";
                else
                    OffboardStatus = "Resign";

                sb.Append("<tr><td>" + item.EmpRegNo + "</td><td><a href='#' data-candID=" + item.CandID + " class='tableNot'>" + DocCount.Count() + "</a> ");
                if (SessionManage.Current.PermitFunctions.Where(m => m.FunTypeID == FunctionTypes.CandidateProfile).Select(m => m.UserPermiStatus).SingleOrDefault())
                    sb.Append("<a href=" + path + " class='text-green bio'><b>&nbsp;&nbsp;" + item.CandName + "</b></a></td>");
                else
                    sb.Append("" + item.CandName + "</td>");
                sb.Append("<td>" + item.EmpTypName + "</td><td>" + item.DesigName + "</td>");
                //foreach (var prj in prjctdetails)
                //{
                //    if (prj.EmpID == item.EmpID)
                //    {
                //        sb.Append("<td>" + prj.ProjName + "</td>");
                //        i++;
                //    }
                //}
                //if (i == 0)
                    sb.Append("<td>" + item.Gradename + "</td>");
                sb.Append("<td>" + item.Citizen + "</td><td>" + item.EmpStartDate.ToString("dd-MM-yyyy") + "</td>");
                //if(SessionManage.Current.PermitFunctions.Where(m=> m.FunTypeID == FunctionTypes.CandidateProfile).Select(m=>m.UserPermiStatus).SingleOrDefault())
                //    sb.Append("<td><a href=" + path + "><i class='fa fa-fw fa-eye'></i></a>&nbsp;");
                //sb.Append("<td><a href='#a' data-empid=" + item.EmpID + " class='BtnOffBoard'><i class=''>OffBoard</i></a>&nbsp;");
                if (SessionManage.Current.PermitFunctions.Where(m => m.FunTypeID == FunctionTypes.EmpCV).Select(m => m.UserPermiStatus).SingleOrDefault())
                    sb.Append("<td><a href='#a' data-empid=" + item.EmpID + " class='BtnCV'><i class='fa fa-file-text-o'></i></a>&nbsp;");
                //if (SessionManage.Current.PermitFunctions.Where(m => m.FunTypeID == FunctionTypes.EmpSalDetails).Select(m => m.UserPermiStatus).SingleOrDefault())
                //    sb.Append("<a href=" + salpath + " class='btnSalary'><i class='fa fa-usd' aria-hidden='true'></i></a>&nbsp;");
                //if (SessionManage.Current.PermitFunctions.Where(m => m.FunTypeID == FunctionTypes.Edit).Select(m => m.UserPermiStatus).SingleOrDefault())
                //    sb.Append("<a href=" + EditPath + " class='' ><i class='fa fa-fw fa-edit'></i></a>&nbsp;");
                if (SessionManage.Current.PermitFunctions.Where(m => m.FunTypeID == FunctionTypes.Delete).Select(m => m.UserPermiStatus).SingleOrDefault())
                    sb.Append("<a href='#a' data-empid=" + item.EmpID + " data-candid=" + item.CandID + " data-name=" + item.CandName + " class='BtnDelete'><i class='fa fa-fw fa-trash-o'></i></a>&nbsp;</td></tr>");
            }

            return Content(sb.ToString());
        }

        public ActionResult ValidDocumentResults(Int64 CandID)
        {
            var DocDetails = empObj.ListEmployeesDocs(CandID, 2);
            return View(DocDetails);
        }

        public ActionResult EmployeeProfile(string EmpId, string CandID)
        {
            SessionManage.Current.EmpID = Convert.ToInt64(EmpId);
            SessionManage.Current.CandID = Convert.ToInt64(CandID);

            var employeeProfileDetails = empObj.ListEmployeesDetails(Convert.ToInt64(EmpId));
            return View(employeeProfileDetails);
        }

        public ActionResult EmployerHistoryList()
        {
            var Employeeworkhistory = empObj.ListEmployeeHistory(SessionManage.Current.EmpID);
            return View(Employeeworkhistory);
        }

        public ActionResult DocumentsLisTablewise()
        {
            var DocumentLists = empObj.ListEmployeeDocuments(SessionManage.Current.EmpID);
            return View(DocumentLists);
        }

        public ActionResult EmployeeSalaryDetails()
        {
            var itemsObj = empObj.ListEmployeeSalarys(SessionManage.Current.EmpID);
            ViewBag.BasicSalary = empObj.GetBasicSal(SessionManage.Current.EmpID);
            return View(itemsObj);
        }

        public ActionResult EmployeeLeaves()
        {
            var Leavedetails = empObj.ListLeaveDetails(SessionManage.Current.EmpID);
            return View(Leavedetails);
        }

        public ActionResult CandidateCV(string EmpID)
        {
            try
            {
                EmployeeCVDetails projObj = new EmployeeCVDetails();

                projObj.EmpDetail = empObj.ListEmployeesDetails(Convert.ToInt64(EmpID));
                projObj.EmpHistory = empObj.ListEmployeeHistory(Convert.ToInt64(EmpID));
                projObj.EmpSkillSet = empObj.ListEmployeeSkillSet(Convert.ToInt64(EmpID));
                projObj.EmpLangKnown = empObj.ListEmployeeLanguageKnown(Convert.ToInt64(EmpID));

                return View(projObj);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult DeclarationContentProfile()
        {
            try
            {
                var ListContent = empObj.DeclarationDetailsProfile();

                var declarationdetails = empObj.GetDeclarationProf();
                if (declarationdetails.Count == 0)
                    ViewBag.DeclarationContent = null;
                else
                    ViewBag.DeclarationContent = declarationdetails;

                return View(ListContent);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult SalaryPayment(Int64 EmpId)
        {
            try
            {
                var empname = empObj.GetEmplyeeName(EmpId);
                ViewBag.empid = EmpId;
                ViewBag.empname = empname.CandName;

                return View();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int SaveSalaryDetails(TblSalaryPay salarypay, string PayDate)
        {
            try
            {
                salarypay.PayDate = common.CommonDateConvertion(PayDate);
                int Output = empObj.SaveSalaryDetails(salarypay);
                return Output;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public ActionResult Delete(Int64 EmpID, Int64 CandId)
        {
            try
            {
                int result = empObj.DeleteEmployee(EmpID, CandId);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult AllowancePayment(Int64 EmpId)
        {
            try
            {
                ViewBag.EmpId = EmpId;

                // var count = empObj.CreateEdit(EmpId);

                //if (count != 0)
                //{
                var FixSalaType = empObj.GetAllowanceSalaryType(EmpId);
                ViewBag.FixSalaType = FixSalaType;

                var FixallType = empObj.GetAllowanceType(EmpId);
                ViewBag.FixallType = FixallType;
                //}
                //else
                //{
                //    var FixSalaType = empObj.GetAllowanceSalaryTypeEMP(EmpId);
                //    ViewBag.FixallType = FixSalaType;

                //    var FixallType = empObj.GetAllowanceTypeEMP(EmpId);
                //    ViewBag.FixallType = FixallType;
                //}

                return View();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult AllowancePayment_ShowDetails(Int64 EmpId, Int64 Year, int month)
        {
            try
            {
                ViewBag.EmpId = EmpId;
                ViewBag.Year = Year;
                ViewBag.month = month;

                var FixSalaType = empObj.GetAllowanceSalaryType(EmpId);
                ViewBag.FixSalaType = FixSalaType;
                ViewBag.EmpFixsalaType = empObj.GetAllowanceSalaryType_Emp(EmpId, Year, month);

                var FixallType = empObj.GetAllowanceType(EmpId);
                ViewBag.FixallType = FixallType;
                ViewBag.EmpFixallType = empObj.GetAllowanceType_Emp(EmpId, Year, month);

                return View();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult AllowanceEmpAssignment(string ParentID, string ChildId, int Year, int Month, Int64 EmpId = 0)
        {
            try
            {
                var FixSalaTypID = ParentID.Replace("\"", "").Replace("[", "").Replace("]", "").Replace("null", "");
                var FixallType = ChildId.Replace("\"", "").Replace("[", "").Replace("]", "").Replace("null", "");

                if (FixSalaTypID == "" && FixallType == "")
                {
                    return Json(new { Message = "Please Select Atleast One CheckBox", Result = -1 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string[] FixedID = FixSalaTypID.Split(',');
                    string[] FixallTypeId = FixallType.Split(',');

                    var length1 = FixedID.Count();
                    var length2 = FixallTypeId.Count();

                    DataTable Parent = new DataTable();
                    Parent.Columns.Add("FixSalaTypID", typeof(int));

                    for (int i = 0; i < length1; i++)
                    {
                        int FixSalaTyp_ID = Convert.ToInt32(FixedID[i]);
                        DataRow newrow;
                        newrow = Parent.NewRow();
                        newrow["FixSalaTypID"] = FixSalaTyp_ID;
                        Parent.Rows.Add(newrow);
                    }

                    DataTable ChildDet = new DataTable();
                    ChildDet.Columns.Add("FixSalaTypID", typeof(int));
                    ChildDet.Columns.Add("FixallowmAmounts", typeof(float));
                    ChildDet.Columns.Add("FixallEmpTypes", typeof(string));
                    for (int i = 0; i < length2; i = i + 4)
                    {
                        DataRow newrow;
                        newrow = ChildDet.NewRow();
                        newrow["FixSalaTypID"] = Convert.ToInt32(FixallTypeId[i + 3]);
                        newrow["FixallowmAmounts"] = Convert.ToDouble(FixallTypeId[i + 2]);
                        newrow["FixallEmpTypes"] = FixallTypeId[i];
                        ChildDet.Rows.Add(newrow);
                    }

                    var Iresult = empObj.AllowanceEmpAssignmentSave(Parent, ChildDet, Year, Month, EmpId);
                    if (Iresult > 0)
                    {
                        return Json(new { Message = "Allowance Assign Saved Successfully", Result = 1 }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { Message = "Allowance Assign Saving Failed !!", Result = 0 }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult ListEmployeeSalary(Int64 EmpID)
        {
            var Results = empObj.ListEmployeeSalary(EmpID);
            return View(Results);
        }

        public JsonResult AddNewSalary(TblEmployeeSalary empsalaObj, string SalStartDate)
        {
            empsalaObj.SalStartDate = common.CommonDateConvertion(SalStartDate);
            DateTime now = empsalaObj.SalStartDate;
            DateTime lastDayLastMonth = new DateTime(now.Year, now.Month, 1);
            lastDayLastMonth = lastDayLastMonth.AddDays(-1);

            empsalaObj.SalEndDate = lastDayLastMonth;
            bool result = empObj.AddNewSalary(empsalaObj);

            return Json(true);
        }

        public JsonResult GetEmployeeDates(Int64 EmpID)
        {
            var results = empObj.GetEmployeeDates(EmpID);
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AllowanceListing(Int64 EmpID, int year, int month)
        {
            var itemsObj = empObj.AllowanceListing(EmpID, year, month);
            return View(itemsObj);
        }

        public JsonResult GetDeductionTypes()
        {
            var data = empObj.GetDeductionTypes();
            //List<data> datas=new List<data>
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public int DeductionTypeSave(string DeductionType)    
        {
            return empObj.DeductionTypeSave(DeductionType); 
        }

        public void DeleteDeductionType(int DeductType_Id)
        {
            empObj.DeleteDeductionType(DeductType_Id);
        }

        public JsonResult SaveDeductions(string List,Int64 EmpId)
        {
            var DeductionItems = JsonConvert.DeserializeObject<TblDeductions[]>(List);
            foreach (var item in DeductionItems)
            {
                TblDeductions deducts = new TblDeductions();
                deducts.DeductType_Id = item.DeductType_Id;
                deducts.EmpID = EmpId;
                deducts.DeductYear = item.DeductYear;
                deducts.DeductMonth = item.DeductMonth;
                deducts.Deduction = item.Deduction;
                deducts.Amount = item.Amount;
                deducts.Remarks = item.Remarks;
                empObj.SaveDeductions(deducts);
            }
            return Json(true);
        }

        public JsonResult ListDeductions(int Year, int Month, Int64 EmpID)
        {
            var data = empObj.ListDeductions(Year, Month, EmpID);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }
}