using System;
using System.Collections.Generic;
using System.Text;
using Dapper.Contrib.Extensions;
namespace ZFCTAPI.Data.Records
{
    /// <summary>
    /// 还款时投资人及其标的信息
    /// </summary>
    [Table("InvestorRepaymentRecord")]
    public class InvestorRepaymentRecord:BaseEntity
    {

        public string MerBillNo { get; set; }
        /// <summary>
        /// 项目Id
        /// </summary>
        public int BorrowId { get; set; }
        /// <summary>
        /// 项目编号
        /// </summary>
        public string BorrowNo { get; set; }
        /// <summary>
        /// 项目期数
        /// </summary>
        public int BorrowPeriod { get; set; }
        /// <summary>
        /// 发生时间
        /// </summary>
        public DateTime CreDt { get; set; }
        /// <summary>
        /// 入账客户号
        /// </summary>
        public string CreditPlaCustId { get; set; }
        /// <summary>
        /// 入账客户名称
        /// </summary>
        public string CreditName { get; set; }
        /// <summary>
        /// 出账客户号
        /// </summary>
        public string OutgoingPlaCustId { get; set; }
        /// <summary>
        /// 出账客户名称
        /// </summary>
        public string OutgoingName { get; set; }
        /// <summary>
        /// 交易本金
        /// </summary>
        public decimal TransAmt { get; set; }
        /// <summary>
        /// 交易利息
        /// </summary>
        public string Interest { get; set; }
        /// <summary>
        /// 交易罚息
        /// </summary>
        public string PenaltyInterest { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
