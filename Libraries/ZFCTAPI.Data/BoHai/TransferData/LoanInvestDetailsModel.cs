using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.TransferData
{
    /// <summary>
    /// TODO：标的数据迁移，标的投资明细Model
    /// </summary>
    public class LoanInvestDetailsModel
    {
        // <summary>
        /// 序号 
        /// </summary>
        public string ID { get; set; }
        // <summary>
        /// 投资流水号 
        /// </summary>
        public string InvestMerNo { get; set; }
        // <summary>
        /// 账户存管平台客户号
        /// </summary>
        public string PlaCustId { get; set; }
        // <summary>
        /// 交易金额 
        /// </summary>
        public int TransAmt { get; set; }
        // <summary>
        /// 商户保留域 
        /// </summary>
        public string MerPriv { get; set; }
    }
}
