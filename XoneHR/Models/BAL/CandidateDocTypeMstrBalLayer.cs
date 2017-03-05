using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using System.Data;

namespace XoneHR.Models.BAL
{
    public class CandidateDocTypeMstrBalLayer
    {
        private XoneDbLayer db;
        public CandidateDocTypeMstrBalLayer()
        {
            db = new XoneDbLayer();
        }
        public List<CandidateDocTypeDetail> GetCandidateCocTypeMaster()
        {
            return db.DapperToList<CandidateDocTypeDetail>(" select a.ValidDocTypID,b.Citizen,c.DesigName,d.DocStypName from TblValidDoctypeMaster as a join TblCityZenship as b on a.CizenID=b.CizenID " +
                                                           " join TblDesignation as c on a.DesigID=c.DesigID " +
                                                           " join TblDocumentSubTypes as d on a.DocStypID=d.DocStypID " +
                                                           " where b.IsStatus=1 and c.DesigUserTyp=1 and c.DesigStatus=1 and d.DocStypStatus=1");
        }
        public List<TblCityZenship> ComboListCitizenship()
        {
            try
            {
                var ListCitizenship = db.DapperToList<TblCityZenship>("select CizenID,Citizen from TblCityZenship where IsStatus=1");
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
                var ListDesignation = db.DapperToList<TblDesignation>("select DesigID,DesigName from TblDesignation where DesigStatus=1 and DesigUserTyp=1");
                return ListDesignation;
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
                var ListDocumntType = db.DapperToList<TblDocumentTypes>("select DocTypID,DocTypName from TblDocumentTypes where DocTypStatus=1");
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
                var ListDocumntSubType = db.DapperToList<TblDocumentSubTypes>("select DocStypID,DocStypName from TblDocumentSubTypes where DocStypStatus=1 and DocTypID=@DocTypID", paraObj);
                return ListDocumntSubType;
            }
            catch
            {
                return null;
            }
        }
        public int ManageCanddidateDocTypeMaster(TblValidDoctypeMaster DocObj)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@CizenID", DocObj.CizenID);
                param.Add("@DesigID", DocObj.DesigID);
                param.Add("@DocStypID", DocObj.DocStypID);
                param.Add("@OUT", 1, DbType.Int32, ParameterDirection.Output);
                db.DapperExecute("USP_ManageCandidateDocType", param,CommandType.StoredProcedure);
                var Out =  param.Get<Int32>("OUT");
                return Out;
            }
            catch(Exception ex)
            {
                return 0;
            }
        }
        public int DeleteCandDocSubType(Int32 ValidDocTypID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@ValidDocTypID", ValidDocTypID);
                return db.DapperExecute(" delete from TblValidDoctypeMaster where ValidDocTypID=@ValidDocTypID", param);

            }catch(Exception ex)
            {
                return 0;
            }
                
        }
    }
}