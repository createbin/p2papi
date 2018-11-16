using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.ApiModels.SubmitModels
{
    public class STransferDetail : BaseSubmitModel
    {
        public int TransferId { get; set; }
    }

    public class SInvestTransfer : BaseSubmitModel
    {
        public int TransferId { get; set; }

        public int LoanId { get; set; }

        public decimal InvestMoney { get; set; }
    }

    /// <summary>
    /// 债权转让申请
    /// </summary>
    public class SDoTransferData : BaseSubmitModel
    {
        /// <summary>
        /// 投资ID
        /// </summary>
        public int InvestId { get; set; }

        /// <summary>
        /// 转让折扣
        /// </summary>
        public decimal Discount { get; set; }

        /// <summary>
        /// 转让金额
        /// </summary>
        public decimal IdualMoney { get; set; }
    }

    /// <summary>
    /// 撤回债权转让
    /// </summary>
    public class SRecallTransfer : BaseSubmitModel
    {
        /// <summary>
        /// 转让申请id
        /// </summary>
        public int Id { get; set; }
    }
}