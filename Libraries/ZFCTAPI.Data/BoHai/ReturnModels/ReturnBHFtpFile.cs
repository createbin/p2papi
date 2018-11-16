using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.ReturnModels
{
    /// <summary>
    /// 投标对账 (ftp文件 )
    /// </summary>
    public class RBHBidReconciliation
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string TransId { get; set; }
        /// <summary>
        /// MercId
        /// </summary>
        public string MercId { get; set; }
        /// <summary>
        /// 账户存管平台ID
        /// </summary>
        public string PlaCustId { get; set; }
        /// <summary>
        /// 交易金额
        /// </summary>
        public string TransAmt { get; set; }
        /// <summary>
        /// 标的ID
        /// </summary>
        public string BorrowId { get; set; }
        /// <summary>
        /// 订单日期
        /// </summary>
        public string CreDt { get; set; }
        /// <summary>
        /// 订单时间
        /// </summary>
        public string CreTm { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public string OrdSts { get; set; }
        /// <summary>
        /// 商户流水号
        /// </summary>
        public string MerBillNo { get; set; }
    }

    /// <summary>
    /// 充值对账 (ftp文件 )
    /// </summary>
    public class RBHRechargeReconciliation
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrdNo { get; set; }
        /// <summary>
        /// 订单日期
        /// </summary>
        public string CreDt { get; set; }
        /// <summary>
        /// 账户存管平台ID
        /// </summary>
        public string PlaCustId { get; set; }
        /// <summary>
        /// 交易金额
        /// </summary>
        public string TransAmt { get; set; }
        /// <summary>
        /// 手续费
        /// </summary>
        public string FeeAmt { get; set; }
        /// <summary>
        /// 商户流水号
        /// </summary>
        public string MerBillNo { get; set; }
        /// <summary>
        /// 充值渠道
        /// </summary>
        public string ChargeCorg { get; set; }
    }

    /// <summary>
    /// 提现对账 (ftp文件)
    /// </summary>
    public class RBHWithDrawReconciliation
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrdNo { get; set; }
        /// <summary>
        /// 订单日期
        /// </summary>
        public string CreDt { get; set; }
        /// <summary>
        /// 账户存管平台ID
        /// </summary>
        public string PlaCustId { get; set; }
        /// <summary>
        /// 交易金额
        /// </summary>
        public string TransAmt { get; set; }
        /// <summary>
        /// 手续费
        /// </summary>
        public string FeeAmt { get; set; }
        /// <summary>
        /// 商户流水号
        /// </summary>
        public string MerBillNo { get; set; }
        /// <summary>
        /// 提现状态
        /// </summary>
        public string WdcSts { get; set; }
        /// <summary>
        /// 失败原因
        /// </summary>
        public string FalRsn { get; set; }
    }
    /// <summary>
    /// 实时红包对账
    /// </summary>
    public class RBHRedReconciliation
    {
        public string TransId { get; set; }

        public string MerBillNo { get; set; }

        public string CreDt { get; set; }

        public string TransAmt { get; set; }

        public string PlaCustId { get; set; }
    }
}
