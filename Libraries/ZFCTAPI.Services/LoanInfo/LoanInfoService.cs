using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Dapper;
using ZFCTAPI.Core;
using ZFCTAPI.Core.Enums;
using ZFCTAPI.Data.Pages;
using ZFCTAPI.Data.PRO;
using ZFCTAPI.Services.Repositorys;
using ZFCTAPI.Data.LoanInfo;
using ZFCTAPI.Data.ApiModels.ReturnModels;
using ZFCTAPI.Data.CST;

namespace ZFCTAPI.Services.LoanInfo
{
    public interface ILoanInfoService : IRepository<PRO_loan_info>
    {
        PRO_loan_info GetProReleaseById(int proloanId);

        PageDataView<PRO_loan_info> LoanInfoPage(LoanType loanType, int page = 1, int pageSize = 5);

        /// <summary>
        /// 获取tbloanlabel集合
        /// </summary>
        /// <returns></returns>
        List<tblabel> GettbloanlabelList(int loanId);

        /// <summary>
        /// 获取产品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        PRO_product_info GetProductInfo(int id);

        /// <summary>
        /// 获取微信端推荐标的
        /// </summary>
        /// <param name="period"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        PRO_loan_info WechatRecommand(int period);


        #region 用户

        /// <summary>
        /// 用户借款统计
        /// </summary>
        /// <param name="id">用户编号</param>
        /// <param name="statisticsItem">统计项目</param>
        /// <returns></returns>
        LoanStatisticsModel LoanStatistics(int id, List<LoanStatisticsType> statisticsItem);

        /// <summary>
        /// 根据项目状态获得分页数据
        /// </summary>
        /// <param name="id">用户编号</param>
        /// <param name="dataDictionary">项目状态</param>
        /// <param name="page">页数</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns></returns>
        PageDataView<PRO_loan_info> GetPageByState(int id,IList<int> dataDictionary,int page = 1, int pageSize = 5);

        /// <summary>
        /// 获取用户借款明细
        /// </summary>
        /// <param name="id">用户编号</param>
        /// <param name="page">页数</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns></returns>
        PageDataView<RLoanItemDetail> LoanItemDetail(int id, int page = 1, int pageSize = 5);

        #endregion 用户

        /// <summary>
        /// 方法：根据放款日期、借款期限、期限类型得到到期日期
        /// </summary>
        /// <param name="strQXLX">1.期限类型(1：年 2：月 3：日)</param>
        /// <param name="datJKKSR">2.放款日期</param>
        /// <param name="loanPeriod">3.借款期限</param>
        /// <returns>到期日期</returns>
        DateTime GetEndDateByStartDate(int strQXLX, DateTime datJKKSR, int loanPeriod);

        /// <summary>
        /// 查找该用户所有还款中或已结清的项目
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        List<PRO_loan_info> GetProLoanInfoListByUserId(int userId);

        /// <summary>
        /// 获取标的绑定用户信息
        /// </summary>
        /// <param name="loanId"></param>
        /// <returns></returns>
        PRO_loan_userInfo GetLoanUserInfo(int loanId);
        /// <summary>
        /// 获取标的产品信息
        /// </summary>
        /// <returns></returns>
        PRO_product_info GetLoanProduct(int? loanId);
        List<PRO_loan_repayfollow> GetRepayFollows(int loanId);
        /// <summary>
        /// 获得抵押物信息
        /// </summary>
        /// <returns></returns>
        string GetCollateralInformation(int loanId);

        /// <summary>
        /// 根据流水号查询
        /// </summary>
        /// <param name="merBillNo"></param>
        /// <returns></returns>
        PRO_loan_info GetLoanInfoByMerBillNo(string merBillNo);
        /// <summary>
        /// 推荐新手标的
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        List<PRO_loan_info> RecommandNewHandLoan(int count);
        /// <summary>
        /// 推荐一般标的
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        List<PRO_loan_info> RecommandCommonLoan(int count);

        /// <summary>
        /// 借款总数量
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        int HasLoan(int Id);

