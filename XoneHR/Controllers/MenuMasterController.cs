using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XoneHR.Models;
using XoneHR.Models.BAL;

namespace XoneHR.Controllers
{
    public class MenuMasterController : Controller
    {
        MenuMasterBalLayer MenuMstrBal = new MenuMasterBalLayer();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetMenuMasterParentList()
        {
            try
            {
                var ItemList = MenuMstrBal.GetMenuMasterParentList();
                var ItemsData = (from a in ItemList select new { id = a.MenuID, parentid = a.MenuParentID, text = a.MenuName, MenuController = a.MenuController , MenuAction = a.MenuAction }).ToList();
                return Json(ItemsData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public ActionResult GetMenuMasterData(int id)
        {
            try
            {
                var ItemData = MenuMstrBal.GetMenuMasterData(id);
                return Json(ItemData, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }
        [HttpPost]
        public ActionResult ManageMenuMaster(TblMenuMaster MenuMstr)
        {
            try
            {
                var iResult = MenuMstrBal.ManageMenuMaster(MenuMstr);
                if (iResult == 1)
                {
                    return Json(new { Message = "Data Saved Successfully", Result = iResult }, JsonRequestBehavior.AllowGet);
                }
                else if (iResult == 2)
                {
                    return Json(new { Message = "Data Already Exist", Result = iResult }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Message = "Data Saving Failed", Result = iResult }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
