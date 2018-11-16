using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.TransferInfo
{
    /// <summary>
    /// 借款统计模型
    /// </summary>
    public class TransferStatisticsModel
    {
        /// <summary>
        /// 转出中
        /// </summary>
        public int TransferingCount { get; set; }

        /// <summary>
        /// 已转出
        /// </summary>
        public int TransferedCount { get; set; }
    }
}