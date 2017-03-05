using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using System.Data;
using System.Data.SqlClient;


namespace XoneHR.Models
{
    public class DesignationPermissionBalLayer
    {
        private XoneDbLayer db;
        public DesignationPermissionBalLayer()
        {
            db = new XoneDbLayer();
        }
        public List<TblDepartment> GetDepartment(Int16 UserType)
        {
            DynamicParameters para = new DynamicParameters();
            para.Add("@UserType", UserType);
            return db.DapperToList<TblDepartment>("select * from TblDepartment where DepUserTyp=@UserType and DepStatus=1", para);
        }
        public DesignationList GetAppDesigDetails(Int32 ID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@DesigID", ID);
            return db.DapperFirst<DesignationList>("select * from TblDesignation as a join TblDepartment as b on a.DepID=b.DepID where a.DesigID=@DesigID and DesigStatus=1", param);
        }
        public List<DesignationList> GetDesignation()
        {

            return db.DapperToList<DesignationList>("select * from TblDesignation as a join TblDepartment as b on a.DepID=b.DepID where a.DesigUserTyp=0 and a.DesigStatus=1");
                         
        }
        public List<GetMenuPermission> GetJqxJsonData(Int32 DesigID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@DesigID", DesigID);

            //var count = db.DapperFirst<GetMenuPermission>("select COUNT(DesigMenuID) as cnt from TblMenuDesigPermission where DesigID=@DesigID", param);

            if (DesigID == 0)
            {
                return db.DapperToList<GetMenuPermission>("select a.MenuID,a.MenuParentID,a.MenuName,a.MenuStatus as MenuStatus from TblMenuMaster as a where a.MenuStatus=1");
            }
            else
            {
               // return db.DapperToList<GetMenuPermission>("select a.MenuID,a.MenuParentID,a.MenuName,b.DesigPermistatus as MenuStatus from TblMenuMaster as a left JOIN TblMenuDesigPermission as b on a.MenuID=b.MenuID where b.DesigID=@DesigID and a.MenuStatus=1", param);
                return db.DapperToList<GetMenuPermission>("select t.MenuID,t.MenuParentID,t.MenuName, ISNULL(r.DesigPermistatus, 0 ) as MenuStatus  FROM " +
                                                          "(select a.MenuID,a.MenuParentID,a.MenuName,a.MenuStatus as MenuStatus from TblMenuMaster as a where a.MenuStatus=1) as t " +
                                                          "LEFT OUTER JOIN (SELECT c.MenuID,c.DesigPermistatus FROM TblMenuDesigPermission c WHERE c.DesigID=@DesigID) AS r  on t.MenuID=r.MenuID", param);

            }

            
        }
        public int ManageDesigPermission(DataTable DT, string DesigID, Int16 AppDepID, string AppDesigName, Int16 Flag,Int16 DesigUserTyp)
        {
            Int32 Desig_ID = 0;
            if (DesigID != null )
            {
                Desig_ID = Convert.ToInt32(DesigID);
            }

            SqlParameter[] param = {
                                       new SqlParameter("@Datatable_MenuID",SqlDbType.Structured){ Value=DT }, 
                                       new SqlParameter("@Out",SqlDbType.Int){ Direction=ParameterDirection.Output },  
                                       new SqlParameter("@DesigID",SqlDbType.Int){Value = Desig_ID},
                                       new SqlParameter("@AppDepID",SqlDbType.SmallInt){Value = AppDepID},
                                       new SqlParameter("@AppDesigName",SqlDbType.VarChar){Value = AppDesigName},
                                       new SqlParameter("@Flag",SqlDbType.SmallInt){Value = Flag},
                                       new SqlParameter("@DesigUserTyp",SqlDbType.SmallInt) {Value=DesigUserTyp }
                                   };

            int result = db.ExecuteWithDataTable("USP_ManageDesigPermission", param, CommandType.StoredProcedure);
            string output = param[1].Value.ToString();
            int OutData = Convert.ToInt32(output);
            return OutData;
        }
        public bool  DeleteAppDesignation(Int32 AppDesigID)
        {
            try
            {
            DynamicParameters param = new DynamicParameters();
            param.Add("@AppDesigID", AppDesigID);

            int result1 = db.DapperExecute("update TblDesignation set DesigStatus=0 where DesigID=@AppDesigID", param);
            int result2 = db.DapperExecute(" delete from TblMenuDesigPermission where AppDesigID=@AppDesigID", param);
            int result3 = db.DapperExecute("update TblGrade set GradeStatus=0 where DesigID=@AppDesigID", param);

            return true;
            }
            catch{
                return false;
            }

        }
         
    }
}