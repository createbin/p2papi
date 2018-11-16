using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.ApiModels.SubmitModels
{
    #region 结算中心用户新增

    public class SJsRegisterModel : BaseSubmitModel
    {
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdCard { get; set; }

        /// <summary>
        /// 用户特性 1：投资户 2：融资户 3：投融资一体户 4：企业户 5 担保户
        /// </summary>
        public string BusinessProperty { get; set; }
    }

    public class SJsCoRegisterModel : BaseSubmitModel
    {
        /// <summary>
        /// 企业名称
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 法人代表姓名
        /// </summary>
        public string CorperationName { get; set; }

        /// <summary>
        /// 法人证件号
        /// </summary>
        public string CorperationIdCard { get; set; }

        /// <summary>
        /// 开户名称
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// 是否为但保户
        /// 1：不是 2：是
        /// </summary>
        public int IsGuarantee { get; set; } = 1;

        public string ContactUser { get; set; }

        public string ContactPhone { get; set; }
        /// <summary>
        /// 三证号或者组织构代码
        /// </summary>
        public string IdCard { get; set; }
        /// <summary>
        /// 营业执照bianhao
        /// </summary>
        public string LicenseCode { get; set; }
        /// <summary>
        /// 税务登记号
        /// </summary>
        public string TaxId { get; set; }
    }

    #endregion 结算中心用户新增

    #region 结算中心大额充值户

    public class SJsOpenChargeModel : BaseSubmitModel
    {
        /// <summary>
        /// 行外对公账号
        /// </summary>
        public string AccountNo { get; set; }
        /// <summary>
        /// 清算行号
        /// </summary>
        public string AccountBk { get; set; }
        /// <summary>
        /// 对公户名
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// 1：新增 2：修改
        /// </summary>
        public string Type { get; set; } = "1";
    }

    public class SJsChargeRecord : BaseSubmitModel
    {
        /// <summary>
        /// 1-历史记录查询 2-当前记录查询
        /// </summary>
        public string QueryType { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        public string StartDate { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public string EndDate { get; set; }
        /// <summary>
        /// queryTyp-2 必输
        ///存管平台返回两个自然日内此流水号至
        ///最新一笔流水间所有记录（若无历史流
        ///水送 0，则返回两天内所有记录
        /// </summary>
        public string TransId { get; set; }
        /// <summary>
        /// 当前查询页数（首次查询送 1）
        /// </summary>
        public string PageNo { get; set; }
    }

    #endregion

    public class SBhRealNameModel : BaseSubmitModel
    {
        /// <summary>
        /// 银行卡代号
        /// </summary>
        public string BankCode { get; set; }

        /// <summary>
        /// 银行卡号码
        /// </summary>
        public string BankCodeNo { get; set; }
    }

    public class SBhBindMobile : BaseSubmitModel
    {
        public string NewPhone { get; set; }

        public string VerCode { get; set; }
    }

    /// <summary>
    /// 账户充值
    /// </summary>
    public class SBHAccountRecharge : BaseSubmitModel
    {
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Money { get; set; }

        /// <summary>
        /// 用户属性
        /// </summary>
        public int UserAttribute { get; set; }
    }

    /// <summary>
    /// 账户提现
    /// </summary>
    public class SBHAccountWithdraw : BaseSubmitModel
    {
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Money { get; set; }

        /// <summary>
        /// 用户属性
        /// </summary>
        public int UserAttribute { get; set; }
    }

    /// <summary>
    /// 融资转账
    /// </summary>
    public class SFinanceTransfer : BaseSubmitModel
    {
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Money { get; set; }
    }

    /// <summary>
    /// 用户交易记录
    /// </summary>
    public class SAccountTrading : BasePageModel
    {
        /// <summary>
        /// 交易类型
        /// 0 全部
        /// 1 非充值提现
        /// 2 充值提现
        /// 3 充值
        /// 4 提现
        /// 5 项目回款
        /// 6 投资
        /// 7 红包
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? Max { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? Min { get; set; }
    }


    public class SBoHaiBindPass : BaseSubmitModel
    {
        /// <summary>
        /// 1修改支付密码,0找回支付密码
        /// </summary>
        public string OperationType { get; set; }
    }



    /// <summary>
    /// 用户账户查询
    /// </summary>
    public class SJsQueryChargeAccount : BaseSubmitModel
    {
        /// <summary>
        /// 用户属性
        /// </summary>
        public int UserAttribute { get; set; }
    }
    /// <summary>
    /// 用户账户查询
    /// </summary>
    public class SIsAuth : BaseSubmitModel
    {
        /// <summary>
        /// 授权类型1：投资2：缴费 3：还款
        /// </summary>
        public int AuthType { get; set; }
    }

    /// <summary>
    /// 用户账户查询
    /// </summary>
    public class SAuthType : BaseSubmitModel
    {
        /// <summary>
        /// 授权类型1：授权2：解授权
        /// </summary>
        public string AuthType { get; set; } = "1";
    }

    public class SUserType : BaseSubmitModel
    {
        /// <summary>
        /// 0:投资户 1:融资户
        /// </summary>
        public string UserType { get; set; } = "1";
    }


    public class SOfflineRecharge : BaseSubmitModel
    {
        public DateTime SDate { get; set; }

        public DateTime EDate { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// 总页数 page不为1 时返回
        /// </summary>
        public int PageTotal { get; set; }
        /// <summary>
        /// 总页数 page不为1 时返回
        /// </summary>
        public int Total { get; set; }
    }


    public class SVerifyCompanyInfo : BaseSubmitModel
    {
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

    /// <summary>
    /// 销户
    /// </summary>
    public class ColseAccount : BaseSubmitModel
    {
        /// <summary>
        /// 存管平台客户号
        /// </summary>
        public string PlatformUid { get; set; }
    }

    #region 商户控台传递的用户平台编号

    public class SMerchatUsers : BaseSubmitModel
    {
        public string UserPlaCustIds { get; set; }
    }

    #endregion
}