        /// <summary>
        /// 获得借款的所有投资户
        /// </summary>
        /// <param name="loanId"></param>
        /// <returns></returns>
        IList<CST_user_info> GetInvesterByLoanId(int loanId);
        /// <summary>
        /// 根据指定的借款人查询出借款人现在在融资的所有的标的
        /// </summary>
        /// <returns></returns>
        List<PRO_loan_info> LoansByPeriods(int loanerId);

        /// <summary>
        /// 根据编号获取标的信息
        /// </summary>
        /// <param name="loanid"></param>
        /// <returns></returns>
        PRO_loan_info GetLoanInfoByID(string loanid);

        bool UpdateBohaiByID(string loanid);
    }

    public class LoanInfoService : Repository<PRO_loan_info>, ILoanInfoService
    {
        /// <summary>
        /// 方法：根据放款日期、借款期限、期限类型得到到期日期
        /// </summary>
        /// <param name="strQXLX">1.期限类型(1：年 2：月 3：日)</param>
        /// <param name="datJKKSR">2.放款日期</param>
        /// <param name="loanPeriod">3.借款期限</param>
        /// <returns>到期日期</returns>
        public DateTime GetEndDateByStartDate(int strQXLX, DateTime datJKKSR, int loanPeriod)
        {
            DateTime dtEndDate = datJKKSR;
            if (strQXLX == DataDictionary.deadlinetype_Month)//期限类型
            {
                dtEndDate = dtEndDate.AddMonths(loanPeriod);
            }
            else if (strQXLX == DataDictionary.deadlinetype_Day)
            {
                dtEndDate = dtEndDate.AddDays(loanPeriod);
            }
            return dtEndDate;
        }

        public PRO_loan_info GetProReleaseById(int proloanId)
        {
            return Find(proloanId);
        }

        public PageDataView<PRO_loan_info> LoanInfoPage(LoanType loanType, int page = 1, int pageSize = 5)
        {
            var result = new PageDataView<PRO_loan_info>();
            if (loanType == LoanType.NewHand)
            {
                result = NewHandList(page, pageSize);
            }
            else if (loanType == LoanType.Recommand)
            {
                result = RecommandList(page, pageSize);
            }
            return result;
        }

        public List<tblabel> GettbloanlabelList(int loanId)
        {
            var result = new List<tblabel>();

            #region 待修改

            var loanLable = this._Conn.Query<tbloanlabel>("select * from tbloanlabel where loanId='" + loanId + "'").ToList();
            if (!loanLable.Any()) return result;
            foreach (var lable in loanLable)
            {
                var lableInfo =
                    this._Conn.QueryFirstOrDefault<tblabel>("select * from tblabel where id='" + lable.labelId + "'");
                if (lableInfo != null)
                {
                    result.Append(lableInfo);
                }
            }

            #endregion 待修改

            return result;
        }

        public PRO_product_info GetProductInfo(int id)
        {
            try
            {
                return _Conn.QueryFirstOrDefault<PRO_product_info>("select * from PRO_product_info where Id=" + id);
            }
            catch {
               return default(PRO_product_info);
            }
         
        }

        public PRO_loan_info WechatRecommand(int period)
        {
            var result = new List<PRO_loan_info>();

            var condition = $" pro_is_new=0 and pro_delsign=0 and pro_loan_period={period} and pro_is_product=0";
            condition += $" and BoHai=1";
            condition += $" and (pro_loan_state in ({DataDictionary.projectstate_Tender},{DataDictionary.projectstate_FullScalePending},{DataDictionary.projectstate_StayTransfer},{DataDictionary.projectstate_Repayment},{DataDictionary.projectstate_Settled},{DataDictionary.projectstate_Overdue},{DataDictionary.bank_state_using}))";
            var sbBuilder = new StringBuilder();
            sbBuilder.Append("select * from PRO_loan_info");
            sbBuilder.Append(" where" + condition);
            //sbBuilder.Append(" order by pro_public_date");

            var loanInfos= _Conn.Query<PRO_loan_info>(sbBuilder.ToString());

            if (loanInfos == null || loanInfos.Count() == 0)
            {
                return null;
            }
            //如果剩余可投金额>0,取发布时间最早的；如果标的列表全部投满，取最近发布的
            if (loanInfos.Where(p=>p.pro_surplus_money>0).Count()==0)
            {
                return loanInfos.OrderByDescending(p => p.pro_public_date).FirstOrDefault();
            }
            else
            {
                //return loanInfos.Where(p => p.pro_surplus_money > 0).OrderBy(p => p.pro_public_date).FirstOrDefault();
                return loanInfos.Where(p => p.pro_surplus_money > 0).OrderByDescending(p => p.pro_public_date).FirstOrDefault();
            }
        }

