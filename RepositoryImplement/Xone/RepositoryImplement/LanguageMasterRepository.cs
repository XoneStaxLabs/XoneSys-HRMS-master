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
    public class LanguageMasterRepository :ILanguageMaster
    {
        private XoneContext db = new XoneContext();
        private DapperLayer DapperObj = new DapperLayer();

        public IEnumerable<TblLanguageDetails> ListLanguageDetails()
        {
            return db.TblLanguageDetails.Where(m => m.LanguageStatus == true).ToList();
        }

        public int CreateLanguage(TblLanguageDetails LangObj,Int64 UID)
        {
            try
            {
                var count = db.TblLanguageDetails.Where(m => m.LanguageName == LangObj.LanguageName && m.LanguageStatus == true).Count();
                if(count == 0)
                {
                    LangObj.CreatedBy = UID;
                    LangObj.CreatedDate = DateTimeOffset.Now;
                    LangObj.LanguageStatus = true;
                    db.TblLanguageDetails.Add(LangObj);
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

        public TblLanguageDetails GetLangDetails(Int16 LanguageID)
        {
            return db.TblLanguageDetails.Where(m => m.LanguageID == LanguageID).FirstOrDefault();
        }

        public int EditLngDetails(TblLanguageDetails LngObj, Int64 UID)
        {
            try
            {
                var count = db.TblLanguageDetails.Where(m => m.LanguageName == LngObj.LanguageName && m.LanguageID != LngObj.LanguageID).Count();
                if (count == 0)
                {
                    TblLanguageDetails details = new TblLanguageDetails();
                    details = db.TblLanguageDetails.Find(LngObj.LanguageID);
                    details.LanguageName = LngObj.LanguageName;
                    details.LastUpdatedBy = UID;
                    details.LastUpdatedDate = DateTimeOffset.Now;
                    db.Entry(details).State = EntityState.Modified;
                    db.SaveChanges();
                    return 1;
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public string GetLanguageName(Int16 LanguageID)
        {
            return db.TblLanguageDetails.Where(m => m.LanguageID == LanguageID).Select(m => m.LanguageName).SingleOrDefault();
        }

        public bool CheckDeletableStatus(Int16 LanguageID)
        {
            //Candidat/emp status not checked 
            var count = db.TblCandidateLanguage.Where(m => m.LanguageID == LanguageID).Count();
            if (count == 0)
                return true;
            else
                return false;
        }
        
        public int DeleteLanguage(Int16 LanguageID, Int64 UID)
        {
            try
            {
                TblLanguageDetails lng = new TblLanguageDetails();
                lng = db.TblLanguageDetails.Find(LanguageID);
                lng.LanguageStatus = false;
                lng.LastUpdatedBy = UID;
                lng.LastUpdatedDate = DateTimeOffset.Now;
                db.Entry(lng).State = EntityState.Modified;
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
            //throw new NotImplementedException();
            db.Dispose();
        }
    }
}