﻿    using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.Xone;
using RepositoryImplement.Xone.RepositoryDerive;
using DbContexts.Xone;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace RepositoryImplement.Xone.RepositoryImplement
{
    public class CitizenMasterRepository : ICitizenMaster

    {
        private XoneContext db = new XoneContext();
        private DapperLayer DapperObj = new DapperLayer();

        public IEnumerable<TblCitizenDetails> CitizenDetails()
        {
            return db.TblCitizenDetails.Where(m => m.IsStatus == true).ToList();

        }

        public int CreateCitizen(TblCitizenDetails obj)
        {
            try
            {
                var count = db.TblCitizenDetails.Where(m => (m.CitizenName == obj.CitizenName || m.CitizenCode == obj.CitizenCode) && m.IsStatus == true).Count();
                if (count == 0)
                {
                    obj.ModifiedDate = DateTime.Now;
                    obj.IsStatus = true;
                    db.TblCitizenDetails.Add(obj);
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

        public TblCitizenDetails GetDetailsForEdit(Int16 CitizenID)
        {
            return db.TblCitizenDetails.Where(m => m.CitizenID == CitizenID).FirstOrDefault();
        }

        public int EditCitizenDetails(TblCitizenDetails obj)
        {
            try
            {
                var count = db.TblCitizenDetails.Where(m => (m.CitizenName == obj.CitizenName || m.CitizenCode == obj.CitizenCode) && m.IsStatus == true && m.CitizenID != obj.CitizenID).Count();
                if (count == 0)
                {
                    TblCitizenDetails citizen = new TblCitizenDetails();
                    citizen = db.TblCitizenDetails.Find(obj.CitizenID);
                    citizen.CitizenName = obj.CitizenName;
                    citizen.CitizenDesc = obj.CitizenDesc;
                    citizen.CitizenCode = obj.CitizenCode;
                    citizen.ModifiedDate = DateTime.Now;
                    db.Entry(citizen).State = EntityState.Modified;
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

        public string GetCitizenName(Int16 CitizenID)
        {
            return db.TblCitizenDetails.Where(m => m.CitizenID == CitizenID).Select(m => m.CitizenName).SingleOrDefault();
        }

        public int DeleteCitizen(Int16 CitizenID)
        {
            try
            {
                TblCitizenDetails citizen = new TblCitizenDetails();
                citizen = db.TblCitizenDetails.Find(CitizenID);
                citizen.IsStatus = false;
                db.Entry(citizen).State = EntityState.Modified;
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
            //throw new NotImplementedException();
            db.Dispose();
        }
    }
}