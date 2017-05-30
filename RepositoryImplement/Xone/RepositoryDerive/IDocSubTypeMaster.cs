using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.Xone;

namespace RepositoryImplement.Xone.RepositoryDerive
{
    public interface IDocSubTypeMaster:IDisposable
    {
        IEnumerable<DocumentSubTypeList> ListDocumentDetails();
        IEnumerable<TblDocumentTypes> GetDocTypes();
        int CreateDocumentSubType(TblDocumentSubTypes SubTypes,Int64 UID);
        TblDocumentSubTypes GetDetailsForEdit(Int32 DocSubtypeID);
        int EditDocumentSubType(TblDocumentSubTypes SubTypes, Int64 UID);
        bool CheckDeletableStatus(Int32 DocSubtypeID);
        string GetDocumentName(Int32 DocSubtypeID);
        int DeleteDocType(Int32 DocSubtypeID, Int64 UID);

    }
}  