        public string GetCollateralInformation(int loanId)
        {
            var carCount = _Conn.QueryFirstOrDefault<int>($"SELECT Count(*) FROM PRO_car_info WHERE pro_loan_id = {loanId}");
            var houseCount = _Conn.QueryFirstOrDefault<int>($"SELECT Count(*) FROM PRO_house_info WHERE pro_loan_id = {loanId}");
            var otherCount = _Conn.QueryFirstOrDefault<int>($"SELECT Count(*) FROM PRO_other_info WHERE pro_loan_id = {loanId}");
            string collateralInformation = string.Empty;
            if (carCount != 0)
                collateralInformation += $"抵押车辆{carCount}辆；";
            if (houseCount != 0)
                collateralInformation += $"抵押房产{houseCount}所；";
            if (otherCount != 0)
                collateralInformation += $"其他抵押物{otherCount}个；";
            if (string.IsNullOrEmpty(collateralInformation))
                collateralInformation = "无抵押物";
            return collateralInformation;
        }

        #region 数据

        private PageDataView<PRO_loan_info> NewHandList(int page = 1, int pageSize = 5)
        {
            var criteria = new PageCriteria { Condition = "1=1" };
            criteria.Condition += " and pro_delsign=0 and pro_is_new=1 and pro_is_product=0";
            criteria.Condition += " and Bohai=1";
            criteria.Condition += $" and pro_loan_state in({DataDictionary.projectstate_Tender},{DataDictionary.projectstate_FullScalePending},{DataDictionary.projectstate_StayTransfer},{DataDictionary.projectstate_Repayment},{DataDictionary.projectstate_Settled},{DataDictionary.projectstate_Overdue},{DataDictionary.bank_state_using})";
            var innerTable = "PRO_loan_info";
            criteria.CurrentPage = page;
            criteria.PageSize = pageSize;
            criteria.TableName = innerTable;
            criteria.Sort = "pro_loan_state,pro_public_date desc";
            var pageData = GetPageData<PRO_loan_info>(criteria);
            return pageData;
        }

        private PageDataView<PRO_loan_info> RecommandList(int page = 1, int pageSize = 5)
        {
            var criteria = new PageCriteria { Condition = "1=1" };
            criteria.Condition += " and pro_delsign=0 and pro_is_new=0 and pro_is_product=0";
            criteria.Condition += " and Bohai=1";
            criteria.Condition += $" and pro_loan_state in({DataDictionary.projectstate_Tender},{DataDictionary.projectstate_FullScalePending},{DataDictionary.projectstate_StayTransfer},{DataDictionary.projectstate_Repayment},{DataDictionary.projectstate_Settled},{DataDictionary.projectstate_Overdue},{DataDictionary.bank_state_using})";
            var innerTable = "PRO_loan_info";
            criteria.CurrentPage = page;
            criteria.PageSize = pageSize;
            criteria.TableName = innerTable;
            criteria.Sort = "pro_loan_state,pro_public_date desc";
            var pageData = GetPageData<PRO_loan_info>(criteria);
            return pageData;
        }

