using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XoneHR.Models;

namespace XoneHR.Controllers
{
    public class ContractInfoMasterController : Controller
    {
        ContractInfoMasterBLayer contractobj = new ContractInfoMasterBLayer();
        //
        // GET: /ContractInfoMaster/

        public ActionResult Index()
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
        public ActionResult List()
        {
            try
            {
                var ContractList = contractobj.GetContractList();
                return View(ContractList);
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
        public ActionResult ContractInfoType()
        {
            try
            {
                var ContractList = contractobj.GetContractInfoType();
                return Json(ContractList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult Create(string ConInformation,int ConInfoTypID, string ConInfoDetails="")
        {
            try
            {
                int result = contractobj.ManageContractInfo(1, ConInformation, ConInfoDetails, ConInfoTypID, 1, 0);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public ActionResult Edit(int ConInfoID, string ConInformation, int ConInfoTypID, int status = 0, string ConInfoDetails = "")
        {
            try
            {
                int result = contractobj.ManageContractInfo(2, ConInformation, ConInfoDetails, ConInfoTypID, status, ConInfoID);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public ActionResult Delete(Int32 ConInfoID)
        {
            try
            {
                int result = contractobj.ManageContractInfo(3, "","", 0, 0, ConInfoID);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
