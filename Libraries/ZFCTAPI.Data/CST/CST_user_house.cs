using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ZFCTAPI.Data.SYS;

namespace ZFCTAPI.Data.CST
{
    /// <summary>
    /// 数据表CST_user_house的数据库实体类
    /// </summary>
    [Table("CST_user_house")]
    public class CST_user_house:BaseEntity
    {
        #region 关联数据
        /// <summary>
        /// 用户信息表
        /// </summary>
        public virtual CST_user_info UserInfo { get; set; }

        /// <summary>
        /// 供款状况
        /// </summary>
        public virtual SYS_data_dictionary PayState { get; set; }
        #endregion

        #region 基本信息
        /// <summary>
        ///  主键ID 
        /// </summary>	
        //public int Id { get; set; }

        /// <summary>
        ///  用户ID 
        /// </summary>	
        public int? cst_user_id { get; set; }

        /// <summary>
        ///  房产地址 
        /// </summary>	
        public string cst_house_address { get; set; }

        /// <summary>
        ///  建筑面积 
        /// </summary>	
        public decimal? cst_house_area { get; set; }

        /// <summary>
        ///  1：有 0：无 
        /// </summary>	
        public int? cst_pay_state { get; set; }

        /// <summary>
        ///  建筑日期 
        /// </summary>	
        public DateTime? cst_house_date { get; set; }

        /// <summary>
        ///  所有权1 
        /// </summary>	
        public string cst_owner_one { get; set; }

        /// <summary>
        ///  所有权1产权份额 
        /// </summary>	
        public decimal? cst_share_one { get; set; }

        /// <summary>
        ///  所有权2 
        /// </summary>	
        public string cst_owner_two { get; set; }

        /// <summary>
        ///  所有权2产权份额 
        /// </summary>	
        public decimal? cst_share_two { get; set; }

        /// <summary>
        ///  贷款年限 
        /// </summary>	
        public int? cst_loan_period { get; set; }

        /// <summary>
        ///  每月供款 
        /// </summary>	
        public decimal? cst_month_pay { get; set; }

        /// <summary>
        ///  尚欠供款余额 
        /// </summary>	
        public decimal? cst_sett_pay { get; set; }

        /// <summary>
        ///  按揭银行 
        /// </summary>	
        public string cst_pay_bank { get; set; }
        #endregion
    }
}
