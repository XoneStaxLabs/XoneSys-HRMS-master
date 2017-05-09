using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XoneHR.Models;

namespace XoneHR.Controllers
{
    public class DocumentSubTypeController : Controller
    {

        DocumentSubTypeBLayer documenttypeobj = new DocumentSubTypeBLayer();
        CommonFunctions common = new CommonFunctions();

        [AuthorizeAll]
        public ActionResult Index()
        {
            try
            {
                SessionManage.Current.PermitFunctions = common.GetPermissionList("Index", "DocumentSubType");
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
                SessionManage.Current.PermitFunctions = common.GetPermissionList("Index", "DocumentSubType");
                var DocumentSubTypeList = documenttypeobj.GetDocumentSubTypeList();
                return View(DocumentSubTypeList);
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
        public ActionResult GetDocumentType()
        {
            try
            {
                var DocumentTypeList = documenttypeobj.GetDocumentType();
                return Json(DocumentTypeList,JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult Create(string DocStypName, int DocTypID)
        {
            try
            {
                int result = documenttypeobj.ManageDocumentSubType(1, DocStypName, DocTypID, 1, 0);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public ActionResult Edit(int DocStypID, string DocStypName, int DocTypID, int status = 0)
        {
            try
            {
                int result = documenttypeobj.ManageDocumentSubType(2, DocStypName, DocTypID, status, DocStypID);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public ActionResult Delete(Int32 DocStypID)
        {
            try
            {
                int result = documenttypeobj.ManageDocumentSubType(3, "", 0,0, DocStypID);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
