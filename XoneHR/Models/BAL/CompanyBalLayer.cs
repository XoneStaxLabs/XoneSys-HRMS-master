using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace XoneHR.Models.BAL
{
    public class CompanyBalLayer
    {
        private XoneDbLayer db;

        public CompanyBalLayer()
        {
            db = new XoneDbLayer();
        }
        public TblCompanyMaster GetCompanyMaster()
        {
            DynamicParameters param = new DynamicParameters();
            return db.DapperFirst<TblCompanyMaster>("select * from TblCompanyMaster",param);
        }
        public List<TblCompanyEmail> GetCompanyEmails()
        {
            return db.DapperToList<TblCompanyEmail>("select * from TblCompanyEmail");
        }
        public int UpdateCompanyMaster(TblCompanyMaster ComObj)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@CompName", ComObj.CompName);
                param.Add("@CompTest", ComObj.CompTest);
                param.Add("@CompLogo", ComObj.CompLogo);
                if (ComObj.CompLogo != "")
                {
                    return db.DapperExecute("Update TblCompanyMaster set CompName=@CompName , CompTest=@CompTest , CompLogo=@CompLogo", param);
                }
                else
                {
                    return db.DapperExecute("Update TblCompanyMaster set CompName=@CompName , CompTest=@CompTest", param);
                }
            }
            catch(Exception ex)
            {
                return 0;
            }
        }
        public int UpdateCompanyEmails(TblCompanyEmail ComEmail)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@CompEmailID", ComEmail.CompEmailID);
                param.Add("@EmailID",ComEmail.EmailID);
                param.Add("@CompPortNo",ComEmail.CompPortNo);
                param.Add("@CompPassword",ComEmail.CompPassword);
                return db.DapperExecute(" update TblCompanyEmail set EmailID=@EmailID , CompPortNo=@CompPortNo, CompPassword=@CompPassword where CompEmailID=@CompEmailID", param);

            }catch(Exception ex)
            {
                return 0;
            }
        }

    }
}