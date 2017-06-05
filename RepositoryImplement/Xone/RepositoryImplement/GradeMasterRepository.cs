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
            return DpprObj.DapperToList<GradeList>("select a.GradeID,a.Gradename,a.GradeCode,c.DesignationName,e.DeptName,b.GradeDesignationId from TblGrades a join TblGradeDesignationMappings b on a.GradeID=b.GradeID join TblDesignations c on b.DesignationID=c.DesignationID " +
                " join TblDeptDesignationMappings d on d.DesignationID=c.DesignationID join TblDepartments e on d.DeptID=e.DeptID  where b.MappingStatus=1");
        }

        public IEnumerable<TblDepartment> GetDepartment()
        {
            return db.TblDepartment.Where(m => m.DeptStatus == true).ToList();
        }

        public List<TblDesignation> GetDesignation(Int32 DeptID=0)
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

        public int AddNewGrade(int DesignationID, string Gradename, string GradeCode, Int64 UID)
        {
            try
            {
                var count = db.TblGradeDesignationMapping.Where(m => m.DesignationID == DesignationID).Count();
                if (count > 0)
                {
                    var GrdChk = db.TblGrade.Where(m => m.Gradename == Gradename && m.GradeStatus == true).Count();
                    if (GrdChk == 0)
                    {
                        using (TransactionScope scope = new TransactionScope())
                        {
                            TblGrade grade = new TblGrade();
                            grade.Gradename = Gradename;
                            grade.GradeCode = GradeCode;
                            grade.GradeStatus = true;
                            grade.CreatedBy = UID;
                            grade.CreatedDate = DateTimeOffset.Now;
                            db.TblGrade.Add(grade);
                            db.SaveChanges();

                            TblGradeDesignationMapping mpping = new TblGradeDesignationMapping();
                            mpping.GradeID = grade.GradeID;
                            mpping.DesignationID = DesignationID;
                            mpping.CreatedBy = UID;
                            mpping.CreatedDate = DateTimeOffset.Now;
                            mpping.MappingStatus = true;
                            db.TblGradeDesignationMapping.Add(mpping);
                            db.SaveChanges();

                            scope.Complete();
                            scope.Dispose();
                            return 1;
                        }                           
                    }
                    else
                        return 0;
                }
                else
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        TblGrade grade = new TblGrade();
                        grade.Gradename = Gradename;
                        grade.GradeCode = GradeCode;
                        grade.GradeStatus = true;
                        grade.CreatedBy = UID;
                        grade.CreatedDate = DateTimeOffset.Now;
                        db.TblGrade.Add(grade);
                        db.SaveChanges();

                        TblGradeDesignationMapping mpping = new TblGradeDesignationMapping();
                        mpping.GradeID = grade.GradeID;
                        mpping.DesignationID = DesignationID;
                        mpping.CreatedBy = UID;
                        mpping.CreatedDate = DateTimeOffset.Now;
                        mpping.MappingStatus = true;
                        db.TblGradeDesignationMapping.Add(mpping);
                        db.SaveChanges();

                        scope.Complete();
                        scope.Dispose();
                        return 1;
                    }
                }
            }
            catch(Exception ex)
            {
                return -1;
            }
           
        }

        public GradeList GetDetailsForEdit(int GradeID,int GradeDesignationId)
        {
            DynamicParameters para = new DynamicParameters();
            para.Add("@GradeID", GradeID);
            para.Add("@GradeDesignationId", GradeDesignationId);
            return DpprObj.DapperFirst<GradeList>("select a.GradeID,a.Gradename,a.GradeCode,c.DesignationID,e.DeptID from TblGrades a join TblGradeDesignationMappings b on a.GradeID=b.GradeID join TblDesignations c on b.DesignationID=c.DesignationID " +
               " join TblDeptDesignationMappings d on d.DesignationID=c.DesignationID join TblDepartments e on d.DeptID=e.DeptID where b.MappingStatus=1 and a.GradeID=@GradeID and b.GradeDesignationId=@GradeDesignationId", para);
        }

        public int EditGrade(GradeList editObj, Int64 UID)
        {
            try
            {
                //var count = db.TblGradeDesignationMapping.Where(m => m.DesignationID == editObj.DesignationID && m.GradeID == editObj.GradeID && m.GradeDesignationId != editObj.GradeDesignationId).Count();
                DynamicParameters param = new DynamicParameters();
                param.Add("@DesignationID", editObj.DesignationID);
                param.Add("@Gradename", editObj.Gradename);
                param.Add("@GradeDesignationId", editObj.GradeDesignationId);

                var count = DpprObj.DapperSingle("Select count(*)from TblGradeDesignationMappings a join TblGrades b on a.GradeID=b.GradeID where DesignationID = @DesignationID and Gradename = @Gradename and GradeDesignationId != @GradeDesignationId", param);
                if(Convert.ToInt32(count) == 0)
                {
                    //var GrdChk = db.TblGrade.Where(m => m.Gradename == editObj.Gradename && m.GradeStatus == true && m.GradeID != editObj.GradeID).Count();
                    using (TransactionScope scope = new TransactionScope())
                    {
                        TblGrade grd = new TblGrade();
                        grd = db.TblGrade.Find(editObj.GradeID);
                        grd.Gradename = editObj.Gradename;
                        grd.GradeCode = editObj.GradeCode;
                        grd.LastUpdatedBy = UID;
                        grd.LastUpdatedDate = DateTimeOffset.Now;
                        db.Entry(grd).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                        TblGradeDesignationMapping mpping = new TblGradeDesignationMapping();
                        mpping = db.TblGradeDesignationMapping.Find(editObj.GradeDesignationId);
                        mpping.DesignationID = editObj.DesignationID;
                        //mpping.GradeID = editObj.GradeID;
                        mpping.LastUpdatedBy = UID;
                        mpping.LastUpdatedDate = DateTimeOffset.Now;
                        db.Entry(mpping).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                        scope.Complete();
                        scope.Dispose();
                    }
                    return 1;

                }
                return 0;
            }
            catch(Exception ex)
            {
                return -1;
            }
        }

        public string GetGradeName(int GradeID)
        {
            return db.TblGrade.Where(m => m.GradeID == GradeID).Select(m => m.Gradename).SingleOrDefault();
        }

        public int DeleteGrade(int GradeID, int GradeDesignationId,Int64 UID)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    TblGrade grd = new TblGrade();
                    grd = db.TblGrade.Find(GradeID);
                    grd.GradeStatus = false;
                    grd.LastUpdatedBy = UID;
                    grd.LastUpdatedDate = DateTimeOffset.Now;
                    db.Entry(grd).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    TblGradeDesignationMapping mpping = new TblGradeDesignationMapping();
                    mpping = db.TblGradeDesignationMapping.Find(GradeDesignationId);
                    mpping.MappingStatus = false;
                    mpping.LastUpdatedBy = UID;
                    mpping.LastUpdatedDate = DateTimeOffset.Now;
                    db.Entry(mpping).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    scope.Complete();
                    scope.Dispose();
                    return 1;
                }
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