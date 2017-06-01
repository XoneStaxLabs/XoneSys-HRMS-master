using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.Xone;
using RepositoryImplement.Xone.RepositoryDerive; 

namespace RepositoryImplement.Xone.RepositoryDerive
{
    public interface IDeductionTypeMaster : IDisposable
    {

        IEnumerable<TblDeductionType> ListDeductionTypes();
        int AddNewDeductionType(TblDeductionType deudcts, Int64 UID);
        TblDeductionType GetDetailForEdit(Int32 DeductTypeID);
        int EditDeductionType(TblDeductionType deudcts, Int64 UID);
        string GetDeductionTypeText(Int32 DeductTypeID);
        int DeleteDeductionType(Int32 DeductTypeID, Int64 UID);

    }
}