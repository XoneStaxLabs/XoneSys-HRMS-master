using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace XoneHR.Models
{
    public class HolidayListBal
    {

        private XoneDbLayer db;

        public HolidayListBal()
        {
            db = new XoneDbLayer();
        }

        public IEnumerable<TblHolidayList> GetHoliday()
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                return db.DapperIEnumerable<TblHolidayList>("select *From TblHolidayList where HoliStatus=1", param);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int SaveHolidays(string HoliText, DateTime HoliDate)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@HoliText", HoliText);
                param.Add("@HoliDate", HoliDate);
                param.Add("@out", 1, DbType.Int32, ParameterDirection.Output);
                int Result = db.DapperExecute("USP_SaveHoliday", param, CommandType.StoredProcedure);

                Int32 output = param.Get<Int32>("out");
                return output;
            }
            catch(Exception ex)
            {
                return 0;
            }
            
        }
        public int Delete(int HolidayID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@HolidayID", HolidayID);
                int Result = db.DapperExecute("Delete from TblHolidayList where HolidayID=@HolidayID", param);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

    }
}