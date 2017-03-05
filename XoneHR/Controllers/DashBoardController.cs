using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using XoneHR.Models;
using XoneHR.Models.BAL;

namespace XoneHR.Controllers
{
    public class DashBoardController : Controller
    {

        private DashBoardBal dashboardObj;

        public  DashBoardController()
        {
            dashboardObj = new DashBoardBal();
        }
        public ActionResult DashBoard()
        {
            var results = dashboardObj.DashBordUserCounts();
            return View(results);
        }

       

        public ActionResult DashBoardusers()
        {
            StringBuilder sb = new StringBuilder();


            var Results =dashboardObj.DashBoardusers();
            foreach (var items in Results)
            {
                string path = "/Employee/EmployeeProfile?EmpId=" + items.EmpID;

                sb.Append("<li>");
                if (items.CandPhoto != "" && items.CandPhoto != null )
                    sb.Append("<img src=" + items.CandPhoto + " alt='User Image'>");
                //sb.Append("<li><img src=" + items.CandPhoto + " alt='User Image'>");
                sb.Append("<a class='users-list-name' href=" + path + ">" + items.CandName + "</a><span class='users-list-date'>" + items.EmpStartDate.ToShortDateString() + "</span> </li>");

            }

            return Content(sb.ToString());
        }

        public ActionResult AbsenceAllocation()
        {
            StringBuilder sb = new StringBuilder();
            var result = dashboardObj.AbsenceAllocation();

            foreach (var item in result)
            {//<td>" + item.Schedule + "</td>          <td>" + item.ShiftName + "</td>
                sb.Append("<tr><td>" + item.ProjName + "</td> <td>" + item.AbsDateFrom.ToShortDateString() + "</td> <td><span class='label label-danger'>" + item.AbsentEmployee + "</span></td><td><span class='label label-success'>" + item.AllocateEmployee + "</span></td></tr>");  //<td>" + item.AbsDateTo.ToShortDateString() + "</td> 
            }

            return Content(sb.ToString());
        }

        public ActionResult Listallmemos()
        {
            StringBuilder sb = new StringBuilder();
            var results = dashboardObj.ListMemos(SessionManage.Current.AppUID);
           

            foreach (var items in results)
            {
                
                 var Docsresults =dashboardObj.ListMemosDocuments(items.MemoID);

                string subject = items.MemoSubject + "-Reply";

                sb.Append("<div class='item'><img src=" + items.AppPhoto + " alt='user image' class='online'>");
                sb.Append("<p class='message'><a href='#' class='name'><small class='text-muted pull-right'><i class='fa fa-clock-o'></i> " + items.MemoAddDate.ToLongDateString() + "</small>" + items.AppFirstName + " (" + items.DesigName + ")<br />Subject: " + items.MemoSubject + "</a>" + items.MemoText + "</p>");
                foreach (var docs in Docsresults)
                {
                    sb.Append("<div class='attachment'><h4>Attachments:</h4><p class='filename'>"+docs.MemoDocument+"</p><div class='pull-right'><button class='btn btn-primary btn-sm btn-flat'>Open</button></div></div><a href='#' class='relpymemos' data-memoid=" + items.MemoID + " data-reply=" + items.MemoAddedby + " data-Subject=" + subject + "><i class='fa fa-mail-reply'></i></a></div>");

                }

            }

            return Content(sb.ToString());

        }

        public ActionResult AddReplyMemo(string subject, string senduid, string memotext,string memorepid)
        {
            TblMemos memo = new TblMemos();
            memo.MemoSubject = subject;
            memo.MemoText = memotext;
            memo.MemoPriority = 1;
            memo.MemoRepID = Convert.ToInt32(memorepid);

            var result = dashboardObj.AddnewmemosReply(memo, Convert.ToInt32(senduid));
            if (result == 1)
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }


            
        }

        public ActionResult GetPassportExpiryList()
        {
            var results = dashboardObj.PassportExpiryListsTopFive();
            return View(results);
        }

        public ActionResult GetPLRDExpiryList()
        {
            var results = dashboardObj.PLRDExpiryListsTopFive();
            return View(results);
        }


        public ActionResult PassportExpiryList()
        {
            var results = dashboardObj.PassportExpiryLists();
            return View(results);
        }

        public ActionResult PLRDExpiryList()
        {
            var results = dashboardObj.PLRDExpiryLists();
            return View(results);
        }
        public ActionResult LeaveDetails()
        {
            StringBuilder sb = new StringBuilder();
            var results = dashboardObj.ListLeaveDetails();


            foreach (var items in results)
            {
                

                sb.Append("<tr><td>" + items.CandName + "</td><td>" + items.LeaveType + "</td><td><span class='label label-danger'>Pending</span></td></tr>");

            
            }

            return Content(sb.ToString());
        }

        public JsonResult GetCalendarEvents()
        {
            var List = dashboardObj.HolidayList();
            if (List != null)
            {
                var eventList = from e in List
                                select new
                                {
                                    id = e.HolidayID,
                                    title = e.HoliText,
                                    start = e.HoliDate.ToString(),
                                    end = e.HoliDate.ToString(),
                                    color = "#FF8000",
                                    allDay = false
                                };
                var rows = eventList.ToArray();
                return Json(rows, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
