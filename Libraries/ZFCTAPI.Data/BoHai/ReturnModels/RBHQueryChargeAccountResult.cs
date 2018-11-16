using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.ReturnModels
{
    public class RBHQueryChargeAccountResult
    {
        public RBHQueryChargeAccountResult()
        {
            SvcBody = new RBHQueryChargeAccountResultBody();
        }

        public RBHQueryChargeAccountResultBody SvcBody { get; set; }
    }

    public class RBHQueryChargeAccountResultBody
    {
        /// <summary>
        /// 大额充值账号
        /// </summary>
        public string chargeAccount { get; set; }
        /// <summary>
        /// 清算行号
        /// </summary>
        public string accountBk { get; set; }
        /// <summary>
        /// 大额充值账户户名
        /// </summary>
        public string accountName { get; set; }
        /// <summary>
        /// 实名状态
        /// </summary>
        public string realNameFlg { get; set; }
        /// <summary>
        /// 实名打款金额
        /// </summary>
        public string chargeAmt { get; set; }
        /// <summary>
        /// 账户存管平台客户号
        /// </summary>
        public string plaCustId { get; set; }
    }

}
