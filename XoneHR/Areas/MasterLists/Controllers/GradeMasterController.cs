using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Xone;
using RepositoryImplement.Xone.RepositoryDerive;

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
    }
}