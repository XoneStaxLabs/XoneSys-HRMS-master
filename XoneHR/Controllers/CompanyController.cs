using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using XoneHR.Models;
using XoneHR.Models.BAL;
using System.IO;

namespace XoneHR.Controllers
{
    public class CompanyController : Controller
    {
        public CompanyBalLayer ComyObj = new CompanyBalLayer();
        public ActionResult Index()
        {
            CompanyDetails ComDet = new CompanyDetails();
            ComDet.ComMstr = ComyObj.GetCompanyMaster();
            ComDet.ComEmail = ComyObj.GetCompanyEmails();
            return View(ComDet);
        }
        [HttpPost]
        public ActionResult UpdateCompanyMaster()
        {
            try
            {
                var Photopath = "";
                TblCompanyMaster ComMstrObj = new TblCompanyMaster();
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
                var Name = Request.Form["CompName"].ToString();
                var Test = Request.Form["CompTest"].ToString();

                ComMstrObj.CompName = Name;
                ComMstrObj.CompTest = Test;
                ComMstrObj.CompLogo = Photopath;
                var Result = ComyObj.UpdateCompanyMaster(ComMstrObj);

                if (Result == 1)
                {
                    return Json(new { Message = "Company Data Updated Successfully", Result = 1 }, JsonRequestBehavior.AllowGet);
                }
                else if (Result == -1)
                {
                    return Json(new { Message = "Company Data Already Exist", Result = -1 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Message = "Company Data Updated Failed", Result = 0 }, JsonRequestBehavior.AllowGet);
                }
            }
            catch{
                 return Json(new { Message = "Company Data Updated Failed", Result = 0 }, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public ActionResult UpdateCompanyEmails( Int16[] CompEmailID)
        {
            try
            {
                int Result = 0;
                 
                //string Email_ID = Request[EmailID+rowcnt].ToString();

                for (var i = 0; i < CompEmailID.Length; i++)
                {
                    string name = "EmailID" + CompEmailID[i];
                    string Portno = "CompPortNo" + CompEmailID[i];
                    string password = "CompPassword" + CompEmailID[i];
                    TblCompanyEmail commailobj = new TblCompanyEmail();
                    commailobj.CompEmailID = CompEmailID[i];
                    commailobj.EmailID = Request[name];
                    commailobj.CompPortNo = Convert.ToInt32(Request[Portno]);
                    commailobj.CompPassword = Request[password];

                    Result = ComyObj.UpdateCompanyEmails(commailobj);
                }
                if (Result == 1)
                {
                    return Json(new { Message = "Company Data Updated Successfully", Result = 1 }, JsonRequestBehavior.AllowGet);
                }
                else if (Result == -1)
                {
                    return Json(new { Message = "Company Data Already Exist", Result = -1 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Message = "Company Data Updated Failed", Result = 0 }, JsonRequestBehavior.AllowGet);
                }               

            }catch
            {
               return Json(new { Message = "Company Data Updated Failed", Result = 0 }, JsonRequestBehavior.AllowGet);
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
    }
}
