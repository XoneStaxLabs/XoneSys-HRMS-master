using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace XoneHR.Models
{
    public class CityzenBalLayer
    {
        private XoneDbLayer db;
        public CityzenBalLayer()
        {
            db=new XoneDbLayer();
        }

        public IEnumerable<TblCityZenship> List()
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                return db.DapperIEnumerable<TblCityZenship>("Select CizenID,Citizen,CitiZenCode,CitizenNoname,IsStatus from TblCityZenship");
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public int CityzenCreate(string CityZen, string CityZenNoname, int flag, int CizenID = 0, int status=0)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@flag", flag);
            param.Add("@CizenID", CizenID);
            param.Add("@Citizen", CityZen);
            param.Add("@CitiZenNoname", CityZenNoname);
            param.Add("@OUT", 1, DbType.Int32, ParameterDirection.Output);
            param.Add("@status", Convert.ToBoolean(status));

            db.DapperExecute("USP_SaveCityzen", param, CommandType.StoredProcedure);
            Int32 output = param.Get<Int32>("OUT");
            return output;
        }

    }
}