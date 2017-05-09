using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace XoneHR.Models.BAL
{
    public class PayrollBal
    {
        private XoneDbLayer db;

        public PayrollBal()
        {
            db = new XoneDbLayer();
        }

        public List<EmployeeDetails> ListAllEmployees()
        {
            try
            {
                DynamicParameters Paraobj = new DynamicParameters();

                var Employeelists = db.DapperToList<EmployeeDetails>(" select *from TblEmployee empl join TblCandidate candidate on empl.CandID=candidate.CandID join TblDesignation desig on candidate.DesigID=desig.DesigID " +
                                    "join TblRace race on race.RaceID = candidate.RaceID join TblReligion relig on relig.ReligID = candidate.ReligID join TblMaritalStatus matstatus on matstatus.MaritID = candidate.MaritID " +
                                    "join TblCityZenship citizn on citizn.CizenID = candidate.CizenID left join TblEmployeeType emptype on emptype.EmpTypID = empl.EmpTypID left join TblOffBoarding offboard on empl.EmpID = offboard.EmpID " +
                                    "left join TblGrade grd on candidate.GradeID=grd.GradeID where empl.EmpStatus = 1  AND Emp_IsApproved = 1", Paraobj);
                                    
                return Employeelists;
            }
            catch
            {
                return null;
            }
        }

        public  IEnumerable<SummaryDetails> Getsummarydetails(Int64 year, int month, Int64 emp)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@month", month);
                param.Add("@year", year);
                param.Add("@EmpID", emp);
                param.Add("@Out", 1, DbType.Int32, ParameterDirection.Output);
                //return db.DapperFirst<SalaryslipContent>("USP_PayrollSalarySlip", param, CommandType.StoredProcedure);
                return db.DapperIEnumerable<SummaryDetails>("USP_GetSummaryDetails", param, CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IEnumerable<calenderSummaryDetails> Getcalendersummarydetails(Int64 year, int month, Int64 emp)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@month", month);
                param.Add("@year", year);
                param.Add("@EmpID", emp);
                param.Add("@Out", 1, DbType.Int32, ParameterDirection.Output);
                //return db.DapperFirst<SalaryslipContent>("USP_PayrollSalarySlip", param, CommandType.StoredProcedure);
                return db.DapperIEnumerable<calenderSummaryDetails>("USP_GetCalenderSummaryDetails", param, CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                return null;
            }
        }


    }

}