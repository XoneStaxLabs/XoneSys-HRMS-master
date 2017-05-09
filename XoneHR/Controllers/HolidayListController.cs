using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XoneHR.Models;

namespace XoneHR.Controllers
{
    public class HolidayListController : Controller
    {
        HolidayListBal Holiday = new HolidayListBal();
        private CommonFunctions common=new CommonFunctions();

        [AuthorizeAll]
        public ActionResult Index()
        {
            SessionManage.Current.PermitFunctions = common.GetPermissionList("Index", "HolidayList");
            return View();
        }

        public ActionResult List()
        {
            try
            {
                SessionManage.Current.PermitFunctions = common.GetPermissionList("Index", "HolidayList");
                var LangKnownList = Holiday.GetHoliday();
                return View(LangKnownList);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult AddNew()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Create(string HoliText=null, string HoliDate=null)
        {
            int result = Holiday.SaveHolidays(HoliText, common.CommonDateConvertion(HoliDate));
            return Json(1,JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(Int32 HolidayID)
        {
            int result = Holiday.Delete(HolidayID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}
