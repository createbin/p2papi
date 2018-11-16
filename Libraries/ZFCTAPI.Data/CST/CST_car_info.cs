using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ZFCTAPI.Data.CST
{
    /// <summary>
    /// 数据表CST_car_info的数据库实体类
    /// </summary>
    [Table("CST_car_info")]
    public partial class CST_car_info : BaseEntity
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int cst_user_id { get; set; }


        /// <summary>
        ///  汽车品牌 
        /// </summary>	
        public string cst_car_brand { get; set; }

        /// <summary>
        ///  汽车车系 
        /// </summary>	
        public string cst_car_type { get; set; }

        /// <summary>
        ///  颜色 
        /// </summary>	
        public string cst_car_color { get; set; }

        /// <summary>
        ///  排量 
        /// </summary>	
        public string cst_car_output { get; set; }

        /// <summary>
        ///  购买年份 
        /// </summary>	
        public int? cst_car_buyYear { get; set; }

        /// <summary>
        ///  上牌日期 
        /// </summary>	
        public int? cst_card_year { get; set; }

        /// <summary>
        ///  里程数 
        /// </summary>	
        public decimal? cst_car_run { get; set; }

        /// <summary>
        ///  评估价格 
        /// </summary>	
        public decimal? cst_assess_money { get; set; }

        /// <summary>
        ///  汽车现址 
        /// </summary>	
        public string cst_car_address { get; set; }

        public virtual CST_user_info UserInfo { get; set; }
    }
}
