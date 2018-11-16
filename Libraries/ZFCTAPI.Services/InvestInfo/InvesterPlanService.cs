using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using ZFCTAPI.Data.PRO;
using ZFCTAPI.Services.Repositorys;
using ZFCTAPI.Data.InvestInfo;
using ZFCTAPI.Core.Enums;
using ZFCTAPI.Core;
using ZFCTAPI.Services.LoanInfo;
using ZFCTAPI.Core.Helpers;
using System.Data;

namespace ZFCTAPI.Services.InvestInfo
{
    public interface IInvesterPlanService : IRepository<PRO_invester_plan>
    {
        /// <summary>
        /// 获取用户的盈利总额
        /// </summary>
        /// <returns></returns>
        decimal SumUserGains();

        /// <summary>
        /// 用户投资还款计划统计
        /// </summary>
        /// <param name="id">用户编号</param>
        /// <param name="statisticsItem">统计项目</param>
        /// <returns></returns>
        InvestPlanStatisticsModel InvestPlanStatistics(int id, List<InvestPlanStatisticsType> statisticsItem);

        /// <summary>
        /// 计算投资用户应获罚息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        decimal CalculationOverRate(int id);

        List<PRO_invester_plan> GetListByCondition(bool? isClear, int investId = 0);

        /// <summary>
        /// 根据还款计划查询投资计划
        /// </summary>
        /// <param name="loanId"></param>
        /// <returns></returns>
        List<PRO_invester_plan> GetListByPlanId(int planId);

        /// <summary>
        /// 查询还款计划明细
        /// </summary>
        /// <param name="id">用户编号</param>
        /// <param name="isClear">0 未结清 1已结清</param>
        /// <returns></returns>
        IEnumerable<InvestPlanDetail> GetDetailListByUserId(int id, int isClear);

        /// <summary>
        /// 生成收回计划表
        /// </summary>
        /// <param name="loanInfo"></param>
        /// <param name="investList"></param>
        /// <param name="loanPlanList"></param>
        /// <param name="tips"></param>
        /// <param name="isAdd"></param>
        /// <returns></returns>
        List<PRO_invester_plan> RecoveryPlanTable(PRO_loan_info loanInfo, List<PRO_invest_info> investList, List<PRO_loan_plan> loanPlanList, ref string tips, bool isAdd = false, IDbTransaction transaction = null);

        /// <summary>
        /// 根据标的信息查询回款计划
        /// </summary>
        /// <param name="loanId"></param>
        /// <returns></returns>
        List<PRO_invester_plan> GetInvesterPlanByLoanId(int loanId);

        /// <summary>
        /// 获取 收益复投利息
        /// </summary>
        /// <param name="investMoney"></param>
        /// <param name="rate"></param>
        /// <param name="loan_period"></param>
        /// <param name="planCount"></param>
        /// <returns></returns>
        decimal getloanPlanRate(decimal investMoney, decimal rate, int loan_period, int planCount);

        /// <summary>
        /// huou
        /// </summary>
        /// <param name="loanId"></param>
        /// <returns></returns>
        IList<PRO_invester_plan> GetCurrentDayPlan(int userId);


    }

    public class InvesterPlanService : Repository<PRO_invester_plan>, IInvesterPlanService
    {
        private readonly ILoanPlanService _loanPlanService;
        private readonly ILoanInfoService _iloanInfoService;

        public InvesterPlanService(ILoanPlanService loanPlanService,
            ILoanInfoService iloanInfoService)
        {
            _loanPlanService = loanPlanService;
            _iloanInfoService = iloanInfoService;
        }

