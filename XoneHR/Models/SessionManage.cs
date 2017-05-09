using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XoneHR.Models
{
    public class SessionManage
    {

        #region Session Manage
        // PRIVATE CONSTRUCTOR
        private SessionManage()
        {
           
        }

        // GETS THE CURRENT SESSION.
        public static SessionManage Current
        {
            get
            {
                SessionManage session =
                (SessionManage)HttpContext.Current.Session["__MySession__"];
                if (session == null)
                {
                    session = new SessionManage();
                    HttpContext.Current.Session["__MySession__"] = session;
                }
                return session;
            }
        }

       // ADD YOUR SESSION PROPERTIES HERE
        
        public Int64 CandID { get; set; }
        public Int64 EmpID { get; set; }
        public Int64 AppUID { get; set; }

        public Int64 GlobalEmpID { get; set; }

        public string AppUname { get; set; }
        public string Designame { get; set; }
        public string AppPhoto { get; set; }
        public string Dateformat { get; set; }
        public List<Permission> PermitFunctions { get; set; }
        public Int64 EmployeeId { get; set; }

        public List<TblSalaryPay> MyDatas { get; set; }
        
        #endregion 
    }
}