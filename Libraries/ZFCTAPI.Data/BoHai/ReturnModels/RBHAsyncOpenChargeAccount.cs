using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.ReturnModels
{
    public class RBHAsyncOpenChargeAccount:RBHAsyncBase
    {
        /// <summary>
        /// 线下充值账号
        /// </summary>
        public string chargeAccount { get; set; }

        /// <summary>
        ///线下充值账户户名
        /// </summary>
        public string accountName { get; set; }
        /// <summary>
        /// 清算行号
        /// </summary>
        public string accountBk { get; set; }

        /// <summary>
        /// 实名打款金额
        /// </summary>
        public string chargeAmt { get; set; }
    }
}
