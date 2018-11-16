using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.ReturnModels
{
    /// <summary>
    /// 充值异步回调
    /// </summary>
    public class RBHAsyncWebRecharge:RBHAsyncBase
    {
        /// <summary>
        /// 交易金额
        /// 交易金额中包含商户手续费收入金额（不含营销金额）
        /// </summary>
        public string transAmt { get; set; }

        /// <summary>
        /// 账户存管平台流水号
        /// </summary>
        public string transId { get; set; }

        /// <summary>
        /// 商户手续费收入
        /// 交易金额中包含商户手续费收入金额（不含营销金额）
        /// </summary>
        public string merFeeAmt { get; set; }

        /// <summary>
        /// 手续费模式
        /// 商户收取用户手续费， FeeType 为 1 时不可空， FeeType 为 0 时上送 0
        /// </summary>
        public string FeeType { get; set; }

        /// <summary>
        /// 手续费
        /// 平台收取商户手续费，具体金额以签约协议为准
        /// </summary>
        public string feeAmt { get; set; }

        /// <summary>
        /// 商户保留域
        /// 用户平台编码
        /// </summary>
        public string merPriv { get; set; }
    }
}