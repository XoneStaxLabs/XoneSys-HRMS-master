using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace XoneHR.Models.BAL
{
    public class ApplicationUserBalLayer
    {
        private XoneDbLayer db;        
        public ApplicationUserBalLayer()
        {
            db = new XoneDbLayer();
        }
        public List<TblAppUser> GetAppUserList(Int16 UserType)
        {
            DynamicParameters para = new DynamicParameters();
            para.Add("@UserType", UserType);
            if(UserType != 0)
            {
                return db.DapperToList<TblAppUser>("select * from TblAppUser as a join TblDesignation as b on a.DesigID=b.DesigID  " +
                                           " join TblCityZenship as c on a.CitizenID=c.CizenID " +
                                           " where a.AppStatus=1 and a.AppUserTypID=@UserType", para);
            }
            else
            {
                return db.DapperToList<TblAppUser>("select * from TblAppUser as a join TblDesignation as b on a.DesigID=b.DesigID  " +
                                          " join TblCityZenship as c on a.CitizenID=c.CizenID " +
                                          " where a.AppStatus=1", para);
            }

        }
        public List<TblCityZenship> ComboListCitizenship()
        {
            try
            {
                var ListCitizenship = db.DapperToList<TblCityZenship>("USP_ComboListCitizenship", CommandType.StoredProcedure);
                return ListCitizenship;
            }
            catch
            {
                return null;
            }
        }
        public List<TblDesignation> CombolistDesignation()
        {
            try
            {
                //var ListDesignation = db.DapperToList<TblDesignation>(" select DesigID,DesigName from TblDesignation where DesigStatus=1 and DepID !=1 ");  //and DesigUserTyp=1
                var ListDesignation = db.DapperToList<TblDesignation>("Select DesigID,DesigName from TblDesignation where DesigStatus=1 and DesigUserTyp=0");  

                return ListDesignation;
            }
            catch
            {
                return null;
            }
        }
        public List<TblDesignation> GetDesignations(Int16 UserTypID)
        {
            try
            {
                DynamicParameters para = new DynamicParameters();
                para.Add("@UserTypID", UserTypID);
                var List = db.DapperToList<TblDesignation>("Select DesigID,DesigName from TblDesignation where DesigStatus=1 and DesigUserTyp=@UserTypID",para);
                return List;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        public CandidateItems ManageApplicationUser(TblAppUser UserObj)
        {
            DynamicParameters param = new DynamicParameters();
            if(UserObj.AppUID !=0)
            {
                param.Add("@Flag", 2);
            }
            else
            {
                param.Add("@Flag", 1);
            }

            param.Add("@AppUID", UserObj.AppUID);
            param.Add("@AppFirstName", UserObj.AppFirstName.Trim());
            param.Add("@CitizenID", UserObj.CitizenID);
            param.Add("@DesigID", UserObj.DesigID);
            param.Add("@AppAddress", string.IsNullOrEmpty(UserObj.AppAddress) ? "" : UserObj.AppAddress.Trim());
            param.Add("@AppMobile", UserObj.AppMobile);
            param.Add("@AppUserName", UserObj.AppUserName.Trim());
            param.Add("@AppPwd", UserObj.AppPwd);
            param.Add("@AppEmail", UserObj.AppEmail);
            param.Add("@AppGender", UserObj.AppGender);
            param.Add("@AppPhoto", UserObj.AppPhoto);
            param.Add("@AppUserTypID", UserObj.AppUserTypID);
            param.Add("@UserID", null, DbType.Int64, ParameterDirection.Output);
            param.Add("@ErrorOutput", null, DbType.Int32, ParameterDirection.Output);
            int Result = db.DapperExecute("USP_ManageApplicationUser", param, CommandType.StoredProcedure);

            CandidateItems CandObj = new CandidateItems();
            CandObj.TableID = param.Get<Int64>("UserID");
            CandObj.OutputID = param.Get<Int32>("ErrorOutput");

            return CandObj;

        }
        public List<GetMenuPermission> GetJqxJsonData(Int64 AppUserID,Int32 DesigID,int Flag)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@AppUserID", AppUserID);
            param.Add("@DesigID", DesigID);
            //Flag {0 New User, 1 Edit User}
            if (Flag == 1)

                return db.DapperToList<GetMenuPermission>("select t.MenuID,t.MenuParentID,t.MenuName, ISNULL(r.UserPermistatus, 0 ) as MenuStatus  FROM " +
                                    "(select a.MenuID,a.MenuParentID,a.MenuName,b.DesigPermistatus as MenuStatus from TblMenuMaster a join TblMenuDesigPermission as b on a.MenuID=b.MenuID where b.DesigID=@DesigID and a.MenuStatus=1 and b.DesigPermistatus=1) as t " +
                                    "LEFT OUTER JOIN (SELECT c.MenuID,c.UserPermistatus FROM TblMenuUserPermission c WHERE c.AppUserID=@AppUserID) AS r  on t.MenuID=r.MenuID", param);

               //return db.DapperToList<GetMenuPermission>("select a.MenuID,a.MenuParentID,a.MenuName,b.UserPermistatus as MenuStatus from TblMenuMaster as a LEFT JOIN TblMenuUserPermission as b on a.MenuID=b.MenuID where b.AppUserID=@AppUserID", param);
            else
                return db.DapperToList<GetMenuPermission>("select a.MenuID,a.MenuParentID,a.MenuName,b.DesigPermistatus as MenuStatus from TblMenuMaster as a left JOIN TblMenuDesigPermission as b on a.MenuID=b.MenuID where b.DesigID=@DesigID and a.MenuStatus=1 and b.DesigPermistatus=1", param);

        }
        public int ManageUserPermission(DataTable DT, Int64 AppUserID, Int16 Flag)
        {
            SqlParameter[] param = {
                                       new SqlParameter("@Datatable_MenuID",SqlDbType.Structured){ Value=DT }, 
                                       new SqlParameter("@Out",SqlDbType.Int){ Direction=ParameterDirection.Output },  
                                       new SqlParameter("@AppUserID",SqlDbType.BigInt){Value = AppUserID},
                                       new SqlParameter("@Flag",SqlDbType.SmallInt){Value = Flag}
                                   };

            int result = db.ExecuteWithDataTable("USP_ManageUserPermission", param, CommandType.StoredProcedure);
            string output = param[1].Value.ToString();
            int OutData = Convert.ToInt32(output);
            return OutData;
        }
        public TblAppUser GetAppUserEditDetails(Int64 AppUserID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@AppUserID", AppUserID);
            return db.DapperFirst<TblAppUser>(" select * from TblAppUser as a join TblDesignation as b on a.DesigID=b.DesigID " +
                                              " join TblCityZenship as c on a.CitizenID=c.CizenID " +
                                              " where a.AppStatus=1 and a.AppUID=@AppUserID", param);
            //a.AppUserTypID=1 and
        }
        public bool DeleteAppUser(Int64 AppUID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@AppUID", AppUID);

                int result1 = db.DapperExecute(" delete from TblAppUser where AppUID=@AppUID", param);
                int result2 = db.DapperExecute(" delete from TblMenuUserPermission where AppUserID = @AppUID", param);

                return true;
            }
            catch{

                return false;
            }
            
        }
        public List<MemosUser> GetUserProfileMemo(Int64 AppUID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@AppUID", AppUID);
            return db.DapperToList<MemosUser>("select * from TblMemos as a join TblmemoUserwise as b on a.MemoID=b.MemoID where b.AppUID=@AppUID ORDER BY a.MemoSubject ASC", param);
        }
        public List<UserProfileWorkSchedule> GetUserWrokScheduleList(Int64 AppUID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@AppUID", AppUID);
            return db.DapperToList<UserProfileWorkSchedule>("select distinct(prj.ProjID), prj.ProjName,sch.Schedule,sht.ShiftName,sht.ShiftFrom,sht.ShiftTo from TblProject as prj join Tblschedule as sch on prj.ProjID=sch.ProjID " +
                                                            "join TblShift sht on prj.ProjID=sht.ProjID " +
                                                            "join TblEmpShift as empsht on prj.ProjID=empsht.ProjID " +
                                                            "join TblAppUser as appuser on empsht.EmpID = appuser.EmpID " +
                                                            "where appuser.AppStatus=1 and appuser.AppUserTypID=2 and appuser.AppUID = @AppUID", param);
        }
        public List<UserProfileLeaveDetails> GetUserLeaveDetails(Int64 AppUID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@AppUID", AppUID);
            return db.DapperToList<UserProfileLeaveDetails>("select Distinct a.LeaveType,b.LeavesText,c.DayType from TblLeaveType as a join TblLeaveEmpwise as b on a.LeavetypID=b.LeavetypID " +
                                                            "join TblDayType as c on b.LeaveDaytype=c.DayTypID "+
                                                            "join TblAppUser as d on b.EmpID=d.EmpID "+
                                                            "where d.AppStatus=1  and d.AppUID=@AppUID and d.AppUserTypID=2", param);
        }
    }
}