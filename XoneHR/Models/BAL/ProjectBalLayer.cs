using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Dapper;
using System.Data.SqlClient;

namespace XoneHR.Models.BAL
{
    public class ProjectBalLayer
    {
        private CommonFunctions common = new CommonFunctions();
        private XoneDbLayer db;

        public ProjectBalLayer()
        {
            db = new XoneDbLayer();
        }

        public ProjectItems AddNewProject(TblProject ProjectObj)
        {
            try
            {
                DynamicParameters paraobj = new DynamicParameters();
                paraobj.Add("@ProjName", ProjectObj.ProjName);
                paraobj.Add("@ProjFrom", ProjectObj.ProjFrom);
                paraobj.Add("@ProjTo", ProjectObj.ProjTo);
                paraobj.Add("@ProjLocation", ProjectObj.ProjLocation);
                paraobj.Add("@projTypID", ProjectObj.projTypID);
                paraobj.Add("@ProjEmpno", 0);
                paraobj.Add("@ProcompTypID", ProjectObj.ProcompTypID);
                paraobj.Add("@ProjCompAmount", ProjectObj.ProjCompAmount);

                paraobj.Add("@ProjectId", null, DbType.Int64, ParameterDirection.Output);
                paraobj.Add("@ErrorOutput", null, DbType.Int32, ParameterDirection.Output);

                int Result = db.DapperExecute("USP_AddNewProject", paraobj, CommandType.StoredProcedure);

                ProjectItems projctObj = new ProjectItems();
                projctObj.TableID = paraobj.Get<Int64>("ProjectId");
                projctObj.OutputID = paraobj.Get<Int32>("ErrorOutput");
                return projctObj;
            }
            catch
            {
                return null;
            }
        }

        public ProjectItems AddProjectGradeAssign(TblProjectGradeAssign obj)
        {
            try
            {
                DynamicParameters paraobj = new DynamicParameters();
                paraobj.Add("@DesigID", obj.DesigID);
                paraobj.Add("@GradeID", obj.GradeID);
                paraobj.Add("@ProjID", obj.ProjID);
                paraobj.Add("@PrjGradeAssgnStatus", obj.PrjGrdAssgnStatus);
                paraobj.Add("@PrjGradeEmpNO", obj.PrjGradeEmpNO);
                paraobj.Add("@PrjGrdAssignID", obj.PrjGrdAssignID);
                paraobj.Add("@ShiftID", obj.ShiftID);
                paraobj.Add("@ProjectId", null, DbType.Int64, ParameterDirection.Output);
                paraobj.Add("@ErrorOutput", null, DbType.Int32, ParameterDirection.Output);
                int Result = db.DapperExecute("USP_AddProjectGradeAssign", paraobj, CommandType.StoredProcedure);

                ProjectItems projctObj = new ProjectItems();
                projctObj.OutputID = paraobj.Get<Int32>("ErrorOutput");
                return projctObj;
            }
            catch
            {
                return null;
            }
        }

        public ProjectItems AddNewshift(TblShift ShiftObj)
        {
            try
            {
                DynamicParameters paraobj = new DynamicParameters();
                paraobj.Add("@ShiftName", ShiftObj.ShiftName);
                paraobj.Add("@ShiftFrom", ShiftObj.ShiftFrom);
                paraobj.Add("@ShiftTo", ShiftObj.ShiftTo);
                paraobj.Add("@ShiftEmpNo", ShiftObj.ShiftEmpNo);
                paraobj.Add("@ProjID", ShiftObj.ProjID);

                paraobj.Add("@ErrorOutput", null, DbType.Int32, ParameterDirection.Output);
                paraobj.Add("@ShiftID", null, DbType.Int32, ParameterDirection.Output);

                int Result = db.DapperExecute("USP_AddNewShift", paraobj, CommandType.StoredProcedure);

                ProjectItems projctObj = new ProjectItems();
                projctObj.OutputID = paraobj.Get<Int32>("ErrorOutput");
                projctObj.TableID = paraobj.Get<Int32>("ShiftID");
                return projctObj;
            }
            catch
            {
                return null;
            }
        }

        //public ProjectItems AddNewEmpProject(ProjectmasterID ProjmasterObj)
        //{
        //    try
        //    {
        //        DynamicParameters paraobj = new DynamicParameters();
        //        paraobj.Add("@EmpID", ProjmasterObj.EmpID);
        //        paraobj.Add("@ProjID", ProjmasterObj.ProjID);

        //        paraobj.Add("@ErrorOutput", null, DbType.Int32, ParameterDirection.Output);
        //        int Result = db.DapperExecute("USP_AddNewEmpProject", paraobj, CommandType.StoredProcedure);

        //        ProjectItems projctObj = new ProjectItems();
        //        projctObj.OutputID = paraobj.Get<Int32>("ErrorOutput");
        //        return projctObj;

        //    }
        //    catch
        //    {
        //        return null;

        //    }

        //}

        public ProjectItems AddNewEmpshift(ProjectmasterID ProjmasterObj)
        {
            try
            {
                DynamicParameters paraobj = new DynamicParameters();
                paraobj.Add("@ShiftID", ProjmasterObj.ShiftID);
                paraobj.Add("@EmpID", ProjmasterObj.EmpID);
                paraobj.Add("@SchID", ProjmasterObj.SchID);
                paraobj.Add("@ProjID", ProjmasterObj.ProjID);
                paraobj.Add("@ErrorOutput", null, DbType.Int32, ParameterDirection.Output);
                int Result = db.DapperExecute("USP_AddNewEmpShift", paraobj, CommandType.StoredProcedure);

                ProjectItems projctObj = new ProjectItems();
                projctObj.OutputID = paraobj.Get<Int32>("ErrorOutput");
                return projctObj;
            }
            catch
            {
                return null;
            }
        }

        public void UpdateAttendance(Int32 ShiftId, Int64 ProjectID, Int32 ScheduleID)
        {
            DynamicParameters para = new DynamicParameters();
            para.Add("@ShiftId", ShiftId);
            para.Add("@ProjectID", ProjectID);
            para.Add("@ScheduleID", ScheduleID);

            db.DapperExecute("USP_UpdateAttendanceTable", para, CommandType.StoredProcedure);
        }

