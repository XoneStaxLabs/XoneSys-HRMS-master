using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DbContexts.Xone;
using RepositoryImplement.Xone.RepositoryDerive;

namespace RepositoryImplement.Xone.RepositoryImplement
{
    public class GradeMasterRepository:IGradeMaster
    {
        XoneContext db = new XoneContext();
        DapperLayer DpprObj = new DapperLayer();



        public void Dispose()
        {
            db.Dispose();
        }
    }
}