        public List<PRO_loan_info> LoansByPeriods(int loanerId)
        {
            var sBuilder=new StringBuilder();
            sBuilder.Append("select * from PRO_loan_info");
            sBuilder.Append($" where pro_add_emp='{loanerId}' and");
            sBuilder.Append(" pro_loan_state in(20,21,23,24,25,734,794) and pro_delsign=0");
            var result = _Conn.Query<PRO_loan_info>(sBuilder.ToString()).ToList();
            return result;
        }

        #endregion 数据

        #region 用户

        public LoanStatisticsModel LoanStatistics(int id, List<LoanStatisticsType> statisticsItem)
        {
            if (statisticsItem.Count == 0)
                return new LoanStatisticsModel();

            #region 拼装统计字段
            var stateDictionary = new List<int> {
                    DataDictionary.projectstate_StaySend,
                    DataDictionary.projectstate_StayPlatformaudit,
                    DataDictionary.projectstate_Tender,
                    DataDictionary.projectstate_StayRelease,
                    DataDictionary.auditlink_Fullstandardaudit,
                    DataDictionary.projectstate_FullScalePending,
                    DataDictionary.projectstate_StayTransfer,
                    DataDictionary.projectstate_Overdue,
                    DataDictionary.projectstate_Repayment,
                    DataDictionary.projectstate_Settled
                };
            var listsqlOutParameters = new List<string>();
            if (statisticsItem.Contains(LoanStatisticsType.LoanCount))
                listsqlOutParameters.Add($"count(case when a.pro_loan_state in ({string.Join(",", stateDictionary)})  then a.Id end) as LoanCount");

            if (statisticsItem.Contains(LoanStatisticsType.LoanMoney))
                listsqlOutParameters.Add($"sum(case when a.pro_loan_state  in ({string.Join(",", stateDictionary)})   then a.pro_loan_money end) as LoanMoney");

            if (statisticsItem.Contains(LoanStatisticsType.BiddingCount))
                listsqlOutParameters.Add($"count(case when a.pro_loan_state in({DataDictionary.projectstate_StaySend},{DataDictionary.projectstate_StayPlatformaudit},{DataDictionary.projectstate_Tender},{DataDictionary.projectstate_StayRelease})  then a.Id end) as BiddingCount");

            if (statisticsItem.Contains(LoanStatisticsType.BiddingMoney))
                listsqlOutParameters.Add($"sum(case when a.pro_loan_state in({DataDictionary.projectstate_StaySend},{DataDictionary.projectstate_StayPlatformaudit},{DataDictionary.projectstate_Tender},{DataDictionary.projectstate_StayRelease})  then a.pro_loan_money end) as BiddingMoney");

            if (statisticsItem.Contains(LoanStatisticsType.FullCount))
                listsqlOutParameters.Add($"count(case when a.pro_loan_state in({DataDictionary.auditlink_Fullstandardaudit},{DataDictionary.projectstate_FullScalePending},{DataDictionary.projectstate_StayTransfer})  then a.Id end) as FullCount");

            if (statisticsItem.Contains(LoanStatisticsType.FullMoney))
                listsqlOutParameters.Add($"sum(case when a.pro_loan_state in({DataDictionary.auditlink_Fullstandardaudit},{DataDictionary.projectstate_FullScalePending},{DataDictionary.projectstate_StayTransfer})  then a.pro_loan_money end) as FullMoney");

            if (statisticsItem.Contains(LoanStatisticsType.RepaymentCount))
                listsqlOutParameters.Add($"count(case when a.pro_loan_state in({DataDictionary.projectstate_Overdue},{DataDictionary.projectstate_Repayment})  then a.Id end) as RepaymentCount");

            if (statisticsItem.Contains(LoanStatisticsType.RepaymentMoney))
                listsqlOutParameters.Add($"sum(case when a.pro_loan_state in({DataDictionary.projectstate_Overdue},{DataDictionary.projectstate_Repayment})  then a.pro_loan_money end) as RepaymentMoney");

            if (statisticsItem.Contains(LoanStatisticsType.ClearedCount))
                listsqlOutParameters.Add($"count(case when a.pro_loan_state = {DataDictionary.projectstate_Settled}  then a.Id end) as ClearedCount");

            if (statisticsItem.Contains(LoanStatisticsType.ClearedMoney))
                listsqlOutParameters.Add($"sum(case when a.pro_loan_state = {DataDictionary.projectstate_Settled}  then a.pro_loan_money end) as ClearedMoney");

            #endregion 拼装统计字段

            #region 拼装sql

            var sqlTableParameters = new StringBuilder(" from");
            sqlTableParameters.Append(" PRO_loan_info a");

            var sqlWhereParameters = new StringBuilder(" where");
            sqlWhereParameters.Append($" a.pro_add_emp = {id}");
            sqlWhereParameters.Append(" and a.pro_delsign = 0");

            var sqlString = new StringBuilder("select ");
            sqlString.Append(string.Join(",", listsqlOutParameters));
            sqlString.Append(sqlTableParameters);
            sqlString.Append(sqlWhereParameters);

            #endregion 拼装sql

            return _Conn.QueryFirstOrDefault<LoanStatisticsModel>(sqlString.ToString());
        }     

