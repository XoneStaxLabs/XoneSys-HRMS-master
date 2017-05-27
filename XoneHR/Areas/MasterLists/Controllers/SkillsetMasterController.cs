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
    public class SkillsetMasterController : Controller
    {
        private readonly ISkillsetMaster ISkillsetMaster;

        public SkillsetMasterController(ISkillsetMaster ISkillsetMaster)
        {
            this.ISkillsetMaster = ISkillsetMaster;
        }


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListSkillsetDetails()
        {
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();

            List<TblSkillDetails> skills = new List<TblSkillDetails>();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            var GetAll = ISkillsetMaster.ListSkillDetails();
            if (!string.IsNullOrEmpty(searchValue))
            {
                GetAll = GetAll.Where(a => a.SkillName.ToLower().Trim().Contains(searchValue.ToLower().Trim())).ToList();
            }

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                GetAll = (from prd in GetAll orderby sortColumn + " " + sortColumnDir select prd).ToList();
            }
            recordsTotal = GetAll.Count();
            skills = GetAll.Skip(skip).Take(pageSize).ToList();

            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = skills });

        }

        public ActionResult AddNewSkills(TblSkillDetails Skills)
        {
            var Result = ISkillsetMaster.CreateSkills(Skills, SessionManage.Current.UID);
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

        public JsonResult GetEditSkillDetails(Int32 SkillID)
        {
            var details = ISkillsetMaster.GetEditSkillDetails(SkillID);
            return Json(details, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditSkillsets(TblSkillDetails skills)
        {
            var Result = ISkillsetMaster.EditSkillsets(skills, SessionManage.Current.UID);
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

        public bool CheckDeletableStatus(int SkillID)
        {
            return ISkillsetMaster.CheckDeletableStatus(SkillID);
        }
        
        public JsonResult GetSkillName(int SkillID)
        {
            var name = ISkillsetMaster.GetSkillName(SkillID);
            return Json(name, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteSkillset(int SkillID)
        {
            var Result = ISkillsetMaster.DeleteSkillset(SkillID, SessionManage.Current.UID);
            var Success = Json(new { Message = "Data Deleted Successfully", Icon = "success", Result = Result }, JsonRequestBehavior.AllowGet);
            var Fail = Json(new { Message = "Data Delete Failed", Icon = "error", Result = -1 }, JsonRequestBehavior.AllowGet);
            if (Result == 1)
                return Success;
            else
                return Fail;
        }

    }
}