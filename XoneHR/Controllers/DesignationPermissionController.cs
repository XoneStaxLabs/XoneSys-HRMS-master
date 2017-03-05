using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XoneHR.Models;
using System.Data;
using XoneHR.Models.BAL;

namespace XoneHR.Controllers
{
    public class DesignationPermissionController : Controller
    {
        DesignationPermissionBalLayer DesigPerObj = new DesignationPermissionBalLayer();
        private CommonFunctions common=new CommonFunctions();

       // [AuthorizeAll]
        public ActionResult Index()
        {
            SessionManage.Current.PermitFunctions = common.GetPermissionList("Index", "DesignationPermission");
            return View();
        }
        public ActionResult List()
        {
            SessionManage.Current.PermitFunctions = common.GetPermissionList("Index", "DesignationPermission");
            var DesigDetails = DesigPerObj.GetDesignation();

            return View(DesigDetails);
        }
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult Edit(string ID)
        {
            var List = DesigPerObj.GetAppDesigDetails(Convert.ToInt32(ID));
            return View(List);
        }
        public ActionResult GetDepartment(Int16 UserType)
        {
            try
            {         
                var Dept = DesigPerObj.GetDepartment(UserType);
                return Json(Dept, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return null;
            }            
        }         
        public ActionResult GetjsonData(string DesigID)
        {
            try
            {
                var ItemList = DesigPerObj.GetJqxJsonData(Convert.ToInt32(DesigID));
                var ItemsData = (from a in ItemList select new { id = a.MenuID, parentid = a.MenuParentID, text = a.MenuName, @checked = a.MenuStatus }).ToList();
                return Json(ItemsData, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        [HttpPost]
        public ActionResult ManageDesigPermission(string Data, string DesigID, string AppDepID, string AppDesigName, Int16 Flag,Int16 DesigUserTyp)
        {
            try
            {
                var iResult = 0;
                string Items = Data.Replace("\"", "").Replace("[", "").Replace("]", "");
                if(Items == "")
                {
                    return Json(new { Message = "Please Select Atleast One Menu", Result = -1 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string[] MenuID = Items.Split(',');

                    DataTable DT = new DataTable();
                    DT.Columns.Add("MenuID", typeof(Int32));
                    for(int i=0; i<MenuID.Length;i++)
                    {
                        DataRow newrow;
                        newrow = DT.NewRow();
                        newrow["MenuID"] = MenuID[i];
                        DT.Rows.Add(newrow);
                    }

                    iResult = DesigPerObj.ManageDesigPermission(DT, DesigID, Convert.ToInt16(AppDepID), AppDesigName, Flag, DesigUserTyp);
                }
                if (iResult == 1)
                {
                    return Json(new { Message = "Designation Saved Successfully", Result = iResult }, JsonRequestBehavior.AllowGet);
                }else if(iResult == 2)
                {
                    return Json(new { Message = "Designation Already Exist", Result = iResult }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Message = "Designation Saving Failed", Result = iResult }, JsonRequestBehavior.AllowGet);
                }
            }
            catch(Exception ex)
            {
                return Json(new { Message = "Designation Saving Failed", Result = 0 }, JsonRequestBehavior.AllowGet);
            }
            

           
        }
        [HttpPost]
        public ActionResult Delete(string AppDesigID)
        {
            var iResult = DesigPerObj.DeleteAppDesignation(Convert.ToInt32(AppDesigID));

            if(iResult == true)
            {
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
