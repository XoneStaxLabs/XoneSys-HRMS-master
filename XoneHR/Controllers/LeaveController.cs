using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XoneHR.Models;
using XoneHR.Models.BAL;
using System.Data;
using System.Text;

using System.IO;
namespace XoneHR.Controllers
{
    public class LeaveController : Controller
    {
        CommonFunctions common = new CommonFunctions();

        private LeaveManage leaveObj; 
        public LeaveController()
        {
            leaveObj = new LeaveManage();
        }
        [AuthorizeAll]
        public ActionResult Index()
        {
            SessionManage.Current.PermitFunctions = common.GetPermissionList("Index", "Leave");
            return View();
        }
        public ActionResult LeaveList(Int64 Empid = 0, int LeaveType = 0, Int16 Status = -1)
        {
            SessionManage.Current.PermitFunctions = common.GetPermissionList("LeaveRequestMain", "Leave");
            var LeaveDetails = leaveObj.ListEmployeesLeaves(Empid, LeaveType, Status);
            return View(LeaveDetails);
        }

        [AuthorizeAll]
        public ActionResult LeaveCreate()
        {
            return View();
        }
        public ActionResult AttendanceMark(string projectid,string projectname)
        {
            ViewBag.prjID = projectid;
            ViewBag.PrjName = projectname;
            return View();
        }
        public ActionResult Attendancebook(int Codevalue, int ScheduleID,Int64 ProjiD)
        {

            ViewBag.Codevalue = Codevalue;
            ViewBag.ScheduleID = ScheduleID;
            ViewBag.ProjiD = ProjiD;

            return View();
        }

