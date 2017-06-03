using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.Xone;

namespace RepositoryImplement.Xone.RepositoryDerive
{
    public interface ICheckListMaster
    {
        IEnumerable<TblCheckListTypes> getListDetails();
    }
}