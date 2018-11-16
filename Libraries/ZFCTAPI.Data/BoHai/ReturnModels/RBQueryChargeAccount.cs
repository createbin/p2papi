using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.ReturnModels
{
    /// <summary>
    /// 线下充值账号查询
    /// </summary>
    public class RBQueryChargeAccount
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
        /// 资金种类(1投资  2融资)
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
