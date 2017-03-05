using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XoneHR.Models;

namespace XoneHR.Controllers
{
    public class RaceController : Controller
    {
        RaceBLayer Raceobj = new RaceBLayer();
        CommonFunctions common = new CommonFunctions();

        [AuthorizeAll]
        public ActionResult Index()
        {
            try
            {
                SessionManage.Current.PermitFunctions = common.GetPermissionList("Index", "Race");
                return View();
            }
            catch(Exception ex)
            {
                return null;
            }
            
        }
        public ActionResult List()
        {
            try
            {
                SessionManage.Current.PermitFunctions = common.GetPermissionList("Index", "Race");
                var RaceList = Raceobj.GetRaceList();
                return View(RaceList);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public ActionResult RaceAddNew()
        {
            try
            {
                return View();
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        public ActionResult Create(string RaceName)
        {
            try
            {
                int result = Raceobj.ManageRace(1, RaceName, 1, 0);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public ActionResult Edit(Int16 RaceID, string RaceName, int status = 0)
        {
            try
            {
                int result = Raceobj.ManageRace(2, RaceName, status, RaceID);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public ActionResult Delete(Int16 RaceID)
        {
            try
            {
                int result = Raceobj.ManageRace(3, "",  0, RaceID);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
