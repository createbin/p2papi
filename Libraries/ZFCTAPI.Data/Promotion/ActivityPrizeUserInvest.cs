using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.Promotion
{
    /// <summary>
    /// 活动统计表
    /// </summary>
    [Table("ActivityPrizeUserInvest")]
    public class ActivityPrizeUserInvest : BaseEntity
    {
        /// <summary>
        /// 活动主题id
        /// </summary>
        public int ActivityId { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal AnnualAmount { get; set; }

        public decimal SurplusAnnualAmount { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        public DateTime LastEditDate { get; set; }

    }
}
