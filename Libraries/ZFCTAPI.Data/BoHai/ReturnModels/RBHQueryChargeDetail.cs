using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.ReturnModels
{
    public class RBHQueryChargeDetail:RBHBaseModel
    {
        public RBHQueryChargeDetailBody SvcBody { get; set; }
    }

    public class RBHQueryChargeDetailBody
    {
        public RBHQueryChargeDetailBody()
        {
            items=new List<QueryChargeDetailItems>();
        }

        public string totalPage { get; set; }
        public string totalNum { get; set; }
        public List<QueryChargeDetailItems> items { get; set; }
    }

    public class QueryChargeDetailItems
    {
        /// <summary>
        /// 记账日期
        /// </summary>
        public string acdate { get; set; }
        /// <summary>
        /// 账户存管平台流水号
        /// </summary>
        public string transId { get; set; }
        /// <summary>
        /// 交易金额
        /// </summary>
        public string transAmt { get; set; }
        /// <summary>
        /// 手续费
        /// </summary>
        public string feeAmt { get; set; }
        /// <summary>
        /// 交易状态
        /// </summary>
        public string transStat { get; set; }
        /// <summary>
        /// 失败原因
        /// </summary>
        public string falRsn { get; set; }
    }
}
