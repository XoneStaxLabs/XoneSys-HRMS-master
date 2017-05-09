using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Data.SqlClient;

namespace XoneHR.Models.BAL
{
    public class EmployeeBallayer
    {
        private CommonFunctions common = new CommonFunctions();
        private XoneDbLayer db;

        public EmployeeBallayer()
        {
            db = new XoneDbLayer();
        }


        //public List<EmployeeDetails> ListAllEmployees(Int16 ListTypeId)
        public List<EmployeeDetails> ListAllEmployees()
        {
            try
            {
                DynamicParameters Paraobj=new DynamicParameters ();
                Paraobj.Add("@CodingNo",1);
                //Paraobj.Add("@ListTypeId", ListTypeId);

                var Employeelists = db.DapperToList<EmployeeDetails>("USP_ListemployeeDatatable", Paraobj, CommandType.StoredProcedure);
                return Employeelists;
            }
            catch {

                return null;
            }
        }

        public List<EmpProject> ProjectDetails()
        {
            var details = db.DapperToList<EmpProject>("Select empl.EmpID,prjt.ProjName from TblEmployee empl join TblEmpProject empprjt on empprjt.EmpID=empl.EmpID join TblProject prjt on prjt.ProjID = empprjt.ProjID where projTypID=1");
            return details;
        }

        public EmployeeDetails ListEmployeesDetails(Int64 empID)
        {
            try
            {
                DynamicParameters Paraobj = new DynamicParameters();
                Paraobj.Add("@EmpID", empID);

                var Employeelists = db.DapperFirst<EmployeeDetails>("USP_ListemployeeDetails", Paraobj, CommandType.StoredProcedure);
                return Employeelists;
            }
            catch
            {

                return null;
            }
        }


        public List<EmployeeValidationDocs> ListEmployeesDocs(Int64 CandID,int flag)
        {
            try
            {
                DynamicParameters Paraobj = new DynamicParameters();
                Paraobj.Add("@candID", CandID);
                Paraobj.Add("@flag", flag);

                var Employeelists = db.DapperToList<EmployeeValidationDocs>("USP_ListValiationDocs", Paraobj, CommandType.StoredProcedure);
                return Employeelists;
            }
            catch
            {

                return null;
            }
        }

        public List<DocumentDetails> ListEmployeeDocuments(Int64 empID)
        {
            try
            {
                DynamicParameters Paraobj = new DynamicParameters();
                Paraobj.Add("@EmpID", empID);

                var DocumentsList = db.DapperToList<DocumentDetails>("USP_ListemployeeDocuments", Paraobj, CommandType.StoredProcedure);
                return DocumentsList;
            }
            catch
            {

                return null;
            }
        }

        public List<TblEmployeHistory> ListEmployeeHistory(Int64 empID)
        {
            try
            {
                DynamicParameters Paraobj = new DynamicParameters();
                Paraobj.Add("@EmpID", empID);

                var EmpHistory = db.DapperToList<TblEmployeHistory>("USP_ListemployeeWorkHistory", Paraobj, CommandType.StoredProcedure);
                return EmpHistory;
            }
            catch
            {

                return null;
            }
        }

        public SalaryDetails ListEmployeeSalarys(Int64 EmpID)
        {
            try
            {
                DynamicParameters paraObj = new DynamicParameters();
                paraObj.Add("@EmpID", EmpID);

                SalaryDetails salObj =new SalaryDetails ();

                var items = db.DapperMultiResults("USP_ListemployeSalary", paraObj, CommandType.StoredProcedure);
                salObj.empsalarydetails= items.Read<EmpSalaryDetails>().ToList();
                salObj.empsalarytypeDetails = items.Read<EmpSalryTypeDetails>().ToList();

                return salObj;
            }
            catch {

                return null;

            }
        }

