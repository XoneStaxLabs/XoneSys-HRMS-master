using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RepositoryImplement.Xone.RepositoryDerive;
using Model.Xone;
using DbContexts.Xone;
using Dapper;
using System.Data.Entity;

namespace RepositoryImplement.Xone.RepositoryImplement
{
    public class DocSubTypeRepository:IDocSubTypeMaster
    {
        XoneContext db = new XoneContext();
        DapperLayer DpprObj = new DapperLayer();

        public IEnumerable<DocumentSubTypeList> ListDocumentDetails()
        {
            List<DocumentSubTypeList> list = new List<DocumentSubTypeList>();
            DynamicParameters para = new DynamicParameters();
            list = DpprObj.DapperToList<DocumentSubTypeList>("Select a.DocSubtypeID,a.DocTypeID,a.DocSubtypeName,b.DocTypeName from TblDocumentSubTypes a join TblDocumentTypes b on a.DocTypeID=b.DocTypeID where a.DocSubtypeStatus = 1", para);
            return list;
        }

        public IEnumerable<TblDocumentTypes> GetDocTypes()
        {
            return db.TblDocumentTypes.Where(m => m.DocTypeStatus == true).ToList();
        }

        public int CreateDocumentSubType(TblDocumentSubTypes SubTypes,Int64 UID)
        {
            try
            {
                var count = db.TblDocumentSubTypes.Where(m => m.DocSubtypeName == SubTypes.DocSubtypeName &&  m.DocSubtypeStatus == true && m.DocTypeID == SubTypes.DocTypeID).Count();
                if(count == 0)
                {
                    SubTypes.DocSubtypeStatus = true;
                    SubTypes.CreatedBy = UID;
                    SubTypes.CreatedDate = DateTimeOffset.Now;
                    db.TblDocumentSubTypes.Add(SubTypes);
                    db.SaveChanges();
                    return 1;
                }
                else
                { return 0; }
            }
            catch(Exception ex)
            {
                return -1;
            }
        }

        public TblDocumentSubTypes GetDetailsForEdit(Int32 DocSubtypeID)
        {
            return db.TblDocumentSubTypes.Where(m => m.DocSubtypeID == DocSubtypeID).FirstOrDefault();
        }

        public int EditDocumentSubType(TblDocumentSubTypes SubTypes, Int64 UID)
        {
            try
            {
                var count = db.TblDocumentSubTypes.Where(m => m.DocSubtypeName == SubTypes.DocSubtypeName && m.DocSubtypeStatus == true && m.DocTypeID == SubTypes.DocTypeID && m.DocSubtypeID != SubTypes.DocSubtypeID).Count();
                if (count == 0)
                {
                    TblDocumentSubTypes TypeObj = new TblDocumentSubTypes();
                    TypeObj = db.TblDocumentSubTypes.Find(SubTypes.DocSubtypeID);
                    TypeObj.DocTypeID = SubTypes.DocTypeID;
                    TypeObj.DocSubtypeName = SubTypes.DocSubtypeName;
                    TypeObj.LastUpdatedBy = UID;
                    TypeObj.LastUpdatedDate = DateTimeOffset.Now;
                    db.Entry(TypeObj).State = EntityState.Modified;
                    db.SaveChanges();
                    return 1;
                }
                else
                    return 0;
            }
            catch(Exception ex)
            { return -1; }
                        
        }

        public bool CheckDeletableStatus(Int32 DocSubtypeID)
        {
            var count = db.TblCandidateDocuments.Where(m => m.DocStypID == DocSubtypeID).Count();
            if (count == 0)
                return true;
            else
                return false;
        }

        public string GetDocumentName(Int32 DocSubtypeID)
        {
            return db.TblDocumentSubTypes.Where(m => m.DocSubtypeID == DocSubtypeID).Select(m => m.DocSubtypeName).SingleOrDefault();
        }

        public int DeleteDocType(Int32 DocSubtypeID, Int64 UID)
        {
            try
            {
                TblDocumentSubTypes docsubtype = new TblDocumentSubTypes();
                docsubtype = db.TblDocumentSubTypes.Find(DocSubtypeID);
                docsubtype.DocSubtypeStatus = false;
                docsubtype.LastUpdatedBy = UID;
                docsubtype.LastUpdatedDate = DateTimeOffset.Now;
                db.Entry(docsubtype).State = EntityState.Modified;
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