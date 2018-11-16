using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.ApiModels.ReturnModels
{
    /// <summary>
    /// 还款计划
    /// </summary>
    public class RMyLoanPlanModel
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 应还日期
        /// </summary>
        public DateTime? PayDate { get; set; }

        /// <summary>
        /// 应还本金
        /// </summary>
        public decimal PayMoney { get; set; }

        /// <summary>
        /// 应还利息
        /// </summary>
        public decimal PayRate { get; set; }

        /// <summary>
        /// 应还本息
        /// </summary>
        public decimal PayPrincipal { get; set; }

        /// <summary>
        /// 期数
        /// </summary>
        public int? Period { get; set; }

        /// <summary>
        /// 利率
        /// </summary>
        public decimal? Interest { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string LoanName { get; set; }

        /// <summary>
        /// 还款时间
        /// </summary>
        public DateTime? CollectDate { get; set; }

        /// <summary>
        /// 应还服务费
        /// </summary>
        public decimal? PayServiceFee { get; set; }

        /// <summary>
        /// 是否还清
        /// </summary>
        public string IsClear { get; set; }

        /// <summary>
        /// 是否正在使用
        /// </summary>
        public bool IsUsing { get; set; }

        /// <summary>
        /// 还款类型
        /// </summary>
        public string CollectType { get; set; }
        /// <summary>
        /// 逾期金额
        /// </summary>
        public decimal OverRateMoney { get; set; } = 0.00m;

    }

    /// <summary>
    /// 还款计划明细
    /// </summary>
    public class RRepayDetail
    {
        /// <summary>
        /// 记录id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 项目名称(用途)
        /// </summary>
        public string LoanName { get; set; }

        /// <summary>
        /// 借款金额
        /// </summary>
        public decimal LoanMoney { get; set; }

        /// <summary>
        /// 借款利率
        /// </summary>
        public decimal LoanRate { get; set; }

        /// <summary>
        /// 借款期限
        /// </summary>
        public string LoanPeriod { get; set; }

        /// <summary>
        /// 期数
        /// </summary>
        public int Period { get; set; }
        
        /// <summary>
        /// 状态
        /// </summary>
        public string LoanState { get; set; }

        /// <summary>
        /// 下次还款日期
        /// </summary>
        public DateTime NextRepayDate { get; set; }

        /// <summary>
        /// 还款日期
        /// </summary>
        public DateTime RepayDate { get; set; }

        /// <summary>
        /// 剩余期数
        /// </summary>
        public decimal SurplusPeriod { get; set; }

        /// <summary>
        /// 已还期数
        /// </summary>
        public decimal SettledPeriod { get; set; }

        /// <summary>
        /// 已还本金
        /// </summary>
        public decimal SettledPrincipal { get; set; }

        /// <summary>
        /// 未还本金
        /// </summary>
        public decimal Principal { get; set; }

        /// <summary>
        /// 已还利息
        /// </summary>
        public decimal SettledInterest { get; set; }

        /// <summary>
        /// 未还利息
        /// </summary>
        public decimal Interest { get; set; }

        /// <summary>
        /// 应还逾期费
        /// </summary>
        public decimal LateFee { get; set; }

        /// <summary>
        /// 已还逾期费
        /// </summary>
        public decimal SettledLateFee { get; set; }

        /// <summary>
        /// 应还服务费
        /// </summary>
        public decimal ServiceFee { get; set; }

        /// <summary>
        /// 已还服务费
        /// </summary>
        public decimal SettledServiceFee { get; set; }

        /// <summary>
        /// 下次应还费用
        /// </summary>
        public decimal NextWaitPayMoney { get; set; }

        /// <summary>
        /// 本期应还总额
        /// </summary>
        public decimal CurrenyWaitPayMoney { get; set; }

        /// <summary>
        /// 应还总额
        /// </summary>
        public decimal WaitPayMoney { get; set; }

        /// <summary>
        /// 是否还清
        /// </summary>
        public string IsClear { get; set; }
    }

    public class RMyRepayLoan
    {
        public int LoanId { get; set; }

        public string LoanName { get; set; }

        public string WaitRepayMoney { get; set; }

        public string WaitRepayPeriod { get; set; }

        public string LoanMoney { get; set; }

        public string LoanPeriod { get; set; }

        public string LoanRate { get; set; }

        public string FullDate { get; set; }

        public string RepayType { get; set; }

        public string RepayTypeName { get; set; }

        public List<RMyLoanPlanModel> RepayLoanPlans { get; set; } = new List<RMyLoanPlanModel>();
    }

    public class RMyRepayLoans
    {
        public int Count { get; set; }

        public List<RMyRepayLoan> RepayLoans { get; set; }=new List<RMyRepayLoan>();

    }

    public class RGurClearedPlan
    {
        public string LoanId { get; set; }

        public string LoanNo { get; set; }

        public string LoanName { get; set; }

        public string PlanPeriod { get; set; }
        /// <summary>
        /// 还款日期
        /// </summary>
        public string PayDate { get; set; }
        /// <summary>
        /// 已还本金
        /// </summary>
        public string PayMoney { get; set; }
        /// <summary>
        /// 已还利息
        /// </summary>
        public string PayRate { get; set; }
        /// <summary>
        /// 已还总额
        /// </summary>
        public string PayPrincipal { get; set; }
        /// <summary>
        /// 已还服务费
        /// </summary>
        public string PayServiceFee { get; set; }
    }

}