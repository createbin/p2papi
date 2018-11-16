using System;
using Dapper.Contrib.Extensions;

namespace ZFCTAPI.Data.CST
{
    /// <summary>
    /// 数据表CST_user_info的数据库实体类
    /// </summary>
    [Table("CST_user_info")]
    public partial class CST_user_info : BaseEntity
    {
        #region 用户基本信息
        /// <summary>
        ///  实名认证Id 
        /// </summary>	
        public int? cst_real_Id { get; set; }

        /// <summary>
        ///  父级用户Id 
        /// </summary>	
        public int? parent_user_Id { get; set; }

        /// <summary>
        ///  用户名 
        /// </summary>	
        public string cst_user_name { get; set; }

        /// <summary>
        ///  登录密码 
        /// </summary>	
        public string cst_user_pwd { get; set; }

        /// <summary>
        ///  交易密码 
        /// </summary>	
        public string cst_deal_pwd { get; set; }

        /// <summary>
        ///  1：个人 2：企业 
        /// </summary>	
        public int? cst_user_type { get; set; }

        /// <summary>
        ///  手机号码 
        /// </summary>	
        public string cst_user_phone { get; set; }

        /// <summary>
        ///  电子邮箱 
        /// </summary>	
        public string cst_user_email { get; set; }

        /// <summary>
        ///  登录次数 
        /// </summary>	
        public int? cst_login_count { get; set; }

        /// <summary>
        ///  网站头像 
        /// </summary>	
        public string cst_user_pic { get; set; }



        /// <summary>
        ///  积分最后修改时间 
        /// </summary>	
        public DateTime? cst_score_edit_date { get; set; }

        /// <summary>
        ///  1：是 0：否 
        /// </summary>	
        public int? cst_user_state { get; set; }

        /// <summary>
        ///  1：开启 0：未开启 
        /// </summary>	
        public int? cst_automatic_invest { get; set; }

        /// <summary>
        ///  外键表：CST_user_info 
        /// </summary>	
        public int? cst_recommend_userId { get; set; }

        /// <summary>
        ///  推荐人类型 
        /// </summary>	
        public int? cst_recommend_type { get; set; }

        /// <summary>
        ///  信用额度 
        /// </summary>	
        public decimal? cst_loan_credit { get; set; }

        /// <summary>
        ///  可用信用额度 
        /// </summary>	
        public decimal? cst_credit_sett { get; set; }

        /// <summary>
        ///  注册日期 
        /// </summary>	
        public DateTime? cst_add_date { get; set; }

        /// <summary>
        ///   0：否 ;  1:实名认证审核中 2：实名认证通过；3：审核未通过 
        /// </summary>	
        public int? cst_realProve_status { get; set; }

        /// <summary>
        ///  邮箱激活时验证码 
        /// </summary>	
        public string cst_verify_code { get; set; }

        /// <summary>
        ///  邮箱激活时时间 
        /// </summary>	
        public DateTime? cst_verify_date { get; set; }

        /// <summary>
        ///  手机认证日期 
        /// </summary>	
        public DateTime? cst_mobile_date { get; set; }

        /// <summary>
        ///  婚姻状况 
        /// </summary>
        public int? cst_is_marry { get; set; }


        /// <summary>
        ///  联系人 
        /// </summary>
        public string cst_contract_person { get; set; }


        /// <summary>
        ///  联系人电话 
        /// </summary>
        public string cst_contract_phone { get; set; }


        /// <summary>
        ///  月收入 
        /// </summary>
        public decimal? cst_month_money { get; set; }



        /// <summary>
        ///  所属地区 
        /// </summary>
        public int? cst_area_id { get; set; }

        /// <summary>
        /// 用户所属机构ID
        /// </summary>
        public int? cst_gov_id { get; set; }

        /// <summary>
        ///  详细地址 
        /// </summary>
        public string cst_person_address { get; set; }

        /// <summary>
        ///  第三方支付账户表主键 
        /// </summary>
        public int? cst_account_id { get; set; }

        /// <summary>
        /// customer表外键
        /// </summary>
        public int? cst_customer_id { get; set; }

        /// <summary>
        /// 是否在职
        /// </summary>
        public bool? cst_is_work { get; set; }

        /// <summary>
        /// 单位id
        /// </summary>
        public int? pay_company_id { get; set; }

        /// <summary>
        /// 交易密码
        /// </summary>
        public string cst_trade_pwd { get; set; }

        /// <summary>
        /// 营业部ID
        /// </summary>
        public int? YingybID { get; set; }
        /// <summary>
        /// 用户等级/角色 0理财经理1 理财经理员工
        /// </summary>
        public int? cst_licairank { get; set; }
        /// <summary>
        /// 手势密码
        /// </summary>
        public string GestureCipher { get; set; }
        /// <summary>
        /// 企業用戶
        /// </summary>
        public int? CompanyId { get; set; }
        /// <summary>
        /// 学历
        /// </summary>
        public int? cst_user_education { get; set; }

        /// <summary>
        /// 收入情况
        /// </summary>
        public int? cst_income_type { get; set; }

        /// <summary>
        /// 负债情况
        /// </summary>
        public int? cst_user_debt { get; set; }
        #endregion

    }
}
