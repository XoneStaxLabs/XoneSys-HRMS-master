using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using XoneHR.Models;
using System.Web;
using System.Data;


namespace XoneHR.Models
{

    public class SkillSetBLayer
    {
        private XoneDbLayer db;
        public SkillSetBLayer()
        {
            db = new XoneDbLayer();
        }
        public IEnumerable<TblSkillSets> GetSkillList()
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                return db.DapperIEnumerable<TblSkillSets>("select *From TblSkillSets", param);

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public int ManageSkill(int flag,string skillname="",string skilldesc="",Int32 status=0,int SkillId=0)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@skillname", skillname);
            param.Add("@skilldesc", skilldesc);
            param.Add("@flag", flag);
            param.Add("@skillid", SkillId);
            param.Add("@Status",Convert.ToBoolean(status));
            param.Add("@OUT", 1, DbType.Int32, ParameterDirection.Output);

            db.DapperExecute("USP_SaveSkillSet", param, CommandType.StoredProcedure);
            Int32 output = param.Get<Int32>("OUT");
            return output;
        }

    }
}