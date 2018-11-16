using System;
using System.Collections.Generic;
using System.Text;
using ZFCTAPI.Data.CST;
using ZFCTAPI.Data.SYS;

namespace ZFCTAPI.Data.GOV
{
    /// <summary>
	/// 数据表GOV_company_info的数据库实体类
	/// </summary>
	public partial class GOV_company_info : BaseEntity
    {
        #region 关联数据

        /// <summary>
        /// 用户信息
        /// </summary>
        public virtual CST_user_info Cstuserinfo { get; set; }

        /// <summary>
        /// 企业规模
        /// </summary>
        public virtual SYS_data_dictionary Govscale { get; set; }

        /// <summary>
        /// 证件类型
        /// </summary>
        public virtual SYS_data_dictionary Govcardtype { get; set; }

        /// <summary>
        /// 机构类型
        /// </summary>
        public virtual SYS_data_dictionary GovType { get; set; }

        /// <summary>
        /// 第三方开户账号
        /// </summary>
        public virtual CST_account_info GovAccountInfo { get; set; }

        /// <summary>
        /// 所属区域
        /// </summary>
        public virtual SYS_area_info Sysareainfo { get; set; }

        #endregion

        #region 基本信息
        /// <summary>
		/// gov_user_name
		/// </summary>	
        public string gov_user_name { get; set; }

        /// <summary>
        /// gov_user_id
        /// </summary>	
        public int? gov_user_id { get; set; }

        /// <summary>
		/// sys_gov_email
		/// </summary>	
        public string sys_gov_email { get; set; }

        /// <summary>
        /// gov_no
        /// </summary>	
        public string gov_no { get; set; }

        /// <summary>
        /// gov_business_license
        /// </summary>	
        public string gov_business_license { get; set; }

        /// <summary>
        /// gov_scale
        /// </summary>	
        public int? gov_scale { get; set; }

        /// <summary>
        /// gov_registe_capital
        /// </summary>	
        public decimal? gov_registe_capital { get; set; }

        /// <summary>
        /// gov_total_capital
        /// </summary>	
        public decimal? gov_total_capital { get; set; }

        /// <summary>
        /// gov_max_lend_money
        /// </summary>	
        public decimal? gov_max_lend_money { get; set; }

        /// <summary>
        /// gov_registe_division
        /// </summary>	
        public string gov_registe_division { get; set; }

        /// <summary>
        /// gov_registe_address
        /// </summary>	
        public string gov_registe_address { get; set; }

        /// <summary>
        /// gov_registe_date
        /// </summary>	
        public DateTime? gov_registe_date { get; set; }

        /// <summary>
        /// gov_contracter
        /// </summary>	
        public string gov_contracter { get; set; }

        /// <summary>
        /// gov_contract_phone
        /// </summary>	
        public string gov_contract_phone { get; set; }

        /// <summary>
        /// gov_cust_manager
        /// </summary>	
        public int? gov_cust_manager { get; set; }

        /// <summary>
        /// gov_lawer
        /// </summary>	
        public string gov_lawer { get; set; }

        /// <summary>
        /// gov_card_type
        /// </summary>	
        public int? gov_card_type { get; set; }

        /// <summary>
        /// gov_card_no
        /// </summary>	
        public string gov_card_no { get; set; }

        /// <summary>
        /// gov_credit_level
        /// </summary>	
        public string gov_credit_level { get; set; }

        /// <summary>
        /// gov_area_id
        /// </summary>	
        public int? gov_area_id { get; set; }

        /// <summary>
        /// 机构类型（1：小贷，2：担保，3：理财加盟商）
        /// </summary>
        public int? gov_type { set; get; }


        /// <summary>
        /// 担保机构主键
        /// </summary>
        public int? gov_guarId { set; get; }

        public virtual GOV_company_info GovCompanyInfo { get; set; }

        /// <summary>
        /// 担保机构
        /// </summary>
        private ICollection<GOV_company_info> _govCompanyInfos;

        public virtual ICollection<GOV_company_info> GovCompanyInfos
        {
            get { return _govCompanyInfos ?? (_govCompanyInfos = new List<GOV_company_info>()); }
            protected set { _govCompanyInfos = value; }
        }

        /// <summary>
		/// gov_address_detail
		/// </summary>	
        public string gov_address_detail { get; set; }

        /// <summary>
        /// gov_max_guar_money
        /// </summary>	
        public decimal? gov_max_guar_money { get; set; }

        /// <summary>
        /// gov_sett_guar_money
        /// </summary>	
        public decimal? gov_sett_guar_money { get; set; }

        /// <summary>
        /// gov_logo
        /// </summary>	
        public string gov_logo { get; set; }

        /// <summary>
        /// gov_introl
        /// </summary>	
        public string gov_introl { get; set; }

        /// <summary>
        /// gov_team_manage
        /// </summary>	
        public string gov_team_manage { get; set; }

        /// <summary>
        /// gov_develop_history
        /// </summary>	
        public string gov_develop_history { get; set; }

        /// <summary>
        /// gov_guar_card
        /// </summary>	
        public string gov_guar_card { get; set; }

        /// <summary>
        /// gov_partner
        /// </summary>	
        public string gov_partner { get; set; }

        /// <summary>
        /// gov_cooperat_agreement
        /// </summary>	
        public string gov_cooperat_agreement { get; set; }

        /// <summary>
        /// gov_is_use
        /// </summary>	
        public bool gov_is_use { get; set; }

        /// <summary>
        /// gov_state
        /// </summary>	
        public int? gov_state { get; set; }

        /// <summary>
        /// gov_check_emp
        /// </summary>	
        public int? gov_check_emp { get; set; }

        /// <summary>
        /// gov_check_date
        /// </summary>	
        public DateTime? gov_check_date { get; set; }

        /// <summary>
        /// gov_add_emp
        /// </summary>	
        public int? gov_add_emp { get; set; }

        /// <summary>
        /// gov_add_date
        /// </summary>	
        public DateTime? gov_add_date { get; set; }

        /// <summary>
        /// gov_delsign
        /// </summary>	
        public bool gov_delsign { get; set; }

        /// <summary>
        /// 机构对应开户表的主键
        /// </summary>
        public int? gov_account_id { get; set; }

        #endregion
    }
}
