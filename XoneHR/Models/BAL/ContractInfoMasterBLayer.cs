using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;

using System.Web;
using System.Data;

using XoneHR.Models;

namespace XoneHR.Models
{
    public class ContractInfoMasterBLayer
    {
        private XoneDbLayer db;
        public ContractInfoMasterBLayer()
        {
            db = new XoneDbLayer();
        }
        public IEnumerable<TblContractInfoMaster> GetContractList()
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                return db.DapperIEnumerable<TblContractInfoMaster>("select a.ConInfoID,b.ConInfoTypID,b.ConInfoTypName,a.ConInformation," +
                    "a.ConInfoDetails,a.ConInfoStatus from TblContractInfoMaster a left join TblContractInfoType b on " +
                    "a.ConInfoTypID=b.ConInfoTypID", param);

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IEnumerable<TblContractInfoType> GetContractInfoType()
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                return db.DapperIEnumerable<TblContractInfoType>("select * from TblContractInfoType where ConInfoTypStatus=1", param);

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public int ManageContractInfo(int flag, string ConInformation = "", string ConInfoDetails = "", int ConInfoTypID = 0, Int32 status = 0, int ConInfoID = 0)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@ConInformation", ConInformation);
            param.Add("@ConInfoDetails", ConInfoDetails);
            param.Add("@flag", flag);
            param.Add("@ConInfoTypID", ConInfoTypID);
            param.Add("@ConInfoID", ConInfoID);
            param.Add("@Status", Convert.ToBoolean(status));
            param.Add("@OUT", 1, DbType.Int32, ParameterDirection.Output);

            db.DapperExecute("USP_SaveContractInfo", param, CommandType.StoredProcedure);
            Int32 output = param.Get<Int32>("OUT");
            return output;
        }

    }
}