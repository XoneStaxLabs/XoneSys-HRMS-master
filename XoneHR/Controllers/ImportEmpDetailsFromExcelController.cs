using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;
using XoneHR.Models;
using Dapper;
using Excel = Microsoft.Office.Interop.Excel;
using XoneHR.Models.BAL;

namespace XoneHR.Controllers
{
    public class ImportEmpDetailsFromExcelController : Controller
    {
        //
        // GET: /ImportEmpDetailsFromExcel/

        ImportEmpDetailsFromExcelBal BalObj = new ImportEmpDetailsFromExcelBal();
        CommonFunctions common = new CommonFunctions();

        private XoneDbLayer db;
        public ImportEmpDetailsFromExcelController()
        {
            db = new XoneDbLayer();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Import()
        {
            return View();
        }

        [HttpPost]
        //public ActionResult Import(string id)
        //{
        //    var jsnsucess = Json(new { Message = "", val = 0 }, JsonRequestBehavior.AllowGet);
        //    var jsnfaild = Json(new { Message = "", val = 0 }, JsonRequestBehavior.AllowGet);
        //    var jsnIncorrectFileName = Json(new { Message = "", val = 0 }, JsonRequestBehavior.AllowGet);
        //    var jsnIncorrectFiletype = Json(new { Message = "", val = 0 }, JsonRequestBehavior.AllowGet);
        //    var jsnEmptyFile = Json(new { Message = "", val = 0 }, JsonRequestBehavior.AllowGet);
        //    var jsnUnMatchColumn = Json(new { Message = "", val = 0 }, JsonRequestBehavior.AllowGet);

        //    try
        //    {
        //        var UsyFile = Request.Files[0];

        //        if (UsyFile != null)
        //        {
        //            // int strFilePath = obj_file.FolderManagement(LoginType.ToString(), GroupID, CompanyID, OFFID);
        //            var fileName = Path.GetFileName(UsyFile.FileName);
        //            var ext = Path.GetExtension(UsyFile.FileName);
        //            FileInfo fileInfo = new FileInfo(Request.Files[0].FileName);

        //            if (Request.Files[0].ContentLength <= 0)
        //            {
        //                return (jsnEmptyFile);
        //            }
        //            else if (!(fileInfo.Name.Contains(".xlsx") || fileInfo.Name.Contains(".XLSX")))
        //            {
        //                return (jsnIncorrectFiletype);
        //            }
        //            else
        //            {
        //                string Strpath = "~\\Docs";

        //                if (!(Directory.Exists(Server.MapPath(Strpath))))
        //                {
        //                    Directory.CreateDirectory(Server.MapPath(Strpath));
        //                }

        //                var path = Path.Combine(Server.MapPath(Strpath), fileName);
        //                var files = Path.Combine((Strpath), fileName);

        //                if (System.IO.File.Exists(path))
        //                    System.IO.File.Delete(path);

        //                //  using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))

        //                UsyFile.SaveAs(path);
        //                Session["path"] = path;
        //                string CSV_Path = Convert.ToString(Session["path"]);
        //                DataTable dt = new DataTable();
        //                string Feedback = string.Empty;
        //                string line = string.Empty;
        //                string[] strArray;
        //                DataRow row;
        //                Regex r = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
        //                StreamReader sr = new StreamReader(CSV_Path);
        //                line = sr.ReadLine();
        //                //  line = "Cm_id," + line;
        //                strArray = r.Split(line);
        //                Array.ForEach(strArray, s => dt.Columns.Add(new DataColumn()));

        //                while ((line = sr.ReadLine()) != null)
        //                {
        //                    row = dt.NewRow();
        //                    row.ItemArray = r.Split(line);
        //                    dt.Rows.Add(row);
        //                }

        //                sr.Dispose();



        //                DynamicParameters param = new DynamicParameters();
        //                param.Add("@data", dt);
        //                param.Add("@UserID", null, DbType.Int64, ParameterDirection.Output);
        //                param.Add("@ErrorOutput", null, DbType.Int32, ParameterDirection.Output);
        //                int Result = db.DapperExecute("USP_EXPORTFROMCSV", param, CommandType.StoredProcedure);

        //                CandidateItems CandObj = new CandidateItems();
        //                CandObj.TableID = param.Get<Int64>("UserID");
        //                CandObj.OutputID = param.Get<Int32>("ErrorOutput");
        //                return (jsnsucess);

        //            }
        //        }
        //        else
        //        {
        //            return (jsnfaild);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return (jsnfaild);
        //    }

        //}
        public ActionResult Import(string id)
        {
            var jsnsucess = Json(new { Message = "Success", val = 1 }, JsonRequestBehavior.AllowGet);
            var jsnFailed = Json(new { Message = "Failed", val = 0 }, JsonRequestBehavior.AllowGet);

            try
            {
                var UsyFile = Request.Files[0];
                if (UsyFile != null)
                {                    
                    var fileName = Path.GetFileName(UsyFile.FileName);
                    var ext = Path.GetExtension(UsyFile.FileName);
                    FileInfo fileInfo = new FileInfo(Request.Files[0].FileName);
                    string Strpath = "~\\Docs";
                    if (!(Directory.Exists(Server.MapPath(Strpath))))
                    {
                        Directory.CreateDirectory(Server.MapPath(Strpath));
                    }
                    var path = Path.Combine(Server.MapPath(Strpath), fileName);
                    var files = Path.Combine((Strpath), fileName);
                    if (!System.IO.File.Exists(path))
                        UsyFile.SaveAs(path);

                    Session["path"] = path;
                }

                Excel.Application xlApp = new Excel.Application();
                Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(Session["path"].ToString());
                Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
                Excel.Range xlRange = xlWorksheet.UsedRange;
               
                int rCnt = 0;

                DataTable ExcelData = new DataTable();
                ExcelData.Columns.Add("name", typeof(string));                
                ExcelData.Columns.Add("ADDRESS", typeof(string));
                ExcelData.Columns.Add("phone", typeof(string));
                ExcelData.Columns.Add("mobile", typeof(string));
                ExcelData.Columns.Add("email", typeof(string));
                ExcelData.Columns.Add("gender", typeof(string));
                ExcelData.Columns.Add("DATE_OF_BIRTH", typeof(string));                
                ExcelData.Columns.Add("Race", typeof(string));
                ExcelData.Columns.Add("HighEducation", typeof(string));
                ExcelData.Columns.Add("Grade", typeof(string));
                ExcelData.Columns.Add("Nationality", typeof(string));
                ExcelData.Columns.Add("NRIC_IN_NO", typeof(string));                
                ExcelData.Columns.Add("PLRD_Exp", typeof(string));
                ExcelData.Columns.Add("EmpType", typeof(Int16));
                ExcelData.Columns.Add("BASIC_SALARY", typeof(string));
               
                DataRow newrow;
                for (rCnt = 2; rCnt <= xlRange.Rows.Count - 1; rCnt++)
                {
                    newrow = ExcelData.NewRow();
                    newrow["name"] = Convert.ToString((xlRange.Cells[rCnt,2] as Excel.Range).Value2);
                    if ((xlRange.Cells[rCnt,3] as Excel.Range).Value2 != null)
                        newrow["ADDRESS"] = Convert.ToString((xlRange.Cells[rCnt,3] as Excel.Range).Value2);
                    else
                        newrow["ADDRESS"] = null;

                    if ((xlRange.Cells[rCnt, 4] as Excel.Range).Value2 != null)
                        newrow["phone"] = Convert.ToString((xlRange.Cells[rCnt, 4] as Excel.Range).Value2);
                    else
                        newrow["phone"] = null;

                    if ((xlRange.Cells[rCnt, 5] as Excel.Range).Value2 != null)
                        newrow["mobile"] = Convert.ToString((xlRange.Cells[rCnt, 5] as Excel.Range).Value2);
                    else
                        newrow["mobile"] = null;
                    
                    string email = (xlRange.Cells[rCnt,6] as Excel.Range).Value2;
                    if (email != null)
                        newrow["email"] = Convert.ToString((xlRange.Cells[rCnt,6] as Excel.Range).Value2);
                    else
                        newrow["email"] = null;

                    newrow["gender"] = Convert.ToString((xlRange.Cells[rCnt,7] as Excel.Range).Value2);
                    //DateTime DATE_OF_BIRTH = DateTime.FromOADate(Convert.ToDouble((xlRange.Cells[rCnt,9] as Excel.Range).Value2));
                    if ((xlRange.Cells[rCnt,9] as Excel.Range).Value2 != null)
                        newrow["DATE_OF_BIRTH"] = DateTime.FromOADate(Convert.ToDouble((xlRange.Cells[rCnt, 9] as Excel.Range).Value2));
                    else
                        newrow["DATE_OF_BIRTH"] = null;

                    if ((xlRange.Cells[rCnt, 12] as Excel.Range).Value2 != null)
                        newrow["Race"] = Convert.ToString((xlRange.Cells[rCnt, 12] as Excel.Range).Value2);
                    else
                        newrow["Race"] = null;

                    if((xlRange.Cells[rCnt, 13] as Excel.Range).Value2 != null)
                        newrow["HighEducation"] = Convert.ToString((xlRange.Cells[rCnt,13] as Excel.Range).Value2);
                    else
                        newrow["HighEducation"] = null;

                    newrow["Grade"] = Convert.ToString((xlRange.Cells[rCnt,15] as Excel.Range).Value2);
                    newrow["Nationality"] = Convert.ToString((xlRange.Cells[rCnt,16] as Excel.Range).Value2);
                    newrow["NRIC_IN_NO"] = Convert.ToString((xlRange.Cells[rCnt,18] as Excel.Range).Value2);
                   
                    if ((xlRange.Cells[rCnt,21] as Excel.Range).Value2 != null)
                        newrow["PLRD_Exp"] = DateTime.FromOADate(Convert.ToDouble((xlRange.Cells[rCnt,21] as Excel.Range).Value2));
                    else
                        newrow["PLRD_Exp"] = null;

                    newrow["EmpType"] = Convert.ToInt16((xlRange.Cells[rCnt,24] as Excel.Range).Value2);
                    newrow["BASIC_SALARY"] = Convert.ToString((xlRange.Cells[rCnt,30] as Excel.Range).Value2);
                 
                    ExcelData.Rows.Add(newrow);
                }

                int result = 0;//int result = BalObj.insertIntoTemp(ExcelData, 1);
                if (result > 0)
                {
                    //result = BalObj.ProcessTempData();
                    if (result > 0)
                    {
                        return jsnsucess;
                    }
                    else
                    {
                        return jsnFailed;
                    }
                }
                else
                {
                    return jsnFailed;
                }
            }
            catch (Exception ex)
            {
                return jsnFailed;
            }
        }

        public ActionResult Update(string id)
        {
            var jsnsucess = Json(new { Message = "Success", val = 1 }, JsonRequestBehavior.AllowGet);
            var jsnFailed = Json(new { Message = "Failed", val = 0 }, JsonRequestBehavior.AllowGet);

            try
            {
                var UsyFile = Request.Files[0];
                if (UsyFile != null)
                {
                    var fileName = Path.GetFileName(UsyFile.FileName);
                    var ext = Path.GetExtension(UsyFile.FileName);
                    FileInfo fileInfo = new FileInfo(Request.Files[0].FileName);
                    string Strpath = "~\\Docs";
                    if (!(Directory.Exists(Server.MapPath(Strpath))))
                    {
                        Directory.CreateDirectory(Server.MapPath(Strpath));
                    }
                    var path = Path.Combine(Server.MapPath(Strpath), fileName);
                    var files = Path.Combine((Strpath), fileName);
                    if (!System.IO.File.Exists(path))
                        UsyFile.SaveAs(path);

                    Session["path"] = path;
                }

                Excel.Application xlApp = new Excel.Application();
                Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(Session["path"].ToString());
                Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
                Excel.Range xlRange = xlWorksheet.UsedRange;

                int rCnt = 0;

                DataTable ExcelData = new DataTable();
                ExcelData.Columns.Add("NRIC_IN_NO", typeof(string));
                ExcelData.Columns.Add("EmpName", typeof(string));
                ExcelData.Columns.Add("PLRDIssue_Date", typeof(string));
                ExcelData.Columns.Add("PLRDExp_Date", typeof(string));

                DataRow newrow;
                for (rCnt = 2; rCnt <= xlRange.Rows.Count - 1; rCnt++)
                {
                    newrow = ExcelData.NewRow();
                    newrow["NRIC_IN_NO"] = Convert.ToString((xlRange.Cells[rCnt, 2] as Excel.Range).Value2);                    
                    newrow["EmpName"] = Convert.ToString((xlRange.Cells[rCnt, 4] as Excel.Range).Value2);
                    DateTime Issue_Date = DateTime.FromOADate(Convert.ToDouble((xlRange.Cells[rCnt, 9] as Excel.Range).Value2));
                    //if ((xlRange.Cells[rCnt, 9] as Excel.Range).Value2 != null)
                    newrow["PLRDIssue_Date"] = common.ChangeDateToSqlFormat(Issue_Date); //ToShortDateString();
                    DateTime Exp_Date = DateTime.FromOADate(Convert.ToDouble((xlRange.Cells[rCnt, 7] as Excel.Range).Value2));
                    newrow["PLRDExp_Date"] = common.ChangeDateToSqlFormat(Exp_Date);

                    ExcelData.Rows.Add(newrow);
                }

                int result = BalObj.insertIntoTemp(ExcelData, 2);
                if (result > 0)
                {
                    result = BalObj.ProcessTempData2();
                    if (result > 0)
                    {
                        return jsnsucess;
                    }
                    else
                    {
                        return jsnFailed;
                    }
                }
                else
                {
                    return jsnFailed;
                }
            }
            catch (Exception ex)
            {
                return jsnFailed;
            }
        }

    }
}
