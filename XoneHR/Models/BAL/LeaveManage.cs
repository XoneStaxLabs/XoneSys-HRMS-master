using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace XoneHR.Models.BAL
{
    public class LeaveManage
    {
        CommonFunctions common = new CommonFunctions();

        private XoneDbLayer db;
        
        public LeaveManage()
        {
            db = new XoneDbLayer();
        } 
        public LeaveItems AddnewLeaveapplication(LeaveApplication LeaveAppObj)
        {
            try
            {
                DynamicParameters paraObj = new DynamicParameters();
                paraObj.Add("@EmpID", LeaveAppObj.EmpID);
                paraObj.Add("@LeavetypID", LeaveAppObj.LeavetypID);
                paraObj.Add("@EmpLeaveDate", LeaveAppObj.EmpLeaveDate);
                paraObj.Add("@EmpLeavetodate", LeaveAppObj.EmpLeavetodate);
                paraObj.Add("@DayTypID", LeaveAppObj.DayTypID);
                paraObj.Add("@EmpappDays", LeaveAppObj.EmpappDays);
                paraObj.Add("@EmpappRemks", LeaveAppObj.EmpappRemks);
                paraObj.Add("@SchID", 0);

                paraObj.Add("@ErrorOutput", null, DbType.Int32, ParameterDirection.Output);

                int Result = db.DapperExecute("USP_AddEmpLeaveApp", paraObj, CommandType.StoredProcedure);

                LeaveItems leaveObj = new LeaveItems();             
                leaveObj.OutputID = paraObj.Get<Int32>("ErrorOutput");
                return leaveObj;

            }
            catch
            {
                return null;
            }
        }
        public LeaveItems AddNewAttendance(TblAttendance tblattendobj, string IsOFFDay, string Attenddate)
        {
            try
            {
                DynamicParameters paraObj = new DynamicParameters();
                paraObj.Add("@EmpID", tblattendobj.EmpID);
                paraObj.Add("@AttendtimeIN", tblattendobj.AttendtimeIN);
                paraObj.Add("@AttendtimeOut", tblattendobj.AttendtimeOut);
                paraObj.Add("@Attenddate", common.CommonDateConvertion(Attenddate));
                paraObj.Add("@AttendRemks", tblattendobj.AttendRemks);
                paraObj.Add("@AttendHoliStatus", tblattendobj.AttendHoliStatus);
                paraObj.Add("@ProjID", tblattendobj.ProjID);
                paraObj.Add("@SchID", tblattendobj.SchID);

                if (IsOFFDay == "on")
                {
                    paraObj.Add("@IsOFFDay",Convert.ToBoolean(1));
                }
                else
                {
                    paraObj.Add("@IsOFFDay", Convert.ToBoolean(0));
                }
               


                paraObj.Add("@ErrorOutput", null, DbType.Int32, ParameterDirection.Output);

                int Result = db.DapperExecute("USP_AddNewAttendance", paraObj, CommandType.StoredProcedure);

                LeaveItems leaveObj = new LeaveItems();
                leaveObj.OutputID = paraObj.Get<Int32>("ErrorOutput");
                return leaveObj;

            }
            catch
            {
                return null;
            }
        }
        public List<TblLeaveType> ComboListgetLeaveType(Int64 EmpID)
        {
            try
            {
                DynamicParameters para = new DynamicParameters();
                para.Add("@EmpID", EmpID);
                para.Add("@Out", 1, DbType.Int32, ParameterDirection.Output);
                var LeaveTypes = db.DapperToList<TblLeaveType>("USP_CombolistleaveType",para, CommandType.StoredProcedure);
                return LeaveTypes;
            }
            catch
            {
                return null;
            }
        }
        public List<TblDayType> ComboListDayType()
        {
            try
            {
                var DayTypes = db.DapperToList<TblDayType>("USP_CombolistDayType", CommandType.StoredProcedure);
                return DayTypes;
            }
            catch
            {
                return null;
            }
        }
        public List<EmployeeProfile> ComboListEmployee()
        {
            try
            {
                return db.DapperToList<EmployeeProfile>("USP_CombolistEmployeeLeaves", CommandType.StoredProcedure);
            }
            catch
            {
                return null;
            }
        }
        public List<LeaveApplication> ListEmployeesLeaves(int EmpappStatus)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@EmpappStatus", EmpappStatus);
                var leavelists = db.DapperToList<LeaveApplication>("USP_ListleaveEmployee",param, CommandType.StoredProcedure);
                return leavelists;
            }
            catch
            {
                return null;
            }
        } 
        public List<AttendanceBook> ListAttendanceBook(int codevalue, int ScheduleID,Int64 ProjectId)
        {
            try
            {

                DynamicParameters para = new DynamicParameters();
                para.Add("@codevalue", codevalue);
                para.Add("@schid", ScheduleID);
                para.Add("@ProjID", ProjectId);

                var leavelists = db.DapperToList<AttendanceBook>("USP_ListAttendanceBook",para, CommandType.StoredProcedure);
                return leavelists;
            }
            catch
            {
                return null;
            }
        }
        public LeaveItemsOutput LeaveTypeCheck(Int64 EmpID, Int32 LeaveTypID, double empDiffernce,Int32 DayTyp)
        {
            try
            {

                DynamicParameters para = new DynamicParameters();
                para.Add("@EmpID", EmpID);
                para.Add("@LeaveTypID", LeaveTypID);
                para.Add("@EmpLeaveDeffer", empDiffernce);
                para.Add("@DayTypID", DayTyp);

                para.Add("@output", null, DbType.Int32, ParameterDirection.Output);
                para.Add("@PendingDays", null,DbType.Double, ParameterDirection.Output);

                int Result = db.DapperExecute("USP_LeaveTypeCheck", para, CommandType.StoredProcedure);

                LeaveItemsOutput leaveObj = new LeaveItemsOutput();
                leaveObj.OutputID = para.Get<Int32>("output");
                leaveObj.PendingDays = para.Get<Double>("PendingDays");
                return leaveObj;
            }
            catch
            {
                return null;
            }
        }
        public List<LeaveDates> LeaveDates()
        {
            try
            {

                var leaveDates = db.DapperToList<LeaveDates>("select *from TblEmployeeLeaveApp where EmpappStatus=1 and LeavetypID!=9");
                return leaveDates;
            }
            catch
            {
                return null;
            }
        }
        public List<AttendDate> AttendDates(Int32 ScheduleID, Int64 ProjectId, int codevalue)
        {
            try
            {
                DynamicParameters para = new DynamicParameters();              
                para.Add("@schid", ScheduleID);
                para.Add("@ProjID", ProjectId);
                para.Add("@codevalue", codevalue);

                var AttendDate = (dynamic)null;
                if (codevalue == 2)
                {
                    AttendDate = db.DapperToList<AttendDate>("select Attenddate,EmpID,IsOFFDay from TblAttendance where ProjID=@ProjID and SchID=@schid", para);

                }
                else
                {
                    AttendDate = db.DapperToList<AttendDate>("select Attenddate,EmpID,IsOFFDay from TblAttendance where ProjID=@ProjID", para);

                }

                return AttendDate;
            }
            catch
            {
                return null;
            }
        }
        public List<HolidayAttendance> ListAttendanceHoliday()
        {
            try
            {

                var HolidayLists = db.DapperToList<HolidayAttendance>("select HolidayID,HoliText,HoliDate from TblHolidayList where holistatus=1");
                return HolidayLists;
            }
            catch
            {
                return null;
            }
        }
        public List<ScheduleListsapp> ComboListSchedule(int monthid, Int64 ProjID)
        {
            try
            {
                DynamicParameters para = new DynamicParameters();
                para.Add("@monthID", monthid);
                para.Add("@ProjID", ProjID);
                return db.DapperToList<ScheduleListsapp>("USP_CombolistShedule", para, CommandType.StoredProcedure);
            }
            catch
            {
                return null;
            }
        }
        public List<TblProject> GetProjectList(Int32 flag,Int64 EmpID)
        {
            DynamicParameters para = new DynamicParameters();
            para.Add("@EmpID", SessionManage.Current.GlobalEmpID);
            if (flag == 0)
                return db.DapperToList<TblProject>("select ProjID,ProjName from TblProject");
            else
                return db.DapperToList<TblProject>("select a.ProjID,a.ProjName from TblProject a join TblEmpProject b on a.ProjID=b.ProjID where b.EmpID=@EmpID",para);
        }
        public IEnumerable<EmpAbsenceAllocation> GetEmpProject(Int64 EmpLeaveappID,DateTime date)
        {
            DynamicParameters para = new DynamicParameters();
            para.Add("@EmpLeaveappID", EmpLeaveappID);
            para.Add("@date", date);
            return db.DapperIEnumerable<EmpAbsenceAllocation>("Select empleave.EmpLeaveappID,emp.EmpID,cand.CandName,shif.ShiftID,shif.ShiftName,schd.SchID,schd.Schedule,proj.ProjID,proj.ProjName from TblEmployeeLeaveApp as empleave join TblEmployee as emp on empleave.EmpID=emp.EmpID join TblCandidate cand on emp.CandID=cand.CandID join TblEmpShift empshif on empshif.EmpID=emp.EmpID join TblShift shif on shif.ShiftID=empshif.EmpshiftID join Tblschedule schd on empleave.SchID=schd.SchID join TblProject proj on proj.ProjID=schd.ProjID  where empleave.EmpLeaveappID=@EmpLeaveappID and( CONVERT(date,empleave.EmpLeaveDate)=CONVERT(date,@date) or CONVERT(date,empleave.EmpLeavetodate)=CONVERT(date,@date) )", para);
            
        }
        public List<Tblschedule> GetScheduleList(Int64 ProjID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@ProjID", ProjID);
            return db.DapperToList<Tblschedule>("select SchID,Schedule from Tblschedule where ProjID=@ProjID",param);
        }
        public List<TblShift> GetShiftDetails (Int64 ProjID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@ProjID", ProjID);
            return db.DapperToList<TblShift>("select ShiftID,ShiftName from TblShift where ProjID=@ProjID", param);
        }
        public List<EmployeeProfile> GetLeaveEmpDetails (Int64 ProjID , Int32 SchID , Int32 ShiftID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@ProjID", ProjID);
            param.Add("@SchID", SchID);
            param.Add("@ShiftID", ShiftID);
            return db.DapperToList<EmployeeProfile>("select Distinct( emp.EmpID) as EmpID ,cand.CandName from TblCandidate as cand join TblEmployee as emp on cand.CandID=emp.CandID " +
                                                    " join TblEmpShift as empshft on emp.EmpID=empshft.EmpID   " +
                                                    " join TblEmployeeLeaveApp as Empleaveapp on emp.EmpID=Empleaveapp.EmpID " +
                                                    " where empshft.ProjID=@ProjID and empshft.SchID=@SchID and empshft.ShiftID=@ShiftID and emp.EmpStatus=1", param);  //and Empleaveapp.EmpappDays=1
        }
        public LeaveEmployeDetails GerLeaveEmpList(Int64 EmpID, Int32 SchID, DateTime Date)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@EmpID", EmpID);
            param.Add("@SchID", SchID);
            param.Add("@Date", Date);
            var datats = db.DapperFirst<LeaveEmployeDetails>(" select a.EmpLeaveappID, a.LeavetypID,b.LeaveType,a.EmpLeaveDate,a.EmpLeavetodate,a.EmpappDays,c.DayTypID,c.DayType,c.DayValue,a.EmpappRemks,a.SchID from TblEmployeeLeaveApp as a  " +
                                                             " join TblLeaveType as b on a.LeavetypID=b.LeavetypID  " +
                                                             " join TblDayType as c on a.DayTypID=c.DayTypID  " +
                                                             " where a.EmpID=@EmpID and a.SchID=@SchID and @Date between EmpLeaveDate and EmpLeavetodate", param);
           
            return datats;
        }
        public List<EmployeeProfile> GetAssignedEmployee(Int64 EmpID,DateTime LeaveDate)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@EmpID", EmpID);
            param.Add("@LeaveDate", LeaveDate);
            return db.DapperToList<EmployeeProfile>("select Distinct( emp.EmpID) as EmpID ,cand.CandName from TblCandidate as cand join TblEmployee as emp on cand.CandID=emp.CandID where emp.EmpTypID=2 and emp.EmpID not in(select AbsallEmpID from TblAbscenceAllocate where convert(date,AbsDateFrom)=convert(date,@LeaveDate) or convert(date,AbsDateTo)=convert(date,@LeaveDate))", param);
        }
        public List<LeaveEmployeDetails> GetAssignedEmployeeDateList(Int64 EmpID, Int32 SchID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@EmpID", EmpID);
            param.Add("@SchID", SchID);
            return db.DapperToList<LeaveEmployeDetails>("select  a.EmpLeaveDate,a.EmpLeavetodate  from TblEmployeeLeaveApp as a  " +
                                                       " join TblLeaveType as b on a.LeavetypID=b.LeavetypID " +
                                                       " join TblDayType as c on a.DayTypID=c.DayTypID " +
                                                       " where a.EmpID=@EmpID and a.SchID=@SchID", param);
        }
        public int SaveEmployeeAssigned(TblAbscenceAllocate EmpObj , DataTable DT)
        {
            
            SqlParameter[] param = {
                                       new SqlParameter("@AbsenceData",SqlDbType.Structured){ Value=DT }, 
                                       new SqlParameter("@OUT",SqlDbType.Int){ Direction=ParameterDirection.Output },
                                       new SqlParameter("@AbsEmpID",SqlDbType.BigInt){ Value=EmpObj.AbsEmpID },
                                       new SqlParameter("@ProjID",SqlDbType.Int){Value = EmpObj.ProjID},
                                       new SqlParameter("@SchID",SqlDbType.Int){Value = EmpObj.SchID},
                                       new SqlParameter("@ShiftID",SqlDbType.Int){Value = EmpObj.ShiftID} 
                                   };


            int result = db.ExecuteWithDataTable("USP_SaveAbsenceEmpAllocation", param, CommandType.StoredProcedure);
            string output = param[1].Value.ToString();
            Int32 Out = Convert.ToInt32(output);
            return Out;
        }
        public string CheckAbsenceEmp(Int64 AbsallEmpID, DateTime Date , Int64 ProjID , int SchID , int ShiftID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@AbsallEmpID",AbsallEmpID);
            param.Add("@Date",Date);
            param.Add("@ProjID", ProjID);
            param.Add("@SchID", SchID);
            param.Add("@ShiftID", ShiftID);

            var cnt = db.DapperSingle("select COUNT(a.AbsentID) as OutputID from TblAbscenceAllocate  as a where a.ProjID=@ProjID and a.SchID=@SchID and a.ShiftID=@ShiftID and a.AbsallEmpID=@AbsallEmpID and a.AbsDateFrom=@Date ", param);
             
            return cnt;
        }
        public int UpdateLeaveStatus(Int32 ID, byte StatusID, Int64 AppUID)
        {
            try
            {
                
                DynamicParameters para = new DynamicParameters();
                para.Add("@EmpLeaveappID", ID);

                var status = "0"; //when approved rejected leave,
                if (StatusID == 2)
                    status = db.DapperSingle("Select count(*) from TblEmployeeLeaveApp where empappstatus=1 and EmpLeaveappID=@EmpLeaveappID", para);
                if (status == null)
                    status = "0";

                para.Add("@StatusID", StatusID);
                para.Add("@status", Convert.ToInt16(status));
                para.Add("@AppUID", AppUID);
                para.Add("@Output", 1, DbType.Int32, ParameterDirection.Output);
                int Result=db.DapperExecute("USP_LeaveEmpwise", para, CommandType.StoredProcedure);

                return para.Get<Int32>("Output");
            }
            catch(Exception ex) {
                return 0;
            }            
        }
        public WeekDates GetScheduleDates(Int32 SchID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@SchID", SchID);

                return db.DapperFirst<WeekDates>("select StartDate,EndDate from Tblschedule  where SchID=@SchID", param);

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public Time GetAttendanceTime(Int64 EmpID,DateTime EmpDate)
        {
            DynamicParameters param =new DynamicParameters();
            param.Add("@EmpID",EmpID);
            param.Add("@EmpDate",EmpDate);

            return db.DapperFirst<Time>("Select AttendtimeIN,AttendtimeOut,IsOFFDay from TblAttendance where EmpID=@EmpID and Attenddate=convert(date,@EmpDate)", param);
        }
        public int RemoveAttendance(TblAttendance tblattendobj)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@EmpID", tblattendobj.EmpID);
                param.Add("@Attenddate", tblattendobj.Attenddate);

                db.DapperExecute("Delete from TblAttendance where EmpID=@EmpID and Attenddate=@Attenddate", param);
                db.DapperExecute("Delete from TblTimeOFF where EmpID=@EmpID and TimeOFFDate=@Attenddate", param);
                return 1;
            }
            catch(Exception ex)
            {
                return 0;
            }
            
        }
        public Boolean GetLeaveStatus(Int64 EmpID, DateTime EmpDate)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@EmpID", EmpID);
            param.Add("@EmpDate", EmpDate);

            string count = db.DapperSingle("select count(*) from TblEmployeeLeaveApp where EmpID=@EmpID and convert(date,@EmpDate) >= EmpLeaveDate and convert(date,@EmpDate) <= EmpLeavetodate and EmpappStatus=0", param);
            if (Convert.ToInt32(count) > 0)
                return true;

            else
                return false;
        }
        public int AddTimeOff(TblTimeOFF timeoff)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@Empid", timeoff.EmpID);
                param.Add("@TimeOFFDate", timeoff.TimeOFFDate);
                param.Add("@TimeOFFStart", timeoff.TimeOFFStart);
                param.Add("@TimeOFFEnd", timeoff.TimeOFFEnd);
                param.Add("@TimeOFFRemarks", timeoff.TimeOFFRemarks);

                db.DapperExecute("Insert into TblTimeOFF(EmpID,TimeOFFDate,TimeOFFStart,TimeOFFEnd,TimeOFFRemarks) values(@Empid,convert(date,@TimeOFFDate),@TimeOFFStart,@TimeOFFEnd,@TimeOFFRemarks)", param);
                return 1;
            }
            catch(Exception ex)
            {
                return 0;
            }
           
        }
        public List<TblTimeOFF> ListTimeOFFs(Int64 EmpID, string TimeOFFDate)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@EmpID",EmpID);
                param.Add("@TimeOFFDate",common.CommonDateConvertion(TimeOFFDate));

                var list = db.DapperToList<TblTimeOFF>("Select * from TblTimeOFF where EmpID=@EmpID and TimeOFFDate=convert(date,@TimeOFFDate)", param);
                return list;

            }
            catch(Exception ex)
            {
                return null;
            }
        }
        public void RemoveTimeOFF(Int64 TimeOFFID)
        {
            try
            {
                DynamicParameters param=new DynamicParameters();
                param.Add("@TimeOFFID",TimeOFFID);
                db.DapperExecute("Delete from TblTimeOFF where TimeOFFID=@TimeOFFID",param);
            }
            catch
            {               
            }
        }
        public int UpdateLeaveStatus_EmpAssignment(EmpAbsenceAllocation absence)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@EmpID", absence.EmpID);
                param.Add("@AssignEmpID", absence.AssignEmpID);
                param.Add("@LeaveDate", absence.LeaveDate);
                param.Add("@ProjID", absence.ProjID);
                param.Add("@SchID", absence.SchID);
                param.Add("@ShiftID", absence.ShiftID);
                param.Add("@LeavetypID", absence.LeavetypID);
                param.Add("@AppUID", SessionManage.Current.AppUID);
                param.Add("@Out", 1, DbType.Int32, ParameterDirection.Output);

                int Result = db.DapperExecute("USP_AbsenceAllocation", param, CommandType.StoredProcedure);
                return param.Get<Int32>("Out");
            }
            catch
            {
                return 0;
            }
        }
        public string GetEmpName(Int64 EmpID)
        {
            DynamicParameters para = new DynamicParameters();
            para.Add("@EmpID", EmpID);
            return db.DapperSingle("Select a.CandName from TblCandidate a join TblEmployee b on a.CandID=b.CandID where b.EmpID=@EmpID", para);
        }
        public List<LeaveDetails> ListLeaveDetails(Int64 EmpID)
        {
            try
            {
                DynamicParameters paraObj = new DynamicParameters();
                paraObj.Add("@EmpID", EmpID);

                var leaveDetails = db.DapperToList<LeaveDetails>("USP_Listemployeeleaves", paraObj, CommandType.StoredProcedure);
                return leaveDetails;
            } //USP_Listemployeeleaves -also used in employeecontroller 
            catch
            {
                return null;
            }
        }
        public List<LeaveDates> GetLeaveDates(Int64 EmpID, Int16 TypeId)
        {
            DynamicParameters para = new DynamicParameters();
            para.Add("@EmpID", EmpID);
            para.Add("@TypeId", TypeId);

            return db.DapperToList<LeaveDates>("USP_GetLeaveDates", para,CommandType.StoredProcedure);
        }
        public int ApproveLeaveStatus(Int64 EmpID,Int16 LeavetypID,DataTable Dt,int Status)
        {
            SqlParameter[] param = {
                                       new SqlParameter("@Datatable_LeaveDates",SqlDbType.Structured){ Value = Dt },
                                       new SqlParameter("@Out",SqlDbType.Int){ Direction = ParameterDirection.Output },
                                       new SqlParameter("@EmpID",SqlDbType.Int){ Value = EmpID},
                                       new SqlParameter("@LeavetypID",SqlDbType.Int){ Value = LeavetypID },
                                       new SqlParameter("@AppUID",SqlDbType.BigInt) { Value = SessionManage.Current.AppUID },
                                       new SqlParameter("@Status",SqlDbType.Int) { Value = Status }
                                   };

            int result = db.ExecuteWithDataTable("USP_ApproveLeaveDates", param, CommandType.StoredProcedure);
            string output = param[1].Value.ToString();
            int OutData = Convert.ToInt32(output);
            return OutData;
        }
        public void CancelLeave(Int64 EmpLeaveappID, Int16 LeavetypID, Int64 EmpID,string Date)
        {
            DynamicParameters para = new DynamicParameters();
            para.Add("@EmpLeaveappID", EmpLeaveappID);
            para.Add("@EmpID", EmpID);
            para.Add("@LeavetypID", LeavetypID);
            para.Add("@Date",common.CommonDateConvertion(Date));
            int result = db.DapperExecute("delete from TblEmployeeLeaveApp where EmpLeaveappID=@EmpLeaveappID", para);
            int result1 = db.DapperExecute("Update TblLeaveEmpwise set LeavesText=LeavesText+1, LeavesTaken=LeavesTaken-1  where EmpID=@EmpID and LeavetypID=@LeavetypID", para);
            int result2 = db.DapperExecute("Delete from TblAbscenceAllocate where AbsEmpID=@EmpID and convert(date,AbsDateFrom)=convert(date,'@Date')", para);
        }
        public int AuthorizeStatus(Int64 EmpID, Int16 LeavetypID,Int16 ApprovdLeavetype, int status, string Date,int flag)
        {
            try
            {
                DynamicParameters para = new DynamicParameters();
                para.Add("@EmpID", EmpID);
                para.Add("@LeavetypID", LeavetypID);
                para.Add("@ApprovdLeavetype", ApprovdLeavetype);
                para.Add("@status", status);
                para.Add("@AppUID", SessionManage.Current.AppUID);
                para.Add("@Date", common.CommonDateConvertion(Date));
                para.Add("@flag", flag);
                para.Add("@Output", 1, DbType.Int32, ParameterDirection.Output);
                int result = db.DapperExecute("USP_UpdateAuthorize", para, CommandType.StoredProcedure);
                return para.Get<Int32>("Output");
            }
            catch(Exception ex)
            {
                return 0;
            }            
        }
        public int UnpaidApproveLeave(Int64 EmpID, Int16 LeavetypID, DataTable Dt)
        {
            SqlParameter[] param = {
                                       new SqlParameter("@Datatable_LeaveDates",SqlDbType.Structured){ Value = Dt },
                                       new SqlParameter("@Out",SqlDbType.Int){ Direction = ParameterDirection.Output },
                                       new SqlParameter("@EmpID",SqlDbType.Int){ Value = EmpID},
                                       new SqlParameter("@LeavetypID",SqlDbType.Int){ Value = LeavetypID },
                                       new SqlParameter("@AppUID",SqlDbType.BigInt) { Value = SessionManage.Current.AppUID }
                                   };

            int result = db.ExecuteWithDataTable("USP_UnpaidApproveLeave", param, CommandType.StoredProcedure);
            string output = param[1].Value.ToString();
            int OutData = Convert.ToInt32(output);
            return OutData;
        }

    }
}