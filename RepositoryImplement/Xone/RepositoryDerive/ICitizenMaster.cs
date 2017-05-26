using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.Xone;

namespace RepositoryImplement.Xone.RepositoryDerive
{
    public interface ICitizenMaster : IDisposable
    {
        IEnumerable<TblCitizenDetails> CitizenDetails();
        int CreateCitizen(TblCitizenDetails obj,Int64 UID);
        TblCitizenDetails GetDetailsForEdit(Int16 CitizenID);
        int EditCitizenDetails(TblCitizenDetails obj, Int64 UID);
        string GetCitizenName(Int16 CitizenID);
        int DeleteCitizen(Int16 CitizenID,Int64 UID);
        bool CheckCitizenDeleteAvailability(Int64 CitizenID);

    } 
}