using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace XoneHR.Models.BAL
{
    public class SetOffBalLayer
    {
        private XoneDbLayer db;

        public SetOffBalLayer()
        {
            db = new XoneDbLayer();
        }

        public List<Employees> GetEmployees()
        {
            try
            {
                var emp = db.DapperToList<Employees>("Select a.EmpID,b.CandName from TblEmployee a join TblCandidate b on a.CandID=b.CandID where a.EmpTypID=1");
                return emp;
                
            }
            catch
            {
                return null;
            }
        }

        public int AllowanceEmpAssignmentSave(DataTable Dt,DateTime OFFDay)
        {
           
            SqlParameter[] param = {
                                       new SqlParameter("@EmpIds",SqlDbType.Structured){ Value=Dt },
                                       new SqlParameter("@OFFDay",SqlDbType.DateTime){Value=OFFDay},
                                       new SqlParameter("@Outs",SqlDbType.Int){ Direction=ParameterDirection.Output }                                       
                                       
                                   };


            int result = db.ExecuteWithDataTable("USP_EmployeeOffs", param, CommandType.StoredProcedure);
            string output = param[2].Value.ToString();
            int OutData = Convert.ToInt32(output);
            return OutData;

        }

    }
}