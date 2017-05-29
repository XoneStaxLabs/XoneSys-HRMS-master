using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RepositoryImplement.Xone.RepositoryDerive;
using Model.Xone;

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
    }
}