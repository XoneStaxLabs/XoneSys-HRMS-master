using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using XoneHR.Models;
using System.Data;

namespace XoneHR.Models
{
    public class RaceBLayer
    {
        private XoneDbLayer db;

        public RaceBLayer()
        {
            db = new XoneDbLayer();
        }
        public IEnumerable<TblRace> GetRaceList()
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                return db.DapperIEnumerable<TblRace>("select * from TblRace", param);
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        public int ManageRace(int flag, string RaceName = "", Int32 status = 0, Int16 RaceID = 0)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@RaceName", RaceName.Trim());
            param.Add("@flag", flag);
            param.Add("@RaceID", RaceID);
            param.Add("@Status", Convert.ToBoolean(status));
            param.Add("@OUT", 1, DbType.Int32, ParameterDirection.Output);

            db.DapperExecute("USP_ManageRace", param, CommandType.StoredProcedure);
            Int32 output = param.Get<Int32>("OUT");
            return output;
        }
    }
}