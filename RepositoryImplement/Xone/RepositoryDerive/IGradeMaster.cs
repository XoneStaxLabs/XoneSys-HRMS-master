using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.Xone;
using RepositoryImplement.Xone.RepositoryDerive;

namespace RepositoryImplement.Xone.RepositoryDerive
{
    public interface IGradeMaster:IDisposable
    {
        IEnumerable<GradeList> ListGradeDetails();
        IEnumerable<TblDepartment> GetDepartment();
        List<TblDesignation> GetDesignation(Int32 DeptID);
        /*int AddNewGrade(int DesignationID, string Gradename, string GradeCode, Int64 UID);*/
    }
}