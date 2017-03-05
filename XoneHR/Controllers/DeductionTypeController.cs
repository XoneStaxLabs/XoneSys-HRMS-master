using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XoneHR.Models;

namespace XoneHR.Controllers
{
    public class DeductionTypeController : Controller
    {

        DesductionTypebal DesductionTypebalobj = new DesductionTypebal();
        CommonFunctions common = new CommonFunctions();
        //
        // GET: /DeductionType/

        public ActionResult Index()
        {
            try
            {
               // SessionManage.Current.PermitFunctions = common.GetPermissionList("Index", "DeductionType");
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
                //SessionManage.Current.PermitFunctions = common.GetPermissionList("Index", "DeductionType");
                var DeductionTypeList = DesductionTypebalobj.GetDeductionTypeList();
                return View(DeductionTypeList);
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

        public ActionResult Create(string DeductionTypeName)
        {
            try
            {
                int result = DesductionTypebalobj.ManageDeductionType(1, DeductionTypeName, 0);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult Edit(int DeductType_Id, string DeductionTypeName)
        {
            try
            {
                int result = DesductionTypebalobj.ManageDeductionType(2, DeductionTypeName, DeductType_Id);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult Delete(Int32 DeductType_Id)
        {
            try
            {
                int result = DesductionTypebalobj.ManageDeductionType(3, "", DeductType_Id);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
