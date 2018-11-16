using System;
using System.Collections.Generic;
using System.Text;
using Dapper.Contrib.Extensions;

namespace ZFCTAPI.Data.Records
{
    /// <summary>
    /// 还款时还款人信息
    /// </summary>
    [Table("LenderLendingRecord")]
    public class LenderRepaymentRecord:BaseEntity
    {
        /// <summary>
        /// 标的id
        /// </summary>
        public int BorrowId { get; set; }

        /// <summary>
        /// 满标划转订单号
        /// </summary>
        public string MerBillNo { get; set; }
        /// <summary>
        /// 标的编号
        /// </summary>
        public string BorrowNo { get; set; }
        /// <summary>
        /// 项目期数
        /// </summary>
        public int BorrowPeriod { get; set; }
        /// <summary>
        /// 还款金额
        /// </summary>
        public decimal TransAmt { get; set; }
        /// <summary>
        /// 还款手续费
        /// </summary>
        public decimal FeeAmt { get; set; }
        /// <summary>
        /// 还款人平台号
        /// </summary>
        public string BorrowPlaCustId { get; set; }
        /// <summary>
        /// 还款人姓名
        /// </summary>
        public string BorrowCustName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 1 个人，2 企业
        /// </summary>
        public int Attribute { get; set; }

        /// <summary>
        /// 还款是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 银行返回描述
        /// </summary>
        public string BankDesc { get; set; }

        public int InvesterCount { get; set; }
    }
}
