using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.ReturnModels
{
    /// <summary>
    /// 转让成交
    /// </summary>
    ///
    public class RBHClaimsTransferDeal : RBHBaseModel
    {
        public RBHClaimsTransferDealBody SvcBody { get; set; }
    }

    public class RBHClaimsTransferDealBody
    {
        /// <summary>
        /// 账户存管平台流水
        /// </summary>
        public string transId { get; set; }
    }
}