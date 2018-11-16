using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ZFCTAPI.Data.SYS;

namespace ZFCTAPI.Data.CST
{
    /// <summary>
	/// 数据表CST_cust_info的数据库实体类
	/// </summary>
	[Table("CST_cust_info")]
	public partial class CST_cust_info : BaseEntity
    {
        #region 关联数据
        /// <summary>
        /// 客户信息
        /// </summary>
        public virtual CST_user_info UserInfo { get; set; }
        /// <summary>
        /// 企业性质
        /// </summary>
        public virtual SYS_data_dictionary CompanyType { get; set; }
        /// <summary>
        /// 企业规模
        /// </summary>
        public virtual SYS_data_dictionary CompanyScale { get; set; }
        /// <summary>
        /// 证件类型
        /// </summary>
        public virtual SYS_data_dictionary CardType { get; set; }

        #endregion

        #region 基本信息
        /// <summary>
        ///  外键表：CST_user_info 
        /// </summary>	
        public int? cst_user_id { get; set; }

        /// <summary>
        ///  公司名称 
        /// </summary>	
        public string cst_cust_name { get; set; }

        /// <summary>
        ///  组织机构代码 
        /// </summary>	
        public string cst_cust_no { get; set; }

        /// <summary>
        ///  1：政府机关 2：国有企业 3：台（港、澳）资企业 4：合资企业 5：个体户 6：事业性单位 7：私营企业 
        /// </summary>	
        public int? cst_company_type { get; set; }

        /// <summary>
        ///  1: 50人以上 2: 50~500人以下 3: 500人以上 
        /// </summary>	
        public int? cst_company_scale { get; set; }

        /// <summary>
        ///  资产总额 
        /// </summary>	
        public decimal? cst_total_capital { get; set; }

        /// <summary>
        ///  注册资金 
        /// </summary>	
        public decimal? cst_registe_capital { get; set; }

        /// <summary>
        ///  注册日期 
        /// </summary>	
        public DateTime? cst_registe_date { get; set; }

        /// <summary>
        ///  注册地址 
        /// </summary>	
        public string cst_registe_address { get; set; }

        /// <summary>
        ///  联系人 
        /// </summary>	
        public string cst_contractor_name { get; set; }

        /// <summary>
        ///  联系电话 
        /// </summary>	
        public string cst_contractor_phone { get; set; }

        /// <summary>
        ///  家庭住址 
        /// </summary>	
        public string cst_contractor_address { get; set; }

        /// <summary>
        ///  法定代表人 
        /// </summary>	
        public string cst_cust_lawer { get; set; }

        /// <summary>
        ///  证件类型 
        /// </summary>	
        public int cst_card_type { get; set; }

        /// <summary>
        ///  证件号码 
        /// </summary>	
        public string cst_card_no { get; set; }

        /// <summary>
        ///  公司简介 
        /// </summary>	
        public string cst_company_introl { get; set; }

        /// <summary>
        ///  公司成就 
        /// </summary>	
        public string cst_company_success { get; set; }

        /// <summary>
        ///  办公地址 
        /// </summary>	
        public string cst_company_address { get; set; }

        /// <summary>
        ///  营业执照 
        /// </summary>	
        public string cst_business_license { get; set; }

        /// <summary>
        ///  营业执照号码 
        /// </summary>	
        public string cst_business_license_no { get; set; }
        #endregion
    }
}
