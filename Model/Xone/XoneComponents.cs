using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model.Xone
{
    public class DocumentSubTypeList
    {
        public int DocSubtypeID { get; set; }
        public int DocTypeID { get; set; } 
        public string DocSubtypeName { get; set; }
        public string DocTypeName { get; set; }
    }

    public class DepartmentList
    {
        public Int32 DeptID { get; set; }
        public string DeptCode { get; set; }
        public string DeptName { get; set; }
        public string UserType { get; set; }
    }

    public class HolidayList
    {
        public int HolidayID { get; set; }
        public string Holiday { get; set; }
        public DateTime HolidayDate { get; set; }
        public string Description { get; set; }
    }

    public class GradeList
    {
        public int GradeID { get; set; }
        public int GradeCode { get; set; }
        public string Gradename { get; set; }
        public string DesignationName { get; set; }
        public string DeptName { get; set; }
    }
    public class CandidateDocTypeDetail
    {
        public int ValidDocTypID { get; set; }
        public string CitizenName { get; set; }
        public string DesignationName { get; set; }
        public string DocSubtypeName { get; set; }
    }


}  