        public ActionResult AttendanceBooklist(int Codevalue, int ScheduleID, Int64 ProjiD)
        {
            AttendanceMaster master = new AttendanceMaster();
            StringBuilder sb = new StringBuilder();

            master.AttendanceBook = leaveObj.ListAttendanceBook(Codevalue, ScheduleID, ProjiD);
            master.HolidayAttendance = leaveObj.ListAttendanceHoliday();
            master.LeaveEmployeDetails = leaveObj.LeaveDates();
            var AttendDates = leaveObj.AttendDates(ScheduleID, ProjiD, Codevalue);

            foreach (var items in master.AttendanceBook)
            {
                var shftFrom = items.ShiftFrom.Replace(" ", "-");
                var shftTo = items.ShiftTo.Replace(" ", "-");


                var Sun = master.HolidayAttendance.Where(m => m.HoliDate.ToShortDateString() == items.Wsun.ToShortDateString()).Count();
                var Mon = master.HolidayAttendance.Where(m => m.HoliDate.ToShortDateString() == items.WMon.ToShortDateString()).Count();
                var Tue = master.HolidayAttendance.Where(m => m.HoliDate.ToShortDateString() == items.Wtue.ToShortDateString()).Count();
                var Wed = master.HolidayAttendance.Where(m => m.HoliDate.ToShortDateString() == items.Wwed.ToShortDateString()).Count();
                var Thu = master.HolidayAttendance.Where(m => m.HoliDate.ToShortDateString() == items.Wthu.ToShortDateString()).Count();
                var Fri = master.HolidayAttendance.Where(m => m.HoliDate.ToShortDateString() == items.Wfri.ToShortDateString()).Count();
                var Sat = master.HolidayAttendance.Where(m => m.HoliDate.ToShortDateString() == items.Wsat.ToShortDateString()).Count();

                var aSun = AttendDates.Where(m => m.Attenddate.ToShortDateString() == items.Wsun.ToShortDateString() && m.EmpID == items.EmpID && m.IsOFFDay == false).Count();
                var aMon = AttendDates.Where(m => m.Attenddate.ToShortDateString() == items.WMon.ToShortDateString() && m.EmpID == items.EmpID && m.IsOFFDay == false).Count();
                var aTue = AttendDates.Where(m => m.Attenddate.ToShortDateString() == items.Wtue.ToShortDateString() && m.EmpID == items.EmpID && m.IsOFFDay == false).Count();
                var aWed = AttendDates.Where(m => m.Attenddate.ToShortDateString() == items.Wwed.ToShortDateString() && m.EmpID == items.EmpID && m.IsOFFDay == false).Count();
                var aThu = AttendDates.Where(m => m.Attenddate.ToShortDateString() == items.Wthu.ToShortDateString() && m.EmpID == items.EmpID && m.IsOFFDay == false).Count();
                var aFri = AttendDates.Where(m => m.Attenddate.ToShortDateString() == items.Wfri.ToShortDateString() && m.EmpID == items.EmpID && m.IsOFFDay == false).Count();
                var aSat = AttendDates.Where(m => m.Attenddate.ToShortDateString() == items.Wsat.ToShortDateString() && m.EmpID == items.EmpID && m.IsOFFDay == false).Count();

                var SunOFF = AttendDates.Where(m => m.Attenddate.ToShortDateString() == items.Wsun.ToShortDateString() && m.EmpID == items.EmpID && m.IsOFFDay == true).Count();
                var MonOFF = AttendDates.Where(m => m.Attenddate.ToShortDateString() == items.WMon.ToShortDateString() && m.EmpID == items.EmpID && m.IsOFFDay == true).Count();
                var TueOFF = AttendDates.Where(m => m.Attenddate.ToShortDateString() == items.Wtue.ToShortDateString() && m.EmpID == items.EmpID && m.IsOFFDay == true).Count();
                var WedOFF = AttendDates.Where(m => m.Attenddate.ToShortDateString() == items.Wwed.ToShortDateString() && m.EmpID == items.EmpID && m.IsOFFDay == true).Count();
                var ThuOFF = AttendDates.Where(m => m.Attenddate.ToShortDateString() == items.Wthu.ToShortDateString() && m.EmpID == items.EmpID && m.IsOFFDay == true).Count();
                var FriOFF = AttendDates.Where(m => m.Attenddate.ToShortDateString() == items.Wfri.ToShortDateString() && m.EmpID == items.EmpID && m.IsOFFDay == true).Count();
                var SatOFF = AttendDates.Where(m => m.Attenddate.ToShortDateString() == items.Wsat.ToShortDateString() && m.EmpID == items.EmpID && m.IsOFFDay == true).Count();
                              
                var Sunday = from leave in master.LeaveEmployeDetails where leave.EmpID == items.EmpID && (leave.EmpLeaveDate <= items.Wsun.Date && leave.EmpLeavetodate >= items.Wsun.Date) && leave.DayTypID == 1 select leave.EmpID;
                var Monday = from leave in master.LeaveEmployeDetails where leave.EmpID == items.EmpID && (leave.EmpLeaveDate <= items.WMon.Date && leave.EmpLeavetodate >= items.WMon.Date) && leave.DayTypID == 1 select leave.EmpID;
                var Tuesday = from leave in master.LeaveEmployeDetails where leave.EmpID == items.EmpID && (leave.EmpLeaveDate <= items.Wtue.Date && leave.EmpLeavetodate >= items.Wtue.Date) && leave.DayTypID == 1 select leave.EmpID;
                var Wednesday = from leave in master.LeaveEmployeDetails where leave.EmpID == items.EmpID && (leave.EmpLeaveDate <= items.Wwed.Date && leave.EmpLeavetodate >= items.Wwed.Date) && leave.DayTypID == 1 select leave.EmpID;
                var Thursday = from leave in master.LeaveEmployeDetails where leave.EmpID == items.EmpID && (leave.EmpLeaveDate <= items.Wthu.Date && leave.EmpLeavetodate >= items.Wthu.Date) && leave.DayTypID == 1 select leave.EmpID;
                var Friday = from leave in master.LeaveEmployeDetails where leave.EmpID == items.EmpID && (leave.EmpLeaveDate <= items.Wfri.Date && leave.EmpLeavetodate >= items.Wfri.Date) && leave.DayTypID == 1 select leave.EmpID;
                var Saturday = from leave in master.LeaveEmployeDetails where leave.EmpID == items.EmpID && (leave.EmpLeaveDate <= items.Wsat.Date && leave.EmpLeavetodate >= items.Wsat.Date) && leave.DayTypID == 1 select leave.EmpID;
               
                string scls, stitle, mcls, mtitle, tcls, ttitle, wcls, wtitle, thcls, thtitle, fcls, ftitle, sacls, satitle;

                if (Sunday.Count() != 0)
                {
                    scls = "not-active";
                    stitle = "Leave-Today";
                }
                else
                {
                    scls = "hoverPop";
                    stitle = "";
                }
                if (Monday.Count() != 0)
                {
                    mcls = "not-active";
                    mtitle = "Leave-Today";
                }
                else
                {
                    mcls = "hoverPop";
                    mtitle = "";
                }
                if (Tuesday.Count() != 0)
                {
                    tcls = "not-active";
                    ttitle = "Leave-Today";
                }
                else
                {
                    tcls = "hoverPop";
                    ttitle = "";
                }
                if (Wednesday.Count() != 0)
                {
                    wcls = "not-active";
                    wtitle = "Leave-Today";
                }
                else
                {
                    wcls = "hoverPop";
                    wtitle = "";
                }
                if (Thursday.Count() != 0)
                {
                    thcls = "not-active";
                    thtitle = "Leave-Today";
                }
                else
                {
                    thcls = "hoverPop";
                    thtitle = "";
                }

                if (Friday.Count() != 0)
                {
                    fcls = "not-active";
                    ftitle = "Leave-Today";
                }
                else
                {
                    fcls = "hoverPop";
                    ftitle = "";
                }

                if (Saturday.Count() != 0)
                {
                    sacls = "not-active";
                    satitle = "Leave-Today";
                }
                else
                {
                    sacls = "hoverPop";
                    satitle = "";
                }

                //Off:<span class='shiftEmp label label-danger'>" + items.SalaWeekOff + "</span>  
                sb.Append("<tr><td><img height='70px' width='70px' src=" + items.CandPhoto + " alt='' /><span class='nameEmp'>" + items.CandName + "</span>&nbsp;<span class='shiftEmp label label-success'>" + items.ShiftName + "</span><br /> </td>");
                sb.Append("<td title=" + stitle + ">" + items.Wsun.Day + "<div class=" + scls + "><a href='#'  class='AddAttend' data-emp=" + items.EmpID + " data-empname=" + items.CandName + " data-Shift=" + shftFrom + " data-ShiftTo=" + shftTo + " data-empdate=" + items.Wsun.Date.ToString("dd/MM/yyyy") + " data-holistatus=" + Convert.ToBoolean(Sun) + " data-off=" + aSun + " data-offstatus=" + Convert.ToBoolean(SunOFF) + " data-Shiftemp=" + items.ShiftID + "><i class='fa fa-plus' aria-hidden='true'></i></a></div></td>");
                sb.Append("<td title=" + mtitle + ">" + items.WMon.Day + "<div class=" + mcls + "><a href='#' class='AddAttend' data-emp=" + items.EmpID + " data-empname=" + items.CandName + " data-Shift=" + shftFrom + " data-ShiftTo=" + shftTo + " data-empdate=" + items.WMon.Date.ToString("dd/MM/yyyy") + " data-holistatus=" + Convert.ToBoolean(Mon) + " data-off=" + aMon + " data-offstatus=" + Convert.ToBoolean(MonOFF) + " data-Shiftemp=" + items.ShiftID + "><i class='fa fa-plus' aria-hidden='true'></i></a></div></td>");
                sb.Append("<td title=" + ttitle + ">" + items.Wtue.Day + "<div class=" + tcls + "><a href='#' class='AddAttend' data-emp=" + items.EmpID + " data-empname=" + items.CandName + " data-Shift=" + shftFrom + " data-ShiftTo=" + shftTo + " data-empdate=" + items.Wtue.Date.ToString("dd/MM/yyyy") + " data-holistatus=" + Convert.ToBoolean(Tue) + " data-off=" + aTue + " data-offstatus=" + Convert.ToBoolean(TueOFF) + " data-Shiftemp=" + items.ShiftID + "><i class='fa fa-plus' aria-hidden='true'></i></a></div></td>");
                sb.Append("<td title=" + wtitle + ">" + items.Wwed.Day + "<div class=" + wcls + "><a href='#' class='AddAttend' data-emp=" + items.EmpID + " data-empname=" + items.CandName + " data-Shift=" + shftFrom + " data-ShiftTo=" + shftTo + " data-empdate=" + items.Wwed.Date.ToString("dd/MM/yyyy") + " data-holistatus=" + Convert.ToBoolean(Wed) + " data-off=" + aWed + " data-offstatus=" + Convert.ToBoolean(WedOFF) + " data-Shiftemp=" + items.ShiftID + "><i class='fa fa-plus' aria-hidden='true'></i></a></div></td>");
                sb.Append("<td title=" + thtitle + ">" + items.Wthu.Day + "<div class=" + thcls + "><a href='#' class='AddAttend' data-emp=" + items.EmpID + " data-empname=" + items.CandName + " data-Shift=" + shftFrom + " data-ShiftTo=" + shftTo + " data-empdate=" + items.Wthu.Date.ToString("dd/MM/yyyy") + " data-holistatus=" + Convert.ToBoolean(Thu) + " data-off=" + aThu + " data-offstatus=" + Convert.ToBoolean(ThuOFF) + " data-Shiftemp=" + items.ShiftID + "><i class='fa fa-plus' aria-hidden='true'></i></a></div></td>");
                sb.Append("<td title=" + ftitle + ">" + items.Wfri.Day + "<div class=" + fcls + "><a href='#' class='AddAttend' data-emp=" + items.EmpID + " data-empname=" + items.CandName + " data-Shift=" + shftFrom + " data-ShiftTo=" + shftTo + " data-empdate=" + items.Wfri.Date.ToString("dd/MM/yyyy") + " data-holistatus=" + Convert.ToBoolean(Fri) + " data-off=" + aFri + " data-offstatus=" + Convert.ToBoolean(FriOFF) + " data-Shiftemp=" + items.ShiftID + "><i class='fa fa-plus' aria-hidden='true'></i></a></div></td>");
                sb.Append("<td title=" + satitle + ">" + items.Wsat.Day + "<div class=" + sacls + "><a href='#' class='AddAttend' data-emp=" + items.EmpID + " data-empname=" + items.CandName + " data-Shift=" + shftFrom + " data-ShiftTo=" + shftTo + " data-empdate=" + items.Wsat.Date.ToString("dd/MM/yyyy") + " data-holistatus=" + Convert.ToBoolean(Sat) + " data-off=" + aSat + " data-offstatus=" + Convert.ToBoolean(SatOFF) + " data-Shiftemp=" + items.ShiftID + "><i class='fa fa-plus' aria-hidden='true'></i></a></div></td></tr>");
                
            }
            return Content(sb.ToString());
        }

