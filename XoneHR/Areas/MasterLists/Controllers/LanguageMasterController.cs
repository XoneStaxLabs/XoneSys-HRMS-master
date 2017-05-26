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
    public class LanguageMasterController : Controller
    {
        private readonly ILanguageMaster ILanguageMaster;

        public LanguageMasterController(ILanguageMaster ILanguageMaster)
        {
            this.ILanguageMaster = ILanguageMaster;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListLanguageDetails()
        {
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();

            List<TblLanguageDetails> Lngdetails = new List<TblLanguageDetails>();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            var GetAll = ILanguageMaster.ListLanguageDetails();
            if (!string.IsNullOrEmpty(searchValue))
            {
                GetAll = GetAll.Where(a => a.LanguageName.ToLower().Trim().Contains(searchValue.ToLower().Trim())).ToList();
            }

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                GetAll = (from prd in GetAll orderby sortColumn + " " + sortColumnDir select prd).ToList();
            }

            recordsTotal = GetAll.Count();
            Lngdetails = GetAll.Skip(skip).Take(pageSize).ToList();

            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = Lngdetails });

        }

        public ActionResult AddNewLanguage(TblLanguageDetails LngDetails)
        {
            var Result = ILanguageMaster.CreateLanguage(LngDetails, SessionManage.Current.UID);
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

        public JsonResult GetLangDetails(Int16 LanguageID)
        {
            TblLanguageDetails lngdetails = new TblLanguageDetails();
            lngdetails = ILanguageMaster.GetLangDetails(LanguageID);
            return Json(lngdetails, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditLngDetails(TblLanguageDetails LngObj)
        {
            var Result = ILanguageMaster.EditLngDetails(LngObj, SessionManage.Current.UID);
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

        public JsonResult GetLanguageName(Int16 LanguageID)
        {
            var name = ILanguageMaster.GetLanguageName(LanguageID);
            return Json(name, JsonRequestBehavior.AllowGet);
        }

        public bool CheckDeletableStatus(Int16 LanguageID)
        {
            return ILanguageMaster.CheckDeletableStatus(LanguageID);            
        }

        public ActionResult DeleteLanguage(Int16 LanguageID)
        {
            var Result = ILanguageMaster.DeleteLanguage(LanguageID, SessionManage.Current.UID);
            var Success = Json(new { Message = "Data Deleted Successfully", Icon = "success", Result = Result }, JsonRequestBehavior.AllowGet);
            var Fail = Json(new { Message = "Data Delete Failed", Icon = "error", Result = -1 }, JsonRequestBehavior.AllowGet);
            if (Result == 1)
                return Success;
            else
                return Fail;
        }

    }
}