        public InvestPlanStatisticsModel InvestPlanStatistics(int id, List<InvestPlanStatisticsType> statisticsItem)
        {
            if (statisticsItem.Count == 0)
                return new InvestPlanStatisticsModel();

            #region 拼装统计字段

            var listsqlOutParameters = new List<string>();
            if (statisticsItem.Contains(InvestPlanStatisticsType.CumulativeIncome))
                listsqlOutParameters.Add("sum(case when a.pro_is_clear = 1 then a.pro_collect_rate+a.pro_collect_over_rate end) as CumulativeIncome");

            if (statisticsItem.Contains(InvestPlanStatisticsType.ThridDaysIncome))
                listsqlOutParameters.Add($"sum(case when a.pro_is_clear = 1 and a.pro_collect_date >='{DateTime.Now.AddDays(-30).Date.ToString("yyyy/MM/dd HH:mm:ss")}' then a.pro_collect_rate+a.pro_collect_over_rate end) as ThridDaysIncome");

            if (statisticsItem.Contains(InvestPlanStatisticsType.WaitReceivePrincipal))
                listsqlOutParameters.Add("sum(case when a.pro_is_clear = 0 then a.pro_pay_money end) as WaitReceivePrincipal");

            //今天的本金+利息
            if (statisticsItem.Contains(InvestPlanStatisticsType.TodayWaitReceive))
                listsqlOutParameters.Add("sum(case when a.pro_is_clear = 0 and DateDiff(dd,a.pro_pay_date,getdate())>=0 then a.pro_pay_money+a.pro_pay_rate end)  as TodayWaitReceive");

            if (statisticsItem.Contains(InvestPlanStatisticsType.WaitReceiveIncome))
                listsqlOutParameters.Add("sum(case when a.pro_is_clear = 0  then a.pro_pay_rate end) as WaitReceiveIncome");

            if (statisticsItem.Contains(InvestPlanStatisticsType.ReceivedPrincipal))
                listsqlOutParameters.Add("sum(case when a.pro_is_clear = 1 then a.pro_collect_money end) as ReceivedPrincipal");

            if (statisticsItem.Contains(InvestPlanStatisticsType.NoClearCount))
                listsqlOutParameters.Add("count(case when a.pro_is_clear = 0 then a.Id end) as NoClearCount");

            if (statisticsItem.Contains(InvestPlanStatisticsType.WaitReceiveTotal))
                listsqlOutParameters.Add("sum(case when a.pro_is_clear = 0 then a.pro_pay_money+pro_pay_rate end) as WaitReceiveTotal");
            if (statisticsItem.Contains(InvestPlanStatisticsType.NextRePayDay))
                listsqlOutParameters.Add("max(case when a.pro_is_clear = 0 then a.pro_pay_date end) as NextRePayDay");

            #endregion 拼装统计字段

            #region 拼装sql

            var sqlTableParameters = new StringBuilder(" from");
            sqlTableParameters.Append(" PRO_invester_plan a");
            sqlTableParameters.Append(" left join PRO_invest_info b on a.pro_invest_id = b.Id");

            var sqlWhereParameters = new StringBuilder(" where");
            sqlWhereParameters.Append($" b.pro_invest_emp = {id}");
            sqlWhereParameters.Append(" and b.is_invest_succ =1 ");
            sqlWhereParameters.Append(" and b.pro_delsign = 0");

            var sqlString = new StringBuilder("select ");
            sqlString.Append(string.Join(",", listsqlOutParameters));
            sqlString.Append(sqlTableParameters);
            sqlString.Append(sqlWhereParameters);

            #endregion 拼装sql

            return _Conn.QueryFirstOrDefault<InvestPlanStatisticsModel>(sqlString.ToString());
        }

        public decimal SumUserGains()
        {
            var sum = _Conn.QueryFirst<decimal>(
                "select SUM(pro_collect_rate+pro_collect_over_rate) from PRO_invester_plan where pro_invest_id>0");
            return sum;
        }

        public List<PRO_invester_plan> GetListByCondition(bool? isClear, int investId = 0)
        {
            #region 查询条件

            var sqlStence = "1=1";
            if (isClear != null)
            {
                sqlStence += $" and pro_is_clear='{isClear}'";
            }
            if (investId != 0)
            {
                sqlStence += $" and pro_invest_id='{investId}'";
            }

            #endregion 查询条件

            var builder = new StringBuilder();
            builder.Append("select * from PRO_invester_plan");
            builder.Append(" where " + sqlStence);
            var result = _Conn.Query<PRO_invester_plan>(builder.ToString()).ToList();
            return result;
        }

