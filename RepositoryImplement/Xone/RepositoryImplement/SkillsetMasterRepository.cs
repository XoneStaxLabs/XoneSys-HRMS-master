using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RepositoryImplement.Xone.RepositoryDerive;
using Model.Xone;
using DbContexts.Xone;
using System.Data.Entity;


namespace RepositoryImplement.Xone.RepositoryImplement
{
    public class SkillsetMasterRepository:ISkillsetMaster
    {
        private XoneContext db = new XoneContext();
        private DapperLayer DapperObj = new DapperLayer();

        public IEnumerable<TblSkillDetails> ListSkillDetails()
        {
            return db.TblSkillDetails.Where(m => m.SkillStatus == true).ToList();
        }

        public int CreateSkills(TblSkillDetails Skills, Int64 UID)
        {
            try
            {
                var count = db.TblSkillDetails.Where(m => m.SkillName == Skills.SkillName).Count();
                if (count == 0)
                {
                    Skills.SkillStatus = true;
                    Skills.CreatedBy = UID;
                    Skills.CreatedDate = DateTimeOffset.Now;
                    db.TblSkillDetails.Add(Skills);
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
        
        public TblSkillDetails GetEditSkillDetails(Int32 SkillID)
        {
            return db.TblSkillDetails.Where(m => m.SkillID == SkillID).FirstOrDefault();
        }

        public int EditSkillsets(TblSkillDetails skills, Int64 UID)
        {
            try
            {
                var count = db.TblSkillDetails.Where(m => m.SkillName == skills.SkillName && m.SkillID != skills.SkillID).Count();
                if(count == 0)
                {
                    TblSkillDetails skillset = new TblSkillDetails();
                    skillset = db.TblSkillDetails.Find(skills.SkillID);
                    skillset.SkillName = skills.SkillName;
                    skillset.Description = skills.Description;
                    skillset.LastUpdatedBy = UID;
                    skillset.LastUpdatedDate = DateTimeOffset.Now;
                    db.Entry(skillset).State = EntityState.Modified;
                    db.SaveChanges();
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch(Exception ex)
            {
                return -1;
            }
        }

        public bool CheckDeletableStatus(Int32 SkillID)
        {
            var count = db.TblCandidateSkillset.Where(m => m.CandSklID == SkillID).Count();
            if (count == 0)
                return true;
            else
                return false;
        }

        public string GetSkillName(Int32 SkillID)
        {
            return db.TblSkillDetails.Where(m => m.SkillID == SkillID).Select(m => m.SkillName).SingleOrDefault();
        }

        public int DeleteSkillset(Int32 SkillID, Int64 UID)
        {
            try
            {
                TblSkillDetails skills = new TblSkillDetails();
                skills = db.TblSkillDetails.Find(SkillID);
                skills.SkillStatus = false;
                skills.LastUpdatedBy = UID;
                skills.LastUpdatedDate = DateTimeOffset.Now;
                db.Entry(skills).State = EntityState.Modified;
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