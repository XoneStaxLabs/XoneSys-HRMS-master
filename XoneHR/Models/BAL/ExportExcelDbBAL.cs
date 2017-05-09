using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace XoneHR.Models.BAL
{    
    public class ExportExcelDbBAL
    {
        private XoneDbLayer db;
        public ExportExcelDbBAL()
        {
            db = new XoneDbLayer();
        }
        
        public int insertIntoTable(DataTable Dt,string TblName)
        {
            DynamicParameters para = new DynamicParameters();
            StringBuilder sb = new StringBuilder();
            // db.DapperExecute("CREATE TYPE [dbo].[UserDefineType] AS TABLE([][nvarchar(200)] NULL, [][nvarchar(200)] NULL)", para);
            sb.Append("CREATE TYPE [dbo].[UserDefineType] AS TABLE( ");
            foreach (DataColumn column in Dt.Columns)
            {
                sb.Append(" ["+column.ColumnName+ "][nvarchar](200) NULL ,");
            }
            
            
            db.DapperExecute(""+ sb.ToString().TrimEnd(',') + ")", para);

            SqlParameter[] param = {
                                    new SqlParameter("@UserDefineType",SqlDbType.Structured){ Value=Dt },
                                    new SqlParameter("@TableName",SqlDbType.NVarChar) { Value=TblName },
                                    new SqlParameter("@OUT",SqlDbType.Int){ Direction=ParameterDirection.Output }
                                };
            int Result = db.ExecuteWithDataTable("USP_ExceltoDatabase", param, CommandType.StoredProcedure);
            return 0;
        }

    }
}