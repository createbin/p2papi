using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ZFCTAPI.Data.CST;

namespace ZFCTAPI.Data.PRO
{
    /// <summary>
	/// 数据表PRO_invester_cash的数据库实体类
	/// </summary>
	[Table("PRO_invester_cash")]
	public partial class PRO_invester_cash : BaseEntity
    {
        /// <summary>
        ///  用户ID 
        /// </summary>	
        public int? rep_user_id { get; set; }

        /// <summary>
        ///  资金总额 
        /// </summary>	
        public decimal? rep_total_cash { get; set; }

        /// <summary>
        ///  可用余额 
        /// </summary>	
        public decimal? rep_sett_money { get; set; }

        /// <summary>
        ///  回收本金 
        /// </summary>	
        public decimal? rep_collect_money { get; set; }

        /// <summary>
        ///  回收利息 
        /// </summary>	
        public decimal? rep_collect_rate { get; set; }

        /// <summary>
        ///  回收罚金 
        /// </summary>	
        public decimal? rep_collect_over_rate { get; set; }

        /// <summary>
        ///  转让债权 
        /// </summary>	
        public decimal? rep_transfer_money { get; set; }

        /// <summary>
        ///  转让手续费 
        /// </summary>	
        public decimal? rep_transfer_fee { get; set; }

        /// <summary>
        ///  买入债权 
        /// </summary>	
        public decimal? rep_buy_money { get; set; }

        /// <summary>
        ///  投资金额 
        /// </summary>	
        public decimal? rep_invest_money { get; set; }

        /// <summary>
        ///  投资冻结金额 
        /// </summary>	
        public decimal? rep_invest_frozen { get; set; }

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
