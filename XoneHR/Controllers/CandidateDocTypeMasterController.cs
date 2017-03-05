using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using XoneHR.Models;
using XoneHR.Models.BAL;

namespace XoneHR.Controllers
{
    public class CandidateDocTypeMasterController : Controller
    {
        public CandidateDocTypeMstrBalLayer candDocTypeobj = new CandidateDocTypeMstrBalLayer();
        CommonFunctions common = new CommonFunctions();

        [AuthorizeAll]
        public ActionResult Index()
        {
            SessionManage.Current.PermitFunctions = common.GetPermissionList("Index", "CandidateDocTypeMaster");
            return View();
        }
        public ActionResult List()
        {
            SessionManage.Current.PermitFunctions = common.GetPermissionList("Index", "CandidateDocTypeMaster");
            var list = candDocTypeobj.GetCandidateCocTypeMaster();
            return View(list);
        }
        public ActionResult AddNew()
        {
            return View();
        }
        public JsonResult GetCitizenship()
        {
            try
            {
                var ListCitizenship = candDocTypeobj.ComboListCitizenship();
                return Json(ListCitizenship, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
           
        }
        public JsonResult GetDesignation()
        {
            try
            {
                var ListDesignation = candDocTypeobj.CombolistDesignation();
                return Json(ListDesignation, JsonRequestBehavior.AllowGet);

            }catch(Exception ex)
            {
                return null;
            }
        }
        public JsonResult GetDocumentType()
        {
            try
            {
                var ListDocumentType = candDocTypeobj.ComboListDocumentType();
                return Json(ListDocumentType, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        public JsonResult GetDocumentSubType(int DocumenTypID)
        {
            try
            {
                var ListDocumentType = candDocTypeobj.ComboListDocumentSubType(DocumenTypID);
                return Json(ListDocumentType, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        [HttpPost]
        public ActionResult Create(TblValidDoctypeMaster DocObj)
        {
            try
            {
                var Iresult = candDocTypeobj.ManageCanddidateDocTypeMaster(DocObj);
                if (Iresult == 1)
                {
                    return Json(new { Message = "Data Updated Successfully", Result = 1 }, JsonRequestBehavior.AllowGet);
                }
                else if (Iresult == -1)
                {
                    return Json(new { Message = "Data Already Exist", Result = -1 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Message = "Data Updated Failed", Result = 0 }, JsonRequestBehavior.AllowGet);
                }
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        [HttpPost]
        public ActionResult Delete(Int32 ValidDocTypID = 0)
        {
            try
            {
                int result = candDocTypeobj.DeleteCandDocSubType(ValidDocTypID);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
