using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace XoneHR.Models.BAL
{
    public class MemosBal
    {
        private XoneDbLayer db;

        public MemosBal()
        {
            db = new XoneDbLayer();
        }

        public List<TblDesignation> ListAllDesignation(int filterID)
        {
            try
            {
                DynamicParameters para = new DynamicParameters();
                para.Add("@DesigUserTyp", filterID);

                var listdesignation = db.DapperToList<TblDesignation>("select *from TblDesignation where DesigUserTyp=@DesigUserTyp", para);
                return listdesignation;
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<TblMemosUserList> MemosUserList()
        {
            try
            {
                var MemosUserList = db.DapperIEnumerable<TblMemosUserList>("select a.*,b.AppFirstName,c.* from TblmemoUserwise a join TblAppUser b on a.AppUID=b.AppUID  join  TblMemos c on a.MemoID = c.MemoID");
                return MemosUserList;
            }
            catch
            {
                return null;
            }
        }

        public Int32 Addnewmemos(TblMemos memoObj)
        {
            try
            {
                DynamicParameters paraObj = new DynamicParameters();
                paraObj.Add("@MemoSubject", memoObj.MemoSubject);
                paraObj.Add("@MemoText", memoObj.MemoText);
                paraObj.Add("@MemoPriority", memoObj.MemoPriority);
                paraObj.Add("@MemoStatus", true);
                paraObj.Add("@MemoAddedby", SessionManage.Current.AppUID);

                paraObj.Add("@outputid", null, DbType.Int32, ParameterDirection.Output);

                db.DapperExecute("USP_AddMemos", paraObj, CommandType.StoredProcedure);
                Int32 memoid = paraObj.Get<Int32>("outputid");

                return memoid;
            }
            catch
            {
                return 0;
            }
        }

        public void AddnewmemosDesigs(int memoid, Int64 userid)
        {
            try
            {
                DynamicParameters paraObj = new DynamicParameters();
                paraObj.Add("@AppUID", userid);
                paraObj.Add("@MemoID", memoid);

                db.DapperExecute("insert into TblmemoUserwise (AppUID,MemoID,ReadStatus)values(@AppUID,@MemoID,0)", paraObj);
            }
            catch { }
        }

        public List<GetAppUserIDs> GetAppUserIDs(int Desigid)
        {
            try
            {
                DynamicParameters para = new DynamicParameters();
                para.Add("@DesigID", Desigid);

                var AppUserids = db.DapperToList<GetAppUserIDs>("select AppUID from TblAppUser where DesigID=@DesigID", para);
                return AppUserids;
            }
            catch
            {
                return null;
            }
        }

        public List<MemosUser> ListMemos(Int64 userID)
        {
            try
            {
                DynamicParameters para = new DynamicParameters();
                para.Add("@AppUID", userID);

                var listmemos = db.DapperToList<MemosUser>("select *from TblMemos memo join TblmemoUserwise memouser on memo.MemoID=memouser.MemoID where memouser.AppUID=@AppUID", para);
                return listmemos;
            }
            catch
            {
                return null;
            }
        }

        public List<TblAppUser> ListEmployees(int DesigID)
        {
            try
            {
                DynamicParameters para = new DynamicParameters();
                para.Add("@DesigID", DesigID);

                string query = "select AppUID,AppFirstName from TblAppUser where DesigID=@DesigID";

                return db.DapperToList<TblAppUser>(query, para);
            }
            catch
            { return null; }
        }

        public List<TblProject> ListProjects()
        {
            try
            {
                //where ProjStatus=1
                string query = "select *from TblProject";
                return db.DapperToList<TblProject>(query);
            }
            catch
            { return null; }
        }

        public void AddProjectwisememo(Int64 projectid, int memoid)
        {
            try
            {
                DynamicParameters para = new DynamicParameters();
                para.Add("@projectID", projectid);
                para.Add("@memosID", memoid);

                db.DapperExecute("USP_AddMemoProjectwisEmployee", para, CommandType.StoredProcedure);
            }
            catch
            { }
        }

        public void AddNewEmployeememos(Int64 appUid, int memosID)
        {
            try
            {
                DynamicParameters para = new DynamicParameters();
                para.Add("@appUid", appUid);
                para.Add("@memosID", memosID);

                string Query = "insert into TblmemoUserwise (AppUID,MemoID,ReadStatus)values(@appUid,@memosID,0)";

                db.DapperExecute(Query, para);
            }
            catch
            { }
        }

        public void AddMemoDocuments(string filepath, int memosID)
        {
            try
            {
                DynamicParameters para = new DynamicParameters();
                para.Add("@filepath", filepath);
                para.Add("@memosID", memosID);

                db.DapperExecute("insert into TblMemoDocument (MemoDocument,MemoID) values(@filepath,@memosID)", para);
            }
            catch { }
        }
    }
}