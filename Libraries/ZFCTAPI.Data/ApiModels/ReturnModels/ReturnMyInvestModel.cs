using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.ApiModels.ReturnModels
{
    /// <summary>
    /// 我的投资-投标中
    /// </summary>
    public class RBiddingInvest
    {
        /// <summary>
        /// 投资编号
        /// </summary>
        public int InvestId { get; set; }

        /// <summary>
        /// 项目编号
        /// </summary>
        public int LoanId { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string LoanName { get; set; }

        /// <summary>
        /// 项目利率
        /// </summary>
        public decimal LoanRate { get; set; }

        /// <summary>
        /// 借款金额
        /// </summary>
        public decimal LoanMoney { get; set; }

        /// <summary>
        /// 项目期限
        /// </summary>    
        public int LoanPeriod { get; set; }

        /// <summary>
        /// 项目期限类型
        /// </summary>    
        public int LoanPeriodType { get; set; }

        /// <summary>
        /// 项目期限描述
        /// </summary>    
        public string LoanPeriodDesc { get; set; }

        /// <summary>
        /// 投资金额
        /// </summary>
        public decimal InvestMoney { get; set; }

        /// <summary>
        /// 项目还款方式
        /// </summary>
        public string LoanRepayType { get; set; }

        /// <summary>
        /// 投资时间
        /// </summary>
        public DateTime InvestDate { get; set; }

        /// <summary>
        /// 预期收益
        /// </summary>
        public decimal ExpectedRevenue { get; set; }

        /// <summary>
        /// 交易状态
        /// </summary>
        public int TraState { get; set; }

        /// <summary>
        /// 项目状态
        /// </summary>
        public int LoanState { get; set; }

        /// <summary>
        /// 项目状态描述
        /// </summary>
        public string LoanStateDesc { get; set; }

        /// <summary>
        /// 红包金额
        /// </summary>
        public decimal UseRedMoney { get; set; }

        /// <summary>
        /// 是否渤海项目
        /// </summary>
        public bool Bohai { get; set; }
    }

    /// <summary>
    /// 我的投资-还款中
    /// </summary>
    public class RRepaymentInvest
    {
        /// <summary>
        /// 投资编号
        /// </summary>
        public int InvestId { get; set; }

        /// <summary>
        /// 项目编号
        /// </summary>
        public int LoanId { get; set; }

        /// <summary>
        /// 项目名车
        /// </summary>
        public string LoanName { get; set; }

        /// <summary>
        /// 项目利率
        /// </summary>
        public decimal LoanRate { get; set; }

        /// <summary>
        /// 投资金额
        /// </summary>
        public decimal InvestMoney { get; set; }

        /// <summary>
        /// 借款金额
        /// </summary>
        public decimal LoanMoney { get; set; }

        /// <summary>
        /// 还款日
        /// </summary>
        public DateTime PayDate { get; set; }

        /// <summary>
        /// 还款金额
        /// </summary>
        public decimal PayMoeny { get; set; }

        /// <summary>
        /// 项目状态
        /// </summary>
        public int LoanState { get; set; }

        /// <summary>
        /// 项目状态描述
        /// </summary>
        public string LoanStateDesc { get; set; }
    }

    /// <summary>
    /// 我的投资-已结清
    /// </summary>
    public class RClearedInvest
    {
        /// <summary>
        /// 投资编号
        /// </summary>
        public int InvestId { get; set; }

        /// <summary>
        /// 项目编号
        /// </summary>
        public int LoanId { get; set; }

        /// <summary>
        /// 项目名车
        /// </summary>
        public string LoanName { get; set; }

        /// <summary>
        /// 项目利率
        /// </summary>
        public decimal LoanRate { get; set; }

        /// <summary>
        /// 投资金额
        /// </summary>
        public decimal InvestMoney { get; set; }

        /// <summary>
        /// 借款金额
        /// </summary>
        public decimal LoanMoney { get; set; }
        
        /// <summary>
        /// 投资时间
        /// </summary>
        public DateTime InvestDate { get; set; }
        /// <summary>
        /// 回款时间
        /// </summary>
        public DateTime ClearedDate { get; set; }

        /// <summary>
        /// 到期时间
        /// </summary>
        public DateTime ExpireDate { get; set; }

        /// <summary>
        /// 起息时间
        /// </summary>
        public DateTime InterestDate { get; set; }

        /// <summary>
        /// 预期收益
        /// </summary>
        public decimal ExpectedRevenue { get; set; }

        /// <summary>
        /// 项目状态
        /// </summary>
        public int LoanState { get; set; }

        /// <summary>
        /// 项目状态描述
        /// </summary>
        public string LoanStateDesc { get; set; }

        /// <summary>
        /// 项目期限
        /// </summary>    
        public int LoanPeriod { get; set; }

        /// <summary>
        /// 项目期限类型
        /// </summary>    
        public int LoanPeriodType { get; set; }

        /// <summary>
        /// 项目期限描述
        /// </summary>  
        public string LoanPeriodDesc { get; set; }

        /// <summary>
        /// 项目还款方式
        /// </summary>
        public string LoanRepayType { get; set; }


        /// <summary>
        /// 红包金额
        /// </summary>
        public decimal UseRedMoney { get; set; }
    }

    /// <summary>
    /// 我的投资-还款中-APP
    /// </summary>
    public class RAPPRepaymentInvest
    {
        /// <summary>
        /// 投资编号
        /// </summary>
        public int InvestId { get; set; }

        /// <summary>
        /// 项目编号
        /// </summary>
        public int LoanId { get; set; }

        /// <summary>
        /// 项目名车
        /// </summary>
        public string LoanName { get; set; }

        /// <summary>
        /// 项目利率
        /// </summary>
        public decimal LoanRate { get; set; }

        /// <summary>
        /// 投资金额
        /// </summary>
        public decimal InvestMoney { get; set; }

        /// <summary>
        /// 借款金额
        /// </summary>
        public decimal LoanMoney { get; set; }

        /// <summary>
        /// 投资时间
        /// </summary>
        public DateTime InvestDate { get; set; }

        /// <summary>
        /// 到期时间
        /// </summary>
        public DateTime ExpireDate { get; set; }

        /// <summary>
        /// 起息时间
        /// </summary>
        public DateTime InterestDate { get; set; }

        /// <summary>
        /// 预期收益
        /// </summary>
        public decimal ExpectedRevenue { get; set; }

        /// <summary>
        /// 项目状态
        /// </summary>
        public int LoanState { get; set; }

        /// <summary>
        /// 项目状态描述
        /// </summary>
        public string LoanStateDesc { get; set; }

        /// <summary>
        /// 项目期限
        /// </summary>    
        public int LoanPeriod { get; set; }

        /// <summary>
        /// 项目期限类型
        /// </summary>    
        public int LoanPeriodType { get; set; }

        /// <summary>
        /// 项目期限描述
        /// </summary>  
        public string LoanPeriodDesc { get; set; }

        /// <summary>
        /// 项目还款方式
        /// </summary>
        public string LoanRepayType { get; set; }

        /// <summary>
        /// 红包金额
        /// </summary>
        public decimal UseRedMoney { get; set; }
    }

    /// <summary>
    /// 投资还款计划统计
    /// </summary>
    public class RInvestPlanStatistics
    {
        /// <summary>
        /// 还款日期
        /// </summary>
        public DateTime PayDate { get; set; }

        /// <summary>
        /// 待收总额/已收总额
        /// </summary>
        public decimal TotalCollection { get; set; }

        public IEnumerable<RInvestPlanDetail> InvestPlans { get; set; }
    }

    /// <summary>
    /// 投资还款计划信息
    /// </summary>
    public class RInvestPlanDetail
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string LoanName { get; set; }

        /// <summary>
        /// 待收总额/已收总额
        /// </summary>
        public decimal TotalCollection { get; set; }

        /// <summary>
        /// 项目还款方式
        /// </summary>
        public string LoanRepayType { get; set; }
    }

    /// <summary>
    /// 投资还款计划
    /// </summary>
    public class RInvestRepayPlan
    {
        /// <summary>
        /// 待收总额
        /// </summary>
        public decimal WaitRepayMoeny { get; set; }

        /// <summary>
        /// 已收总额
        /// </summary>
        public decimal RepayMoeny { get; set; }

        /// <summary>
        /// 投资还款计划
        /// </summary>
        public IEnumerable<RInvestRepayPlanDetail> InvestPlans { get; set; }
    }

    /// <summary>
    /// 投资还款计划明细
    /// </summary>
    public class RInvestRepayPlanDetail
    {
        /// <summary>
        /// 还款时间
        /// </summary>
        public DateTime RepayDate { get; set; }

        /// <summary>
        /// 本金
        /// </summary>
        public decimal RepayMoeny { get; set; }

        /// <summary>
        /// 收益
        /// </summary>
        public decimal RepayRate { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string RepayStateDesc { get; set; }
    }
}