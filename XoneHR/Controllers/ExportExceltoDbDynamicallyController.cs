using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Excel = Microsoft.Office.Interop.Excel;
using XoneHR.Models.BAL;
using XoneHR.Models;
using System.Data;
using System.IO;

namespace XoneHR.Controllers
{
    public class ExportExceltoDbDynamicallyController : Controller
    {
        ExportExcelDbBAL BalObj = new ExportExcelDbBAL();
        CommonFunctions common = new CommonFunctions();

        private XoneDbLayer db;
        public ExportExceltoDbDynamicallyController()
        {
            db = new XoneDbLayer();
        }
        public ActionResult Index()
        {
            return View();
        }

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
                int ColumnCount = xlRange.Columns.Count;
                int rCnt = 0;

                DataTable ExcelData = new DataTable();
                for(int i=1; i < ColumnCount; i++)
                {
                    ExcelData.Columns.Add("" + Convert.ToString((xlRange.Cells[1, i] as Excel.Range).Value2) + "", typeof(string));
                }               

                DataRow newrow;
                for (rCnt = 2; rCnt <= xlRange.Rows.Count - 1; rCnt++)
                {
                    newrow = ExcelData.NewRow();
                    for (int i = 1; i < ColumnCount; i++)
                    {
                        newrow[""+ Convert.ToString((xlRange.Cells[1, i] as Excel.Range).Value2) + ""] = Convert.ToString((xlRange.Cells[rCnt, i] as Excel.Range).Value2);
                    }
                    //DateTime DATE_OF_BIRTH = DateTime.FromOADate(Convert.ToDouble((xlRange.Cells[rCnt,9] as Excel.Range).Value2));                    
                    ExcelData.Rows.Add(newrow);
                }

                int result = BalObj.insertIntoTable(ExcelData);
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

    }
}
