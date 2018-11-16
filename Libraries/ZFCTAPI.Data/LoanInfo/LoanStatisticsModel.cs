using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.LoanInfo
{
    /// <summary>
    /// 借款统计模型
    /// </summary>
    public class LoanStatisticsModel
    {
        /// <summary>
        /// 借款数量
        /// </summary>
        public int LoanCount { get; set; }

        /// <summary>
        /// 借款总额
        /// </summary>
        public decimal LoanMoney { get; set; }

        /// <summary>
        /// 投标中数量
        /// </summary>
        public int BiddingCount { get; set; }

        /// <summary>
        /// 投标中总额
        /// </summary>
        public decimal BiddingMoney { get; set; }

        /// 满标中数量
        /// </summary>
        public int FullCount { get; set; }

        /// <summary>
        /// 满标中总额
        /// </summary>
        public decimal FullMoney { get; set; }

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
    }
}