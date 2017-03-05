using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace XoneHR.Models.BAL
{
    public class ImportEmpDetailsFromExcelBal
    {
        private XoneDbLayer db;
        public ImportEmpDetailsFromExcelBal()
        {
            db = new XoneDbLayer();
        }

        public int insertIntoTemp(DataTable dt,int flag)
        {
            int OutData;
            if (flag == 1)
            {
                SqlParameter[] param = {
                                       new SqlParameter("@TT_EXCELTEMPDATA",SqlDbType.Structured){ Value=dt },
                                       new SqlParameter("@OUT",SqlDbType.Int){ Direction=ParameterDirection.Output },
                                       new SqlParameter("@FLAG",SqlDbType.SmallInt){Value = flag}
                                   };
                int Result = db.ExecuteWithDataTable("USP_MANAGE_EXCEL_DATA", param, CommandType.StoredProcedure);
                string output = param[1].Value.ToString();
                OutData = Convert.ToInt32(output);
                return OutData;
            }
            else
            {
                SqlParameter[] param = {
                                       new SqlParameter("@TT_EXCELTEMPDATA2",SqlDbType.Structured){ Value=dt },
                                       new SqlParameter("@OUT",SqlDbType.Int){ Direction=ParameterDirection.Output }
                                   };
                int Result = db.ExecuteWithDataTable("USP_MANAGE_EXCEL_DATA2", param, CommandType.StoredProcedure);
                string output = param[1].Value.ToString();
                OutData = Convert.ToInt32(output);
                return OutData;
            }            
            
        }
              

        public int ProcessTempData()
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@OUT", 1);
            int Result = db.DapperExecute("USP_PROCESS_TEMP_EXCELDATA", param, CommandType.StoredProcedure);
            int OutData = param.Get<int>("OUT");
            return OutData;
        }

        public int ProcessTempData2()
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@OUT", 1);
            int Result = db.DapperExecute("USP_PROCESS_TEMP_EXCELDATA2", param, CommandType.StoredProcedure);
            int OutData = param.Get<int>("OUT");
            return OutData;
        }

    }
}