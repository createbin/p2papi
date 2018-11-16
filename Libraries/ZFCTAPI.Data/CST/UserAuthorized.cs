using System;
using System.Collections.Generic;
using System.Text;
using Dapper.Contrib.Extensions;

namespace ZFCTAPI.Data.CST
{
    [Table("UserAuthorized")]
    public class UserAuthorized:BaseEntity
    {
        /// <summary>
        /// 账户id
        /// </summary>
        public int AccountId { get; set; }
        /// <summary>
        /// 渤海帐号id
        /// </summary>
        public string PlanCustId { get; set; }

        /// <summary>
        /// 授权时请求流水号
        /// </summary>
        public string MerBillNo { get; set; }

        /// <summary>
        /// 投资权限
        /// </summary>
        public bool InvestAuth { get; set; } = false;
        /// <summary>
        /// 投资权限开始时间
        /// </summary>
        public DateTime? InvestAuthStartTime { get; set; } = null;
        /// <summary>
        /// 投资权限结束时间
        /// </summary>
        public DateTime? InvestAuthEndTime { get; set; } = null;
        /// <summary>
        /// 收费权限
        /// </summary>
        public bool PaymentAuth { get; set; } = false;
        /// <summary>
        /// 收费权限开始时间
        /// </summary>
        public DateTime? PaymentAuthStartTime { get; set; } = null;
        /// <summary>
        ///收费权限结束时间
        /// </summary>
        public DateTime? PaymentAuthEndTime { get; set; } = null;

        /// <summary>
        ///还款权限
        /// </summary>
        public bool RepaymentAuth { get; set; } = false;
        /// <summary>
        ///还款权限开始时间
        /// </summary>
        public DateTime? RepaymentAuthStratTime { get; set; } = null;

        /// <summary>
        /// 还款权限结束时间
        /// </summary>
        public DateTime? RepaymentAuthEndTime { get; set; } = null;
        /// <summary>
        ///创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        ///修改时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
