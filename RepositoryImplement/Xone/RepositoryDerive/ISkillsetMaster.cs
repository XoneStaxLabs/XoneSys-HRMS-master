using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.Xone;

namespace RepositoryImplement.Xone.RepositoryDerive
{
    public interface ISkillsetMaster
    {
        IEnumerable<TblSkillDetails> ListSkillDetails();
        int CreateSkills(TblSkillDetails Skills, Int64 UID);
        TblSkillDetails GetEditSkillDetails(Int32 SkillID);
        int EditSkillsets(TblSkillDetails skills, Int64 UID);
        bool CheckDeletableStatus(Int32 SkillID);
        string GetSkillName(Int32 SkillID);
        int DeleteSkillset(Int32 SkillID, Int64 UID);

    }
}