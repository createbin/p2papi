using System;
using System.Collections.Generic;
using System.Text;
using Dapper.Contrib.Extensions;

namespace ZFCTAPI.Data.PRO
{
    /// <summary>
    /// 标相关的用户信息
    /// </summary>
    [Table("PRO_loan_userInfo")]
    public partial class PRO_loan_userInfo : BaseEntity
    {
        /// <summary>
        /// 用户信息表
        /// </summary>
        public virtual PRO_loan_info loanInfo { get; set; }


        /// <summary>
        /// 其他平台负债情况
        /// </summary>
        public string cst_user_thriddebt { get; set; }

        /// <summary>
        /// 认证项目
        /// </summary>
        public string cst_auth_item { get; set; }

        /// <summary>
        /// 历史逾期笔数
        /// </summary>
        public int? cst_loan_overduecount { get; set; }

        /// <summary>
        /// 历史贷款笔数
        /// </summary>
        public int? cst_loan_count { get; set; }

        /// <summary>
        /// 未结清贷款笔数
        /// </summary>
        public int? cst_loan_repaycount { get; set; }

        /// <summary>
        /// 未结清贷款金额
        /// </summary>
        public decimal? cst_loan_repaymoney { get; set; }
    }
}
