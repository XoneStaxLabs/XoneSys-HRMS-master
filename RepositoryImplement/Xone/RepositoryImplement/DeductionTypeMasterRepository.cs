using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RepositoryImplement.Xone.RepositoryDerive;
using Model.Xone;
using DbContexts.Xone;

namespace RepositoryImplement.Xone.RepositoryImplement
{
    public class DeductionTypeMasterRepository : IDeductionTypeMaster
    {
        XoneContext db = new XoneContext();

        public IEnumerable<TblDeductionType> ListDeductionTypes()
        {
            return db.TblDeductionType.Where(m => m.DeductionStatus == true).ToList();
        }

        public int AddNewDeductionType(TblDeductionType deducts, Int64 UID)
        {
            try
            {
                var count = db.TblDeductionType.Where(m => m.DeductionType == deducts.DeductionType && m.DeductionStatus == true).Count();
                if (count == 0)
                {
                    deducts.DeductionStatus = true;
                    deducts.CreatedBy = UID;
                    deducts.CreatedDate = DateTimeOffset.Now;
                    db.TblDeductionType.Add(deducts);
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

        public TblDeductionType GetDetailForEdit(Int32 DeductTypeID)
        {
            return db.TblDeductionType.Where(m => m.DeductTypeID == DeductTypeID).FirstOrDefault();
        }

        public int EditDeductionType(TblDeductionType deducts, Int64 UID)
        {
            try
            {
                var count = db.TblDeductionType.Where(m => m.DeductionType == deducts.DeductionType && m.DeductTypeID != deducts.DeductTypeID && m.DeductionStatus == true).Count();
                if (count == 0)
                {
                    TblDeductionType typeobj = new TblDeductionType();
                    typeobj = db.TblDeductionType.Find(deducts.DeductTypeID);
                    typeobj.DeductionType = deducts.DeductionType;
                    typeobj.LastUpdatedBy = UID;
                    typeobj.LastUpdatedDate = DateTimeOffset.Now;
                    db.Entry(typeobj).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return 1;
                }
                else
                    return 0;
            }
            catch(Exception ex)
            { return -1; }
        }

        public string GetDeductionTypeText(Int32 DeductTypeID)
        {
            return db.TblDeductionType.Where(m => m.DeductTypeID == DeductTypeID).Select(m => m.DeductionType).SingleOrDefault();
        }

        public int DeleteDeductionType(Int32 DeductTypeID, Int64 UID)
        {
            try
            {
                TblDeductionType typ = new TblDeductionType();
                typ = db.TblDeductionType.Find(DeductTypeID);
                typ.DeductionStatus = false;
                typ.LastUpdatedBy = UID;
                typ.LastUpdatedDate = DateTimeOffset.Now;
                db.Entry(typ).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return 1;
            }
            catch(Exception ex)
            { return -1; }
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}