using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace XoneHR.Models.BAL
{
    public class CandidateApplication : Controller
    {
        private XoneDbLayer db;
        private int flagid = 0;
        private CommonFunctions common;

        public CandidateApplication()
        {
            db = new XoneDbLayer();
            common = new CommonFunctions();
        }

        public CandidateItems AddNewCandidateForm(TblCandidate tblCandobj, DataTable DT1, DataTable DT2, string IssueDate = null, string ExpiryDate = null, string LicenseNum = null)
        {
            try
            {
                DynamicParameters paraObj = new DynamicParameters();
                paraObj.Add("@CandID", tblCandobj.CandID);
                paraObj.Add("@CandNo", tblCandobj.CandNo);
                paraObj.Add("@CizenID", tblCandobj.CizenID);
                paraObj.Add("@CandName", tblCandobj.CandName);
                paraObj.Add("@CandMobile", tblCandobj.CandMobile);
                paraObj.Add("@CandPhone", tblCandobj.CandPhone);
                paraObj.Add("@CandAddress", tblCandobj.CandAddress);
                paraObj.Add("@CandEmail", tblCandobj.CandEmail);
                paraObj.Add("@CandNRICNo", tblCandobj.CandNRICNo);
                paraObj.Add("@DesigID", tblCandobj.DesigID);
                paraObj.Add("@CandDob", tblCandobj.CandDob);
                paraObj.Add("@CandAge", tblCandobj.CandAge);
                paraObj.Add("@CandRegDate", tblCandobj.CandRegDate);
                paraObj.Add("@CandGender", tblCandobj.CandGender);
                paraObj.Add("@CandPlaceofBirth", tblCandobj.CandPlaceofBirth);
                paraObj.Add("@RaceID", tblCandobj.RaceID);
                paraObj.Add("@ReligID", tblCandobj.ReligID);
                paraObj.Add("@MaritID", tblCandobj.MaritID);
                paraObj.Add("@CandPhoto", tblCandobj.CandPhoto);
                paraObj.Add("@CandSalrexpted", tblCandobj.CandSalrexpted);
                paraObj.Add("@CandHgEducation", tblCandobj.CandHgEducation);
                paraObj.Add("@CandDetails", tblCandobj.CandDetails);
                paraObj.Add("@FinNo", tblCandobj.FinNo);

                paraObj.Add("@GradeId", tblCandobj.GradeID);

                //if (tblCandobj.CizenID == 2)
                //{ tblCandobj.PassportExpiry = Convert.ToDateTime("1/1/2000 12:00:00 AM"); }
                if (tblCandobj.Resident_Id == 3 || tblCandobj.Resident_Id == 4)
                {
                    paraObj.Add("@PassportExpiry", tblCandobj.PassportExpiry);
                    paraObj.Add("@FinExpiry", tblCandobj.FinExpiry);
                    paraObj.Add("@PassportNo", tblCandobj.PassportNO_WorkPermit);
                }
                else
                    paraObj.Add("@PassportNo", tblCandobj.PassportNo_Sig_Citizen_PR);

                paraObj.Add("@Resident_Id", tblCandobj.Resident_Id);

                //PLRD Details
                if (IssueDate != null && IssueDate != "" && IssueDate != "01-01-0001")
                    paraObj.Add("@IssueDate", common.CommonDateConvertion(IssueDate));

                if (ExpiryDate != null && ExpiryDate != "" && ExpiryDate != "01-01-0001")
                    paraObj.Add("@ExpiryDate", common.CommonDateConvertion(ExpiryDate));

                paraObj.Add("@LicenseNum", LicenseNum);

                paraObj.Add("@CandidateID", null, DbType.Int64, ParameterDirection.Output);
                paraObj.Add("@ErrorOutput", null, DbType.Int32, ParameterDirection.Output);

                int Result = db.DapperExecute("USP_AddCandidate", paraObj, CommandType.StoredProcedure);

                CandidateItems CandObj = new CandidateItems();
                CandObj.TableID = paraObj.Get<Int64>("CandidateID");
                CandObj.OutputID = paraObj.Get<Int32>("ErrorOutput");

                if (tblCandobj.CandID > 0)
                {
                    flagid = 2;
                }
                else
                {
                    flagid = 1;
                }

                SqlParameter[] param = {
                                       new SqlParameter("@datatable_skills",SqlDbType.Structured){ Value=DT1 },
                                       new SqlParameter("@datatable_langknown",SqlDbType.Structured){ Value=DT2 },
                                       new SqlParameter("@Out",SqlDbType.BigInt){ Direction=ParameterDirection.Output },
                                       new SqlParameter("@CandidateID",SqlDbType.BigInt){ Value=CandObj.TableID },
                                       new SqlParameter("@flagid",SqlDbType.Int){ Value = flagid }
                                   };

                int result = db.ExecuteWithDataTable("USP_SaveCadidate_Lang_Skill", param, CommandType.StoredProcedure);
                string output = param[2].Value.ToString();

                return CandObj;
            }
            catch
            {
                return null;
            }
        }

        public int ManageReligion(int flag, string ReligionName = "", Int32 status = 0, Int16 ReligID = 0)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@ReligionName", ReligionName.Trim());
            param.Add("@flag", flag);
            param.Add("@ReligID", ReligID);
            param.Add("@Status", Convert.ToBoolean(status));
            param.Add("@OUT", 1, DbType.Int32, ParameterDirection.Output);

            db.DapperExecute("USP_ManageReligion", param, CommandType.StoredProcedure);
            Int32 output = param.Get<Int32>("OUT");
            return output;
        }

        public CandidateItems AddNewCitizendetails(TblCitizenDetails tblCitizenObj, int HidTab1 = 0)
        {
            try
            {
                DynamicParameters ParamObj = new DynamicParameters();
                ParamObj.Add("@CandID", tblCitizenObj.CandID);
                ParamObj.Add("@CitzOrdrDate", tblCitizenObj.CitzOrdrDate);
                ParamObj.Add("@CitzOrdrDateOne", null);
                ParamObj.Add("@CitzNsVaction", tblCitizenObj.CitzNsVaction);
                ParamObj.Add("@CitzReglrVaction", tblCitizenObj.CitzReglrVaction);
                ParamObj.Add("@CitzReservist", tblCitizenObj.CitzReservist);
                ParamObj.Add("@CitzDetID", tblCitizenObj.CitzDetID);
                ParamObj.Add("@CitizenDetailsID", null, DbType.Int32, ParameterDirection.Output);
                ParamObj.Add("@ErrorOutput", null, DbType.Int32, ParameterDirection.Output);
                //  ParamObj.Add("@HidTab1", HidTab1);

                int Result = db.DapperExecute("USP_AddCitizenDetails", ParamObj, CommandType.StoredProcedure);

                CandidateItems CandObj = new CandidateItems();
                CandObj.TableID = ParamObj.Get<Int32>("CitizenDetailsID");
                CandObj.OutputID = ParamObj.Get<Int32>("ErrorOutput");
                return CandObj;
            }
            catch
            {
                return null;
            }
        }

        public CandidateItems AddNewEmployeeHistory(TblEmployeHistory tblEmphistObj, int HidTab3)
        {
            try
            {
                DynamicParameters paramObj = new DynamicParameters();
                paramObj.Add("@CandID", tblEmphistObj.CandID);
                paramObj.Add("@EmphEmployer", tblEmphistObj.EmphEmployer);
                paramObj.Add("@EmphStartDate", tblEmphistObj.EmphStartDate);
                paramObj.Add("@EmphEndDate", tblEmphistObj.EmphEndDate);
                paramObj.Add("@EmphExpYr", tblEmphistObj.EmphExpYr);
                paramObj.Add("@EmphSalryLast", tblEmphistObj.EmphSalryLast);
                paramObj.Add("@EmpHistoryID", tblEmphistObj.EmphID);
                paramObj.Add("@HidTab3", HidTab3);
                paramObj.Add("@EmphID", null, DbType.Int32, ParameterDirection.Output);
                paramObj.Add("@ErrorOutput", null, DbType.Int32, ParameterDirection.Output);

                int Result = db.DapperExecute("USP_AddEmpHistory", paramObj, CommandType.StoredProcedure);

                CandidateItems CandObj = new CandidateItems();
                CandObj.TableID = paramObj.Get<Int32>("EmphID");
                CandObj.OutputID = paramObj.Get<Int32>("ErrorOutput");
                return CandObj;
            }
            catch
            {
                return null;
            }
        }

        public CandidateItems AddNewEmergencyContact(TblEmergencyContact tblemergencyContact)
        {
            try
            {
                DynamicParameters paramObj = new DynamicParameters();
                paramObj.Add("@CandID", tblemergencyContact.CandID);
                paramObj.Add("@EmrgContact", tblemergencyContact.EmrgContact);
                paramObj.Add("@EmrgRelationship", tblemergencyContact.EmrgRelationship);
                paramObj.Add("@EmrgContactNo", tblemergencyContact.EmrgContactNo);
                paramObj.Add("@EmrgDetails", tblemergencyContact.EmrgDetails);
                paramObj.Add("@EmrgStatus", tblemergencyContact.EmrgStatus);

                paramObj.Add("@ErrorOutput", null, DbType.Int32, ParameterDirection.Output);
                int Result = db.DapperExecute("USP_AddEmrgContacts", paramObj, CommandType.StoredProcedure);

                CandidateItems CandObj = new CandidateItems();
                CandObj.TableID = 0;
                CandObj.OutputID = paramObj.Get<Int32>("ErrorOutput");
                return CandObj;
            }
            catch
            {
                return null;
            }
        }

        public CandidateItems AddBankDetails(TblCandidateBank tblCandbankObj, int HidTab5)
        {
            try
            {
                DynamicParameters paraObj = new DynamicParameters();
                paraObj.Add("@CandID", tblCandbankObj.CandID);
                paraObj.Add("@CandBnkName", tblCandbankObj.CandBnkName);
                paraObj.Add("@CandBnkCode", tblCandbankObj.CandBnkCode);
                paraObj.Add("@CandbrnchCode", tblCandbankObj.CandbrnchCode);
                paraObj.Add("@CandBnkAcno", tblCandbankObj.CandBnkAcno);
                paraObj.Add("@CandBnkID", tblCandbankObj.CandBnkID);
                paraObj.Add("@HidTab5", HidTab5);
                paraObj.Add("@ErrorOutput", null, DbType.Int32, ParameterDirection.Output);
                int Result = db.DapperExecute("USP_AddCandidateBankDetails", paraObj, CommandType.StoredProcedure);

                CandidateItems CandObj = new CandidateItems();
                CandObj.TableID = 0;
                CandObj.OutputID = paraObj.Get<Int32>("ErrorOutput");
                return CandObj;
            }
            catch
            {
                return null;
            }
        }

        public CandidateItems AddDocuments(TblDocuments tblDocumObj, Int32 HidTab2)
        {
            try
            {
                DynamicParameters paraObj = new DynamicParameters();
                paraObj.Add("@DocStypID", tblDocumObj.DocStypID);
                paraObj.Add("@CandID", tblDocumObj.CandID);
                paraObj.Add("@DocFiles", tblDocumObj.DocFiles);
                paraObj.Add("@HidTab2", HidTab2);

                paraObj.Add("@ErrorOutput", null, DbType.Int32, ParameterDirection.Output);
                int Result = db.DapperExecute("USP_AddDocument", paraObj, CommandType.StoredProcedure);

                CandidateItems CandObj = new CandidateItems();
                CandObj.TableID = 0;
                CandObj.OutputID = paraObj.Get<Int32>("ErrorOutput");
                return CandObj;
            }
            catch
            {
                return null;
            }
        }

        public CandidateItems CandidateApproval(Int64 CandID, int AppStatus, string Remarks=null)
        {
            try
            {
                DynamicParameters ParaObj = new DynamicParameters();
                ParaObj.Add("@CandID", CandID);
                ParaObj.Add("@CandAppStatus", AppStatus);
                ParaObj.Add("@CandRemark", Remarks);

                ParaObj.Add("@ErrorOutput", null, DbType.Int32, ParameterDirection.Output);
                int Result = db.DapperExecute("USP_ApprovalCandidate", ParaObj, CommandType.StoredProcedure);

                CandidateItems CandObj = new CandidateItems();
                CandObj.TableID = 0;
                CandObj.OutputID = ParaObj.Get<Int32>("ErrorOutput");
                return CandObj;
            }
            catch
            {
                return null;
            }
        }

        public CandidateDesigItems candidateAppoinment(CandidateAppoinment CandAppoinObj, bool CPF, double Levy, string SalaWeekOff, string EndDate = null)
        {
            try
            {
                DynamicParameters paraObj = new DynamicParameters();
                paraObj.Add("@CandID", CandAppoinObj.CandID);
                paraObj.Add("@Startdate", CandAppoinObj.StartDate);
                if (EndDate == "" || EndDate == null)
                {
                    paraObj.Add("@Enddate", null);
                }
                else
                {
                    paraObj.Add("@Enddate", CandAppoinObj.EndDate);
                }
                paraObj.Add("@Employeetype", CandAppoinObj.Employeetyp);
                paraObj.Add("@EmploymntType", CandAppoinObj.EmployeeType);
                paraObj.Add("@BasicSalary", CandAppoinObj.BasicSalary);
                paraObj.Add("@SalaWeekOff", SalaWeekOff);
                paraObj.Add("@Emp_IsApproved", CandAppoinObj.Emp_IsApproved);
                paraObj.Add("@EmployeeID", null, DbType.Int64, ParameterDirection.Output);
                paraObj.Add("@ErrorOutput", null, DbType.Int32, ParameterDirection.Output);
                paraObj.Add("@DesignationID", null, DbType.Int32, ParameterDirection.Output);
                paraObj.Add("@EmpSubTypeID", CandAppoinObj.EmpSubTypeID);
                paraObj.Add("@EmpRegNo", CandAppoinObj.EmpRegNo);
                paraObj.Add("@RestDay_Type", CandAppoinObj.RestDay_Type);
                if (CandAppoinObj.RestDay_Type == 1)
                    paraObj.Add("@RestDay_Fixed", CandAppoinObj.RestDay_Fixed);
                paraObj.Add("@EmpPartTime_Pay", CandAppoinObj.EmpPartTime_Pay);

                int Result = db.DapperExecute("USP_AddEmployee", paraObj, CommandType.StoredProcedure);

                CandidateDesigItems CandObj = new CandidateDesigItems();
                CandObj.TableID = paraObj.Get<Int64>("EmployeeID");
                CandObj.OutputID = paraObj.Get<Int32>("ErrorOutput");
                CandObj.DesignationID = paraObj.Get<Int32>("DesignationID");

                //SqlParameter[] param = {
                //                       new SqlParameter("@datatable_funds",SqlDbType.Structured){ Value=Dt },
                //                       new SqlParameter("@ErrorOutput",SqlDbType.Int){ Direction=ParameterDirection.Output },
                //                       new SqlParameter("@EmpId",SqlDbType.BigInt){ Value=CandObj.TableID },
                //                       new SqlParameter("@CPF",SqlDbType.Bit){ Value=CPF },
                //                       new SqlParameter("@Levy",SqlDbType.Int){ Value=Levy }
                //                   };
                //int result = db.ExecuteWithDataTable("USP_UpdateemployeeFundTypes", param, CommandType.StoredProcedure);
                DynamicParameters param = new DynamicParameters();
                param.Add("@CPF", CPF);
                param.Add("@Levy", Levy);
                param.Add("@EmpId", CandObj.TableID);
                param.Add("@ErrorOutput", 1, DbType.Int32, ParameterDirection.Output);
                param.Add("@FundType", CandAppoinObj.FundType);

                int result = db.DapperExecute("USP_UpdateemployeeFundTypes", param, CommandType.StoredProcedure);
                return CandObj;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //public void UpdateRestDayFixed(DataTable dt1,Int64 EmpID)
        //{
        //    SqlParameter[] param = {
        //                               new SqlParameter("@datatable_RestDays",SqlDbType.Structured){ Value=dt1 },
        //                               new SqlParameter("@Out",SqlDbType.Int){ Direction=ParameterDirection.Output },
        //                               new SqlParameter("@EmployeeId",SqlDbType.BigInt){ Value= EmpID}
        //                           };
        //    int result = db.ExecuteWithDataTable("USP_UpdateRestDay", param, CommandType.StoredProcedure);
        //}

        //public void UpdateRestOptional(Int16 RestDay_Optional,Int64 EmpID)
        //{
        //    DynamicParameters para = new DynamicParameters();
        //    para.Add("@RestDay_Optional", RestDay_Optional);
        //    para.Add("@EmpID", EmpID);
        //    db.DapperExecute("Insert into TblRestDay(Emp_ID,RestDay) values(@EmpID,@RestDay_Optional)", para);
        //}

        public int EditcandidateAppoinment(CandidateAppoinment CandAppoinObj, bool CPF, double Levy, string EndDate)
        {
            try
            {
                DynamicParameters paraObj = new DynamicParameters();
                paraObj.Add("@CandID", CandAppoinObj.CandID);
                paraObj.Add("@EmpID", CandAppoinObj.EmpID);
                paraObj.Add("@Startdate", CandAppoinObj.StartDate);
                paraObj.Add("@EmpTypID", CandAppoinObj.Employeetyp);
                paraObj.Add("@BasicSalary", CandAppoinObj.BasicSalary);
                paraObj.Add("@Emp_IsApproved", CandAppoinObj.Emp_IsApproved);
                paraObj.Add("@EmpSubTypeID", CandAppoinObj.EmpSubTypeID);
                paraObj.Add("@EmpPartTime_Pay", CandAppoinObj.EmpPartTime_Pay);
                paraObj.Add("@CPF", CPF);
                paraObj.Add("@EmpRegNo", CandAppoinObj.EmpRegNo);

                if (EndDate.ToString() == "01/01/0001 12:00:00 AM" || EndDate == "")
                {
                    paraObj.Add("@Enddate", null);
                }
                else
                {
                    paraObj.Add("@Enddate", CandAppoinObj.EndDate);
                }
                string count = db.DapperSingle("Select count(*) from TblEmployee where EmpRegNo=@EmpRegNo and EmpID!=@EmpID", paraObj);
                if (Convert.ToInt16(count) == 0)
                {
                    var iresult = db.DapperExecute("update TblEmployee set EmpStartDate=@Startdate,EmpEndDate=@Enddate,EmpTypID=@EmpTypID,Emp_IsApproved=@Emp_IsApproved,CPF_Status=@CPF,EmpSubTypeID=@EmpSubTypeID,EmpPartTime_Pay=@EmpPartTime_Pay,EmpRegNo=@EmpRegNo where CandID=@CandID", paraObj);
                    var iresult1 = db.DapperExecute("update TblSalaryEmployeeWise set SalaEmpbasicSalary=@BasicSalary where EmpID=@EmpID", paraObj);

                    CandidateDesigItems CandObj = new CandidateDesigItems();
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@CPF", CPF);
                    param.Add("@Levy", Levy);
                    param.Add("@EmpId", CandAppoinObj.EmpID);
                    param.Add("@ErrorOutput", 1, DbType.Int32, ParameterDirection.Output);
                    param.Add("@FundType", CandAppoinObj.FundType);

                    int result = db.DapperExecute("USP_UpdateemployeeFundTypes", param, CommandType.StoredProcedure);
                    return 1;
                }
                else
                    return 2;
            }
            catch
            {
                return 0;
            }
        }

        public CandidateItems AddfixedAllowanceDesig(TblFixedAllowanceEmpwise AllowanceObj)
        {
            try
            {
                DynamicParameters paraObj = new DynamicParameters();
                paraObj.Add("@CodingNo", 1);
                paraObj.Add("@FixSalaTypID", AllowanceObj.FixSalaTypID);
                paraObj.Add("@EmpID", AllowanceObj.EmpID);

                paraObj.Add("@FixallowID", null, DbType.Int32, ParameterDirection.Output);
                paraObj.Add("@ErrorOutput", null, DbType.Int32, ParameterDirection.Output);

                int Result = db.DapperExecute("USP_AddAllowances", paraObj, CommandType.StoredProcedure);

                CandidateItems CandObj = new CandidateItems();
                CandObj.TableID = paraObj.Get<Int32>("FixallowID");
                CandObj.OutputID = paraObj.Get<Int32>("ErrorOutput");

                return CandObj;
            }
            catch
            {
                return null;
            }
        }

        public CandidateItems AddfixedAllowanceTypes(TblFixedAllowanceEmpTypes AllowanceObj)
        {
            try
            {
                DynamicParameters paraObj = new DynamicParameters();
                paraObj.Add("@CodingNo", 2);
                paraObj.Add("@FixallEmpTypes", AllowanceObj.FixallEmpTypes);
                paraObj.Add("@FixallEmpAmounts", AllowanceObj.FixallEmpAmounts);
                paraObj.Add("@FixAllEmpD", AllowanceObj.FixAllEmpD);

                paraObj.Add("@FixallowID", null, DbType.Int32, ParameterDirection.Output);
                paraObj.Add("@ErrorOutput", null, DbType.Int32, ParameterDirection.Output);

                int Result = db.DapperExecute("USP_AddAllowances", paraObj, CommandType.StoredProcedure);

                CandidateItems CandObj = new CandidateItems();
                CandObj.TableID = paraObj.Get<Int32>("FixallowID");
                CandObj.OutputID = paraObj.Get<Int32>("ErrorOutput");

                return CandObj;
            }
            catch
            {
                return null;
            }
        }

        public CandidateItems UploadPhoto(Int64 CandID, string Photopath)
        {
            try
            {
                DynamicParameters ParaObj = new DynamicParameters();
                ParaObj.Add("@CandID", CandID);
                ParaObj.Add("@PhotoPath", Photopath);

                ParaObj.Add("@ErrorOutput", null, DbType.Int32, ParameterDirection.Output);

                int Result = db.DapperExecute("USP_UploadPhotocandidate", ParaObj, CommandType.StoredProcedure);

                CandidateItems CandObj = new CandidateItems();
                CandObj.OutputID = ParaObj.Get<Int32>("ErrorOutput");

                return CandObj;
            }
            catch
            {
                return null;
            }
        }

        public List<TblDeclaration> GetDeclaration(int declarationCode)
        {
            try
            {
                DynamicParameters paraObj = new DynamicParameters();
                paraObj.Add("@DeclarationCode", declarationCode);

                List<TblDeclaration> ListDeclaObj = db.DapperToList<TblDeclaration>("USP_GetDeclartions", paraObj, CommandType.StoredProcedure);
                return ListDeclaObj;
            }
            catch
            {
                return null;
            }
        }

        public CandidateDetails ListCandidateDetails(Int64 CandID, int flag)//flag: 0-from candidate, 1-from employee
        {
            try
            {
                DynamicParameters ParaObj = new DynamicParameters();
                ParaObj.Add("@CandID", CandID);
                ParaObj.Add("@flag", flag);

                var CandidatelistObj = db.DapperFirst<CandidateDetails>("USP_ListCandidate", ParaObj, CommandType.StoredProcedure);
                return CandidatelistObj;
            }
            catch
            {
                return null;
            }
        }

        public bool GetAppStatus(Int64 CandID)
        {
            try
            {
                DynamicParameters para = new DynamicParameters();
                para.Add("@CandID", CandID);
                string status = db.DapperSingle("Select count(*) from TblEmployee where CandID=@CandID and Emp_IsApproved=0", para);
                return Convert.ToBoolean(Convert.ToInt16(status));
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public EmployeeDetails ListCandidateEmpDetails(Int64 CandID)
        {
            try
            {
                DynamicParameters para = new DynamicParameters();
                para.Add("@CandID", CandID);
                var list = db.DapperFirst<EmployeeDetails>("select emp.EmpTypID,EmpTypName,fund.FundType,emp.EmpStartDate,EmpEndDate from TblEmployee emp " +
                            "join TblEmployeeType emptype on emp.EmpTypID=emptype.EmpTypID left join TblFundTypeMaster fund on emp.FundType = fund.FundTypeID " +
                            "where  emp.CandID = @CandID", para);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public double GetBasicSal(Int64 CandID)
        {
            DynamicParameters para = new DynamicParameters();
            para.Add("@CandID", CandID);
            string sal = db.DapperSingle("Select top 1 BasicSalary from TblEmployeeSalary where empid=(select EmpID from TblEmployee where CandID=@CandID) order by SalMastID desc", para);
            return Convert.ToDouble(sal);
        }

        public List<DocumentDetails> ListDocuments(TblDocuments tbldocuobj, int SearchID)
        {
            try
            {
                DynamicParameters paraObj = new DynamicParameters();
                paraObj.Add("@CandID", tbldocuobj.CandID);
                paraObj.Add("@DocStypID", tbldocuobj.DocStypID);
                paraObj.Add("@SearchID", SearchID);

                List<DocumentDetails> ListDocuments = db.DapperToList<DocumentDetails>("USP_ListDocuments", paraObj, CommandType.StoredProcedure);
                return ListDocuments;
            }
            catch
            {
                return null;
            }
        }

        public List<TblEmployeHistory> ListEmpHistory(Int64 CandID, string TableID)
        {
            try
            {
                if (TableID == "" || TableID == null)
                {
                    DynamicParameters paraObj = new DynamicParameters();
                    paraObj.Add("@EmphID", 0);
                    paraObj.Add("@CandID", CandID);

                    List<TblEmployeHistory> ListEmpHistory = db.DapperToList<TblEmployeHistory>("USP_ListEmphistory", paraObj, CommandType.StoredProcedure);
                    return ListEmpHistory;
                }
                else
                {
                    Int64 ID = Convert.ToInt64(TableID);

                    DynamicParameters paraObj = new DynamicParameters();
                    paraObj.Add("@EmphID", ID);
                    paraObj.Add("@CandID", CandID);

                    List<TblEmployeHistory> ListEmpHistory = db.DapperToList<TblEmployeHistory>("USP_ListEmphistory", paraObj, CommandType.StoredProcedure);
                    return ListEmpHistory;
                }
            }
            catch
            {
                return null;
            }
        }

        public List<TblEmployeHistory> EditListEmpHistory(Int64 CandID, string EmphID)
        {
            try
            {
                DynamicParameters paraObj = new DynamicParameters();

                if (EmphID == "" || EmphID == null)
                {
                    paraObj.Add("@CandID", CandID);
                    List<TblEmployeHistory> ListEmpHistory = db.DapperToList<TblEmployeHistory>("select * from TblEmployeHistory where CandID=@CandID", paraObj);
                    return ListEmpHistory;
                }
                else
                {
                    int EMPDetailsid = Convert.ToInt32(EmphID);
                    paraObj.Add("@EMPDetailsid", EMPDetailsid);
                    List<TblEmployeHistory> ListEmpHistory = db.DapperToList<TblEmployeHistory>("select * from TblEmployeHistory where EmphID=@EMPDetailsid", paraObj);
                    return ListEmpHistory;
                }
            }
            catch
            {
                return null;
            }
        }

        public List<CandidateDetails> ListCandidateDatatables(Int64 CandID, int StatusID, int CodingID)
        {
            try
            {
                DynamicParameters ParaObj = new DynamicParameters();
                ParaObj.Add("@CandID", CandID);
                ParaObj.Add("@Status", StatusID);
                ParaObj.Add("@codingStatus", CodingID);

                List<CandidateDetails> CandidatelistObj = db.DapperToList<CandidateDetails>("USP_ListCandidateDataTables", ParaObj, CommandType.StoredProcedure);
                return CandidatelistObj;
            }
            catch
            {
                return null;
            }
        }

        public List<TblDocumentTypes> ComboListDocumentType()
        {
            try
            {
                var ListDocumntType = db.DapperToList<TblDocumentTypes>("USP_ComboListDocumentType", CommandType.StoredProcedure);
                return ListDocumntType;
            }
            catch
            {
                return null;
            }
        }

        public List<TblDocumentSubTypes> ComboListDocumentSubType(int DocTypeID)
        {
            try
            {
                DynamicParameters paraObj = new DynamicParameters();
                paraObj.Add("@DocTypID", DocTypeID);

                var ListDocumntSubType = db.DapperToList<TblDocumentSubTypes>("USP_ComboListDocumentSubType", paraObj, CommandType.StoredProcedure);
                return ListDocumntSubType;
            }
            catch
            {
                return null;
            }
        }

        public List<TblRace> ComboListRace()
        {
            try
            {
                var ListRace = db.DapperToList<TblRace>("USP_ComboListRace", CommandType.StoredProcedure);
                return ListRace;
            }
            catch
            {
                return null;
            }
        }

        public List<TblReligion> ComboListReligion()
        {
            try
            {
                var ListReligion = db.DapperToList<TblReligion>("USP_ComboListReligion", CommandType.StoredProcedure);
                return ListReligion;
            }
            catch
            {
                return null;
            }
        }

        public List<TblMaritalStatus> ComboMaritalStatus()
        {
            try
            {
                var ListMaritalStatus = db.DapperToList<TblMaritalStatus>("USP_ComboListMaritalStatus", CommandType.StoredProcedure);
                return ListMaritalStatus;
            }
            catch
            {
                return null;
            }
        }

        public List<TblCityZenship> ComboListCitizenship()
        {
            try
            {
                var ListCitizenship = db.DapperToList<TblCityZenship>("USP_ComboListCitizenship", CommandType.StoredProcedure);
                return ListCitizenship;
            }
            catch
            {
                return null;
            }
        }

        public List<TblDesignation> CombolistDesignation()
        {
            try
            {
                //DynamicParameters para = new DynamicParameters();
                //para.Add("@UserType", UserType);
                var ListDesignation = db.DapperToList<TblDesignation>("USP_ComboListDesignations", CommandType.StoredProcedure);
                return ListDesignation;
            }
            catch
            {
                return null;
            }
        }

        public List<TblGrade> CombolistGrade(int desigid)
        {
            try
            {
                DynamicParameters para = new DynamicParameters();
                para.Add("@desigid", desigid);
                var ListGrade = db.DapperToList<TblGrade>("select GradeID,Gradename from TblGrade where DesigID=@desigid and GradeStatus=1", para);
                return ListGrade;
            }
            catch
            {
                return null;
            }
        }

        public List<FixedAllowanceDesig> ListFixedAllowanceDesigwise(int DesignationID, int CodingNo, int fixAlloDesig)
        {
            try
            {
                DynamicParameters paraobj = new DynamicParameters();
                paraobj.Add("@DesigId", DesignationID);
                paraobj.Add("@CodingNo", CodingNo);
                paraobj.Add("@FixAllDesigID", fixAlloDesig);

                List<FixedAllowanceDesig> FixedallowanceObj = db.DapperToList<FixedAllowanceDesig>("USP_ListAllowanceDetails", paraobj, CommandType.StoredProcedure);
                return FixedallowanceObj;
            }
            catch
            {
                return null;
            }
        }

        public TblCityZenship GetCitizenDetails(Int16 CitizenID)
        {
            try
            {
                DynamicParameters paraObj = new DynamicParameters();
                paraObj.Add("@CitizenID", CitizenID);

                return db.DapperFirst<TblCityZenship>("USP_GetCitizenDetails", paraObj, CommandType.StoredProcedure);
            }
            catch
            {
                return null;
            }
        }

        public List<TblQuestions> GetInertviewQuestion()
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@CandID", SessionManage.Current.CandID);
                var LisInterviewQns = db.DapperToList<TblQuestions>("select * from TblQuestions", param);
                //var LisInterviewQns = db.DapperToList<TblQuestions>("select * from TblQuestions where QuestID != (select a.QuestID from TblQuestions a join TblCandidateAnswers b on a.QuestID=b.QuestID and QtypeID=1 and b.CandID=@CandID)");
                return LisInterviewQns;
            }
            catch
            {
                return null;
            }
        }

        public List<TblCandidateAnswers> GetAnswers()
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@CandID", SessionManage.Current.CandID);
            return db.DapperToList<TblCandidateAnswers>("Select *from TblCandidateAnswers where CandID=@CandID", param);
        }

        public TblCandidateAnswers AddInterviewData(TblCandidateAnswers tblCandidateAnsobj)
        {
            try
            {
                DynamicParameters ParamObj = new DynamicParameters();
                ParamObj.Add("@CandID", tblCandidateAnsobj.CandID);
                //ParamObj.Add("@CitzOrdrDate", tblCitizenObj.CitzOrdrDate);
                //ParamObj.Add("@CitzOrdrDateOne", tblCitizenObj.CitzOrdrDateOne);
                //ParamObj.Add("@CitzNsVaction", tblCitizenObj.CitzNsVaction);
                //ParamObj.Add("@CitzReglrVaction", tblCitizenObj.CitzReglrVaction);
                //ParamObj.Add("@CitzReservist", tblCitizenObj.CitzReservist);

                //ParamObj.Add("@CitizenDetailsID", null, DbType.Int32, ParameterDirection.Output);
                //ParamObj.Add("@ErrorOutput", null, DbType.Int32, ParameterDirection.Output);

                int Result = db.DapperExecute("USP_AddCitizenDetails", ParamObj, CommandType.StoredProcedure);

                TblCandidateAnswers CandObj = new TblCandidateAnswers();
                //CandObj.TableID = ParamObj.Get<Int32>("CitizenDetailsID");
                //CandObj.OutputID = ParamObj.Get<Int32>("ErrorOutput");
                return CandObj;
            }
            catch
            {
                return null;
            }
        }

        public Int64 AddInterviewData(DataTable DT, Int64 CandID)
        {
            SqlParameter[] param = {
                                       new SqlParameter("@InterviewData",SqlDbType.Structured){Value=DT},
                                       new SqlParameter("@Out",SqlDbType.BigInt){Direction=ParameterDirection.Output},
                                       new SqlParameter("@CandID",SqlDbType.BigInt){Value=CandID}
                                   };

            int result = db.ExecuteWithDataTable("USP_SaveInterviewData", param, CommandType.StoredProcedure);
            string output = param[1].Value.ToString();
            return Convert.ToInt64(output);
        }

        public int ManageInterviewQns(string QuestName = "")
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@QuestName", QuestName.Trim());
            param.Add("@OUT", 1, DbType.Int32, ParameterDirection.Output);

            db.DapperExecute("USP_ManageInterviewQns", param, CommandType.StoredProcedure);
            Int32 output = param.Get<Int32>("OUT");
            return output;
        }

        public List<TblSkillSets> GetSkillSets()
        {
            try
            {
                DynamicParameters paraObj = new DynamicParameters();
                var DataList = db.DapperToList<TblSkillSets>("Select SkilID,SkilsetName from TblSkillSets where SkilStatus=1", paraObj);
                return DataList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<TblLanguageKnown> GetLangKnown()
        {
            try
            {
                DynamicParameters paraobj = new DynamicParameters();
                var list = db.DapperToList<TblLanguageKnown>("Select LangknID,LanguageName from TblLanguageKnown where LangKnStatus=1", paraobj);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<TblCandidateSkillset> ListCandSkillsetIDs(Int64 CandID)
        {
            try
            {
                DynamicParameters para = new DynamicParameters();
                para.Add("@CandID", CandID);

                return db.DapperToList<TblCandidateSkillset>("select * from TblCandidateSkillset where CandID=@CandID", para);
            }
            catch
            {
                return null;
            }
        }

        public List<TblCandidateLanguage> ListCandLangKnownIDs(Int64 CandID)
        {
            try
            {
                DynamicParameters para = new DynamicParameters();
                para.Add("@CandID", CandID);

                return db.DapperToList<TblCandidateLanguage>("select * from TblCandidateLanguage where CandID=@CandID", para);
            }
            catch
            {
                return null;
            }
        }

        public TblCitizenDetails GetEditCitizenDetails(Int64 CandID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@CandID", CandID);
                return db.DapperFirst<TblCitizenDetails>("select * from TblCitizenDetails where CandID=@CandID", param);
            }
            catch
            {
                return null;
            }
        }

        public TblCandidateBank GetBankDetails(Int64 CandID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@CandID", CandID);
            return db.DapperFirst<TblCandidateBank>("select * from TblCandidateBank where CandID = @CandID", param);
        }

        public List<QuestionAnswer> ListInterview(Int64 CandID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@CandID", CandID);
                var LisInterviewQns = db.DapperToList<QuestionAnswer>("select a.QuestName,b.Answer,b.AnsRating from TblQuestions a join TblCandidateAnswers b on a.QuestID=b.QuestID and QtypeID=1  and b.CandID = @CandID ", param);
                return LisInterviewQns;
            }
            catch
            {
                return null;
            }
        }

        public int CheckMobileNoUnique(int flag, string Mob, int create_edit, Int64 CandID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@create_edit", create_edit);
                param.Add("@flag", 1, DbType.Int32);
                param.Add("@Mob", Mob);
                param.Add("@CandID", CandID);
                param.Add("@Out", 1, DbType.Int32, ParameterDirection.Output);
                var count = db.DapperExecute("USP_Unique_Mob_Email_Phone", param, CommandType.StoredProcedure);
                int output = param.Get<Int32>("Out");
                return output;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public int CheckPhNoUnique(int flag, string ph, int create_edit, Int64 CandID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@create_edit", create_edit);
                param.Add("@flag", 3, DbType.Int32);
                param.Add("@ph", ph);
                param.Add("@CandID", CandID);
                param.Add("@Out", 1, DbType.Int32, ParameterDirection.Output);
                var count = db.DapperExecute("USP_Unique_Mob_Email_Phone", param, CommandType.StoredProcedure);
                int output = param.Get<Int32>("Out");
                return output;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public int EmailUnique(int flag, string email, int create_edit, Int64 CandID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@create_edit", create_edit);
                param.Add("@flag", 2, DbType.Int32);
                param.Add("@email", email);
                param.Add("@CandID", CandID);
                param.Add("@Out", 1, DbType.Int32, ParameterDirection.Output);
                var count = db.DapperExecute("USP_Unique_Mob_Email_Phone", param, CommandType.StoredProcedure);
                int output = param.Get<Int32>("Out");
                return output;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public int CheckNRIC_Unique(int flag, string NRIC, int create_edit, Int64 CandID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@create_edit", create_edit);
                param.Add("@flag", 4, DbType.Int32);
                param.Add("@NRIC", NRIC);                
                param.Add("@CandID", CandID);
                param.Add("@Out", 1, DbType.Int32, ParameterDirection.Output);
                var count = db.DapperExecute("USP_Unique_Mob_Email_Phone", param, CommandType.StoredProcedure);
                int output = param.Get<Int32>("Out");
                return output;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }


        public int CheckFin_Unique(int flag, string Fin, int create_edit, Int64 CandID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@create_edit", create_edit);
                param.Add("@flag", 5, DbType.Int32);
                param.Add("@Fin", Fin);
                param.Add("@CandID", CandID);
                param.Add("@Out", 1, DbType.Int32, ParameterDirection.Output);
                var count = db.DapperExecute("USP_Unique_Mob_Email_Phone", param, CommandType.StoredProcedure);
                int output = param.Get<Int32>("Out");
                return output;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public CandidateItems AddEmergencyContact(TblEmergencyContact EmergencyContact, int flag, int HidTab4)
        {
            try
            {
                DynamicParameters ParaObj = new DynamicParameters();
                ParaObj.Add("@flag", flag);
                ParaObj.Add("@CandID", EmergencyContact.CandID);
                ParaObj.Add("@EmrgContact", EmergencyContact.EmrgContact);
                ParaObj.Add("@EmrgRelationship", EmergencyContact.EmrgRelationship);
                ParaObj.Add("@EmrgContactNo", EmergencyContact.EmrgContactNo);
                ParaObj.Add("@EmrgDetails", EmergencyContact.EmrgDetails);
                ParaObj.Add("@EmrgStatus", 1, DbType.Int32);
                ParaObj.Add("@Output", 1, DbType.Int32, ParameterDirection.Output);
                ParaObj.Add("@HidTab4", HidTab4);

                int Result = db.DapperExecute("USP_SaveEmergenctContact", ParaObj, CommandType.StoredProcedure);

                CandidateItems Items = new CandidateItems();
                Items.TableID = 0;
                Items.OutputID = ParaObj.Get<Int32>("Output");
                return Items;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<TblEmployeeType>GetEmployeeType()
        {
            var TblEmployeeType = db.DapperToList<TblEmployeeType>("Select EmpTypID,EmpTypName from TblEmployeeType");
            return TblEmployeeType;
        }

        public List<TblDeclarationType> GetDeclarationDetails()
        {
            var Details = db.DapperToList<TblDeclarationType>("select distinct a.* from TblDeclarationType a join TblDeclaration b on a.DeclTypeID=b.DeclTypeID ");
            return Details;
        }

        public List<TblDeclaration> GetDeclContent()
        {
            var Content = db.DapperToList<TblDeclaration>("select * from TblDeclaration");
            return Content;
        }

        public int SaveDeclaration(DataTable Dt, int flag, int HidTab6)
        {
            //DynamicParameters param = new DynamicParameters();
            //param.Add("@CandID", SessionManage.Current.CandID);
            //param.Add("@DeclID", Declaration.DeclID);
            //param.Add("@CandDeclAns", Declaration.CandDeclAns);
            //param.Add("@CandDeclAnsDetail", Declaration.CandDeclAnsDetail);
            //param.Add("@out", 1, DbType.Int32, ParameterDirection.Output);

            //int Result = db.DapperExecute("USP_SaveDeclarationDetails", param, CommandType.StoredProcedure);
            //return Result;
            SqlParameter[] param = {
                                       new SqlParameter("@DeclarationContent",SqlDbType.Structured){Value=Dt},
                                       new SqlParameter("@Out",SqlDbType.Int){Direction=ParameterDirection.Output},
                                       new SqlParameter("@CandID",SqlDbType.BigInt){Value=SessionManage.Current.CandID} ,
                                       new SqlParameter("@flag",SqlDbType.Int){Value=flag},
                                       new SqlParameter("@HidTab6",SqlDbType.Int){Value=HidTab6}
                                   };

            int result = db.ExecuteWithDataTable("USP_SaveDeclarationDetails", param, CommandType.StoredProcedure);
            string output = param[1].Value.ToString();
            return Convert.ToInt32(output);
        }

        public List<DeclarationDetails> DeclarationDetailsProfile()
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@CandID", SessionManage.Current.CandID);
            var content = db.DapperToList<DeclarationDetails>("select  a.DeclTypeID,a.DeclName,b.declID,b.DeclContent,c.CandDeclAns,c.CandDeclAnsDetail from TblDeclarationType a join TblDeclaration b on a.DeclTypeID=b.DeclTypeID join TblCandidateDeclaration c on b.DeclID=c.DeclID where c.CandID=@CandID", param);
            //if (content.Count() == 0)
            //    return db.DapperToList<DeclarationDetails>("select  a.DeclTypeID,b.declID,b.DeclContent from TblDeclarationType a join TblDeclaration b on a.DeclTypeID=b.DeclTypeID");
            return content;
        }

        public List<TblDeclarationType> GetDeclarationProf()
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@CandID", SessionManage.Current.CandID);
            var Details = db.DapperToList<TblDeclarationType>("select  distinct a.* from TblDeclarationType a join TblDeclaration b on a.DeclTypeID=b.DeclTypeID join TblCandidateDeclaration c on b.DeclID=c.DeclID where c.CandID=@CandID", param);
            return Details;
        }

        public List<DeclarationDetails> DeclarationDetailsProfile1()
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@CandID", SessionManage.Current.CandID);

            var content = db.DapperToList<DeclarationDetails>("select  a.DeclTypeID,b.declID,b.DeclContent from TblDeclarationType a join TblDeclaration b on a.DeclTypeID=b.DeclTypeID");
            return content;
        }

        public List<TblDeclarationType> GetDeclarationProf1()
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@CandID", SessionManage.Current.CandID);
            var Details = db.DapperToList<TblDeclarationType>("select  distinct a.* from TblDeclarationType a left join TblDeclaration b on a.DeclTypeID=b.DeclTypeID");
            return Details;
        }

        public int RemoveEmpHistory(int EmphID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@EmphID", EmphID);
                int Result = db.DapperExecute("Delete from TblEmployeHistory where EmphID=@EmphID", param);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int RemoveQuestions(Int64 QuestID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@QuestID", QuestID);
                int Result = db.DapperExecute("Delete from TblQuestions where QuestID=@QuestID", param);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int RemoveDocuments(Int64 DocID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@DocID", DocID);
                int Result = db.DapperExecute("Delete from TblDocuments where DocID=@DocID", param);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public CandidateOfferLetterData OfferLetterContent()
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@CandId", SessionManage.Current.CandID);
            return db.DapperFirst<CandidateOfferLetterData>("select a.CandName,a.CandAddress,a.CandEmail,b.CandAppDate,c.DesigName,d.DepName,a.CandOfferSalary,a.CandJoinDate from TblCandidate a join TblCandidateApproval b on a.CandID=b.CandID " +
                " join TblDesignation c on a.DesigID=c.DesigID join TblDepartment d on c.DepID=d.DepID where CandAppStatus=2  and a.CandID=@CandId ", param);
        }

        public int UpdateCandididateSalary(TblCandidate cndObj)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@CandID", cndObj.CandID);
                param.Add("@CandJoinDate", cndObj.CandJoinDate);
                param.Add("@CandOfferSalary", cndObj.CandOfferSalary);
                int updatedata = db.DapperExecute(" Update TblCandidate set CandJoinDate=@CandJoinDate , CandOfferSalary = @CandOfferSalary where CandID=@CandID", param);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public List<TblFundTypeMaster> GetFundTypes()
        {
            try
            {
                return db.DapperToList<TblFundTypeMaster>("select FundTypeID,FundType from TblFundTypeMaster");
            }
            catch
            {
                return null;
            }
        }

        public int UpdateEmployeePDFPath(string pdffile, Int64 CandId)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@EmpPDF", pdffile);
            param.Add("@CandId", CandId);
            return db.DapperExecute("Update TblEmployee set EmpPDF=@EmpPDF where CandID=@CandId", param);
        }

        public TblCompanyEmail CompanyEmailID(Int16 ID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@CompEmailID", ID);
            return db.DapperFirst<TblCompanyEmail>("select * from TblCompanyEmail where CompEmailID=@CompEmailID", param);
        }

        public EmployeeEditData GetEditEmployeeEmpDetails(Int64 CandID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@CandID", CandID);
                return db.DapperFirst<EmployeeEditData>("select * from TblEmployee as emp join TblEmployeeSalary as empsal on emp.EmpID=empsal.EmpID join TblCandidate cand on emp.CandID=cand.CandID where emp.CandID=@CandID and empsal.SalEndDate is null", param);
            }
            catch
            {
                return null;
            }
        }

        public List<TblCandidateTabMaster> GetTabValdation(Int64 CandID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@CandID", CandID);
                //return db.DapperToList<TblCandidateTabMaster>("select * from TblCandidateTabMaster as a where a.CandTabID !=2 and a.CandTabID not in (select b.CandTabID from TblCandTabValidationStatus as b where b.CandID=@CandID)", param);
                return db.DapperToList<TblCandidateTabMaster>("select * from TblCandidateTabMaster as a where a.CandTabID not in (select b.CandTabID from TblCandTabValidationStatus as b where b.CandID=@CandID)", param);
            }
            catch
            {
                return null;
            }
        }

        public CandidateItems AddPLRDDetails(TblPLRDDetails PLRDDetails, int HidTab8)
        {
            try
            {
                DynamicParameters paraObj = new DynamicParameters();
                paraObj.Add("@CandID", PLRDDetails.CandID);
                paraObj.Add("@IssueDate", PLRDDetails.IssueDate);
                paraObj.Add("@ExpiryDate", PLRDDetails.ExpiryDate);
                paraObj.Add("@LicenseNum", PLRDDetails.LicenseNum);
                paraObj.Add("@HidTab8", HidTab8);
                paraObj.Add("@ErrorOutput", null, DbType.Int32, ParameterDirection.Output);
                int Result = db.DapperExecute("USP_AddCandidatePLRDDetails", paraObj, CommandType.StoredProcedure);

                CandidateItems CandObj = new CandidateItems();
                CandObj.TableID = 0;
                CandObj.OutputID = paraObj.Get<Int32>("ErrorOutput");
                return CandObj;
            }
            catch
            {
                return null;
            }
        }

        public TblPLRDDetails GetPLRDDetails(Int64 CandID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@CandID", CandID);
            return db.DapperFirst<TblPLRDDetails>("select PLRDId,CandID,FORMAT(IssueDate,'dd/MM/yyyy')as dtissue,FORMAT(ExpiryDate,'dd/MM/yyyy')as dtexpiry,LicenseNum from TblPLRDDetails where CandID = @CandID", param);
        }

        public List<TblResidentStatus> GetResidentStatus()
        {
            try
            {
                var List = db.DapperToList<TblResidentStatus>("Select * from TblResidentStatus");
                return List;
            }
            catch
            {
                return null;
            }
        }

        public List<TblFixecSalaryTypes> GetFixecSalaryTypes(Int64 DesigID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@DesigID", DesigID);
            return db.DapperToList<TblFixecSalaryTypes>("select a.FixSalaTypID,a.FixSalaType,b.FixAllStatus as FixSalaStatus from TblFixecSalaryTypes as a " +
                                                        " left join TblFixedAllowanceDesigwise as b on a.FixSalaTypID=b.FixSalaTypID where b.DesigID=@DesigID and a.FixSalaStatus=1 and b.FixAllStatus=1", param);
        }

        public List<TblFixedAllowanceMaster> GetFixedAllowanceMaster(Int64 DesigID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@DesigID", DesigID);
            var data = db.DapperToList<TblFixedAllowanceMaster>("USP_GetFixedAllowanceList", param, CommandType.StoredProcedure);
            return data;
        }

        public int AllowanceEmpAssignmentSave(DataTable Dt1, DataTable Dt2, Int64 EmpId)
        {
            SqlParameter[] param = {
                                       new SqlParameter("@Datatable_Parent",SqlDbType.Structured){ Value=Dt1 },
                                       new SqlParameter("@Datatable_Child",SqlDbType.Structured){ Value=Dt2 },
                                       new SqlParameter("@Out",SqlDbType.Int){ Direction=ParameterDirection.Output },
                                       new SqlParameter("@EmpId",SqlDbType.BigInt){Value=EmpId},
                                   };

            int result = db.ExecuteWithDataTable("USP_EmpAllowanceSave", param, CommandType.StoredProcedure);
            string output = param[2].Value.ToString();
            int OutData = Convert.ToInt32(output);
            return OutData;
        }

        public List<TblLeaveMasterwise> GetLeaveMasterwise(Int64 DesigID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@DesigID", DesigID);
            return db.DapperToList<TblLeaveMasterwise>("USP_GetLeaveWiseData", param, CommandType.StoredProcedure);
        }

        public int EmployeeLeaveSave(DataTable Dt, Int64 EmpId)
        {
            SqlParameter[] param = {
                                       new SqlParameter("@Datatable_Leave",SqlDbType.Structured){ Value=Dt },
                                       new SqlParameter("@Out",SqlDbType.Int){ Direction=ParameterDirection.Output },
                                       new SqlParameter("@EmpId",SqlDbType.BigInt){Value=EmpId},
                                   };

            int result = db.ExecuteWithDataTable("USP_EmpLeaveSave", param, CommandType.StoredProcedure);
            string output = param[1].Value.ToString();
            int OutData = Convert.ToInt32(output);
            return OutData;
        }

        public JsonResult UpdateCandidateStatus(Int64 CandID)
        {
            DynamicParameters para = new DynamicParameters();
            para.Add("@CandID", CandID);
            string Empid = db.DapperSingle("Select EmpID from Tblemployee where CandID=@CandID", para);
            para.Add("@EmpId", Empid);
            int allcount = Convert.ToInt32(db.DapperSingle("Select count(*) from TblFixedAllowanceEmpwise where EmpID=@EmpId and FixAllEmpStatus=1", para));
            if (allcount == 0)
            {
                return Json(new { Message = "Please assign allowances for this employee", Result = -1 }, JsonRequestBehavior.AllowGet);
            }
            int leavcount = Convert.ToInt32(db.DapperSingle("Select count(*) from TblLeaveEmpwise where EmpID=@EmpId and LeaveEmpStatus=1", para));
            if (leavcount == 0)
            {
                return Json(new { Message = "Please fix leaves of this employee", Result = 0 }, JsonRequestBehavior.AllowGet);
            }

            //db.DapperExecute("Update TblEmployee set Emp_IsApproved=1 where CandID=@CandID", para);
            //db.DapperExecute("Update TblCandidateApproval set CandAppStatus=5 where CandID=@CandID", para);
            //db.DapperExecute("Update TblAppUser set AppUserTypID=1 where EmpID=convert(bigint,@EmpId)", para);

            return Json(new { Message = "Added Employee", Result = 1 }, JsonRequestBehavior.AllowGet);
        }

        public CandidateAppoinment EmployeeDetailsEdit(Int64 EmpId)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@EmpId", EmpId);
                return db.DapperFirst<CandidateAppoinment>("Select EmpStartDate as StartDate,EmpEndDate as EndDate,EmpTypID as Employeetyp,FundType,b.BasicSalary,c.Resident_Id,EmpSubTypeID,a.EmpRegNo from TblEmployee a join TblEmployeeSalary b on a.EmpID=b.EmpID join TblCandidate c on a.CandID=c.CandID where a.EmpID=@EmpId", param);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<LeaveDetails> GetLeaveDetails(Int64 EmpId)
        {
            try
            {
                DynamicParameters para = new DynamicParameters();
                para.Add("@EmpId", EmpId);

                return db.DapperToList<LeaveDetails>("Select * from TblLeaveType a join TblLeaveMasterwise b on a.LeavetypID=b.LeavetypID join TblLeaveEmpwise c on a.LeavetypID=c.LeavetypID where c.EmpID=@EmpId", para);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void SaveLeaveDetails(LeaveDetails details, Int32 HidTab9)
        {
            DynamicParameters para = new DynamicParameters();
            para.Add("@EligibleLeaves", details.EligibleLeaves);
            para.Add("@EarnedLeaves", details.EarnedLeaves);
            para.Add("@LeavesTaken", details.LeavesTaken);
            para.Add("@LeavesText", details.LeavesText);
            para.Add("@EmpID", details.EmployeeID);
            para.Add("@LeavetypID", details.LeavetypID);
            para.Add("@HidTab9", HidTab9);

            db.DapperExecute("Update TblLeaveEmpwise set  LeavesText=@LeavesText,EligibleLeaves=@EligibleLeaves,EarnedLeaves=@EarnedLeaves,LeavesTaken=@LeavesTaken where EmpID=@EmpID and LeavetypID=@LeavetypID", para);
            var CandID = db.DapperSingle("Select CandID from TblEmployee where EmpID=@EmpID", para);
            para.Add("@CandID", Convert.ToInt64(CandID));
            var count = db.DapperSingle("Select count(*) from TblCandTabValidationStatus where CandTabID=@HidTab9 and CandID=@CandID  and CandValidStatus=1", para);
            if (Convert.ToInt32(count) == 0)
                db.DapperExecute("insert into TblCandTabValidationStatus values (@HidTab9,@CandID,1)", para);
        }

        public List<TblCheckListTypes> GetOffboardCheckListType()
        {
            try
            {
                DynamicParameters param = new DynamicParameters();

                return db.DapperToList<TblCheckListTypes>("Select * from TblCheckListTypes", param);
            }
            catch
            {
                return null;
            }
        }

        public List<TblCheckListTypesEdit> GetOffboardCheckListTypeEdit(Int64 Emp_ID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@Emp_ID", Emp_ID);
                return db.DapperToList<TblCheckListTypesEdit>("select a.*,b.CheckListID,b.EmpID,b.Quantity,b.Size,b.CheckListStatus from TblCheckListTypes a Left join TblOnboardCheckList b on a.CheckListTypeID = b.CheckListTypeID and b.EmpID=@Emp_ID", param);
            }
            catch
            {
                return null;
            }
        }

        public Int64 Find(Int64 id)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@CandID", id);
            string Empid = db.DapperSingle("Select EmpID from Tblemployee where CandID=@CandID", param);
            Int64 Emp_id = Convert.ToInt64(Empid);
            return Emp_id;
        }

        public Int64 OffboardCheckListSave(TblOnboardCheckList usergobj, Int64 Can_Id, Int32 HidTab8)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@CheckListTypeID", usergobj.CheckListTypeID);
            param.Add("@Price", usergobj.Price);
            param.Add("@Size", usergobj.Size);
            param.Add("@Quantity", usergobj.Quantity);
            param.Add("@CheckListStatus", usergobj.CheckListStatus);
            param.Add("@EmpID", usergobj.EmpID);
            param.Add("@CandID", Can_Id);
            param.Add("@HidTab8", HidTab8);
            // var count = db.DapperExecute("SELECT COUNT(CheckListID) FROM TblOnboardCheckList where EmpID = @EmpID and CheckListTypeID =@CheckListTypeID and CheckListStatus = @CheckListStatus", param);

            //if(count >0)
            //{
            //    db.DapperExecute("Update TblLeaveEmpwise set  LeavesText=@LeavesText,EligibleLeaves=@EligibleLeaves,EarnedLeaves=@EarnedLeaves,LeavesTaken=@LeavesTaken where EmpID=@EmpID and LeavetypID=@LeavetypID", para);
            //}
            //else
            //{
            db.DapperExecute("insert into TblOnboardCheckList (CheckListTypeID,EmpID,Price,Quantity,Size,CheckListStatus) values (@CheckListTypeID,@EmpID,@Price,@Quantity,@Size,@CheckListStatus)", param);           
            // }
            if(HidTab8 != 0)
            {
                var count = db.DapperSingle("Select count(*) from TblCandTabValidationStatus where CandTabID=@HidTab8 and CandID=@CandID  and CandValidStatus=1", param);
                if (Convert.ToInt32(count) == 0)
                    db.DapperExecute("insert into TblCandTabValidationStatus values (@HidTab8,@CandID,1)", param);
            }

            db.DapperExecute("Update TblEmployee set Emp_IsApproved=1 where CandID=@CandID", param);
            db.DapperExecute("Update TblCandidateApproval set CandAppStatus=5 where CandID=@CandID", param);
            db.DapperExecute("Update TblAppUser set AppUserTypID=1 where EmpID=convert(bigint,@EmpID)", param);

            return usergobj.EmpID;
        }

        public void CheckListSave(Int64 Can_Id,Int64 Empid)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@EmpID", Empid);
            param.Add("@CandID", Can_Id);

            db.DapperExecute("Update TblEmployee set Emp_IsApproved=1 where CandID=@CandID", param);
            db.DapperExecute("Update TblCandidateApproval set CandAppStatus=5 where CandID=@CandID", param);
            db.DapperExecute("Update TblAppUser set AppUserTypID=1 where EmpID=convert(bigint,@EmpID)", param);
        }       

        public void OffboardCheckListDelete(Int64 Emp_Id)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@Emp_Id", Emp_Id);
            db.DapperExecute("Delete from TblOnboardCheckList where EmpID =@Emp_Id", param);
        }

        //public void OffboardCheckListEdit(Int64 Emp_Id, Int16 CheckListTypeID)
        //{
        //    DynamicParameters param = new DynamicParameters();
        //    param.Add("@Emp_Id", Emp_Id);
        //    param.Add("@CheckListTypeID", CheckListTypeID);

        //    db.DapperExecute("Delete from TblOnboardCheckList where EmpID =@Emp_Id and CheckListTypeID=@CheckListTypeID", param);
        //}

        public int CandBnkAcnoAlredyExists(string bankacc, string CandBnkID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@accnum", bankacc);
            Int32 Canbankid = Convert.ToInt32(CandBnkID);
            param.Add("@CandBnkID", Canbankid);
            if (Canbankid != 0)
            {
                string count = db.DapperSingle("Select count(*) from TblCandidateBank where CandBnkAcno=@accnum and CandBnkID!=@CandBnkID and CandBnkStatus = 1", param);
                return Convert.ToInt32(count);
            }
            else
            {
                string count = db.DapperSingle("Select count(*) from TblCandidateBank where CandBnkAcno=@accnum and CandBnkStatus = 1", param);
                return Convert.ToInt32(count);
            }

            //}
            //else
            //{
            //    string count = db.DapperSingle("Select count(*) from TblCandidateBank where CandBnkAcno=@accnum and CandID!=@CandID CandBnkStatus = 1", param);
            //    return Convert.ToInt32(count);
            //}
        }

        public List<TblEmployeeSubType> GetEmpSubType(Int16 EmpTypID)
        {
            DynamicParameters para = new DynamicParameters();
            para.Add("@EmpTypID", EmpTypID);
            var EmpTypes = db.DapperToList<TblEmployeeSubType>("Select EmpSubTypeID,EmpSubType from TblEmployeeSubType where EmpTypID=@EmpTypID", para);
            return EmpTypes;
        }
    }
}