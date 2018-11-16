using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.ApiModels.ReturnModels
{
    /// <summary>
    /// 可转出
    /// </summary>
    public class RCanTransfer
    {
        /// <summary>
        /// 投资编号
        /// </summary>
        public int InvestId { get; set; }

        /// <summary>
        /// 债权转让编号
        /// </summary>
        public int TransferId { get; set; }

        /// <summary>
        /// 可转份额
        /// </summary>
        public decimal CanTransferMoney { get; set; }

        /// <summary>
        /// 下个还款日
        /// </summary>
        public DateTime NextPayData { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string LoanName { get; set; }

        /// <summary>
        /// 利率
        /// </summary>
        public decimal LoanRate { get; set; }

        /// <summary>
        /// 待收本息
        /// </summary>
        public decimal WaitPrincipal { get; set; }

        /// <summary>
        /// 期数
        /// </summary>
        public decimal Period { get; set; }

        /// <summary>
        /// 折让率
        /// </summary>
        public decimal Discount { get; set; }

        /// <summary>
        /// 折让金额
        /// </summary>
        public decimal DiscountMoney { get; set; }

        /// <summary>
        /// 剩余天数
        /// </summary>
        public int SurplusDay { get; set; }

        /// <summary>
        /// 手续费率
        /// </summary>
        public decimal TransferRate { get; set; }


        /// <summary>
        /// 是否申请中
        /// </summary>
        public bool IsApply { get; set; }


    }

    /// <summary>
    /// 转出中
    /// </summary>
    public class RTransfering
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string LoanName { get; set; }

        /// <summary>
        /// 利率
        /// </summary>
        public decimal LoanRate { get; set; }

        /// <summary>
        /// 可转份额
        /// </summary>
        public decimal CanTransferMoney { get; set; }

        /// <summary>
        /// 期数
        /// </summary>
        public decimal Period { get; set; }

        /// <summary>
        /// 折让率
        /// </summary>
        public decimal Discount { get; set; }

        /// <summary>
        /// 折让金额
        /// </summary>
        public decimal DiscountMoney { get; set; }

        /// <summary>
        /// 手续费
        /// </summary>
        public decimal produceFee { get; set; }

        /// <summary>
        /// 认购金额
        /// </summary>
        public decimal BuyMoney { get; set; }

        /// <summary>
        /// 认购金额
        /// </summary>
        public decimal BuySpeed { get; set; }

        /// <summary>
        /// 转让时间
        /// </summary>
        public DateTime TransferData { get; set; }

        /// <summary>
        /// 到期时间
        /// </summary>
        public DateTime EndData { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string State { get; set; }
    }

    /// <summary>
    /// 已转出
    /// </summary>
    public class RTransferOut
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string LoanName { get; set; }

        /// <summary>
        /// 利率
        /// </summary>
        public decimal LoanRate { get; set; }

        /// <summary>
        /// 可转份额
        /// </summary>
        public decimal CanTransferMoney { get; set; }

        /// <summary>
        /// 期数
        /// </summary>
        public decimal Period { get; set; }

        /// <summary>
        /// 折让率
        /// </summary>
        public decimal Discount { get; set; }

        /// <summary>
        /// 折让金额
        /// </summary>
        public decimal DiscountMoney { get; set; }

        /// <summary>
        /// 手续费
        /// </summary>
        public decimal produceFee { get; set; }

        /// <summary>
        /// 认购金额
        /// </summary>
        public decimal BuyMoney { get; set; }

        /// <summary>
        /// 认购进度
        /// </summary>
        public decimal BuySpeed { get; set; }

        /// <summary>
        /// 转让时间
        /// </summary>
        public DateTime TransferData { get; set; }

        /// <summary>
        /// 到期时间
        /// </summary>
        public DateTime EndData { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string State { get; set; }
    }

    /// <summary>
    /// 已转入
    /// </summary>
    public class RTransferIn
    {
        /// <summary>
        /// 投资编号
        /// </summary>
        public int InvestId { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string LoanName { get; set; }

        /// <summary>
        /// 利率
        /// </summary>
        public decimal LoanRate { get; set; }

        /// <summary>
        /// 可转份额
        /// </summary>
        public decimal CanTransferMoney { get; set; }

        /// <summary>
        /// 期数
        /// </summary>
        public decimal Period { get; set; }

        /// <summary>
        /// 折让率
        /// </summary>
        public decimal Discount { get; set; }

        /// <summary>
        /// 折让金额
        /// </summary>
        public decimal DiscountMoney { get; set; }

        /// <summary>
        /// 手续费
        /// </summary>
        public decimal produceFee { get; set; }

        /// <summary>
        /// 认购金额
        /// </summary>
        public decimal BuyMoney { get; set; }

        /// <summary>
        /// 认购进度
        /// </summary>
        public decimal BuySpeed { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string State { get; set; }
    }
}
