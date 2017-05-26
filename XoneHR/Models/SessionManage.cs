using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XoneHR.Models
{
    public class SessionManage
    {
        #region Session Manage

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
        public Int64 UID { get; set; }

        #endregion

    }
}