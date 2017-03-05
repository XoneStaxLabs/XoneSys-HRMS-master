using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace XoneHR.Models.BAL
{
    public class LoginBal
    {
        private XoneDbLayer db;

        public LoginBal()
        {
            db = new XoneDbLayer();
        }

        public TblAppUser LoginChecking(string Username, string Pwd)
        {
            TblAppUser user = new TblAppUser();

            try
            {
                DynamicParameters para = new DynamicParameters();
                para.Add("@AppUserName", Username);
                para.Add("@AppPwd", Pwd);

                user = db.DapperFirst<TblAppUser>("select * from TblAppUser appuser join  TblDesignation desig on appuser.DesigID=desig.DesigID where AppUserName=@AppUserName and AppPwd=@AppPwd and AppStatus=1", para);

                if (user == null)
                {
                    user = new TblAppUser();
                    user.AppStatus = false;
                }
                if (user != null)
                {
                    DateTime Dt = DateTime.Now;
                    para.Add("@AppUID", user.AppUID);
                    para.Add("@Date", Dt);
                
                    db.DapperExecute("Update TblmemoUserwise set ReadStatus=1,ReadDate=convert(date,@Date) where AppUID=@AppUID",para); 
                }

                return user;
            }
            catch
            {
                user.AppStatus = false;
                return user;
            }
        }

        public List<TblMenuMaster> GetMenus()
        {
            try
            {
                DynamicParameters para = new DynamicParameters();
                para.Add("@AppUserID", SessionManage.Current.AppUID);

                var MenuLists = db.DapperToList<TblMenuMaster>("select *from TblMenuMaster menu join TblMenuUserPermission userper on userper.MenuID =menu.MenuID where userper.AppUserID=@AppUserID and menu.MenuStatus=1 order by MenuOrdNo asc", para);
                return MenuLists;
            }
            catch
            {
                return null;
            }
        }

        public Int32 ChangePassword(string Oldpass, Int64 AppuID, string Confirmpwd)
        {
            try
            {
                DynamicParameters para = new DynamicParameters();
                para.Add("@Oldpass", Oldpass);
                para.Add("@Confirmpwd", Confirmpwd);
                para.Add("@AppuID", AppuID);

                para.Add("@output", null, DbType.Int32, ParameterDirection.Output);

                db.DapperExecute("USP_UserSettings", para, CommandType.StoredProcedure);
                var Output = para.Get<Int32>("output");

                return Output;
            }
            catch
            {
                return 0;
            }
        }
    }
}