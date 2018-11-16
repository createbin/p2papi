namespace ZFCTAPI.Data.Popular
{
    /// <summary>
    /// 借款统计模型
    /// </summary>
    public class RedStatisticsModel
    {
        /// <summary>
        /// 红包总数量
        /// </summary>
        public int RedCount { get; set; }

        /// <summary>
        /// 红包总额
        /// </summary>
        public decimal RedMoney { get; set; }

        /// <summary>
        /// 待使用数量
        /// </summary>
        public int WaitUseCount { get; set; }

        /// <summary>
        /// 待使用总额
        /// </summary>
        public decimal WaitUseMoney { get; set; }

        /// 已使用数量
        /// </summary>
        public int UsedCount { get; set; }

        /// <summary>
        /// 已使用总额
        /// </summary>
        public decimal UsedMoney { get; set; }

        /// <summary>
        /// 已过期数量
        /// </summary>
        public int ExpiredCount { get; set; }

        /// <summary>
        /// 已过期总额
        /// </summary>
        public decimal ExpiredMoney { get; set; }
    }
}