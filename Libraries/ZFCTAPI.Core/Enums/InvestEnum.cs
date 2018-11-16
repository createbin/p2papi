using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Core.Enums
{
    /// <summary>
    /// 用户投资还款计划统计类型
    /// </summary>
    public enum InvestPlanStatisticsType
    {
        /// <summary>
        /// 累计收益
        /// </summary>
        CumulativeIncome,

        /// <summary>
        /// 累计收益
        /// </summary>
        ThridDaysIncome,

        /// <summary>
        /// 待收本金
        /// </summary>
        WaitReceivePrincipal,

        /// <summary>
        /// 今日待收
        /// </summary>
        TodayWaitReceive,

        /// <summary>
        /// 已收本金
        /// </summary>
        ReceivedPrincipal,

        /// <summary>
        /// 待收收益
        /// </summary>
        WaitReceiveIncome,

        /// <summary>
        /// 待收总额
        /// </summary>
        WaitReceiveTotal,

        /// <summary>
        /// 未还清还款计划
        /// </summary>
        NoClearCount,

        /// <summary>
        /// 最小还款日期
        /// </summary>
        NextRePayDay,
    }

    /// <summary>
    /// 用户投资统计类型
    /// </summary>
    public enum InvestStatisticsType
    {
        /// <summary>
        /// 用户投资总数
        /// </summary>
        InvestCount,

        /// <summary>
        /// 用户投资总额
        /// </summary>
        InvestMoney,

        /// <summary>
        /// 投标中数量
        /// </summary>
        BiddingCount,

        /// <summary>
        /// 投标中总额
        /// </summary>
        BiddingMoney,

        /// <summary>
        /// 还款中数量
        /// </summary>
        RepaymentCount,

        /// <summary>
        /// 还款中总额
        /// </summary>
        RepaymentMoney,

        /// <summary>
        /// 已结清数量
        /// </summary>
        ClearedCount,

        /// <summary>
        /// 已结清总额
        /// </summary>
        ClearedMoney,

        /// <summary>
        /// 债权转让投资数量
        /// </summary>
        TransInvestCount
    }
}