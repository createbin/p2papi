using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.ApiModels.SubmitModels
{
    public class SLoanDetail : BaseSubmitModel
    {
        /// <summary>
        /// 标的id
        /// </summary>
        public int LoanId { get; set; }
    }

    public class SLoanInvester : BasePageModel
    {
        /// <summary>
        /// 标的id
        /// </summary>
        public int LoanId { get; set; }
    }

    public class SLoanPlan : BasePageModel
    {
        /// <summary>
        /// 标的id
        /// </summary>
        public int LoanId { get; set; }
    }

    public class SRepayDetail : BaseSubmitModel
    {
        /// <summary>
        /// 项目还款计划的id
        /// </summary>
        public int LoanPlanId { get; set; }
    }
    /// <summary>
    /// 单个红包划转接口
    /// </summary>
    public class SRedEnvelopeInfo:BaseSubmitModel
    {
        /// <summary>
        /// 营销活动编号
        /// </summary>
        public int LoanId { get; set; }
        /// <summary>
        /// 营销活动信息
        /// </summary>
        public string LoanName { get; set; }
        /// <summary>
        /// 用户平台码
        /// </summary>
        public string UserPlatformCode { get; set; }
        /// <summary>
        /// 红包金额
        /// </summary>
        public string CampaignMoney { get; set; }
        /// <summary>
        /// 预留字段
        /// </summary>
        public string Extension { get; set; }
    }


    public class SendPhoneCode : BaseSubmitModel
    {
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; }
    }

    /// <summary>
    /// 微信登录
    /// </summary>
    public class SWeChatLoginModel : BaseSubmitModel
    {
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 手机验证码
        /// </summary>
        public string PhoneCode { get; set; }
        /// <summary>
        /// IP地址
        /// </summary>
        public string Ip { get; set; }
    }

    public class SSubmitCreditProject : BaseSubmitModel
    {
        /// <summary>
        /// 标的id
        /// </summary>
        public int LoanId { get; set; }
    }


    public class SGuaranteeLoans:BaseSubmitModel
    {
        /// <summary>
        /// 项目编号
        /// </summary>
        public string LoanNo { get; set; }
        /// <summary>
        /// 借款人/借款企业姓名
        /// </summary>
        public string Loaner { get; set; }
    }
}