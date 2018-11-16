using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.ReturnModels
{
    public class RBHOpenChargeAccount : RBHBaseModel
    {
        public RBHOpenChargeAccountBody SvcBody { get; set; }
    }

    public class RBHOpenChargeAccountBody
    {
        /// <summary>
        /// 大额充值账号
        /// </summary>
        public string chargeAccount { get; set; }
        /// <summary>
        /// 大额充值账户户名
        /// </summary>
        public string accountName { get; set; }
    }
}
