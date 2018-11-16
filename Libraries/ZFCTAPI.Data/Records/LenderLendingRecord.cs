using System;
using System.Collections.Generic;
using System.Text;
using Dapper.Contrib.Extensions;
namespace ZFCTAPI.Data.Records
{
    /// <summary>
    /// 满标划转时还款人及标的信息
    /// </summary>
    [Table("LenderLendingRecord")]
    public class LenderLendingRecord:BaseEntity
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
        /// 标的借款金额
        /// </summary>
        public decimal BorrowerAmt { get; set; }
        /// <summary>
        /// 放款方式
        /// </summary>
        public string ReleaseType { get; set; }
        /// <summary>
        /// 借款人平台编号
        /// </summary>
        public string BorrowPlaCustId { get; set; }
        /// <summary>
        /// 借款人姓名
        /// </summary>
        public string BorrowCustName { get; set; }
        /// <summary>
        /// 借款手续费
        /// </summary>
        public string BorrowFee { get; set; }
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

        public int InvesterCount { get; set; }

        /// <summary>
        /// 银行返回描述
        /// </summary>
        public string BankDesc { get; set; }
    }
}
