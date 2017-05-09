using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XoneHR.Models.BAL;
using XoneHR.Models;
using System.Globalization;
using System.Data;
using Newtonsoft.Json;
using System.Drawing;

namespace XoneHR.Controllers
{
    public class ProjectController : Controller
    {
        private CommonFunctions common = new CommonFunctions();
        private ProjectBalLayer projectObj;

        public ProjectController()
        {
            projectObj = new ProjectBalLayer();
        }

        [AuthorizeAll]
        public ActionResult Index()
        {
            SessionManage.Current.PermitFunctions = common.GetPermissionList("Index", "Project");
            return View();
        }

        public ActionResult ProjectList()
        {
            SessionManage.Current.PermitFunctions = common.GetPermissionList("Index", "Project");
            var ProjectDetails = projectObj.ListProjectDatatable();
            return View(ProjectDetails);
        }

        public ActionResult Create()
        {
            return View();
        }

        public JsonResult GetDesignation()
        {
            var IList = projectObj.GetDesignation();
            return Json(IList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGrade(int DesigID = 0)
        {
            var IGradeList = projectObj.GetGrade(DesigID);
            return Json(IGradeList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProjectType()
        {
            var projecttype = projectObj.ComboListprojectType();
            return Json(projecttype, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmployees(Int64 ProjID = 0, Int16 Flag = 0)
        {
            var employeelist = projectObj.ComboListEmployee(ProjID, Flag);
            return Json(employeelist, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddNewProject(TblProject projObj, string ProjFrom, string ProjTo, string ShiftList, string GradeList)
        {
            projObj.ProjFrom = common.CommonDateConvertion(ProjFrom);
            projObj.ProjTo = common.CommonDateConvertion(ProjTo);

            var Shiftitems = JsonConvert.DeserializeObject<TblShift[]>(ShiftList);
            var Gradeitems = JsonConvert.DeserializeObject<TblProjectGradeAssign[]>(GradeList);

            ProjectItems probj = projectObj.AddNewProject(projObj);
            if (probj.OutputID != 0)
            {
                foreach (var sht in Shiftitems)
                {
                    TblShift obj = new TblShift();
                    obj.ShiftName = sht.ShiftName;
                    obj.ShiftFrom = sht.ShiftFrom;
                    obj.ShiftTo = sht.ShiftTo;
                    obj.ShiftEmpNo = 0;
                    obj.ProjID = probj.TableID;

                    var ShiftDetails = projectObj.AddNewshift(obj);

                    foreach (var grd in Gradeitems)
                    {
                        if (sht.Indexval == grd.ShiftID)
                        {
                            TblProjectGradeAssign Graobj = new TblProjectGradeAssign();
                            Graobj.GradeID = Convert.ToInt32(grd.GradeID);
                            Graobj.DesigID = Convert.ToInt32(grd.DesigID);
                            Graobj.PrjGradeEmpNO = Convert.ToInt32(grd.PrjGradeEmpNO);
                            Graobj.PrjGrdAssignID = Convert.ToInt32(grd.PrjGrdAssignID);
                            Graobj.PrjGrdAssgnStatus = true;
                            Graobj.ProjID = probj.TableID;
                            Graobj.ShiftID = Convert.ToInt32(ShiftDetails.TableID);
                            projectObj.AddProjectGradeAssign(Graobj);
                        }
                    }
                }

                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }

        public ActionResult Roster(string ProjID)
        {
            ViewBag.ProjectID = ProjID;
            var ProjectDetails = projectObj.GetProjectDetails(Convert.ToInt64(ProjID));
            return View(ProjectDetails);
        }

        public ActionResult ScheduleList(Int64 projectID, int monthCode)
        {
            var Projectschedule = projectObj.ListprojectSchedule(projectID, monthCode);
            return Json(Projectschedule, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAssignedShedulesList(Int64 projectID, int monthCode)
        {
            var Projectschedule = projectObj.ListprojectAssignSchedule(projectID, monthCode);
            var selected = from cnd in Projectschedule select new[] { cnd.SchID }.ToArray();
            return Json(selected, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ComboListMonths()
        {
            var months = projectObj.CombolistMonths();
            return Json(months, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Shiftlist(Int64 projectID, int monthcode)
        {
            RosterShiftList obj = new RosterShiftList();
            obj.EmployeeProfile = projectObj.ListShiftsEmployees(projectID);
            obj.ShiftList = projectObj.ListShifts(projectID);

            obj.ProjectmasterIDs = projectObj.ListScheduleEmployeesIDs(projectID.ToString());
            obj.RosterscheduleList = projectObj.ListRosterSchedule(projectID);
            obj.ProjectSchedule = projectObj.ListprojectSchedule(projectID, monthcode);

            return View(obj);
        }

        public ActionResult ShiftCreate()
        {
            return View();
        }

        public ActionResult EditProject(string ProjectID)
        {
            ProjectEditList projObj = new ProjectEditList();

            projObj.projComplete = projectObj.GetProjectEditDetails(Convert.ToInt64(ProjectID));
            //projObj.ShiftDetails = projectObj.ListShifts(Convert.ToInt64(ProjectID));
            //projObj.DesignationID = projectObj.GetProjectDesignationID(Convert.ToInt64(ProjectID));
            //var EmployeeIDs = projectObj.ListProjectEmployeesIDs(Convert.ToInt64(ProjectID));
            //var selected = from emp in EmployeeIDs select new[] { emp.EmpID };

            return View(projObj);
        }

        public JsonResult GetSelectedEmployee(Int64 ProjectID, int DesigID)
        {
            var GradeList = projectObj.ListProjectGrade(Convert.ToInt64(ProjectID), DesigID);
            var selected = from emp in GradeList select new[] { emp.GradeID }.ToArray();
            return Json(selected, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddNewShift(string ShiftList, Int64 ProjID)
        {
            var Shiftitems = JsonConvert.DeserializeObject<TblShift[]>(ShiftList);

            foreach (var sht in Shiftitems)
            {
                TblShift obj = new TblShift();
                obj.ShiftName = sht.ShiftName;
                obj.ShiftFrom = sht.ShiftFrom;
                obj.ShiftTo = sht.ShiftTo;
                obj.ShiftEmpNo = 0;
                obj.ShiftID = sht.ShiftID;
                obj.ProjID = ProjID;
                projectObj.Updateshift(obj);
            }
            return Json(true);
        }

        public JsonResult UpdateProject(TblProject projObj, string ProjFrom, string ProjTo, string ShiftList, string GradeList)
        {
            projObj.ProjFrom = common.CommonDateConvertion(ProjFrom);
            projObj.ProjTo = common.CommonDateConvertion(ProjTo);

            var Shiftitems = JsonConvert.DeserializeObject<TblShift[]>(ShiftList);
            var Gradeitems = JsonConvert.DeserializeObject<TblProjectGradeAssign[]>(GradeList);

            ProjectItems probj = projectObj.UpdateProject(projObj);

            if (probj.OutputID != 0)
            {
                foreach (var grd in Gradeitems)
                {
                    TblProjectGradeAssign Grobj = new TblProjectGradeAssign();
                    Grobj.GradeID = Convert.ToInt32(grd.GradeID);
                    Grobj.DesigID = Convert.ToInt32(grd.DesigID);
                    Grobj.PrjGradeEmpNO = Convert.ToInt32(grd.PrjGradeEmpNO);
                    Grobj.PrjGrdAssignID = Convert.ToInt32(grd.PrjGrdAssignID);
                    Grobj.PrjGrdAssgnStatus = true;
                    Grobj.ProjID = projObj.ProjID;
                    Grobj.ShiftID = grd.ShiftID;

                    projectObj.AddProjectGradeAssign(Grobj);
                }

                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }

        public string ToMonthName(DateTime dateTime)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dateTime.Month);
        }

        public JsonResult AddNewSchedule(int ShiftID = 0, Int64 ProjID = 0, string SchStartDate = null, string SchEndDate = null)
        {
            DateTime SchfrmDate = common.CommonDateConvertion(SchStartDate);
            DateTime SchToDate = common.CommonDateConvertion(SchEndDate);

            var MonthName = ToMonthName(SchfrmDate);
            var Startday = SchfrmDate.Day;
            var EndDay = SchToDate.Day;
            int MonthID = SchfrmDate.Month;

            var Schedule = MonthName + " " + Startday + " - " + EndDay;

            var IResult = projectObj.SaveShiftSchedule(ShiftID, ProjID, SchfrmDate, SchToDate, Schedule, MonthID);

            return Json(IResult, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult AddnewEmpShifts(string ShiftId, string EmpIDs, string ScheduleID, string ProjectID)
        //{
        //    var sCheduleIDs = CommonFunctions.DeserializeToList<string>(ScheduleID);
        //    var EmployeeID = CommonFunctions.DeserializeToList<string>(EmpIDs);
        //    ProjectItems project = new ProjectItems();

        //    for (int i = 0; i < sCheduleIDs.Count(); i++)
        //    {
        //        for (int e = 0; e < EmployeeID.Count(); e++)
        //        {
        //            ProjectmasterID probj = new ProjectmasterID();
        //            probj.EmpID = Convert.ToInt64(EmployeeID[e]);
        //            probj.ProjID = Convert.ToInt64(ProjectID);
        //            probj.SchID = Convert.ToInt32(sCheduleIDs[i]);
        //            probj.ShiftID = Convert.ToInt32(ShiftId);

        //            WeekDates wk = projectObj.GetScheduleDate(probj.SchID);

        //            var DateDiffe = (wk.EndDate - wk.StartDate).TotalDays;

        //            TblWeek weekObj = new TblWeek();
        //            weekObj.SchID = probj.SchID;
        //            weekObj.ShiftID = probj.ShiftID;
        //            weekObj.EmpID = probj.EmpID;
        //            weekObj.Wsun = wk.StartDate;
        //            weekObj.ProjID = Convert.ToInt64(ProjectID);

        //            for (int w = 0; w < DateDiffe; w++)
        //            {
        //                switch (w)
        //                {
        //                    case 0:
        //                        weekObj.WMon = wk.StartDate.AddDays(1);
        //                        break;

        //                    case 1:
        //                        weekObj.Wtue = weekObj.WMon.AddDays(1);
        //                        break;

        //                    case 2:
        //                        weekObj.Wwed = weekObj.Wtue.AddDays(1);
        //                        break;

        //                    case 3:
        //                        weekObj.Wthu = weekObj.Wwed.AddDays(1);
        //                        break;

        //                    case 4:
        //                        weekObj.Wfri = weekObj.Wthu.AddDays(1);
        //                        break;

        //                    case 5:
        //                        weekObj.Wsat = weekObj.Wfri.AddDays(1);
        //                        break;
        //                }

        //            }

        //            ProjectItems pr = projectObj.AddNewWeeks(weekObj);

        //            project = projectObj.AddNewEmpshift(probj,0);

        //        }

        //    }

        //    if (project.OutputID != 0)
        //    {
        //        return Json(true);
        //    }
        //    else
        //    {
        //        return Json(false);
        //    }
        //      //  return Json(true);
        //}

        //public JsonResult AddnewEmpShiftsSingle(string ShiftId, string EmpIDs, string ScheduleID, string ProjectID)
        //{
        //    if (EmpIDs != "null")
        //    {
        //        var EmployeeID = CommonFunctions.DeserializeToList<string>(EmpIDs);
        //        ProjectItems project = new ProjectItems();
        //        var temp = 0;
        //        var abc = 0;
        //        string itemarray = "";
        //        for (int e = 0; e < EmployeeID.Count(); e++)
        //        {
        //            temp += 1;
        //            ProjectmasterID probj = new ProjectmasterID();
        //            probj.EmpID = Convert.ToInt64(EmployeeID[e]);
        //            probj.ProjID = Convert.ToInt64(ProjectID);
        //            probj.SchID = Convert.ToInt32(ScheduleID);
        //            probj.ShiftID = Convert.ToInt32(ShiftId);

        //            WeekDates wk = projectObj.GetScheduleDate(probj.SchID);

        //            var DateDiffe = (wk.EndDate - wk.StartDate).TotalDays;

        //            TblWeek weekObj = new TblWeek();
        //            weekObj.SchID = probj.SchID;
        //            weekObj.ShiftID = probj.ShiftID;
        //            weekObj.EmpID = probj.EmpID;
        //            weekObj.Wsun = wk.StartDate;
        //            weekObj.ProjID = Convert.ToInt64(ProjectID);

        //            for (int w = 0; w < DateDiffe; w++)
        //            {
        //                switch (w)
        //                {
        //                    case 0:
        //                        weekObj.WMon = wk.StartDate.AddDays(1);
        //                        break;

        //                    case 1:
        //                        weekObj.Wtue = weekObj.WMon.AddDays(1);
        //                        break;

        //                    case 2:
        //                        weekObj.Wwed = weekObj.Wtue.AddDays(1);
        //                        break;

        //                    case 3:
        //                        weekObj.Wthu = weekObj.Wwed.AddDays(1);
        //                        break;

        //                    case 4:
        //                        weekObj.Wfri = weekObj.Wthu.AddDays(1);
        //                        break;

        //                    case 5:
        //                        weekObj.Wsat = weekObj.Wfri.AddDays(1);
        //                        break;
        //                }
        //            }

        //            ProjectItems pr = projectObj.AddNewWeeks(weekObj,temp);

        //            project = projectObj.AddNewEmpshift(probj, temp);

        //            if(project.OutputID == 2)
        //            {
        //                itemarray += project.OutName +  "," ;
        //                abc = 1;
        //            }
        //        }

        //        if (project.OutputID > 0)
        //        {
        //            projectObj.UpdateAttendance(Convert.ToInt32(ShiftId),Convert.ToInt64(ProjectID), Convert.ToInt32(ScheduleID));

        //            if (abc == 0)
        //            {
        //                return Json(new { Message = "Saved Successfully", Result = 1 }, JsonRequestBehavior.AllowGet);
        //            }
        //            else
        //            {
        //                return Json(new { Message = itemarray.ToString().Substring(0,itemarray.Length-1), Result = -1 }, JsonRequestBehavior.AllowGet);
        //            }
        //        }
        //        else
        //        {
        //            return Json(new { Message = "Saved failed", Result = 0 },JsonRequestBehavior.AllowGet);
        //        }
        //        //  return Json(true);
        //    }
        //    else
        //    {
        //        return Json(2 , JsonRequestBehavior.AllowGet);
        //    }
        //}

        public ActionResult GetCompansationType()
        {
            var Type = projectObj.GetCompansationType();
            return Json(Type, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult GetAssinedshiftEmployee(string SchID , string ProjID,string shiftid)
        // {
        //    //var EmployeeIDs = projectObj.ListScheduleEmployeesIDs(SchID, Convert.ToInt64(ProjID));
        //    //var selected = from emp in EmployeeIDs select new[] { emp.EmpID }.Distinct().ToArray();
        //    //return Json(selected, JsonRequestBehavior.AllowGet);

        //}

        public ActionResult ProjectSchedule(Int64 projectid, string projectname, string ProjFromDate, string ProjToDate)
        {
            ViewBag.projectid = projectid;
            ViewBag.projectname = projectname;
            ViewBag.ProjFromDate = ProjFromDate.Substring(0, 10);
            ViewBag.ProjToDate = ProjToDate.Substring(0, 10);

            return View();
        }

        public ActionResult GetEmpAssignGrade(int Desig_ID, Int64 ProjID = 0, int ShiftID = 0)
        {
            var IList = projectObj.GetEmpAssignGrade(Desig_ID, ProjID, ShiftID);
            return Json(IList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProjectEmployeeList(int GradeID = 0)
        {
            var IListEmp = projectObj.GetProjectEmployeeList(GradeID);
            return Json(IListEmp, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckEmpLeaveApply(Int64 EmpID = 0, string SchFromDate = null, string SchEndDate = null)
        {
            DateTime SchFrmDate = common.CommonDateConvertion(SchFromDate);
            DateTime SchEdDate = common.CommonDateConvertion(SchEndDate);

            var EmpCnt = projectObj.CheckEmpLeaveApply(EmpID, SchFrmDate.Date, SchEdDate.Date);
            return Json(EmpCnt.EmpCount, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveProjectAssign(TblShift shiftobj, string SchStartDate, string SchEndDate, string EmployeeList)
        {
            var items = JsonConvert.DeserializeObject<EmpProjectEmployeeList[]>(EmployeeList);
            //var OffDetails = EmpOffs.Replace("\"", "").Replace("[", "").Replace("]", "").Replace("null", "").Replace("{", "").Replace("}", "");
            //string[] Offs = OffDetails.Split(',');
            //  List<LeaveDate> LeaveObj = new List<LeaveDate>();

            var EmpCount = items.Length;
            foreach (var list in items)
            {
                ProjectmasterID probj = new ProjectmasterID();
                probj.EmpID = Convert.ToInt64(list.EmpID);
                probj.ProjID = Convert.ToInt64(shiftobj.ProjID);
                probj.SchID = Convert.ToInt32(list.SchID);
                probj.ShiftID = Convert.ToInt32(list.ShiftID);
                var IItemResult = projectObj.AddNewEmpshift(probj);
                //---Employee Offdays
                //if(list.EmpOffs != null)
                //{
                //    for(int i=0;i< list.EmpOffs.Count();i++)
                //    {
                //       if(list.EmpOffs[i]!="0")
                //            projectObj.UpdateOptionalOffDays(list.EmpID, common.CommonDateConvertion(list.EmpOffs[i]));
                //    }
                //}
                //------------------------------
            }

            return Json(1, JsonRequestBehavior.AllowGet);
        }

        //---Employee Offdays
        public void SaveOptionalRestDays(Int64 EmpID, string[] EmpOFFs = null, string OffDates = null)
        {
            if (EmpOFFs != null)
            {
                for (int i = 0; i < EmpOFFs.Length; i++)
                {
                    if (EmpOFFs[i] != "0")
                        projectObj.UpdateOptionalOffDays(EmpID, common.CommonDateConvertion(EmpOFFs[i]));
                }
            }
            if (OffDates != "" && OffDates != null)
            {
                var Dates = OffDates.Replace("\"", "").Replace("[", "").Replace("]", "").Replace("null", "");
                string[] DateArray = Dates.Split(',');
                for (int i = 0; i < DateArray.Length; i++)
                {
                    if (i == 0)
                        projectObj.DeleteRestDays(EmpID);

                    if (DateArray[i] != "0")
                        projectObj.UpdateOptionalOffDays(EmpID, common.CommonDateConvertion(DateArray[i]));
                }
            }
        }

        public ActionResult ProjectLeaveEmpDetails(Int64 EmpID = 0, string SchFromDate = null, string SchToDate = null, int GradeID = 0, int OffStatus = 0)
        {
            DateTime SchfrmDate = common.CommonDateConvertion(SchFromDate);
            DateTime SchEndDate = common.CommonDateConvertion(SchToDate);

            ViewBag.StartDate = SchFromDate;
            ViewBag.EndDate = SchToDate;
            ViewBag.EmpID = EmpID;

            RelifeEmpDetails obj = new RelifeEmpDetails();
            obj.PrjLeveobj = projectObj.GetProjectLeaveEmpDetails(EmpID, SchfrmDate.Date, SchEndDate.Date);
            obj.PrjEmpRelifeObj = projectObj.GetRelifeEmployeeList(EmpID);
            obj.Absencealloobj = projectObj.GetAbscenceAllocateList();
            if (OffStatus > 0)
            {
                ViewBag.OFFDetails = projectObj.GetOFFDates(EmpID, SchfrmDate.Date, SchEndDate.Date);
            }

            return View(obj);
        }

        public JsonResult SaveReliefEmployee(string EmpLeaveDate = null, string RelifeEmpName = null, string AbsEmpID = null, string ProjID = null, byte ReliefConfirmation = 0)
        {
            var Iresult = 0;
            //for (int i = 0; i < EmpLeaveDate.Length; i++)
            //{
                if (RelifeEmpName != null)
                {
                    TblAbscenceAllocate obj = new TblAbscenceAllocate();

                    obj.AbsEmpID = Convert.ToInt64(AbsEmpID);
                    obj.AbsallEmpID = Convert.ToInt64(RelifeEmpName);
                    obj.AbsDateFrom = common.CommonDateConvertion(EmpLeaveDate);
                    obj.AbsDateTo = common.CommonDateConvertion(EmpLeaveDate);
                    obj.ProjID = Convert.ToInt64(ProjID);
                    obj.SchID = 0;
                    obj.ShiftID = 0;
                    obj.ReliefConfirmation = Convert.ToBoolean(ReliefConfirmation);
                    Iresult = projectObj.SaveReliefEmployee(obj);
                }
            //}

            return Json(Iresult, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCalendarEvents(Int64 ProjID)
        {
            var List = projectObj.GetCalendarEvents(ProjID);

            if (List != null)
            {
                var eventList = from e in List
                                select new
                                {
                                    id = e.ShiftID,
                                    schid = e.SchID,
                                    title = e.ShiftName,
                                    start = e.StartDate.ToString(),
                                    end = e.EndDate.ToString(),
                                    color = e.Color,
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

        public JsonResult GetProject()
        {
            var list = projectObj.GetProject(0);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ProjectAttendance(Int64 ProjID = 0, int DefaultStatus = 0, string AbsDateFrom = null)
        {
            ViewBag.ProjID = ProjID;
            if (DefaultStatus != 0)
                ViewBag.CurrentDate = DateTime.Now.Date;
            else
                ViewBag.CurrentDate = common.CommonDateConvertion(AbsDateFrom);

            return View();
        }

        public ActionResult ListProjectWiseEmp(Int64 ProjID = 0, string TodayDate = null, Int32 ShiftID = 0)
        {

            if (TodayDate == null)
            {
                TodayDate = DateTime.Now.ToString("dd/MM/yyyy");
            }

            ProjectComponentList obj = new ProjectComponentList();
            obj.ProjObj = projectObj.GetProject(ProjID);
            obj.ProjList = projectObj.GetListProjectWiseEmp(common.CommonDateConvertion(TodayDate).Date,ProjID, ShiftID);
            obj.EmpReliefConfirm = projectObj.GetReliefEmpConfirmList();            

            obj.EmpAbsnceAllcateobj = projectObj.GetEmpAbsnceAllcatelist(common.CommonDateConvertion(TodayDate).Date);
            obj.GetAttendanceobj = projectObj.GetAttandanceProjectList(ProjID, common.CommonDateConvertion(TodayDate).Date);
            // obj.GetEmployeeleaves = projectObj.GetEMployeeLeave();
            ViewBag.Date = TodayDate;
            ViewBag.LeaveDate = common.CommonDateConvertion(TodayDate).Date;

            return View(obj);
        }

        public ActionResult SaveProjectAttandance(Int64 ProjID = 0, string[] EmpID = null, string[] ShiftFrom = null, string[] ShiftTo = null, string[] RelifeEmpName = null, string StartDate = null)
        {
            var Iresult = 0;

            if (EmpID != null)
            {
                for (int j = 0; j < EmpID.Length; j++)
                {
                    TblAttendance obj = new TblAttendance();

                    obj.EmpID = Convert.ToInt64(EmpID[j]);

                    DateTime Attdate = common.CommonDateConvertion(StartDate);
                    // bool offday = projectObj.EmployeeOFFday(obj.EmpID, Convert.ToDateTime(Attdate));
                    bool offday = projectObj.GetOffdayStatus(obj.EmpID, Attdate);
                    bool Holiday = projectObj.EmployeeHoliday(obj.EmpID, Attdate);

                    obj.AttendtimeIN = ShiftFrom[j];
                    obj.AttendtimeOut = ShiftTo[j];
                    obj.Attenddate = Attdate.Date;
                    obj.ProjID = Convert.ToInt64(ProjID);
                    obj.AttendHoliStatus = Holiday;
                    obj.IsOFFDay = offday;
                    Iresult = projectObj.SaveProjectAttandance(obj);
                }
            }
            else
            {
                Iresult = -1;
            }
            return Json(Iresult, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ProjectEditShiftGradeList(Int64 ProjID)
        {
            ProjectShiftGradeList obj = new ProjectShiftGradeList();
            obj.ShiftObj = projectObj.GetProjectShiftList(ProjID);
            obj.PrjgrdAssignobj = projectObj.GetProjectGradeAssignList(ProjID);

            return Json(new { shiftdata = obj.ShiftObj, gradedata = obj.PrjgrdAssignobj }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProjectShift(Int64 ProjID)
        {
            var Ilist = projectObj.GetProjectShiftList(ProjID);

            return Json(Ilist, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteShift(int ShiftID)
        {
            var IList = projectObj.DeleteProjectShift(ShiftID);
            return Json(IList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteGrade(int PrjGrdAssignID)
        {
            var Iresult = projectObj.DeleteProjectGradeAssign(PrjGrdAssignID);
            return Json(Iresult, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ManageAbsenceAllocation(Int64 ProjID, Int64 AbsEmpID, Int64 AbsallEmpID, string AbsDateFrom, string EmpIDArray , byte ReliefConfirmation)
        {
            DateTime Dates = common.CommonDateConvertion(AbsDateFrom);

            var EmpIds = (EmpIDArray.Replace("\"", "").Replace("[", "").Replace("]", "").Replace("null", "")).Split(',');
            Int64[] Emps = Array.ConvertAll(EmpIds, s => Int64.Parse(s));
            if (Emps.Contains(AbsallEmpID))
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
            else
            {
                TblAbscenceAllocate objd = new TblAbscenceAllocate();
                objd.AbsEmpID = AbsEmpID;
                objd.AbsallEmpID = AbsallEmpID;
                objd.AbsDateFrom = Dates.Date;
                objd.ProjID = ProjID;
                objd.ReliefConfirmation =Convert.ToBoolean(ReliefConfirmation);

                var Iresult = projectObj.ManageAbsenceAllocation(objd);
                return Json(Iresult, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateManageAbsenceAllocation(Int64 ProjID, Int64 AbsEmpID, Int64 AbsallEmpID, string AbsDateFrom, byte ReliefConfirmation)
        {
            DateTime Dates = common.CommonDateConvertion(AbsDateFrom);

            
            TblAbscenceAllocate objd = new TblAbscenceAllocate();
            objd.AbsEmpID = AbsEmpID;
            objd.AbsallEmpID = AbsallEmpID;
            objd.AbsDateFrom = Dates.Date;
            objd.ProjID = ProjID;
            objd.ReliefConfirmation = Convert.ToBoolean(ReliefConfirmation);

            var Iresult = projectObj.ManageAbsenceAllocation(objd);
            return Json(Iresult, JsonRequestBehavior.AllowGet);
             
        }

        public JsonResult GetScheduleList(int ShiftID, Int64 ProjID)
        {
            var ScheduleList = projectObj.GetScheduleList(ShiftID, ProjID);

            return Json(ScheduleList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEditProjectSchedule(Int64 ProjID, int SchID, int ShiftID)
        {
            var IresultEditdetails = projectObj.GetEditProjectSchedule(ProjID, SchID, ShiftID);
            return Json(IresultEditdetails, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteEmpShiftEmployeeAssign(Int64 ProjID, int ShiftID, int SchID, Int64 EmpID, string SchFromDate = null, string SchToDate = null, string flg = null)
        {
            var confirmaton = projectObj.CounttblAttendance(SchID, EmpID);
            if (confirmaton == 1 && flg == null)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var IlistDeleted = projectObj.DeleteEmpShiftEmployeeAssign(ProjID, ShiftID, SchID, EmpID, SchFromDate, SchToDate, flg);
                return Json(IlistDeleted, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ScheduleDetails(int scheduleid)
        {
            ViewBag.schedule = scheduleid;
            var Details = projectObj.GetScheduleEmps(scheduleid);
            return View(Details);
        }

        public int DeleteSchedule(int schID, string flg)
        {
            int count = projectObj.GetStartandEndDate(schID);

            if (count == 0 && flg == null)
            {
                return 0;
            }
            else
            {
                return projectObj.DeleteProjectSchedule(schID, flg);
            }
        }

        public ActionResult ShiftSchedule(int projectid)
        {
            projectCalendar calendar = projectObj.GetProjectShiftschedule(projectid);
            return View(calendar);
        }

        public ActionResult SHiftDetails(int shiftid, int projectid)
        {
            projectCalendar calendar = projectObj.GetProjectShiftandschedule(projectid, shiftid);
            return View(calendar);
        }

        public int DeleteAllShift(int shift, string flg)
        {
            int count = projectObj.GetCountinAttendance(shift);

            if (count == 0 && flg == null)
            {
                return 0;
            }
            else
            {
                return projectObj.DeleteShift(shift, flg);
            }
        }

        public int RemoveAttandance(string Empid, string StartDate)
        {
            var EmpId = Empid.Replace("\"", "").Replace("[", "").Replace("]", "").Replace("null", "");
            string[] RemoveEmpId = EmpId.Split(',');
            DateTime Attdate = common.CommonDateConvertion(StartDate);
            for (int i = 0; i < RemoveEmpId.Count(); i++)
            {
                projectObj.RemoveFromAttandance(Convert.ToInt64(RemoveEmpId[i]), Attdate);
            }
            return 1;
        }

        public int GetLeaveStat(Int64 EmpId, string date)
        {
            var leave = projectObj.GetLeaveStatus(EmpId, common.CommonDateConvertion(date));
            //var offday = projectObj.GetOffdayStatus(EmpId, common.CommonDateConvertion(date));
            // return Json(new { ResultLeave = leave, ResultOff = offday }, JsonRequestBehavior.AllowGet);
            return leave;
        }

        public string GetLeaveTypeName(Int64 EmpID, string date)
        {
            return projectObj.GetLeaveTypeName(EmpID, common.CommonDateConvertion(date));
        }

        public ActionResult GetDates(string StartDate, string EndDate)
        {
            List<LeaveDate> LeaveObj = new List<LeaveDate>();

            DateTime Sdate = common.CommonDateConvertion(StartDate);
            DateTime Edate = common.CommonDateConvertion(EndDate);
            var Datediff = (Edate - Sdate).TotalDays;
            for (int i = 0; i <= Datediff; i++)
            {
                LeaveDate obj = new LeaveDate();
                obj.Leavedate = Sdate;
                Sdate = Sdate.AddDays(1);

                LeaveObj.Add(obj);
            }
            var iitem = from s in LeaveObj select new SelectListItem { Text = s.Leavedate.ToString("dd/MM/yyyy"), Value = s.Leavedate.ToString("dd/MM/yyyy") };
            return Json(iitem.ToArray(), JsonRequestBehavior.AllowGet);
        }

        public Int16 EmpOffStatus(Int64 EmpID)
        {
            Int16 Offstat = projectObj.EmpOffStatus(EmpID);
            return Offstat;
        }

        public ActionResult GetShifts(Int64 ProjID)
        {
            var shifts = projectObj.GetShifts(ProjID);
            return Json(shifts, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOffDetails(Int64 EmpID)
        {
            var details = projectObj.GetOffDetails(EmpID);
            return Json(details, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetScheduleWiseOffDayStatus(Int64 EmpID, string SchFromDate, string SchEndDate, Int16 FixedDay)
        {
            DateTime SchFrmDate = common.CommonDateConvertion(SchFromDate);
            DateTime SchEdDate = common.CommonDateConvertion(SchEndDate);

            var EmpCnt = projectObj.GetScheduleWiseOffDayStatus(EmpID, SchFrmDate.Date, SchEdDate.Date, FixedDay);
            return Json(EmpCnt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSavedOffDates(Int64 EmpID, string StartDate, string EndDate)
        {
            DateTime Sdate = common.CommonDateConvertion(StartDate);
            DateTime Edate = common.CommonDateConvertion(EndDate);

            var Dates = projectObj.GetSavedOffDates(EmpID, Sdate, Edate);
            var SelectedItems = from a in Dates select new[] { a.OFFDates.ToString("dd/MM/yyyy") }.ToArray();
            return Json(SelectedItems, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RemoveAbsenceAllocation(Int64 AbsentEmp, Int64 AllocateEmp, string Date)
        {
            int result = projectObj.RemoveAbsenceAllocation(AbsentEmp, AllocateEmp, common.CommonDateConvertion(Date));
            return Json(result);
        }

    }
}