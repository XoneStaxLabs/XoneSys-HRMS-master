using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Xone
{
    public class TblCitizenDetails
    {
        [Key]
        public Int16 CitizenID { get; set; }
        public string CitizenName { get; set; }
        public string CitizenCode { get; set; }
        public string CitizenDesc { get; set; }
        public bool IsDeletable { get; set; }
        public bool IsStatus { get; set; }
        public Int64 CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public Int64 LastUpdatedBy { get; set; }
        public DateTimeOffset LastUpdatedDate { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }

    }

    public class TblRace
    {
        [Key]
        public Int16 RaceID { get; set; }
        public string RaceName { get; set; }
        public bool RaceStatus { get; set; }
        public Int64 CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public Int64 LastUpdatedBy { get; set; }
        public DateTimeOffset LastUpdatedDate { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }

    public class TblLanguageDetails
    {
        [Key]
        public Int16 LanguageID { get; set; }
        public string LanguageName { get; set; }
        public bool LanguageStatus { get; set; }
        public Int64 CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public Int64 LastUpdatedBy { get; set; }
        public DateTimeOffset LastUpdatedDate { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }

    }

    public class TblSkillDetails
    {
        [Key]
        public Int32 SkillID { get; set; }
        public string SkillName { get; set; }
        public string Description { get; set; }
        public bool SkillStatus { get; set; }
        public Int64 CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public Int64 LastUpdatedBy { get; set; }
        public DateTimeOffset LastUpdatedDate { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
     
    public class TblDocumentTypes
    {
        [Key]
        public int DocTypeID { get; set; }
        public string DocTypeName { get; set; }
        public bool DocTypeStatus { get; set; }
        public Int64 CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public Int64 LastUpdatedBy { get; set; }
        public DateTimeOffset LastUpdatedDate { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
    } 

    public class TblDocumentSubTypes
    {
        [Key]
        public int DocSubtypeID { get; set; }
        public int DocTypeID { get; set; }
        [ForeignKey("DocTypeID")]        
        public virtual TblDocumentTypes TblDocumentTypes { get; set; }
        public string DocSubtypeName { get; set; }
        public bool DocSubtypeStatus { get; set; }
        public  Int64 CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public Int64 LastUpdatedBy { get; set; }
        public DateTimeOffset LastUpdatedDate { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }

    public class TblCandidate
    {
        [Key]
        public Int64 CandID { get; set; }
        public Int64 CandNo { get; set; }
        public Int16 CitizenID { get; set; }
        public string CandName { get; set; }
        public string CandMobile { get; set; }
        public string CandPhone { get; set; }
        public string CandAddress { get; set; }
        public string CandEmail { get; set; }
        public Int32 GradeID { get; set; }
        public Int32 DesignationID { get; set; }
        public DateTime CandDob { get; set; }
        public DateTimeOffset CandRegDate { get; set; }
        public string CandGender { get; set; }
        public string CandPlaceofBirth { get; set; }
        public Int16 RaceID { get; set; }
        public Int16 ReligonID { get; set; }
        public Int16 MaritID { get; set; }
        public string CandDetails { get; set; }
        public string CandPhoto { get; set; }
        public int CandSalrexpted { get; set; }
        public string CandHgEducation { get; set; }
        public Int64 CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public Int64 ModifiedBy { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }

    }

    public class TblCandidateLanguage
    {
        [Key]
        public Int32 CandLangID { get; set; }
        public Int64 CandID { get; set; }
        public Int16 LanguageID { get; set; }
        public Int64 ModifiedBy { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }

    }

    public class TblCandidateSkillset
    {
        [Key]
        public Int32 CandSklID { get; set; }
        public Int64 CandID { get; set; }
        public Int32 SkilID { get; set; }
        public Int64 ModifiedBy { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }   

    public class TblCandidateDocuments
    {
        [Key]
        public int CanddocID { get; set; }
        public Int64 CandID { get; set; }
        public int DocStypID { get; set; }
        [ForeignKey("DocStypID")]
        public virtual TblDocumentSubTypes TblDocumentSubTypes { get; set; }
        public string CandDocuments { get; set; }
        public DateTimeOffset CandDocAddeddate { get; set; }
        public Int64 ModifiedBy { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }

    }

}