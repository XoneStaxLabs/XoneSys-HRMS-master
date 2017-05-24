using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.Xone;
using DbContexts.Xone;
using RepositoryImplement.Xone.RepositoryDerive;

namespace RepositoryImplement.Xone.RepositoryImplement
{
    public class RaceMasterRepository : IRaceMaster
    {
        private XoneContext db = new XoneContext();
        private DapperLayer DapperObj = new DapperLayer();

        public IEnumerable<TblRace> ListRaceDetails()
        {
            return db.TblRace.Where(m => m.RaceStatus == true).ToList();
        }

        public int CreateRace(TblRace RaceObj)
        {
            try
            {
                var count = db.TblRace.Where(m => m.RaceName == RaceObj.RaceName && m.RaceStatus == true).Count();
                if (count == 0)
                {
                    RaceObj.ModifiedDate = DateTime.Now;
                    RaceObj.RaceStatus = true;
                    db.TblRace.Add(RaceObj);
                    db.SaveChanges();
                    return 1;
                }
                else
                {
                    return 0;
                }
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