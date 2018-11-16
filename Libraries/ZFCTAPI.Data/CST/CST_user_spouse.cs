using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ZFCTAPI.Data.CST
{
    /// <summary>
    /// 数据表CST_user_spouse的数据库实体类
    /// </summary>
    [Table("CST_user_spouse")]
    public partial class CST_user_spouse : BaseEntity
    {
        /// <summary>
        ///  用户ID 
        /// </summary>	
        public int? cst_user_id { get; set; }

        /// <summary>
        ///  配偶姓名 
        /// </summary>	
        public string cst_spouse_name { get; set; }

        /// <summary>
        ///  每月薪资 
        /// </summary>	
        public decimal? cst_spouse_income { get; set; }

        /// <summary>
        ///  移动电话 
        /// </summary>	
        public string cst_spouse_phone { get; set; }

        /// <summary>
        ///  单位电话 
        /// </summary>	
        public string cst_work_phone { get; set; }

        /// <summary>
        ///  工作单位 
        /// </summary>	
        public string cst_work_union { get; set; }

        /// <summary>
        ///  职务 
        /// </summary>	
        public string cst_work_position { get; set; }

        /// <summary>
        ///  单位地址 
        /// </summary>	
        public string cst_work_address { get; set; }

        public virtual CST_user_info UserInfo { get; set; }
    }
}
