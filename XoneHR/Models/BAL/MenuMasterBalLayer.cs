using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Dapper;

namespace XoneHR.Models.BAL
{
    public class MenuMasterBalLayer
    {
        private XoneDbLayer db;
        public MenuMasterBalLayer()
        {
            db = new XoneDbLayer();
        }
        public List<GetMenuMasterList> GetMenuMasterParentList()
        {
            DynamicParameters param = new DynamicParameters(); 
            return db.DapperToList<GetMenuMasterList>("select a.MenuID,a.MenuParentID,a.MenuName,a.MenuController,a.MenuAction,a.MenuStatus from TblMenuMaster as a ");
        }
        public GetMenuMasterList GetMenuMasterData(int id)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@id", id);
            return db.DapperFirst<GetMenuMasterList>("select a.MenuID,a.MenuParentID,a.MenuName,a.MenuController,a.MenuAction,a.MenuStatus from TblMenuMaster as a where a.MenuID=@id", param);
        }
        public int ManageMenuMaster(TblMenuMaster MenuMstr)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@MenuParentID", MenuMstr.MenuParentID);
            param.Add("@MenuName", MenuMstr.MenuName);
            param.Add("@MenuController", MenuMstr.MenuController);
            param.Add("@MenuAction", MenuMstr.MenuAction);
            param.Add("@MenuIcons", MenuMstr.MenuIcons);
            param.Add("@OUT", 1,DbType.Int32, ParameterDirection.Output);
            int Result = db.DapperExecute("USP_ManageMenuMaster", param, CommandType.StoredProcedure);
            int Output = param.Get<Int32>("OUT");
            return Output;            
        }
    }
}