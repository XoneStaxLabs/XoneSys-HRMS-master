using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.Xone;
using DbContexts.Xone;
using RepositoryImplement.Xone.RepositoryDerive;
using Dapper;
using System.Data.Entity;

namespace RepositoryImplement.Xone.RepositoryImplement
{
    public class DepartmentMasterRepository : IDepartmentMaster
    {
        XoneContext db = new XoneContext();
        DapperLayer DpprObj = new DapperLayer();


        public IEnumerable<DepartmentList> DepartmentList()
        {
            DynamicParameters param = new DynamicParameters();
            var list = DpprObj.DapperToList<DepartmentList>("Select a.DeptID,a.DeptCode,a.DeptName,b.UserType from TblDepartments a join TblUserTypes b on a.UserTypeId=b.UserTypeId where a.DeptStatus=1", param);
            return list;
        }

        public IEnumerable<TblUserType> GetUserType()
        {
            return db.TblUserType.Where(m => m.UserTypeStatus == true).ToList();
        }

        public int CreateDepartment(TblDepartment department, Int64 UID)
        {
            try
            {
                var count = db.TblDepartment.Where(m => m.DeptName == department.DeptName && m.UserTypeId == department.UserTypeId && m.DeptStatus == true).Count();
                if (count == 0)
                {
                    department.DeptStatus = true;
                    department.CreatedBy = UID;
                    department.CreatedDate = DateTimeOffset.Now;
                    db.TblDepartment.Add(department);
                    db.SaveChanges();
                    return 1;
                }
                else
                    return 0;
            }
            catch(Exception ex)
            {
                return -1;
            }
            
        }

        public int EditDeparmentDetails(TblDepartment department, Int64 UID)
        {
            try
            {
                var count = db.TblDepartment.Where(m => m.DeptName == department.DeptName && m.DeptID != department.DeptID && m.UserTypeId == department.UserTypeId && m.DeptStatus == true).Count();
                if (count == 0)
                {
                    TblDepartment deptObj = new TblDepartment();
                    deptObj = db.TblDepartment.Find(department.DeptID);
                    deptObj.DeptName = department.DeptName;
                    deptObj.DeptCode = department.DeptCode;
                    deptObj.UserTypeId = department.UserTypeId;
                    deptObj.LastUpdatedBy = UID;
                    deptObj.LastUpdatedDate = DateTimeOffset.Now;
                    db.Entry(deptObj).State = EntityState.Modified;
                    db.SaveChanges();
                    return 1;
                }
                else
                    return 0;
            }
            catch(Exception ex)
            {
                return -1;
            }
        }

        public TblDepartment GetDeptEditDetails(Int32 DeptID)
        {
            return db.TblDepartment.Where(m => m.DeptID == DeptID).FirstOrDefault();
        }

        public string GetDepartmentname(Int32 DeptID)
        {
            return db.TblDepartment.Where(m => m.DeptID == DeptID).Select(m => m.DeptName).SingleOrDefault();
        }

        public int DeleteDepartment(Int32 DeptID, Int64 UID)
        {
            try
            {
                TblDepartment dept = new TblDepartment();
                dept = db.TblDepartment.Find(DeptID);
                dept.DeptStatus = false;
                dept.LastUpdatedBy = UID;
                dept.LastUpdatedDate = DateTimeOffset.Now;
                db.Entry(dept).State = EntityState.Modified;
                db.SaveChanges();
                return 1;
            }
            catch(Exception ex)
            {
                return -1;
            }
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}