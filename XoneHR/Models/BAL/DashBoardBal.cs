using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using System.Data;


namespace XoneHR.Models.BAL
{
    public class DashBoardBal
    {

        private XoneDbLayer db;
        public DashBoardBal()
        {
            db=new XoneDbLayer();
        }


        public DashBoardUsercount DashBordUserCounts()
        {
            try
            {

                DynamicParameters para = new DynamicParameters();
                para.Add("@status", 1);

                DashBoardUsercount usercount = new DashBoardUsercount();
                usercount.CandidateCount = db.DapperSingle("select COUNT(*) as CandidateCount from TblCandidate cand join TblCandidateApproval candapp on cand.CandID=candapp.CandID where CandStatus=@status and CandAppStatus=2", para);
                usercount.EmployeeCount = db.DapperSingle("select COUNT(*) as EmployeeCount from TblEmployee where EmpStatus=@status", para);
                usercount.AppUsersCount = db.DapperSingle("select COUNT(*) as AppUsersCount from TblAppUser where AppStatus=@status", para);
                usercount.Projectcount = db.DapperSingle("select COUNT(*) as Projectcount from TblProject where ProjStatus=@status", para);
                usercount.LeaveCount = LeaveCount();

                return usercount;
            }
            catch
            {
                DashBoardUsercount usercount = new DashBoardUsercount();
                usercount.CandidateCount = "0";
                usercount.EmployeeCount = "0";
                usercount.AppUsersCount ="0";
                usercount.Projectcount = "0";
                return usercount;
            }
        }

        public List<DashBoardusers> DashBoardusers()
        {
            try
            {
                var Users = db.DapperToList<DashBoardusers>("select top 8 emp.EmpID,emp.EmpStartDate,cand.CandName,cand.CandPhoto from TblEmployee emp join TblCandidate cand on emp.CandID=cand.CandID where emp.EmpStatus=1 and Emp_IsApproved=1 order by EmpID desc");
                return Users;
            }
            catch
            {
                return null;
            }

        }


        public List<AbsenceAllocation> AbsenceAllocation()
        {
            try
            {
                var Users = db.DapperToList<AbsenceAllocation>("USP_ListAbsentEmployeeAlloclist",CommandType.StoredProcedure);
                return Users;
            }
            catch
            {
                return null;
            }

        }



        public List<MemosUser> ListMemos(Int64 userID)
        {
            try
            {
                DynamicParameters para = new DynamicParameters();
                para.Add("@AppUID", userID);

                var listmemos = db.DapperToList<MemosUser>("USP_ListMemoms", para,CommandType.StoredProcedure);
                return listmemos;
            }
            catch
            {
                return null;
            }
        }


        public List<MemoDocuments> ListMemosDocuments(int memoID)
        {
            try
            {
                DynamicParameters para = new DynamicParameters();
                para.Add("@memoid", memoID);

                var listmemosDocs = db.DapperToList<MemoDocuments>("select *from TblMemoDocument where MemoID=@memoid", para);
                return listmemosDocs;
            }
            catch
            {
                return null;
            }
        }

        public Int32 AddnewmemosReply(TblMemos memoObj,int appuID)
        {
            try
            {
                DynamicParameters paraObj = new DynamicParameters();
                paraObj.Add("@MemoSubject", memoObj.MemoSubject);
                paraObj.Add("@MemoText", memoObj.MemoText);
                paraObj.Add("@MemoPriority", memoObj.MemoPriority);
                paraObj.Add("@MemoStatus", true);
                paraObj.Add("@MemoAddedby", SessionManage.Current.AppUID);

                paraObj.Add("@appuID", appuID);
                paraObj.Add("@MemoRepID",memoObj.MemoRepID);



                db.DapperExecute("USP_AddMemosReply", paraObj, CommandType.StoredProcedure);           

                return 1;
            }
            catch
            {

                return 0;
            }
        }


        public List<PassportExpiryLists> PassportExpiryLists()
        {
            try
            {
                var ExpiryLists = db.DapperToList<PassportExpiryLists>("USP_ListPassportExpiry",CommandType.StoredProcedure);
                return ExpiryLists;
            }
            catch
            {
                return null;
            }
        }

        public List<PLRDExpiryLists>PLRDExpiryLists()
        {
            try
            {
                var ExpiryLists = db.DapperToList<PLRDExpiryLists>("USP_ListPLRDExpiry", CommandType.StoredProcedure);
                return ExpiryLists;
            }
            catch
            {
                return null;
            }
        }

        public List<PassportExpiryLists> PassportExpiryListsTopFive()
        {
            try
            {
                var ExpiryLists = db.DapperToList<PassportExpiryLists>("USP_ListPassportExpirytopFive", CommandType.StoredProcedure);
                return ExpiryLists;
            }
            catch
            {
                return null;
            }
        }

        public List<PLRDExpiryLists>PLRDExpiryListsTopFive()
        {
            try
            {
                var ExpiryLists = db.DapperToList<PLRDExpiryLists>("USP_PLRDExpirytopFive", CommandType.StoredProcedure);
                return ExpiryLists;
            }
            catch
            {
                return null;
            }
        }

        public List<LeaveApplication> ListLeaveDetails()
        {
            try
            {


                var leaveDetails = db.DapperToList<LeaveApplication>("USP_ListleaveDetails", CommandType.StoredProcedure);
                return leaveDetails;
            }
            catch
            {
                return null;
            }
        }

        public string LeaveCount()
        {
            try
            {
                DynamicParameters paraObj = new DynamicParameters();
                paraObj.Add("@status", 0);

                var leaveDetails = db.DapperSingle("select count (*) from TblEmployeeLeaveApp where EmpappStatus=@status", paraObj);
                return leaveDetails;
            }
            catch
            {
                return null;
            }
        }

        public List<TblHolidayList> HolidayList()
        {
            try
            {
                var List = db.DapperToList<TblHolidayList>("Select HolidayID,HoliText,convert(date,HoliDate) as HoliDate  from TblHolidayList");
                return List;
            }

            catch(Exception ex)
            {
                return null;
            }
        }


    }
}