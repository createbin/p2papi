using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.ApiModels.ReturnModels
{
    /// <summary>
    /// 招标中标
    /// </summary>
    public class RBiddingLoan
    {
        /// <summary>
        /// 项目编号
        /// </summary>
        public int LoanId { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string LoanName { get; set; }

        /// <summary>
        /// 借款利率
        /// </summary>
        public decimal LoanRate { get; set; }

        /// <summary>
        /// 借款金额
        /// </summary>
        public decimal LoanMoney { get; set; }

        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime ApplyDate { get; set; }

        /// <summary>
        /// 借款期限描述
        /// </summary>
        public string LoanPeriodDesc { get; set; }

        /// <summary>
        /// 投资金额
        /// </summary>
        public decimal InvestMoney { get; set; }

        /// <summary>
        /// 招标进度
        /// </summary>
        public decimal LoanSpeed { get; set; }

        /// <summary>
        /// 项目状态描述
        /// </summary>
        public string LoanStateDesc { get; set; }

        /// <summary>
        /// 借款利息
        /// </summary>
        public decimal LoanInterest { get; set; }

        /// <summary>
        /// 借款服务费
        /// </summary>
        public decimal LoanServiceFee { get; set; }

        /// <summary>
        /// 还款总额
        /// </summary>
        public decimal LoanRePayMoney { get; set; }
    }

    /// <summary>
    /// 满标中标
    /// </summary>
    public class RFullLoan
    {
        /// <summary>
        /// 项目编号
        /// </summary>
        public int LoanId { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string LoanName { get; set; }

        /// <summary>
        /// 借款利率
        /// </summary>
        public decimal LoanRate { get; set; }

        /// <summary>
        /// 借款金额
        /// </summary>
        public decimal LoanMoney { get; set; }

        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime ApplyDate { get; set; }

        /// <summary>
        /// 借款期限描述
        /// </summary>
        public string LoanPeriodDesc { get; set; }

        /// <summary>
        /// 投资金额
        /// </summary>
        public decimal InvestMoney { get; set; }

        /// <summary>
        /// 项目状态描述
        /// </summary>
        public string LoanStateDesc { get; set; }

        /// <summary>
        /// 借款利息
        /// </summary>
        public decimal LoanInterest { get; set; }

        /// <summary>
        /// 借款服务费
        /// </summary>
        public decimal LoanServiceFee { get; set; }

        /// <summary>
        /// 还款总额
        /// </summary>
        public decimal LoanRePayMoney { get {
                return LoanMoney + LoanInterest + LoanServiceFee;
            }
        }

        public string FullDate { get; set; }
    }

    /// <summary>
    /// 还款中标
    /// </summary>
    public class RRepaymentLoan
    {
        /// <summary>
        /// 项目编号
        /// </summary>
        public int LoanId { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string LoanName { get; set; }

        /// <summary>
        /// 借款利率
        /// </summary>
        public decimal LoanRate { get; set; }

        /// <summary>
        /// 借款金额
        /// </summary>
        public decimal LoanMoney { get; set; }

        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime ApplyDate { get; set; }

        /// <summary>
        /// 借款期限描述
        /// </summary>
        public string LoanPeriodDesc { get; set; }

        /// <summary>
        /// 应还时间
        /// </summary>
        public DateTime NoRepayDate { get; set; }

        /// <summary>
        /// 未还期数
        /// </summary>
        public int NoRepayPeriod { get; set; }

        /// <summary>
        /// 未还金额
        /// </summary>
        public decimal NoRepayMoney { get; set; }

        /// <summary>
        /// 借款罚息
        /// </summary>
        public decimal LoanOverRate { get; set; }

        /// <summary>
        /// 借款利息
        /// </summary>
        public decimal LoanInterest { get; set; }

        /// <summary>
        /// 借款服务费
        /// </summary>
        public decimal LoanServiceFee { get; set; }

        /// <summary>
        /// 项目状态
        /// </summary>
        public string LoanState { get; set; }

        /// <summary>
        /// 项目状态描述
        /// </summary>
        public string LoanStateDesc { get; set; }

        /// <summary>
        /// 还款总额
        /// </summary>
        public decimal LoanRePayMoney { get {
                return LoanMoney + LoanOverRate + LoanInterest + LoanServiceFee;
            }
        }
    }

    /// <summary>
    /// 已结清标
    /// </summary>
    public class RClearedLoan
    {
        /// <summary>
        /// 项目编号
        /// </summary>
        public int LoanId { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string LoanName { get; set; }

        /// <summary>
        /// 借款利率
        /// </summary>
        public decimal LoanRate { get; set; }

        /// <summary>
        /// 借款金额
        /// </summary>
        public decimal LoanMoney { get; set; }

        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime ApplyDate { get; set; }

        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime ClearDate { get; set; }

        /// <summary>
        /// 借款期限描述
        /// </summary>
        public string LoanPeriodDesc { get; set; }

        /// <summary>
        /// 借款罚息
        /// </summary>
        public decimal LoanOverRate { get; set; }

        /// <summary>
        /// 借款利息
        /// </summary>
        public decimal LoanInterest { get; set; }

        /// <summary>
        /// 借款服务费
        /// </summary>
        public decimal LoanServiceFee { get; set; }

        /// <summary>
        /// 项目状态
        /// </summary>
        public string LoanState { get; set; }

        /// <summary>
        /// 项目状态描述
        /// </summary>
        public string LoanStateDesc { get; set; }

        /// <summary>
        /// 还款总额
        /// </summary>
        public decimal LoanRePayMoney
        {
            get
            {
                return LoanMoney + LoanOverRate + LoanInterest + LoanServiceFee;
            }
        }
    }

    /// <summary>
    /// 借款明细
    /// </summary>
    public class RLoanItemDetail
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string LoanName { get; set; }

        /// <summary>
        /// 借款金额
        /// </summary>
        public decimal LoanMoney { get; set; }

        /// <summary>
        /// 借款期限
        /// </summary>
        public string LoanPeriod { get; set; }

        /// <summary>
        /// 期限类型
        /// </summary>
        public string PeriodType { get; set; }

        /// <summary>
        /// 已还本息
        /// </summary>
        public decimal RepayMoney { get; set; }

        /// <summary>
        /// 待还本息
        /// </summary>
        public decimal WaitRepayMoney { get; set; }

        /// <summary>
        /// 待还期数
        /// </summary>
        public int WaitRepayPeriod { get; set; }
    }
}