using System;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;

namespace ZFCTAPI.Data.CST
{
    /// <summary>
    /// 数据表CST_account_info的数据库实体类
    /// </summary>
    [Table("CST_account_info")]
    public partial class CST_account_info : BaseEntity
    {
        /// <summary>
        ///  外键表：CST_user_info（用户类型1、2）外键表：SYS_gov_info（用户类型3）外键表：SYS_employee（用户类型4） 
        /// </summary>	
        public int? act_user_id { get; set; }

        /// <summary>
        ///  1：个人用户 2：企业用户 3：平台加盟商（小贷公司/担保公司）4：理财经理 
        /// </summary>	
        public int? act_user_type { get; set; }

        /// <summary>
        ///  法人姓名 
        /// </summary>	
        public string act_legal_name { get; set; }

        /// <summary>
        ///  用户姓名(企业名称) 
        /// </summary>	
        public string act_user_name { get; set; }

        /// <summary>
        ///  身份证号(法人身份证号码) 
        /// </summary>	
        public string act_user_card { get; set; }

        /// <summary>
        ///  开户账户 
        /// </summary>	
        public string act_account_no { get; set; }

        /// <summary>
        ///  手机号码(法人手机号码) 
        /// </summary>	
        public string act_user_phone { get; set; }

        /// <summary>
        ///  邮箱地址(法人邮箱地址) 
        /// </summary>	
        public string act_user_email { get; set; }

        /// <summary>
        ///  开户银行地区代码
        /// </summary>	
        public string act_bank_area_no { get; set; }

        /// <summary>
        ///  开户银行行别
        /// </summary>	
        public string act_bank_level { get; set; }

        /// <summary>
        ///  开户银行支行名称
        /// </summary>	
        public string act_bank_name { get; set; }

        /// <summary>
        ///  账号(企业对公户账号) 
        /// </summary>	
        public string act_user_account { get; set; }

        /// <summary>
        ///  提现密码 
        /// </summary>	
        public string act_cash_pwd { get; set; }

        /// <summary>
        ///  登录密码 
        /// </summary>	
        public string act_login_pwd { get; set; }

        /// <summary>
        ///  备注 
        /// </summary>	
        public string act_account_mark { get; set; }
        /// <summary>
        /// 是否结算中心开户
        /// </summary>
        public bool JieSuan { get; set; }
        /// <summary>
        /// 是否渤海开户
        /// </summary>
        public bool BoHai { get; set; }
        /// <summary>
        /// 渤海银行账户号
        /// </summary>
        public string cst_plaCustId { get; set; }

        /// <summary>
        /// 开户类型 1 投资户 2 融资户 3投融资户 4企业户 5担保户
        /// </summary>
        public int? act_business_property { get; set; }
        /// <summary>
        /// 投资户结算id
        /// 默认以10000+id
        /// </summary>
        public string invest_platform_id { get; set; }
        /// <summary>
        /// 融资户id
        /// 默认以20000+id
        /// </summary>
        public string financing_platform_id { get; set; }
        /// <summary>
        /// 个人线下充值账户
        /// </summary>
        public string personal_charge_account { get; set; }
        /// <summary>
        /// 结算返回信息
        /// </summary>
        public string JieSuanMsg { get; set; }
        /// <summary>
        /// 渤海返回信息
        /// </summary>
        public string BhMsg { get; set; }
        /// <summary>
        /// 结算返回码
        /// </summary>
        public string JieSuanCode { get; set; }
        /// <summary>
        /// 渤海返回码
        /// </summary>
        public string BhCode { get; set; }
        /// <summary>
        /// 结算开户时间
        /// </summary>
        public DateTime? JieSuanTime { get; set; }
    }
}
