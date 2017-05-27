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
    public class DocumentTypeMasterController : Controller
    {
        private readonly IDocumentTypeMaster IDocumentTypeMaster;
        public DocumentTypeMasterController(IDocumentTypeMaster IDocumentTypeMaster)
        {
            this.IDocumentTypeMaster = IDocumentTypeMaster;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListDocumentDetails()
        {
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();

            List<TblDocumentTypes> documents = new List<TblDocumentTypes>();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            var GetAll = IDocumentTypeMaster.ListDocumentDetails();
            if (!string.IsNullOrEmpty(searchValue))
            {
                GetAll = GetAll.Where(m => m.DocTypeName.ToLower().Trim().Contains(searchValue.ToLower().Trim())).ToList();
            }

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                GetAll = (from prd in GetAll orderby sortColumn + " " + sortColumnDir select prd).ToList();
            }
            recordsTotal = GetAll.Count();
            documents = GetAll.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = documents });

        }

        public ActionResult CreateDocumentType(TblDocumentTypes docs)
        {
            var Result = IDocumentTypeMaster.CreateDocumentType(docs, SessionManage.Current.UID);
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

        public JsonResult GetDocEditDetails(int DocTypeID)
        {
            return Json(IDocumentTypeMaster.GetDocEditDetails(DocTypeID),JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditDocDetails(TblDocumentTypes Doc)
        {
            var Result = IDocumentTypeMaster.EditDocDetails(Doc, SessionManage.Current.UID);
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

        //public bool CheckDeletableStatus(int DocTypeID)
        //{
        //    return IDocumentTypeMaster.CheckDeletableStatus(DocTypeID);
        //}
        public JsonResult GetDocumentName(int DocTypeID)
        {
            var name = IDocumentTypeMaster.GetDocumentName(DocTypeID);
            return Json(name, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteDocType(int DocTypeID)
        {
            var Result = IDocumentTypeMaster.DeleteDocType(DocTypeID, SessionManage.Current.UID);
            var Success = Json(new { Message = "Data Deleted Successfully", Icon = "success", Result = Result }, JsonRequestBehavior.AllowGet);
            var Fail = Json(new { Message = "Data Delete Failed", Icon = "error", Result = -1 }, JsonRequestBehavior.AllowGet);
            if (Result == 1)
                return Success;
            else
                return Fail;
        }

    }
}