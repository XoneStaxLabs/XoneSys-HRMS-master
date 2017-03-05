using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XoneHR.Models
{
    public class CandidateItems
    {
        public Int64 TableID { get; set; }
        public Int32 OutputID { get; set; }
    }

    public class CandidateDetails
    {
        public Int64 CandID { get; set; }
        public Int64 CandNo { get; set; }
        public string Citizen { get; set; }
        public Int16 CizenID { get; set; }
        public string CandName { get; set; }
        public string CandMobile { get; set; }
        public string CandPhone { get; set; }
        public string CandAddress { get; set; }
        public string CandEmail { get; set; }
        public string CandNRICNo { get; set; }
        public string DesigName { get; set; }
        public Int32 DesigID { get; set; }
        public DateTime CandDob { get; set; }
        public Int32 CandAge { get; set; }
        public DateTime CandRegDate { get; set; }
        public string CandGender { get; set; }
        public string CandPlaceofBirth { get; set; }
        public string RaceName { get; set; }
        public Int16 RaceID { get; set; }
        public string ReligName { get; set; }
        public Int16 ReligID { get; set; }
        public string MaritulStatusType { get; set; }
        public Int16 MaritID { get; set; }
        public string CandDetails { get; set; }
        public bool CandStatus { get; set; }
        public string CandPhoto { get; set; }
        public int CandAppStatus { get; set; }
        public Int32 CandSalrexpted { get; set; }
        public string CandHgEducation { get; set; }
        public int EmrgID { get; set; }
        public string EmrgContact { get; set; }
        public string EmrgRelationship { get; set; }
        public string EmrgContactNo { get; set; }
        public string EmrgDetails { get; set; }
        public float CandOfferSalary { get; set; }
        public DateTime CandJoinDate { get; set; }
        public DateTime PassportExpiry { get; set; }
        public string FinNo { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public Int16 Dep_UserType { get; set; }
        public Int32 GradeID { get; set; }
        public Int16 Resident_Id { get; set; }
        public string PassportNo { get; set; }
        public DateTime FinExpiry { get; set; }
        public Int64 EmpID { get; set; }
        public string Resident_Name { get; set; }
        public string Gradename { get; set; }
        public Int64 LicenseNum { get; set; }
    }

    public class DocumentDetails
    {
        public Int32 DocID { get; set; }
        public string DocTypName { get; set; }
        public string DocStypName { get; set; }
        public string DocFiles { get; set; }
        public Int32 DocTypID { get; set; }
        public Int32 DocStypID { get; set; }
    }

    public class CandidateAppoinment
    {
        public Int64 CandID { get; set; }
        public Int64 EmpID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Int32 Employeetyp { get; set; }
        public Int16 EmployeeType { get; set; }
        public double BasicSalary { get; set; }
        public Int16 FundType { get; set; }

        // EmpPartTime_Pay: 1- hourly base salary, 2- daily wise base salary 0- its not part time emp
        public Int16 EmpPartTime_Pay { get; set; }

        //1: When employee, 0: when approve candidate(offrlttr sent,appointment letter not given)
        public bool Emp_IsApproved { get; set; }

        public float EmpLevyAmnt { get; set; }
        public Int16 Resident_Id { get; set; }

        //Emp_PermenanentType : 1:Permanent Day,2.Permanent Night,3.Not Permanent Employee
        public Int16 Emp_PermenanentType { get; set; }

        public Int64 EmpRegNo { get; set; }
        public Int16 RestDay_Type { get; set; }  // 0: Optional Off, 1: Fixed Off

        // public DateTime RestDay_Optional { get; set; }
        public Int16 RestDay_Fixed { get; set; }
    }

    public class CandidateDesigItems
    {
        public Int64 TableID { get; set; }
        public Int32 OutputID { get; set; }

        public Int32 DesignationID { get; set; }
    }

    public class FixedAllowanceDesig
    {
        public Int32 FixAllDesigID { get; set; }
        public Int32 FixSalaTypID { get; set; }
        public string FixallTypes { get; set; }
        public double FixallAmounts { get; set; }
    }

    public class EmployeeDetails
    {
        public Int64 CandID { get; set; }
        public Int64 CandNo { get; set; }
        public string Citizen { get; set; }
        public string CandName { get; set; }
        public string CandMobile { get; set; }
        public string CandPhone { get; set; }
        public string CandAddress { get; set; }
        public string CandEmail { get; set; }
        public string CandNRICNo { get; set; }
        public string DesigName { get; set; }
        public DateTime CandDob { get; set; }
        public Int32 CandAge { get; set; }
        public DateTime CandRegDate { get; set; }
        public string CandGender { get; set; }
        public string CandPlaceofBirth { get; set; }
        public string RaceName { get; set; }
        public string ReligName { get; set; }
        public string MaritulStatusType { get; set; }
        public string CandDetails { get; set; }
        public bool CandStatus { get; set; }
        public string CandPhoto { get; set; }
        public int CandAppStatus { get; set; }
        public Int32 CandSalrexpted { get; set; }
        public string CandHgEducation { get; set; }
        public string EmpRegNo { get; set; }
        public DateTime EmpStartDate { get; set; }
        public DateTime EmpEndDate { get; set; }
        public string EmpTypName { get; set; }
        public string EmpTypeName { get; set; }
        public Int64 EmpID { get; set; }
        public int EmrgID { get; set; }
        public string EmrgContact { get; set; }
        public string EmrgRelationship { get; set; }
        public string EmrgContactNo { get; set; }
        public string EmrgDetails { get; set; }
        public Int32 SalaEmpbasicSalary { get; set; }
        public Int32 DesigID { get; set; }
        public Int32 CizenID { get; set; }
        public string FinNo { get; set; }
        public DateTime CandJoinDate { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public Int16 OFFBoard_Status { get; set; }
        public float BasicSalary { get; set; }// tblemployeesalary

        //Emp_PermenanentType : 1:Permanent Day,2.Permanent Night,3.Not Permanent Employee
        public Int16 Emp_PermenanentType { get; set; }

        public Int16 Resident_Id { get; set; }
        public string PassportNo { get; set; }
        public DateTime FinExpiry { get; set; }
        public string Resident_Name { get; set; }
        public DateTime PassportExpiry { get; set; }
        public string Gradename { get; set; }
        public DateTime OFFBoard_NoticeStart { get; set; }
        public DateTime OFFBoard_NoticeEnd { get; set; }
        public Int16 RestDay_Type { get; set; }  // 0: Optional Off, 1: Fixed Off
        public Int16 RestDay_Fixed { get; set; }
        public Int64 LicenseNum { get; set; }
    }

    public class RestDays
    {
        public Int64 EmpID { get; set; }
        public Int16 RestDay_Type { get; set; }  // 0: Optional Off, 1: Fixed Off
        public Int16 RestDay_Fixed { get; set; }
    }

    public class EmpSalaryDetails
    {
        public string FixSalaType { get; set; }
        public int FixAllEmpD { get; set; }
    }

    public class EmpSalryTypeDetails
    {
        public int FixAllEmpD { get; set; }
        public string FixallEmpTypes { get; set; }
        public double FixallEmpAmounts { get; set; }
    }

    public class SalaryDetails
    {
        public List<EmpSalaryDetails> empsalarydetails { get; set; }
        public List<EmpSalryTypeDetails> empsalarytypeDetails { get; set; }
    }

    public class LeaveDetails
    {
        public string LeaveType { get; set; }
        public string LeavesText { get; set; } //Available leave
        public Int32 TotalEligible { get; set; }
        public Int32 TotalEarned { get; set; }
        public Int32 TotalTaken { get; set; }
        public Int32 TotalLeavesText { get; set; }
        public double EligibleLeaves { get; set; }
        public double EarnedLeaves { get; set; }
        public double LeavesTaken { get; set; }
        public Int16 LeavetypID { get; set; }
        public Int64 EmployeeID { get; set; }
    }

    public class ProjectItems
    {
        public Int32 OutputID { get; set; }
        public Int64 TableID { get; set; }
        public string OutName { get; set; }
    }

    public class ProjectDetails
    {
        public Int64 ProjID { get; set; }
        public string ProjName { get; set; }
        public DateTime ProjFrom { get; set; }
        public DateTime ProjTo { get; set; }
        public string ProjLocation { get; set; }
        public Int16 ProjStatus { get; set; }
        public Int32 ProjEmpno { get; set; }

        public string ShiftName { get; set; }
        public string ShiftFrom { get; set; }
        public string ShiftTo { get; set; }
        public Int32 ShiftEmpNo { get; set; }
        public string MonthNames { get; set; }

        public string ProjectType { get; set; }
        public Int32 ProjNoofShift { get; set; }

        public Int32 ProcompTypID { get; set; }
        public string ProCompType { get; set; }
        public float ProjCompAmount { get; set; }
    }

    public class ProjectmasterID
    {
        public Int64 ProjID { get; set; }
        public Int64 EmpID { get; set; }
        public Int32 ShiftID { get; set; }
        public Int32 SchID { get; set; }
    }

    public class EmployeeProfile
    {
        public Int64 EmpID { get; set; }
        public string CandName { get; set; }
        public bool Status { get; set; }
        public Int32 ShiftID { get; set; }
        public Int32 EmpTypID { get; set; }
    }

    public class Projecttype
    {
        public Int32 ProjTypID { get; set; }
        public string ProjectType { get; set; }
    }

    public class ProjectSchedule
    {
        public string Schedule { get; set; }
        public Int32 SchID { get; set; }

        public bool EmpShiftStatus { get; set; }
    }

    public class MonthsMaster
    {
        public Int32 MonthID { get; set; }
        public string MonthNames { get; set; }
    }

    public class RosterShiftList
    {
        public List<EmployeeProfile> EmployeeProfile { get; set; }

        public List<TblShift> ShiftList { get; set; }
        public List<ProjectmasterID> ProjectmasterIDs { get; set; }

        public List<RosterscheduleList> RosterscheduleList { get; set; }
        public List<ProjectSchedule> ProjectSchedule { get; set; }
    }

    public class ProjectCompleteDetails
    {
        public Int64 ProjID { get; set; }
        public string ProjName { get; set; }
        public DateTime ProjFrom { get; set; }
        public DateTime ProjTo { get; set; }
        public string ProjLocation { get; set; }
        public Int16 ProjStatus { get; set; }
        public Int32 ProjEmpno { get; set; }

        public Int32 ShiftID { get; set; }
        public string ShiftName { get; set; }
        public string ShiftFrom { get; set; }
        public string ShiftTo { get; set; }
        public Int32 ShiftEmpNo { get; set; }
        public string MonthNames { get; set; }

        public string ProjectType { get; set; }
        public Int32 ProjNoofShift { get; set; }
        public Int32 ProjTypID { get; set; }

        public Int32 ProcompTypID { get; set; }
        public float ProjCompAmount { get; set; }
    }

    public class ProjectEditList
    {
        public ProjectCompleteDetails projComplete { get; set; }

        //public List<TblShift> ShiftDetails { get; set; }
        public TblProjectGradeAssign DesignationID { get; set; }

        //public List<ProjectmasterID> ProjectmasterID { get; set; }
    }

    public class EmployeeCVDetails
    {
        public EmployeeDetails EmpDetail { get; set; }
        public List<TblEmployeHistory> EmpHistory { get; set; }
        public List<TblSkillSets> EmpSkillSet { get; set; }
        public List<TblLanguageKnown> EmpLangKnown { get; set; }
    }

    public class LeaveItems
    {
        public Int32 OutputID { get; set; }
        public Int64 TableID { get; set; }
    }

    public class LeaveApplication
    {
        public Int32 EmpLeaveappID { get; set; }
        public Int64 EmpID { get; set; }
        public string CandName { get; set; }
        public Int32 LeavetypID { get; set; }
        public string LeaveType { get; set; }
        public DateTime EmpLeaveDate { get; set; }
        public DateTime EmpLeavetodate { get; set; }
        public Int32 DayTypID { get; set; }
        public string DayType { get; set; }
        public double EmpappDays { get; set; }

        //0-Pending , 1 - Approved , 2-Rejected, 3-Authorized
        public byte EmpappStatus { get; set; }

        public string EmpappRemks { get; set; }

        public Int32 SchID { get; set; }
    }

    public class AttendanceBook
    {
        public Int32 WeekID { get; set; }
        public Int32 SchID { get; set; }
        public Int32 ShiftID { get; set; }
        public string ShiftName { get; set; }
        public Int64 EmpID { get; set; }
        public string CandName { get; set; }

        public DateTime Wsun { get; set; }
        public DateTime WMon { get; set; }
        public DateTime Wtue { get; set; }

        public DateTime Wwed { get; set; }
        public DateTime Wthu { get; set; }
        public DateTime Wfri { get; set; }

        public DateTime Wsat { get; set; }
        public string CandPhoto { get; set; }
        public string SalaWeekOff { get; set; }
        public string ShiftFrom { get; set; }
        public string ShiftTo { get; set; }
    }

    public class ScheduleListsapp
    {
        public Int32 SchID { get; set; }
        public string Schedule { get; set; }
    }

    public class WeekDates
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class DesignationAssignment
    {
        public List<TblFixecSalaryTypes> FixedSalaryType { get; set; }
        public List<TblFixedAllowanceMaster> FixedAllowanceMaster { get; set; }
        public TblSalaryMasterwise SalaryMastrWise { get; set; }
        public List<TblLeaveMasterwise> LeaveMasterwise { get; set; }
    }

    public class DeclarationDetails
    {
        public Int32 DeclTypeID { get; set; }
        public Int32 DeclID { get; set; }
        public string DeclName { get; set; }
        public string DeclContent { get; set; }
        public bool CandDeclAns { get; set; }
        public string CandDeclAnsDetail { get; set; }
    }

    public class DesignationList
    {
        public int DesigID { get; set; }
        public int DepID { get; set; }
        public string DesigName { get; set; }
        public string DepName { get; set; }
        public bool DesigStatus { get; set; }
        public Int16 DesigUserTyp { get; set; }
    }

    public class TblCandidateTabList
    {
        public int CandTabID { get; set; }
    }

    public class LeaveEmployeDetails
    {
        public int LeavetypID { get; set; }
        public string LeaveType { get; set; }
        public DateTime EmpLeaveDate { get; set; }
        public DateTime EmpLeavetodate { get; set; }
        public int EmpappDays { get; set; }
        public int DayTypID { get; set; }
        public string DayType { get; set; }
        public float DayValue { get; set; }
        public string EmpappRemks { get; set; }
    }

    public class LeaveDate
    {
        public DateTime Leavedate { get; set; }
    }

    public class RosterscheduleList
    {
        public int SchID { get; set; }
        public string Schedule { get; set; }
        public int ShiftID { get; set; }
    }

    public class Dateschedule
    {
        public DateTime Start_Week { get; set; }
        public DateTime End_Week { get; set; }
    }

    public class CandidateOfferLetterData
    {
        public string CandName { get; set; }
        public string CandAddress { get; set; }
        public string CandEmail { get; set; }
        public DateTime CandAppDate { get; set; }
        public string DesigName { get; set; }
        public string DepName { get; set; }
        public float CandOfferSalary { get; set; }
        public DateTime CandJoinDate { get; set; }
    }

    public class HolidayAttendance
    {
        public Int32 HolidayID { get; set; }
        public string HoliText { get; set; }
        public DateTime HoliDate { get; set; }
    }

    public class AttendanceMaster
    {
        public List<HolidayAttendance> HolidayAttendance { get; set; }
        public List<AttendanceBook> AttendanceBook { get; set; }
        public List<LeaveDates> LeaveEmployeDetails { get; set; }
    }

    public class GetMenuPermission
    {
        public int MenuID { get; set; }
        public int MenuParentID { get; set; }
        public string MenuName { get; set; }
        public bool MenuStatus { get; set; }
        public int cnt { get; set; }
    }

    public class AppDesignationList
    {
        public int AppDesigID { get; set; }
        public Int16 AppDepID { get; set; }
        public string AppDesigName { get; set; }
        public string AppDepName { get; set; }
    }

    public class SummaryDetails
    {
        public Int16 stats { get; set; }
        public Int32 Leavetypcount { get; set; }
        public string LeaveType { get; set; }
        public Int64 EmpID { get; set; }
        public DateTime OFFDates { get; set; }
        public double Overtime_worked { get; set; }
        public double totalhours_worked { get; set; }
        public double WorkedHolidays { get; set; }
        public Int16 RestDay_Type { get; set; }
        public Int32 restdaycount { get; set; }
        public string restdayname { get; set; }
    }



    public class SalaryAdvanceslip
    {
        public SalaryslipContent SalaryslipContent { get; set; }

        public List<TblSalaryPayComponent> SalarypayList { get; set; }
    }

    public class SalaryslipContent
    {
        public string EmpName { get; set; }
        public string NRICno { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMode { get; set; }
        public float BasicPay { get; set; }
        public float OT_hours { get; set; }
        public float RatePerHour { get; set; }
        public float OT_payment { get; set; }
        public float PH_payment { get; set; }
        public float UnPaidLeavePay { get; set; }
        public float GrossPay { get; set; }
        public float CPF_employee { get; set; }
        public float CPF_employer { get; set; }
        public float MBMF { get; set; }
        public float SINDA { get; set; }
        public float CDAC { get; set; }
        public float ECF { get; set; }
        public float Advance { get; set; }
        public float TotalContribution { get; set; }
        public float TotalDeduct { get; set; }
        public float NettPay { get; set; }
        public float HalfCutting { get; set; }
        public float Emp_levy { get; set; }
        public float Emp_cpfStatus { get; set; }
        public string FinNo { get; set; }
        public string CandBnkName { get; set; }
        public string CandBnkAcno { get; set; }

        //public DateTime DOB { get; set; }
        //public int Age { get; set; }
        //public string FundType { get; set; }
        //public float FundAmnt { get; set; }
        //public float TotalAdvance { get; set; }
        //public float FinalPay { get; set; }
    }

    public class SalaryslipContents
    {
        // public Int64 EmpID { get; set; }
        public string EmpName { get; set; }

        public string NRICno { get; set; }
        public string DOB { get; set; }
        public int Age { get; set; }
        public float BasicPay { get; set; }
        public float PH_payment { get; set; }
        public float OT_payment { get; set; }
        public float GrossPay { get; set; }
        public float CPF_employer { get; set; }
        public float CPF_employee { get; set; }
        public float Emp_levy { get; set; }
        public string FundType { get; set; }
        public float FundAmnt { get; set; }
        public float NettPay { get; set; }

        public float TotalAdvance { get; set; }
        public float FinalPay { get; set; }
    }

    public class GetAppUserIDs
    {
        public Int64 AppUID { get; set; }
    }

    public class MemosUser
    {
        public string AppFirstName { get; set; }
        public string DesigName { get; set; }
        public string MemoSubject { get; set; }
        public string MemoText { get; set; }

        public Int64 MemoAddedby { get; set; }
        public DateTime MemoAddDate { get; set; }
        public Int32 MemoID { get; set; }
        public string AppPhoto { get; set; }
    }

    public class UserProfileDetails
    {
        public TblAppUser AppUserObj { get; set; }
        public List<MemosUser> MemmouserObj { get; set; }
        public List<UserProfileWorkSchedule> UserWorkschObj { get; set; }
        public List<UserProfileLeaveDetails> UserLeaveObj { get; set; }
    }

    public class UserProfileWorkSchedule
    {
        public string ProjName { get; set; }
        public Int64 ProjID { get; set; }
        public string Schedule { get; set; }
        public string ShiftName { get; set; }
        public string ShiftFrom { get; set; }
        public string ShiftTo { get; set; }
        public Int32 ShiftID { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int SchID { get; set; }
        public string Color { get; set; }
    }

    public class UserProfileLeaveDetails
    {
        public string LeaveType { get; set; }
        public string LeavesText { get; set; }
        public string DayType { get; set; }
    }

    public class LeaveDates
    {
        public DateTime EmpLeaveDate { get; set; }
        public DateTime EmpLeavetodate { get; set; }
        public Int64 EmpID { get; set; }
        public Int32 DayTypID { get; set; }
    }

    public class AttendDate
    {
        public DateTime Attenddate { get; set; }
        public Int64 EmpID { get; set; }
        public bool IsOFFDay { get; set; }
    }

    public class DashBoardUsercount
    {
        public string CandidateCount { get; set; }
        public string EmployeeCount { get; set; }
        public string AppUsersCount { get; set; }
        public string Projectcount { get; set; }
        public string LeaveCount { get; set; }
    }

    public class DashBoardusers
    {
        public Int64 EmpID { get; set; }
        public DateTime EmpStartDate { get; set; }
        public string CandName { get; set; }
        public string CandPhoto { get; set; }
    }

    public class EmployeeValidationDocs
    {
        public Int32 DocStypID { get; set; }
        public string DocStypName { get; set; }

        public Int64 CandID { get; set; }
        public Int32 DesigID { get; set; }
        public Int32 CizenID { get; set; }
    }

    public class AbsenceAllocation
    {
        public string ProjName { get; set; }
        public string Schedule { get; set; }
        public string ShiftName { get; set; }
        public string AbsentEmployee { get; set; }
        public string AllocateEmployee { get; set; }
        public DateTime AbsDateFrom { get; set; }
        public DateTime AbsDateTo { get; set; }
    }

    public class CompanyDetails
    {
        public TblCompanyMaster ComMstr { get; set; }
        public List<TblCompanyEmail> ComEmail { get; set; }
    }

    public class EmployeeEditData
    {
        public Int64 EmpID { get; set; }
        public int EmpTypID { get; set; }
        public DateTime EmpStartDate { get; set; }
        public DateTime EmpEndDate { get; set; }
        public float BasicSalary { get; set; }
        public bool CPF_Status { get; set; } // 0: CPF only for employer, 1: For Employee & Employer
        public float EmpLevyAmnt { get; set; }
        public Int16 FundType { get; set; }
        public int SalaEmpID { get; set; }

        //---
        public Int16 Resident_Id { get; set; }

        // EmpPartTime_Pay: 1- hourly base salary, 2- daily wise base salary 0- its not part time emp
        public Int16 EmpPartTime_Pay { get; set; }

        //Emp_PermenanentType : 1:Permanent Day,2.Permanent Night,3.Not Permanent Employee
        public Int16 Emp_PermenanentType { get; set; }

        public Int64 EmpRegNo { get; set; }
    }

    public class EmpAllowanceAssignment
    {
        public TblFixecSalaryTypes FixecSalaryTypes { get; set; }
        public TblFixedAllowanceTypes FixedAllowanceTypes { get; set; }
    }

    public class CandidateDocTypeDetail
    {
        public int ValidDocTypID { get; set; }
        public string Citizen { get; set; }
        public string DesigName { get; set; }
        public string DocStypName { get; set; }
    }

    public class LeaveItemsOutput
    {
        public Int32 OutputID { get; set; }
        public double PendingDays { get; set; }
    }

    public class PassportExpiryLists
    {
        public string CandName { get; set; }
        public string CandMobile { get; set; }
        public string CandPhone { get; set; }
        public string CandAddress { get; set; }
        public string CandEmail { get; set; }
        public string Citizen { get; set; }
        public DateTime PassportExpiry { get; set; }
        public string RemainDays { get; set; }
    }

    public class PLRDExpiryLists
    {
        public string CandName { get; set; }
        public string CandMobile { get; set; }
        public string CandPhone { get; set; }
        public string CandAddress { get; set; }
        public string CandEmail { get; set; }
        public string Citizen { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string RemainDays { get; set; }
    }

    public class Time
    {
        public string AttendtimeIN { get; set; }
        public string AttendtimeOut { get; set; }
        public bool IsOFFDay { get; set; }
    }

    public class TblCandidateTabMaster
    {
        public int CandTabID { get; set; }
        public string CandTabName { get; set; }
    }

    public class MemoDocuments
    {
        public string MemoDocument { get; set; }
        public Int32 MemoID { get; set; }
    }

    public class GetMenuMasterList
    {
        public int MenuID { get; set; }
        public int MenuParentID { get; set; }
        public string MenuName { get; set; }
        public string MenuController { get; set; }
        public string MenuAction { get; set; }
        public bool MenuStatus { get; set; }
    }

    public class AllowanceListing
    {
        public string FixallEmpTypes { get; set; }
        public float FixallEmpAmounts { get; set; }
        public string FixSalaType { get; set; }
    }

    public class Employees
    {
        public Int64 EmpID { get; set; }
        public string CandName { get; set; }
        public Int16 EmpTypID { get; set; }
    }

    public class EmpBasicDetails
    {
        public string CandName { get; set; }
        public string CandNRICNo { get; set; }
        public DateTime CandDob { get; set; }
        public Int32 CandAge { get; set; }
        public Int64 EmpID { get; set; }
        public int CizenID { get; set; }
        public Int16 FundType { get; set; }
        public bool CPF_Status { get; set; }
        public float EmpLevyAmnt { get; set; }

        //
        public float BasicSalary { get; set; }
    }

    public class UpLeaveDetails
    {
        public Int64 EmpID { get; set; }
        public float UnpaidLeaves { get; set; }
    }

    public class CommonFields
    {
        public DateTime date { get; set; }
        public int NumOfHolidays { get; set; }
        public int TotDays_Month { get; set; }
        public DateTime MonthFirst { get; set; }
        public DateTime MonthLast { get; set; }
    }

    public class EmployeeSalaryDetails
    {
        public List<EmpBasicDetails> Employees { get; set; }
        public List<TblAttendance> Attendance { get; set; }
        public List<TblTimeOFF> TblTimeOFF { get; set; }
        public List<UpLeaveDetails> UpLeaves { get; set; }
        public IEnumerable<CommonFields> CommonFields { get; set; }
    }

    public class GetMoth
    {
        public Int32 Id { get; set; }
        public string Name { get; set; }
    }

    public class ScheduleEmps
    {
        public Int32 SchID { get; set; }
        public int EmpCount { get; set; }
    }

    public class ScheduleWiseEmp
    {
        public Int64 EmpID { get; set; }
        public string EmpName { get; set; }
        public string Desig { get; set; }
        public int OFFs { get; set; }
        public int Leaves { get; set; }
    }

    public class PaidUnpaidLeaves
    {
        public string LeaveType { get; set; }
        public float EmpappDays { get; set; }
    }

    public class Permission
    {
        public Int32 MenuID { get; set; }
        public string MenuName { get; set; }
        public Int32 MenuParentID { get; set; }
        public string MenuController { get; set; }
        public string MenuAction { get; set; }
        public Int32 MenuOrdNo { get; set; }
        public Int32 FunTypeID { get; set; }
        public Int64 AppUserID { get; set; }
        public bool UserPermiStatus { get; set; }
    }

    public class EmpAbsenceAllocation
    {
        public Int64 EmpLeaveappID { get; set; }
        public string CandName { get; set; }
        public string shiftName { get; set; }
        public string Schedule { get; set; }
        public string ProjName { get; set; }
        public Int64 EmpID { get; set; }
        public Int32 ShiftID { get; set; }
        public Int32 SchID { get; set; }
        public Int64 ProjID { get; set; }
        public Int64 AssignEmpID { get; set; }
        public DateTime LeaveDate { get; set; }
        public Int32 LeavetypID { get; set; }
    }

    public class EmpProject
    {
        public string ProjName { get; set; }
        public Int64 EmpID { get; set; }
    }

    public class EmpProjectEmployeeList
    {
        public string EmpName { get; set; }
        public Int64 EmpID { get; set; }
        public string EmpDesigName { get; set; }
        public Int32 DesigID { get; set; }
        public string EmpGrade { get; set; }
        public int GradeID { get; set; }
        public Int16 EmpLeaveStatus { get; set; }
        public int ShiftID { get; set; }
        public int SchID { get; set; }
        public string[] EmpOffs { get; set; }
    }

    public class ProjectLeaveEmployeDetails
    {
        public Int64 EmpID { get; set; }
        public int LeavetypID { get; set; }
        public string LeaveType { get; set; }
        public DateTime EmpLeaveDate { get; set; }
        public DateTime EmpLeavetodate { get; set; }
        public int EmpappDays { get; set; }
        public int DayTypID { get; set; }
        public string DayType { get; set; }
        public float DayValue { get; set; }
        public Int16 EmpappStatus { get; set; }
    }

    public class EmployeesRelife
    {
        public Int64 EmpID { get; set; }
        public string CandName { get; set; }
        public DateTime AbsDateFrom { get; set; }
        public Int64 AbsEmpID { get; set; }
        public Int64 AbsallEmpID { get; set; }
    }

    public class RelifeEmpDetails
    {
        public List<ProjectLeaveEmployeDetails> PrjLeveobj { get; set; }
        public List<ReliefEmpConfirmList> PrjEmpRelifeObj { get; set; }
        public List<TblAbscenceAllocate> Absencealloobj { get; set; }
    }

    public class ListProjectWiseEmp
    {
        public Int64 EmpID { get; set; }
        public Int64 ProjID { get; set; }
        public string CandName { get; set; }
        public string Gradename { get; set; }
        public string ShiftName { get; set; }
        public string ShiftFrom { get; set; }
        public string ShiftTo { get; set; }
        public int GradeID { get; set; }

        public int schID { get; set; }
    }

    public class ProjectShiftGradeList
    {
        public List<TblShift> ShiftObj { get; set; }
        public List<ProjectGradeAssignList> PrjgrdAssignobj { get; set; }
    }

    public class ProjectGradeAssignList
    {
        public int PrjGrdAssignID { get; set; }
        public Int64 ProjID { get; set; }
        public int DesigID { get; set; }
        public int GradeID { get; set; }
        public string DesigName { get; set; }
        public string Gradename { get; set; }
        public int PrjGradeEmpNO { get; set; }

        public string ShiftName { get; set; }
    }

    public class ProjectComponentList
    {
        public List<TblProject> ProjObj { get; set; }
        public List<ListProjectWiseEmp> ProjList { get; set; }
        public List<ReliefEmpConfirmList> EmpReliefConfirm { get; set; }
        public List<SelectedReliefEmp> EmpAbsnceAllcateobj { get; set; }
        public List<TblAttendance> GetAttendanceobj { get; set; }

        public List<TblEmployeeLeaveApp> GetEmployeeleaves { get; set; }
    }

    public class ReliefEmpConfirmList
    {
        public Int64 EmpID { get; set; }
        public string CandName { get; set; }
        public int EmpTypID { get; set; }
        public string EmpTypName { get; set; }
        public string CandMobile { get; set; }
        public int GradeID { get; set; }
    }

    public class SelectedReliefEmp
    {
        public Int64 AbsEmpID { get; set; }
        public Int64 AbsallEmpID { get; set; }
        public string CandName { get; set; }
        public Int64 ProjID { get; set; }
        public string Gradename { get; set; }
        public bool ReliefConfirmation { get; set; }
        public string CandMobile { get; set; }
        public string EmpTypName { get; set; }

    }

    public class ScheduleDropList
    {
        public int SchID { get; set; }
        public string Schedule { get; set; }
        public int MonthID { get; set; }
        public Int64 ProjID { get; set; }
        public int ShiftID { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }

    public class EditProjectSchedule
    {
        public int ShiftID { get; set; }
        public string Shift { get; set; }
        public int SchID { get; set; }
        public string Schedule { get; set; }
        public Int64 EmpID { get; set; }
        public string EmpName { get; set; }
        public int DesigID { get; set; }
        public string SchFromDate { get; set; }
        public string SchToDate { get; set; }
        public string EmpDesigName { get; set; }
        public int GradeID { get; set; }
        public string EmpGrade { get; set; }
        public int EmpLeaveStatus { get; set; }
        public int OffStatus { get; set; }
    }

    public class projectCalendar
    {
        public List<TblShift> ShiftObj { get; set; }
        public List<TblProject> ProjectObj { get; set; }
        public List<Tblschedule> ScheduleObj { get; set; }
        public List<ListProjectWiseEmp> ListProjectWiseEmp { get; set; }
    }

    public class OFFDetails
    {
        public Int64 EmpID { get; set; }
        public DateTime OFFDates { get; set; }
        public string Date { get; set; }
    }

    public class OffboardList
    {
        public Int64 CheckListID { get; set; }
        public Int16 CheckListTypeID { get; set; }
        public int Quantity { get; set; }

        public string CheckListDetails { get; set; }
    }

    public class TblMemosUserList
    {
        public Int32 MemoID { get; set; }
        public string MemoSubject { get; set; }
        public string MemoText { get; set; }
        public Int32 MemoPriority { get; set; }
        public bool MemoStatus { get; set; }
        public Int64 MemoAddedby { get; set; }
        public DateTime MemoAddDate { get; set; }
        public DateTime ReadDate { get; set; }
        public Int32 MemoRepID { get; set; }
        public Int32 memoUserID { get; set; }
        public Int64 AppUID { get; set; }
        public string AppFirstName { get; set; }
        public bool ReadStatus { get; set; }
    }

    public class calenderSummaryDetails
    {
        public Int64 id { get; set; }
        public string title { get; set; }
        public string start { get; set; }
        public string enddt { get; set; }
        public string color { get; set; }
    }
    public class TblSalaryPayComponent
    {
        public Int64 PayId { get; set; }
        public Int64 EmpId { get; set; }
        public Int32 SalYear { get; set; }
        public Int32 SalMonth { get; set; }
        public float PayAmount { get; set; }
        public Int16 PayType { get; set; }
        public string PayDate { get; set; }
        public Int16 PayMode { get; set; }
    }

}