        public decimal CalculationOverRate(int id)
        {
            string sqlString = $@"select

a.InvestPayRate,
b.pro_sett_over_rate as SurplusOverRate,
b.pro_pay_date as PayDate,
b.pro_pay_money as PayMoney,
b.pro_pay_rate as PayRate,
b.pro_collect_date as CollectDate

from
(select
sum(a.pro_pay_rate) as InvestPayRate ,
a.pro_plan_id from PRO_invester_plan a
left join PRO_loan_plan b on a.pro_plan_id = b.Id
left join PRO_invest_info c on a.pro_invest_id = c.Id
where b.pro_is_clear=0 and c.pro_invest_emp={id}
and b.pro_pay_type != {DataDictionary.repaymenstate_Replace}
and b.pro_pay_type != {DataDictionary.RepaymentType_PlatformDaihuan}
and b.pro_pay_type != {DataDictionary.RepaymentType_GuarRepayDaihuan}
and b.pro_pay_type != {DataDictionary.RepaymentType_CompulsoryrepayDaihuan}
group by a.pro_plan_id )as a

left join PRO_loan_plan as b on a.pro_plan_id = b.Id";

            var query = _Conn.Query<CalculationInvestPlanOverRateModel>(sqlString);

            if (query == null)
                return 0.00m;

            return Math.Round(query.Sum(p =>
            {
                if (p.PayRate == 0 || p.InvestPayRate == 0)
                    return 0.00m;

                var loanOverRate = _loanPlanService.CalculationOverRate(new Data.LoanInfo.CalculationLoanPlanOverRateRequest
                {
                    SurplusOverRate = p.SurplusOverRate,
                    CollectDate = p.CollectDate,
                    PayDate = p.PayDate,
                    PayMoney = p.PayMoney,
                    PayRate = p.PayRate
                });

                if (loanOverRate == 0)
                    return 0.00m;

                return loanOverRate / p.PayRate * p.InvestPayRate;
            }), 2);
        }

        public List<PRO_invester_plan> GetListByPlanId(int planId)
        {
            var sqlStr = "SELECT * FROM PRO_invester_plan where pro_plan_id=" + planId;
            return _Conn.Query<PRO_invester_plan>(sqlStr).ToList();
        }

