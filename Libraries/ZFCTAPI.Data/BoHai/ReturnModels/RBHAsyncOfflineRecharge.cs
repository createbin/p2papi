using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Text;

namespace ZFCTAPI.Data.BoHai.ReturnModels
{
    public class RBHAsyncOfflineRecharge : RBHAsyncBase
    {
        /// <summary>
        /// 交易金额
        /// </summary>
        public string transAmt { get; set; }

        /// <summary>
        /// 账户存管平台流水号
        /// </summary>
        public string transId { get; set; }
        /// <summary>
        /// 平台收取商户手续费，具体金额以签约协议为准
        /// </summary>
        public string feeAmt { get; set; }
        /// <summary>
        /// 1 投资户  2 融资户
        /// </summary>
        public string transTyp { get; set; }
    }
}
