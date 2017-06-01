using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.Xone;
using DbContexts.Xone;
using RepositoryImplement.Xone.RepositoryDerive;
using System.Data.Entity;

namespace RepositoryImplement.Xone.RepositoryImplement
{
    public class HolidayMasterRepository : IHolidayMaster
    {
        private XoneContext db = new XoneContext();
        private DapperLayer DapperObj = new DapperLayer();

        public IEnumerable<HolidayList> ListHolidays()
        {
            var list = db.TblHolidayList.Where(m => m.HoliStatus == true).ToList();
            var datas = from a in list select new HolidayList { HolidayID = a.HolidayID, HolidayDate = a.HolidayDate.DateTime, Holiday = a.Holiday, Description = a.Description };
            return datas;
        }

        public int AddHolidays(TblHolidayList Holilist, Int64 UID)
        {
            try
            {
                var count = db.TblHolidayList.Where(m => m.Holiday == Holilist.Holiday && m.HoliStatus == true).Count();
                if (count == 0)
                {
                    Holilist.HoliStatus = true;
                    Holilist.ModifiedBy = UID;
                    Holilist.ModifiedDate = DateTimeOffset.Now;
                    db.TblHolidayList.Add(Holilist);
                    db.SaveChanges();
                    return 1;
                }
                else
                    return 0;
            }
            catch(Exception ex)
            {
                return -1;
            }
            
        }

        public string GetHolidatyText(Int32 HolidayID)
        {
            return db.TblHolidayList.Where(m => m.HolidayID == HolidayID).Select(m => m.Holiday).SingleOrDefault();
        }

        public int DeleteHoliday(Int32 HolidayID, Int64 UID)
        {
            try
            {
                TblHolidayList list = new TblHolidayList();
                list = db.TblHolidayList.Find(HolidayID);
                list.HoliStatus = false;
                list.ModifiedBy = UID;
                list.ModifiedDate= DateTimeOffset.Now;
                db.Entry(list).State = EntityState.Modified;
                db.SaveChanges();
                return 1;
            }
            catch(Exception ex)
            {
                return -1;
            }
            
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}