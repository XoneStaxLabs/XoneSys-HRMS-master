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
    public class DeductionTypeMasterController : Controller
    {
        private readonly IDeductionTypeMaster IDeductionTypeMaster;

        public DeductionTypeMasterController(IDeductionTypeMaster IDeductionTypeMaster)
        {
            this.IDeductionTypeMaster = IDeductionTypeMaster;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListDeductionType()
        {
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();

            List<TblDeductionType> list = new List<TblDeductionType>();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            var GetAll = IDeductionTypeMaster.ListDeductionTypes();
            if (!string.IsNullOrEmpty(searchValue))
            {
                GetAll = GetAll.Where(m => m.DeductionType.ToLower().Trim().Contains(searchValue.ToLower().Trim())).ToList();
            }

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                GetAll = (from prd in GetAll orderby sortColumn + " " + sortColumnDir select prd).ToList();
            }

            recordsTotal = GetAll.Count();
            list = GetAll.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = list });
        }

        public ActionResult AddNewDeductionType(TblDeductionType deducts)
        {
            var Result = IDeductionTypeMaster.AddNewDeductionType(deducts, SessionManage.Current.UID);
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

        public JsonResult GetDetailForEdit(Int32 DeductTypeID)
        {
            var details = IDeductionTypeMaster.GetDetailForEdit(DeductTypeID);
            return Json(details, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditDeductionType(TblDeductionType deducts)
        {
            var Result = IDeductionTypeMaster.EditDeductionType(deducts, SessionManage.Current.UID);
            var Success = Json(new { Message = "Data Update Successfully", Icon = "success", Result = Result }, JsonRequestBehavior.AllowGet);
            var Fail = Json(new { Message = "Data Update Failed", Icon = "error", Result = -1 }, JsonRequestBehavior.AllowGet);
            var AlreadyExist = Json(new { Message = "Data Already Exist", Icon = "warning", Result = 0 }, JsonRequestBehavior.AllowGet);

            if (Result == 1)
                return Success;
            else if (Result == 0)
                return AlreadyExist;
            else
                return Fail;
        }

        public JsonResult GetDeductionTypeText(Int32 DeductTypeID)
        {
            var text = IDeductionTypeMaster.GetDeductionTypeText(DeductTypeID);
            return Json(text, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteDeductionType(Int32 DeductTypeID)
        {
            var Result = IDeductionTypeMaster.DeleteDeductionType(DeductTypeID, SessionManage.Current.UID);
            var Success = Json(new { Message = "Data Deleted Successfully", Icon = "success", Result = Result }, JsonRequestBehavior.AllowGet);
            var Fail = Json(new { Message = "Data Delete Failed", Icon = "error", Result = -1 }, JsonRequestBehavior.AllowGet);
            if (Result == 1)
                return Success;
            else
                return Fail;
        }

    }
}