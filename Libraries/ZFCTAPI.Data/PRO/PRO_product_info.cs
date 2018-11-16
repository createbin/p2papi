using System;
using System.Collections.Generic;
using System.Text;
using Dapper.Contrib.Extensions;

namespace ZFCTAPI.Data.PRO
{
    /// <summary>
    /// 数据表PRO_product_info的数据库实体类
    /// </summary>
    [Table("PRO_product_info")]
    public partial class PRO_product_info : BaseEntity
    {
        /// <summary>
        ///  产品名称 
        /// </summary>	
        public string pdo_product_name { get; set; }

        /// <summary>
        ///  产品编码 
        /// </summary>	
        public int? pdo_product_code { get; set; }

        /// <summary>
        ///  父产品编码 
        /// </summary>	
        public int? pdo_parent_code { get; set; }

        /// <summary>
        ///  网贷平台首页显示的产品类型图片的路径 
        /// </summary>	
        public string pro_product_logo { get; set; }

        /// <summary>
        ///  我要投资的产品列表显示的图片 
        /// </summary>	
        public string pro_proList_logo { get; set; }

        /// <summary>
        ///  产品描述 
        /// </summary>	
        public string pro_product_remark { get; set; }

        /// <summary>
        ///  备注 
        /// </summary>	
        public string pdo_product_mark { get; set; }

    }
}
