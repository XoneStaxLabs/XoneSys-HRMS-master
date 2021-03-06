﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RepositoryImplement.Xone.RepositoryDerive;
using Model.Xone;
using XoneHR.Models;

namespace XoneHR.Areas.MasterLists.Controllers
{
    public class RaceMasterController : Controller
    {
        private readonly IRaceMaster IRaceMaster;
        public RaceMasterController(IRaceMaster IRaceMaster)
        {
            this.IRaceMaster = IRaceMaster;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListRaceDetails()
        {
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();

            List<TblRace> racedetails = new List<TblRace>();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

            var GetAll = IRaceMaster.ListRaceDetails();
            if (!string.IsNullOrEmpty(searchValue))
            {
                GetAll = GetAll.Where(m => m.RaceName.ToLower().Trim().Contains(searchValue.ToLower().Trim())).ToList();
            }

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                GetAll = (from prd in GetAll orderby sortColumn + " " + sortColumnDir select prd).ToList();
            } 

            recordsTotal = GetAll.Count();
            racedetails = GetAll.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = racedetails });
        }

        public JsonResult AddNewRace(TblRace RaceObj)
        {
            var Result = IRaceMaster.CreateRace(RaceObj, SessionManage.Current.UID);
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

        public JsonResult GetDetailsForEdit(Int16 RaceID)
        {
            TblRace race = new TblRace();
            race = IRaceMaster.GetDetailsForEdit(RaceID);
            return Json(race, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditRaceDetails(TblRace race)
        {
            var Result = IRaceMaster.EditRaceDetails(race, SessionManage.Current.UID);
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

        public bool CheckRaceDeletableStatus(Int16 RaceId)
        {
            return IRaceMaster.CheckRaceDeletableStatus(RaceId);
            //if (Status)
            //    return Json(1, JsonRequestBehavior.AllowGet);
            //else
            //    return Json(0, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRaceName(Int16 RaceId)
        {
            var name = IRaceMaster.GetRaceName(RaceId);
            return Json(name, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteRace(Int16 RaceId)
        {
            var Result = IRaceMaster.DeleteRace(RaceId,SessionManage.Current.UID);
            var Success = Json(new { Message = "Data Deleted Successfully", Icon = "success", Result = Result }, JsonRequestBehavior.AllowGet);
            var Fail = Json(new { Message = "Data Delete Failed", Icon = "error", Result = -1 }, JsonRequestBehavior.AllowGet);
            if (Result == 1)
                return Success;
            else
                return Fail;
        }
        
    }
}