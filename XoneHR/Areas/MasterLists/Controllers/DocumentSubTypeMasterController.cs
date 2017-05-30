using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RepositoryImplement.Xone.RepositoryDerive;
using Model.Xone;
using XoneHR.Models;

namespace XoneHR.Areas.MasterLists.Controllers
{
    public class DocumentSubTypeMasterController : Controller
    {
        private readonly IDocSubTypeMaster IDocSubTypeMaster;

        public DocumentSubTypeMasterController(IDocSubTypeMaster IDocSubTypeMaster)
        {
            this.IDocSubTypeMaster = IDocSubTypeMaster;
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

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            List<DocumentSubTypeList> List =new List<DocumentSubTypeList>();
            var GetAll = IDocSubTypeMaster.ListDocumentDetails();
            if (!string.IsNullOrEmpty(searchValue))
            {
                GetAll = GetAll.Where(m => m.DocSubtypeName.ToLower().Trim().Contains(searchValue.ToLower().Trim())).ToList();
            }

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                GetAll = (from prd in GetAll orderby sortColumn + " " + sortColumnDir select prd).ToList();
            }
             
            recordsTotal = GetAll.Count();
            List = GetAll.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = List });

        }

        public JsonResult GetDocTypes()
        {
            var doctypes = IDocSubTypeMaster.GetDocTypes();
            return Json(doctypes, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateDocumentSubType(TblDocumentSubTypes SubTypes)
        {
            var Result = IDocSubTypeMaster.CreateDocumentSubType(SubTypes,SessionManage.Current.UID);
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

        public JsonResult GetDetailsForEdit(int DocSubtypeID)
        {
            var details = IDocSubTypeMaster.GetDetailsForEdit(DocSubtypeID);
            return Json(details, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditDocumentSubType(TblDocumentSubTypes subtypes)
        {
            var Result = IDocSubTypeMaster.EditDocumentSubType(subtypes, SessionManage.Current.UID);
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

        public bool CheckDeletableStatus(Int32 DocSubtypeID)
        {
            return IDocSubTypeMaster.CheckDeletableStatus(DocSubtypeID);
        }

        public JsonResult GetDocumentName(int DocSubtypeID)
        {
            var name = IDocSubTypeMaster.GetDocumentName(DocSubtypeID);
            return Json(name, JsonRequestBehavior.AllowGet);
        }
         
        public ActionResult DeleteDocType(int DocSubtypeID)
        {
            var Result = IDocSubTypeMaster.DeleteDocType(DocSubtypeID, SessionManage.Current.UID);
            var Success = Json(new { Message = "Data Deleted Successfully", Icon = "success", Result = Result }, JsonRequestBehavior.AllowGet);
            var Fail = Json(new { Message = "Data Delete Failed", Icon = "error", Result = -1 }, JsonRequestBehavior.AllowGet);
            if (Result == 1)
                return Success;
            else
                return Fail;
        }

    }
}