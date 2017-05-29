using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RepositoryImplement.Xone.RepositoryDerive;
using Model.Xone;
using DbContexts.Xone;
using Dapper;

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

        public void Dispose()
        {
            db.Dispose();
        }
    }
}