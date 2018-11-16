using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using ZFCTAPI.Core;
using ZFCTAPI.Data.ApiModels.ReturnModels;
using ZFCTAPI.Data.Pages;
using ZFCTAPI.Data.PRO;
using ZFCTAPI.Services.Repositorys;
using ZFCTAPI.Data.InvestInfo;
using ZFCTAPI.Core.Enums;

namespace ZFCTAPI.Services.InvestInfo
{
    public interface IInvestInfoService : IRepository<PRO_invest_info>
    {
        /// <summary>
        /// 投资总额
        /// </summary>
        /// <returns></returns>
        decimal SumInvestMoney();

        /// <summary>
        /// 获取投资中分页数据
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PageDataView<RBiddingInvest> BiddingLoanPage(int userId, int page = 1, int pageSize = 5);

        /// <summary>
        /// 获取还款中的分页数据
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PageDataView<RRepaymentInvest> RepaymentLoanPage(int userId, int page = 1, int pageSize = 5);

        /// <summary>
        /// 获取已结清的分页数据
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PageDataView<RClearedInvest> ClearedLoanPage(int userId, int page = 1, int pageSize = 5);
        /// <summary>
        /// 获取App还款中的分页数据
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PageDataView<RAPPRepaymentInvest> AppRepaymentLoanPage(int userId, int page = 1, int pageSize = 5);

        /// <summary>
        /// 用户投资统计
        /// </summary>
        /// <param name="id">用户编号</param>
        /// <param name="statisticsItem">统计项目</param>
        /// <returns></returns>
        InvestStatisticsModel InvestStatistics(int id, List<InvestStatisticsType> statisticsItem);

        PRO_invest_info GetInvestInfoByCondition(bool? investSuccess, bool? isDel,int transferId = 0);
        /// <summary>
        /// 通过guid获取项目信息
        /// </summary>
        /// <param name="investGuid"></param>
        /// <returns></returns>
        PRO_invest_info GetInvestInfoByInvestGuid(Guid investGuid);

        PageDataView<PRO_invest_info> LoanInvesterPage(int loanId, int page, int pageSize);

        int CountInvest(int investerId);

        List<PRO_invest_info> GetInvestListByLoanId(int loanId, int investType);

        /// <summary>
        /// 根据标ID的获取集合
        /// </summary>
        /// <param name="loanId"></param>
        /// <param name="isUse">true 正在使用,false 未使用,null 查询全部</param>
        /// <returns></returns>
        List<PRO_invest_info> GetInvestListByLoanId(int loanId,bool? isUse);

        /// <summary>
        /// 获取标的成功的投资标的
        /// </summary>
        /// <param name="loanId"></param>
        /// <returns></returns>
        List<PRO_invest_info> GetLoanSuccessInevst(int loanId);

        /// <summary>
        /// 是否是第一次投资
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        bool ValidateFirstInvest(int userId);
        /// <summary>
        /// 用户在投资的标的
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<PRO_invest_info> UserInvestInfos(int userId);
        /// <summary>
        /// 用户使用红包的标的
        /// </summary>
        /// <param name="loanId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<PRO_invest_info> UserInvestWithRed(int loanId, int userId);

        /// <summary>
        /// 流水号查询
        /// </summary>
        /// <param name="merBillNo"></param>
        /// <returns></returns>
        PRO_invest_info GetInvestInfoByMerBillNo(string merBillNo);
    }

    public class InvestInfoService : Repository<PRO_invest_info>, IInvestInfoService
    {
        public decimal SumInvestMoney()
        {
            var sum = _Conn.QueryFirst<decimal>(
                "select SUM(pro_credit_money) from PRO_invest_info where pro_transfer_id is null and pro_delsign=0");
            return sum;
        }