        public JsonResult GetLeaveType(Int64 EmpID)
        {
            var leaveTypes = leaveObj.ComboListgetLeaveType(EmpID);
            return Json(leaveTypes, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDayType()
        {
            var daytype = leaveObj.ComboListDayType();
            return Json(daytype, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetEmployees()
        {
            var Employeelists = leaveObj.ComboListEmployee();
            return Json(Employeelists, JsonRequestBehavior.AllowGet);

        }
        public JsonResult ComboListSchedule(int monthid, Int64 ProjID)
        {
            var schedulelists = leaveObj.ComboListSchedule(monthid, ProjID);
            return Json(schedulelists, JsonRequestBehavior.AllowGet);

        }
        public JsonResult AddNewAttendance(TblAttendance tblattendobj, string IsOFFDay, string Attenddate)
        {
            //tblattendobj.Attenddate = common.CommonDateConvertion(Attenddate);
            LeaveItems lobj = leaveObj.AddNewAttendance(tblattendobj, IsOFFDay, Attenddate);
            //if (lobj.OutputID != 0)
            //{
            //    return Json(true);
            //}
            //else
            //{
            //    return Json(false);
            //}
            return Json(lobj.OutputID);            
           
        }

        [HttpPost]
        public ActionResult AddnewleaveApplication(LeaveApplication leaveappob, string empStartenddate = null, string EmpLeaveDate = null, string EmpLeavetodate = null,Int64 EmpID = 0)
        {
            double Nofdays = 0.0;

            //var Emphdates = empStartenddate.Split('-');
           
            leaveappob.EmpLeaveDate = common.CommonDateConvertion(EmpLeaveDate);
            leaveappob.EmpLeavetodate = common.CommonDateConvertion(EmpLeavetodate);                      

            if (leaveappob.EmpLeavetodate.ToString() != "1/1/0001 12:00:00 AM")
            {
                Nofdays = (leaveappob.EmpLeavetodate - leaveappob.EmpLeaveDate).TotalDays;
                Nofdays = Nofdays + 1;
            }
            else
            {
                Nofdays = 1;
                leaveappob.EmpLeavetodate = Convert.ToDateTime("1/1/2000 12:00:00 AM");
            }

            leaveappob.EmpappDays = Nofdays;
            
            //if (SessionManage.Current.GlobalEmpID != 0)
            //{
            //    leaveappob.EmpID = SessionManage.Current.GlobalEmpID;
            //}
            leaveappob.EmpID = EmpID;

            LeaveItemsOutput output = leaveObj.LeaveTypeCheck(leaveappob.EmpID, leaveappob.LeavetypID, leaveappob.EmpappDays,leaveappob.DayTypID);

            if (output != null)
            {

                if (output.OutputID == 1 || output.OutputID == 3)
                {
                    LeaveItems leaveiemObj = leaveObj.AddnewLeaveapplication(leaveappob);
                    if (leaveiemObj.OutputID == 1)
                        return Json(new { OutputID = leaveiemObj.OutputID, PendingDays = output.PendingDays }, JsonRequestBehavior.AllowGet);
                    else
                    { 
                        if (leaveiemObj.OutputID == 2) //Already leave applied on requested date
                            return Json(new { OutputID = -2, PendingDays = 0 }, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { OutputID = output.OutputID, PendingDays = output.PendingDays }, JsonRequestBehavior.AllowGet);

            }
            else
                return Json(new { OutputID = 0, PendingDays = 0 }, JsonRequestBehavior.AllowGet);

        }

        private string GetUniqueKey()
        {
            try
            {
                //STRING AUTOGENERATING CODE.........
                var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                var random = new Random();
                var result = new string(
                    Enumerable.Repeat(chars, 8)
                              .Select(s => s[random.Next(s.Length)])
                              .ToArray());

                return result.ToString();
            }
            catch
            {
                return "ErrorPage";
            }
        }

        [AuthorizeAll]
        //Absense Mode Employee Assignment
        public ActionResult LeaveEmpList()
        {
            try
            {
                return View();

            }catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult LeaveEmpListFromAttendance(Int64 ProjID, Int32 SchID, Int32 ShiftID, Int64 EmpID, string OffDay=null)
        {
            try
            {
                ViewBag.ProjID = ProjID;
                ViewBag.SchID = SchID;
                ViewBag.ShiftID = ShiftID;
                ViewBag.EmpID = EmpID;
                DateTime Offdate = common.CommonDateConvertion(OffDay);
                ViewBag.Offdate = Offdate.ToShortDateString();

                return View();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult GetProjectList(Int32 flag=0,Int64 EmpID=0)
        {
            try
            {
                var Projectdetails = leaveObj.GetProjectList(flag, EmpID);
                return Json(Projectdetails , JsonRequestBehavior.AllowGet);
            }catch (Exception ex)
            {
                return null;
            }                       
        }

        public ActionResult GetEmpProject(Int64 EmpLeaveappID,DateTime date)
        {
            try
            {
                var project = leaveObj.GetEmpProject(EmpLeaveappID, date);
                return Json(project, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        public ActionResult GetScheduleList(string ProjID)
        {
            try
            {
                var ScheduleList = leaveObj.GetScheduleList(Convert.ToInt64(ProjID));
                return Json(ScheduleList, JsonRequestBehavior.AllowGet);
            }catch(Exception ex)
            {
                return null;
            }
        }
        public ActionResult GetShiftList(string ProjID)
        {
            try
            {
                var ShiftDetails = leaveObj.GetShiftDetails(Convert.ToInt64(ProjID));
                return Json(ShiftDetails, JsonRequestBehavior.AllowGet);
            }catch(Exception ex)
            {
                return null;
            }
        }
        public ActionResult GetLeaveEmployeeList(string ProjID , string SchID , string ShiftID)
        {
            try
            {
                var LeaveEmpDetails = leaveObj.GetLeaveEmpDetails(Convert.ToInt64(ProjID), Convert.ToInt32(SchID), Convert.ToInt32(ShiftID));
                return Json(LeaveEmpDetails, JsonRequestBehavior.AllowGet);

            }catch(Exception ex)
            {
                return null;
            }

        }
        public ActionResult GetLeaveEmpDetails(string EmpID, string SchID, string Date)
        {
            try
            {
                var LeaveEmpDetails = leaveObj.GerLeaveEmpList(Convert.ToInt64(EmpID), Convert.ToInt32(SchID), Convert.ToDateTime(Date));
                return Json(LeaveEmpDetails, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public ActionResult GetAssignedEmployee(Int64 EmpID,string LeaveDate=null)
        {
            try
            {
                var GetAssignedEmp = leaveObj.GetAssignedEmployee(EmpID,Convert.ToDateTime(LeaveDate));
                return Json(GetAssignedEmp, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return null; 
            }
        }
        public ActionResult GetAssignedDateList(string EmpID, string SchID)
        {
            List<LeaveDate> LeaveObj = new List<LeaveDate>();
            var GetDateList = leaveObj.GetAssignedEmployeeDateList(Convert.ToInt64(EmpID), Convert.ToInt32(SchID));

            foreach (var items in GetDateList)
            {
                var EndDate = items.EmpLeavetodate;
                var Startdate = items.EmpLeaveDate;
                var Datediff = (EndDate - Startdate).TotalDays;
                for (int i = 0; i <= Datediff; i++)
                {
                    LeaveDate obj = new LeaveDate();
                    obj.Leavedate = Startdate;
                    Startdate = Startdate.AddDays(1);

                    LeaveObj.Add(obj);
                }
            }
            var iitem = from s in LeaveObj select new SelectListItem { Text = s.Leavedate.ToString("dd/MM/yyyy"), Value = s.Leavedate.ToString("dd/MM/yyyy") };
            return Json(iitem.ToArray(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveAssignedEmp(TblAbscenceAllocate AbscenceAllocateObj, string[] AbsallEmpID ,string[] EmpLeaveDate)
        {
            try
            { 
                if (AbsallEmpID == null)
                {
                    return Json(new { Message = "Please Add Adhoc Employee !!", Result = 0 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    DataTable DT = new DataTable();
                    DT.Columns.Add("AbsallEmpID", typeof(Int64));
                    DT.Columns.Add("EmpLeaveDate", typeof(DateTime));

                    for (int i = 0; i < AbsallEmpID.Length; i++)
                    {
                        DataRow newrow;
                        newrow = DT.NewRow();
                        newrow["AbsallEmpID"] = Convert.ToInt64(AbsallEmpID[i]);
                        string date1 = common.ChangeDateToSqlFormat(Convert.ToDateTime(EmpLeaveDate[i]));
                        newrow["EmpLeaveDate"] = Convert.ToDateTime(date1);
                        DT.Rows.Add(newrow);
                    }


                    var Iresult = leaveObj.SaveEmployeeAssigned(AbscenceAllocateObj, DT);
                    if (Iresult > 0)
                    {
                        return Json(new { Message = "Employee Absence Allocation Saved Successfully", Result = Iresult }, JsonRequestBehavior.AllowGet);
                    }
                    else if (Iresult == -1)
                    {
                        return Json(new { Message = "Employee Absence Allocation Exists Already", Result = -1 }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { Message = "Employee Absence Allocation Saving Failed !!", Result = 0 }, JsonRequestBehavior.AllowGet);
                    }
                }
                 

            }
            catch (Exception ex)
            {
                return Json(new { Message = "Employee Absence Allocation Saving Failed !!", Result = 0 }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult CheckingAbsenceAssignedEmp(string AbsallEmpID , string LeaveDate , string ProjID , string SchID , string ShiftID)
        {
            string Date = common.ChangeDateToSqlFormat(Convert.ToDateTime(LeaveDate));

            var count = leaveObj.CheckAbsenceEmp(Convert.ToInt64(AbsallEmpID), Convert.ToDateTime(Date), Convert.ToInt64(ProjID), Convert.ToInt32(SchID), Convert.ToInt32(ShiftID));

            return Json(count,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateLeaveStatus(string LeaveData)
        {
            var LeaveItems = LeaveData.Replace("\"", "").Replace("[", "").Replace("]", "").Replace("null", "");
            var Result = 0;
            if (LeaveItems == "")
            {
                return Json(new { Message = "Please select atleast one leave approval status", Result = -1 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string[] Items = LeaveItems.Split(',');

                for (int i = 0; i < Items.Count(); i = i + 2)
                { 
                    Int32 ID = Convert.ToInt32( Items[i]);
                    Byte StatusID  =Convert.ToByte( Items[i + 1]);

                    Result = leaveObj.UpdateLeaveStatus(ID, StatusID,SessionManage.Current.AppUID);
                }
            }
            if (Result == 1)
            {
                return Json(new { Message = "Saved Successfully", Result = 1 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Message = "Saving Failed", Result = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetScheduleDates(string SchID)
        {
            try
            {
                var Dates = leaveObj.GetScheduleDates(Convert.ToInt32(SchID));
                return Json(Dates, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult GetAttendanceTime(Int64 EmpID,string EmpDate)
        {
            try
            {
                var Times = leaveObj.GetAttendanceTime(EmpID, common.CommonDateConvertion(EmpDate));
                return Json(Times, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return null;
            }            
        }

        public JsonResult RemoveAttendance(TblAttendance tblattendobj, string Attenddate)
        {
            tblattendobj.Attenddate = common.CommonDateConvertion(Attenddate);
            int result = leaveObj.RemoveAttendance(tblattendobj);
            if (result != 0)
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }


        }

        public Boolean GetLeaveStatus(Int64 EmpID, string EmpDate)
        {
            try
            {
                var status = leaveObj.GetLeaveStatus(EmpID, common.CommonDateConvertion(EmpDate));
                return status;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public int AddTimeOff(TblTimeOFF timeoff,string TimeOFFDate)
        {
            timeoff.TimeOFFDate = common.CommonDateConvertion(TimeOFFDate);
            int result = leaveObj.AddTimeOff(timeoff);
            
            return result;
        }

        public ActionResult ListTimeOFFs(Int64 EmpID, string TimeOFFDate)
        {
            var TimeList = leaveObj.ListTimeOFFs(EmpID,TimeOFFDate);
            return View(TimeList);
        }

        public void RemoveTimeOFF(Int64 TimeOFFID)
        {
            leaveObj.RemoveTimeOFF(TimeOFFID);
        }

        public ActionResult UpdateLeaveStatus_EmpAssignment(EmpAbsenceAllocation absence)
        {
            var Result = leaveObj.UpdateLeaveStatus_EmpAssignment(absence);
            if (Result > 0)
            {
                return Json(new { Message = "Approve Leave and Allocate Employee Successfully", Result = Result }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Message = "Employee Absence Allocation Failed !!", Result = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        //[AuthorizeAll]
        public ActionResult LeaveRequestMain(string Status=null)
        {
            SessionManage.Current.PermitFunctions = common.GetPermissionList("LeaveRequestMain", "Leave");
            if (Status != null)
                ViewBag.Status = Convert.ToInt16(Status);
            else
                ViewBag.Status = -1;
            return View();
        }

        public string GetEmpName(Int64 EmpID)
        {
            return leaveObj.GetEmpName(EmpID);
        }

        public ActionResult EmployeeLeaves(Int64 EmpID,int leavetypeid = 0)
        {
            var Leavedetails = leaveObj.ListLeaveDetails(EmpID);
            ViewBag.leavetypeid = leavetypeid;
            return View(Leavedetails);
        }
        
        public ActionResult GetLeaveDates(Int64 EmpID,Int16 TypeId)
        {
            var list = leaveObj.GetLeaveDates(EmpID, TypeId);
            return View(list);
        }

        public ActionResult ApproveLeaveStatus(string Date, Int64 EmpID, Int16 LeavetypID,int Status)
        {
           
            var EmpLeaveDate = Date.Replace("\"", "").Replace("[", "").Replace("]", "").Replace("null", "");
            if (EmpLeaveDate == "")
                return Json(new { Message = "Please Confirm Atleast One Date", Result = -1, Icon = "Warning" }, JsonRequestBehavior.AllowGet);
            else
            {
                string[] EmpLeaveDates = EmpLeaveDate.Split(',');
                DataTable LeaveDate = new DataTable();
                LeaveDate.Columns.Add("EmpLeaveDate", typeof(DateTime));
                for (int i = 0; i < EmpLeaveDates.Count(); i++)
                {
                    DataRow newrow;
                    newrow = LeaveDate.NewRow();
                    newrow["EmpLeaveDate"] = common.CommonDateConvertion(EmpLeaveDates[i]);
                    LeaveDate.Rows.Add(newrow);
                }
                var result = leaveObj.ApproveLeaveStatus(EmpID, LeavetypID, LeaveDate, Status);
                if (result == 1)
                    return Json(new { Message = "Leave Approved Successfully", Result = 1, Icon = "success" }, JsonRequestBehavior.AllowGet);
                else if (result == 2)
                    return Json(new { Message = "Leave Rejected", Result = 2, Icon = "warning" }, JsonRequestBehavior.AllowGet);
                else
                {
                    return Json(new { Message = " Failed !!", Result = 0, Icon = "error" }, JsonRequestBehavior.AllowGet);
                }
            }                       

        }

        public void CancelLeave(Int64 EmpLeaveappID,Int16 LeavetypID,Int64 EmpID,string Date)
        {
            leaveObj.CancelLeave(EmpLeaveappID, LeavetypID, EmpID, Date);
        }
        public int UpdateAuthorizeStatus(Int64 EmpID, Int16 LeavetypID,Int16 ApprovdLeavetype, int status, string Date,Int32 DayTypID)
        {
            LeaveItemsOutput output = new LeaveItemsOutput();
            if (LeavetypID != ApprovdLeavetype)
            {
                output = leaveObj.LeaveTypeCheck(EmpID, LeavetypID, 1, DayTypID);
                if (output != null)
                {
                    if (output.OutputID == 1)
                    {
                        int result = leaveObj.AuthorizeStatus(EmpID, LeavetypID, ApprovdLeavetype, status, Date,0);
                        return result;
                    }                        
                    else
                        return 0;
                }
                else
                    return 0;
            }              
            else
            {
                int result = leaveObj.AuthorizeStatus(EmpID, LeavetypID, ApprovdLeavetype, status, Date,1);
                return result;
            }
        }
        public int AuthorizeStatusUnpaid(Int64 EmpID, Int16 LeavetypID, Int16 ApprovdLeavetype, int status, string Date, Int32 DayTypID)
        {
            return leaveObj.AuthorizeStatus(EmpID, LeavetypID, ApprovdLeavetype, status, Date, 0);
        }
        public ActionResult UnpaidApproveLeave(string Date, Int64 EmpID, Int16 LeavetypID)
        {
            var EmpLeaveDate = Date.Replace("\"", "").Replace("[", "").Replace("]", "").Replace("null", "");
            if (EmpLeaveDate == "")
                return Json(new { Message = "Please Confirm Atleast One Date", Result = -1 }, JsonRequestBehavior.AllowGet);
            else
            {
                string[] EmpLeaveDates = EmpLeaveDate.Split(',');
                DataTable LeaveDate = new DataTable();
                LeaveDate.Columns.Add("EmpLeaveDate", typeof(DateTime));
                for (int i = 0; i < EmpLeaveDates.Count(); i++)
                {
                    DataRow newrow;
                    newrow = LeaveDate.NewRow();
                    newrow["EmpLeaveDate"] = common.CommonDateConvertion((EmpLeaveDates[i]));
                    LeaveDate.Rows.Add(newrow);
                }
                var result = leaveObj.UnpaidApproveLeave(EmpID, LeavetypID, LeaveDate);
                if (result == 1)
                    return Json(new { Message = "Action Success", Result = 1, Icon = "success" }, JsonRequestBehavior.AllowGet);               
                else
                    return Json(new { Message = "Action Failed !!", Result = 0, Icon = "error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult LeaveRequestedEmployee()
        {
            var Employeelists = leaveObj.LeaveRequestedEmployee();
            return Json(Employeelists, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RequestedLeaveTypes(Int64 EmpID)
        {
            var types = leaveObj.RequestedLeaveTypes(EmpID);
            return Json(types, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RequestedLeaves_Status(int LeaveType,Int64 EmpID)
        {
            var Status = leaveObj.RequestedLeaves_Status(LeaveType, EmpID);
           // var StatusArray = from stat in Status select new[] { stat.EmpappStatus }.ToArray();
            return Json(Status, JsonRequestBehavior.AllowGet);
        }

    }
}
