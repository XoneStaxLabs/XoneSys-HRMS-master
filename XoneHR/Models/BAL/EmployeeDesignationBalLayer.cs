using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace XoneHR.Models.BAL
{
    public class EmployeeDesignationBalLayer
    {
        private XoneDbLayer db;
        public EmployeeDesignationBalLayer()
        {
            db = new XoneDbLayer();
        }

        public List<TblDepartment> GetDepartment()
        {
            return db.DapperToList<TblDepartment>("select * from TblDepartment where DepUserTyp=1 and DepStatus=1");
        }
        public TblDesignation GetDesignationEdit(Int32 DesigID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@DesigID", DesigID);
            return db.DapperFirst<TblDesignation>("select * from TblDesignation where DesigID=@DesigID and DesigStatus=1", param);
        }
        public List<TblDepartment> AllGetDepartment(Int16 UserType)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@UserType", UserType);
            return db.DapperToList<TblDepartment>("select * from TblDepartment where DepUserTyp=@UserType and DepStatus=1", param);
        }
        public List<TblDesignation> AllGetDesignation(Int32 DepID, Int16 UserType)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@DepID", DepID);
            param.Add("@UserType", UserType);
            return db.DapperToList<TblDesignation>("select * from TblDesignation where DepID=@DepID and DesigUserTyp = @UserType and DesigStatus=1", param);
        }
        public int ManageDesignation(Int32 FLAG ,TblDesignation TblDesigObj, Int32 DesigID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@DepID", TblDesigObj.DepID);
            param.Add("@DesigName", string.IsNullOrEmpty(TblDesigObj.DesigName) ? "" : TblDesigObj.DesigName.Trim());
            param.Add("@SaveUpdate", FLAG);
            param.Add("@DesigSID", DesigID);
            param.Add("@OUT", 1, DbType.Int32, ParameterDirection.Output);

            db.DapperExecute("USP_ManageDesignation", param, CommandType.StoredProcedure);
            Int32 OUT = param.Get<Int32>("OUT");
            return OUT;
            
        }

        public List<TblFixecSalaryTypes> GetFixecSalaryTypes(Int64 DesigID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@DesigID", DesigID);
            if (DesigID != 0)
                return db.DapperToList<TblFixecSalaryTypes>("select a.FixSalaTypID,a.FixSalaType,b.FixAllStatus as FixSalaStatus from TblFixecSalaryTypes as a " +
                                                        " left join TblFixedAllowanceDesigwise as b on a.FixSalaTypID=b.FixSalaTypID where b.DesigID=@DesigID and a.FixSalaStatus=1 and b.FixAllStatus=1", param);
            else
                return db.DapperToList<TblFixecSalaryTypes>("select distinct(a.FixSalaTypID),a.FixSalaType,b.FixallowmStatus  as FixSalaStatus from TblFixecSalaryTypes as a " +
                                                        " left join TblFixedAllowanceMaster as b on a.FixSalaTypID=b.FixSalaTypID where a.FixSalaStatus=1 and b.FixallowmStatus=1", param);

        }
        public List<TblFixedAllowanceMaster> GetFixedAllowanceMaster(Int64 DesigID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@DesigID", DesigID);
            var data =  db.DapperToList<TblFixedAllowanceMaster>("USP_GetFixedAllowanceList",param,CommandType.StoredProcedure);
            return data;
        }
        public TblSalaryMasterwise GetSalaryMastrWise(Int64 DesigID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@DesigID", DesigID);
            return db.DapperFirst<TblSalaryMasterwise>("USP_GetSalaryMasterwise",param,CommandType.StoredProcedure);
        }
        public List<TblLeaveMasterwise> GetLeaveMasterwise(Int64 DesigID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@DesigID", DesigID);
            return db.DapperToList<TblLeaveMasterwise>("USP_GetLeaveWiseData",param,CommandType.StoredProcedure);
        }
        public int ManageDesigssignment(int flag, DataTable Dt1, DataTable Dt2, int DesigID, string SalaMasterPayDate, string Salamasterperiod, DataTable Dt3)
        {
            SqlParameter[] param = {
                                       new SqlParameter("@Datatable_Parent",SqlDbType.Structured){ Value=Dt1 },
                                       new SqlParameter("@Datatable_Child",SqlDbType.Structured){ Value=Dt2 },
                                       new SqlParameter("@Out",SqlDbType.Int){ Direction=ParameterDirection.Output }, 
                                       new SqlParameter("@flagid",SqlDbType.Int){Value = flag},
                                       new SqlParameter("@DesigID",SqlDbType.Int){Value = DesigID},
                                       new SqlParameter("@SalaMasterPayDate",SqlDbType.NVarChar){Value=SalaMasterPayDate},
                                       new SqlParameter("@Salamasterperiod",SqlDbType.NVarChar){Value=Salamasterperiod},
                                       new SqlParameter("@Datatable_Leave",SqlDbType.Structured){ Value=Dt3 },
                                   };


            int result = db.ExecuteWithDataTable("USP_ManageDesigAssignment", param, CommandType.StoredProcedure);
            string output = param[2].Value.ToString();
            int OutData = Convert.ToInt32(output);
            return OutData;
        }
        public List<DesignationList> GetDesignationList()
        {
            DynamicParameters param = new DynamicParameters();
            return db.DapperToList<DesignationList>("SELECT a.DesigID,a.DepID,a.DesigName,b.DepName,a.DesigStatus FROM TblDesignation as a join TblDepartment as b on a.DepID=b.DepID where a.DesigUserTyp=1 and b.DepUserTyp=1 and a.DesigStatus=1", param);
        }
        public List<GetMenuPermission> GetJqxJsonData(Int32 DesigID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@DesigID", DesigID);

            var count = db.DapperFirst<GetMenuPermission>("select COUNT(DesigMenuID) as cnt from TblMenuDesigPermission where DesigID=@DesigID", param);
           
            if(count.cnt==0)
            {
                return db.DapperToList<GetMenuPermission>("select a.MenuID,a.MenuParentID,a.MenuName,a.MenuStatus as MenuStatus from TblMenuMaster as a where a.MenuStatus=1");
            }
            else
            {
                //return db.DapperToList<GetMenuPermission>("select a.MenuID,a.MenuParentID,a.MenuName,b.DesigPermistatus as MenuStatus from TblMenuMaster as a left JOIN TblMenuDesigPermission as b on a.MenuID=b.MenuID where b.DesigID=@DesigID and a.MenuStatus=1", param);
                return db.DapperToList<GetMenuPermission>("select t.MenuID,t.MenuParentID,t.MenuName, ISNULL(r.DesigPermistatus, 0 ) as MenuStatus  FROM " +
                                                          "(select a.MenuID,a.MenuParentID,a.MenuName,a.MenuStatus as MenuStatus from TblMenuMaster as a where a.MenuStatus=1) as t " +
                                                          "LEFT OUTER JOIN (SELECT c.MenuID,c.DesigPermistatus FROM TblMenuDesigPermission c WHERE c.DesigID=@DesigID) AS r  on t.MenuID=r.MenuID", param);
            }

        }
        public DesignationList AllGetDesigDetails(Int32 ID , Int16 UserType)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@DesigID", ID);
            param.Add("@UserType", UserType);
            return db.DapperFirst<DesignationList>("select * from TblDesignation as a join TblDepartment as b on a.DepID=b.DepID where a.DesigID=@DesigID and a.DesigUserTyp=@UserType and b.DepUserTyp=@UserType and a.DesigStatus=1", param);
        }
        public int AllManageDesigPermission(DataTable DT, string DesigID)
        {
            SqlParameter[] param = {
                                       new SqlParameter("@Datatable_MenuID",SqlDbType.Structured){ Value=DT }, 
                                       new SqlParameter("@Out",SqlDbType.Int){ Direction=ParameterDirection.Output },  
                                       new SqlParameter("@DesigID",SqlDbType.Int){Value = Convert.ToInt32(DesigID)},
                                       
                                   };

            int result = db.ExecuteWithDataTable("USP_AllManageDesigPermission", param, CommandType.StoredProcedure);
            string output = param[1].Value.ToString();
            int OutData = Convert.ToInt32(output);
            return OutData;
        }
    }
}