using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XoneHR.Models;
using XoneHR.Models.BAL;

namespace XoneHR.Controllers
{
    public class EmployeeDesignationController : Controller
    {
        EmployeeDesignationBalLayer DesigBal = new EmployeeDesignationBalLayer();
        CommonFunctions common = new CommonFunctions();
        
        [AuthorizeAll]
        public ActionResult Index()
        {
            SessionManage.Current.PermitFunctions = common.GetPermissionList("Index", "EmployeeDesignation");
            return View();
        }
        public ActionResult List()
        {
            SessionManage.Current.PermitFunctions = common.GetPermissionList("Index", "EmployeeDesignation");
            var Desiglist = DesigBal.GetDesignationList();
            return View(Desiglist);
        }
        public ActionResult GetDepartment()
        {
            try
            {
                var Department = DesigBal.GetDepartment();
                return Json(Department, JsonRequestBehavior.AllowGet);

            }catch(Exception ex)
            {
                return null;
            }
        }
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult Edit(string ID)
        {
            var ListDesig = DesigBal.GetDesignationEdit(Convert.ToInt32(ID));
            return View(ListDesig);
        }
        public ActionResult DesignationAssignment(Int64 DesigID = 0)
        {
            try
            {
               // ViewBag.DesigID = Convert.ToInt32(DesigID);
                DesignationAssignment DesigAssignment = new DesignationAssignment();
                DesigAssignment.FixedSalaryType = DesigBal.GetFixecSalaryTypes(DesigID);
                DesigAssignment.FixedAllowanceMaster = DesigBal.GetFixedAllowanceMaster(DesigID);
                DesigAssignment.SalaryMastrWise = DesigBal.GetSalaryMastrWise(DesigID);
                DesigAssignment.LeaveMasterwise = DesigBal.GetLeaveMasterwise(DesigID);

                return View(DesigAssignment);

            }catch(Exception ex)
            {
                return null;
            }
        }
        public ActionResult ManageDesignationAssignment(string ParentID, string ChildId, string DepID, string DesigName, Int32 DesigID = 0, string SalaMasterPayDate = null, string Salamasterperiod = null, string Leave = null)
        {
            try
            { 
                var FixSalaTypID = ParentID.Replace("\"", "").Replace("[", "").Replace("]", "").Replace("null", "");
                
                var FixallowmID = ChildId.Replace("\"", "").Replace("[", "").Replace("]", "").Replace("null", "");

                var LeaveID = Leave.Replace("\"", "").Replace("[", "").Replace("]", "").Replace("null", "");

                if (FixSalaTypID == "" && FixallowmID == "")
                {
                    return Json(new { Message = "Please Select Atleast One CheckBox", Result = -1 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string[] FixedID = FixSalaTypID.Split(','); 
                    string[] FixedAllowID = FixallowmID.Split(',');
                    string[] Leavetyp_ID = LeaveID.Split(',');
                    

                    var length1 = FixedID.Count();
                    var length2 = FixedAllowID.Count();
                    var length3 = Leavetyp_ID.Count();
                    //DECLARE DATATABLE
                    DataTable Parent = new DataTable();
                    Parent.Columns.Add("FixSalaTypID", typeof(int));

                    for (int i = 0; i < length1; i++)
                    {
                        var FixSalaTyp_ID = Convert.ToInt32(FixedID[i]);
                        DataRow newrow;
                        newrow = Parent.NewRow();
                        newrow["FixSalaTypID"] = FixSalaTyp_ID;
                        Parent.Rows.Add(newrow);
                    }

                    DataTable ChildDet = new DataTable();
                    ChildDet.Columns.Add("FixSalaTypID", typeof(int));
                    ChildDet.Columns.Add("FixallowmID", typeof(int));
                    ChildDet.Columns.Add("FixallowmAmounts", typeof(float));                     
                    for (int i = 0; i < length2; i=i+3)
                    {                         
                        DataRow newrow;
                        newrow = ChildDet.NewRow();                        
                        newrow["FixSalaTypID"] = FixedAllowID[i];
                        newrow["FixallowmID"] = FixedAllowID[i+1];
                        newrow["FixallowmAmounts"] = Convert.ToDouble( FixedAllowID[i+2]);
                        ChildDet.Rows.Add(newrow);                                              
                       
                    }
                    DataTable Leavedt = new DataTable();
                    Leavedt.Columns.Add("LeavetypID", typeof(int));
                    Leavedt.Columns.Add("LeavesMastNo", typeof(string));
                    for (int i = 0; i < length3; i = i+2)
                    {
                        DataRow newrow;
                        newrow = Leavedt.NewRow();
                        newrow["LeavetypID"] = Leavetyp_ID[i];
                        newrow["LeavesMastNo"] = Convert.ToString(Leavetyp_ID[i + 1]);
                        Leavedt.Rows.Add(newrow);

                    }
                    TblDesignation Desig = new TblDesignation();
                    Desig.DesigID = DesigID;
                    Desig.DepID = Convert.ToInt32(DepID);
                    Desig.DesigName = DesigName;
                    int flag = 0;
                    if(DesigID == 0)
                    {
                        flag = 1;
                    }else
                    {
                        flag = 2;
                    }

                    var IresultDesig = DesigBal.ManageDesignation(flag, Desig, DesigID);

                    if (IresultDesig > 0)
                    {
                        var Iresult = DesigBal.ManageDesigssignment(flag, Parent, ChildDet, Convert.ToInt32(IresultDesig), SalaMasterPayDate, Salamasterperiod, Leavedt);
                        if (Iresult > 0)
                        {
                            return Json(new { Message = "Designation Assign Saved Successfully", Result = 1 }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { Message = "Designation Assign Saving Failed !!", Result = 0 }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else if (IresultDesig == -1)
                    {
                        return Json(new { Message = "Designation Exists Already", Result = -1 }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { Message = "Designation Saving Failed !!", Result = 0 }, JsonRequestBehavior.AllowGet);
                    }

                    
                }
                
            }
            catch (Exception ex)
            {
                return Json(new { Message = "Designation Assign Saving Failed !!", Result = 0 }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Delete(TblDesignation DesigID, Int32 DesigSID)
        {
            try
            {
                int result = DesigBal.ManageDesignation(3, DesigID, DesigSID);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public ActionResult Test()
        {
            return View();

        }
        public ActionResult DesignationPermission(string ID , Int16 Type)
        {
            var Datas = DesigBal.AllGetDesigDetails(Convert.ToInt32(ID), Type);

            return View(Datas);
        }
        public ActionResult AllGetDepartment(Int16 UserType)
        {
            try
            {
                var Department = DesigBal.AllGetDepartment(UserType);
                return Json(Department, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public ActionResult AllGetDesignation(string ID, Int16 UserType)
        {
            try
            {
                var Desig = DesigBal.AllGetDesignation(Convert.ToInt32(ID), UserType);
                return Json(Desig, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public ActionResult GetjsonData(string DesigID)
        {
            try
            {
                var ItemList = DesigBal.GetJqxJsonData(Convert.ToInt32(DesigID));
                var ItemsData = (from a in ItemList select new { id = a.MenuID, parentid = a.MenuParentID, text = a.MenuName, @checked = a.MenuStatus }).ToList();
                return Json(ItemsData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [HttpPost]
        public ActionResult AllManageDesigPermission(string Data, string DesigID)
        {
            try
            {
                var iResult = 0;
                string Items = Data.Replace("\"", "").Replace("[", "").Replace("]", "");
                if (Items == "")
                {
                    return Json(new { Message = "Please Select Atleast One Menu", Result = -1 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string[] MenuID = Items.Split(',');

                    DataTable DT = new DataTable();
                    DT.Columns.Add("MenuID", typeof(Int32));
                    for (int i = 0; i < MenuID.Length; i++)
                    {
                        DataRow newrow;
                        newrow = DT.NewRow();
                        newrow["MenuID"] = MenuID[i];
                        DT.Rows.Add(newrow);
                    }

                    iResult = DesigBal.AllManageDesigPermission(DT, DesigID);
                }
                if (iResult == 1)
                {
                    return Json(new { Message = "Designation Saved Successfully", Result = iResult }, JsonRequestBehavior.AllowGet);
                }
                else if (iResult == 2)
                {
                    return Json(new { Message = "Designation Already Exist", Result = iResult }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Message = "Designation Saving Failed", Result = iResult }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Message = "Designation Saving Failed", Result = 0 }, JsonRequestBehavior.AllowGet);
            }



        }
    }
}
