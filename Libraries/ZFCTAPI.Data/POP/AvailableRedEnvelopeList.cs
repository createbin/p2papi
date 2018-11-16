using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.POP
{
    /// <summary>
    /// 可用红包项
    /// </summary>
    public class AvailableRedEnvelope
    {
        /// <summary>
        /// 用户红包记录表主键
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 可使用金额
        /// </summary>
        public decimal EnableAmount { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        ///有效期 
        /// </summary>
        public DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// 红包状态
        /// </summary>
        public string Status { get; set; }
    }

    public class InterfaceRedEnvelope : AvailableRedEnvelope
    {
        public bool IsCanUse { get; set; }
        public string Introduction { get; set; }
    }


}
