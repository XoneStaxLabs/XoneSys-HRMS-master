using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using XoneHR.Models;

namespace XoneHR.Models
{
    public class CommonFunctions
    {
        CommonBalLayer CommonBal=new CommonBalLayer(); 

        public static List<string> InvalidJsonElements;

        public static IList<T> DeserializeToList<T>(string jsonString)
        {
            InvalidJsonElements = null;
            var array = JArray.Parse(jsonString);
            IList<T> objectsList = new List<T>();
            foreach (var item in array)
            {
                try
                {
                    objectsList.Add(item.ToObject<T>());
                }
                catch
                {
                    InvalidJsonElements = InvalidJsonElements ?? new List<string>();
                    InvalidJsonElements.Add(item.ToString());
                }
            }
            return objectsList;
        }

        //public static DateTime DatetimeConvertion(string date1)
        //{
        //    //DateTime parsedDate = DateTime.ParseExact(date1.ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);

        //    //string strPassDate = "21/05/2016";
        //    DateTime dt1 = DateTime.ParseExact("07/13/2016", "MM/dd/yyyy", CultureInfo.InvariantCulture);
        //    //DateTime dt = DateTime.ParseExact(date1.ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        //    //string strDate = dt.ToString("yyyy-mm-dd");
        //    string dt = "07/13/2016";
        //    DateTime date = DateTime.ParseExact(dt, "MM/dd/yyyy", null);

        //    return date;
        //}

        public string ChangeDateToSqlFormat(DateTime dt)
        {
            DateTime dtNew;
            int dd = dt.Day;
            int MM = dt.Month;
            int yyyy = dt.Year;
            int hh = dt.Hour;
            int mm = dt.Minute;
            int ss = dt.Second;

            string strDate = yyyy + "/" + MM + "/" + dd + " " + hh + ":" + mm + ":" + ss;
            //  dtNew = Convert.ToDateTime(strDate);
            return strDate;
        }
        
        public DateTime CommonDateConvertion(string Dt)
        {
            var Dateformat = SessionManage.Current.Dateformat;


            var FormatItems = Dateformat.Split('/');
            var DateItems = Dt.Contains('/') ? Dt.Split('/') : Dt.Split('-');

            string dd = "";
            string MM = "";
            string yyyy = "";
            int value = 0;

            foreach (var item in FormatItems)
            {
                if (item == "DD")
                {
                    dd = DateItems[value].ToString();
                }
                else if (item == "MM")
                {
                    MM = DateItems[value].ToString();
                }
                else if (item == "YYYY")
                {
                    yyyy = DateItems[value].ToString();
                }
                value++;
            }


            var OutputDate = yyyy.ToString() + '/' + MM.ToString() + '/' + dd.ToString();
            DateTime OutputDt = Convert.ToDateTime(OutputDate);

            return OutputDt;

        }

        public List<Permission> GetPermissionList(string  ActionName,string  Controller)
        {
            var permi = CommonBal.GetPermissionList(ActionName, Controller);
            return permi;
        }

        public bool GetPermissionStatus(string ActionName, string Controller)
        {
            var status = CommonBal.GetPermissionStatus(ActionName, Controller);
            return status;
        }

        //public List<TblFunctionType> FunctionTypes()
        //{
        //    var Funtypes = CommonBal.GetFunctionTypes();
        //}

        //public void ActionLog(string TableName, ActionType Act_Type,string Act_Controller,Int64 Act_Key,Int64 Modifiedby)
        //{
        //    CommonBal.ActionLog(TableName, Act_Type, Act_Controller, Act_Key, Modifiedby);
        //}


    }
}