using System;
using System.Collections.Generic;
using System.Text;
using ZFCTAPI.Data.Records;

namespace ZFCTAPI.Data.ApiModels.SubmitModels
{
    public class SubmitRepaymentModel
    {
       

        

        /// <summary>
        /// 还款人信息
        /// </summary>
        public LenderRepaymentRecord Lender { get; set; }

        /// <summary>
        /// 投资人信息
        /// </summary>
        public List<InvestorRepaymentRecord> Investors { get; set; }
    }

    public class SubmitRaiseLoanModel
    {
        

        

        /// <summary>
        /// 借款人信息
        /// </summary>
        public LenderLendingRecord LenderLending { get; set; }

        /// <summary>
        /// 投资人信息
        /// </summary>
        public List<InvestorLendingRecord> InvestorLendings { get; set; }
    }
}
