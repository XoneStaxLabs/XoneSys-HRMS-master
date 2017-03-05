using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using System.Data;

namespace XoneHR.Models.BAL
{
    public class EmpCheckListMasterBalLayer
    {
         private XoneDbLayer db;
         public EmpCheckListMasterBalLayer()
        {
            db = new XoneDbLayer();
        }
         public List<TblCheckListTypes> GetEmpChkLst()
        {
            try
            {
                return db.DapperToList<TblCheckListTypes>("select  * from TblCheckListTypes");
            }
            catch(Exception ex)
            {
                return null;
            }
        }

         public int ManageEmpChkLst(TblCheckListTypes ChklstObj, int Flag)
         {
             try
             {
                 DynamicParameters param = new DynamicParameters();

                 param.Add("@CheckListTypeID", ChklstObj.CheckListTypeID);
                 param.Add("@CheckListDetails", string.IsNullOrEmpty(ChklstObj.CheckListDetails) ? "" : ChklstObj.CheckListDetails.Trim());
                 param.Add("@price", ChklstObj.Price);
                 param.Add("@Flag", Flag);                
                 param.Add("@OUT", 1, DbType.Int32, ParameterDirection.Output);
                 db.DapperExecute("USP_ManageEmpCheckListMaster", param, CommandType.StoredProcedure);
                 Int32 output = param.Get<Int32>("OUT");
                 return output;

             }
             catch (Exception ex)
             {
                 return 0;
             }
         }

         public int DeleteEmpChkLst(int CheckListTypeID)
         {
             try
             {
                 DynamicParameters param = new DynamicParameters();
                 param.Add("@CheckListTypeID", CheckListTypeID);
                 return db.DapperExecute("delete from TblCheckListTypes where CheckListTypeID=@CheckListTypeID", param);
             }
             catch (Exception ex)
             {
                 return 0;
             }
         }
    }
}