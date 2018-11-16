using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.Repayment
{
    /// <summary>
    /// 数据表PRO_pay_record的数据库实体类
    /// </summary>
    [Table("PRO_pay_record")]
    public class PRO_pay_record: BaseEntity
    {
        #region 基本信息
        /// <summary>
        ///  主键ID 
        /// </summary>	
        //public int Id { get; set; }

        /// <summary>
        ///  贷款ID 
        /// </summary>	
        public int? pro_loan_id { get; set; }

        /// <summary>
        ///  还款日期 
        /// </summary>	
        public DateTime? pro_pay_date { get; set; }

        /// <summary>
        ///  归还本金 
        /// </summary>	
        public decimal? pro_pay_money { get; set; }

        /// <summary>
        ///  归还利息 
        /// </summary>	
        public decimal? pro_pay_rate { get; set; }

        /// <summary>
        ///  归还罚金 
        /// </summary>	
        public decimal? pro_pay_over_rate { get; set; }

        /// <summary>
        ///  归还平台服务费 
        /// </summary>	
        public decimal? pro_pay_service_fee { get; set; }

        /// <summary>
        ///  归还担保服务费 
        /// </summary>	
        public decimal? pro_pay_guar_fee { get; set; }

        /// <summary>
        ///  因罚金是时时计算的，当第一次罚金未还清时，保留剩余未还罚金 
        /// </summary>	
        public decimal? pro_sett_over_rate { get; set; }

        /// <summary>
        ///  归还期数 
        /// </summary>	
        public int? pro_pay_period { get; set; }

        /// <summary>
        ///  1：正常还款 2：平台代还 3：强制还款 
        /// </summary>	
        public int? pro_pay_type { get; set; }
        #endregion
    }
}
