using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ZFCTAPI.Data.PRO;
using ZFCTAPI.Data.SYS;

namespace ZFCTAPI.Data.CST
{
    /// <summary>
    /// 数据表CST_transaction_info的数据库实体类
    /// </summary>
    [Table("CST_transaction_info")]
    public partial class CST_transaction_info : BaseEntity
    {
        #region 基本信息
        /// <summary>
        ///  用户主键 
        /// </summary>	
        public int? pro_user_id { get; set; }

        /// <summary>
        ///  用户类型 
        /// </summary>	
        public int? pro_user_type { get; set; }

        /// <summary>
        ///  项目ID 
        /// </summary>	
        public int? pro_loan_id { get; set; }

        /// <summary>
        ///  交易金额 
        /// </summary>	
        public decimal? pro_transaction_money { get; set; }

        /// <summary>
        ///  交易时间 
        /// </summary>	
        public DateTime? pro_transaction_time { get; set; }

        /// <summary>
        ///  交易流水号 
        /// </summary>	
        public string pro_transaction_no { get; set; }

        /// <summary>
        ///  交易后投资账户余额 
        /// </summary>	
        public decimal? pro_balance_money { get; set; }

        /// <summary>
        ///  交易后融资账户余额 
        /// </summary>	
        public decimal? pro_finance_money { get; set; }
        

        /// <summary>
        ///  0：冻结 1：解冻 2：划转 3：充值 4：提现 
        /// </summary>	
        public int? pro_transaction_type { get; set; }

        /// <summary>
        ///  交易备注 
        /// </summary>	
        public string pro_transaction_remarks { get; set; }

        /// <summary>
        ///  平台服务费 
        /// </summary>	
        public decimal? pro_platforms_fee { get; set; }

        /// <summary>
        ///  担保服务费 
        /// </summary>	
        public decimal? pro_guarantee_fee { get; set; }

        /// <summary>
        ///  交易状态 
        /// </summary>	
        public int? pro_transaction_status { get; set; }

        /// <summary>
        ///  交易后账户金额
        /// </summary>	
        public decimal? pro_account_money { get; set; }

        /// <summary>
        ///  交易后账户冻结金额
        /// </summary>	
        public decimal? pro_frozen_money { get; set; }

        /// <summary>
        ///  待收金额
        /// </summary>	
        public decimal? pro_receivable_money { get; set; }

        /// <summary>
        ///  交易描述 
        /// </summary>	
        public string pro_description { get; set; }


        /// <summary>
        ///  充值类型 
        /// </summary>	
        public int? pro_recharge_type { get; set; }

        /// <summary>
        ///  完成时间 
        /// </summary>	
        public DateTime? pro_complete_time { get; set; }

        /// <summary>
        /// 充值来源
        /// </summary>
        public int? pro_transaction_resource { get; set; }


        public string TrxId { get; set; }
        /// <summary>
        /// 银行流水
        /// </summary>
        public string DepoBankSeq { get; set; }

        public string GateBusiId { get; set; }
        public string BankId { get; set; }
        public string OpenAcctId { get; set; }
        public string FeeAmt { get; set; }
        public string FeeCustId { get; set; }
        public string FeeAcctId { get; set; }
        public string ServFee { get; set; }
        public string ServFeeAcctId { get; set; }
        public string MarketingInformation { get; set; }

        #endregion

    }
}
