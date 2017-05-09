using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using System.Data;

namespace XoneHR.Models
{
    public class EmployeesListBal
    {
        CommonFunctions common = new CommonFunctions();
        private XoneDbLayer db;
        public EmployeesListBal()
        {
            db = new XoneDbLayer();
        }

        public List<SalaryslipContents> GetSalarySlip(Int64 year, int month) 
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@month", month);
                param.Add("@year", year);              
                param.Add("@Out", 1, DbType.Int32, ParameterDirection.Output);
                List<SalaryslipContents> SalarySlip = db.DapperToList<SalaryslipContents>("USP_SalarySpreadsheet_New", param, CommandType.StoredProcedure);
                return SalarySlip;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<EmpBasicDetails> GetEmployeeDetails(Int64 year,int month)
        {
            try
            {
                DynamicParameters para = new DynamicParameters();
                para.Add("@month", month);
                para.Add("@year", year);
                string datestring = db.DapperSingle("Select top 1 convert(date,Attenddate) from TblAttendance where datepart(month,Attenddate) = @month AND  datepart(year,Attenddate) = @year", para);
                DateTime date = Convert.ToDateTime(datestring);
                para.Add("@date", date);
                date = common.CommonDateConvertion(Convert.ToString(date.ToShortDateString()));
                var list = db.DapperToList<EmpBasicDetails>("Select  a.CandName,a.CandNRICNo,a.CandDob,a.CandAge,b.EmpID,a.CizenID,b.FundType,b.CPF_Status,b.EmpLevyAmnt,c.BasicSalary from TblCandidate a join TblEmployee b on a.CandID=b.CandID join TblEmployeeSalary c on b.EmpID=c.EmpID "+
                           "where b.EmpStatus=1 and( @date between c.SalStartDate and c.SalEndDate or c.SalStartDate <= @date and SalEndDate is null)", para);
                
                return list;                
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<TblAttendance> GetAttendance(Int64 year, int month)
        {
            try
            {
                DynamicParameters para = new DynamicParameters();
                para.Add("@month", month);
                para.Add("@year", year);

                return db.DapperToList<TblAttendance>("Select * from TblAttendance where datepart(month,Attenddate) = @month AND  datepart(year,Attenddate) = @year", para);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<TblTimeOFF> GetTimeOFFDetails(Int64 year, int month)
        {
            try
            {
                DynamicParameters para = new DynamicParameters();
                para.Add("@month", month);
                para.Add("@year", year);

                return db.DapperToList<TblTimeOFF>("Select * from TblTimeOFF where datepart(month,TimeOFFDate) = @month AND  datepart(year,TimeOFFDate) = @year", para);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<UpLeaveDetails> GetUPLeaves(Int64 year, int month)
        {
            try
            {
                DynamicParameters para = new DynamicParameters();
                para.Add("@month", month);
                para.Add("@year", year);
                para.Add("@out",1,DbType.Int32,ParameterDirection.Output);

                return db.DapperToList<UpLeaveDetails>("Usp_GetUpLeaves", para, CommandType.StoredProcedure);               

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IEnumerable<CommonFields> GetCommonFields(Int64 year, int month)
        {
            try
            {
                DynamicParameters para = new DynamicParameters();
                para.Add("@month", month);
                para.Add("@year", year);
                para.Add("@out", 1, DbType.Int32, ParameterDirection.Output);

                return db.DapperIEnumerable<CommonFields>("Usp_GetSalaryCalDatas", para, CommandType.StoredProcedure);     
            }
            catch (Exception ex)
            {
                return null;
            }  
        }


    }
}