        public ProjectItems UpdateProject(TblProject ProjectObj)
        {
            try
            {
                DynamicParameters paraobj = new DynamicParameters();
                paraobj.Add("@ProjName", ProjectObj.ProjName);
                paraobj.Add("@ProjFrom", ProjectObj.ProjFrom);
                paraobj.Add("@ProjTo", ProjectObj.ProjTo);
                paraobj.Add("@ProjLocation", ProjectObj.ProjLocation);
                paraobj.Add("@projTypID", ProjectObj.projTypID);
                paraobj.Add("@ProjEmpno", 0);
                paraobj.Add("@ProjectId", ProjectObj.ProjID);
                paraobj.Add("@ProcompTypID", ProjectObj.ProcompTypID);
                paraobj.Add("@ProjCompAmount", ProjectObj.ProjCompAmount);

                paraobj.Add("@ErrorOutput", null, DbType.Int32, ParameterDirection.Output);

                int Result = db.DapperExecute("USP_UpdateProject", paraobj, CommandType.StoredProcedure);

                ProjectItems projctObj = new ProjectItems();
                projctObj.OutputID = paraobj.Get<Int32>("ErrorOutput");
                return projctObj;
            }
            catch
            {
                return null;
            }
        }

        public ProjectItems Updateshift(TblShift ShiftObj)
        {
            try
            {
                DynamicParameters paraobj = new DynamicParameters();
                paraobj.Add("@ShiftName", ShiftObj.ShiftName);
                paraobj.Add("@ShiftFrom", ShiftObj.ShiftFrom);
                paraobj.Add("@ShiftTo", ShiftObj.ShiftTo);
                paraobj.Add("@ShiftEmpNo", ShiftObj.ShiftEmpNo);
                paraobj.Add("@ProjID", ShiftObj.ProjID);
                paraobj.Add("@ShiftID", ShiftObj.ShiftID);

                paraobj.Add("@ErrorOutput", null, DbType.Int32, ParameterDirection.Output);
                paraobj.Add("@shtID", null, DbType.Int32, ParameterDirection.Output);
                int Result = db.DapperExecute("USP_UpdateShift", paraobj, CommandType.StoredProcedure);

                ProjectItems projctObj = new ProjectItems();
                projctObj.OutputID = paraobj.Get<Int32>("ErrorOutput");
                projctObj.TableID = paraobj.Get<Int32>("shtID");
                return projctObj;
            }
            catch
            {
                return null;
            }
        }

        public int DeleteProjectGradeAssign(int PrjGrdAssignID)
        {
            DynamicParameters paraobj = new DynamicParameters();
            paraobj.Add("@PrjGrdAssignID", PrjGrdAssignID);
            int result = db.DapperExecute("delete from TblProjectGradeAssign where PrjGrdAssignID = @PrjGrdAssignID", paraobj);
            return 1;
        }

        //public ProjectItems UpdateEmpProject(ProjectmasterID ProjmasterObj, int temp)
        //{
        //    try
        //    {
        //        DynamicParameters paraobj = new DynamicParameters();
        //        paraobj.Add("@EmpID", ProjmasterObj.EmpID);
        //        paraobj.Add("@ProjID", ProjmasterObj.ProjID);
        //        paraobj.Add("@Temp", temp);
        //        paraobj.Add("@ErrorOutput", null, DbType.Int32, ParameterDirection.Output);
        //        int Result = db.DapperExecute("USP_UpdateEmpProject", paraobj, CommandType.StoredProcedure);

        //        ProjectItems projctObj = new ProjectItems();
        //        projctObj.OutputID = paraobj.Get<Int32>("ErrorOutput");
        //        return projctObj;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        //public ProjectItems AddNewSchedule(int flag, string schedule, int monthID, Int64 projectid, DateTime startdate, DateTime enddate)
        //{
        //    try
        //    {
        //        DynamicParameters paraObj = new DynamicParameters();
        //        paraObj.Add("@flag", flag);
        //        paraObj.Add("@schedule", schedule);
        //        paraObj.Add("@monthID", monthID);
        //        paraObj.Add("@projectId", projectid);
        //        paraObj.Add("@startdate", startdate);
        //        paraObj.Add("@endate", enddate);
        //        paraObj.Add("@ErrorOutput", null, DbType.Int32, ParameterDirection.Output);
        //        int Result = db.DapperExecute("USP_AddNewSchedule", paraObj, CommandType.StoredProcedure);

        //        ProjectItems projctObj = new ProjectItems();
        //        projctObj.OutputID = paraObj.Get<Int32>("ErrorOutput");
        //        return projctObj;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        //public ProjectItems AddNewWeeks(TblWeek weekobj, int Temp = 0)
        //{
        //    try
        //    {
        //        DynamicParameters paraObj = new DynamicParameters();
        //        paraObj.Add("@SchID", weekobj.SchID);
        //        paraObj.Add("@ShiftID", weekobj.ShiftID);
        //        paraObj.Add("@EmpID", weekobj.EmpID);
        //        paraObj.Add("@Wsun", weekobj.Wsun);
        //        paraObj.Add("@WMon", weekobj.WMon);
        //        paraObj.Add("@Wtue", weekobj.Wtue);
        //        paraObj.Add("@Wwed", weekobj.Wwed);
        //        paraObj.Add("@Wthu", weekobj.Wthu);
        //        paraObj.Add("@Wfri", weekobj.Wfri);
        //        paraObj.Add("@Wsat", weekobj.Wsat);
        //        paraObj.Add("@ProjID", weekobj.ProjID);
        //        paraObj.Add("@Temp", Temp);

        //        paraObj.Add("@ErrorOutput", null, DbType.Int32, ParameterDirection.Output);
        //        int Result = db.DapperExecute("USP_AddWeeks", paraObj, CommandType.StoredProcedure);

        //        ProjectItems projctObj = new ProjectItems();
        //        projctObj.OutputID = paraObj.Get<Int32>("ErrorOutput");
        //        return projctObj;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        public List<ProjectDetails> ListProjectDatatable()
        {
            try
            {
                return db.DapperToList<ProjectDetails>("USP_ListprojectsDatatable", CommandType.StoredProcedure);
            }
            catch
            {
                return null;
            }
        }

        public List<Projecttype> ComboListprojectType()
        {
            try
            {
                return db.DapperToList<Projecttype>("USP_CombolistProjectType", CommandType.StoredProcedure);
            }
            catch
            {
                return null;
            }
        }

