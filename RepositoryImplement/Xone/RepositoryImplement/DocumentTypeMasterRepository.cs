using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.Xone;
using DbContexts.Xone;
using RepositoryImplement.Xone.RepositoryDerive;
using System.Data.Entity;
using System.Transactions;

namespace RepositoryImplement.Xone.RepositoryImplement
{
    public class DocumentTypeMasterRepository:IDocumentTypeMaster
    {
        XoneContext db = new XoneContext();
        DapperLayer DpprObj = new DapperLayer();

        public IEnumerable<TblDocumentTypes> ListDocumentDetails()
        {
            return db.TblDocumentTypes.Where(m => m.DocTypeStatus == true).ToList();
        }

        public int CreateDocumentType(TblDocumentTypes DocObj, Int64 UID)
        {
            try
            {
                var count = db.TblDocumentTypes.Where(m => m.DocTypeName == DocObj.DocTypeName).Count();
                if (count == 0)
                {
                    DocObj.DocTypeStatus = true;
                    DocObj.CreatedBy = UID;
                    DocObj.CreatedDate = DateTimeOffset.Now;
                    db.TblDocumentTypes.Add(DocObj);
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

        public TblDocumentTypes GetDocEditDetails(Int32 DocTypeID)
        {
            return db.TblDocumentTypes.Where(m => m.DocTypeID == DocTypeID).FirstOrDefault();
        }

        public int EditDocDetails(TblDocumentTypes DocObj, Int64 UID)
        {
            try
            {
                var count = db.TblDocumentTypes.Where(m => m.DocTypeName == DocObj.DocTypeName && m.DocTypeID != DocObj.DocTypeID).Count();
                if (count == 0)
                {
                    TblDocumentTypes doctype = new TblDocumentTypes();
                    doctype = db.TblDocumentTypes.Find(DocObj.DocTypeID);
                    doctype.DocTypeName = DocObj.DocTypeName;
                    doctype.LastUpdatedBy = UID;
                    doctype.LastUpdatedDate = DateTimeOffset.Now;
                    db.Entry(doctype).State = EntityState.Modified;
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

        //public bool CheckDeletableStatus(Int32 DocTypeID)
        //{            
        //}

        public string GetDocumentName(Int32 DocTypeID)
        {
            return db.TblDocumentTypes.Where(m => m.DocTypeID == DocTypeID).Select(m => m.DocTypeName).SingleOrDefault();
        }

        public int DeleteDocType(Int32 DocTypeID, Int64 UID)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    TblDocumentTypes doctype = new TblDocumentTypes();
                    doctype = db.TblDocumentTypes.Find(DocTypeID);
                    doctype.DocTypeStatus = false;
                    doctype.LastUpdatedBy = UID;
                    doctype.LastUpdatedDate = DateTimeOffset.Now;
                    db.Entry(doctype).State = EntityState.Modified;
                    db.SaveChanges();
                                       
                    TblDocumentSubTypes docsubtype = new TblDocumentSubTypes();
                    var DocSubtypeID = db.TblDocumentSubTypes.Where(m => m.DocTypeID == DocTypeID).Select(m => m.DocSubtypeID).ToList();
                    foreach (var id in DocSubtypeID)
                    {
                        docsubtype = db.TblDocumentSubTypes.Find(id);
                        docsubtype.DocSubtypeStatus = false;
                        docsubtype.LastUpdatedBy = UID;
                        docsubtype.LastUpdatedDate = DateTimeOffset.Now;
                        db.Entry(docsubtype).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    return 1;
                }
                
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