using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.Xone;

namespace RepositoryImplement.Xone.RepositoryDerive
{
    public interface IDocumentTypeMaster
    {
        IEnumerable<TblDocumentTypes> ListDocumentDetails();
        int CreateDocumentType(TblDocumentTypes DocObj, Int64 UID);
        TblDocumentTypes GetDocEditDetails(Int32 DocTypeID);
        int EditDocDetails(TblDocumentTypes DocObj, Int64 UID);
        //bool CheckDeletableStatus(Int32 DocTypeID);
        string GetDocumentName(Int32 DocTypeID);
        int DeleteDocType(Int32 DocTypeID, Int64 UID);
    }
}