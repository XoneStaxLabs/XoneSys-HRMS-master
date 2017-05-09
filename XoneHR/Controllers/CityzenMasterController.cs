using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XoneHR.Models;

namespace XoneHR.Controllers
{
    public class CityzenMasterController : Controller
    {
        CityzenBalLayer cityobj = new CityzenBalLayer();
        CommonFunctions common = new CommonFunctions();

        [AuthorizeAll]
        public ActionResult Index()
        {
            SessionManage.Current.PermitFunctions = common.GetPermissionList("Index", "CityzenMaster");
            return View();
        }

        public ActionResult List()
        {
            try
            {
                SessionManage.Current.PermitFunctions = common.GetPermissionList("Index", "CityzenMaster");
                var CityzenList = cityobj.List();
                return View(CityzenList);
            }
            catch(Exception ex)
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

        public ActionResult Create(string CityZen, string CityZenNoname)
        {
            try
            {
                int result = cityobj.CityzenCreate(CityZen, CityZenNoname,1);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return null;
            }

        }

        public ActionResult Edit(int CizenID, string Citizen, string CitiZenNoname, int status)
        {
            try
            {
                int result = cityobj.CityzenCreate(Citizen, CitiZenNoname, 2, CizenID, status);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult Delete(int CizenID)
        {
            try
            {
                int result = cityobj.CityzenCreate("", "", 3, CizenID);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
