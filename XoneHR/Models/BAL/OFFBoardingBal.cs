using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Dapper;
using System.Data.SqlClient;

namespace XoneHR.Models.BAL
{
    public class OFFBoardingBal
    {
        private XoneDbLayer db;

        public OFFBoardingBal()
        {
            db = new XoneDbLayer();
        }

        public List<EmployeeDetails> ListAllEmployees()
        {
            try
            {
                DynamicParameters Paraobj = new DynamicParameters();
                Paraobj.Add("@CodingNo", 2);

                var Employeelists = db.DapperToList<EmployeeDetails>("USP_ListemployeeDatatable", Paraobj, CommandType.StoredProcedure);
                return Employeelists;
            }
            catch
            {
                return null;
            }
        }

        public int AddOFFBoardingRequest(TblOffBoarding offboarding)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@OFFBoard_NoticeStart", offboarding.OFFBoard_NoticeStart);
                param.Add("@OFFBoard_NoticeEnd", offboarding.OFFBoard_NoticeEnd);
                // param.Add("@OFFBoard_Resgnation", offboarding.OFFBoard_Resgnation);
                param.Add("@OFFBoard_Remark", offboarding.OFFBoard_Remark);
                param.Add("@offboard_EmpID", offboarding.EmpID);

                // db.DapperExecute("Insert into TblOffBoarding(EmpID,OFFBoard_NoticeStart,OFFBoard_NoticeEnd,OFFBoard_Resgnation,OFFBoard_Remark,OFFBoard_Status) values(@offboard_EmpID,convert(date,@OFFBoard_NoticeStart),convert(date,@OFFBoard_NoticeEnd),null,@OFFBoard_Remark,2)", param);
                db.DapperExecute("Update TblOffBoarding set OFFBoard_NoticeStart=@OFFBoard_NoticeStart,OFFBoard_NoticeEnd=@OFFBoard_NoticeEnd,OFFBoard_Remark=@OFFBoard_Remark,OFFBoard_Status=1 where EmpID=@offboard_EmpID", param);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int UpdateOffBoardStatus(Int64 EmpID, Int16 status)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@EmpID", EmpID);
                param.Add("@status", status);
                if (status == 0)
                    db.DapperExecute("Insert into TblOffBoarding(EmpID,OFFBoard_Status) values(@EmpID,@status)", param);
                else
                    db.DapperExecute("Update TblOffBoarding set OFFBoard_Status=@status where EmpID=@EmpID", param);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public List<TblOffBoarding> ListReqstedEmployees()
        {
            var list = db.DapperToList<TblOffBoarding>("Select a.*,c.CandName from TblOffBoarding a join TblEmployee b on a.EmpID=b.EmpID join TblCandidate c on b.CandID=c.CandID where a.OFFBoard_Status!=3");
            return list;
        }

        public void OffBoardingStatus(Int64 EmpId, Int16 status)
        {
            DynamicParameters para = new DynamicParameters();
            para.Add("@EmpId", EmpId);
            para.Add("@OFFBoard_Status", status);
            db.DapperExecute("Update TblOffBoarding set OFFBoard_Status=@OFFBoard_Status where EmpID=@EmpId", para);
        }

        public List<Employees> ListEmployee()
        {
            return db.DapperToList<Employees>("Select a.EmpID,b.CandName from TblEmployee a join TblCandidate b on a.CandID=b.CandID  where EmpStatus=1 and EmpID not in(select EmpID from TblOffBoarding)");
        }

        public void Relieve()
        {
            DynamicParameters para = new DynamicParameters();
            db.DapperExecute("Update TblOffBoarding set OFFBoard_Status=2 where convert(date,OFFBoard_NoticeEnd)<=convert(date,getdate()) and OFFBoard_Status=1", para);
        }

        public void ReInstateEmployee(Int64 EmpId)
        {
            DynamicParameters para = new DynamicParameters();
            para.Add("@EmpId", EmpId);
            db.DapperExecute("Delete from TblOffBoarding where EmpID=@EmpId", para);
        }

        public void SaveChecklistDetails(Int64 EmpId)
        {
            DynamicParameters para = new DynamicParameters();
            para.Add("@EmpId", EmpId);
            db.DapperExecute("Update TblOffBoarding set OFFBoard_Status=3 where EmpID=@EmpId", para);
        }

        public void SaveCheckListTypeIDs(TblOffboardCheckList usergobj)
        {
            DynamicParameters para = new DynamicParameters();

            para.Add("@CheckListTypeID", usergobj.CheckListTypeID);
            para.Add("@CheckListID", usergobj.CheckListID);
            para.Add("@Quantity", usergobj.Quantity);
            int result = db.DapperExecute("Insert into TblOffboardCheckList(CheckListID,CheckListTypeID,Quantity) values(@CheckListID,@CheckListTypeID,@Quantity)", para);
        }

        public string EmployeeName(Int64 EmpID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@EmpID", EmpID);
            return db.DapperSingle("Select CandName from TblCandidate a join TblEmployee b on a.CandID=b.CandID where b.EmpID=@EmpID", param);
        }

        public List<OffboardList> GetCheckListElements(Int64 EmpId)
        {
            DynamicParameters para = new DynamicParameters();
            para.Add("@EmpID", EmpId);
            return db.DapperToList<OffboardList>("Select b.CheckListID,b.Quantity,a.CheckListTypeID,a.CheckListDetails from TblCheckListTypes  a  join TblOnboardCheckList  b on a.CheckListTypeID = b.CheckListTypeID where b.EmpID =@EmpID", para);
        }
    }
}