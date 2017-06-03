using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Xone;
using RepositoryImplement.Xone.RepositoryDerive;

namespace XoneHR.Areas.MasterLists.Controllers
{
    public class EmpCheckListMasterController : Controller
    {
        private ICheckListMaster ICheckListMaster;
        public EmpCheckListMasterController(ICheckListMaster ICheckListMaster)
        {
            this.ICheckListMaster = ICheckListMaster;
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult getListDetails()
        {

            var list = ICheckListMaster.getListDetails();
            return Json(new { data = list, recordsTotal= list.Count() },JsonRequestBehavior.AllowGet);
        }

    }
}