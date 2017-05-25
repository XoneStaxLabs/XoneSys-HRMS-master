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
        int CreateCitizen(TblCitizenDetails obj);
        TblCitizenDetails GetDetailsForEdit(Int16 CitizenID);
        int EditCitizenDetails(TblCitizenDetails obj);
        string GetCitizenName(Int16 CitizenID);
        int DeleteCitizen(Int16 CitizenID);
        bool CheckCitizenDeleteAvailability(Int64 CitizenID);

    } 
}