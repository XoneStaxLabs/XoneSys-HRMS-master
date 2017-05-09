using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using XoneHR.Models;
using XoneHR.Models.BAL;

using iTextSharp.text;
using iTextSharp.text.pdf;

namespace XoneHR.Controllers
{
    public class CandidateController : Controller
    {
        private CandidateApplication candAppObj;
        private RaceBLayer Raceobj = new RaceBLayer();
        private CommonFunctions common;

        public CandidateController()
        {
            candAppObj = new CandidateApplication();
            common = new CommonFunctions();
        }

        [AuthorizeAll]
        public ActionResult Index()
        {
            var GetOffboardCheckListType = candAppObj.GetOffboardCheckListType();
            SessionManage.Current.PermitFunctions = common.GetPermissionList("Index", "Candidate");
            return View(GetOffboardCheckListType);
        }

        public ActionResult CandidateList(int statusID = 0)
        {
            var CandidateList = (dynamic)null;
            SessionManage.Current.PermitFunctions = common.GetPermissionList("Index", "Candidate");
            if (SessionManage.Current.PermitFunctions.Where(m => m.FunTypeID == FunctionTypes.List).Select(m => m.UserPermiStatus).SingleOrDefault())
            {
                if (statusID != 0)
                {
                    CandidateList = candAppObj.ListCandidateDatatables(0, statusID, 2);
                }
                else
                {
                    CandidateList = candAppObj.ListCandidateDatatables(0, 1, 1);
                }
                return View(CandidateList);
            }
            else
                return View();
        }

        public ActionResult Create(int id = 0)
        {
            if (id == 0)
            {
                Int64 EmployeeId = SessionManage.Current.EmployeeId;
            }

            var GetOffboardCheckListType = candAppObj.GetOffboardCheckListType();
            ViewBag.value = id;
            return View(GetOffboardCheckListType);
        }

