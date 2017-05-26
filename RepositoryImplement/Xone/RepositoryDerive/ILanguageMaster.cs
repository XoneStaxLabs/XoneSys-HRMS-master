using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.Xone;

namespace RepositoryImplement.Xone.RepositoryDerive
{
    public interface ILanguageMaster:IDisposable
    {
        IEnumerable<TblLanguageDetails> ListLanguageDetails();
        int CreateLanguage(TblLanguageDetails LangObj,Int64 UID);
        TblLanguageDetails GetLangDetails(Int16 LanguageID);
        int EditLngDetails(TblLanguageDetails LangObj, Int64 UID);
        string GetLanguageName(Int16 LanguageID);
        bool CheckDeletableStatus(Int16 LanguageID);
        int DeleteLanguage(Int16 LanguageID, Int64 UID);

    }
}