using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.PRO
{
    /// <summary>
    /// 数据表PRO_house_info的数据库实体类
    /// </summary>
    public partial class PRO_house_info : BaseEntity
    {
        public virtual PRO_loan_info LoanInfo { get; set; }

        /// <summary>
        ///  主键ID 
        /// </summary>	
        //public int Id { get; set; }

        /// <summary>
        ///  贷款ID 
        /// </summary>	
        public int? pro_loan_id { get; set; }

        /// <summary>
        ///  房产面积 
        /// </summary>	
        public decimal? pro_house_measure { get; set; }

        /// <summary>
        ///  占地面积 
        /// </summary>	
        public decimal? pro_cover_measure { get; set; }

        /// <summary>
        ///  房产证号 
        /// </summary>	
        public string pro_house_no { get; set; }

        /// <summary>
        ///  所属小区 
        /// </summary>	
        public string pro_house_area { get; set; }

        /// <summary>
        ///  房龄 
        /// </summary>	
        public decimal? pro_house_age { get; set; }

        /// <summary>
        ///  1：是 0：否 
        /// </summary>	
        public bool pro_house_mortgage { get; set; }

        /// <summary>
        ///  评估价格 
        /// </summary>	
        public decimal? pro_assess_money { get; set; }
    }
}
