using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XoneHR.Models
{
    public class TblCandidate
    {
        public Int64 CandID { get; set; }
        public Int64 CandNo { get; set; }
        public Int16 CizenID { get; set; }
        public string CandName { get; set; }
        public string CandMobile { get; set; }
        public string CandPhone { get; set; }
        public string CandAddress { get; set; }
        public string CandEmail { get; set; }
        public string CandNRICNo { get; set; }
        public Int32 DesigID { get; set; }
        public DateTime CandDob { get; set; }
        public Int32 CandAge { get; set; }
        public DateTime CandRegDate { get; set; }
        public string CandGender { get; set; }
        public string CandPlaceofBirth { get; set; }
        public Int16 RaceID { get; set; }
        public Int32 ReligID { get; set; }
        public Int16 MaritID { get; set; }
        public string CandDetails { get; set; }
        public bool CandStatus { get; set; }
        public string CandPhoto { get; set; }
        public Int32 CandSalrexpted { get; set; }
        public string CandHgEducation { get; set; }
        public float CandOfferSalary { get; set; }
        public DateTime CandJoinDate { get; set; }
        public DateTime PassportExpiry { get; set; }
        public string FinNo { get; set; }
        public Int16 Dep_UserType { get; set; }
        public Int32 GradeID { get; set; }
        public Int16 Resident_Id { get; set; }
        public string PassportNo { get; set; }
        public DateTime FinExpiry { get; set; }

        //
        public string PassportNo_Sig_Citizen_PR { get; set; }

        public string PassportNO_WorkPermit { get; set; }
    }

    public class TblCityZenship
    {
        public Int16 CizenID { get; set; }
        public string Citizen { get; set; }
        public string CitiZenCode { get; set; }

        public string CitiZenNoname { get; set; }
        public bool IsStatus { get; set; }
    }

    public class TblDesignation
    {
        public Int32 DesigID { get; set; }
        public Int32 DepID { get; set; }
        public string DesigName { get; set; }
        public bool DesigStatus { get; set; }
        public Int16 DesigUserTyp { get; set; }
    }

    public class TblRace
    {
        public Int16 RaceID { get; set; }
        public string RaceName { get; set; }
        public bool RaceStatus { get; set; }
    }

    public class TblReligion
    {
        public Int32 ReligID { get; set; }
        public string ReligName { get; set; }
        public bool ReligStatus { get; set; }
    }

    public class TblSkillSets
    {
        public Int32 SkilID { get; set; }
        public string SkilsetName { get; set; }
        public string Description { get; set; }
        public bool SkilStatus { get; set; }
    }

    public class TblMaritalStatus
    {
        public Int16 MaritID { get; set; }
        public string MaritulStatusType { get; set; }
        public bool MaritStatus { get; set; }
    }

    public class TblCitizenDetails
    {
        public Int32 CitzDetID { get; set; }
        public Int64 CandID { get; set; }
        public DateTime? CitzOrdrDate { get; set; }
        public DateTime? CitzOrdrDateOne { get; set; }
        public string CitzNsVaction { get; set; }
        public string CitzReglrVaction { get; set; }
        public byte CitzReservist { get; set; }
        public bool CitzStatus { get; set; }
    }

    public class TblEmployeHistory
    {
        public Int32 EmphID { get; set; }
        public Int64 CandID { get; set; }
        public string EmphEmployer { get; set; }
        public DateTime EmphStartDate { get; set; }
        public DateTime EmphEndDate { get; set; }
        public double EmphExpYr { get; set; }
        public Int32 Emphesptsalry { get; set; }
        public Int32 EmphSalryLast { get; set; }
        public bool EmphStatus { get; set; }
    }

    public class TblEmergencyContact
    {
        public Int32 EmrgID { get; set; }
        public Int64 CandID { get; set; }
        public string EmrgContact { get; set; }
        public string EmrgRelationship { get; set; }
        public string EmrgContactNo { get; set; }
        public string EmrgDetails { get; set; }
        public bool EmrgStatus { get; set; }
    }

    public class TblCandidateBank
    {
        public Int32 CandBnkID { get; set; }
        public Int64 CandID { get; set; }
        public string CandBnkName { get; set; }
        public string CandBnkCode { get; set; }
        public string CandbrnchCode { get; set; }
        public string CandBnkAcno { get; set; }
        public bool CandBnkStatus { get; set; }
    }

    public class TblDeclarationType
    {
        public Int32 DeclTypeID { get; set; }
        public Int32 DeclCode { get; set; }
        public string DeclName { get; set; }
        public bool DeclStatus { get; set; }
    }

    public class TblDeclaration
    {
        public Int32 DeclID { get; set; }
        public Int32 DeclTypeID { get; set; }
        public string DeclContent { get; set; }
        public bool DeclStatus { get; set; }
    }

    public class TblDocumentTypes
    {
        public Int32 DocTypID { get; set; }
        public string DocTypName { get; set; }
        public bool DocTypStatus { get; set; }
    }

    public class TblDocumentSubTypes
    {
        public Int32 DocStypID { get; set; }
        public Int32 DocTypID { get; set; }
        public string DocStypName { get; set; }
        public string DocTypName { get; set; }
        public bool DocStypStatus { get; set; }
    }

    public class TblDocuments
    {
        public Int32 DocID { get; set; }
        public Int32 DocStypID { get; set; }
        public Int64 CandID { get; set; }
        public string DocFiles { get; set; }
        public bool DocStatus { get; set; }
        public DateTime DocDate { get; set; }
    }

    public class TblContractInfoType
    {
        public Int32 ConInfoTypID { get; set; }
        public string ConInfoTypName { get; set; }
        public string ConInfoRemks { get; set; }
        public bool ConInfoTypStatus { get; set; }
    }

    public class TblContractInfoMaster
    {
        public Int32 ConInfoID { get; set; }
        public Int32 ConInfoTypID { get; set; }
        public string ConInformation { get; set; }
        public string ConInfoDetails { get; set; }
        public string ConInfoTypName { get; set; }
        public bool ConInfoStatus { get; set; }
    }

    public class TblFixedAllowanceEmpwise
    {
        public Int32 FixAllEmpD { get; set; }
        public Int32 FixSalaTypID { get; set; }
        public Int64 EmpID { get; set; }
        public bool FixAllEmpStatus { get; set; }
    }

    public class TblFixedAllowanceEmpTypes
    {
        public Int32 FixallEmpTypeID { get; set; }
        public string FixallEmpTypes { get; set; }
        public double FixallEmpAmounts { get; set; }
        public bool FixallempStatus { get; set; }
        public Int32 FixAllEmpD { get; set; }
    }

    public class TblProject
    {
        public Int64 ProjID { get; set; }
        public string ProjName { get; set; }
        public DateTime ProjFrom { get; set; }
        public DateTime ProjTo { get; set; }
        public string ProjLocation { get; set; }
        public Int16 ProjStatus { get; set; }
        public Int32 projTypID { get; set; }
        public Int32 ProjEmpno { get; set; }
        public Int32 ProjNoofShift { get; set; }
        public Int32 ProcompTypID { get; set; }
        public float ProjCompAmount { get; set; }
    }

    public class TblShift
    {
        public Int32 ShiftID { get; set; }
        public string ShiftName { get; set; }
        public string ShiftFrom { get; set; }
        public string ShiftTo { get; set; }
        public Int32 ShiftEmpNo { get; set; }
        public Int64 ProjID { get; set; }
        public int Indexval { get; set; }
    }

    public class TblQuestions
    {
        public Int64 QuestID { get; set; }
        public int QtypeID { get; set; }
        public int QSubtypeID { get; set; }
        public string QuestName { get; set; }
        public bool QuestStatus { get; set; }
    }

    public class TblCandidateAnswers
    {
        public Int64 AnsID { get; set; }
        public Int64 CandID { get; set; }
        public Int64 QuestID { get; set; }
        public int SubQuestID { get; set; }
        public string Answer { get; set; }
        public string AnsRemarks { get; set; }
        public float AnsRating { get; set; }
        public bool AnsStatus { get; set; }
        public DateTime AnsDate { get; set; }
    }

    public class TblLanguageKnown
    {
        public Int16 LangknID { get; set; }
        public string LanguageName { get; set; }
        public bool LangKnStatus { get; set; }
    }

    public class TblCandidateSkillset
    {
        public int CandSklID { get; set; }
        public Int64 CandID { get; set; }
        public int SkilID { get; set; }
        public bool CandskilStatus { get; set; }
    }

    public class TblCandidateLanguage
    {
        public int CandLangID { get; set; }
        public Int64 CandIDCandID { get; set; }
        public Int16 LangknID { get; set; }
    }

    public class QuestionAnswer
    {
        public string QuestName { get; set; }
        public string Answer { get; set; }
        public float AnsRating { get; set; }
    }

    public class TblDayType
    {
        public Int32 DayTypID { get; set; }
        public string DayType { get; set; }
        public Int32 DayValue { get; set; }
    }

    public class TblLeaveType
    {
        public Int32 LeavetypID { get; set; }
        public string LeaveType { get; set; }
        public string LeavesText { get; set; }
    }

    public class TblAttendance
    {
        public Int32 AttendID { get; set; }
        public Int64 EmpID { get; set; }
        public string AttendtimeIN { get; set; }
        public string AttendtimeOut { get; set; }
        public DateTime Attenddate { get; set; }

        public string AttendRemks { get; set; }

        public bool AttendHoliStatus { get; set; }

        public Int64 ProjID { get; set; }
        public Int32 SchID { get; set; }
        public bool IsOFFDay { get; set; }
    }

    public class TblWeek
    {
        public Int32 WeekID { get; set; }
        public Int32 SchID { get; set; }
        public Int32 ShiftID { get; set; }
        public Int64 EmpID { get; set; }
        public DateTime Wsun { get; set; }
        public DateTime WMon { get; set; }
        public DateTime Wtue { get; set; }
        public DateTime Wwed { get; set; }
        public DateTime Wthu { get; set; }
        public DateTime Wfri { get; set; }
        public DateTime Wsat { get; set; }
        public Int64 ProjID { get; set; }
    }

    public class TblEmployeeType
    {
        //1:Permanent 2:Parttime 3:Adhoc
        //Emp_PermenanentType : 1:Permanent Day,2.Permanent Night,3.Not Permanent Employee
        public Int32 EmpTypID { get; set; }

        public string EmpTypName { get; set; }
        public bool EmpTypStatus { get; set; }
    }

    public class TblDepartment
    {
        public int DepID { get; set; }
        public string DepName { get; set; }
        public bool DepStatus { get; set; }
        public int DepUserTyp { get; set; }
    }

    public class TblFixecSalaryTypes
    {
        public int FixSalaTypID { get; set; }
        public string FixSalaType { get; set; }
        public bool FixSalaStatus { get; set; }
    }

    public class TblFixedAllowanceDesigwise
    {
        public int FixAllDesigID { get; set; }
        public int FixSalaTypID { get; set; }
        public int DesigID { get; set; }
        public bool FixAllStatus { get; set; }
    }

    public class TblFixedAllowanceTypes
    {
        public int FixallTypeID { get; set; }
        public string FixallTypes { get; set; }
        public float FixallAmounts { get; set; }
        public bool FixallStatus { get; set; }
        public int FixAllDesigID { get; set; }

        //
        public string FixallEmpTypes { get; set; }

        public float FixallEmpAmounts { get; set; }
        public int FixSalaTypID { get; set; }
    }

    public class TblSalaryDeginationwise
    {
        public int SalaDesigID { get; set; }
        public int DesigID { get; set; }
        public string SalaPayDate { get; set; }
        public string SalaPeriod { get; set; }
    }

    public class TblLeaveDesignationwise
    {
        public int LeaveDesigID { get; set; }
        public int LeavetypID { get; set; }
        public int DesigID { get; set; }
        public string Leaves { get; set; }
        public bool LeaveStatus { get; set; }
    }

    public class TblFixedAllowanceMaster
    {
        public int FixallowmID { get; set; }
        public int FixSalaTypID { get; set; }
        public string FixallowmTypes { get; set; }
        public float FixallowmAmounts { get; set; }
        public bool FixallowmStatus { get; set; }
    }

    public class TblSalaryMasterwise
    {
        public int SalaMasterID { get; set; }
        public string SalaMasterPayDate { get; set; }
        public string Salamasterperiod { get; set; }
        public bool SalamasterStatus { get; set; }
    }

    public class TblLeaveMasterwise
    {
        public int LeavetypID { get; set; }
        public int LeavesMastNo { get; set; }
        public string LeaveType { get; set; }
    }

    public class TblCandidateDeclaration
    {
        public Int32 CandDeclID { get; set; }
        public Int64 CandID { get; set; }
        public Int32 DeclID { get; set; }
        public bool CandDeclAns { get; set; }
        public string CandDeclAnsDetail { get; set; }
    }

    public class TblProjectCompenseType
    {
        public Int32 ProcompTypID { get; set; }
        public string ProCompType { get; set; }
    }

    public class Tblschedule
    {
        public int SchID { get; set; }
        public string Schedule { get; set; }
        public int MonthID { get; set; }
        public Int32 ShiftID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class TblAbscenceAllocate
    {
        public int AbsentID { get; set; }
        public Int64 AbsEmpID { get; set; }
        public Int64 AbsallEmpID { get; set; }
        public DateTime AbsDateFrom { get; set; }
        public DateTime AbsDateTo { get; set; }
        public Int64 ProjID { get; set; }
        public int SchID { get; set; }
        public int ShiftID { get; set; }
        public bool ReliefConfirmation { get; set; }
    }

    public class TblHolidayList
    {
        public int HolidayID { get; set; }
        public string HoliText { get; set; }
        public DateTime HoliDate { get; set; }
        public bool HoliStatus { get; set; }
    }

    public class TblAppUser
    {
        public Int64 AppUID { get; set; }
        public string AppUserName { get; set; }
        public string DesigName { get; set; }
        public string Citizen { get; set; }
        public string AppPwd { get; set; }
        public string AppFirstName { get; set; }
        public Int16 CitizenID { get; set; }
        public string AppMobile { get; set; }
        public string AppAddress { get; set; }
        public string AppEmail { get; set; }
        public int DesigID { get; set; }
        public string AppGender { get; set; }
        public string AppPhoto { get; set; }
        public bool AppStatus { get; set; }
        public Int32 AppUserTypID { get; set; }
        public Int64 EmpID { get; set; }
    }

    public class TblMenuMaster
    {
        public Int32 MenuID { get; set; }
        public string MenuName { get; set; }
        public Int32 MenuParentID { get; set; }
        public string MenuController { get; set; }
        public string MenuAction { get; set; }
        public string MenuIcons { get; set; }
        public bool MenuStatus { get; set; }
        public Int32 MenuOrdNo { get; set; }
    }

    public class TblAppDesignation
    {
        public int AppDesigID { get; set; }
        public Int16 AppDepID { get; set; }
        public string AppDesigName { get; set; }
    }

    public class TblAppDepartment
    {
        public Int16 AppDepID { get; set; }
        public string AppDepName { get; set; }
    }

    public class TblMemos
    {
        public Int32 MemoID { get; set; }
        public string MemoSubject { get; set; }
        public string MemoText { get; set; }
        public Int32 MemoPriority { get; set; }
        public bool MemoStatus { get; set; }
        public Int64 MemoAddedby { get; set; }
        public DateTime MemoAddDate { get; set; }
        public DateTime? MemoReadDate { get; set; }

        public Int32 MemoRepID { get; set; }
    }

    public class TblmemoUserwise
    {
        public Int32 memoUserID { get; set; }
        public Int64 AppUID { get; set; }
        public Int32 MemoID { get; set; }
    }

    public class TblSalaryPay
    {
        public Int64 PayId { get; set; }
        public Int64 EmpId { get; set; }
        public Int32 SalYear { get; set; }
        public Int32 SalMonth { get; set; }
        public float PayAmount { get; set; }
        public Int16 PayType { get; set; }
        public DateTime PayDate { get; set; }
        public Int16 PayMode { get; set; }
    }
       

    public class TblFundTypeMaster
    {
        public int FundTypeID { get; set; }
        public string FundType { get; set; }
        public bool FundStatus { get; set; }
    }

    public class TblCompanyEmail
    {
        public Int16 CompEmailID { get; set; }
        public Int16 CompID { get; set; }
        public string EmailID { get; set; }
        public int CompPortNo { get; set; }
        public string CompPassword { get; set; }
    }

    public class TblCompanyMaster
    {
        public Int16 CompID { get; set; }
        public string CompName { get; set; }
        public string CompLogo { get; set; }
        public string CompTest { get; set; }
    }

    public class TblValidDoctypeMaster
    {
        public int ValidDocTypID { get; set; }
        public int DocStypID { get; set; }
        public int DesigID { get; set; }
        public int CizenID { get; set; }
    }

    public class TblEmployeeSalary
    {
        public Int64 SalMastID { get; set; }
        public Int64 EmpID { get; set; }
        public DateTime SalStartDate { get; set; }
        public DateTime SalEndDate { get; set; }
        public double BasicSalary { get; set; }
    }

    public class TblTimeOFF
    {
        public Int64 TimeOFFID { get; set; }
        public Int64 EmpID { get; set; }
        public DateTime TimeOFFDate { get; set; }
        public string TimeOFFStart { get; set; }
        public string TimeOFFEnd { get; set; }
        public string TimeOFFRemarks { get; set; }
    }

    public class TblEntity
    {
        public int Tbl_ID { get; set; }
        public string Tbl_Name { get; set; }
        public string Tbl_KeyColumn { get; set; }
        public bool Tbl_IsActive { get; set; }
    }

    public class TblActionLog
    {
        public Int64 Act_ID { get; set; }
        public int Tbl_ID { get; set; }
        public ActionType Act_Type { get; set; }
        public string Act_Controller { get; set; }
        public Int64 Act_Key { get; set; }
        public string Act_Action { get; set; }
        public Int64 Act_Modifiedby { get; set; }
        public DateTime Act_ModifiedOn { get; set; }
        public bool Act_IsActive { get; set; }
    }

    public enum ActionType
    {
        Create = 1, Insert = 2, Update = 3, Delete = 4
    }

    //public class TblFunctionType
    //{
    //    public int FunTypeID { get; set; }
    //    public string FunTypeName { get; set; }
    //}

    public class TblPLRDDetails
    {
        public Int64 PLRDId { get; set; }
        public Int64 CandID { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public Int64 LicenseNum { get; set; }

        public string dtissue { get; set; }
        public string dtexpiry { get; set; }
    }

    public class TblOffBoarding
    {
        public Int64 OFFBoard_id { get; set; }
        public Int64 EmpID { get; set; }
        public DateTime OFFBoard_NoticeStart { get; set; }
        public DateTime OFFBoard_NoticeEnd { get; set; }
        public DateTime OFFBoard_Resgnation { get; set; }
        public string OFFBoard_Remark { get; set; }

        // 1: Request to offboarding, 2: Noticeperiod Start, 3:Resign
        public Int16 OFFBoard_Status { get; set; }

        //
        public string CandName { get; set; }
    }

    public class TblGrade
    {
        public Int32 GradeID { get; set; }
        public Int32 DesigID { get; set; }
        public string DesigName { get; set; }
        public string Gradename { get; set; }
        public bool GradeStatus { get; set; }
        public int PrjGradeEmpNO { get; set; }
        public int DepID { get; set; }
        public string DepName { get; set; }
    }

    public class TblResidentStatus
    {
        public Int16 Resident_Id { get; set; }
        public string Resident_Name { get; set; }
    }

    public class TblProjectGradeAssign
    {
        public Int32 PrjGrdAssignID { get; set; }
        public Int64 ProjID { get; set; }
        public Int32 ShiftID { get; set; }
        public int DesigID { get; set; }
        public int GradeID { get; set; }
        public bool PrjGrdAssgnStatus { get; set; }
        public int PrjGradeEmpNO { get; set; }
    }

    public class TblEmployeeLeaveApp
    {
        public Int32 EmpLeaveappID { get; set; }
        public Int64 EmpID { get; set; }
        public Int32 LeavetypID { get; set; }
        public DateTime EmpLeaveDate { get; set; }
        public DateTime EmpLeavetodate { get; set; }
        public Int32 DayTypID { get; set; }
        public double EmpappDays { get; set; }

        //0-Pending , 1 - Approved , 2-Rejected, 3-Authorized
        public Int16 EmpappStatus { get; set; }

        public string EmpappRemks { get; set; }
        public Int64 EmpLeaveAppBy { get; set; }
        public Int32 SchID { get; set; }
        public string LeaveDocument { get; set; }
    }

    public class TblCheckListTypes
    {
        public Int16 CheckListTypeID { get; set; }
        public string CheckListDetails { get; set; }
        public float Price { get; set; }
    }

    public class TblOnboardCheckList
    {
        public Int64 CheckListID { get; set; }
        public Int64 EmpID { get; set; }
        public Int16 CheckListTypeID { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string Size { get; set; }
        public Int16 CheckListStatus { get; set; }
    }

    public class TblOffboardCheckList
    {
        public Int64 OffboardListID { get; set; }
        public Int64 CheckListID { get; set; }
        public Int16 CheckListTypeID { get; set; }
        public int Quantity { get; set; }
    }

    public class TblDeductionType
    {
        public Int32 DeductType_Id { get; set; }
        public string DeductionType { get; set; }
    }

    public class TblDeductions
    {
        public Int64 Deduct_Id { get; set; }
        public int DeductType_Id { get; set; }
        public Int64 EmpID { get; set; }
        public int DeductYear { get; set; }
        public int DeductMonth { get; set; }
        public string Deduction { get; set; }
        public float Amount { get; set; }
        public string Remarks { get; set; }
        public DateTime AddedDate { get; set; }
        //component
        public string MonthName { get; set; }
    }

}