using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.RiskAssessment
{
    [Table("RiskByUser")]
    public class RiskByUser : BaseEntity
    {
        /// <summary>
        /// 答案编号
        /// </summary>
        public int AId { get; set; }

        /// <summary>
        /// 积分
        /// </summary>
        public decimal Score { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 用户类型编号
        /// </summary>
        public int UserTypeId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
    }
}
