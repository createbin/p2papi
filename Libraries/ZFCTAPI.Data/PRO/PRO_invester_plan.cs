using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.PRO
{
    /// <summary>
	/// 数据表PRO_invester_plan的数据库实体类
	/// </summary>
    [Table("PRO_invester_plan")]
	public partial class PRO_invester_plan : BaseEntity
    {
        /// <summary>
        ///  借款人还款计划表ID 
        /// </summary>	
        public int? pro_plan_id { get; set; }

        /// <summary>
        ///  投资记录ID 
        /// </summary>	
        public int? pro_invest_id { get; set; }

        /// <summary>
        ///  应还本金 
        /// </summary>	
        public decimal? pro_pay_money { get; set; }

        /// <summary>
        ///  应还利息 
        /// </summary>	
        public decimal? pro_pay_rate { get; set; }

        /// <summary>
        ///  应还罚金 
        /// </summary>	
        public decimal? pro_pay_over_rate { get; set; }

        /// <summary>
        ///  应还总额 
        /// </summary>	
        public decimal? pro_pay_total { get; set; }

        /// <summary>
        ///  实还本金 
        /// </summary>	
        public decimal? pro_collect_money { get; set; }

        /// <summary>
        ///  实还利息 
        /// </summary>	
        public decimal? pro_collect_rate { get; set; }

        /// <summary>
        ///  实还罚金 
        /// </summary>	
        public decimal? pro_collect_over_rate { get; set; }

        /// <summary>
        ///  实还总额 
        /// </summary>	
        public decimal? pro_collect_total { get; set; }

        /// <summary>
        ///  应还日期 
        /// </summary>	
        public DateTime? pro_pay_date { get; set; }

        /// <summary>
        ///  实还日期 
        /// </summary>	
        public DateTime? pro_collect_date { get; set; }

        /// <summary>
        ///  因罚金是时时计算的，当第一次罚金未还清时，保留剩余未还罚金 
        /// </summary>	
        public decimal? pro_sett_over_rate { get; set; }

        /// <summary>
        ///  期数 
        /// </summary>	
        public int? pro_loan_period { get; set; }

        /// <summary>
        ///  1：是 0：否 
        /// </summary>	
        public bool pro_is_clear { get; set; }

        public decimal? pro_experience_money { get; set; }
    }
}
