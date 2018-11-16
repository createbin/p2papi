using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ZFCTAPI.Data.CST;
using ZFCTAPI.Data.GOV;
using ZFCTAPI.Data.SYS;

namespace ZFCTAPI.Data.PRO
{
    /// <summary>
    /// 数据表PRO_loan_info的数据库实体类
    /// </summary>
    [Table("PRO_loan_info")]
    public partial class PRO_loan_info : BaseEntity
    {
        #region 项目基本信息

        /// <summary>
        ///  项目logo 
        /// </summary>	
        public string pro_loan_logo { get; set; }

        /// <summary>
        ///  项目编号 
        /// </summary>	
        public string pro_loan_no { get; set; }

        /// <summary>
        ///  借款人类型 (10为企业)
        /// </summary>	
        public int? pro_add_emp_type { get; set; }

        /// <summary>
        ///  外键表：CST_user_info
        ///外键表：SYS_employee 
        /// </summary>	
        public int? pro_add_emp { get; set; }

        /// <summary>
        ///  借款人姓名 
        /// </summary>	
        public string pro_user_name { get; set; }

        /// <summary>
        ///  外键表：SYS_gov_info
        ///借款机构
        /// </summary>	
        public int? pro_loan_gov { get; set; }

        /// <summary>
        ///  外键表：dbo.GOV_company_info
        ///担保机构
        /// </summary>	
        public int? pro_loan_guar_gov { get; set; }

        /// <summary>
        ///  借款金额 
        /// </summary>	
        public decimal? pro_loan_money { get; set; }

        /// <summary>
        /// 年化收益率+追加收益率
        /// </summary>	
        public decimal? pro_loan_rate { get; set; }
        /// <summary>
        /// 年化收益率
        /// </summary>
        public decimal? pro_loan_originalrate { get; set; }
        /// <summary>
        /// 追加收益率
        /// </summary>
        public decimal? pro_loan_ratehike { get; set; }

        /// <summary>
        ///  借款期限 
        /// </summary>	
        public int? pro_loan_period { get; set; }

        /// <summary>
        /// 项目期限
        /// </summary>	
        public int? pro_period_type { get; set; }

        /// <summary>
        ///  项目进度 
        /// </summary>	
        public decimal? pro_loan_speed { get; set; }

        /// <summary>
        ///  最低投标金额 
        /// </summary>	
        public decimal? pro_min_invest_money { get; set; }

        /// <summary>
        ///  最高投标金额 
        /// </summary>	
        public decimal? pro_max_invest_money { get; set; }

        /// <summary>
        ///  1：等额本息 2：按月还息到期还本 3：等额本金

        /// </summary>	
        public string pro_pay_type { get; set; }

        /// <summary>
        ///  招标开始日期 
        /// </summary>	
        public DateTime? pro_invest_startTime { get; set; }

        /// <summary>
        ///  招标到期日 
        /// </summary>	
        public DateTime? pro_invest_endDate { get; set; }

        /// <summary>
        ///  1：固定还款日 2：非固定还款日

        /// </summary>	
        public int? pro_collect_type { get; set; }

        /// <summary>
        ///  借款用途 
        /// </summary>	
        public string pro_loan_use { get; set; }

        /// <summary>
        ///  项目备注 
        /// </summary>	
        public string pro_loan_introl { get; set; }

        /// <summary>
        /// 还款来源
        /// </summary>
        public string pro_loan_Sourcerepayment { get; set; }

        /// <summary>
        ///  24:还款中;25:已结清
        /// </summary>	
        public int? pro_loan_state { get; set; }

        /// <summary>
        /// 是否全部划转成功（如果有部分投资人钱未划转成功，那么该笔项目仍然能够操作划转，但项目状态仍然为还款中）
        /// </summary>
        public bool pro_isAllTrans { get; set; }

        /// <summary>
        ///  发布日期 
        /// </summary>	
        public DateTime? pro_public_date { get; set; }

        /// <summary>
        ///  满标日期 
        /// </summary>	
        public DateTime? pro_full_date { get; set; }

        /// <summary>
        /// 结算日期
        /// </summary>
        public int? pro_collect_date { get; set; }

        /// <summary>
        ///  添加日期即申请日期 
        /// </summary>	
        public DateTime? pro_add_date { get; set; }

        /// <summary>
        ///  开始日期即放款日期 
        /// </summary>	
        public DateTime? pro_start_date { get; set; }

        /// <summary>
        ///  流标日期 
        /// </summary>	
        public DateTime? pro_missing_date { get; set; }

        /// <summary>
        ///  到期日期即截止日期 = 放款日期 + 借款期限 
        /// </summary>	
        public DateTime? pro_end_date { get; set; }

        /// <summary>
        ///  1：已删除 0：未删除 
        /// </summary>	
        public bool pro_delsign { get; set; }

        /// <summary>
        ///  1：个人 2：企业 3：借款机构 4：平台 
        /// </summary>	
        public int? pro_add_type { get; set; }

