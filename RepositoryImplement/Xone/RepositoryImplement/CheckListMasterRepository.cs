using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.Xone;
using RepositoryImplement.Xone.RepositoryDerive;
using DbContexts.Xone;

namespace RepositoryImplement.Xone.RepositoryImplement
{
    public class CheckListMasterRepository : ICheckListMaster
    {
        XoneContext db = new XoneContext();

        public IEnumerable<TblCheckListTypes> getListDetails()
        {
            return db.TblCheckListTypes.Where(m => m.ChkListStatus == true).ToList();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}