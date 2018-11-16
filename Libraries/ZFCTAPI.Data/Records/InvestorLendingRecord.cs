using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper.Contrib.Extensions;

namespace ZFCTAPI.Data.Records
{
    /// <summary>
    /// 满标划转时投资人信息
    /// </summary>
    [Table("InvestorLendingRecord")]
    public class InvestorLendingRecord:BaseEntity
    {

        public string MerBillNo { get; set; }
        /// <summary>
        /// 借款id
        /// </summary>
        public int BorrowId { get; set; }
        /// <summary>
        /// 创建时间
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
        /// 交易金额
        /// </summary>
        public decimal TransAmt { get; set; }
        /// <summary>
        /// 冻结的订单号
        /// </summary>
        public string FreezeId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
