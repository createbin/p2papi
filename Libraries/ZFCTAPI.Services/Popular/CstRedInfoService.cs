using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using ZFCTAPI.Core.Enums;
using ZFCTAPI.Data.CST;
using ZFCTAPI.Services.Repositorys;
using ZFCTAPI.Data.Popular;
using ZFCTAPI.Data.Pages;
using ZFCTAPI.Data.ApiModels.ReturnModels;

namespace ZFCTAPI.Services.Popular
{
    public interface ICstRedInfoService : IRepository<CST_red_info>
    {
        /// <summary>
        /// 根据红包id获取所有用户红包信息个数
        /// </summary>
        /// <param name="redId"></param>
        /// <returns></returns>
        int GetRedInfosByredIdNumber(int redId);

        decimal GetRedInfosAggregateAmount(int redId);

        bool IsRedInfoExistence(int userid, int redId);

        CST_red_info GetRedInfoByInvestId(int investId);

        /// <summary>
        /// 获取用户可用红包
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<CST_red_info> GeListEnableRedInfos(int userId);

        int AvaliableRedCount(int customerId);
        /// <summary>
        /// 标的使用红包列表
        /// </summary>
        /// <param name="loanId"></param>
        /// <returns></returns>
        List<CST_red_info> LoanRedList(int loanId);
        /// <summary>
        /// 标的已用的红包列表
        /// </summary>
        /// <param name="loanId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<CST_red_info> UserInvestWithRed(int loanId, int userId);

        /// <summary>
        /// 投资使用的红包
        /// </summary>
        /// <param name="investId"></param>
        /// <param name="cancelId"></param>
        /// <returns></returns>
        CST_red_info InvestUseRed(int? investId=0,int? cancelId=0);


        CST_red_info GetCstRedInfoByCondition(int redId=0);
        #region 用户

        /// <summary>
        /// 用户红包统计
        /// </summary>
        /// <param name="id">用户Customer编号</param>
        /// <param name="statisticsItem">统计项目</param>
        /// <returns></returns>
        RedStatisticsModel UserRedStatistics(int id, List<RedStatisticsType> statisticsItem);

        /// <summary>
        /// 用户红包分页
        /// </summary>
        /// <param name="id">Customer编号</param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PageDataView<CST_red_info> UserRedPage(int id, int page = 1, int pageSize = 5);

        /// <summary>
        /// 用户可用红包分页
        /// </summary>
        /// <param name="id">Customer编号</param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PageDataView<CST_red_info> UserRedWaitUsePage(int id, int page = 1, int pageSize = 5);

        /// <summary>
        /// 用户已用红包分页
        /// </summary>
        /// <param name="id">Customer编号</param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PageDataView<CST_red_info> UserRedUsedPage(int id, int page = 1, int pageSize = 5);

        /// <summary>
        /// 用户过期红包分页
        /// </summary>
        /// <param name="id">Customer编号</param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PageDataView<CST_red_info> UserRedExpiredPage(int id, int page = 1, int pageSize = 5);

        #endregion 用户
    }

    public class CstRedInfoService : Repository<CST_red_info>, ICstRedInfoService
    {
        public int GetRedInfosByredIdNumber(int redId)
        {
            var sqlStence = new StringBuilder();
            sqlStence.Append("select count(*) from CST_red_info");
            sqlStence.Append(" where cst_red_id='" + redId + "'");
            return _Conn.QueryFirst<int>(sqlStence.ToString());
        }

        public decimal GetRedInfosAggregateAmount(int redId)
        {
            var sqlStence = new StringBuilder();
            sqlStence.Append("select sum(cst_red_money) from CST_red_info");
            sqlStence.Append(" where cst_red_id='" + redId + "'");
            var result= _Conn.QueryFirstOrDefault<decimal?>(sqlStence.ToString());
            return result ?? 0.00m;
        }

        public bool IsRedInfoExistence(int userid, int redId)
        {
            var sqlStence = new StringBuilder();
            sqlStence.Append("select * from CST_red_info");
            sqlStence.Append(" where cst_red_id='" + redId + "'");
            sqlStence.Append(" and cst_user_id='" + userid + "'");
            var redinfo = _Conn.QueryFirstOrDefault<CST_red_info>(sqlStence.ToString());
            return redinfo != null ? true : false;
        }

        public List<CST_red_info> GeListEnableRedInfos(int userId)
        {
            var sqlStence = new StringBuilder();
            sqlStence.Append("select a.* from CST_red_info a");
            sqlStence.Append(" inner join POP_envelope_red b");
            sqlStence.Append(" on a.cst_red_id =b.Id");
            sqlStence.Append(" where cst_red_employ=0 and cst_user_id=" + userId + " and cst_red_investId is null");
            sqlStence.Append(" and (b.pop_red_useAstrictType='" + GebruiksType.Invest.ToString() + "' or b.pop_red_useAstrictType='" + GebruiksType.Unlimited.ToString() + "')");
            return _Conn.Query<CST_red_info>(sqlStence.ToString()).ToList();
        }

