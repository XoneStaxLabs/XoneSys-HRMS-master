using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.Xone;

namespace RepositoryImplement.Xone.RepositoryDerive
{
    public interface IHolidayMaster : IDisposable
    {
        IEnumerable<HolidayList> ListHolidays();
        int AddHolidays(TblHolidayList Holilist, Int64 UID);
        string GetHolidatyText(Int32 HolidayID);
        int DeleteHoliday(Int32 HolidayID, Int64 UID);

    }
}