using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XoneHR.Models;
using XoneHR.Models.BAL;

namespace XoneHR.Controllers
{
    public class MemosController : Controller
    {
        private MemosBal memosbalObj = new MemosBal();

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Index()
        {
            var MemosUserList = memosbalObj.MemosUserList();
            return View(MemosUserList);
        }

        public JsonResult GetDesignations(int filterID)
        {
            var DesignationList = memosbalObj.ListAllDesignation(filterID);
            return Json(DesignationList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddNewmemos(TblMemos memoObj, int[] DesigID, string memo, int[] projID, int[] EmpID, string selectEmp)
        {
            Int32 memosID = memosbalObj.Addnewmemos(memoObj);

            if (memo == "1")
            {
                if (memosID != 0)
                {
                    if (selectEmp == "on")
                    {
                        for (int a = 0; a < EmpID.Count(); a++)
                        {
                            memosbalObj.AddNewEmployeememos(EmpID[a], memosID);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < DesigID.Count(); i++)
                        {
                            var Userids = memosbalObj.GetAppUserIDs(DesigID[i]);

                            foreach (var items in Userids)
                            {
                                memosbalObj.AddnewmemosDesigs(memosID, items.AppUID);
                            }
                        }
                    }

                    return Json(true);
                }
                else
                {
                    return Json(false);
                }
            }
            else
            {
                //project Wise

                for (int c = 0; c < projID.Count(); c++)
                {
                    memosbalObj.AddProjectwisememo(projID[c], memosID);
                }

                return Json(true);
            }
        }

        public ActionResult Notifications()
        {
            var listmemos = memosbalObj.ListMemos(SessionManage.Current.AppUID);
            return View(listmemos);
        }

        public JsonResult GetEmployees(string DesignationID)
        {
            if (DesignationID != "null")
            {
                List<TblAppUser> ListObj = new List<TblAppUser>();
                var EmployeeList = (dynamic)null;

                string Items = DesignationID.Replace("\"", "").Replace("[", "").Replace("]", "");
                string[] DesigIDs = Items.Split(',');

                for (int i = 0; i < DesigIDs.Length; i++)
                {
                    EmployeeList = memosbalObj.ListEmployees(Convert.ToInt32(DesigIDs[i]));
                    ListObj.AddRange(EmployeeList);
                }

                return Json(ListObj, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }

        public JsonResult GetProjects()
        {
            var listproject = memosbalObj.ListProjects();
            return Json(listproject, JsonRequestBehavior.AllowGet);
        }
    }
}