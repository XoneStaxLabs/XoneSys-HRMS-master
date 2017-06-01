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
    public class DepartmentMasterController : Controller
    {
        private IDepartmentMaster IDepartmentMaster;
        public DepartmentMasterController(IDepartmentMaster IDepartmentMaster)
        {
            this.IDepartmentMaster = IDepartmentMaster;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DepartmentList()
        {
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();

            List<DepartmentList> DeptList = new List<DepartmentList>();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            var GetAll = IDepartmentMaster.DepartmentList();
            if (!string.IsNullOrEmpty(searchValue))
            {
                GetAll = GetAll.Where(a => a.DeptName.ToLower().Trim().Contains(searchValue.ToLower().Trim())).ToList();
            }

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                GetAll = (from prd in GetAll orderby sortColumn + " " + sortColumnDir select prd).ToList();
            }
            recordsTotal = GetAll.Count();
            DeptList = GetAll.Skip(skip).Take(pageSize).ToList();

            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = DeptList });

        }

        public JsonResult GetUserType()
        {
            var type = IDepartmentMaster.GetUserType();
            return Json(type, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateDepartment(TblDepartment department)
        {
            var Result = IDepartmentMaster.CreateDepartment(department, SessionManage.Current.UID);
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

        public JsonResult GetDeptEditDetails(int DeptID)
        {
            var editdetails = IDepartmentMaster.GetDeptEditDetails(DeptID);
            return Json(editdetails,JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditDeparmentDetails(TblDepartment department)
        {
            var Result = IDepartmentMaster.EditDeparmentDetails(department, SessionManage.Current.UID);
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

        public JsonResult GetDepartmentname(int DeptID)
        {
            var name = IDepartmentMaster.GetDepartmentname(DeptID);
            return Json(name, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteDepartment(int DeptID)
        {
            var Result = IDepartmentMaster.DeleteDepartment(DeptID, SessionManage.Current.UID);
            var Success = Json(new { Message = "Data Deleted Successfully", Icon = "success", Result = Result }, JsonRequestBehavior.AllowGet);
            var Fail = Json(new { Message = "Data Delete Failed", Icon = "error", Result = -1 }, JsonRequestBehavior.AllowGet);
            if (Result == 1)
                return Success;
            else
                return Fail;
        }

    }
}