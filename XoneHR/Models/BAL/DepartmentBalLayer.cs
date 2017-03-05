using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using System.Data;

namespace XoneHR.Models.BAL
{
    public class DepartmentBalLayer
    {
        private XoneDbLayer db;
        public DepartmentBalLayer()
        {
            db = new XoneDbLayer();
        }
        public List<TblDepartment> GetDepartmentList()
        {
            try
            {
                return db.DapperToList<TblDepartment>("select  * from TblDepartment");
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        public int ManageDepartment(TblDepartment DepObj, int Flag, int DepStatus)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@DepUserTyp", DepObj.DepUserTyp);
                param.Add("@DepID", DepObj.DepID);
                param.Add("@DepName", string.IsNullOrEmpty(DepObj.DepName)? "" : DepObj.DepName.Trim());
                param.Add("@Flag", Flag);
                param.Add("@DepStatus", Convert.ToBoolean(DepStatus));
                param.Add("@OUT", 1, DbType.Int32, ParameterDirection.Output);
                db.DapperExecute("USP_ManageDepartmentMaster",param,CommandType.StoredProcedure);
                Int32 output = param.Get<Int32>("OUT");
                return output;

            }catch(Exception ex)
            {
                return 0;
            }
        }
        public int DeleteDepartment(int DepID)
        {
            try
            {                
                DynamicParameters param = new DynamicParameters();
                param.Add("@DepID", DepID);
                param.Add("@OUT", 1, DbType.Int32, ParameterDirection.Output);
                db.DapperExecute("USP_DeleteDepartmentMaster", param, CommandType.StoredProcedure);
                Int32 output = param.Get<Int32>("OUT");
                return output;
                //return db.DapperExecute("delete from TblDepartment where DepID=@DepID", param);
            }
            catch(Exception ex)
            {
                return 0;
            }
        }
    }
}