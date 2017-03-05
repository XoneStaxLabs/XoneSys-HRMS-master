using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using XoneHR.Models;
using System.Web;
using System.Data;


namespace XoneHR.Models
{
    public class DocumentSubTypeBLayer
    {
         private XoneDbLayer db;
         public DocumentSubTypeBLayer()
        {
            db = new XoneDbLayer();
        }
        public IEnumerable<TblDocumentSubTypes> GetDocumentSubTypeList()
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                return db.DapperIEnumerable<TblDocumentSubTypes>("select a.DocStypID,a.DocStypName,a.DocStypStatus,a.DocTypID,b.DocTypName "+
                    "from TblDocumentSubTypes a join TblDocumentTypes b on a.DocTypID=b.DocTypID  ", param);

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IEnumerable<TblDocumentTypes> GetDocumentType()
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                return db.DapperIEnumerable<TblDocumentTypes>("select * from TblDocumentTypes where DocTypStatus=1", param);

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public int ManageDocumentSubType(int flag, string DocStypName = "", int DocTypID = 0, Int32 status = 0, int DocStypID = 0)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@DocStypName", DocStypName);
            param.Add("@flag", flag);
            param.Add("@DocTypID", DocTypID);
            param.Add("@DocStypID", DocStypID);
            param.Add("@Status",Convert.ToBoolean(status));
            param.Add("@OUT", 1, DbType.Int32, ParameterDirection.Output);

            db.DapperExecute("USP_SaveDocumentSubType", param, CommandType.StoredProcedure);
            Int32 output = param.Get<Int32>("OUT");
            return output;
        }

    }
}