        public List<LeaveDetails> ListLeaveDetails(Int64 EmpID)
        {
            try
            {
                DynamicParameters paraObj=new DynamicParameters ();
                paraObj.Add("@EmpID",EmpID);

                var leaveDetails = db.DapperToList<LeaveDetails>("USP_Listemployeeleaves", paraObj, CommandType.StoredProcedure);
                return leaveDetails;
            }//USP_Listemployeeleaves also used in leavecontroller
            catch
            {
                return null;
            }
        }
        public List<TblSkillSets> ListEmployeeSkillSet(Int64 EmpID)
        {
            try
            {
                DynamicParameters paraObj = new DynamicParameters();
                paraObj.Add("@EmpID", EmpID);

                var EmployeeSkillSet = db.DapperToList<TblSkillSets>("select c.SkilsetName,c.SkilID from TblCandidateSkillset as a join TblEmployee b on a.CandID=b.CandID  " +
                                                                 "join TblSkillSets as c on a.SkilID=c.SkilID where b.EmpStatus=1 and b.EmpID=@EmpID",paraObj);
                return EmployeeSkillSet;
            }
            catch
            {
                return null;
            }
        }
        public List<TblLanguageKnown> ListEmployeeLanguageKnown(Int64 EmpID)
        {
            try
            {
                DynamicParameters paraObj = new DynamicParameters();
                paraObj.Add("@EmpID", EmpID);

                var LanguageKnown = db.DapperToList<TblLanguageKnown>("select c.LanguageName,c.LangknID from TblEmployee as a join TblCandidateLanguage as b on a.CandID=b.CandID " +
                                                                      "join TblLanguageKnown as c on b.LangknID=c.LangknID where a.EmpStatus=1 and a.EmpID=@EmpID",paraObj);
                return LanguageKnown;
            }
            catch
            {
                return null;
            }
        }

        public List<DeclarationDetails> DeclarationDetailsProfile()
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@EmpID", SessionManage.Current.EmpID);
            var content = db.DapperToList<DeclarationDetails>("select  a.DeclTypeID,a.DeclName,b.declID,b.DeclContent,c.CandDeclAns,c.CandDeclAnsDetail from TblDeclarationType a join TblDeclaration b on a.DeclTypeID=b.DeclTypeID join TblCandidateDeclaration c on b.DeclID=c.DeclID where c.CandID=@EmpID", param);
            //if (content.Count() == 0)
            //    return db.DapperToList<DeclarationDetails>("select  a.DeclTypeID,b.declID,b.DeclContent from TblDeclarationType a join TblDeclaration b on a.DeclTypeID=b.DeclTypeID");
            return content;
        }

