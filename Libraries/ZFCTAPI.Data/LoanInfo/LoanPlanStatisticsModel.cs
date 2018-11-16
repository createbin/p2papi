using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.LoanInfo
{
    public class LoanPlanStatisticsModel
    {
        /// <summary>
        /// 今日待还
        /// </summary>
        public decimal TodayWaitRepay { get; set; }

        /// <summary>
        /// 待还本金
        /// </summary>
        public decimal WaitRepayPrincipal { get; set; }

        /// <summary>
        /// 待还利息
        /// </summary>
        public decimal WaitRepayRate { get; set; }

        /// <summary>
        /// 待还服务费
        /// </summary>
        public decimal WaitRepayServiceFee { get; set; }

        /// <summary>
        /// 待还总额
        /// </summary>
        public decimal WaitRepayTotal { get; set; }

        /// <summary>
        /// 已还本金
        /// </summary>
        public decimal RepayPrincipal { get; set; }

        /// <summary>
        /// 已还利息
        /// </summary>
        public decimal RepayRate { get; set; }

        /// <summary>
        /// 已还服务费
        /// </summary>
        public decimal RepayServiceFee { get; set; }
    }
}