using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.SubmitModels
{
    /// <summary>
    /// 融资账户转账
    /// </summary>
    public class SBHFinanceTransfer : SBHBaseModel
    {
        public SBHFinanceTransfer()
        {
            SvcBody = new SBHFinanceTransferBody();
        }

        public SBHFinanceTransferBody SvcBody { get; set; }
    }

    public class SBHFinanceTransferBody
    {
        /// <summary>
        /// Y
        /// 用户平台编码
        /// 投资账户
        /// </summary>
        public string platformUidInvestment { get; set; }

        /// <summary>
        /// Y
        /// 用户平台编码
        /// 融资账户
        /// </summary>
        public string platformUidFinance { get; set; }

        /// <summary>
        /// Y
        /// 交易金额
        /// </summary>
        public string amount { get; set; }

        /// <summary>
        /// N
        /// 备注
        /// </summary>
        public string comment { get; set; }
    }
}
