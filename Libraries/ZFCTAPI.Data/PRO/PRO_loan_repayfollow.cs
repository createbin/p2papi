using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ZFCTAPI.Data.PRO
{
    [Table("PRO_loan_repayfollowService")]
    public class PRO_loan_repayfollow : BaseEntity
    {
        #region 基本信息
        /// <summary>
        /// 项目编号
        /// </summary>
        public int? pro_loan_id { get; set; }

        /// <summary>
        /// 还款跟踪类型
        /// </summary>
        public int? pro_loan_followtype { get; set; }

        /// <summary>
        /// 组别
        /// </summary>
        public string pro_loan_followgroup { get; set; }

        /// <summary>
        /// 跟踪时间
        /// </summary>
        public DateTime pro_loan_followdate { get; set; }

        /// <summary>
        /// 跟踪信息
        /// </summary>
        public string pro_loan_followInfo { get; set; }

        /// <summary>
        ///  操作人  外键表：CST_user_info 
        /// </summary>	
        public int? pro_operate_emp { get; set; }

        /// <summary>
        ///  操作日期 
        /// </summary>	
        public DateTime? pro_operate_date { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool pro_delsign { get; set; }
        #endregion
    }
}
