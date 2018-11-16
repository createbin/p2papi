using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.ReturnModels
{
    /// <summary>
    /// 申购
    /// </summary>
    public class RBHClaimsPurchase : RBHBaseModel
    {
        public RBHClaimsPurchaseBody SvcBody { get; set; }
    }

    public class RBHClaimsPurchaseBody
    {
        /// <summary>
        /// 账户存管平台流水
        /// </summary>
        public string transId { get; set; }

        /// <summary>
        /// 账户存管平台流水
        /// 平台收取商户手续费，具体金额以签约协议为准。
        /// </summary>
        public string feeAmtFeeAmt { get; set; }

        /// <summary>
        /// 冻结编号
        /// </summary>
        public string freezeId { get; set; }
    }
}