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
    public class RaceMasterRepository : IRaceMaster
    {
        private XoneContext db = new XoneContext();
        private DapperLayer DapperObj = new DapperLayer();

        public IEnumerable<TblRace> ListRaceDetails()
        {
            return db.TblRace.Where(m => m.RaceStatus == true).ToList();
        }

        public int CreateRace(TblRace RaceObj, Int64 UID)
        {
            try
            {
                var count = db.TblRace.Where(m => m.RaceName == RaceObj.RaceName && m.RaceStatus == true).Count();
                if (count == 0)
                {
                    RaceObj.CreatedBy = UID;
                    RaceObj.CreatedDate = DateTimeOffset.Now;
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

        public TblRace GetDetailsForEdit(Int16 RaceID)
        {
            return db.TblRace.Where(m => m.RaceID == RaceID).FirstOrDefault();
        }

        public int EditRaceDetails(TblRace RaceObj, Int64 UID)
        {
            try
            {
                var count = db.TblRace.Where(m => m.RaceName == RaceObj.RaceName && m.RaceID != RaceObj.RaceID).Count();
                if (count == 0)
                {
                    TblRace racedetails = new TblRace();
                    racedetails = db.TblRace.Find(RaceObj.RaceID);
                    racedetails.RaceName = RaceObj.RaceName;
                    racedetails.LastUpdatedBy = UID;
                    racedetails.LastUpdatedDate = DateTimeOffset.Now;
                    db.Entry(racedetails).State = EntityState.Modified;
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

        public bool CheckRaceDeletableStatus(Int16 RaceID)
        {//Candidate is active not checked
            var count = db.TblCandidate.Where(m => m.RaceID == RaceID).Count();
            if (count == 0)
                return true;
            else
                return false;
        }

        public string GetRaceName(Int16 RaceID)
        {
            return db.TblRace.Where(m => m.RaceID == RaceID).Select(m => m.RaceName).SingleOrDefault();
        }

        public int DeleteRace(Int16 RaceID, Int64 UID)
        {
            try
            {
                TblRace race = new TblRace();
                race = db.TblRace.Find(RaceID);
                race.RaceStatus = false;
                race.LastUpdatedBy = UID;
                race.LastUpdatedDate = DateTimeOffset.Now;
                db.Entry(race).State = EntityState.Modified;
                db.SaveChanges();

                return 1;
            }
            catch (Exception ex)
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