        public ActionResult Edit(string CandID, int EditType = 0)
        {
            try
            {
                ViewBag.value = EditType;
                var CandidateProfile = (dynamic)null;
                if (CandID != null)
                {
                    SessionManage.Current.CandID = Convert.ToInt64(CandID);
                    CandidateProfile = candAppObj.ListCandidateDetails(Convert.ToInt64(CandID), EditType);
                    return View(CandidateProfile);
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult GetTabValdation(Int64 CandID = 0)
        {
            try
            {
                var tabdetails = candAppObj.GetTabValdation(CandID);
                return Json(tabdetails, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult Candidateprofile(string CandID)
        {
            SessionManage.Current.CandID = Convert.ToInt64(CandID);
            SessionManage.Current.PermitFunctions = common.GetPermissionList("Index", "Candidate");
            var CandidateProfile = candAppObj.ListCandidateDetails(Convert.ToInt64(CandID), 0);
            bool status = candAppObj.GetAppStatus(Convert.ToInt64(CandID));
            ViewBag.AppStatus = status;
            return View(CandidateProfile);
        }

        public ActionResult CandidateInterview()
        {
            return View();
        }

        public ActionResult CandidateInterviewList()
        {
            var CandidateList = candAppObj.ListCandidateDatatables(0, 1, 2);
            return View(CandidateList);
        }

        public JsonResult GetDocumentType()
        {
            var ListDocumentType = candAppObj.ComboListDocumentType();
            return Json(ListDocumentType, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDocumentSubType(int DocumenTypID)
        {
            var ListDocumentType = candAppObj.ComboListDocumentSubType(DocumenTypID);
            return Json(ListDocumentType, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRace()
        {
            var ListRaces = candAppObj.ComboListRace();
            return Json(ListRaces, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetReligion()
        {
            var ListReligion = candAppObj.ComboListReligion();
            return Json(ListReligion, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMariatalStatus()
        {
            var ListMariatalStatus = candAppObj.ComboMaritalStatus();
            return Json(ListMariatalStatus, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCitizenship()
        {
            var ListCitizenship = candAppObj.ComboListCitizenship();
            return Json(ListCitizenship, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDesignation()
        {
            var ListDesignation = candAppObj.CombolistDesignation();
            return Json(ListDesignation, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGrade(int desigid)
        {
            var ListGrade = candAppObj.CombolistGrade(desigid);
            return Json(ListGrade, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddNewCandidate(TblCandidate tblCandidateObj, string[] SkilID, string[] LangknID, string CandDob = null, string CandRegDate = null, string PassportExpiry = null, string IssueDate = null, string ExpiryDate = null, string LicenseNum = null, string FinExpiry = null)
        {
            Int16 race_id = Convert.ToInt16(Request["Race_name"]);
            Int32 relg_id = Convert.ToInt32(Request["Religion_name"]);
            tblCandidateObj.RaceID = race_id;
            tblCandidateObj.ReligID = relg_id;

            tblCandidateObj.CandPhoto = "../images/user.png";
            int count = CandDob.Split('/').Length - 1;
            if (count == 0)
                CandDob = CandDob.Insert(2, "/").Insert(5,"/");

            tblCandidateObj.CandDob = common.CommonDateConvertion(CandDob);
            //tblCandidateObj.CandRegDate = common.CommonDateConvertion(CandRegDate);
            tblCandidateObj.CandRegDate = DateTime.Now.Date;
            if (PassportExpiry != "" && PassportExpiry != "01-01-0001")
                tblCandidateObj.PassportExpiry = common.CommonDateConvertion(PassportExpiry);
            if (FinExpiry != "" && FinExpiry != "01-01-0001")
                tblCandidateObj.FinExpiry = common.CommonDateConvertion(FinExpiry);

            DataTable dt1 = new DataTable();
            dt1.Columns.Add("SkilID", typeof(Int32));

            DataTable dt2 = new DataTable();

            dt2.Columns.Add("LangknID", typeof(Int16));

            if (SkilID != null)
            {
                for (int i = 0; i < SkilID.Length; i++)
                {
                    var skillids = Convert.ToInt32(SkilID[i]);
                    DataRow dr = dt1.NewRow();
                    dr["SkilID"] = skillids;
                    dt1.Rows.Add(dr);
                }
            }

            if (LangknID != null)
            {
                for (int i = 0; i < LangknID.Length; i++)
                {
                    var langids = Convert.ToInt16(LangknID[i]);
                    DataRow dr = dt2.NewRow();
                    dr["LangknID"] = langids;
                    dt2.Rows.Add(dr);
                }
            }
            
            //Int64 LicenseNums;

            //if (LicenseNum != "" && LicenseNum != null)
            //    LicenseNums = Convert.ToInt64(LicenseNum);
            //else
            //    LicenseNums = 0;

            CandidateItems CandObj = candAppObj.AddNewCandidateForm(tblCandidateObj, dt1, dt2, IssueDate, ExpiryDate, LicenseNum);
            if (CandObj.OutputID != 0)
            {
                SessionManage.Current.CandID = CandObj.TableID;
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }

        public JsonResult RaceCreate(string RaceName)
        {
            try
            {
                int result = Raceobj.ManageRace(1, RaceName, 1, 0);
                if (result > 0)
                    return Json(new { Message = "Race Added Successfully", Icon = "success", Result = result }, JsonRequestBehavior.AllowGet);
                else if (result == -1)
                    return Json(new { Message = "Race Name Already Exists", Icon = "warning", Result = result }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { Message = "Race Adding Failed", Icon = "error", Result = 0 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(0);
            }
        }

        public JsonResult ReligionCreate(string ReligionName)
        {
            try
            {                
                int result = candAppObj.ManageReligion(1, ReligionName, 1, 0);
                if (result > 0)
                    return Json(new { Message = "Religion Added Successfully", Icon = "success", Result = result }, JsonRequestBehavior.AllowGet);
                else if (result == -1)
                    return Json(new { Message = "Religion Name Already Exists", Icon = "warning", Result = result }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { Message = "Religion Adding Failed", Icon = "error", Result = 0 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(0);
            }
        }

        public JsonResult AddNewCitizenDetails(TblCitizenDetails tblCitizenobj, string HidTab1, string CitzOrdrDate = null)
        {
            if (SessionManage.Current.CandID != 0)
            {
                tblCitizenobj.CandID = SessionManage.Current.CandID;
                if (CitzOrdrDate != "")
                {
                    tblCitizenobj.CitzOrdrDate = common.CommonDateConvertion(CitzOrdrDate);
                    //tblCitizenobj.CitzOrdrDateOne = common.CommonDateConvertion(CitzOrdrDateOne);

                    CandidateItems candObj = candAppObj.AddNewCitizendetails(tblCitizenobj, Convert.ToInt32(HidTab1));
                    if (candObj.OutputID != 0)
                    {
                        return Json(true);
                    }
                    else
                    {
                        return Json(false);
                    }
                }
                else
                {
                    return Json(false);
                }
            }
            else
            {
                return Json(false);
            }
        }

        public JsonResult AddNewEmployerDetails(TblEmployeHistory tblEmphistory, string empStartenddate, string HidTab3)
        {
            if (SessionManage.Current.CandID != 0)
            {
                CultureInfo provider = CultureInfo.InvariantCulture;
                tblEmphistory.CandID = SessionManage.Current.CandID;

                var Emphdates = empStartenddate.Split('-');

                var date1 = Emphdates[0];
                var date2 = Emphdates[1];

                tblEmphistory.EmphStartDate = Convert.ToDateTime(common.CommonDateConvertion(date1));

                tblEmphistory.EmphEndDate = Convert.ToDateTime(common.CommonDateConvertion(date2));

                CandidateItems candOObj = candAppObj.AddNewEmployeeHistory(tblEmphistory, Convert.ToInt32(HidTab3));
                if (candOObj.OutputID != 0)
                {
                    return Json(candOObj.TableID, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(false);
            }
        }

        public JsonResult AddNewBankDetails(TblCandidateBank tblcandbankobj, string HidTab5)
        {
            if (SessionManage.Current.CandID != 0)
            {
                string CandBnkID = Convert.ToString(tblcandbankobj.CandBnkID);
                var Iresult = candAppObj.CandBnkAcnoAlredyExists(tblcandbankobj.CandBnkAcno, CandBnkID);
                if (Iresult > 0)
                {
                    return Json(new { Message = "Bank A/c Number Already Exists", Icon = "warning", Result = Iresult }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    tblcandbankobj.CandID = SessionManage.Current.CandID;

                    CandidateItems candiobj = candAppObj.AddBankDetails(tblcandbankobj, Convert.ToInt32(HidTab5));
                    if (candiobj.OutputID != 0)
                    {
                        return Json(true);
                    }
                    else
                    {
                        return Json(false);
                    }
                }
            }
            else
            {
                return Json(false);
            }
        }

        public JsonResult UploadNewDocuments()
        {
            if (SessionManage.Current.CandID != 0)
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    TblDocuments tblDoc = new TblDocuments();

                    var file = Request.Files["FileUpload"];
                    var fileName = Path.GetFileName(file.FileName);
                    var Extension = Path.GetExtension(file.FileName);
                    string Photopath = "~/Docs/" + GetUniqueKey() + Extension.ToString();

                    Photopath = Photopath.Replace("~", "");

                    file.SaveAs(Server.MapPath(Photopath));

                    var DocumentTypeID = Request.Form["DocumentTypeID"].ToString();
                    var DocumentSubTypeID = Request.Form["DocumentsubTypeID"].ToString();

                    tblDoc.CandID = SessionManage.Current.CandID;
                    tblDoc.DocStypID = Convert.ToInt32(DocumentSubTypeID);
                    tblDoc.DocFiles = Photopath;

                    CandidateItems candobj = candAppObj.AddDocuments(tblDoc, Convert.ToInt32(Request.Form["HidTab2"].ToString()));

                    if (candobj.OutputID != 0)
                    {
                        return Json(true);
                    }
                    else
                    {
                        return Json(false);
                    }
                }

                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }

        public ActionResult DocumentsList(int DocstypeID = 0)
        {
            var DocumentsList = (dynamic)null;
            TblDocuments docuobj = new TblDocuments();
            docuobj.CandID = SessionManage.Current.CandID;
            docuobj.DocStypID = DocstypeID;

            if (DocstypeID != 0)
            {
                DocumentsList = candAppObj.ListDocuments(docuobj, 1);
            }
            else
            {
                DocumentsList = candAppObj.ListDocuments(docuobj, 0);
            }

            return View(DocumentsList);
        }

        public ActionResult EmployerHistoryList(string TableID)
        {
            Int64 CandId = SessionManage.Current.CandID;
            var EmpHistoryList = candAppObj.ListEmpHistory(CandId, TableID);
            return View(EmpHistoryList);
        }

        public ActionResult EditEmployerHistoryList(Int64 CandID, string EmphID)
        {
            var EmpHistoryList = candAppObj.EditListEmpHistory(CandID, EmphID);
            return View(EmpHistoryList);
        }

        public ActionResult EmployerHistoryListProf(string TableID)
        {
            Int64 CandId = SessionManage.Current.CandID;
            var EmpHistoryList = candAppObj.ListEmpHistory(CandId, TableID);
            return View(EmpHistoryList);
        }

        public ActionResult DocumentsLisTablewise(int DocstypeID = 0)
        {
            var DocumentsList = (dynamic)null;
            TblDocuments docuobj = new TblDocuments();
            docuobj.CandID = SessionManage.Current.CandID;
            docuobj.DocStypID = DocstypeID;

            if (DocstypeID != 0)
            {
                DocumentsList = candAppObj.ListDocuments(docuobj, 1);
            }
            else
            {
                DocumentsList = candAppObj.ListDocuments(docuobj, 0);
            }

            return View(DocumentsList);
        }

        public int ApproveCandidate(int appStatus, Int64 CandID = 0, string Remarks=null)
        {
            if (CandID == 0)
                CandID = SessionManage.Current.CandID;
            CandidateItems candObj = candAppObj.CandidateApproval(CandID, appStatus, Remarks);
            if (candObj.OutputID == 2)
            {
                return 2;
            }
            else if (candObj.OutputID != 0)
            {
                return 1;
            }
            else
                return 0;
        }

        public ActionResult CandidateSelectionList()
        {
            var CandidateList = candAppObj.ListCandidateDatatables(0, 2, 2);
            return View(CandidateList);
        }

        public ActionResult CandidateSelection()
        {
            return View();
        }

        public ActionResult CandidateAppoinmentletter(string CandID)
        {
            ViewBag.candID = CandID;
            var Empdetails = candAppObj.ListCandidateEmpDetails(Convert.ToInt64(CandID));
            var CandidateProfile = candAppObj.ListCandidateDetails(Convert.ToInt64(CandID), 0);
            ViewBag.Empdetails = Empdetails;
            ViewBag.BasicSal = candAppObj.GetBasicSal(Convert.ToInt64(CandID));
            return View(CandidateProfile);
        }

        public JsonResult AddNewEmployee(CandidateAppoinment candAppoinobj, string CPF = null, string SalaWeekOff = null, double Levy = 0, string StartDate = null, string EndDate = null, string EmpPartTime_Pay = null, string[] RestDay_Fixed = null)
        {
            //candAppoinobj.Employeetyp = 1;
            candAppoinobj.EmployeeType = 1;
            if (candAppoinobj.CandID == 0)
                candAppoinobj.CandID = SessionManage.Current.CandID;

            //Fund type from multi dropdown
            //DataTable dt = new DataTable();
            //dt.Columns.Add("FundType");
            //if (FundType != null)
            //{
            //    for (int i = 0; i < FundType.Length; i++)
            //    {
            //        DataRow dr = dt.NewRow();
            //        dr["FundType"] = Convert.ToInt16(FundType[i]);
            //        dt.Rows.Add(dr);
            //    }
            //}

            candAppoinobj.StartDate = common.CommonDateConvertion(StartDate);

            bool Cpf;
            if (CPF != null)
                Cpf = (CPF == "1" ? true : false);
            else
                Cpf = false;

            candAppoinobj.Emp_IsApproved = (candAppoinobj.Emp_IsApproved == true ? true : false);

            if (EmpPartTime_Pay == null)
            {
                candAppoinobj.EmpPartTime_Pay = 0;
            }
            else
                candAppoinobj.EmpPartTime_Pay = Convert.ToInt16(EmpPartTime_Pay);

            CandidateDesigItems candObj = candAppObj.candidateAppoinment(candAppoinobj, Cpf, Levy, SalaWeekOff, EndDate);
            if (candObj.OutputID == 1)
            {
                SessionManage.Current.EmployeeId = candObj.TableID;

                //if (RestDay_Fixed != null)
                //{
                //    DataTable dt1 = new DataTable();
                //    dt1.Columns.Add("RestDay_Fixed", typeof(Int16));
                //    for (int i = 0; i < RestDay_Fixed.Length; i++)
                //    {
                //        var ids = Convert.ToInt32(RestDay_Fixed[i]);
                //        DataRow dr = dt1.NewRow();
                //        dr["RestDay_Fixed"] = ids;
                //        dt1.Rows.Add(dr);
                //    }
                //    candAppObj.UpdateRestDayFixed(dt1, candObj.TableID);
                //}
                //if(candAppoinobj.RestDay_Fixed != -1)
                //    candAppObj.UpdateRestOptional(candAppoinobj.RestDay_Optional, candObj.TableID);

                var listAllowances = candAppObj.ListFixedAllowanceDesigwise(candObj.DesignationID, 1, 0);

                foreach (var item in listAllowances)
                {
                    TblFixedAllowanceEmpwise obj = new TblFixedAllowanceEmpwise();
                    obj.FixSalaTypID = item.FixSalaTypID;
                    obj.EmpID = candObj.TableID;
                    obj.FixAllEmpStatus = true;

                    CandidateItems CanditemsObj = candAppObj.AddfixedAllowanceDesig(obj);

                    var listAllowanceTypes = candAppObj.ListFixedAllowanceDesigwise(candObj.DesignationID, 2, item.FixAllDesigID);

                    foreach (var itemone in listAllowanceTypes)
                    {
                        TblFixedAllowanceEmpTypes allowanceObj = new TblFixedAllowanceEmpTypes();
                        allowanceObj.FixallEmpTypes = itemone.FixallTypes;
                        allowanceObj.FixallEmpAmounts = itemone.FixallAmounts;
                        allowanceObj.FixallempStatus = true;
                        allowanceObj.FixAllEmpD = Convert.ToInt32(CanditemsObj.TableID);

                        CandidateItems CanditemsObjone = candAppObj.AddfixedAllowanceTypes(allowanceObj);
                    }
                }

                return Json(new { Id = 1, CanId = SessionManage.Current.CandID });
            }
            else if (candObj.OutputID == 2)
            {
                SessionManage.Current.EmployeeId = candObj.TableID;
                return Json(new { Id = 2 });
            }
            else
                return Json(new { Id = 0 });
        }

        public JsonResult EditNewEmployee(CandidateAppoinment candAppoinobj, string CPF = null, double Levy = 0, string StartDate = null, string EndDate = null)
        {
            if (candAppoinobj.CandID == 0)
            {
                candAppoinobj.CandID = SessionManage.Current.CandID;
            }
            candAppoinobj.StartDate = common.CommonDateConvertion(StartDate);
            if (EndDate != null && EndDate != "")
                candAppoinobj.EndDate = common.CommonDateConvertion(EndDate);

            bool Cpf;
            if (CPF != null)
                Cpf = (CPF == "1" ? true : false);
            else
                Cpf = false;

            candAppoinobj.Emp_IsApproved = (candAppoinobj.Emp_IsApproved == true ? true : false);

            var candObj = candAppObj.EditcandidateAppoinment(candAppoinobj, Cpf, Levy, EndDate);
            if (candObj == 1)
            {
                return Json(new { Id = 1 });
            }
            else if (candObj == 2)
                return Json(new { Id = 2 });
            else
            {
                return Json(new { Id = 0 });
            }
        }

        public JsonResult GetCitizenDetails(Int16 CitizenID)
        {
            var Citizendetails = candAppObj.GetCitizenDetails(CitizenID);
            return Json(Citizendetails, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEditCitizenDetails(Int64 CandID)
        {
            var EditCitizendetails = candAppObj.GetEditCitizenDetails(CandID);
            return Json(EditCitizendetails, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBankDetails(Int64 CandID)
        {
            var Bankdetails = candAppObj.GetBankDetails(CandID);
            return Json(Bankdetails, JsonRequestBehavior.AllowGet);
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

        public JsonResult UploadPhoto()
        {
            if (SessionManage.Current.CandID != 0)
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files["FileUpload"];
                    var fileName = Path.GetFileName(file.FileName);
                    var Extension = Path.GetExtension(file.FileName);
                    string Photopath = "~/CandidatePhoto/" + GetUniqueKey() + Extension.ToString();
                    file.SaveAs(Server.MapPath(Photopath));

                    Photopath = Photopath.Replace("~", "");

                    CandidateItems candobj = candAppObj.UploadPhoto(SessionManage.Current.CandID, Photopath);

                    if (candobj.OutputID != 0)
                    {
                        return Json(true);
                    }
                    else
                    {
                        return Json(false);
                    }
                }

                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }

        public ActionResult GetInertviewQuestion()
        {
            InterviewQstnAnsComponent obj = new InterviewQstnAnsComponent();

            var ListInterviewQns = candAppObj.GetInertviewQuestion();
            var ans = candAppObj.GetAnswers();

            obj.Qustns = ListInterviewQns;
            obj.Answrs = ans;

            return View(obj);
        }

        public ActionResult AddInterviewData(string[] QuestID, string[] Answer, string[] AnsRating)
        {
            if (SessionManage.Current.CandID != 0)
            {
                if (QuestID == null)
                {
                    return Json(new { Message = "Please Add New Question", Result = -1 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    DataTable Dt = new DataTable();
                    Dt.Columns.Add("QuestID");
                    Dt.Columns.Add("Answer");
                    Dt.Columns.Add("AnsRating");
                    //int length = Answer.Count(str => str != "");
                    int length = Answer.Count();
                    for (int inc = 0; inc < length; inc++)
                    {
                        if (Answer[inc] != "")
                        {
                            DataRow dr = Dt.NewRow();
                            dr["QuestID"] = Convert.ToInt64(QuestID[inc]);
                            dr["Answer"] = Convert.ToString(Answer[inc]);
                            dr["AnsRating"] = Convert.ToDouble(AnsRating[inc]);

                            Dt.Rows.Add(dr);
                        }
                    }
                    Int64 CandID = SessionManage.Current.CandID;
                    Int64 result = candAppObj.AddInterviewData(Dt, CandID);

                    if (result != 0)
                    {
                        return Json(new { Message = "Saved Successfully", Result = result }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { Message = "Application Error Please Contact Administrator!", Result = 0, }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            else
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult InterviewQnsAddNew()
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

        public ActionResult SaveInterviewQns(string QuestName)
        {
            try
            {
                int result = candAppObj.ManageInterviewQns(QuestName);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public JsonResult GetSkillSets()
        {
            var skilltype = candAppObj.GetSkillSets();
            return Json(skilltype, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLangKnown()
        {
            var Langs = candAppObj.GetLangKnown();
            return Json(Langs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSelectedSkillset(Int64 CandID)
        {
            var SkillsSet = candAppObj.ListCandSkillsetIDs(Convert.ToInt64(CandID));
            var selected = from cnd in SkillsSet select new[] { cnd.SkilID }.ToArray();
            return Json(selected, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSelectedCandLangKnown(Int64 CandID)
        {
            var LanguageKnown = candAppObj.ListCandLangKnownIDs(Convert.ToInt64(CandID));
            var selected = from cnd in LanguageKnown select new[] { cnd.LangknID }.ToArray();
            return Json(selected, JsonRequestBehavior.AllowGet);
        }

        public ActionResult QuestionAnswerListing()
        {
            try
            {
                var ListInterviewDetails = candAppObj.ListInterview(SessionManage.Current.CandID);
                return View(ListInterviewDetails);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int Mobile_Email_Validation(string Mob = null, string email = null, string ph = null, string NRIC = null, string Fin = null, int flag = 0)
        {
            try
            {
                int valid;
                if (flag == 1)
                    valid = candAppObj.CheckMobileNoUnique(flag, Mob, 1, 0);
                else if (flag == 2)
                    valid = candAppObj.EmailUnique(flag, email, 1, 0);
                else if(flag == 3)
                    valid = candAppObj.CheckPhNoUnique(flag, ph, 1, 0);
                else if (flag == 4)
                    valid = candAppObj.CheckNRIC_Unique(flag, NRIC, 1, 0);
                else  //
                    valid = candAppObj.CheckFin_Unique(flag, Fin, 1, 0);

                return valid;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public int Mobile_Email_ValidationEdit(string Mob = null, string email = null, string ph = null, string NRIC = null, string Fin = null, int flag = 0, Int64 CandID = 0)
        {
            try
            {
                int valid;
                if (flag == 1)
                    valid = candAppObj.CheckMobileNoUnique(flag, Mob, 2, CandID);
                else if (flag == 2)
                    valid = candAppObj.EmailUnique(flag, email, 2, CandID);
                else if (flag == 3)
                    valid = candAppObj.CheckPhNoUnique(flag, ph, 2, CandID);
                else if(flag == 4)
                    valid = candAppObj.CheckNRIC_Unique(flag, NRIC, 2, CandID);
                else  //
                    valid = candAppObj.CheckFin_Unique(flag, Fin, 2, CandID);

                return valid;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public JsonResult AddEmergencyContact(TblEmergencyContact EmergencyContact, int flag, string HidTab4)
        {
            if (SessionManage.Current.CandID != 0)
            {
                EmergencyContact.CandID = SessionManage.Current.CandID;

                CandidateItems candiobj = candAppObj.AddEmergencyContact(EmergencyContact, flag, Convert.ToInt32(HidTab4));
                if (candiobj.OutputID != 0)
                    return Json(true);
                else
                    return Json(false);
            }
            else
                return Json(false);
        }

        public ActionResult OfferLetterFormat()
        {
            var offerdata = candAppObj.OfferLetterContent();
            return View(offerdata);
        }

        [HttpPost]
        public ActionResult SetMail(string lettercontent, string ToEmailID)
        {
            try
            {
                var CompEmaildata = candAppObj.CompanyEmailID(1);

                MailMessage message = new MailMessage();
                message.From = new MailAddress(CompEmaildata.EmailID);
                message.To.Add(new MailAddress(ToEmailID));
                message.Subject = "Offer letter demo";
                message.Body = "Hi, Please find attached the offer letter";
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = CompEmaildata.CompPortNo;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(CompEmaildata.EmailID, CompEmaildata.CompPassword);
                smtp.EnableSsl = true;

                //Create a byte array that will eventually hold our final PDF
                Byte[] bytes;
                using (var ms = new MemoryStream())
                {
                    //Create an iTextSharp Document which is an abstraction of a PDF but **NOT** a PDF
                    using (var doc = new Document())
                    {
                        //Create a writer that's bound to our PDF abstraction and our stream
                        using (var writer = PdfWriter.GetInstance(doc, ms))
                        {
                            doc.Open();

                            //Create a new HTMLWorker bound to our document
                            using (var htmlWorker = new iTextSharp.text.html.simpleparser.HTMLWorker(doc))
                            {
                                //HTMLWorker doesn't read a string directly but instead needs a TextReader (which StringReader subclasses)
                                using (var sr = new StringReader(lettercontent.ToString()))
                                {
                                    //Parse the HTML
                                    htmlWorker.Parse(sr);
                                }
                            }
                            doc.Close();
                        }
                    }
                    bytes = ms.ToArray();
                }

                string pdffile = GetUniqueKey() + ".pdf";
                //var testFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), pdffile);
                var path = Path.Combine(Server.MapPath("~/Docs"), pdffile);
                System.IO.File.WriteAllBytes(path, bytes);

                message.Attachments.Add(new Attachment(path));

                smtp.Send(message);
                return Json(1);
            }
            catch
            {
                return Json(1);
            }
        }

        public ActionResult GetEmployeeType()
        {
            var Emptype = candAppObj.GetEmployeeType();
            return Json(Emptype, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeclarationtabContentList()
        {
            var declarationdetails = candAppObj.GetDeclarationDetails();
            ViewBag.DeclarationContent = candAppObj.GetDeclContent();
            return View(declarationdetails);
        }

        public ActionResult SaveDeclaration(string[] DeclID, string[] CandDeclAns, string[] CandDeclAnsDetail, string HidTab6)
        {
            DataTable Dt = new DataTable();
            Dt.Columns.Add("DeclID");
            Dt.Columns.Add("CandDeclAns");
            Dt.Columns.Add("CandDeclAnsDetail");
            int count = CandDeclAnsDetail.Count();
            string[] CandDeclAnswer = CandDeclAns[0].Split(',');

            for (int i = 0; i < CandDeclAnswer.Count(); i++)
            {
                if (CandDeclAnswer[i] != "")
                {
                    DataRow Dr = Dt.NewRow();
                    Dr["DeclID"] = Convert.ToInt32(DeclID[i]);
                    if (CandDeclAnswer[i] == "1")
                        Dr["CandDeclAns"] = true;
                    else
                        Dr["CandDeclAns"] = false;

                    if (CandDeclAnsDetail[i] == "" && CandDeclAnswer[i] == "1")
                        Dr["CandDeclAnsDetail"] = "";
                    else if (CandDeclAnsDetail[i] == "" && CandDeclAnswer[i] == "0")
                        Dr["CandDeclAnsDetail"] = "";
                    else
                        Dr["CandDeclAnsDetail"] = CandDeclAnsDetail[i];

                    Dt.Rows.Add(Dr);
                }
            }

            int result = candAppObj.SaveDeclaration(Dt, 1, Convert.ToInt32(HidTab6));

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeclarationContentProfile()
        {
            try
            {
                var ListContent = candAppObj.DeclarationDetailsProfile();

                var declarationdetails = candAppObj.GetDeclarationProf();
                if (declarationdetails.Count == 0)
                    ViewBag.DeclarationContent = null;
                else
                    ViewBag.DeclarationContent = declarationdetails;

                return View(ListContent);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult DeclarationtabContentListEdit()
        {
            try
            {
                if (SessionManage.Current.CandID != 0)
                {
                    var ListContent = candAppObj.DeclarationDetailsProfile();

                    var declarationdetails = candAppObj.GetDeclarationProf();
                    if (declarationdetails.Count() == 0)
                    {  //when no datas on edit
                        ViewBag.DeclarationContent = candAppObj.GetDeclarationProf1();
                        ListContent = candAppObj.DeclarationDetailsProfile1();
                        ViewBag.flafg = 1;
                    }
                    else
                        ViewBag.DeclarationContent = declarationdetails;

                    return View(ListContent);
                }
                else
                    return View();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult UpdateDeclaration(string[] DeclID, string[] CandDeclAns, string[] CandDeclAnsDetail, string HidTab6)
        {
            DataTable Dt = new DataTable();
            Dt.Columns.Add("DeclID");
            Dt.Columns.Add("CandDeclAns");
            Dt.Columns.Add("CandDeclAnsDetail");
            int count = CandDeclAnsDetail.Count();
            string[] CandDeclAnswer = CandDeclAns[0].Split(',');

            for (int i = 0; i < CandDeclAnswer.Count(); i++)
            {
                if (CandDeclAnswer[i] != "")
                {
                    DataRow Dr = Dt.NewRow();
                    Dr["DeclID"] = Convert.ToInt32(DeclID[i]);
                    if (CandDeclAnswer[i] == "1")
                        Dr["CandDeclAns"] = true;
                    else
                        Dr["CandDeclAns"] = false;

                    if (CandDeclAnsDetail[i] == "" && CandDeclAnswer[i] == "1")
                        Dr["CandDeclAnsDetail"] = "";
                    else if (CandDeclAnsDetail[i] == "" && CandDeclAnswer[i] == "0")
                        Dr["CandDeclAnsDetail"] = "";
                    else
                        Dr["CandDeclAnsDetail"] = CandDeclAnsDetail[i];

                    Dt.Rows.Add(Dr);
                }
            }

            int result = candAppObj.SaveDeclaration(Dt, 2, Convert.ToInt32(HidTab6));

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public int RemoveEmpHistory(int EmphID)
        {
            var status = candAppObj.RemoveEmpHistory(EmphID);
            return status;
        }

        public int RemoveQuestions(Int64 QuestID)
        {
            var status = candAppObj.RemoveQuestions(QuestID);
            return status;
        }

        public int RemoveDocuments(int DocID)
        {
            var status = candAppObj.RemoveDocuments(DocID);
            return status;
        }

        public ActionResult DocumentsListEdit(int DocstypeID = 0)
        {
            var DocumentsList = (dynamic)null;
            TblDocuments docuobj = new TblDocuments();
            docuobj.CandID = SessionManage.Current.CandID;
            docuobj.DocStypID = DocstypeID;

            if (DocstypeID != 0)
            {
                DocumentsList = candAppObj.ListDocuments(docuobj, 1);
            }
            else
            {
                DocumentsList = candAppObj.ListDocuments(docuobj, 0);
            }

            return View(DocumentsList);
        }

        public ActionResult GetFundTypes()
        {
            var FundTypes = candAppObj.GetFundTypes();
            return Json(FundTypes, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateCandididateSalary(TblCandidate CandUpdateObj, string CandJoinDate)
        {
            try
            {
                CandUpdateObj.CandJoinDate = common.CommonDateConvertion(CandJoinDate);
                var UpdateCand = candAppObj.UpdateCandididateSalary(CandUpdateObj);
                if (UpdateCand == 1)
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult CandAppoinmnetLetterPDF(string lettercontent, string CandId)
        {
            try
            {
                //Create a byte array that will eventually hold our final PDF
                Byte[] bytes;
                using (var ms = new MemoryStream())
                {
                    //Create an iTextSharp Document which is an abstraction of a PDF but **NOT** a PDF
                    using (var doc = new Document())
                    {
                        //Create a writer that's bound to our PDF abstraction and our stream
                        using (var writer = PdfWriter.GetInstance(doc, ms))
                        {
                            doc.Open();
                            writer.CloseStream = false;
                            //Create a new HTMLWorker bound to our document
                            using (var htmlWorker = new iTextSharp.text.html.simpleparser.HTMLWorker(doc))
                            {
                                //HTMLWorker doesn't read a string directly but instead needs a TextReader (which StringReader subclasses)
                                using (var sr = new StringReader(lettercontent.ToString()))
                                {
                                    //Parse the HTML
                                    htmlWorker.Parse(sr);
                                }
                            }
                            doc.Close();
                        }
                    }
                    bytes = ms.ToArray();
                }

                string pdffile = "~/Docs/" + GetUniqueKey() + ".pdf";
                //var testFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), pdffile);
                var path = Server.MapPath(pdffile);
                System.IO.File.WriteAllBytes(path, bytes);
                path = path.Replace("~", "");
                var UpdateEmp = candAppObj.UpdateEmployeePDFPath(pdffile, Convert.ToInt64(CandId));

                //var file = Request.Files["FileUpload"];
                //var fileName = Path.GetFileName(file.FileName);
                //var Extension = Path.GetExtension(file.FileName);
                //string Photopath = "~/Docs/" + GetUniqueKey() + Extension.ToString();

                //file.SaveAs(Server.MapPath(Photopath));

                return Json(1);
            }
            catch (Exception ex)
            {
                return Json(0);
            }
        }

        public ActionResult GetEditEmployeeEmpDetails(Int64 CandID)
        {
            try
            {
                var EditCitizendetails = candAppObj.GetEditEmployeeEmpDetails(CandID);
                if (EditCitizendetails == null)
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(EditCitizendetails, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AddPLRDDetails(TblPLRDDetails PLRDDetails, string HidTab8, string IssueDate = null, string ExpiryDate = null, string LicenseNum = null)
        {
            if (SessionManage.Current.CandID != 0)
            {
                PLRDDetails.CandID = SessionManage.Current.CandID;
                PLRDDetails.IssueDate = common.CommonDateConvertion(IssueDate);
                PLRDDetails.ExpiryDate = common.CommonDateConvertion(ExpiryDate);
                //if (LicenseNum != "" && LicenseNum != null)
                //    PLRDDetails.LicenseNum = Convert.ToInt64(LicenseNum);
                PLRDDetails.LicenseNum = LicenseNum;
                CandidateItems candiobj = candAppObj.AddPLRDDetails(PLRDDetails, Convert.ToInt32(HidTab8));
                if (candiobj.OutputID != 0)
                {
                    return Json(true);
                }
                else
                {
                    return Json(false);
                }
            }
            else
                return Json(false);
        }

        public JsonResult GetPLRDDetails(Int64 CandID)
        {
            var details = candAppObj.GetPLRDDetails(CandID);
            return Json(details, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetResidentStatus()
        {
            try
            {
                var list = candAppObj.GetResidentStatus();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetDesigAllowances(Int64 DesigID, Int64 Empid, Int64 CandID)
        {
            try
            {
                DesignationAssignment DesigAssignment = new DesignationAssignment();
                DesigAssignment.FixedSalaryType = candAppObj.GetFixecSalaryTypes(DesigID);
                DesigAssignment.FixedAllowanceMaster = candAppObj.GetFixedAllowanceMaster(DesigID);
                ViewBag.DesigID = DesigID;
                if (Empid == 0)
                    Empid = SessionManage.Current.EmployeeId;
                ViewBag.Empid = Empid;
                ViewBag.CandID = CandID;
                return View(DesigAssignment);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult EmployeeAllowanceSave(string ParentID, string ChildId, Int64 EmpID)
        {
            try
            {
                var FixSalaTypID = ParentID.Replace("\"", "").Replace("[", "").Replace("]", "").Replace("null", "");
                var FixallType = ChildId.Replace("\"", "").Replace("[", "").Replace("]", "").Replace("null", "");

                if (EmpID > 0)
                {
                    if (FixSalaTypID == "" && FixallType == "")
                    {
                        return Json(new { Message = "Please Select Atleast One CheckBox", Result = -1 }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        string[] FixedID = FixSalaTypID.Split(',');
                        string[] FixallTypeId = FixallType.Split(',');

                        var length1 = FixedID.Count();
                        var length2 = FixallTypeId.Count();

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
                        ChildDet.Columns.Add("FixallowmAmounts", typeof(float));
                        ChildDet.Columns.Add("FixallEmpTypes", typeof(string));
                        for (int i = 0; i < length2; i = i + 3)
                        {
                            DataRow newrow;
                            newrow = ChildDet.NewRow();
                            newrow["FixSalaTypID"] = FixallTypeId[i];
                            newrow["FixallowmAmounts"] = Convert.ToDouble(FixallTypeId[i + 2]);
                            newrow["FixallEmpTypes"] = FixallTypeId[i + 1];
                            ChildDet.Rows.Add(newrow);
                        }

                        Int64 EmpId = EmpID;

                        var Iresult = candAppObj.AllowanceEmpAssignmentSave(Parent, ChildDet, EmpId);
                        if (Iresult > 0)
                        {
                            return Json(new { Message = "Allowances Assigned Saved Successfully", Result = 1 }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { Message = "Allowances Assigngned Saving Failed !!", Result = 0 }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                else
                {
                    return Json(new { Message = "Saving Failed, First Save Employee Details ", Result = 0 }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult GetDesignationLeaves(Int64 DesigID, Int64 Empid, Int64 CandID)
        {
            try
            {
                var List = candAppObj.GetLeaveMasterwise(DesigID);
                if (Empid == 0)
                    Empid = SessionManage.Current.EmployeeId;
                ViewBag.Empid = Empid;
                ViewBag.CandID = CandID;
                return View(List);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult EmployeeLeaveSave(string Leave, Int64 EmpID)
        {
            try
            {
                if (EmpID > 0)
                {
                    var LeaveID = Leave.Replace("\"", "").Replace("[", "").Replace("]", "").Replace("null", "");
                    string[] Leavetyp_ID = LeaveID.Split(',');
                    var length3 = Leavetyp_ID.Count();
                    DataTable Leavedt = new DataTable();
                    Leavedt.Columns.Add("LeavetypID", typeof(int));
                    Leavedt.Columns.Add("LeavesMastNo", typeof(string));
                    for (int i = 0; i < length3; i = i + 2)
                    {
                        DataRow newrow;
                        newrow = Leavedt.NewRow();
                        newrow["LeavetypID"] = Leavetyp_ID[i];
                        newrow["LeavesMastNo"] = Convert.ToString(Leavetyp_ID[i + 1]);
                        Leavedt.Rows.Add(newrow);
                    }
                    Int64 EmployeeId = EmpID;
                    var Iresult = candAppObj.EmployeeLeaveSave(Leavedt, EmployeeId);
                    if (Iresult > 0)
                    {
                        return Json(new { Message = "Leaves Saved Successfully", Result = 1 }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { Message = "Leaves Saving Failed !!", Result = 0 }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { Message = "Saving Failed, First Save Employee Details", Result = 0 }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult UpdateCandidateStatus(Int64 CandID) //Appoinment letter confirm
        {
            try
            {
                return candAppObj.UpdateCandidateStatus(CandID);
            }
            catch (Exception ex)
            {
                return Json(new { Message = "Application Error Please Contact Administrator!", Result = 2 }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult CandidateApproveFromList(Int64 CandID)
        {
            var CandidateProfile = candAppObj.ListCandidateDetails(Convert.ToInt64(CandID), 0);
            return View(CandidateProfile);
        }

        public ActionResult EmployeeDetails(Int16 Resident_Id, Int64 CandID, float CandSalrexpted)
        {
            ViewBag.Resident_Id = Resident_Id;
            ViewBag.CandID = CandID;
            ViewBag.CandSalrexpted = CandSalrexpted;
            return View();
        }

        public ActionResult EmployeeDetailsEdit(Int64 EmployyeID)
        {
            var details = candAppObj.EmployeeDetailsEdit(EmployyeID);
            return View(details);
        }

        public ActionResult EmployeeLeaves(Int64 EmployeeId)
        {
            if (EmployeeId == 0)
            {
                EmployeeId = SessionManage.Current.EmployeeId;
            }

            ViewBag.EmployeeID = EmployeeId;
            var Leavedetails = candAppObj.GetLeaveDetails(EmployeeId);
            return View(Leavedetails);
        }

        public ActionResult CheckListTypeEdit(Int64 Emp_ID)
        {
            var GetOffboardCheckListType = candAppObj.GetOffboardCheckListTypeEdit(Emp_ID);
            return View(GetOffboardCheckListType);
        }

        public int SaveLeaveDetails(int EmpID, string[] EligibleLeaves = null, string[] EarnedLeaves = null, string[] LeavesTaken = null, string[] LeavesText = null, string[] LeavetypID = null, string HidTab9 = null)
        {
            try
            {
                for (int i = 0; i < EligibleLeaves.Length; i++)
                {
                    LeaveDetails details = new LeaveDetails();
                    details.EligibleLeaves = Convert.ToDouble(EligibleLeaves[i]);
                    details.EarnedLeaves = Convert.ToDouble(EarnedLeaves[i]);
                    details.LeavesTaken = Convert.ToDouble(LeavesTaken[i]);
                    details.LeavesText = LeavesText[i];
                    details.LeavetypID = Convert.ToInt16(LeavetypID[i]);
                    details.EmployeeID = EmpID;
                    candAppObj.SaveLeaveDetails(details, Convert.ToInt32(HidTab9));
                }
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public ActionResult OffboardCheckListSave(string Data, string Cnd_id, string HidTab8=null,int flag=0)
        {
            try
            {
                var iResult = 0;
                Int64 Empid = 0;
                Empid = candAppObj.Find(Convert.ToInt64(Cnd_id));
                string Items = Data.Replace("\"", "").Replace("[", "").Replace("]", "");
                if (Items == "")
                {
                    if (flag == 1)
                    {
                        candAppObj.CheckListSave(Convert.ToInt64(Cnd_id), Empid);
                        return Json(new { Message = "Checklist may update on Employee Edit", Result = flag }, JsonRequestBehavior.AllowGet);
                    }                       
                    else
                        return Json(new { Message = "Please Select Atleast One", Result = -1 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    
                    candAppObj.OffboardCheckListDelete(Empid);
                    string[] CheckList = Items.Split(',');
                    for (int i = 0; i < CheckList.Length; i = i + 4)
                    {
                        TblOnboardCheckList usergobj = new TblOnboardCheckList();
                        usergobj.CheckListTypeID = Convert.ToInt16(CheckList[i]);
                        usergobj.Price = Convert.ToDouble(CheckList[i + 1]);
                        if (CheckList[i + 2] == "")
                        {
                            candAppObj.OffboardCheckListDelete(Empid);
                            return Json(new { Message = "Please Fill Quantity", Result = -1 }, JsonRequestBehavior.AllowGet);
                        }
                        if (CheckList[i + 3] != "")
                        {
                            // return Json(new { Message = "Please Fill Size", Result = -1 }, JsonRequestBehavior.AllowGet);
                            usergobj.Size = CheckList[i + 3];
                        }

                        usergobj.Quantity = Convert.ToInt32(CheckList[i + 2]);

                        usergobj.CheckListStatus = 1;
                        usergobj.EmpID = Empid;
                        //  candAppObj.OffboardCheckListEdit(Empid, usergobj.CheckListTypeID);
                        if (HidTab8 != null)
                            candAppObj.OffboardCheckListSave(usergobj, Convert.ToInt64(Cnd_id), Convert.ToInt32(HidTab8));
                        else
                            candAppObj.OffboardCheckListSave(usergobj, Convert.ToInt64(Cnd_id), 0);
                    }
                }
                if (Empid > 0)
                {
                    return Json(new { Message = "Saved Successfully", Result = Empid }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Message = "Saving Failed", Result = iResult }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //public ActionResult CandBnkAcnoAlredyExists(string bankacc = "", Int64 Can_ID = 0)
        //{
        //    if (Can_ID == 0)
        //    {
        //        var Iresult = candAppObj.CandBnkAcnoAlredyExists(bankacc, Can_ID);
        //        if (Iresult > 0)
        //        {
        //            return Json(new { Message = "Supplier Code Already Exists", Icon = "warning", Result = Iresult }, JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return Json(0, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    else
        //    {
        //        var Iresult = candAppObj.CandBnkAcnoAlredyExists(bankacc, Can_ID);
        //        if (Iresult > 0)
        //        {
        //            return Json(new { Message = "Supplier Code Already Exists", Icon = "warning", Result = Iresult }, JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return Json(0, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //}

        public ActionResult GetEmpSubType(Int16 EmpTypID)
        {
            var EmpSub = candAppObj.GetEmpSubType(EmpTypID);
            return Json(EmpSub, JsonRequestBehavior.AllowGet);
        }

    }
}