using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.Xone;
using RepositoryImplement.Xone.RepositoryDerive;
using DbContexts.Xone;
using System.Data.Entity;

namespace RepositoryImplement.Xone.RepositoryImplement
{
    public class DesigDocTypeMasterRepository : IDesigDocTypeMaster
    {
        XoneContext db = new XoneContext();
        DapperLayer dpprObj = new DapperLayer();

        public IEnumerable<CandidateDocTypeDetail> ListDetails()
        {
            return dpprObj.DapperToList<CandidateDocTypeDetail>("select a.ValidDocTypID,b.CitizenName,c.DesignationName,d.DocSubtypeName from TblValidDoctypeMasters as a join TblCitizenDetails as b on a.CitizenID=b.CitizenID " +
                                                           " join TblDesignations as c on a.DesignationID=c.DesignationID join TblDocumentSubTypes as d on a.DocSubtypeID=d.DocSubtypeID " +
                                                           "  where b.IsStatus=1 and a.DocStatus=1 and d.DocSubtypeStatus=1");

        }
        public IEnumerable<TblCitizenDetails> GetCitizen()
        {
            return db.TblCitizenDetails.Where(m => m.IsStatus == true).ToList();
        }
        public IEnumerable<TblDesignation> GetDesignmation()
        {
            return db.TblDesignation.Where(m => m.DesigStatus == true).ToList();
        }
        public IEnumerable<TblDocumentTypes> GetDocType()
        {
            return db.TblDocumentTypes.Where(m => m.DocTypeStatus == true).ToList();
        }
        public IEnumerable<TblDocumentSubTypes> GetDocSubType(int DocTypeID)
        {
            return db.TblDocumentSubTypes.Where(m => m.DocTypeID == DocTypeID && m.DocSubtypeStatus == true);
        }

        public int AddNewDocTypes(TblValidDoctypeMaster validobj, Int64 UID)
        {
            try
            {
                var count = db.TblValidDoctypeMaster.Where(m => m.CitizenID == validobj.CitizenID && m.DesignationID == validobj.DesignationID && m.DocSubtypeID == validobj.DocSubtypeID && m.DocStatus == true).Count();
                if (count == 0)
                {
                    validobj.DocStatus = true;
                    validobj.CreatedBy = UID;
                    validobj.CreatedDate = DateTimeOffset.Now;
                    db.TblValidDoctypeMaster.Add(validobj);
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

        public int DeleteDocType(int ValidDocTypID,Int64 UID)
        {
            try
            {
                TblValidDoctypeMaster docobj = new TblValidDoctypeMaster();
                docobj = db.TblValidDoctypeMaster.Find(ValidDocTypID);
                docobj.DocStatus = false;
                docobj.LastUpdatedBy = UID;
                docobj.LastUpdatedDate = DateTimeOffset.Now;
                db.Entry(docobj).State = EntityState.Modified;
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