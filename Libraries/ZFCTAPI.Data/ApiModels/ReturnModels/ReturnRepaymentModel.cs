using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.ApiModels.ReturnModels
{
    public class ReturnRepaymentModel
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 回款计划表ID
        /// </summary>
        public int InvestPlanId { get; set; }

        /// <summary>
        ///  应还本金 
        /// </summary>	
        public decimal? Pro_pay_money { get; set; }

        /// <summary>
        /// 实还本金
        /// </summary>
        public decimal? Pro_collect_money { get; set; }

        /// <summary>
        ///  应还利息 
        /// </summary>	
        public decimal? Pro_pay_rate { get; set; }

        /// <summary>
        /// 实还利息
        /// </summary>
        public decimal? Pro_collect_rate { get; set; }

        /// <summary>
        ///  应还罚金 
        /// </summary>	
        public decimal? Pro_pay_over_rate { get; set; }

        /// <summary>
        /// 投资金额
        /// </summary>
        public decimal Pro_invest_money { get; set; }

        /// <summary>
        ///  应还总额 
        /// </summary>	
        public decimal? Pro_pay_total { get; set; }

        /// <summary>
        ///  使用体验金金额 
        /// </summary>	
        public decimal? pro_experience_money { get; set; }

        /// <summary>
        ///  开户账户 
        /// </summary>	
        public string Act_account_no { get; set; }

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
        /// 账户名称
        /// </summary>
        public string cst_plaCustName { get; set; }

        /// <summary>
        /// 开户类型 1 投资户 2 融资户
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
    }

    /// <summary>
    /// 募集结果上报投资人信息
    /// </summary>
    public class InvestPerson
    {
        /// <summary>
        /// 投资人序号
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 标的ID
        /// </summary>
        public int LoanId { get; set; }

        /// <summary>
        ///  开户账户 
        /// </summary>	
        public string Act_account_no { get; set; }

        /// <summary>
        /// 投资金额
        /// </summary>
        public decimal Pro_invest_money { get; set; }

        /// <summary>
        /// 投标冻结订单流水号 pro_fro_orderno
        /// </summary>
        public string Pro_fro_orderno { get; set; }

        /// <summary>
        /// 渤海银行账户号
        /// </summary>
        public string cst_plaCustId { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string cst_plaCustName { get; set; }

        /// <summary>
        /// 开户类型 1 投资户 2 融资户
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
        /// 流水号
        /// </summary>
        public string pro_order_no { get; set; }
    }
}