        /// <summary>
        /// 获取投资中分页数据
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public PageDataView<RBiddingInvest> BiddingLoanPage(int userId, int page = 1, int pageSize = 5)
        {
            var criteria = new PageCriteria { Condition = "1=1" };
            criteria.Condition += string.Format(" and b.pro_loan_state in({0},{1},{2})", DataDictionary.projectstate_Tender, DataDictionary.projectstate_FullScalePending, DataDictionary.projectstate_StayTransfer);
            criteria.Condition += " and a.pro_delsign=0 and a.is_invest_succ = 1";
            criteria.Condition += string.Format(" and a.pro_invest_emp={0}", userId);
            var innerTable = " PRO_invest_info a left join PRO_loan_info b on a.pro_loan_id=b.Id";
            var selectSql = new StringBuilder();
            selectSql.Append(" b.pro_loan_use as LoanName,");
            selectSql.Append(" b.pro_loan_money as LoanMoney,");
            selectSql.Append(" b.Id as LoanId,");
            selectSql.Append(" a.Id as InvestId,");
            selectSql.Append(" b.pro_loan_rate as LoanRate,");
            selectSql.Append(" a.pro_invest_money as InvestMoney,");
            selectSql.Append(" a.pro_invest_date as InvestDate,");
            selectSql.Append(" b.pro_loan_state as LoanState,");
            selectSql.Append(" b.pro_loan_period as LoanPeriod,");
            selectSql.Append(" b.pro_period_type as LoanPeriodType,");
            selectSql.Append(" b.pro_pay_type as LoanRepayType,");
            selectSql.Append(" b.Bohai as Bohai");

            criteria.CurrentPage = page;
            criteria.PageSize = pageSize;
            criteria.Fields = selectSql.ToString();
            criteria.PrimaryKey = "a.Id";
            criteria.TableName = innerTable;
            criteria.Sort = "a.pro_invest_date desc";
            var pageData = GetPageData<RBiddingInvest>(criteria);
            return pageData;
        }

