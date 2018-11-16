using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.ReturnModels
{
    /// <summary>
    /// 融资账户转账
    /// </summary>
    public class RBHFinanceTransfer:RBHBaseModel
    {
        public RBHFinanceTransfer()
        {
            SvcBody=new RBHFinanceTransferBody();
        }

        public RBHFinanceTransferBody SvcBody { get; set; }
    }

    public class RBHFinanceTransferBody
    {
        /// <summary>
        /// C
        /// 账户存管平台流水
        /// </summary>
        public string transId { get; set; }

        /// <summary>
        /// N
        /// 消息扩展
        /// </summary>
        public int extension { get; set; }
    }
}
