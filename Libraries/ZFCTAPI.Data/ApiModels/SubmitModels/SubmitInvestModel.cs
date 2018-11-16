using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.ApiModels.SubmitModels
{
    public class SLoanRedPackage : BaseSubmitModel
    {
        public int LoanId { get; set; }

        public decimal InvestMoney { get; set; }
    }

    public class SInvestIncome : BaseSubmitModel
    {
        /// <summary>
        /// 标的id
        /// </summary>
        public int LoanId { get; set; }

        /// <summary>
        /// 还款方式
        /// </summary>
        public string RepaymentType { get; set; }

        /// <summary>
        /// 投资金额
        /// </summary>
        public decimal InvestMoney { get; set; }

        public string DeadLine { get; set; }

        public string BillDay { get; set; }

        public string InType { get; set; }

        public string LoanRate { get; set; }
    }

    /// <summary>
    /// 代收明细
    /// </summary>
    public class SInvestPlanDetail : BasePageModel
    {
        /// <summary>
        /// 1 待收 2 已收
        /// </summary>
        public string Type { get; set; }
    }

    /// <summary>
    /// 投资合同
    /// </summary>
    public class SInvestContract : BaseSubmitModel
    {
        /// <summary>
        /// 1 待收 2 已收
        /// </summary>
        public string InvestId { get; set; }
    }

    /// <summary>
    /// 投资标
    /// </summary>
    public class SInvestLoan : BaseSubmitModel
    {
        /// <summary>
        /// 投资金额
        /// </summary>
        public decimal Money { get; set; }
        /// <summary>
        /// 标编号
        /// </summary>
        public int LoanId { get; set; }
        /// <summary>
        /// 红包id
        /// </summary>
        public int RedId { get; set; }
    }

    /// <summary>
    /// 投资的还款计划
    /// </summary>
    public class SInvestRepayPlan : BaseSubmitModel
    {
        /// <summary>
        /// 投资编号
        /// </summary>
        public int InvestId { get; set; }
    }
}