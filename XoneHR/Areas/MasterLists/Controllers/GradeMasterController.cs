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
    public class GradeMasterController : Controller
    {
        private readonly IGradeMaster IGradeMasterObj;

        public GradeMasterController(IGradeMaster IGradeMaster)
        {
            this.IGradeMasterObj = IGradeMaster;
        }

        public ActionResult Index() 
        {
            return View();
        }

        public ActionResult ListGradeDetails()
        {
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();

            List<GradeList> GradeList = new List<GradeList>();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            var GetAll = IGradeMasterObj.ListGradeDetails();
            if (!string.IsNullOrEmpty(searchValue))
            {
                GetAll = GetAll.Where(m => m.Gradename.ToLower().Trim().Contains(searchValue.ToLower().Trim())).ToList();
            }

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                GetAll = (from prd in GetAll orderby sortColumn + " " + sortColumnDir select prd).ToList();
            }
            recordsTotal = GetAll.Count();
            GradeList = GetAll.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = GradeList });

        }

        public JsonResult GetDepartment()
        {
            var list = IGradeMasterObj.GetDepartment();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDesignation(int DeptID)
        {
            var list = IGradeMasterObj.GetDesignation(DeptID);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddNewGrade(int DesignationID, string Gradename, string GradeCode)
        {
            var Result = IGradeMasterObj.AddNewGrade(DesignationID, Gradename, GradeCode, SessionManage.Current.UID);
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

        public JsonResult GetDetailsForEdit(int GradeID,int GradeDesignationId)
        {
            var list = IGradeMasterObj.GetDetailsForEdit(GradeID, GradeDesignationId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditGrade(GradeList GradeList)
        {
            var Result = IGradeMasterObj.EditGrade(GradeList, SessionManage.Current.UID);
            var Success = Json(new { Message = "Data Updated Successfully", Icon = "success", Result = Result }, JsonRequestBehavior.AllowGet);
            var Fail = Json(new { Message = "Data Updation Failed", Icon = "error", Result = -1 }, JsonRequestBehavior.AllowGet);
            var AlreadyExist = Json(new { Message = "Data Already Exist", Icon = "warning", Result = 0 }, JsonRequestBehavior.AllowGet);

            if (Result == 1)
                return Success;
            else if (Result == 0)
                return AlreadyExist;
            else
                return Fail;
        }

        public JsonResult GetGradeName(int GradeID)
        {
            var text = IGradeMasterObj.GetGradeName(GradeID);
            return Json(text, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteGrade(int GradeID,int GradeDesignationId)
        {
            var Result = IGradeMasterObj.DeleteGrade(GradeID, GradeDesignationId, SessionManage.Current.UID);
            var Success = Json(new { Message = "Data Deleted Successfully", Icon = "success", Result = Result }, JsonRequestBehavior.AllowGet);
            var Fail = Json(new { Message = "Data Delete Failed", Icon = "error", Result = -1 }, JsonRequestBehavior.AllowGet);
            if (Result == 1)
                return Success;
            else
                return Fail;
        }

    }
}