        public List<EmployeeProfile> ComboListEmployee(Int64 ProjID, Int16 Flag)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@ProjID", ProjID);
                param.Add("@FLAG", Flag);
                return db.DapperToList<EmployeeProfile>("USP_CombolistEmployee", param, CommandType.StoredProcedure);
            }
            catch
            {
                return null;
            }
        }

        public ProjectDetails GetProjectDetails(Int64 ProjectID)
        {
            try
            {
                DynamicParameters paraObj = new DynamicParameters();
                paraObj.Add("@ProjectId", ProjectID);

                return db.DapperFirst<ProjectDetails>("USP_ListProjectDetails", paraObj, CommandType.StoredProcedure);
            }
            catch
            {
                return null;
            }
        }

        public List<ProjectSchedule> ListprojectSchedule(Int64 ProjectID, int monthCode)
        {
            try
            {
                DynamicParameters para = new DynamicParameters();
                para.Add("@ProjectId", ProjectID);
                para.Add("@MonthCode", monthCode);

                return db.DapperToList<ProjectSchedule>("USP_ListProjectSchedule", para, CommandType.StoredProcedure);
            }
            catch
            {
                return null;
            }
        }

        public List<ProjectSchedule> ListprojectAssignSchedule(Int64 ProjectID, int monthCode)
        {
            try
            {
                DynamicParameters para = new DynamicParameters();
                para.Add("@ProjectId", ProjectID);
                para.Add("@MonthCode", monthCode);

                return db.DapperToList<ProjectSchedule>("USP_ListAssinedSchedules", para, CommandType.StoredProcedure);
            }
            catch
            {
                return null;
            }
        }

        public List<MonthsMaster> CombolistMonths()
        {
            try
            {
                return db.DapperToList<MonthsMaster>("USP_ComboListMonths", CommandType.StoredProcedure);
            }
            catch
            {
                return null;
            }
        }

        public List<TblShift> ListShifts(Int64 ProjectID)
        {
            try
            {
                DynamicParameters para = new DynamicParameters();
                para.Add("@ProjectId", ProjectID);

                return db.DapperToList<TblShift>("USP_ListShifts", para, CommandType.StoredProcedure);
            }
            catch
            {
                return null;
            }
        }

        public List<EmployeeProfile> ListShiftsEmployees(Int64 ProjectID)
        {
            try
            {
                DynamicParameters para = new DynamicParameters();
                para.Add("@ProjectId", ProjectID);

                return db.DapperToList<EmployeeProfile>("USP_ListShiftsEmployees", para, CommandType.StoredProcedure);
            }
            catch
            {
                return null;
            }
        }

        public ProjectCompleteDetails GetProjectEditDetails(Int64 ProjectID)
        {
            try
            {
                DynamicParameters paraObj = new DynamicParameters();
                paraObj.Add("@ProjectId", ProjectID);

                return db.DapperFirst<ProjectCompleteDetails>("USP_ListProjectDetails", paraObj, CommandType.StoredProcedure);
            }
            catch
            {
                return null;
            }
        }

        public List<TblProjectGradeAssign> ListProjectGrade(Int64 ProjectID, int DesigID)
        {
            try
            {
                DynamicParameters para = new DynamicParameters();
                para.Add("@ProjectID", ProjectID);
                para.Add("@DesigID", DesigID);

                return db.DapperToList<TblProjectGradeAssign>("select * from TblProjectGradeAssign where ProjID = @ProjectID and PrjGrdAssgnStatus = 1 and DesigID = @DesigID", para);
            }
            catch
            {
                return null;
            }
        }

        public List<ProjectmasterID> ListScheduleEmployeesIDs(string Projectid)
        {
            try
            {
                DynamicParameters para = new DynamicParameters();
                para.Add("@Projectid", Convert.ToInt32(Projectid));

                var SelectedEmp = db.DapperToList<ProjectmasterID>("select * from TblEmpShift where ProjID=@Projectid", para);
                return SelectedEmp;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //public WeekDates GetScheduleDate(int schid)
        //{
        //    try
        //    {
        //        DynamicParameters para = new DynamicParameters();
        //        para.Add("@schduleID", schid);

        //        return db.DapperFirst<WeekDates>("USP_GetScheduleDates", para, CommandType.StoredProcedure);
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        public List<TblProjectCompenseType> GetCompansationType()
        {
            var type = db.DapperToList<TblProjectCompenseType>("Select *from TblProjectCompenseType");
            return type;
        }

        public List<RosterscheduleList> ListRosterSchedule(Int64 projectID)
        {
            try
            {
                DynamicParameters paraobj = new DynamicParameters();
                paraobj.Add("@projid", projectID);

                var ListSchedules = db.DapperToList<RosterscheduleList>("select distinct empshft.SchID,empshft.ShiftID,schd.Schedule from TblEmpShift empshft join Tblschedule schd on schd.SchID=empshft.SchID where empshft.ProjID=@projid", paraobj);
                return ListSchedules;
            }
            catch
            {
                return null;
            }
        }

        //public List<Dateschedule> GetDateSchedule(DateTime FromDate, DateTime ToDate)
        //{
        //    DynamicParameters param = new DynamicParameters();
        //    param.Add("@FromDate", FromDate);
        //    param.Add("@ToDate", ToDate);
        //    var data = db.DapperToList<Dateschedule>("USP_DateWiseScheduleDetails", param, CommandType.StoredProcedure);
        //    return data;
        //}

        public TblProjectGradeAssign GetProjectDesignationID(Int64 ProjectID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@ProjectID", ProjectID);

            var cnt = db.DapperFirst<TblProjectGradeAssign>("select DesigID from TblProjectGradeAssign where ProjID = @ProjectID and PrjGrdAssgnStatus = 1", param);

            return cnt;
        }

        public int UpdateProjectStatus(Int64 ProjectID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@ProjectID", ProjectID);
            db.DapperExecute("Update TblProject set ProjStatus=1 where ProjID=@ProjectID", param);
            return 1;
        }

        public List<TblDesignation> GetDesignation()
        {
            return db.DapperToList<TblDesignation>("select * from TblDesignation as a where a.DesigStatus = 1 and a.DesigUserTyp = 1");
        }

        public List<TblGrade> GetGrade(int DeisgID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@DeisgID", DeisgID);

            return db.DapperToList<TblGrade>("select * from TblGrade as a where a.GradeStatus = 1 and a.DesigID = @DeisgID", param);
        }

        public List<TblGrade> GetEmpAssignGrade(int Desig_ID, Int64 ProjID, int ShiftID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@ProjID", ProjID);
            param.Add("@Desig_ID", Desig_ID);
            param.Add("@ShiftID", ShiftID);

            return db.DapperToList<TblGrade>("select  distinct(a.GradeID),a.DesigID,a.Gradename, b.PrjGradeEmpNO from TblGrade as a join TblProjectGradeAssign as b on a.GradeID=b.GradeID where b.ProjID = @ProjID and b.DesigID = @Desig_ID and a.GradeStatus=1 and b.PrjGrdAssgnStatus=1 and b.ShiftID=@ShiftID", param);
        }

        public List<Employees> GetProjectEmployeeList(int GradeID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@GradeID", GradeID);

            return db.DapperToList<Employees>("select * from TblEmployee as a join TblCandidate as b  on a.CandID=b.CandID left join TblEmployeeSubType as c on a.EmpSubTypeID = c.EmpSubTypeID  where a.Emp_IsApproved = 1 and b.GradeID = @GradeID order by b.CandName Asc", param); //and a.EmpTypID = 1
        }

        public ScheduleEmps CheckEmpLeaveApply(Int64 EmpID, DateTime SchFrmDate, DateTime SchEdDate)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@EmpID", EmpID);
            param.Add("@SchFrmDate", SchFrmDate);
            param.Add("@SchEdDate", SchEdDate);

            var cnt = db.DapperFirst<ScheduleEmps>("select COUNT(a.EmpID) as EmpCount  from TblEmployeeLeaveApp as a where a.EmpID = @EmpID and CAST(a.EmpLeaveDate as Date)>=CAST(@SchFrmDate as DATE) and CAST(a.EmpLeaveDate as Date)<=CAST(@SchEdDate as DATE)", param);
            return cnt;
        }

        public int SaveShiftSchedule(int ShiftID, Int64 ProjID, DateTime SchfrmDate, DateTime SchToDate, string Schedule, int MonthID)
        {
            DynamicParameters paraobj = new DynamicParameters();
            paraobj.Add("@ProjID", ProjID);
            paraobj.Add("@Schedule", Schedule);
            paraobj.Add("@MonthID", MonthID);
            paraobj.Add("@SchfrmDate", SchfrmDate);
            paraobj.Add("@SchToDate", SchToDate);
            paraobj.Add("@ShiftID", ShiftID);
            paraobj.Add("@SchID", null, DbType.Int32, ParameterDirection.Output);

            int Result = db.DapperExecute("USP_SaveShiftSchedule", paraobj, CommandType.StoredProcedure);

            ProjectmasterID projctObj = new ProjectmasterID();
            int Iresult = paraobj.Get<Int32>("SchID");
            return Iresult;
        }

        public List<ProjectLeaveEmployeDetails> GetProjectLeaveEmpDetails(Int64 EmpID, DateTime SchfrmDate, DateTime SchEndDate)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@EmpID", EmpID);
            param.Add("@SchfrmDate", SchfrmDate);
            param.Add("@SchEndDate", SchEndDate);

            var list = db.DapperToList<ProjectLeaveEmployeDetails>(" select a.EmpID,a.LeavetypID,b.LeaveType, a.EmpLeaveDate ,a.EmpLeavetodate,a.EmpappDays,a.DayTypID,c.DayType,c.DayValue,a.EmpappStatus from TblEmployeeLeaveApp as a join TblLeaveType as b on a.LeavetypID = b.LeavetypID " +
                       " join TblDayType as c on a.DayTypID = c.DayTypID " +
                       " where a.EmpID = @EmpID and a.EmpLeaveDate between @SchfrmDate and @SchEndDate ", param);
            return list;
        }

        public List<ReliefEmpConfirmList> GetRelifeEmployeeList(Int64 EmpID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@EmpID", EmpID);

            var list = db.DapperToList<ReliefEmpConfirmList>("select  Distinct(a.EmpID),b.CandName, a.EmpTypID, c.EmpTypName , b.CandMobile,b.GradeID from TblEmployee as a join TblCandidate as b on a.CandID=b.CandID  " +
                                "join TblEmployeeType as c on a.EmpTypID = c.EmpTypID  where a.Emp_IsApproved = 1  and c.EmpTypStatus =1 and a.EmpID != @EmpID order by b.CandName Asc ", param);
            return list;
        }

        public List<ReliefEmpConfirmList> GetRelifeEmployeeList1(Int64 EmpID,DateTime Date)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@EmpID", EmpID);
            param.Add("@Date", Date);

            var list = db.DapperToList<ReliefEmpConfirmList>("USP_GetRelifeEmployeeList", param, CommandType.StoredProcedure);
            return list;
        }

        public List<TblAbscenceAllocate> GetAbscenceAllocateList()
        {
            var Iresultlist = db.DapperToList<TblAbscenceAllocate>("select * from TblAbscenceAllocate");
            return Iresultlist;
        }

        public int SaveReliefEmployee(TblAbscenceAllocate obj)
        {
            DynamicParameters paraobj = new DynamicParameters();
            paraobj.Add("@AbsallEmpID", obj.AbsallEmpID);
            paraobj.Add("@AbsDateFrom", obj.AbsDateFrom);
            paraobj.Add("@AbsDateTo", obj.AbsDateTo);
            paraobj.Add("@AbsEmpID", obj.AbsEmpID);
            paraobj.Add("@ProjID", obj.ProjID);
            paraobj.Add("@ReliefConfirmation", obj.ReliefConfirmation);

            paraobj.Add("@ErrorOutput", null, DbType.Int32, ParameterDirection.Output);

            int Result = db.DapperExecute("USP_SaveReliefEmployee", paraobj, CommandType.StoredProcedure);

            int OutputID = paraobj.Get<Int32>("ErrorOutput");
            return OutputID;
        }

        public IEnumerable<UserProfileWorkSchedule> GetCalendarEvents(Int64 ProjID)
        {
            DynamicParameters paraobj = new DynamicParameters();
            paraobj.Add("@ProjID", ProjID);
            return db.DapperToList<UserProfileWorkSchedule>("Select Distinct(a.ShiftID),b.SchID,a.ShiftName,a.ShiftFrom,a.ShiftTo , (FORMAT(b.StartDate,'yyyy/MM/dd')) as StartDate , (FORMAT((DATEADD(DAY,1,b.EndDate)),'yyyy/MM/dd')) as EndDate, (SELECT '#' +  CONVERT(VARCHAR(MAX), CRYPT_GEN_RANDOM(3), 2)) AS Color " +
            "from TblShift a join Tblschedule b on a.ShiftID=b.ShiftID where a.ProjID=@ProjID and b.ProjID =@ProjID ", paraobj);

            //DynamicParameters param = new DynamicParameters();
            //param.Add("@ProjID", ProjID);
            //param.Add("@Out", 1, DbType.Int32, ParameterDirection.Output);
            //return db.DapperIEnumerable<UserProfileWorkSchedule>("GetShiftCalendarEvents", param, CommandType.StoredProcedure);
        }

        public List<TblProject> GetProject(Int64 ProjID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@ProjID", ProjID);
            if (ProjID == 0)
            {
                return db.DapperToList<TblProject>("select * from TblProject");
            }
            else
            {
                return db.DapperToList<TblProject>("select * from TblProject as a where a.ProjID = @ProjID", param);
            }
        }

        public List<ListProjectWiseEmp> GetListProjectWiseEmp(DateTime Date, Int64 ProjID, Int32 ShiftID = 0)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@ProjID", ProjID);
            param.Add("@ShiftID", ShiftID);
            param.Add("@Date", Date);

            if (ProjID == 0)
            {
                return db.DapperToList<ListProjectWiseEmp>("select Distinct(a.EmpID),empshift.ProjID,b.CandName,grade.Gradename,shft.ShiftName,shft.ShiftFrom,shft.ShiftTo,b.GradeID from TblEmployee as a join TblCandidate as b on a.CandID=b.CandID  " +
                                                           " join TblEmpShift as empshift on a.EmpID = empshift.EmpID " +
                                                           " join TblShift as shft on empshift.ShiftID = shft.ShiftID " +
                                                           " join TblGrade as grade on b.GradeID = grade.GradeID " +
                                                           " where a.Emp_IsApproved = 1 and empshift.EmpShiftStatus = 1 order by empshift.ProjID ", param);
            }
            else if (ProjID != 0 && ShiftID == 0)
            {
                return db.DapperToList<ListProjectWiseEmp>("select Distinct(a.EmpID),empshift.ProjID,b.CandName,grade.Gradename,shft.ShiftName,shft.ShiftFrom,shft.ShiftTo,b.GradeID from TblEmployee as a join TblCandidate as b on a.CandID=b.CandID  " +
                                                           " join TblEmpShift as empshift on a.EmpID = empshift.EmpID " +
                                                           " join TblShift as shft on empshift.ShiftID = shft.ShiftID " +
                                                           " join TblGrade as grade on b.GradeID = grade.GradeID " +
                                                           " join Tblschedule sch on sch.SchID=empshift.SchID  " +  //
                                                           " where a.Emp_IsApproved = 1 and  empshift.ProjID = @ProjID and empshift.EmpShiftStatus = 1 and convert(date,@Date) between sch.StartDate and sch.EndDate  order by empshift.ProjID  ", param);
            }
            else
            {
                return db.DapperToList<ListProjectWiseEmp>("select Distinct(a.EmpID),empshift.ProjID,b.CandName,grade.Gradename,shft.ShiftName,shft.ShiftFrom,shft.ShiftTo,b.GradeID from TblEmployee as a join TblCandidate as b on a.CandID=b.CandID  " +
                                                            " join TblEmpShift as empshift on a.EmpID = empshift.EmpID " +
                                                            " join TblShift as shft on empshift.ShiftID = shft.ShiftID " +
                                                            " join TblGrade as grade on b.GradeID = grade.GradeID " +
                                                            " join Tblschedule sch on sch.SchID=empshift.SchID  " +  //
                                                            " where a.Emp_IsApproved = 1 and  empshift.ProjID = @ProjID and empshift.EmpShiftStatus = 1 and empshift.ShiftID = @ShiftID and convert(date,@Date) between sch.StartDate and sch.EndDate order by empshift.ProjID  ", param);
            }
        }

        public int SaveProjectAttandance(TblAttendance obj)
        {
            DynamicParameters paraobj = new DynamicParameters();
            paraobj.Add("@EmpID", obj.EmpID);
            paraobj.Add("@AttendtimeIN", obj.AttendtimeIN);
            paraobj.Add("@AttendtimeOut", obj.AttendtimeOut);
            paraobj.Add("@Attenddate", obj.Attenddate);
            paraobj.Add("@ProjID", obj.ProjID);

            paraobj.Add("@AttendHoliStatus", obj.AttendHoliStatus);
            paraobj.Add("@IsOFFDay", obj.IsOFFDay);

            paraobj.Add("@ErrorOutput", null, DbType.Int32, ParameterDirection.Output);

            int Result = db.DapperExecute("USP_SaveProjectAttandance", paraobj, CommandType.StoredProcedure);

            int OutputID = paraobj.Get<Int32>("ErrorOutput");
            return OutputID;
        }

        public List<TblShift> GetProjectShiftList(Int64 ProjID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@ProjID", ProjID);
            return db.DapperToList<TblShift>("select * from TblShift as a where a.ProjID= @ProjID", param);
        }

        public List<ProjectGradeAssignList> GetProjectGradeAssignList(Int64 ProjID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@ProjID", ProjID);
            return db.DapperToList<ProjectGradeAssignList>("select a.PrjGrdAssignID,a.ProjID,a.DesigID,a.GradeID,b.DesigName,c.Gradename,a.PrjGradeEmpNO,sht.ShiftName from TblProjectGradeAssign as a join TblDesignation as b on a.DesigID=b.DesigID " +
                                                           "join TblGrade as c on a.GradeID = c.GradeID join TblShift sht on sht.ShiftID=a.ShiftID where a.ProjID = @ProjID and a.PrjGrdAssgnStatus = 1 and b.DesigStatus = 1 ", param);
        }

        public int DeleteProjectShift(Int32 ShiftID)
        {
            DynamicParameters paraobj = new DynamicParameters();
            paraobj.Add("@ShiftID", ShiftID);
            int result = db.DapperExecute("delete from TblShift  where ShiftID = @ShiftID", paraobj);
            return 1;
        }

        public List<ReliefEmpConfirmList> GetReliefEmpConfirmList()
        {
            var list = db.DapperToList<ReliefEmpConfirmList>("select Distinct(a.EmpID),b.CandName, a.EmpTypID, c.EmpTypName , b.CandMobile,b.GradeID from TblEmployee as a join TblCandidate as b on a.CandID=b.CandID " +
                                                             "join TblEmployeeType as c on a.EmpTypID = c.EmpTypID " +
                                                             "where a.Emp_IsApproved=1  and c.EmpTypStatus=1 order by b.CandName Asc");
            return list;
        }

        public List<SelectedReliefEmp> GetEmpAbsnceAllcatelist(DateTime AbsenceDate)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@AbsenceDate", AbsenceDate);

            var Ilists = db.DapperToList<SelectedReliefEmp>("select a.AbsEmpID,a.AbsallEmpID,c.CandName,a.ProjID,d.Gradename,a.ReliefConfirmation,c.CandMobile,e.EmpTypName from TblAbscenceAllocate as a join TblEmployee as b on a.AbsallEmpID = b.EmpID " +
                                                               "join TblCandidate as c on b.CandID = c.CandID join TblGrade d on d.GradeID=c.GradeID  " +
                                                               "join TblEmployeeType as e on b.EmpTypID = e.EmpTypID " +
                                                               "where CAST(a.AbsDateFrom as Date)= CAST(@AbsenceDate as Date) and b.Emp_IsApproved = 1", param);
            return Ilists;
        }

        public int ManageAbsenceAllocation(TblAbscenceAllocate objs)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@AbsEmpID", objs.AbsEmpID);
            param.Add("@AbsallEmpID", objs.AbsallEmpID);
            param.Add("@AbsDateFrom", objs.AbsDateFrom);
            param.Add("@ProjID", objs.ProjID);
            param.Add("@ReliefConfirmation", objs.ReliefConfirmation);
            param.Add("@ErrorOutput", null, DbType.Int32, ParameterDirection.Output);

            int Result = db.DapperExecute("USP_ManageAbsenceAllocations", param, CommandType.StoredProcedure);

            int OutputID = param.Get<Int32>("ErrorOutput");
            return OutputID;
        }

        public List<TblAttendance> GetAttandanceProjectList(Int64 ProjID, DateTime Attenddate)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@ProjID", ProjID);
            param.Add("@Attenddate", Attenddate);

            var IList = db.DapperToList<TblAttendance>("select * from TblAttendance as a where CAST(a.Attenddate as Date)= CAST(@Attenddate as Date) and a.ProjID = @ProjID", param);
            return IList;
        }

        public List<ScheduleDropList> GetScheduleList(int ShiftID, Int64 ProjID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@ShiftID", ShiftID);
            param.Add("@ProjID", ProjID);

            var IList = db.DapperToList<ScheduleDropList>("select a.SchID, a.Schedule,a.MonthID,a.ProjID,a.ShiftID,(FORMAT(a.StartDate,'dd/MM/yyyy')) as StartDate,(FORMAT(a.EndDate,'dd/MM/yyyy')) as EndDate from Tblschedule as a  where a.ShiftID = @ShiftID and ProjID = @ProjID", param);
            return IList;
        }

        //public List<TblEmployeeLeaveApp> GetEMployeeLeave(Int64 EmpID,DateTime Date)
        public int GetEMployeeLeave(Int64 EmpID, DateTime Date)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@EmpID", EmpID);
            param.Add("@attDate", Date);

            //var IList = db.DapperToList<TblEmployeeLeaveApp>("select *from TblEmployeeLeaveApp where @attDate between EmpLeaveDate and EmpLeavetodate and EmpID=@EmpID", param);
            //return IList;
            var IList = db.DapperSingle("select count(*)from TblEmployeeLeaveApp where convert(date,@attDate) between EmpLeaveDate and EmpLeavetodate and EmpID=@EmpID and EmpappStatus in (1,3)", param);
            return Convert.ToInt32(IList);
        }

        public bool EmployeeOFFday(Int64 EmpID, DateTime attenddate)
        {
            var day = (int)attenddate.DayOfWeek;
            DynamicParameters paraobj = new DynamicParameters();
            paraobj.Add("@empid", EmpID);
            paraobj.Add("@attenddate", day);

            var count = db.DapperSingle("select COUNT(Emp_ID) from TblRestDay where Emp_ID=@empid and RestDay=@attenddate", paraobj);
            if (count == "0")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool EmployeeHoliday(Int64 EmpID, DateTime attenddate)
        {
            DynamicParameters paraobj = new DynamicParameters();
            paraobj.Add("@attenddate", attenddate);

            var count = db.DapperSingle("select COUNT(HolidayID) from TblHolidayList where HoliDate=@attenddate", paraobj);
            if (count == "0")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public List<EditProjectSchedule> GetEditProjectSchedule(Int64 ProjID, int SchID, int ShiftID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@ProjID", ProjID);
            param.Add("@SchID", SchID);
            param.Add("@ShiftID", ShiftID);
            //var lists = db.DapperToList<EditProjectSchedule>(" select a.ShiftID,c.ShiftName as Shift,a.SchID,b.Schedule as Schedule,a.EmpID,cand.CandName as EmpName,cand.DesigID,desig.DesigName as EmpDesigName,(FORMAT(b.StartDate,'dd/MM/yyyy')) as SchFromDate , (FORMAT(b.EndDate,'dd/MM/yyyy')) as SchToDate, cand.GradeID,grade.Gradename as EmpGrade, COUNT(EmpLevApp.EmpLeaveappID) as EmpLeaveStatus  " +
            //                                                 " from TblEmpShift as a join Tblschedule as b on a.SchID = b.SchID " +
            //                                                 " join TblShift as c on a.ShiftID = c.ShiftID "+
            //                                                 " join TblEmployee as emp on a.EmpID = emp.EmpID "+
            //                                                 " join TblCandidate as cand on emp.CandID = cand.CandID " +
            //                                                 " join TblDesignation as desig on cand.DesigID = desig.DesigID " +
            //                                                 " join TblGrade as grade on cand.GradeID = grade.GradeID " +
            //                                                 " left join  TblEmployeeLeaveApp as EmpLevApp on a.EmpID = EmpLevApp.EmpID " +
            //                                                 " where a.ProjID = @ProjID and  a.SchID = @SchID and a.ShiftID = @ShiftID and emp.Emp_IsApproved = 1" +
            //                                                 " group by a.ShiftID,c.ShiftName,a.SchID,b.Schedule,a.EmpID,cand.CandName ,cand.DesigID,desig.DesigName,b.StartDate , b.EndDate , cand.GradeID,grade.Gradename ", param);
            var lists = db.DapperToList<EditProjectSchedule>("USP_GetEditProjectSchedule", param, CommandType.StoredProcedure);
            return lists;
        }

        public int CounttblAttendance(int SchID, Int64 EmpID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@SchID", SchID);
            param.Add("@EmpID", EmpID);
            string count = db.DapperSingle("Select Count(*) from TblAttendance where SchID = @SchID and EmpID= @EmpID", param);
            Int32 counts = Convert.ToInt32(count);
            if (counts > 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public int DeleteEmpShiftEmployeeAssign(Int64 ProjID, int ShiftID, int SchID, Int64 EmpID, string SchFromDate = null, string SchToDate = null, string flg = null)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@ProjID", ProjID);
            param.Add("@ShiftID", ShiftID);
            param.Add("@SchID", SchID);
            param.Add("@EmpID", EmpID);
            param.Add("@flg", flg);

            var Iresults = db.DapperExecute("delete from TblEmpShift where ProjID = @ProjID and ShiftID = @ShiftID and SchID = @SchID and EmpID = @EmpID", param);
            if (flg == "1")
            {
                var Iresult = db.DapperExecute("Delete from TblAttendance where SchID = @SchID and EmpID= @EmpID", param);
            }

            if (SchFromDate != null && SchToDate != null)
            {
                DateTime SchFrmDate = common.CommonDateConvertion(SchFromDate);
                DateTime SchEdDate = common.CommonDateConvertion(SchToDate);
                param.Add("@SchFrmDate", SchFrmDate);
                param.Add("@SchEdDate", SchEdDate);
                var data = db.DapperExecute("delete from TblRestDay where Emp_ID=@EmpID and RestDay_Dates between @SchFrmDate and @SchEdDate", param);
            }

            return 1;
        }

        public projectCalendar GetProjectShiftschedule(Int64 ProjID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@ProjID", ProjID);

            projectCalendar calendar = new projectCalendar();
            calendar.ShiftObj = db.DapperToList<TblShift>("select *from TblShift where ProjID=@ProjID", param);
            calendar.ScheduleObj = db.DapperToList<Tblschedule>("select *from Tblschedule where ProjID=@ProjID", param);

            return calendar;
        }

        public List<ListProjectWiseEmp> GetScheduleEmps(Int32 scheduleid)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@scheduleid", scheduleid);

            // var details = db.DapperToList<ListProjectWiseEmp>("USP_ProjectShedEmpList", param,CommandType.StoredProcedure);
            var details = db.DapperToList<ListProjectWiseEmp>(" select  empsht.EmpID,empsht.ProjID,cand.CandName,grade.Gradename,shft.ShiftName,shft.ShiftFrom,shft.ShiftTo,cand.GradeID,empsht.SchID " +
                                                              " from TblEmpShift empsht join TblEmployee emp on empsht.EmpID=emp.EmpID " +
                                                              " join TblCandidate cand on cand.CandID=emp.CandID " +
                                                              " join TblGrade grade on grade.GradeID=cand.GradeID " +
                                                              " join TblShift shft on shft.ShiftID=empsht.ShiftID  where empsht.schID=@scheduleid", param);
            return details;
        }

        public int GetStartandEndDate(int ScheID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@ScheID", ScheID);
            Tblschedule schedule = db.DapperFirst<Tblschedule>("select * from Tblschedule where SchID =@ScheID", param);
            param.Add("@StartDate", schedule.StartDate);
            param.Add("@EndDate", schedule.EndDate);
            var count = db.DapperSingle("Select count(*) from TblAttendance where SchID = @ScheID and Attenddate between @StartDate and @EndDate", param);

            if (Convert.ToInt32(count) > 0)
            {
                return 0;
            }
            else
            {
                return 2;
            }
        }

        public int DeleteProjectSchedule(int ScheID, string flg)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@ScheID", ScheID);
            if (flg == "1")
            {
                Tblschedule schedule = db.DapperFirst<Tblschedule>("select * from Tblschedule where SchID =@ScheID", param);
                param.Add("@StartDate", schedule.StartDate);
                param.Add("@EndDate", schedule.EndDate);
                db.DapperExecute("delete from TblAttendance where SchID = @ScheID and Attenddate between @StartDate and @EndDate", param);
            }
            db.DapperExecute("delete from Tblschedule where SchID=@ScheID", param);
            db.DapperExecute("delete from TblEmpShift where SchID=@ScheID", param);

            return 1;
        }

        public projectCalendar GetProjectShiftandschedule(Int64 ProjID, int shiftid)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@ProjID", ProjID);
            param.Add("@shiftid", shiftid);

            projectCalendar calendar = new projectCalendar();
            calendar.ScheduleObj = db.DapperToList<Tblschedule>("select *from Tblschedule where ProjID=@ProjID and ShiftID=@shiftid", param);
            calendar.ListProjectWiseEmp = db.DapperToList<ListProjectWiseEmp>("USP_ProjectShiftandscheduleList", param, CommandType.StoredProcedure);

            return calendar;
        }

        public int GetCountinAttendance(int shift)
        {
            var count = "0";
            int counts = 0;
            DynamicParameters param = new DynamicParameters();
            param.Add("@shift", shift);
            List<Tblschedule> schedule = db.DapperToList<Tblschedule>("select * from Tblschedule where ShiftID =@shift", param);
            foreach (var item in schedule)
            {
                param.Add("@SchID", item.SchID);
                count = db.DapperSingle("Select count(*) from TblAttendance where SchID = @SchID", param);
                counts = Convert.ToInt32(count);
                counts += counts;
            }

            if (counts > 0)
            {
                return 0;
            }
            else
            {
                return 2;
            }
        }

        public int DeleteShift(int shiftid, string flg)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@shiftid", shiftid);
            if (flg == "1")
            {
                List<Tblschedule> schedule = db.DapperToList<Tblschedule>("select * from Tblschedule where ShiftID =@shiftid", param);

                foreach (var item in schedule)
                {
                    param.Add("@SchID", item.SchID);
                    db.DapperExecute("delete from TblAttendance where SchID = @SchID", param);
                }
            }
            db.DapperExecute("delete from Tblschedule where  ShiftID=@shiftid", param);
            db.DapperExecute("delete from TblEmpShift where ShiftID=@shiftid", param);
            db.DapperExecute("delete from TblShift where ShiftID=@shiftid", param);
            return 1;
        }

        public void RemoveFromAttandance(Int64 Empid, DateTime Attdate)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@Empid", Empid);
            param.Add("@Attdate", Attdate);

            db.DapperExecute("Delete from TblAttendance where EmpID=@Empid and Attenddate=@Attdate", param);
        }

        public int GetLeaveStatus(Int64 EmpId, DateTime date)
        {
            DynamicParameters para = new DynamicParameters();
            para.Add("@EmpId", EmpId);
            para.Add("@attDate", date);

            var count = db.DapperSingle("Select count(*) from TblEmployeeLeaveApp where @attDate between EmpLeaveDate and EmpLeavetodate and EmpID=@EmpId and EmpappStatus in(1,3)", para);
            return (Convert.ToInt32(count));
        }

        public bool GetOffdayStatus(Int64 EmpId, DateTime date)
        {
            DynamicParameters para = new DynamicParameters();
            para.Add("@EmpId", EmpId);
            para.Add("@attDate", date);
            para.Add("@ErrorOutput", null, DbType.Int32, ParameterDirection.Output);

            int Result = db.DapperExecute("USP_CheckOffDay", para, CommandType.StoredProcedure);
            Int32 result = para.Get<Int32>("ErrorOutput");
            if (result > 0)
                return true;
            else
                return false;
        }

        public bool GetOffdayStatus1(Int64 EmpId, string date)
        {
            DynamicParameters para = new DynamicParameters();
            para.Add("@EmpId", EmpId);
            para.Add("@attDate", common.CommonDateConvertion(date));
            para.Add("@ErrorOutput", null, DbType.Int32, ParameterDirection.Output);

            int Result = db.DapperExecute("USP_CheckOffDay", para, CommandType.StoredProcedure);
            Int32 result = para.Get<Int32>("ErrorOutput");
            if (result > 0)
                return true;
            else
                return false;
        }

        public string GetLeaveTypeName(Int64 EmpId, DateTime date)
        {
            DynamicParameters para = new DynamicParameters();
            para.Add("@EmpId", EmpId);
            para.Add("@Date", date);

            return db.DapperSingle("Select b.LeaveType from TblEmployeeLeaveApp a join TblLeaveType b on a.LeavetypID=b.LeavetypID where a.EmpID=@EmpId and convert(date,EmpLeaveDate)=convert(date,@Date) and a.EmpappStatus in (1 , 3)", para);
        }

        public void UpdateOptionalOffDays(Int64 EmpID, DateTime date)
        {
            DynamicParameters para = new DynamicParameters();
            para.Add("@EmpId", EmpID);
            para.Add("@Date", date);
            var count = db.DapperSingle("Select count(*) from TblRestDay where Emp_ID=@EmpId and RestDay_Dates=convert(date,@Date)", para);
            if (Convert.ToInt16(count) == 0)
            {
                int result = db.DapperExecute("Insert into TblRestDay(Emp_ID,RestDay_Dates) values(@EmpId,convert(date,@Date))", para);
            }
        }

        public void DeleteRestDays(Int64 EmpID)
        {
            DynamicParameters para = new DynamicParameters();
            para.Add("@EmpId", EmpID);
            db.DapperExecute("Delete from TblRestDay where Emp_ID=@EmpId", para);
        }

        public Int16 EmpOffStatus(Int64 EmpID)
        {
            DynamicParameters para = new DynamicParameters();
            para.Add("@EmpId", EmpID);
            string status = db.DapperSingle("select RestDay_Type from TblEmployee where EmpID=@EmpId", para);
            return Convert.ToInt16(status);
        }

        public List<TblShift> GetShifts(Int64 ProjID)
        {
            DynamicParameters para = new DynamicParameters();
            para.Add("@ProjID", ProjID);

            return db.DapperToList<TblShift>("Select ShiftID,ShiftName from TblShift where ProjID=@ProjID", para);
        }

        public RestDays GetOffDetails(Int64 EmpID)
        {
            DynamicParameters para = new DynamicParameters();
            para.Add("@EmpID", EmpID);

            return db.DapperFirst<RestDays>("Select RestDay_Type,Restday_fixed From TblEmployee where EmpID=@EmpID", para);
        }

        public int GetScheduleWiseOffDayStatus(Int64 EmpID, DateTime SchFromDate, DateTime SchEndDate, Int16 FixedDay)
        {
            DynamicParameters para = new DynamicParameters();
            para.Add("@SchFromDate", SchFromDate);
            para.Add("@SchEndDate", SchEndDate);
            para.Add("@FixedDay", FixedDay);
            para.Add("@ErrorOutput", 1, DbType.Int32, ParameterDirection.Output);

            var data = db.DapperExecute("USP_GetScheduleFixedOffDayStatus", para, CommandType.StoredProcedure);
            Int32 result = para.Get<Int32>("ErrorOutput");
            return result;
        }

        public List<OFFDetails> GetOFFDates(Int64 EmpID, DateTime SchFromDate, DateTime SchEndDate)
        {
            DynamicParameters para = new DynamicParameters();
            para.Add("@EmpID", EmpID);
            para.Add("@SchFromDate", SchFromDate);
            para.Add("@SchEndDate", SchEndDate);

            var data = db.DapperToList<OFFDetails>("USP_GetOFFDates_ReliefAssign", para, CommandType.StoredProcedure);
            return data;
        }

        public List<OFFDetails> GetSavedOffDates(Int64 EmpID, DateTime StartDate, DateTime EndDate)
        {
            DynamicParameters para = new DynamicParameters();
            para.Add("@EmpID", EmpID);
            para.Add("@StartDate", StartDate);
            para.Add("@EndDate", EndDate);

            return db.DapperToList<OFFDetails>("Select RestDay_Dates as OFFDates from TblRestDay where Emp_ID=@EmpID and RestDay_Dates between @StartDate and @EndDate", para);
        }

        public int RemoveAbsenceAllocation(Int64 AbsentEmp, Int64 AllocateEmp, DateTime Date)
        {
            DynamicParameters para = new DynamicParameters();
            para.Add("@AbsentEmp", AbsentEmp);
            para.Add("@AllocateEmp", AllocateEmp);
            para.Add("@Date", Date);

            var result = db.DapperExecute("Delete from TblAbscenceAllocate where AbsEmpID=@AbsentEmp and AbsallEmpID=@AllocateEmp and AbsDateFrom=@Date", para);
            return 1;
        }
    }
}