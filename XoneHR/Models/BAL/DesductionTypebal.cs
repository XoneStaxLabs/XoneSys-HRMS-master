using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using XoneHR.Models;
using System.Web;
using System.Data;

namespace XoneHR.Models
{
    public class DesductionTypebal
    {
        private XoneDbLayer db;
        public DesductionTypebal()
        {
            db = new XoneDbLayer();
        }

        public IEnumerable<TblDeductionType> GetDeductionTypeList()
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                return db.DapperIEnumerable<TblDeductionType>("select * from TblDeductionType", param);

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int ManageDeductionType(int flag, string DeductionTypeName = "", int DeductType_Id = 0)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@DeductionTypeName", DeductionTypeName);
            param.Add("@flag", flag);
            param.Add("@DeductType_Id", DeductType_Id);            
            param.Add("@OUT", 1, DbType.Int32, ParameterDirection.Output);

            db.DapperExecute("USP_SaveDeductionType", param, CommandType.StoredProcedure);
            Int32 output = param.Get<Int32>("OUT");
            return output;
        }

    }
}