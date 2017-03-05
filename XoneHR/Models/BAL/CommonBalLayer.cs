using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace XoneHR.Models
{
    public class CommonBalLayer
    {
        private XoneDbLayer db;
        public CommonBalLayer()
        {
            db = new XoneDbLayer();
        }

        public List<Permission> GetPermissionList(string Action, string Controller)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@Action", Action);
                param.Add("@Controller", Controller);
                param.Add("@AppUID", SessionManage.Current.AppUID);

                List<Permission> List = db.DapperToList<Permission>("USP_ListPermissions", param,CommandType.StoredProcedure);
                return List;
            }
            catch(Exception ex)
            {
                return null;
            }            
        }

        public bool GetPermissionStatus(string Action, string Controller)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@Action", Action);
                param.Add("@Controller", Controller);
                param.Add("@AppUID", SessionManage.Current.AppUID);
                           
                bool status = db.DapperBoolean("Select b.UserPermiStatus from TblMenuMaster a join TblMenuUserPermission b on a.MenuID=b.MenuID where b.AppUserID=@AppUID and a.MenuAction=@Action and a.MenuController=@Controller and a.MenuStatus=1", param);
                return status;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        //public List<TblFunctionType> GetFunctionTypes()
        //{
        //    try
        //    {
        //        return db.DapperToList<TblFunctionType>("Select *from TblFunctionType");
        //    }
        //    catch(Exception ex)
        //    {
        //        return null;
        //    }
        //}

        public void ActionLog(string TableName, ActionType Act_Type, string Act_Controller, Int64 Act_Key, Int64 Modifiedby)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@TableName", TableName);
            param.Add("@Act_Type", Convert.ToInt32(Act_Type));
            param.Add("@Act_Controller", Act_Controller);
            param.Add("@Act_Key", Act_Key);
            param.Add("@Modifiedby", Modifiedby);

            int Tbl_ID = db.DapperExecute("Select Tblk_ID from TblEntity where Tbl_Name=@TableName", param);
            param.Add("@Tblk_ID", Tbl_ID);
            param.Add("@Output", null, DbType.Int32, ParameterDirection.Output);
            int Result = db.DapperExecute("USP_InsertLog", param, CommandType.StoredProcedure);
        }

    }
}