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
        List<TblDesignation> GetDesignation(Int32 DeptID=0);
        int AddNewGrade(int DesignationID, string Gradename, string GradeCode, Int64 UID);
        GradeList GetDetailsForEdit(int GradeID,int GradeDesignationId);
        int EditGrade(GradeList editObj, Int64 UID);
        string GetGradeName(int GradeID);
        int DeleteGrade(int GradeID, int GradeDesignationId,Int64 UID);

    }
}