using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZFCTAPI.Core.Enums
{
    /// <summary>
    /// 借款统计项目
    /// </summary>
    public enum LoanStatisticsType
    {
        /// <summary>
        /// 投标中数量
        /// </summary>
        LoanCount,

        /// <summary>
        /// 投标中总额
        /// </summary>
        LoanMoney,

        /// <summary>
        /// 投标中数量
        /// </summary>
        BiddingCount,

        /// <summary>
        /// 投标中总额
        /// </summary>
        BiddingMoney,

        /// <summary>
        /// 满标中数量
        /// </summary>
        FullCount,

        /// <summary>
        /// 满标中总额
        /// </summary>
        FullMoney,

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
        ClearedMoney
    }

    public enum LoanPlanStatisticsType
    {
        /// <summary>
        /// 今日待还
        /// </summary>
        TodayWaitRepay,

        /// <summary>
        /// 待还本金
        /// </summary>
        WaitRepayPrincipal,

        /// <summary>
        /// 待还利息
        /// </summary>
        WaitRepayRate,

        /// <summary>
        /// 待还服务费
        /// </summary>
        WaitRepayServiceFee,

        /// <summary>
        /// 待还总额（不包含罚息）
        /// </summary>
        WaitRepayTotal,

        /// <summary>
        /// 已还本金
        /// </summary>
        RepayPrincipal,

        /// <summary>
        /// 已还利息
        /// </summary>
        RepayRate,

        /// <summary>
        /// 已还服务费
        /// </summary>
        RepayServiceFee
    }
    #region 还款跟踪类型
    public enum RepayFollowType
    {
        /// <summary>
        /// 资金运用
        /// </summary>
        [Description("借款资金运用情况")]
        FundsUse = 1,
        /// <summary>
        /// 经营及财务
        /// </summary>
        [Description("借款人经营状况及财务状况")]
        ManageCapital = 2,
        /// <summary>
        /// 还款能力
        /// </summary>
        [Description("借款人还款能力变化情况")]
        RepayAbility = 3,
        /// <summary>
        /// 逾期
        /// </summary>
        [Description("借款人逾期情况")]
        Overdue = 4,
        /// <summary>
        /// 涉诉
        /// </summary>
        [Description("借款人涉诉情况")]
        Appeal = 5,

        /// <summary>
        /// 行政处罚
        /// </summary>
        [Description("借款人受行政处罚情况")]
        AdministrativeSanction = 6,

        /// <summary>
        /// 资金运用
        /// </summary>
        [Description("其他可能影响借款人还款的重大信息")]
        InfluenceRepayment = 7,

    }
    #endregion
}