        public int AvaliableRedCount(int customerId)
        {
            var sqlStence = new StringBuilder();
            sqlStence.Append("select count(*) from CST_red_info ");
            sqlStence.Append(" where cst_user_id=" + customerId + " and cst_red_employ=0");
            sqlStence.Append(" and (cst_red_endDate is null or GETDATE()<cst_red_endDate)");
            return _Conn.QueryFirst<int>(sqlStence.ToString());
        }

        public List<CST_red_info> LoanRedList(int loanId)
        {
            //获取标的下使用的红包列表
            var sqlStence = new StringBuilder();
            sqlStence.Append("select a.* from CST_red_info a");
            sqlStence.Append(" inner join PRO_invest_info b");
            sqlStence.Append(" on a.cst_red_investId=b.Id");
            sqlStence.Append(" inner join PRO_loan_info c");
            sqlStence.Append(" on b.pro_loan_id=c.Id");
            sqlStence.Append($" where b.is_invest_succ=1 and b.pro_delsign=0 and a.cst_isTransfer=0 and c.id={loanId}");
            return _Conn.Query<CST_red_info>(sqlStence.ToString())?.ToList();
        }

        public List<CST_red_info> UserInvestWithRed(int loanId, int userId)
        {
            var sqlQuery = new StringBuilder();
            sqlQuery.Append("select b.* from PRO_invest_info a");
            sqlQuery.Append(" inner join CST_red_info b");
            sqlQuery.Append(" on a.Id=b.cst_red_investId");
            sqlQuery.Append($" where a.pro_invest_emp={userId} and a.pro_loan_id={loanId}");
            var redInfo = _Conn.Query<CST_red_info>(sqlQuery.ToString()).ToList();
            return redInfo;
        }

        public CST_red_info InvestUseRed(int? investId=0,int? cancelId=0)
        {
            #region 拼接条件
            var sqlStences = "1=1";
            if (investId != 0)
            {
                sqlStences += $" and cst_red_investId={investId}";
            }
            if (cancelId != 0)
            {
                sqlStences += $" and cst_cancel_investId={investId}";
            }
            #endregion
            var builder=new StringBuilder();
            builder.Append("select * from CST_red_info");
            builder.Append(" where " + sqlStences);
            return _Conn.QueryFirstOrDefault<CST_red_info>(builder.ToString());
        }

        #region 用户

        public RedStatisticsModel UserRedStatistics(int id, List<RedStatisticsType> statisticsItem)
        {
            if (statisticsItem.Count == 0)
                return new RedStatisticsModel();

            #region 拼装统计字段

            var listsqlOutParameters = new List<string>();

            if (statisticsItem.Contains(RedStatisticsType.RedCount))
                listsqlOutParameters.Add("count(Id) as RedCount");

            if (statisticsItem.Contains(RedStatisticsType.RedMoney))
                listsqlOutParameters.Add("sum(cst_red_money) as RedMoney");

            if (statisticsItem.Contains(RedStatisticsType.WaitUseCount))
                listsqlOutParameters.Add("count(case when cst_red_employ = 0 and (cst_red_endDate is null or DateDiff(dd,cst_red_endDate,getdate())<=0)  then Id end) as WaitUseCount");

            if (statisticsItem.Contains(RedStatisticsType.WaitUseMoney))
                listsqlOutParameters.Add("sum(case when cst_red_employ = 0 and (cst_red_endDate is null or DateDiff(dd,cst_red_endDate,getdate())<=0)  then cst_red_money end) as WaitUseMoney");

            if (statisticsItem.Contains(RedStatisticsType.UsedCount))
                listsqlOutParameters.Add("count(case when cst_red_employ = 1 then Id end) as UsedCount");

            if (statisticsItem.Contains(RedStatisticsType.UsedMoney))
                listsqlOutParameters.Add("sum(case when cst_red_employ = 1  then cst_red_money end) as UsedMoney");

            if (statisticsItem.Contains(RedStatisticsType.ExpiredCount))
                listsqlOutParameters.Add("count(case when cst_red_employ = 0 and DateDiff(dd,getdate(),cst_red_endDate)>0 then Id end) as ExpiredCount");

            if (statisticsItem.Contains(RedStatisticsType.ExpiredMoney))
                listsqlOutParameters.Add("sum(case when cst_red_employ = 0 and DateDiff(dd,getdate(),cst_red_endDate)>0 then cst_red_money end) as ExpiredMoney");

            #endregion 拼装统计字段

            #region 拼装sql

            var sqlTableParameters = new StringBuilder(" from");
            sqlTableParameters.Append(" CST_red_info ");

            var sqlWhereParameters = new StringBuilder(" where");
            sqlWhereParameters.Append($" cst_user_id = {id}");

            var sqlString = new StringBuilder("select ");
            sqlString.Append(string.Join(",", listsqlOutParameters));
            sqlString.Append(sqlTableParameters);
            sqlString.Append(sqlWhereParameters);

            #endregion 拼装sql

            return _Conn.QueryFirstOrDefault<RedStatisticsModel>(sqlString.ToString());
        }

