using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XoneHR.Models;

namespace XoneHR.Controllers
{
    public class DocumentTypeController : Controller
    {

        DocumentTypeBLayer documenttypeobj = new DocumentTypeBLayer();
        CommonFunctions common = new CommonFunctions();

        [AuthorizeAll]
        public ActionResult Index()
        {
            try
            {
                SessionManage.Current.PermitFunctions = common.GetPermissionList("Index", "DocumentType");
                return View();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public ActionResult List()
        {
            try
            {
                SessionManage.Current.PermitFunctions = common.GetPermissionList("Index", "DocumentType");
                var DocumentTypeList = documenttypeobj.GetDocumentTypeList();
                return View(DocumentTypeList);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public ActionResult AddNew()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public ActionResult Create(string DocumentTypeName)
        {
            try
            {
                int result = documenttypeobj.ManageDocumentType(1, DocumentTypeName, 1, 0);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public ActionResult Edit(int DocTypID, string DocumentTypeName, int status = 0)
        {
            try
            {
                int result = documenttypeobj.ManageDocumentType(2, DocumentTypeName, status, DocTypID);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public ActionResult Delete(Int32 DocTypID)
        {
            try
            {
                int result = documenttypeobj.ManageDocumentType(3, "", 0, DocTypID);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
