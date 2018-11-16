using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.InvestInfo
{
    /// <summary>
    /// 用户投资统计
    /// </summary>
    public class InvestStatisticsModel
    {
        /// <summary>
        /// 用户投资总数
        /// </summary>
        public int InvestCount { get; set; }

        /// <summary>
        /// 用户投资总额
        /// </summary>
        public decimal InvestMoney { get; set; }

        /// <summary>
        /// 投标中数量
        /// </summary>
        public int BiddingCount { get; set; }

        /// <summary>
        /// 投标中总额
        /// </summary>
        public decimal BiddingMoney { get; set; }

        /// <summary>
        /// 还款中数量
        /// </summary>
        public int RepaymentCount { get; set; }

        /// <summary>
        /// 还款中总额
        /// </summary>
        public decimal RepaymentMoney { get; set; }

        /// <summary>
        /// 已结清数量
        /// </summary>
        public int ClearedCount { get; set; }

        /// <summary>
        /// 已结清总额
        /// </summary>
        public decimal ClearedMoney { get; set; }

        /// <summary>
        /// 债权转让投资数量
        /// </summary>
        public int TransInvestCount { get; set; }
    }
}