        /// <summary>
        ///  外键表：CST_user_info
        ///外键表：SYS_employee"
        ///"外键表：CST_user_info
        ///外键表：SYS_employee

        /// </summary>	
        public int? pro_add_id { get; set; }

        /// <summary>
        ///  是否允许多投 0：是 1：否 
        /// </summary>	
        public bool pro_is_much { get; set; }

        /// <summary>
        ///  剩余可投金额 
        /// </summary>	
        public decimal? pro_surplus_money { get; set; }

        /// <summary>
        ///  实际到账金额 
        /// </summary>	
        public decimal? pro_real_money { get; set; }

        /// <summary>
        ///  能否使用红包 
        /// </summary>	
        public bool pro_enable_red { get; set; }

        /// <summary>
        ///  手机专享（只允许手机端投资） 
        /// </summary>	
        public bool pro_is_cell { get; set; }

        /// <summary>
        ///  新手专享（只允许新手投资） 
        /// </summary>	
        public bool pro_is_new { get; set; }

        /// <summary>
        ///  理财产品主键ID 
        /// </summary>	
        public int? pro_prod_id { get; set; }

        /// <summary>
        ///  产品类型主键Id 
        /// </summary>	
        public int? pro_prod_typeId { get; set; }

        /// <summary>
        /// 当产品类型为企业经营贷的时候必填 
        /// </summary>
        public string pro_pro_licensecode { get; set; }

        /// <summary>
        /// 强制满标前原始金额
        /// </summary>
        public decimal? pro_fullscale_money { get; set; }

        /// <summary>
        /// 是否强制满标
        /// </summary>
        public bool pro_is_fullscale { get; set; }

        /// <summary>
        /// 编号（兆康线下）
        /// </summary>
        public string pro_lend_no { get; set; }

        /// <summary>
        /// 是否允许多次使用红包  0：否 1：是
        /// </summary>
        public bool pro_much_red { get; set; }

        /// <summary>
        /// 项目投资密码
        /// </summary>
        public string pro_pwd { get; set; }
        /// <summary>
        /// 设置是否可用体验金  0：否 1：是
        /// </summary>
        public int? pro_experiencegold { get; set; }

        public string pro_experiencegold_proportion { get; set; }

        /// <summary>
        /// 是否正在使用
        /// </summary>
        public bool pro_is_use { get; set; }

        /// <summary>
        /// 是否设置为中融宝
        /// </summary>
        public bool pro_is_product { get; set; }

        /// <summary>
        /// 最大使用红包金额限制
        /// </summary>
        public decimal? pro_max_red { get; set; }
        /// <summary>
        /// 恒丰返回的错误
        /// </summary>
        public string pro_pro_hferror { get; set; }
        //2016-08-31
        /// <summary>
        /// 该标的担保费 因为 费用总统计中这些数据会被删除或者修改 所以此处保留副本
        /// </summary>
        public decimal? pro_this_guaranteefee { get; set; }

        /// <summary>
        /// (手续费)该标的服务费 因为 费用总统计中这些数据会被删除或者修改 所以此处保留副本
        /// </summary>
        public decimal? pro_this_procedurefee { get; set; }

        /// <summary>
        /// 该标的服务费 设置为可以自定义
        /// </summary>
        public decimal? pro_this_servicefee { get; set; }

        /// <summary>
        /// 还款来源
        /// </summary>
        public int? pro_pay_source { get; set; }

        /// <summary>
        /// 保障措施
        /// </summary>
        public int? pro_guarantee_measures { get; set; }

        /// <summary>
        /// 风测结果
        /// </summary>
        public string pro_risk_result { get; set; }

        /// <summary>
        /// 借款用途
        /// </summary>
        public string pro_loan_purpose { get; set; }

        /// <summary>
        /// 还款保障
        /// </summary>
        public string pro_pay_measures { get; set; }

        #endregion

        /// <summary>
        /// 流水号(2018-3-31)
        /// </summary>
        public string tran_seq_no { get; set; }

        /// <summary>
        /// 满标true,流标fals
        /// </summary>
        public bool? Type { get; set; }

        /// <summary>
        /// 满标提交时间
        /// </summary>
        public DateTime? CreDt { get; set; }

        /// <summary>
        /// 担保人公司
        /// </summary>
        public int? pro_loan_guar_company { get; set; }

        /// <summary>
        /// 标的风险类型
        /// </summary>
        public int? pro_risk_type { get; set; }

        /// <summary>
        /// 投资合同
        /// 0 未生成
        /// 1 待生成
        /// 2 生成中
        /// 3 部分生成失败
        /// 4 生成成功
        /// 5 重新生成
        /// </summary>
        public int pro_invest_contract_status { get; set; }

        /// <summary>
        /// 银行满标处理返回消息
        /// </summary>
        public string bank_resp_desc { get; set; }

        /// <summary>
        /// 是否为渤海标的
        /// </summary>
        public bool Bohai { get; set; }
    }
}
