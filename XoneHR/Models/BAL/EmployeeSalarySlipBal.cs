using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using System.Data;

namespace XoneHR.Models
{
    public class EmployeeSalarySlipBal
    {
        private XoneDbLayer db;
        public EmployeeSalarySlipBal()
        {
            db = new XoneDbLayer();
        }

        public SalaryslipContent GetSalarySlip(Int64 year, int month, Int64 emp)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@month",month);
                param.Add("@year",year);
                param.Add("@EmpID", emp);
                param.Add("@Out", 1, DbType.Int32, ParameterDirection.Output);
                //return db.DapperFirst<SalaryslipContent>("USP_PayrollSalarySlip", param, CommandType.StoredProcedure);
                return db.DapperFirst<SalaryslipContent>("USP_PayrollSalarySlipDayWise", param, CommandType.StoredProcedure);
            
            }
            catch (Exception ex)
            {
                return null;
            } 
        }

        public List<EmployeeProfile> ListEmployees()
        {
            try
            {
                List<EmployeeProfile> ListEmp = db.DapperToList<EmployeeProfile>("select b.EmpID,a.CandName from TblCandidate a join TblEmployee b on a.CandID=b.CandID");
                return ListEmp;
            }
            catch
            {
                return null;
            }
        }

        public List<TblSalaryPayComponent> GetAdvancedetails(Int64 year, int month, Int64 emp)
        {
            try
            {

                //List<TblSalaryPay> salapay = new List<TblSalaryPay>();
                //TblSalaryPay sala = new TblSalaryPay();
                //sala.PayDate = DateTime.Now;
                //sala.PayAmount = 101;
                //salapay.Add(sala);
                //return salapay;

                DynamicParameters param = new DynamicParameters();
                param.Add("@month", month);
                param.Add("@year", year);
                param.Add("@EmpID", emp);
              //  PayType 1=Regularpayment  & 2=Advance payment
                List<TblSalaryPayComponent> ListSal = db.DapperToList<TblSalaryPayComponent>("Select PayId,EmpId,SalYear,SalMonth,PayAmount,PayType,format(PayDate,'dd/MM/yyyy') as PayDate,PayMode from TblSalaryPay where EmpId=@EmpID and SalMonth=@month and SalYear=@year and PayType=2", param);

               // List<TblSalaryPay> ListSal = db.DapperToList<TblSalaryPay>("USP_ListAdvancePay",CommandType.StoredProcedure);


                return ListSal;
            }
            catch(Exception ex)
            {
                
                return null;
            }
        }

        public List<TblFixedAllowanceEmpTypes> GetAllowanceDetails(Int64 year, int month, Int64 emp)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@month", month);
                param.Add("@year", year);
                param.Add("@EmpID", emp);

                List<TblFixedAllowanceEmpTypes> ListAllowance = db.DapperToList<TblFixedAllowanceEmpTypes>("select b.FixallEmpTypes,  CAST(round(b.FixallEmpAmounts, 2) AS FLOAT) as FixallEmpAmounts from TblFixedAllowanceEmpwise a join TblFixedAllowanceEmpTypes b on a.FixAllEmpD=b.FixAllEmpD where EmpID=@EmpID and a.FixAllalloMonth=@month and a.FixAllalloYear=@year and a.FixAllEmpStatus=1 and b.FixAllEmpStatus=1", param);

                return ListAllowance;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public string EmployeeName(Int64 EmpID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@EmpID", EmpID);
            return db.DapperSingle("Select CandName from TblCandidate a join TblEmployee b on a.CandID=b.CandID where b.EmpID=@EmpID", param);
        }

        public int CheckLeaveStatus(Int64 year, int month, Int64 EmpID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@month", month);
            param.Add("@year", year);
            param.Add("@EmpID", EmpID);

            var cnt = db.DapperSingle("Select count(*) from TblEmployeeLeaveApp where EmpID=@EmpID and EmpappStatus=1 and DATEPART(month,EmpLeaveDate)=@month and DATEPART(year,EmpLeaveDate)=@year and DATEPART(month,EmpLeavetodate)=@month and DATEPART(year,EmpLeavetodate)=@year", param);
            return Convert.ToInt32(cnt);
        }

    }
}