using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using ZFCTAPI.Data.SYS;

namespace ZFCTAPI.Data.PRO
{
    /// <summary>
    /// 数据表PRO_lend_record的数据库实体类
    /// </summary>
    [Table("PRO_lend_record")]
    public partial class PRO_lend_record : BaseEntity
    {

        #region 基本信息
        /// <summary>
        ///  主键ID 
        /// </summary>	
        //public int Id { get; set; }

        /// <summary>
        ///  外键：PRO_loan_apply 
        /// </summary>	
        public int? len_loan_id { get; set; }

        /// <summary>
        /// 外键：PRO_transfer_apply
        /// </summary>
        public int? len_transfer_id { get; set; }

        /// <summary>
        ///  外键表：CST_user_info 
        /// </summary>	
        public int? len_user_id { get; set; }

        /// <summary>
        ///  1：正常标 2：转让标   
        /// </summary>	
        public int? len_loan_type { get; set; }

        /// <summary>
        ///  实际划转金额 
        /// </summary>	
        public decimal? len_lend_money { get; set; }

        /// <summary>
        ///  应收手续费 
        /// </summary>	
        public decimal? len_pay_fee { get; set; }

        /// <summary>
        ///  应收担保费 
        /// </summary>	
        public decimal? len_pay_guar_fee { get; set; }

        /// <summary>
        ///  实收手续费 
        /// </summary>	
        public decimal? len_collect_fee { get; set; }

        /// <summary>
        ///  实收担保费 
        /// </summary>	
        public decimal? len_collect_guar_fee { get; set; }

        /// <summary>
        ///  放款日期 
        /// </summary>	
        public DateTime? len_lend_date { get; set; }

        #endregion
    }
}
