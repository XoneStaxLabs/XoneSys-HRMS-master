using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using XoneHR.Models;

namespace XoneHR.Controllers
{
    public class EmployeesListController : Controller
    {
        EmployeesListBal empobj = new EmployeesListBal();

        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult ListEmployee(Int64 year, int month)
        //{           

        //    var salarylist = empobj.GetSalarySlip(year, month);           
        //    return View(salarylist);
        //}

        public ActionResult ExportDetails(Int64 year, int month)
        {
            try
            {
                //StringBuilder sb = new StringBuilder();
                StringWriter sw = new StringWriter();
                string filepath = "~/Docs";
                var salarylist = empobj.GetSalarySlip(year, month);

                //EmployeeSalaryDetails master = new EmployeeSalaryDetails();
                //master.Employees = empobj.GetEmployeeDetails(year, month);
                //master.Attendance = empobj.GetAttendance(year, month);
                //master.UpLeaves = empobj.GetUPLeaves(year, month);
                //master.CommonFields = empobj.GetCommonFields(year, month);
                
                //DataTable Dt = new DataTable();
                //Dt.Columns.Add("EmpName");
                //Dt.Columns.Add("NRICno");
                //Dt.Columns.Add("DOB");
                //Dt.Columns.Add("Age");
                //Dt.Columns.Add("BasicPay");
                //Dt.Columns.Add("PH_payment");
                //Dt.Columns.Add("OT_payment");
                //Dt.Columns.Add("GrossPay");
                //Dt.Columns.Add("CPF_employer");
                //Dt.Columns.Add("CPF_employee");
                //Dt.Columns.Add("Emp_levy");
                //Dt.Columns.Add("FundType");
                //Dt.Columns.Add("FundAmnt");
                //Dt.Columns.Add("NettPay");
                //Dt.Columns.Add("TotalAdvance");
                //Dt.Columns.Add("FinalPay");

                //foreach (var emps in master.Employees)
                //{
                //    DataRow Dr = Dt.NewRow();
                //    float BasicPay, PH_payment, OT_payment, Grosspay, CPF_employee, CPF_employer, FundAmnt, NetSalary, TotalAdvance, FinalPay, MBMF, SINDA, CDAC, ECF, RatePerHourPay, OT_hour, LeaveDays, UnPaidLeavePay, TotalContribution, TotalDeduction, TotalAllowance;
                //    string FundType;
                //    DateTime TimeOut1, TimeIn1;
                //    BasicPay = emps.BasicSalary;
                //    foreach(var attnds in master.Attendance)
                //    {
                       
                //    }
                //}

                ListtoDataTableConverter converter = new ListtoDataTableConverter();
                DataTable dt = converter.ToDataTable(salarylist);

                sw.WriteLine("\"NAME\",\"NRIC/FIN NO\",\"DATE OF BIRTH\",\"AGE\",\"BASIC SALARY\",\"PH/REST DAY\",\"OT SALARY\",\"GROSS SALARY\",\"CPF (YER)\",\"CPF (YEE)\",\"LEVY\",\"FUND\",\"AMT\",\"NET SALARY\",\"TOTAL ADVANCE\",\"GIRO / CHQ PAYMENT\"");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var rowData = dt.Rows[i];
                    sw.WriteLine( string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\",\"{9}\",\"{10}\",\"{11}\",\"{12}\",\"{13}\",\"{14}\",\"{15}\"",
                    rowData.ItemArray[0].ToString(), rowData.ItemArray[1].ToString(), rowData.ItemArray[2].ToString(), rowData.ItemArray[3].ToString(), rowData.ItemArray[4].ToString(), rowData.ItemArray[5].ToString(),
                    rowData.ItemArray[6].ToString(), rowData.ItemArray[7].ToString(), rowData.ItemArray[8].ToString(), rowData.ItemArray[9].ToString(),
                    rowData.ItemArray[10].ToString(), rowData.ItemArray[11].ToString(), rowData.ItemArray[12].ToString(), rowData.ItemArray[13].ToString(),
                    rowData.ItemArray[14].ToString(), rowData.ItemArray[15].ToString(),
                    Environment.NewLine));
                    //sb.Append(NewData);
                }

                string xlfile = DateTime.Now.Millisecond + "SalaryDetails.csv";

                if (!Directory.Exists(Server.MapPath(filepath)))
                {
                    Directory.CreateDirectory(Server.MapPath(filepath));
                }


                string Excelpath = "~/Docs/" + xlfile;
                //string strPath = Server.MapPath(filepath + xlfile);
                string strPath = Path.Combine(Server.MapPath(Excelpath));
                //if (System.IO.File.Exists(strPath))
                //{
                //    System.IO.File.Delete(strPath);
                //}

                var myFile = System.IO.File.Create(strPath);
                myFile.Close();
                using (FileStream fs = new FileStream(strPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    System.IO.File.WriteAllText(strPath, sw.ToString());


                //Response.ContentType = "image/jpg";
                Response.AddHeader("Content-Disposition", "attachment;filename=\"" + Excelpath + "\"");
                Response.TransmitFile(Server.MapPath(Excelpath));
                Response.End();



                return File(strPath, "application/csv");

              
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
 