        public PageDataView<RRepaymentInvest> RepaymentLoanPage(int userId, int page = 1, int pageSize = 5)
        {
            var criteria = new PageCriteria { Condition = "1=1" };
            criteria.Condition += string.Format(" and c.pro_loan_state in({0},{1})", DataDictionary.projectstate_Repayment, DataDictionary.projectstate_Overdue);
            criteria.Condition += " and a.pro_is_clear=0";
            criteria.Condition += string.Format(" and b.pro_invest_emp={0}", userId);
            var innerTable = "PRO_invest_info b left join PRO_invester_plan a on a.pro_invest_id=b.Id left join PRO_loan_info c on b.pro_loan_id=c.Id";
            var selectSql = new StringBuilder();
            selectSql.Append(" c.pro_loan_use as LoanName,");
            selectSql.Append(" c.Id as LoanId,b.Id as InvestId,");
            selectSql.Append(" c.pro_loan_rate as LoanRate,");
            selectSql.Append(" b.pro_invest_money as InvestMoney,");
            selectSql.Append(" b.pro_invest_date as InvestDate,");
            selectSql.Append(" c.pro_loan_state as LoanState,");
            selectSql.Append(" c.pro_loan_money as LoanMoney,");
            selectSql.Append(" a.pro_pay_date as PayDate,");
            selectSql.Append(" a.pro_pay_total as PayMoeny");
            criteria.CurrentPage = page;
            criteria.PageSize = pageSize;
            criteria.Fields = selectSql.ToString();
            criteria.PrimaryKey = "a.Id";
            criteria.TableName = innerTable;
            criteria.Sort = "a.pro_pay_date asc";
            try
            {
                var pageData = GetPageData<RRepaymentInvest>(criteria);
                return pageData;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public PageDataView<RClearedInvest> ClearedLoanPage(int userId, int page = 1, int pageSize = 5)
        {
            var criteria = new PageCriteria { Condition = "1=1" };
            criteria.Condition += string.Format(" and b.pro_loan_period=c.pro_loan_period");
            criteria.Condition += string.Format(" and (c.pro_pay_type={1} or b.pro_loan_state={0})", DataDictionary.projectstate_Settled, DataDictionary.RepaymentType_PlatformDaihuan);
            criteria.Condition += string.Format(" and a.pro_invest_emp={0} and a.pro_delsign=0 and a.is_invest_succ=1 ", userId);
            var innerTable = "PRO_invest_info a left join PRO_loan_plan c on a.pro_loan_id=c.pro_loan_id left join PRO_loan_info b on c.pro_loan_id=b.Id";
            var selectSql = new StringBuilder();
            selectSql.Append(" a.Id as InvestId,");
            selectSql.Append(" b.Id as LoanId,");
            selectSql.Append(" b.pro_loan_use as LoanName,");
            selectSql.Append(" b.pro_loan_money as LoanMoney,");
            selectSql.Append(" b.pro_loan_rate as LoanRate,");
            selectSql.Append(" a.pro_invest_money as InvestMoney,");
            selectSql.Append(" a.pro_invest_date as InvestDate,");
            selectSql.Append(" c.pro_collect_date as ClearedDate,");
            selectSql.Append(" b.pro_start_date as InterestDate,");
            selectSql.Append(" b.pro_end_date as ExpireDate,");
            selectSql.Append(" b.pro_loan_state as LoanState,");
            selectSql.Append(" b.pro_loan_period as LoanPeriod,");
            selectSql.Append(" b.pro_period_type as LoanPeriodType,");
            selectSql.Append(" b.pro_pay_type as LoanRepayType");
            criteria.CurrentPage = page;
            criteria.PageSize = pageSize;
            criteria.Fields = selectSql.ToString();
            criteria.PrimaryKey = "a.Id";
            criteria.TableName = innerTable;
            criteria.Sort = "c.pro_collect_date desc";
            try
            {
                var pageData = GetPageData<RClearedInvest>(criteria);
                return pageData;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public PageDataView<RAPPRepaymentInvest> AppRepaymentLoanPage(int userId, int page = 1, int pageSize = 5)
        {
            var criteria = new PageCriteria { Condition = "1=1" };
            criteria.Condition += string.Format(" and b.pro_loan_period=c.pro_loan_period");
            criteria.Condition += string.Format(" and b.pro_loan_state in({0},{1}) and (c.pro_pay_type !={2} or c.pro_pay_type is null) ", DataDictionary.projectstate_Repayment, DataDictionary.projectstate_Overdue, DataDictionary.RepaymentType_PlatformDaihuan);
            criteria.Condition += string.Format(" and a.pro_invest_emp={0} and a.pro_delsign=0 and a.is_invest_succ=1 ", userId);
            var innerTable = " PRO_invest_info a left join PRO_loan_plan c on a.pro_loan_id=c.pro_loan_id left join PRO_loan_info b on c.pro_loan_id=b.Id ";
            var selectSql = new StringBuilder();
            selectSql.Append(" a.Id as InvestId,");
            selectSql.Append(" b.Id as LoanId,");
            selectSql.Append(" b.pro_loan_use as LoanName,");
            selectSql.Append(" b.pro_loan_money as LoanMoney,");
            selectSql.Append(" b.pro_loan_rate as LoanRate,");
            selectSql.Append(" a.pro_invest_money as InvestMoney,");
            selectSql.Append(" a.pro_invest_date as InvestDate,");
            selectSql.Append(" b.pro_start_date as InterestDate,");
            selectSql.Append(" b.pro_end_date as ExpireDate,");
            selectSql.Append(" b.pro_loan_period as LoanPeriod,");
            selectSql.Append(" b.pro_period_type as LoanPeriodType,");
            selectSql.Append(" b.pro_pay_type as LoanRepayType");
            criteria.CurrentPage = page;
            criteria.PageSize = pageSize;
            criteria.Fields = selectSql.ToString();
            criteria.PrimaryKey = "a.Id";
            criteria.TableName = innerTable;
            criteria.Sort = "c.pro_pay_date asc";
            try
            {
                var pageData = GetPageData<RAPPRepaymentInvest>(criteria);
                return pageData;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public InvestStatisticsModel InvestStatistics(int id, List<InvestStatisticsType> statisticsItem)
        {
            if (statisticsItem.Count == 0)
                return new InvestStatisticsModel();

            #region 拼装统计字段

            var listsqlOutParameters = new List<string>();
            if (statisticsItem.Contains(InvestStatisticsType.InvestCount))
                listsqlOutParameters.Add("count(case when a.is_invest_succ = 1 then a.Id end) as InvestCount");

            if (statisticsItem.Contains(InvestStatisticsType.InvestMoney))
                listsqlOutParameters.Add("sum(case when a.is_invest_succ = 1 then a.pro_invest_money end) as InvestMoney");

            if (statisticsItem.Contains(InvestStatisticsType.BiddingCount))
            {
                listsqlOutParameters.Add($"count(case when b.pro_loan_state in({DataDictionary.projectstate_Tender},{DataDictionary.projectstate_FullScalePending},{DataDictionary.projectstate_StayTransfer}) then a.Id end) as BiddingCount");
            }

            if (statisticsItem.Contains(InvestStatisticsType.BiddingMoney))
            {
                listsqlOutParameters.Add($"sum(case when a.is_invest_succ = 1 and b.pro_loan_state in({DataDictionary.projectstate_Tender},{DataDictionary.projectstate_FullScalePending},{DataDictionary.projectstate_StayTransfer}) then a.pro_invest_money end) as BiddingMoney");
            }

            if (statisticsItem.Contains(InvestStatisticsType.RepaymentCount))
            {
                listsqlOutParameters.Add($"count(case when a.is_invest_succ = 1 and b.pro_loan_state in({DataDictionary.projectstate_Repayment},{DataDictionary.projectstate_Overdue}) and (c.pro_pay_type != {DataDictionary.RepaymentType_PlatformDaihuan} or c.pro_pay_type is null) then a.Id end) as RepaymentCount");
            }

            if (statisticsItem.Contains(InvestStatisticsType.RepaymentMoney))
            {
                listsqlOutParameters.Add($"sum(case when a.is_invest_succ = 1 and b.pro_loan_state in({DataDictionary.projectstate_Repayment},{DataDictionary.projectstate_Overdue}) and (c.pro_pay_type != {DataDictionary.RepaymentType_PlatformDaihuan} or c.pro_pay_type is null) then a.pro_invest_money end)  as RepaymentMoney");
            }

            if (statisticsItem.Contains(InvestStatisticsType.ClearedCount))
            {
                listsqlOutParameters.Add($"count(case when a.is_invest_succ = 1 and (b.pro_loan_state  = {DataDictionary.projectstate_Settled} or c.pro_pay_type = {DataDictionary.RepaymentType_PlatformDaihuan}) then a.Id end) as ClearedCount");
            }

            if (statisticsItem.Contains(InvestStatisticsType.ClearedMoney))
            {
                listsqlOutParameters.Add($"sum(case when a.is_invest_succ = 1 and (b.pro_loan_state  = {DataDictionary.projectstate_Settled} or c.pro_pay_type = {DataDictionary.RepaymentType_PlatformDaihuan}) then a.pro_invest_money end) as ClearedMoney");
            }
            if (statisticsItem.Contains(InvestStatisticsType.TransInvestCount))
            {
                listsqlOutParameters.Add($"count(case when a.is_invest_succ = 1 and a.pro_invest_type = {DataDictionary.investType_transfer} then a.Id end) as TransInvestCount");
            }

            #endregion 拼装统计字段

            #region 拼装sql

            var sqlTableParameters = new StringBuilder(" from");

            sqlTableParameters.Append(" PRO_invest_info a");
            sqlTableParameters.Append(" left join PRO_loan_info b on a.pro_loan_id = b.Id");
            if (statisticsItem.Contains(InvestStatisticsType.ClearedMoney) || statisticsItem.Contains(InvestStatisticsType.ClearedCount) || statisticsItem.Contains(InvestStatisticsType.RepaymentCount) || statisticsItem.Contains(InvestStatisticsType.RepaymentMoney))
            {
                sqlTableParameters.Append(" left join PRO_loan_plan c on b.Id =c.pro_loan_id and c.pro_loan_period = b.pro_loan_period ");
            }

            var sqlWhereParameters = new StringBuilder(" where");
            sqlWhereParameters.Append($" a.pro_invest_emp = {id}");
            sqlWhereParameters.Append(" and a.pro_delsign = 0");

            var sqlString = new StringBuilder("select ");
            sqlString.Append(string.Join(",", listsqlOutParameters));
            sqlString.Append(sqlTableParameters);
            sqlString.Append(sqlWhereParameters);

            #endregion 拼装sql

            return _Conn.QueryFirstOrDefault<InvestStatisticsModel>(sqlString.ToString());
        }

        public PRO_invest_info GetInvestInfoByCondition(bool? investSuccess, bool? isDel,int transferId = 0)
        {
            #region 查询条件

            var sqlStence = "1=1";
            if (investSuccess != null)
            {
                sqlStence += $" and is_invest_succ='{investSuccess}'";
            }
            if (isDel != null)
            {
                sqlStence += $" and pro_delsign='{isDel}'";
            }
            if (transferId != 0)
            {
                sqlStence += $" and pro_transfer_id='{transferId}'";
            }
            #endregion 查询条件

            var builder = new StringBuilder();
            builder.Append("select * from PRO_invest_info");
            builder.Append(" where " + sqlStence);
            var result = _Conn.QueryFirstOrDefault<PRO_invest_info>(builder.ToString());
            return result;
        }

        public PRO_invest_info GetInvestInfoByInvestGuid(Guid investGuid)
        {
            var selectSql = $"select * from pro_invest_info where pro_invest_guid='{investGuid}'";
            return _Conn.QueryFirst<PRO_invest_info>(selectSql.ToString());
        }

        public PageDataView<PRO_invest_info> LoanInvesterPage(int loanId, int page, int pageSize)
        {
            var criteria = new PageCriteria { Condition = "1=1" };
            criteria.Condition += string.Format(" and pro_loan_id={0} and a.pro_delsign=0", loanId);
            criteria.Condition += string.Format(" and a.is_invest_succ=1 and a.pro_invest_type={0}", DataDictionary.investType_normal);
            criteria.Condition += $" and b.pro_loan_state in({DataDictionary.projectstate_Repayment},{DataDictionary.projectstate_Settled},{DataDictionary.projectstate_Tender},{DataDictionary.projectstate_FullScalePending},{DataDictionary.projectstate_StayTransfer},{DataDictionary.projectstate_Overdue})";
            var innerTable = "PRO_invest_info a inner join PRO_loan_info b on a.pro_loan_id=b.Id";
            criteria.CurrentPage = page;
            criteria.PageSize = pageSize;
            criteria.Fields = "a.*";
            criteria.PrimaryKey = "a.Id";
            criteria.TableName = innerTable;
            try
            {
                var pageData = GetPageData<PRO_invest_info>(criteria);
                return pageData;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public int CountInvest(int investerId)
        {
            var selectSql = $"select count(*) from pro_invest_info where pro_invest_emp={investerId} and pro_delsign=0";
            return _Conn.QueryFirst<int>(selectSql.ToString());
        }

        public List<PRO_invest_info> GetInvestListByLoanId(int loanId, int investType)
        {
            if (investType == DataDictionary.investType_normal)
            {
                var sqlStence1 = "select a.* from PRO_invest_info a inner join PRO_loan_info b on a.pro_loan_id=b.Id";
                sqlStence1 += " where a.pro_transfer_state=1 and a.pro_delsign=0";
                sqlStence1 +=
                    $" and  b.Id={loanId} and and (b.pro_loan_state in({DataDictionary.projectstate_Repayment},{DataDictionary.projectstate_Settled})) and a.pro_invest_type={investType}";
                var query1 = _Conn.Query<PRO_invest_info>(sqlStence1).ToList();
                var sqlStence2 =
                    "select a.* from PRO_invest_info a inner join PRO_transfer_apply b on a.Id=b.pro_invest_id";
                sqlStence2 += " where a.pro_invest_emp!=b.pro_user_id and a.pro_delsign=0";
                sqlStence2 += $" and a.pro_transfer_id={loanId} and a.pro_invest_type={investType}";
                var query2 = _Conn.Query<PRO_invest_info>(sqlStence2);
                return query1.Concat(query2).ToList();
            }
            var sqlStence3 = "select * from PRO_invest_info a inner join PRO_transfer_apply b on a.Id=b.pro_invest_id";
            sqlStence3 += " where a.pro_invest_emp!=b.pro_user_id  and a.pro_delsign=0";
            sqlStence3 += $" and a.pro_invest_type={DataDictionary.investType_transfer} and a.pro_transfer_id={loanId}";
            return _Conn.Query<PRO_invest_info>(sqlStence3).ToList();
        }

        public List<PRO_invest_info> GetInvestListByLoanId(int loanId, bool? isUse)
        {
            var sqlStr = "SELECT * FROM PRO_invest_info where is_invest_succ=1 and pro_loan_id = " + loanId;

            if (isUse != null && isUse.Value)
            {
                sqlStr += " pro_is_use= 1";
            }
            else if(isUse != null&&!isUse.Value)
            {
                sqlStr += " pro_is_use= 0";
            }

            return _Conn.Query<PRO_invest_info>(sqlStr).ToList();
        }

        public List<PRO_invest_info> GetLoanSuccessInevst(int loanId)
        {
            var selectSql = $"select * from PRO_invest_info where is_invest_succ=1 and pro_loan_id={loanId}";
            return _Conn.Query<PRO_invest_info>(selectSql.ToString()).ToList();
        }

        public bool ValidateFirstInvest(int userId)
        {
            var sqlStr = "SELECT * FROM PRO_invest_info WHERE pro_delsign =0 and pro_transfer_state=1 and pro_invest_emp=" + userId;

            var investRecords = _Conn.Query<PRO_invest_info>(sqlStr).ToList();
            if(investRecords.Any())
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public List<PRO_invest_info> UserInvestInfos(int userId)
        {
            var sqlStr = "SELECT * FROM PRO_invest_info WHERE pro_delsign =0 and pro_invest_emp=" + userId;
            var investRecords = _Conn.Query<PRO_invest_info>(sqlStr).ToList();
            return investRecords;
        }

        public List<PRO_invest_info> UserInvestWithRed(int loanId, int userId)
        {
            var sqlQuery = new StringBuilder();
            sqlQuery.Append("select a.* from PRO_invest_info a");
            sqlQuery.Append(" inner join CST_red_info b");
            sqlQuery.Append(" on a.Id=b.cst_red_investId");
            sqlQuery.Append($" where a.pro_invest_emp={userId} and a.pro_loan_id={loanId}");
            var investRecords = _Conn.Query<PRO_invest_info>(sqlQuery.ToString()).ToList();
            return investRecords;
        }

        public PRO_invest_info GetInvestInfoByMerBillNo(string merBillNo)
        {
            var sqlStr = $"SELECT * FROM PRO_invest_info WHERE tran_seq_no ='{merBillNo}'";
            return _Conn.Query<PRO_invest_info>(sqlStr).FirstOrDefault();
        }
    }
}