        public PageDataView<CST_red_info> UserRedPage(int id, int page = 1, int pageSize = 5)
        {
            var criteria = new PageCriteria();
            criteria.Condition += $" cst_user_id = {id} ";
            var innerTable = @" CST_red_info";
            var selectSql = new StringBuilder();
            selectSql.Append(" * ");
            criteria.CurrentPage = page;
            criteria.PageSize = pageSize;
            criteria.Fields = selectSql.ToString();
            criteria.PrimaryKey = "Id";
            criteria.TableName = innerTable;
            criteria.Sort = "cst_red_employ asc,cst_red_endDate asc";
            var pageData = GetPageData<CST_red_info>(criteria);
            return pageData;
        }

        public PageDataView<CST_red_info> UserRedWaitUsePage(int id, int page = 1, int pageSize = 5)
        {
            var criteria = new PageCriteria();
            criteria.Condition += $" cst_user_id = {id} ";
            criteria.Condition += " and cst_red_employ =0 ";
            criteria.Condition += " and (cst_red_endDate is null or DateDiff(dd,cst_red_endDate,getdate())<=0) ";
            var innerTable = @" CST_red_info ";
            var selectSql = new StringBuilder();
            selectSql.Append(" * ");
            criteria.CurrentPage = page;
            criteria.PageSize = pageSize;
            criteria.Fields = selectSql.ToString();
            criteria.PrimaryKey = "Id";
            criteria.TableName = innerTable;
            criteria.Sort = "cst_red_employ asc,cst_red_endDate asc";
            var pageData = GetPageData<CST_red_info>(criteria);
            return pageData;
        }

        public PageDataView<CST_red_info> UserRedUsedPage(int id, int page = 1, int pageSize = 5)
        {
            var criteria = new PageCriteria();
            criteria.Condition += $" cst_user_id = {id} ";
            criteria.Condition += " and cst_red_employ =1 ";
            var innerTable = @" CST_red_info ";
            var selectSql = new StringBuilder();
            selectSql.Append(" * ");
            criteria.CurrentPage = page;
            criteria.PageSize = pageSize;
            criteria.Fields = selectSql.ToString();
            criteria.PrimaryKey = "Id";
            criteria.TableName = innerTable;
            criteria.Sort = "cst_red_employ asc,cst_red_endDate asc";
            var pageData = GetPageData<CST_red_info>(criteria);
            return pageData;
        }

        public PageDataView<CST_red_info> UserRedExpiredPage(int id, int page = 1, int pageSize = 5)
        {
            var criteria = new PageCriteria();
            criteria.Condition += $" cst_user_id = {id} ";
            criteria.Condition += " and cst_red_employ =0 ";
            criteria.Condition += " and DateDiff(dd,getdate(),cst_red_endDate)<0 ";
            var innerTable = @" CST_red_info ";
            var selectSql = new StringBuilder();
            selectSql.Append(" * ");
            criteria.CurrentPage = page;
            criteria.PageSize = pageSize;
            criteria.Fields = selectSql.ToString();
            criteria.PrimaryKey = "Id";
            criteria.TableName = innerTable;
            criteria.Sort = "cst_red_employ asc,cst_red_endDate asc";
            var pageData = GetPageData<CST_red_info>(criteria);
            return pageData;
        }

        #endregion 用户

        public CST_red_info GetCstRedInfoByCondition(int redId = 0)
        {
            #region 拼接条件
            var sqlStences = "1=1";
            if (redId != 0)
            {
                sqlStences += $" and Id={redId}";
            }
            #endregion
            var builder = new StringBuilder();
            builder.Append("select * from CST_red_info");
            builder.Append(" where " + sqlStences);
            return _Conn.QueryFirstOrDefault<CST_red_info>(builder.ToString());
        }

        public CST_red_info GetRedInfoByInvestId(int investId)
        {
            var sqlStr = $"SELECT * FROM CST_red_info where cst_red_investId={investId}";
            return _Conn.QueryFirstOrDefault<CST_red_info>(sqlStr);
        }
    }
}