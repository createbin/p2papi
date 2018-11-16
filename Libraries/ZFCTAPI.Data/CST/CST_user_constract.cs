using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ZFCTAPI.Data.CST
{
    /// <summary>
	/// 数据表CST_user_constract的数据库实体类
	/// </summary>
	[Table("CST_user_constract")]
	public partial class CST_user_constract : BaseEntity
    {
        /// <summary>
        ///  外键表：CST_user_info 
        /// </summary>	
        public int? cst_user_id { get; set; }

        /// <summary>
        ///  手机号码 
        /// </summary>	
        public string cst_user_phone { get; set; }

        /// <summary>
        ///  居住地电话 
        /// </summary>	
        public string cst_address_phone { get; set; }

        /// <summary>
        ///  居住所在省市 
        /// </summary>	
        public string cst_address_city { get; set; }

        /// <summary>
        ///  居住地邮编 
        /// </summary>	
        public string cst_address_post { get; set; }

        /// <summary>
        ///  现居住地址 
        /// </summary>	
        public string cst_user_address { get; set; }

        /// <summary>
        ///  第二联系人姓名 
        /// </summary>	
        public string cst_two_name { get; set; }

        /// <summary>
        ///  第二联系人关系 
        /// </summary>	
        public string cst_two_relation { get; set; }

        /// <summary>
        ///  第二联系人电话 
        /// </summary>	
        public string cst_two_phone { get; set; }

        /// <summary>
        ///  第三联系人姓名 
        /// </summary>	
        public string cst_three_name { get; set; }

        /// <summary>
        ///  第三联系人关系 
        /// </summary>	
        public string cst_three_relation { get; set; }

        /// <summary>
        ///  第三联系人电话 
        /// </summary>	
        public string cst_three_phone { get; set; }

        /// <summary>
        ///  第四联系人姓名 
        /// </summary>	
        public string cst_four_name { get; set; }

        /// <summary>
        ///  第四联系人关系 
        /// </summary>	
        public string cst_four_relation { get; set; }

        /// <summary>
        ///  第四联系人电话 
        /// </summary>	
        public string cst_four_phone { get; set; }

        /// <summary>
        ///  MSN 
        /// </summary>	
        public string cst_user_msn { get; set; }

        /// <summary>
        ///  QQ 
        /// </summary>	
        public string cst_user_qq { get; set; }
    }
}
