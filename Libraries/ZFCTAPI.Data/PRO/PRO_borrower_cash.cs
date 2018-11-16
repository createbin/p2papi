using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ZFCTAPI.Data.CST;

namespace ZFCTAPI.Data.PRO
{
    /// <summary>
	/// 数据表PRO_borrower_cash的数据库实体类
	/// </summary>
	[Table("PRO_borrower_cash")]
	public partial class PRO_borrower_cash : BaseEntity
    {
        /// <summary>
        ///  主键ID 
        /// </summary>	
        //public int Id { get; set; }
        /// <summary>
        ///  用户ID 
        /// </summary>	
        public int? rep_user_id { get; set; }

        /// <summary>
        ///  募集总额 
        /// </summary>	
        public decimal? rep_total_money { get; set; }

        /// <summary>
        ///  已还款总额 
        /// </summary>	
        public decimal? rep_total_pay { get; set; }

        /// <summary>
        ///  未还款总额 
        /// </summary>	
        public decimal? rep_total_sett { get; set; }

        /// <summary>
        ///  已还服务费 
        /// </summary>	
        public decimal? rep_pay_fee { get; set; }

        /// <summary>
        ///  未还服务费 
        /// </summary>	
        public decimal? rep_not_pay_fee { get; set; }

        /// <summary>
        ///  已还本金 
        /// </summary>	
        public decimal? rep_pay_money { get; set; }

        /// <summary>
        ///  已还利息 
        /// </summary>	
        public decimal? rep_pay_rate { get; set; }

        /// <summary>
        ///  已还罚金 
        /// </summary>	
        public decimal? rep_pay_over_rate { get; set; }

        /// <summary>
        ///  未还本金 
        /// </summary>	
        public decimal? rep_not_pay_money { get; set; }

        /// <summary>
        ///  未还利息 
        /// </summary>	
        public decimal? rep_not_pay_rate { get; set; }

        /// <summary>
        ///  未还罚金 
        /// </summary>	
        public decimal? rep_not_pay_over_rate { get; set; }

        /// <summary>
        ///  充值总额 
        /// </summary>	
        public decimal? rep_total_recharge { get; set; }

        /// <summary>
        ///  成功提现金额 
        /// </summary>	
        public decimal? rep_out_cash { get; set; }

        /// <summary>
        ///  提现到账金额 
        /// </summary>	
        public decimal? rep_to_account { get; set; }

        /// <summary>
        ///  提现费用 
        /// </summary>	
        public decimal? rep_out_fee { get; set; }


    }
}