        public List<TblDeclarationType> GetDeclarationProf()
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@EmpID", SessionManage.Current.EmpID);
            var Details = db.DapperToList<TblDeclarationType>("select  distinct a.* from TblDeclarationType a join TblDeclaration b on a.DeclTypeID=b.DeclTypeID join TblCandidateDeclaration c on b.DeclID=c.DeclID where c.CandID=@EmpID", param);
            return Details;
        }

        public EmployeeProfile GetEmplyeeName(Int64 EmpId)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@EmpID", EmpId);
            return db.DapperFirst<EmployeeProfile>("Select b.CandName from TblEmployee a join TblCandidate b on a.CandID=b.CandID where a.EmpID=@EmpID", param);
        }

        public int SaveSalaryDetails(TblSalaryPay salarypay)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@EmpId", salarypay.EmpId);
            param.Add("@SalYear", salarypay.SalYear);
            param.Add("@SalMonth", salarypay.SalMonth);
            param.Add("@PayAmount", salarypay.PayAmount);
            param.Add("@PayType", salarypay.PayType);
            param.Add("@PayDate", salarypay.PayDate);
            param.Add("@PayMode", salarypay.PayMode);
            param.Add("@Out", 1, DbType.Int32, ParameterDirection.Output);

            int Result = db.DapperExecute("USP_SaveSalaryDetails", param, CommandType.StoredProcedure);
            int Output = param.Get<Int32>("Out");
            return Output;
        }
        public int DeleteEmployee(Int64 EmpID, Int64 CandID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@EmpID", EmpID);
                param.Add("@CandID", CandID);

                var iresult = db.DapperExecute("update TblEmployee set EmpStatus=0 where EmpID=@EmpID", param);
                var iresult1 = db.DapperExecute("update TblCandidate set CandStatus=0 where CandID=@CandID", param);
                return 1;
            }
            catch(Exception ex)
            {
                return 0;
            }

        }

        public List<TblFixecSalaryTypes> GetAllowanceSalaryType(Int64 EmpId)
        {
            DynamicParameters param = new DynamicParameters();
            var DesigId = db.DapperSingle("select a.DesigID from TblCandidate a join TblEmployee b on a.CandID=b.CandID where b.EmpID=1", param);
            param.Add("@DesigId",Convert.ToInt32(DesigId));

            var FixSalaType = db.DapperToList<TblFixecSalaryTypes>("select FixSalaTypID,FixSalaType from TblFixecSalaryTypes where FixSalaTypID in((select FixSalaTypID from TblFixedAllowanceDesigwise where DesigID=@DesigId and FixAllStatus=1)) and FixSalaStatus=1", param);
            return FixSalaType;                
        }

        public List<TblFixecSalaryTypes> GetAllowanceSalaryType_Emp(Int64 EmpId, Int64 Year, int month)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@EmpId", EmpId);
            param.Add("@Year", Year);
            param.Add("@month", month);

            return db.DapperToList<TblFixecSalaryTypes>("Select FixSalaTypID from TblFixecSalaryTypes where FixSalaTypID in(Select FixSalaTypID from TblFixedAllowanceEmpwise where FixAllalloMonth=@month and FixAllalloYear=@Year and EmpID=@EmpId and FixAllEmpStatus=1)", param);
        }

     
        public List<TblFixedAllowanceTypes> GetAllowanceType(Int64 EmpId)
        {
            DynamicParameters param = new DynamicParameters();
            var DesigId = db.DapperSingle("select a.DesigID from TblCandidate a join TblEmployee b on a.CandID=b.CandID where b.EmpID=1", param);
            param.Add("@DesigId", Convert.ToInt32(DesigId));

            var FixSalaType = db.DapperToList<TblFixedAllowanceTypes>("select b.FixallTypeID,b.FixallTypes,b.FixallAmounts,a.FixSalaTypID from TblFixedAllowanceDesigwise a join TblFixedAllowanceTypes b on a.FixAllDesigID=b.FixAllDesigID where a.DesigID=@DesigId and a.FixAllStatus=1 and b.FixallStatus=1", param);
            return FixSalaType;
        }

        public List<TblFixedAllowanceTypes> GetAllowanceType_Emp(Int64 EmpId, Int64 Year, int month)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@EmpId", EmpId);
            param.Add("@Year", Year);
            param.Add("@month", month);

            var IList = db.DapperToList<TblFixedAllowanceTypes>("Select b.FixallEmpTypes,a.FixSalaTypID,b.FixallEmpAmounts from TblFixedAllowanceEmpwise a join TblFixedAllowanceEmpTypes b on a.FixAllEmpD=b.FixAllEmpD where FixAllalloMonth=@month and FixAllalloYear=@Year and EmpID=@EmpId and a.FixAllEmpStatus=1", param);
            return IList;
        }

        public int AllowanceEmpAssignmentSave(DataTable Dt1, DataTable Dt2, int Year, int Month, Int64 EmpId)
        {

            SqlParameter[] param = {
                                       new SqlParameter("@Datatable_Parent",SqlDbType.Structured){ Value=Dt1 },
                                       new SqlParameter("@Datatable_Child",SqlDbType.Structured){ Value=Dt2 },
                                       new SqlParameter("@Out",SqlDbType.Int){ Direction=ParameterDirection.Output }, 
                                       new SqlParameter("@Year",SqlDbType.Int){Value = Year},
                                       new SqlParameter("@Month",SqlDbType.Int){Value=Month},
                                       new SqlParameter("@EmpId",SqlDbType.BigInt){Value=EmpId},
                                   };


            int result = db.ExecuteWithDataTable("USP_EmpAllowance", param, CommandType.StoredProcedure);
            string output = param[2].Value.ToString();
            int OutData = Convert.ToInt32(output);
            return OutData;

        }

        public int CreateEdit(Int64 EmpId)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@EmpId", EmpId);

            var count = ("select count(*) from TblFixedAllowanceEmpwise where EmpID=@EmpId where FixAllEmpStatus=1");
            return Convert.ToInt32(count);
        }

        public List<TblEmployeeSalary> ListEmployeeSalary(Int64 EmpID)
        {
            try
            {
                DynamicParameters para =new DynamicParameters ();
                para.Add("@EmpID",EmpID);

                var results = db.DapperToList<TblEmployeeSalary>("select *from TblEmployeeSalary where EmpID=@EmpID",para);
                return results;
            }
            catch
            {
                return null;
            }
        }


        public bool AddNewSalary(TblEmployeeSalary salaObj)
        {
            try
            {
                DynamicParameters para = new DynamicParameters();
                para.Add("@BasicSalary", salaObj.BasicSalary);
                para.Add("@SalStartDate", salaObj.SalStartDate);
                para.Add("@SalEndDate", salaObj.SalEndDate);
                para.Add("@EmpID", salaObj.EmpID);

                var Updateresult = db.DapperExecute("update TblEmployeeSalary set SalEndDate=@SalEndDate where SalEndDate is null and EmpID=@EmpID", para);
                var result = db.DapperExecute("insert into TblEmployeeSalary (EmpID,BasicSalary,SalStartDate) values(@EmpID,@BasicSalary,@SalStartDate)", para);



                return true;
            }
            catch
            {
                return false;
            }
        }


        public TblEmployeeSalary GetEmployeeDates(Int64 EmpID)
        {
            try
            {
                DynamicParameters para =new DynamicParameters ();
                para.Add("@EmpID", EmpID);

                TblEmployeeSalary empsal = new TblEmployeeSalary();
                //var startdate = db.DapperSingle("select top 1 SalStartDate from TblEmployeeSalary where EmpID=@EmpID order by SalStartDate asc ", para);
                var Endate = db.DapperSingle("select top 1 (FORMAT(SalStartDate, 'dd/MM/yyyy')) as SalStartDate from TblEmployeeSalary where EmpID=@EmpID order by SalStartDate desc ", para);
                
                DateTime now = Convert.ToDateTime(Endate);
                DateTime lastDayLastMonth = new DateTime(now.Year, now.Month, 1);
                lastDayLastMonth = lastDayLastMonth.AddDays(-1);

                DateTime Endnow = Convert.ToDateTime(Endate);
                DateTime EndlastDayLastMonth = new DateTime(now.Year, now.Month, 1);
                EndlastDayLastMonth = EndlastDayLastMonth.AddYears(-1);


                empsal.SalStartDate = EndlastDayLastMonth;
                empsal.SalEndDate = Convert.ToDateTime(Endate);

                return empsal;

            }
            catch
            {
                return null;
            }
        }

        public List<AllowanceListing> AllowanceListing(Int64 EmpID, int year, int month)
        {
            try
            {
                DynamicParameters para = new DynamicParameters();
                para.Add("@EmpID", EmpID);
                para.Add("@year", year);
                para.Add("@month", month);

                var results = db.DapperToList<AllowanceListing>("Select a.FixallEmpTypes,a.FixallEmpAmounts,c.FixSalaType from TblFixedAllowanceEmpTypes a join TblFixedAllowanceEmpwise b on a.FixAllEmpD=b.FixAllEmpD join TblFixecSalaryTypes c on c.FixSalaTypID=b.FixSalaTypID where b.FixAllalloMonth=@month and b.FixAllalloYear=@year and b.EmpID=@EmpID and b.FixAllEmpStatus=1", para);
                return results;
            }
            catch
            {
                return null;
            }
        }

        public double GetBasicSal(Int64 EmpID)
        {
            try
            {
                DateTime dt=new DateTime();
                dt=DateTime.Now;
                DynamicParameters para=new DynamicParameters();
                para.Add("@EmpID",EmpID);
                para.Add("@date",common.CommonDateConvertion(dt.ToString("dd/MM/yyyy")));
                //var sal = db.DapperSingle("Select BasicSalary from TblEmployeeSalary where empid=@EmpID and ((Convert(date,SalStartDate)<=Convert(date,@date) and Convert(date,SalEndDate) >=convert(date,@date)) OR (Convert(date,SalStartDate)<=convert(date,@date) and SalEndDate Is NULL))", para);
                var sal = db.DapperSingle("Select top 1 BasicSalary from TblEmployeeSalary where empid=@EmpID order by salmastid desc", para);

                return Convert.ToDouble(sal);

            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public Int16 GetEmpType(Int64 EmpID)
        {
            DynamicParameters para = new DynamicParameters();
            para.Add("@EmpID", EmpID);

            var Type = db.DapperSingle("Select EmpTypID from TblEmployee where EmpID=@EmpID", para);  //,EmpPartTime_Pay
            return Convert.ToInt16(Type);
        }

        public Int16 GetPartialEmpDetails(Int64 EmpID)
        {
            DynamicParameters para = new DynamicParameters();
            para.Add("@EmpID", EmpID);

            var EmpPartTime_Pay = db.DapperSingle("Select EmpPartTime_Pay from TblEmployee where EmpID=@EmpID", para);
            return Convert.ToInt16(EmpPartTime_Pay);
        }

        //public List<TblFixecSalaryTypes> GetAllowanceSalaryTypeEMP(Int64 EmpId)
        //{
        //    DynamicParameters param = new DynamicParameters();
        //    param.Add("@EmpId", EmpId);

        //    var FixSalaType = db.DapperToList<TblFixecSalaryTypes>("select FixSalaTypID,FixSalaType from TblFixecSalaryTypes where FixSalaTypID in(select FixSalaTypID from TblFixedAllowanceEmpwise where EmpID=@EmpId )", param);
        //    return FixSalaType;
        //}

        //public List<TblFixedAllowanceTypes> GetAllowanceTypeEMP(Int64 EmpId)
        //{
        //    DynamicParameters param = new DynamicParameters();
        //    var DesigId = db.DapperSingle("select a.DesigID from TblCandidate a join TblEmployee b on a.CandID=b.CandID where b.EmpID=1", param);
        //    param.Add("@DesigId", Convert.ToInt32(DesigId));

        //    var FixSalaType = db.DapperToList<TblFixedAllowanceTypes>("select b.FixallTypeID,b.FixallTypes,b.FixallAmounts,a.FixSalaTypID from TblFixedAllowanceDesigwise a join TblFixedAllowanceTypes b on a.FixAllDesigID=b.FixAllDesigID where a.DesigID=@DesigId and a.FixAllStatus=1 and b.FixallStatus=1", param);
        //    return FixSalaType;
        //}

        public List<TblDeductionType> GetDeductionTypes()
        {
            try
            {
                var data = db.DapperToList<TblDeductionType>("Select * from TblDeductionType");
                TblDeductionType type = new TblDeductionType();
                type.DeductType_Id=0;
                type.DeductionType = "ADD NEW";
                data.Add(type);
                return data;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public int DeductionTypeSave(string DeductionType)
        {
            try
            {
                DynamicParameters para = new DynamicParameters();
                para.Add("@DeductionType", DeductionType);
                var result = db.DapperExecute("Insert into TblDeductionType(DeductionType) values(@DeductionType)", para);
                return 1;
            }
            catch(Exception ex)
            {
                return 0;
            }
        }

        public void DeleteDeductionType(int DeductType_Id)
        {
            DynamicParameters para = new DynamicParameters();
            para.Add("@DeductType_Id", DeductType_Id);
            var result = db.DapperExecute("Delete from TblDeductions where DeductType_Id=@DeductType_Id", para);
        }

        public void SaveDeductions(TblDeductions deducts)
        {
            DynamicParameters para = new DynamicParameters();
            para.Add("@DeductType_Id", deducts.DeductType_Id);
            para.Add("@EmpID", deducts.EmpID);
            para.Add("@Year", deducts.DeductYear);
            para.Add("@Month", deducts.DeductMonth);
            para.Add("@Deduction", deducts.Deduction);
            para.Add("@Amount", deducts.Amount);
            para.Add("@Remarks", deducts.Remarks);

            var count = db.DapperSingle("Select count(*) from TblDeductions where DeductType_Id=@DeductType_Id and EmpID=@EmpID and DeductYear=@Year and DeductMonth=@Month", para);
            if (Convert.ToInt16(count) == 0)
            { var result = db.DapperExecute("Insert into TblDeductions values(@DeductType_Id,@EmpID,@Year,@Month,@Deduction,@Amount,@Remarks,getdate())", para); }
        }

        public List<TblDeductions> ListDeductions(int Year, int Month,Int64 EmpID)
        {
            DynamicParameters para = new DynamicParameters();
            para.Add("@Year", Year);
            para.Add("@Month", Month);
            para.Add("@EmpID",EmpID);

            var data = db.DapperToList<TblDeductions>("Select *from TblDeductions where DeductYear=@Year and DeductMonth=@Month and EmpID=@EmpID", para);
            for (int i = 0; i < data.Count(); i++)
            {
                data[i].MonthName = GetMonthName(data[i].DeductMonth);
            }
            return data;
        }


        public string GetMonthName(int id)
        {
            DynamicParameters para = new DynamicParameters();
            para.Add("@MonthNumber", id);

            return db.DapperSingle("Select DateName( month , DateAdd( month , @MonthNumber , -1 ))", para);
        }

        public List<UniformComponent> UnformDetails(Int64 EmpID)
        {
            DynamicParameters para = new DynamicParameters();
            para.Add("@EmpID", EmpID);

            return db.DapperToList<UniformComponent>("Select CheckListID,CheckListDetails,b.Price,Quantity,Size from TblOnboardCheckList a join TblCheckListTypes b on a.CheckListTypeID=b.CheckListTypeID where EmpID=@EmpID and CheckListStatus=1", para);
        }

    }
}