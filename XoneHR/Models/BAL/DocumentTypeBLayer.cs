using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using XoneHR.Models;
using System.Web;
using System.Data;


namespace XoneHR.Models
{
    public class DocumentTypeBLayer
    {
        private XoneDbLayer db;
        public DocumentTypeBLayer()
        {
            db = new XoneDbLayer();
        }
        public IEnumerable<TblDocumentTypes> GetDocumentTypeList()
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                return db.DapperIEnumerable<TblDocumentTypes>("select * from TblDocumentTypes", param);

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public int ManageDocumentType(int flag, string DocumentTypeName = "", Int32 status = 0, int DocTypID = 0)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@DocumentTypeName", DocumentTypeName);
            param.Add("@flag", flag);
            param.Add("@DocTypID", DocTypID);
            param.Add("@Status",Convert.ToBoolean(status));
            param.Add("@OUT", 1, DbType.Int32, ParameterDirection.Output);

            db.DapperExecute("USP_SaveDocumentType", param, CommandType.StoredProcedure);
            Int32 output = param.Get<Int32>("OUT");
            return output;
        }

    }
}