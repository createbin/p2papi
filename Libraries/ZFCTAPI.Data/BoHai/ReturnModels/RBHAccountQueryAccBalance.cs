using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.ReturnModels
{
    /// <summary>
    /// 可用余额
    /// </summary>
    public class RBHAccountQueryAccBalance : RBHBaseModel
    {
        public RBHAccountQueryAccBalanceBody SvcBody { get; set; }
    }

    public class RBHAccountQueryAccBalanceBody
    {
        /// <summary>
        /// 资金种类
        /// N
        /// 1投资  2融资
        /// </summary>
        public string capTyp { get; set; }
        /// <summary>
        /// 可用余额
        /// </summary>
        public string totalAmount { get; set; }

        /// <summary>
        /// 可提现金额
        /// </summary>
        public string withdrawAmount { get; set; }

        /// <summary>
        /// 冻结金额
        /// </summary>
        public string freezeAmout { get; set; }
    }
}