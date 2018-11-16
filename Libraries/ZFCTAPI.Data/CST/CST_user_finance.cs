using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ZFCTAPI.Data.CST
{
    /// <summary>
    /// 数据表CST_user_finance的数据库实体类
    /// </summary>
    [Table("CST_user_finance")]
    public partial class CST_user_finance : BaseEntity
    {

        public virtual CST_user_info UserInfo { get; set; }
        /// <summary>
        ///  外键表：CST_user_info 
        /// </summary>	
        public int? cst_user_id { get; set; }

        /// <summary>
        ///  每月无抵押贷款还款额 
        /// </summary>	
        public decimal? cst_unsecured_pay { get; set; }

        /// <summary>
        ///  每月信用卡还款金额 
        /// </summary>	
        public decimal? cst_credit_pay { get; set; }

        /// <summary>
        ///  每月房屋按揭金额 
        /// </summary>	
        public decimal? cst_house_pay { get; set; }

        /// <summary>
        ///  每月汽车按揭金额 
        /// </summary>	
        public decimal? cst_car_pay { get; set; }

        /// <summary>
        ///  1：是 0：否 
        /// </summary>	
        public int? cst_owner_house { get; set; }

        /// <summary>
        ///  1：是 0：否 
        /// </summary>	
        public int? cst_owner_car { get; set; }
    }
}
