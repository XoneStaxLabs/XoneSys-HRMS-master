using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.Xone;

namespace RepositoryImplement.Xone.RepositoryDerive
{
    public interface IDesigDocTypeMaster :IDisposable
    {
        IEnumerable<CandidateDocTypeDetail> ListDetails();
        IEnumerable<TblCitizenDetails> GetCitizen();
        IEnumerable<TblDesignation> GetDesignmation();
        IEnumerable<TblDocumentTypes> GetDocType();
        IEnumerable<TblDocumentSubTypes> GetDocSubType(int DocTypeID);
        int AddNewDocTypes(TblValidDoctypeMaster validobj, Int64 UID);
        int DeleteDocType(int ValidDocTypID,Int64 UID);

    }
}