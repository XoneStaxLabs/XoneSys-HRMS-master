using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Xone;
using RepositoryImplement.Xone.RepositoryDerive;
using XoneHR.Models;

namespace XoneHR.Areas.MasterLists.Controllers
{
    public class HolidayMasterController : Controller
    {
        private readonly IHolidayMaster IHolidayMaster;
        public HolidayMasterController(IHolidayMaster IHolidayMaster)
        {
            this.IHolidayMaster = IHolidayMaster;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListHolidays()
        {
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();

            List<HolidayList> details = new List<HolidayList>();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            var GetAll = IHolidayMaster.ListHolidays();
            if (!string.IsNullOrEmpty(searchValue))
            {
                GetAll = GetAll.Where(m => (m.Holiday.ToLower().Trim().Contains(searchValue.ToLower().Trim())) || m.HolidayDate.ToString().Trim().Contains(searchValue.ToLower().Trim())).ToList();
            }

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                GetAll = (from prd in GetAll orderby sortColumn + " " + sortColumnDir select prd).ToList();
            }

            recordsTotal = GetAll.Count();
            details = GetAll.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = details });
        }

        public ActionResult AddHolidays(TblHolidayList Holilist)
        {
            var Result = IHolidayMaster.AddHolidays(Holilist, SessionManage.Current.UID);
            var Success = Json(new { Message = "Data Save Successfully", Icon = "success", Result = Result }, JsonRequestBehavior.AllowGet);
            var Fail = Json(new { Message = "Data Save Failed", Icon = "error", Result = -1 }, JsonRequestBehavior.AllowGet);
            var AlreadyExist = Json(new { Message = "Data Already Exist", Icon = "warning", Result = 0 }, JsonRequestBehavior.AllowGet);

            if (Result == 1)
                return Success;
            else if (Result == 0)
                return AlreadyExist;
            else
                return Fail;
        }
        public JsonResult GetHolidatyText(Int32 HolidayID)
        {
            var text = IHolidayMaster.GetHolidatyText(HolidayID);
            return Json(text, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteHoliday(Int32 HolidayID)
        {
            var Result = IHolidayMaster.DeleteHoliday(HolidayID, SessionManage.Current.UID);
            var Success = Json(new { Message = "Data Deleted Successfully", Icon = "success", Result = Result }, JsonRequestBehavior.AllowGet);
            var Fail = Json(new { Message = "Data Delete Failed", Icon = "error", Result = -1 }, JsonRequestBehavior.AllowGet);
            if (Result == 1)
                return Success;
            else
                return Fail;
        }

    }
}