        public IEnumerable<InvestPlanDetail> GetDetailListByUserId(int id, int isClear)
        {
            return _Conn.Query<InvestPlanDetail>($@"select
c.pro_loan_use as LoanName,
a.pro_pay_date as PayDate,
a.pro_collect_date as CollectDate,
a.pro_pay_total as PayTotal,
a.pro_collect_total as CollectTotal,
c.pro_pay_type as RepayType
from PRO_invester_plan a
left join PRO_invest_info b on a.pro_invest_id = b.Id
left join PRO_loan_info c on b.pro_loan_id = c.Id
where b.pro_invest_emp ={id} and b.is_invest_succ =1 and b.pro_delsign = 0 and a.pro_is_clear ={isClear}");
        }

        /// <summary>
        /// 生成收回计划表
        /// </summary>
        /// <param name="loanInfo">贷款信息</param>
        /// <param name="investList">投资记录表</param>
        /// <param name="loanPlanList">还款计划表</param>
        /// <param name="isAdd">是否保存到数据库</param>
        /// <returns></returns>
        public List<PRO_invester_plan> RecoveryPlanTable(PRO_loan_info loanInfo, List<PRO_invest_info> investList, List<PRO_loan_plan> loanPlanList, ref string tips, bool isAdd = false, IDbTransaction transaction = null)
        {

            #region 相关变量、对象
            bool flag = true;                           //是否继续执行

            List<PRO_invester_plan> investerList = new List<PRO_invester_plan>();       //投资人收回计划表
            List<PRO_invester_plan> investerListTemp = new List<PRO_invester_plan>();   //投资人收回计划表
            #endregion

            #region 生成计划表前判断
            if (loanInfo == null)
            {
                tips = "不存在该项目！";
                flag = false;
            }

            #endregion

            #region 生成计划表
            if (flag)
            {
                #region 相关变量
                //投资人 期数 计划收回日 应收本金 应收利息 应收总额
                int intInvestNo = investList.Count;//投资人数
                decimal decLoanMoney = (decimal)loanInfo.pro_loan_money;//借款金额
                decimal decInvestMoney = 0.00m;//投资金额
                decimal decInvestRatio = 0.00m;//投资占比
                int intInvestId = 0;//投资记录表ID
                decimal decMoney = 0.00m;//应收本金
                decimal decRate = 0.00m;//应收利息
                decimal decTotal = 0.00m;//应收总额
                decimal experience = 0.00m;//体验金

                #endregion


                //判断是否进行收益复投

                investList = investList.OrderByDescending(p => p.pro_is_repeat).ToList();

                #region 循环还款计划表给投资人收回计划表赋值
                for (int j = 0; j < loanPlanList.Count; j++)
                {
                    decimal decMoneyTotal = 0.00m;//应收回本金合计（除去最后一个投资人）
                    decimal decTotInvRate = 0.00m;//当期收回总利息（除去最后一个投资人）

                    decimal decPayMoney = (decimal)loanPlanList[j].pro_pay_rate; //最新还款 利息

                    //循环投资人
                    for (int i = 0; i < investList.Count; i++)
                    {
                        intInvestId = investList[i].Id;//投资记录表ID

                        //判断是否进行收益复投
                        if (!investList[i].pro_is_repeat)
                        {

                            decLoanMoney = investList.Where(p => !p.pro_is_repeat).Sum(p => p.pro_credit_money).GetValueOrDefault();

                            #region 收益返还计算收回计划

                            decInvestMoney = (decimal)investList[i].pro_credit_money;//投资金额
                            experience = (investList[i].pro_experience_money == null) ? 0 : (decimal)investList[i].pro_experience_money;//体验金额度
                            /* 投资占比 = 投资金额 / 借款金额 */
                            decInvestRatio = decInvestMoney / decLoanMoney;
                            if (i == investList.Count - 1)
                            {
                                /* 应收本金 = 应还本金 - 应收本金合计（最后一个投资人前） */
                                decMoney = (decimal)loanPlanList[j].pro_pay_money - decMoneyTotal;
                                decMoney = CommonHelper.Round((Decimal)(decMoney), 2);  //应收本金（四舍五入）
                                /* 应收利息 = 投资占比 * 应还利息 */
                                decRate = (decimal)loanPlanList[j].pro_pay_rate - decTotInvRate;
                                decRate = Decimal.Parse(CommonHelper.getInterest(decRate, 0));//应收利息
                            }
                            else
                            {
                                /* 应收本金 = 投资占比 * 应还本金 */
                                decMoney = decInvestRatio * (decimal)loanPlanList[j].pro_pay_money;
                                decMoney = CommonHelper.Round((Decimal)(decMoney), 2);  //应收本金（四舍五入）
                                decMoneyTotal += decMoney;



                                /* 应收利息 = 投资占比 * 应还利息 */
                                decRate = decInvestRatio * (decimal)loanPlanList[j].pro_pay_rate;
                                decRate = Decimal.Parse(CommonHelper.getInterest(decRate, 0));//应收利息
                                decTotInvRate += decRate;
                            }

                            /* 应收总额 = 应收本金 + 应收利息 */
                            decTotal = decMoney + decRate;

                            PRO_invester_plan investerPlan = new PRO_invester_plan();

                            investerPlan.pro_loan_period = loanPlanList[j].pro_loan_period;//期数
                            investerPlan.pro_plan_id = loanPlanList[j].Id;//借款人还款计划表ID
                            investerPlan.pro_pay_date = loanPlanList[j].pro_pay_date;//应收日期

                            investerPlan.pro_invest_id = intInvestId;//投资记录表ID
                            investerPlan.pro_pay_money = decMoney;//应收本金
                            investerPlan.pro_pay_rate = decRate;//应收利息
                            investerPlan.pro_pay_total = decTotal;//应收总额 intUserID
                            investerPlan.pro_is_clear = false;//是否结清(1：是 0：否)


                            /*//2018/2/5

                            if (NopVersion.CurrentVersion == "3.0.0.3")
                            {
                                if (experience != 0 && decMoney != 0)
                                {
                                    if (i == investList.Count - 1)
                                    {
                                        investerPlan.pro_experience_money = experience - Math.Round(experience / loanPlanList.Count(x => x.pro_pay_money > 0), 2) * i;
                                    }
                                    else
                                        investerPlan.pro_experience_money = experience / loanPlanList.Count(x => x.pro_pay_money > 0);//本期应还体验金
                                }
                                else
                                    investerPlan.pro_experience_money = 0;
                            }
                            */

                            investerList.Add(investerPlan);
                            #endregion
                        }
                        else
                        {
                            #region 收益复投计算收回计划

                            decInvestMoney = (decimal)investList[i].pro_credit_money;//投资金额
                            experience = (investList[i].pro_experience_money == null) ? 0 : (decimal)investList[i].pro_experience_money;//体验金额度
                            /* 投资占比 = 投资金额 / 借款金额 */
                            decInvestRatio = decInvestMoney / decLoanMoney;

                            if (j == loanPlanList.Count - 1)
                            {
                                var tempLoanInfo = _iloanInfoService.Find(investList[i].pro_loan_id.Value);//2018/2/5
                                #region 最后一期
                                //最后一个人?
                                if (i == investList.Count - 1)
                                {
                                    /* 应收本金 = 应还本金 - 应收本金合计（最后一个投资人前） */
                                    decMoney = (decimal)loanPlanList[j].pro_pay_money - decMoneyTotal;
                                    decMoney = CommonHelper.Round((Decimal)(decMoney), 2);  //应收本金（四舍五入）


                                    
                                    /* 应收利息 = 总计算 */
                                    decRate = getloanPlanRate((decimal)investList[i].pro_invest_money, (decimal)tempLoanInfo.pro_loan_rate, (int)tempLoanInfo.pro_loan_period, loanPlanList.Count);
                                    decRate = Decimal.Parse(CommonHelper.getInterest(decRate, 0));//应收利息

                                }
                                else
                                {
                                    /* 应收本金 = 投资占比 * 应还本金 */
                                    decMoney = decInvestRatio * (decimal)loanPlanList[j].pro_pay_money;
                                    decMoney = CommonHelper.Round((Decimal)(decMoney), 2);  //应收本金（四舍五入）
                                    decMoneyTotal += decMoney;

                                    /* 应收利息 = 投资占比 * 应还利息 */
                                    decRate = getloanPlanRate((decimal)investList[i].pro_invest_money, (decimal)tempLoanInfo.pro_loan_rate, (int)tempLoanInfo.pro_loan_period, loanPlanList.Count());

                                    decRate = Decimal.Parse(CommonHelper.getInterest(decRate, 0));//应收利息
                                    decTotInvRate += decRate;

                                    //应还利息= 原利息
                                    decPayMoney += (decimal)loanPlanList[j].pro_pay_rate * (1 - decInvestRatio) + decRate;
                                    //loanPlanList[j].pro_pay_rate += decRate;
                                }
                                #endregion
                            }
                            else
                            {

                                #region 中间期数
                                //最后一个人?
                                if (i == investList.Count - 1)
                                {
                                    /* 应收本金 = 应还本金 - 应收本金合计（最后一个投资人前） */
                                    decMoney = (decimal)loanPlanList[j].pro_pay_money - decMoneyTotal;
                                    decMoney = CommonHelper.Round((Decimal)(decMoney), 2);  //应收本金（四舍五入）
                                    /* 应收利息 = 投资占比 * 应还利息 */
                                    decRate = (decimal)loanPlanList[j].pro_pay_rate - decTotInvRate;
                                    decRate = Decimal.Parse(CommonHelper.getInterest(decRate, 0));//应收利息
                                }
                                else
                                {
                                    /* 应收本金 = 投资占比 * 应还本金 */
                                    decMoney = decInvestRatio * (decimal)loanPlanList[j].pro_pay_money;
                                    decMoney = CommonHelper.Round((Decimal)(decMoney), 2);  //应收本金（四舍五入）
                                    decMoneyTotal += decMoney;

                                    /* 应收利息 = 投资占比 * 应还利息 */
                                    decRate = 0.00m;// decInvestRatio * (decimal)loanPlanList[j].pro_pay_rate;
                                    decRate = Decimal.Parse(CommonHelper.getInterest(decRate, 0));//应收利息
                                    decTotInvRate += decRate;
                                }
                                #endregion
                            }

                            /* 应收总额 = 应收本金 + 应收利息 */
                            decTotal = decMoney + decRate;

                            PRO_invester_plan investerPlan = new PRO_invester_plan();

                            investerPlan.pro_loan_period = loanPlanList[j].pro_loan_period;//期数
                            investerPlan.pro_plan_id = loanPlanList[j].Id;//借款人还款计划表ID
                            investerPlan.pro_pay_date = loanPlanList[j].pro_pay_date;//应收日期

                            investerPlan.pro_invest_id = intInvestId;//投资记录表ID
                            investerPlan.pro_pay_money = decMoney;//应收本金
                            investerPlan.pro_pay_rate = decRate;//应收利息
                            investerPlan.pro_pay_total = decTotal;//应收总额 intUserID

                            if (decTotal > 0)
                            {
                                investerPlan.pro_is_clear = false;//是否结清(1：是 0：否)
                            }
                            else
                            {
                                investerPlan.pro_is_clear = true;
                            }

                            /*//2018/2/5
                            if (NopVersion.CurrentVersion == "3.0.0.3")
                            {
                                if (experience != 0 && decMoney != 0)
                                {
                                    if (i == investList.Count - 1)
                                    {
                                        investerPlan.pro_experience_money = experience - Math.Round(experience / loanPlanList.Count(x => x.pro_pay_money > 0), 2) * i;
                                    }
                                    else
                                        investerPlan.pro_experience_money = experience / loanPlanList.Count(x => x.pro_pay_money > 0);//本期应还体验金
                                }
                                else
                                    investerPlan.pro_experience_money = 0;
                            }
                            */
                            investerList.Add(investerPlan);
                            #endregion
                        }
                    }
                }
                #endregion

                #region 矫正投资人收回计划表数据

                #region **矫正思路**
                /*矫正思路：
                 *  1.得到投资人的“投资金额”
                 *  2.得到投资人收回计划表的“应还本金和”
                 *  3.算出“差额”（差额 = “投资金额”-“应还本金和”）
                 *  4.如果“差额”不等于0，修改投资人收回计划表最后一期的“应还本金”和“应还总额”
                 */
                #endregion

                for (int i = 0; i < investList.Count; i++)
                {
                    if (investerList != null && investerList.Count > 0)
                    {
                        decimal decMoneyTemp = 0.00m;//应还本金（和）
                        decimal decBalance = 0.00m;//差额 = 投资金额 - 应还本金（和）

                        intInvestId = investList[i].Id;//投资记录表ID
                        decInvestMoney = (decimal)investList[i].pro_credit_money;//投资金额
                        investerListTemp = investerList.FindAll(it => it.pro_invest_id == intInvestId);
                        if (investerListTemp != null)
                        {
                            for (int j = 0; j < investerListTemp.Count; j++)
                            {
                                decMoneyTemp += (decimal)investerListTemp[j].pro_pay_money;//应还本金

                                if (j == investerListTemp.Count - 1)//最后一题记录
                                {
                                    decBalance = decInvestMoney - decMoneyTemp;//差额 = 投资金额 - 应还本金（和）
                                    if (decBalance != 0)
                                    {
                                        /*投资人收回计划表
                                         *  字段说明
                                         *      pro_invest_id：投资记录ID；pro_plan_id：借款人还款计划表ID；
                                         *      pro_loan_period：期数；pro_pay_money：应还本金
                                         *      pro_pay_total：应还总额
                                         */
                                        investerList.FindAll(it => it.pro_invest_id == intInvestId && it.pro_plan_id == investerListTemp[j].pro_plan_id
                                            && it.pro_loan_period == investerListTemp[j].pro_loan_period)[0].pro_pay_money += decBalance;//应还本金
                                        investerList.FindAll(it => it.pro_invest_id == intInvestId && it.pro_plan_id == investerListTemp[j].pro_plan_id
                                            && it.pro_loan_period == investerListTemp[j].pro_loan_period)[0].pro_pay_total += decBalance;//应还总额
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion

                if (investerList.Count == 0)
                {
                    tips = "未知原因，未能生成收回记录！";
                    flag = false;
                }

                if (isAdd)
                {
                    foreach (PRO_invester_plan item in investerList)
                    {
                        this.Add(item, transaction);
                    }
                }
            }
            #endregion

            return investerList;
        }

        /// <summary>
        /// 获取 收益复投利息
        /// </summary>
        /// <param name="investMoney">投资金额</param>
        /// <param name="rate">收益率</param>
        /// <param name="loan_period"></param>
        /// <returns></returns>
        public decimal getloanPlanRate(decimal investMoney, decimal rate, int loan_period, int planCount)
        {
            decimal endRate = 0.00m; //最终结算金额
            int loanMon = loan_period / planCount;
            for (int i = 0; i < planCount; i++)
            {
                decimal JsRate = CommonHelper.Round(investMoney * rate * loanMon / 1200, 2);  //收益 =  投资金额 * 收益率 /12
                investMoney += JsRate;                       // 投资金额= 投资金额+本期利息 
                endRate += JsRate;
            }
            return endRate;

        }

        public List<PRO_invester_plan> GetInvesterPlanByLoanId(int loanId)
        {
            var sqlStr = $"SELECT P.* FROM PRO_invest_info I join PRO_invester_plan P ON I.Id = P.pro_invest_id WHERE I.is_invest_succ=1 AND I.PRO_LOAN_ID = {loanId}";

            return _Conn.Query<PRO_invester_plan>(sqlStr).ToList();
        }

        public IList<PRO_invester_plan> GetCurrentDayPlan(int userId)
        {
            return _Conn.Query<PRO_invester_plan>($@"SELECT * FROM PRO_invester_plan AS A 
LEFT JOIN PRO_invest_info AS B ON A.pro_invest_id = B.Id
WHERE B.pro_delsign = 0 AND B.pro_invest_emp = {userId} AND DATEDIFF(DD, A.pro_collect_date, GETDATE()) = 0 AND A.pro_is_clear = 1").ToList();
        }
    }
}