using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.Xone;
using DbContexts.Xone;
using RepositoryImplement.Xone.RepositoryDerive;
using Dapper;
using System.Transactions;

namespace RepositoryImplement.Xone.RepositoryImplement
{
    public class GradeMasterRepository:IGradeMaster
    {
        XoneContext db = new XoneContext();
        DapperLayer DpprObj = new DapperLayer();

        public IEnumerable<GradeList> ListGradeDetails()
        {
            DynamicParameters para = new DynamicParameters();
            return DpprObj.DapperToList<GradeList>("select a.Gradename,a.GradeCode,c.DesignationName from TblGrades a join TblGradeDesignationMappings b on a.GradeID=b.GradeID join TblDesignations c on b.DesignationID=c.DesignationID where b.MappingStatus=1");
        }

        public IEnumerable<TblDepartment> GetDepartment()
        {
            return db.TblDepartment.Where(m => m.DeptStatus == true).ToList();
        }

        public List<TblDesignation> GetDesignation(Int32 DeptID)
        {
            var Desigs = (from a in db.TblDesignation
                          join b in db.TblDeptDesignationMapping on a.DesignationID equals b.DesignationID
                          where b.DeptID == DeptID
                          select new
                          {
                              DesignationID = a.DesignationID,
                              DesignationName = a.DesignationName
                          }).ToList()
                          .Select(x => new TblDesignation()
                          {
                              DesignationID = x.DesignationID,
                              DesignationName = x.DesignationName
                          }).ToList();


            return Desigs;
        }

        //public int AddNewGrade(int DesignationID, string Gradename, string GradeCode, Int64 UID)
        //{
           
        //}

        public void Dispose()
        {
            db.Dispose();
        }
    }
}