        public PageDataView<PRO_loan_info> GetPageByState(int id, IList<int> dataDictionary, int page = 1, int pageSize = 5)
        {
            var criteria = new PageCriteria();
            criteria.Condition += $" pro_add_emp = {id} ";
            criteria.Condition += " and pro_delsign = 0 ";
            if (dataDictionary != null) {
                criteria.Condition += $" and pro_loan_state in({string.Join(",", dataDictionary)})  ";
            }
            criteria.CurrentPage = page;
            criteria.PageSize = pageSize;
            criteria.Fields = " * ";
            criteria.PrimaryKey = "Id";
            criteria.TableName = " PRO_loan_info ";
            criteria.Sort = "pro_add_date desc";
            var pageData = GetPageData<PRO_loan_info>(criteria);
            return pageData;
        }

        public PageDataView<RLoanItemDetail> LoanItemDetail(int id, int page = 1, int pageSize = 5)
        {
            var criteria = new PageCriteria();
            criteria.Condition += "1=1";

            var innerTable = $@" (select
a.pro_loan_id as pro_loan_id,
sum(case when a.pro_is_clear = 1 then a.pro_collect_money + a.pro_collect_rate end) as RepayMoney,
sum(case when a.pro_is_clear = 0 then a.pro_pay_money + a.pro_pay_rate end) as WaitRepayMoney,
count(case when a.pro_is_clear = 0 then a.Id end) as WaitRepayPeriod
from PRO_loan_plan a
left join PRO_loan_info b on a.pro_loan_id = b.Id
where b.pro_add_emp = {id} and pro_loan_state in({DataDictionary.projectstate_Repayment},{DataDictionary.projectstate_Overdue}) and pro_delsign = 0
group by a.pro_loan_id) as a left join PRO_loan_info as b on a.pro_loan_id = b.Id";

            var selectSql = new StringBuilder();
            selectSql.Append(" b.Id as LoanId,");
            selectSql.Append(" b.pro_loan_use as LoanName,");
            selectSql.Append(" b.pro_loan_money as LoanMoney,");
            selectSql.Append(" b.pro_loan_period as LoanPeriod,");
            selectSql.Append(" b.pro_period_type as PeriodType,");
            selectSql.Append(" a.RepayMoney as RepayMoney,");
            selectSql.Append(" a.WaitRepayMoney as WaitRepayMoney,");
            selectSql.Append(" a.WaitRepayPeriod as WaitRepayPeriod");
            criteria.CurrentPage = page;
            criteria.PageSize = pageSize;
            criteria.Fields = selectSql.ToString();
            criteria.PrimaryKey = "b.Id";
            criteria.TableName = innerTable;
            criteria.Sort = "b.pro_add_date desc";
            var pageData = GetPageData<RLoanItemDetail>(criteria);
            return pageData;
        }

