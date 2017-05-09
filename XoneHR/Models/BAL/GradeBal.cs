using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using System.Data;

namespace XoneHR.Models.BAL
{

    public class GradeBal
    {
        private XoneDbLayer db;
        public GradeBal()
        {
            db = new XoneDbLayer();
        }

        public List<TblGrade> GetGradeList()
        {
            try
            {
                return db.DapperToList<TblGrade>("select  a.GradeID,a.DesigID,a.Gradename,b.DesigName,a.GradeStatus,b.DepID,c.DepName from TblGrade a join TblDesignation b on a.DesigID=b.DesigID join TblDepartment c on b.DepID=c.DepID where GradeStatus=1 and DepStatus=1 and DesigStatus=1 ");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<TblDesignation> GetDesignation(Int32 depid)
        {
            try
            {
                DynamicParameters para = new DynamicParameters();
                para.Add("@DepID", depid);

                var ListDesignation = db.DapperToList<TblDesignation>("Select DesigID,DesigName from TblDesignation where DesigStatus=1 and DesigUserTyp=1 and DepID=@DepID",para);

                return ListDesignation;
            }
            catch
            {
                return null;
            }
        }

        public int ManageGrade(TblGrade GradeObj, int Flag, int GradeStatus)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@GradeID", GradeObj.GradeID);
                param.Add("@DesigID", GradeObj.DesigID);
                param.Add("@Gradename", string.IsNullOrEmpty(GradeObj.Gradename) ? "" : GradeObj.Gradename.Trim());
                param.Add("@Flag", Flag);
                param.Add("@GradeStatus", 1);
                param.Add("@OUT", 1, DbType.Int32, ParameterDirection.Output);
                db.DapperExecute("USP_ManageGrade", param, CommandType.StoredProcedure);
                Int32 output = param.Get<Int32>("OUT");
                return output;

            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int DeleteGrade(int GradeID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@GradeID", GradeID);
                return db.DapperExecute("update TblGrade set GradeStatus=0 where GradeID=@GradeID", param);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


    }
}