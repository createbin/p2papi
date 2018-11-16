using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.InvestInfo
{
    public class InvestPlanStatisticsModel
    {
        /// <summary>
        /// 累计收益
        /// </summary>
        public decimal CumulativeIncome { get; set; }

        /// <summary>
        /// 近30天收益
        /// </summary>
        public decimal ThridDaysIncome { get; set; }

        /// <summary>
        /// 待收本金
        /// </summary>
        public decimal WaitReceivePrincipal { get; set; }

        /// <summary>
        /// 今日待收
        /// </summary>
        public decimal TodayWaitReceive { get; set; }

        /// <summary>
        /// 已收本金
        /// </summary>
        public decimal ReceivedPrincipal { get; set; }

        /// <summary>
        /// 待收收益
        /// </summary>
        public decimal WaitReceiveIncome { get; set; }

        /// <summary>
        /// 待收总额
        /// </summary>
        public decimal WaitReceiveTotal { get; set; }

        /// <summary>
        /// 未还清还款计划
        /// </summary>
        public int NoClearCount { get; set; }

        /// <summary>
        /// 最小的还款日期
        /// </summary>
        public DateTime? NextRePayDay { get; set; }
        
    }
}