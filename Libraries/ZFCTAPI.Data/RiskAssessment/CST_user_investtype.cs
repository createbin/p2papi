using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.RiskAssessment
{
    [Table("CST_user_investtype")]
    public class CST_user_investtype : BaseEntity
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public int cst_user_id { get; set; }

        /// <summary>
        /// 用户投资类型
        /// 1 保守型
        /// 2 稳健型
        /// 3 激进型
        /// </summary>
        public int cst_invest_type { get; set; }

        /// <summary>
        /// 问卷积分
        /// </summary>
        public int cst_invest_score { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime cst_create_time { get; set; }

        /// <summary>
        /// 更改时间
        /// </summary>
        public DateTime cst_update_time { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        public bool cst_delsign { get; set; }
    }
}
