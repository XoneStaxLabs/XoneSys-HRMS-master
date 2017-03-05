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
                    // int strFilePath = obj_file.FolderManagement(LoginType.ToString(), GroupID, CompanyID, OFFID);
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

                //string str = "";
                int rCnt = 0;
                //int cCnt = 0;

                DataTable ExcelData = new DataTable();
                ExcelData.Columns.Add("name", typeof(string));
                ExcelData.Columns.Add("NRIC_IN_NO", typeof(string));
                ExcelData.Columns.Add("DATE_OF_BIRTH", typeof(string));
                ExcelData.Columns.Add("age", typeof(string));
                ExcelData.Columns.Add("BASIC_SALARY", typeof(string));
                
                ExcelData.Columns.Add("Nationality", typeof(string));
                ExcelData.Columns.Add("FundType", typeof(string));

                ExcelData.Columns.Add("ADDRESS", typeof(string));
                ExcelData.Columns.Add("mobile", typeof(string));
                ExcelData.Columns.Add("phone", typeof(string));
                ExcelData.Columns.Add("Bank", typeof(string));
                ExcelData.Columns.Add("Account", typeof(string));
                ExcelData.Columns.Add("BankCode", typeof(string));
                ExcelData.Columns.Add("BranchCode", typeof(string));
                DataRow newrow;
                for (rCnt = 2; rCnt <= xlRange.Rows.Count - 1; rCnt++)
                {
                    newrow = ExcelData.NewRow();
                    newrow["name"] = Convert.ToString((xlRange.Cells[rCnt, 2] as Excel.Range).Value2);
                    newrow["NRIC_IN_NO"] = Convert.ToString((xlRange.Cells[rCnt, 3] as Excel.Range).Value2);
                    newrow["DATE_OF_BIRTH"] = Convert.ToString((xlRange.Cells[rCnt, 4] as Excel.Range).Value2);
                    newrow["age"] = Convert.ToString((xlRange.Cells[rCnt, 5] as Excel.Range).Value2);
                    newrow["BASIC_SALARY"] = Convert.ToString((xlRange.Cells[rCnt, 6] as Excel.Range).Value2);

                    newrow["Nationality"] = Convert.ToString((xlRange.Cells[rCnt, 7] as Excel.Range).Value2);
                    newrow["FundType"] = Convert.ToString((xlRange.Cells[rCnt, 8] as Excel.Range).Value2);

                    newrow["ADDRESS"] = Convert.ToString((xlRange.Cells[rCnt, 9] as Excel.Range).Value2);
                    newrow["mobile"] = Convert.ToString((xlRange.Cells[rCnt, 10] as Excel.Range).Value2);
                    newrow["phone"] = Convert.ToString((xlRange.Cells[rCnt, 11] as Excel.Range).Value2);
                    newrow["Bank"] = Convert.ToString((xlRange.Cells[rCnt, 12] as Excel.Range).Value2);
                    newrow["Account"] = Convert.ToString((xlRange.Cells[rCnt, 13] as Excel.Range).Value2);
                    newrow["BankCode"] = Convert.ToString((xlRange.Cells[rCnt, 14] as Excel.Range).Value2);
                    newrow["BranchCode"] = Convert.ToString((xlRange.Cells[rCnt, 15] as Excel.Range).Value2);
                    ExcelData.Rows.Add(newrow);
                }


                int result = BalObj.insertIntoTemp(ExcelData, 1);
                if (result > 0)
                {
                    result = BalObj.ProcessTempData();
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
                    // int strFilePath = obj_file.FolderManagement(LoginType.ToString(), GroupID, CompanyID, OFFID);
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

                //string str = "";
                int rCnt = 0;
                //int cCnt = 0;

                DataTable ExcelData = new DataTable();
                ExcelData.Columns.Add("NRIC_IN_NO", typeof(string));
                ExcelData.Columns.Add("Join_Date", typeof(string));
                ExcelData.Columns.Add("Race", typeof(string));

                DataRow newrow;
                for (rCnt = 4; rCnt <= xlRange.Rows.Count - 1; rCnt++)
                {
                    newrow = ExcelData.NewRow();
                    newrow["NRIC_IN_NO"] = Convert.ToString((xlRange.Cells[rCnt, 3] as Excel.Range).Value2);
                    
                    newrow["Race"] = Convert.ToString((xlRange.Cells[rCnt, 10] as Excel.Range).Value2);
                    //var test=((xlRange.Cells[rCnt, 11] as Excel.Range).Value2);
                    //double d = Convert.ToDouble((xlRange.Cells[rCnt, 11] as Excel.Range).Value2);
                    DateTime dateValue = DateTime.FromOADate(Convert.ToDouble((xlRange.Cells[rCnt, 11] as Excel.Range).Value2));
                    if ((xlRange.Cells[rCnt, 11] as Excel.Range).Value2 != null)
                        newrow["Join_Date"] = common.ChangeDateToSqlFormat(dateValue); //ToShortDateString();
                    else
                        newrow["Join_Date"] = null;

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
