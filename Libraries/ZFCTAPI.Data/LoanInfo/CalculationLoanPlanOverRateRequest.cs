using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.LoanInfo
{
    public class CalculationLoanPlanOverRateRequest
    {
        /// <summary>
        /// 剩余罚息
        /// pro_sett_over_rate
        /// </summary>
        public decimal SurplusOverRate { get; set; }

        /// <summary>
        /// 应该还款日期
        /// pro_pay_date
        /// </summary>
        public DateTime? PayDate { get; set; }

        /// <summary>
        /// 实际还款日期
        /// pro_collect_date
        /// </summary>
        public DateTime? CollectDate { get; set; }

        /// <summary>
        /// 应还本金
        /// pro_pay_money
        /// </summary>
        public decimal PayMoney { get; set; }

        /// <summary>
        /// 应还利息
        /// pro_pay_rate
        /// </summary>
        public decimal PayRate { get; set; }
    }
}