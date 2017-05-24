using Model.Xone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RepositoryImplement.Xone.RepositoryDerive;

namespace XoneHR.Areas.MasterLists.Controllers
{
    public class CitizenMasterController : Controller
    {
        private readonly ICitizenMaster ICitizenMaster;
        public CitizenMasterController(ICitizenMaster ICitizenMaster)
        {
            this.ICitizenMaster = ICitizenMaster;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListCitizenDetails()
        {
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();

            List<TblCitizenDetails> citizendetail = new List<TblCitizenDetails>();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            var GetAll = ICitizenMaster.CitizenDetails();

            if (!string.IsNullOrEmpty(searchValue))
            {
                GetAll = GetAll.Where(a => a.CitizenName.ToLower().Trim().Contains(searchValue.ToLower().Trim())).ToList();
            }

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                GetAll = (from prd in GetAll orderby sortColumn + " " + sortColumnDir select prd).ToList();
            }
             
            recordsTotal = GetAll.Count();
            citizendetail = GetAll.Skip(skip).Take(pageSize).ToList();

            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = citizendetail });
        }
              
        public ActionResult CreateCitizen(TblCitizenDetails citizenobj)
        {
            var Result = ICitizenMaster.CreateCitizen(citizenobj);
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

        public JsonResult GetDetailsForEdit(Int16 CitizenID)
        {
            TblCitizenDetails citizen = new TblCitizenDetails();
            citizen = ICitizenMaster.GetDetailsForEdit(CitizenID);
            return Json(citizen, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditCitizenDetails(TblCitizenDetails citizenobj)
        {
            var Result = ICitizenMaster.EditCitizenDetails(citizenobj);
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

        public JsonResult GetCitizenName(Int16 CitizenID)
        {
            var name = ICitizenMaster.GetCitizenName(CitizenID);
            return Json(name, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteCitizen(Int16 CitizenID)
        {
            var Result = ICitizenMaster.DeleteCitizen(CitizenID);
            var Success = Json(new { Message = "Data Deleted Successfully", Icon = "success", Result = Result }, JsonRequestBehavior.AllowGet);
            var Fail = Json(new { Message = "Data Delete Failed", Icon = "error", Result = -1 }, JsonRequestBehavior.AllowGet);
            if (Result == 1)
                return Success;
            else
                return Fail;
        }

    }
}