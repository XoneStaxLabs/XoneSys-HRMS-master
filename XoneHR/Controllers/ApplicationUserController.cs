using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XoneHR.Models;
using System.Data;
using XoneHR.Models.BAL;
using System.IO;

namespace XoneHR.Controllers
{
    public class ApplicationUserController : Controller
    {
        ApplicationUserBalLayer AppUserObj = new ApplicationUserBalLayer();
        private CommonFunctions common = new CommonFunctions();

        //[AuthorizeAll]
        public ActionResult Index()
        {
            SessionManage.Current.PermitFunctions = common.GetPermissionList("Index", "ApplicationUser");
            return View();
        }
        public ActionResult AppUserList(Int16 UserType)
        {
            SessionManage.Current.PermitFunctions = common.GetPermissionList("Index", "ApplicationUser");
            var UserList = AppUserObj.GetAppUserList(UserType);
            return View(UserList);
        }
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult Edit(string ID)
        {
            try
            {
                var EditUserList = AppUserObj.GetAppUserEditDetails(Convert.ToInt64(ID));
                return View(EditUserList);

            }catch(Exception ex)
            {
                return null;
            }
        }
        public JsonResult GetCitizenship()
        {
            var ListCitizenship = AppUserObj.ComboListCitizenship();
            return Json(ListCitizenship, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDesignation()
        {
            var ListDesignation = AppUserObj.CombolistDesignation();
            return Json(ListDesignation, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDesignations(Int16 UserTypID)
        {
            var List = AppUserObj.GetDesignations(UserTypID);
            return Json(List, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ManageApplicationUser()
        {
            CandidateItems CandObj = new CandidateItems();
            var Photopath = "";
            TblAppUser UserObj  = new TblAppUser();
            if (Request.Files.Count != 0)
            {
               for (int i = 0; i < Request.Files.Count; i++)
                  {                
                    var file = Request.Files["FileUpload"];
                    var fileName = Path.GetFileName(file.FileName);
                    var Extension = Path.GetExtension(file.FileName);
                    Photopath = "~/CandidatePhoto/" + GetUniqueKey() + Extension.ToString();
                    file.SaveAs(Server.MapPath(Photopath));
                    Photopath = Photopath.Replace("~", "");
                  }
            }
            else
            {
                Photopath = "../images/user.png";
            }
                var AppFirstName = Request.Form["AppFirstName"].ToString();
                var CizenID = Request.Form["CizenID"].ToString();
                var DesigID = Request.Form["DesigID"].ToString();
                var AppAddress = Request.Form["AppAddress"].ToString();
                var AppMobile = Request.Form["AppMobile"].ToString();
                var AppUserName = Request.Form["AppUserName"].ToString();
                var AppPwd = Request.Form["AppPwd"].ToString();
                var AppEmail = Request.Form["AppEmail"].ToString();
                var AppGender = Request.Form["AppGender"].ToString();
                var AppUSerID = Request.Form["AppUID"].ToString();

                UserObj.AppFirstName = AppFirstName;
                UserObj.CitizenID = Convert.ToInt16(CizenID);
                UserObj.DesigID = Convert.ToInt32(DesigID);
                UserObj.AppAddress = AppAddress;
                UserObj.AppMobile = AppMobile;
                UserObj.AppUserName = AppUserName;
                UserObj.AppPwd = AppPwd;
                UserObj.AppEmail = AppEmail;
                UserObj.AppGender = AppGender;
                UserObj.AppPhoto = Photopath;
                UserObj.AppUserTypID = 0;
                UserObj.AppUID = Convert.ToInt64(AppUSerID);

                CandObj = AppUserObj.ManageApplicationUser(UserObj);
                                 
             
            if (CandObj.OutputID == 1)
            {
                return Json(new { Message = "User Data Updated Successfully", Result = CandObj.TableID }, JsonRequestBehavior.AllowGet);

            }
            else if (CandObj.OutputID == -1)
            {
                return Json(new { Message = "User Data Already Exist", Result = -1 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Message = "User Data Updated Failed", Result = 0 }, JsonRequestBehavior.AllowGet);
            }
             
        }
        private string GetUniqueKey()
        {
            try
            {
                //STRING AUTOGENERATING CODE.........
                var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                var random = new Random();
                var result = new string(
                    Enumerable.Repeat(chars, 8)
                              .Select(s => s[random.Next(s.Length)])
                              .ToArray());

                return result.ToString();
            }
            catch
            {
                return "ErrorPage";
            }
        }
        public ActionResult GetjsonData(string AppUserID,Int32 DesigID, int Flag)
        {
            try
            {
                var ItemList = AppUserObj.GetJqxJsonData(Convert.ToInt64(AppUserID), DesigID, Flag);
                var ItemsData = (from a in ItemList select new { id = a.MenuID, parentid = a.MenuParentID, text = a.MenuName, @checked = a.MenuStatus }).ToList();
                return Json(ItemsData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [HttpPost]
        public ActionResult ManageUserPermission(string Data, string AppUserID, Int16 Flag)
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

                    iResult = AppUserObj.ManageUserPermission(DT, Convert.ToInt64(AppUserID),Flag);
                }
                if (iResult == 1)
                {
                    return Json(new { Message = "User Permission Saved Successfully", Result = iResult }, JsonRequestBehavior.AllowGet);
                }
                else if (iResult == 2)
                {
                    return Json(new { Message = "User Permission Already Exist", Result = iResult }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Message = "User Permission Saving Failed", Result = iResult }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Message = "User Permission Saving Failed", Result = 0 }, JsonRequestBehavior.AllowGet);
            }



        }
        [HttpPost]
        public ActionResult Delete(Int64 AppUID)
        {
            try
            {
                bool result = AppUserObj.DeleteAppUser(AppUID);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public ActionResult UserProfile(Int64 AppUID)
        {
            try
            {
                UserProfileDetails UserObj = new UserProfileDetails();
                UserObj.AppUserObj = AppUserObj.GetAppUserEditDetails(AppUID);
                UserObj.MemmouserObj = AppUserObj.GetUserProfileMemo(AppUID);
                UserObj.UserWorkschObj = AppUserObj.GetUserWrokScheduleList(AppUID);
                UserObj.UserLeaveObj = AppUserObj.GetUserLeaveDetails(AppUID);
                return View(UserObj);

            }catch (Exception ex)
            {
                return null;
            }
        }

    }
}
