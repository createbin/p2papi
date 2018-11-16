using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.ApiModels.SubmitModels
{
    /// <summary>
    /// 注册实体
    /// </summary>
    public class SRegisterModel : BaseSubmitModel
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 邀请码
        /// </summary>
        public string RecommendCode { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string VerCode { set; get; }
    }

    /// <summary>
    /// 登录实体
    /// </summary>
    public class SLoginModel : BaseSubmitModel
    {

        //用户名
        public string UserName { get; set; }

        // 密码
        public string Password { get; set; }

        /// <summary>
        /// 机型描述
        /// </summary>
        public string PhoneInfo { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public string VersionNum { get; set; }

        /// <summary>
        /// 企业用户
        /// </summary>
        public bool IsCompanyLogin { get; set; }
    }

    /// <summary>
    /// 更改手机号实体
    /// </summary>
    public class SChangePhoneModel : BaseSubmitModel
    {
        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string VerCode { get; set; }
    }

    /// <summary>
    /// 更改邮箱实体
    /// </summary>
    public class SChangeEmailModel : BaseSubmitModel
    {
        /// <summary>
        /// 邮箱
        /// </summary>
        public string EmailNumber { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string VerCode { get; set; }
    }

    /// <summary>
    /// 更改登陆密码实体
    /// </summary>
    public class SChangeLoginPasswordModel : BaseSubmitModel
    {
        /// <summary>
        /// 新密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 老密码
        /// </summary>
        public string OldPassword { get; set; }
    }

    public class ValidateInfo : BaseSubmitModel
    {
        public string Validate { get; set; }
    }

    /// <summary>
    /// 忘记密码
    /// </summary>
    public class ForgetPwd : BaseSubmitModel
    {
        //手机号码
        public string Phone { get; set; }
        //验证码
        public string VerCode { get; set; }
        //修改后用户密码
        public string Password { get; set; }
    }

    public class CompanyForgetPwd: BaseSubmitModel
    {
        //手机号码
        public string PhoneNumber { get; set; }
        //验证码
        public string VerCode { get; set; }

        /// <summary>
        /// true:三证合一,
        /// </summary>
        public bool VerifyType { get; set; }
        /// <summary>
        /// 组织机构代码
        /// </summary>
        public string InstuCode { get; set; }

        /// <summary>
        /// 营业执照编号
        /// </summary>
        public string BusiCode { get; set; }

        /// <summary>
        /// 税务登记号
        /// </summary>
        public string TaxCode { get; set; }
    }

    public class SCompanyRegisterModel:SRegisterModel
    {
        public bool IsOne { get; set; }
        /// <summary>
        /// 组织机构代码
        /// </summary>
        public string OrganizationCode { get; set; }
        /// <summary>
        /// 税务登记号
        /// </summary>
        public string TaxCode { get; set; }
        /// <summary>
        /// 营业执照编号
        /// </summary>
        public string BusinessLicense { get; set; }
        /// <summary>
        /// 统一社会信用代码
        /// </summary>
        public string SocialCredit { get; set; }
        /// <summary>
        /// 联系人姓名
        /// </summary>
        public string ContactUser { get; set; }
    }

    /// <summary>
    /// 投资合同
    /// </summary>
    public class SDownInvestContract : BaseSubmitModel
    {
        /// <summary>
        /// 项目编号
        /// </summary>
        public int InvestId { get; set; }
    }

    /// <summary>
    /// 借款合同
    /// </summary>
    public class SDownLoanContract : BaseSubmitModel
    {
        /// <summary>
        /// 项目编号
        /// </summary>
        public int LoanId { get; set; }
    }
}