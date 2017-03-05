using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace XoneHR.Models.BAL
{
    public class SheduleWiseEmpDetailsBal
    {
        private XoneDbLayer db;
        public SheduleWiseEmpDetailsBal()
        {
            db = new XoneDbLayer();
        }

        public List<TblProject> GetProjects()
        {
            try
            {
                return db.DapperToList<TblProject>("select ProjID,ProjName from TblProject");
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }

        public List<TblShift> GetShifts(Int64 ProjID)
        {
            try
            {
                 DynamicParameters param = new DynamicParameters();
                 param.Add("@ProjID", ProjID);
                 return db.DapperToList<TblShift>("Select ShiftID,ShiftName from TblShift where ProjID=@ProjID", param);
            }
            catch(Exception ex)
            {
                return null;
            }
           
        }

        public List<GetMoth> GetMonths(Int64 ProjID)
        {
            try
            {
                DynamicParameters para = new DynamicParameters();
                para.Add("@ProjID", ProjID);
                para.Add("@Out", 1,DbType.Int32,ParameterDirection.Output);
                return db.DapperToList<GetMoth>("USP_GetPrjectWiseMonth", para, CommandType.StoredProcedure);
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public List<ScheduleEmps>ListSchedules(Int64 ProjID,Int32 month)
        {
            try
            {
                DynamicParameters para=new DynamicParameters();
                para.Add("@ProjID",ProjID);
                para.Add("@month",month);

                return db.DapperToList<ScheduleEmps>("Select  a.SchID,count(EmpID) as EmpCount from TblEmpShift a join Tblschedule b on a.SchID=b.SchID where b.ProjID=@ProjID and b.MonthID=@month group by a.SchID", para);
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public List<ProjectSchedule> NumOfEmployees(Int64 ProjID, Int32 month)
        {
            try
            {
                DynamicParameters para=new DynamicParameters();
                para.Add("@ProjID",ProjID);
                para.Add("@month",month);

                return db.DapperToList<ProjectSchedule>("Select SchID,Schedule from Tblschedule where ProjID=@ProjID and MonthID=@month", para);
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public List<ScheduleWiseEmp> ListEmployees(Int32 SchID)
        {
            try
            {
                DynamicParameters para = new DynamicParameters();
                para.Add("@SchID", SchID);
                para.Add("@Out", 1, DbType.Int32, ParameterDirection.Output);

                var result = db.DapperToList<ScheduleWiseEmp>("USP_ListEmployeesScheduleWise", para, CommandType.StoredProcedure);
                return result;

            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public string GetSchedule(Int32 SchID)
        {
            try
            {
                DynamicParameters para = new DynamicParameters();
                para.Add("@SchID", SchID);

                return db.DapperSingle("Select Schedule from Tblschedule where SchID=@SchID", para);
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public List<AttendDate> OffDays(Int32 SchID, Int64 Empid)
        {
            try
            {
                DynamicParameters para = new DynamicParameters();
                para.Add("@SchID", SchID);
                para.Add("@EmpID", Empid);

                return db.DapperToList<AttendDate>("Select Attenddate from TblAttendance where EmpID=@EmpID and IsOFFDay=1 and SchID=@SchID", para);

            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public List<PaidUnpaidLeaves>LeaveDetails(Int32 SchID, Int64 Empid)
        {
            try
            {
                DynamicParameters para=new DynamicParameters();
                para.Add("@SchID", SchID);
                para.Add("@EmpID", Empid);
                para.Add("@Out", 1, DbType.Int32, ParameterDirection.Output);

                return db.DapperToList<PaidUnpaidLeaves>("USP_PaidUnpaidLeaves", para, CommandType.StoredProcedure);
            }
            catch(Exception ex)
            {
                return null;
            }
        }

    }
}