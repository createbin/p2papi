using System;
using System.Collections.Generic;
using System.Text;
using ZFCTAPI.Data.PRO;

namespace ZFCTAPI.Data.Repayment
{
    public class GenerateCreditorResult
    {
        public virtual List<PRO_loan_plan> ProLoanPlans { get; set; }

        /// <summary>
        /// 信息
        /// </summary>
        public virtual string Msg { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public virtual bool Succeed { get; set; }
    }
}