        public List<PRO_loan_info> GetProLoanInfoListByUserId(int userId)
        {
            var sqlStr = $@"SELECT * FROM PRO_loan_info where pro_delsign=0 and pro_add_emp={userId}
and pro_loan_state in ({DataDictionary.projectstate_Repayment},{DataDictionary.projectstate_Settled})";

            return _Conn.Query<PRO_loan_info>(sqlStr).ToList();
        }

        public PRO_loan_userInfo GetLoanUserInfo(int loanId)
        {
            using (var newConn= CreateConnection())
            {
                return newConn.QueryFirstOrDefault<PRO_loan_userInfo>($"select * from PRO_loan_userInfo where Id={loanId}");
            }
        }

        public PRO_product_info GetLoanProduct(int? loanType)
        {
            using (var newConn = CreateConnection())
            {
                return newConn.QueryFirstOrDefault<PRO_product_info>($"select * from PRO_product_info where Id={loanType}");
            }
        }

        public List<PRO_loan_repayfollow> GetRepayFollows(int loanId)
        {
            return _Conn.Query<PRO_loan_repayfollow>($"select * from PRO_loan_repayfollow where pro_loan_id={loanId} and pro_delsign=0 order by pro_loan_followdate desc,pro_loan_followtype").ToList();
        }

        public PRO_loan_info GetLoanInfoByMerBillNo(string merBillNo)
        {
            var sqlStr = $"SELECT * FROM PRO_loan_info where tran_seq_no='{merBillNo}'";
            return _Conn.Query<PRO_loan_info>(sqlStr).FirstOrDefault();
        }

        public List<PRO_loan_info> RecommandNewHandLoan(int count)
        {
            var sqlQuery=new StringBuilder();
            sqlQuery.Append($"select top({count}) * from pro_loan_info");
            sqlQuery.Append(" where pro_delsign=0 ");
            sqlQuery.Append("and pro_is_product=0 ");
            sqlQuery.Append("and BoHai=1 ");
            sqlQuery.Append("and pro_public_date>='2017-12-1' ");
            sqlQuery.Append(" and pro_is_new=1");
            sqlQuery.Append(" and pro_loan_state=20");
            return _Conn.Query<PRO_loan_info>(sqlQuery.ToString()).ToList();
        }

        public List<PRO_loan_info> RecommandCommonLoan(int count)
        {
            var sqlQuery = new StringBuilder();
            sqlQuery.Append($"select top({count}) * from pro_loan_info");
            sqlQuery.Append(" where pro_delsign=0 ");
            sqlQuery.Append("and pro_is_product=0 ");
            sqlQuery.Append(" and pro_is_new=0");
            sqlQuery.Append(" and pro_loan_state=20");
            return _Conn.Query<PRO_loan_info>(sqlQuery.ToString()).ToList();
        }

        public int HasLoan(int id)
        {
            string sqlString = $@"SELECT COUNT(*) FROM PRO_loan_info WHERE pro_delsign=0 and pro_add_emp = {id}";
            return _Conn.QueryFirstOrDefault<int>(sqlString);
        }

        public IList<CST_user_info> GetInvesterByLoanId(int loanId)
        {
            return _Conn.Query<CST_user_info>($@"SELECT distinct(B.Id) as Flag,B.* FROM PRO_invest_info AS A
LEFT JOIN CST_user_info AS B ON A.pro_invest_emp = B.Id
WHERE A.is_invest_succ = 1 and A.pro_delsign = 0 and A.pro_loan_id = {loanId}").ToList();
        }

        #endregion 用户

        /// <summary>
        /// 根据编号获取标的信息
        /// </summary>
        /// <param name="loanid"></param>
        /// <returns></returns>
        public PRO_loan_info GetLoanInfoByID(string loanid)
        {
            var sqlstr = $"SELECT * FROM dbo.PRO_loan_info where id='{loanid}'";
            var result = _Conn.QueryFirstOrDefault<PRO_loan_info>(sqlstr);
            return result;
        }

        public bool UpdateBohaiByID(string loanid)
        {
            throw new NotImplementedException();
        }
    }
}