using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.InvestInfo
{
    /// <summary>
    /// 待收明细
    /// </summary>
    public class InvestPlanDetail
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string LoanName { get; set; }

        /// <summary>
        /// 应还日期
        /// </summary>
        public DateTime? PayDate { get; set; }

        /// <summary>
        /// 还款日期
        /// </summary>
        public DateTime? CollectDate { get; set; }

        /// <summary>
        /// 应还总额
        /// </summary>
        public decimal PayTotal { get; set; }

        /// <summary>
        /// 还款总额
        /// </summary>
        public decimal? CollectTotal { get; set; }

        /// <summary>
        /// 还款方式
        /// </summary>
        public string RepayType { get; set; }
    }
}