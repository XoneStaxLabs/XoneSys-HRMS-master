using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using XoneHR.Models;
using System.Data;


namespace XoneHR.Models
{
    public class LanguageKnownBLayer
    {
        private XoneDbLayer db;

        public LanguageKnownBLayer()
        {
            db = new XoneDbLayer();
        }
        public IEnumerable<TblLanguageKnown> GetLanguageKnownList()
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                return db.DapperIEnumerable<TblLanguageKnown>("select *From TblLanguageKnown", param);
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        public int ManageLanguageKnown(int flag, string LanguageName = "", Int32 status = 0, Int16 LangknID = 0)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@LanguageName", LanguageName.Trim());
            param.Add("@flag", flag);
            param.Add("@LangknID", LangknID);
            param.Add("@Status", Convert.ToBoolean(status));
            param.Add("@OUT", 1, DbType.Int32, ParameterDirection.Output);
            db.DapperExecute("USP_ManageLanguageKnown", param, CommandType.StoredProcedure);
            Int32 output = param.Get<Int32>("OUT");
            return output;
        }
    }
}