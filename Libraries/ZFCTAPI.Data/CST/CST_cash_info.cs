using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ZFCTAPI.Data.SYS;

namespace ZFCTAPI.Data.CST
{
    /// <summary>
    /// 数据表CST_cash_info的数据库实体类
    /// </summary>
    [Table("CST_cash_info")]
    public partial class CST_cash_info : BaseEntity
    {
        #region 关联数据
        /// <summary>
        /// 账户信息表
        /// </summary>
        public virtual CST_account_info CstAccountInfo { get; set; }
        /// <summary>
        /// 交易类型
        /// </summary>
        public virtual SYS_data_dictionary CashType { get; set; }

        /// <summary>
        /// 交易状态
        /// </summary>
        public virtual SYS_data_dictionary CashState { get; set; }
        #endregion

        #region 基本信息
        /// <summary>
        ///  主键ID 
        /// </summary>	
        //public int Id { get; set; }

        /// <summary>
        ///  账户开户表外键id

        /// </summary>	
        public int? rep_account_id { get; set; }

        /// <summary>
        ///  交易流水号 
        /// </summary>	
        public string rep_serial_number { get; set; }

        /// <summary>
        ///  0.充值；1.提现

        /// </summary>	
        public int? rep_cash_type { get; set; }

        /// <summary>
        ///  交易金额 
        /// </summary>	
        public decimal? rep_cash_money { get; set; }

        /// <summary>
        ///  交易状态 
        /// </summary>	
        public int? rep_cash_state { get; set; }

        /// <summary>
        ///  交易时间 
        /// </summary>	
        public DateTime? rep_cash_data { get; set; }

        /// <summary>
        ///  可用余额 
        /// </summary>	
        public decimal? rep_account_money { get; set; }

        /// <summary>
        ///  异步更新时间 
        /// </summary>	
        public DateTime? rep_cash_enddata { get; set; }

        /// <summary>
        ///  跟踪错误信息 
        /// </summary>	
        public string rep_cash_error { get; set; }

        /// <summary>
        ///  审核意见 
        /// </summary>	
        public string rep_cash_content { get; set; }
        #endregion
    }
}
