using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.ReturnModels
{
    public class RBHQueryChargeAccount:RBHBaseModel
    {
        public RBHQueryChargeAccountBody SvcBody { get; set; }
    }

    public class RBHQueryChargeAccountBody
    {
        /// <summary>
        /// 大额充值账号 
        /// </summary>
        public string chargeAccount { get; set; }
        /// <summary>
        /// 大额充值账户户名
        /// </summary>
        public string accountName { get; set; }
        /// <summary>
        /// 资金种类
        /// </summary>
        public string capTyp { get; set; }
        /// <summary>
        /// 可用余额 
        /// </summary>
        public string avlBal { get; set; }
        /// <summary>
        /// 账户余额 
        /// </summary>
        public string acctBal { get; set; }
        /// <summary>
        /// 冻结余额 
        /// </summary>
        public string frzBal { get; set; }
    }
}
