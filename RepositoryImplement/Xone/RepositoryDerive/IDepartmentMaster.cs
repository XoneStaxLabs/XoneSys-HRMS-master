using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.Xone;


namespace RepositoryImplement.Xone.RepositoryDerive
{
    public interface IDepartmentMaster
    {
        IEnumerable<DepartmentList> DepartmentList();

        IEnumerable<TblUserType> GetUserType();
        int CreateDepartment(TblDepartment department, Int64 UID);
        TblDepartment GetDeptEditDetails(Int32 DeptID);
        int EditDeparmentDetails(TblDepartment department, Int64 UID);
        string GetDepartmentname(Int32 DeptID);
        int DeleteDepartment(Int32 DeptID, Int64 UID);
    }
}