using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Core.Enums
{
    public class JSReturnCode
    {
        public static string Success = "00000000";
        public static string Fail = "11111111";
    }

    public class BHReturnCode
    {
        public static string Success = "000000";
    }

    public class IdentityType
    {
        public static string Other = "00";
        public static string IdCard = "01";
        public static string Passport = "02";
        public static string MilitaryLicense = "03";
        public static string OrganizationCodeNumber = "04";
        public static string ThreeCardNumber = "05";
    }

    /// <summary>
    /// 项目类型
    /// </summary>
    public class ProjectType
    {
        /// <summary>
        /// 债权
        /// </summary>
        public static string Creditor = "01";

        /// <summary>
        /// 股权
        /// </summary>
        public static string Stock = "02";

        /// <summary>
        /// 票据
        /// </summary>
        public static string Bill = "03";
    }

    /// <summary>
    /// 贷款类别
    /// </summary>
    public enum BHLoanType
    {
        /// <summary>
        /// 信用贷款/固收理财
        /// </summary>
        Credit = 1,

        /// <summary>
        /// 担保贷款
        /// </summary>
        Guarantee = 2,

        /// <summary>
        /// 抵押贷款
        /// </summary>
        Pledge = 3,

        /// <summary>
        /// 混合贷款
        /// </summary>
        Mixture = 4
    }

    public enum UserOpenType
    {
        /// <summary>
        /// 新用户
        /// </summary>
        NewUser = 1,

        /// <summary>
        /// 老用户平台绑定
        /// </summary>
        OldUser = 2,

        /// <summary>
        /// 老用户（只有证件信息）绑定
        /// </summary>
        OldUserCard = 3
    }

    public enum UploadFileType
    {
        /// <summary>
        /// 投标对账
        /// </summary>
        InvestChk,

        /// <summary>
        /// 充值对账
        /// </summary>
        PpdChk,

        /// <summary>
        /// 提现对账
        /// </summary>
        WdcCHk,

        /// <summary>
        /// 红包对账
        /// </summary>
        RedCHK,

        /// <summary>
        /// 存量用户注册
        /// </summary>
        StockUserRegisterCHK,
        
        /// <summary>
        /// 存量标的迁移
        /// </summary>
        StockLoanTransferCHK
    }

    /// <summary>
    /// 结算中心接口名
    /// </summary>
    public enum InterfaceName
    {
        //新增、修改项目
        p_submitCreditProject,

        //申购
        p_claimsPurchase,

        //修改项目起息日
        p_updateProject,

        //募集结果上报
        p_raiseResultNoticeEx,

        //还款充值
        p_PayBackRecharge,

        //还款明细
        p_repaymentExecuteEx,

        //转让成交
        p_claimsTransferDeal,

        //商户账户充值
        p_MercRecharge,

        //实时红包
        p_experBonus,

        //统一撤销
        p_revoke,

        //用户新增
        p_UserAddBH,

        //用户注销
        p_UserCancel,

        //红包转账
        p_CampaignTransfer,

        //余额查询
        p_AccountQueryAccBalance,

        //动态口令申请
        p_sendUapMsg,

        //用户信息查询
        p_QueryUserInf,

        //商户账户查询
        p_QueryMerchantAccts,

        //交易状态查询
        p_QueryTransStat,

        //商户账户交易查询
        p_QueryMerchantTrans,

        //开设大额充值户
        p_OpenChargeAccount,

        //大额充值账号查询
        p_QueryChargeAccount,

        //大额充值记录查询
        p_QueryChargeDetail,

        //用户注册绑卡
        p_RealNameWeb,

        //修改绑定银行卡
        p_BindCardWeb,

        //修改手机号
        p_BindMobileNo,

        //修改、找回支付密码
        p_BindPass,

        //用户充值
        p_WebRecharge,

        //用户提现
        p_Drawings,

        //融资账户转账
        p_FinanceTransfer,

        //批量申购撤销
        p_BatInvestCancle,

        p_QueryChargeAccountResult,

        //线下充值同步
        p_RechargeSyn,
        //授权信息查询
        p_QueryAuthInf,
        /// <summary>
        /// 授权
        /// </summary>
        p_AutoInvestAuth,
        /// <summary>
        /// 投资合同上传
        /// </summary>
        p_ContractFileUpload,
        /// <summary>
        /// 商户账户提现
        /// </summary>
        p_MercWithdraw,
        /// <summary>
        /// 存量用户迁移
        /// </summary>
        p_ExistUserTransfer,
        /// <summary>
        /// 存量标的迁移
        /// </summary>
        p_ExistLoanTransfer,

        /// <summary>
        /// 销户
        /// </summary>
        p_CloseAccount
    }

    /// <summary>
    /// 渤海银行返回交易状态
    /// </summary>
    public enum TransState
    {
        /// <summary>
        /// 交易成功 已清分
        /// </summary>
        S1,

        /// <summary>
        /// 交易失败,未清分
        /// </summary>
        F1,

        /// <summary>
        /// 请求处理中
        /// </summary>
        W2,

        /// <summary>
        /// 系统受理中
        /// </summary>
        W3,

        /// <summary>
        /// 银行受理中
        /// </summary>
        W4,

        /// <summary>
        /// 撤标解冻成功
        /// </summary>
        S2,

        /// <summary>
        /// 放款解冻成功
        /// </summary>
        S3,

        /// <summary>
        /// 部分成功,部分冻结
        /// </summary>
        B1,

        /// <summary>
        /// 审批拒绝
        /// </summary>
        R9,

        /// <summary>
        /// 撤标解冻失败
        /// </summary>
        F2
    }

    public class ActType
    {
        public static string All = "000";
        public static string Fee = "800";
        public static string Marketing = "810";
        public static string Prepaid = "820";
        public static string Cash = "830";
        public static string Credit = "840";
    }

    public class AccountType
    {
        public static string Marketing = "2";
        public static string Prepaid = "3";
    }

    public enum QueryTransType
    {
        //充值
        WebRecharge,
        //提现
        Drawings,
        //投标
        BackInvest,
        /// <summary>
        ///投标撤销
        /// </summary>
        InvestCancle,
        /// <summary>
        /// 放款
        /// </summary>
        FileRelease,
        /// <summary>
        /// 还款
        /// </summary>
        FileRepayment,
        /// <summary>
        /// 转让
        /// </summary>
        CreditRightsChange,
        /// <summary>
        /// 实时红包
        /// </summary>
        ExperBonus
    }

    public enum AsyncNotice
    {
        RealNameWebResult,

        DrawingsResult,

        WebRechargeResult,

        BindCardWebResult,

        BindMobileNoResult,

        BindPassResult,

        FileRelease,

        FileRepayment
    }

    public enum MobileAddressType
    {
        /// <summary>
        /// 注册绑卡
        /// </summary>
        CBHBNetLoanRegister,

        /// <summary>
        /// 提现
        /// </summary>
        CBHBNetLoanWithdraw,

        /// <summary>
        /// 充值
        /// </summary>
        CBHBNetLoanRecharge,

        /// <summary>
        /// 修改绑定银行卡
        /// </summary>
        CBHBNetLoanBindCardMessage,

        /// <summary>
        /// 修改手机号
        /// </summary>
        CBHBNetLoanUpdatePhone,

        /// <summary>
        /// 个人修改密码
        /// </summary>
        CBHBNetLoanUpdatePassword,
        /// <summary>
        /// 个人找回密码
        /// </summary>
        CBHBNetLoanGetPassword,

        /// <summary>
        /// 对公修改密码
        /// </summary>
        CBHBNetLoanUpdatePwdPublic,

        /// <summary>
        /// 对公找回密码
        /// </summary>
        CBHBNetLoanGetPwdPublic,

        /// <summary>
        /// 开设对公账户
        /// </summary>
        CBHBNetLoanRegisterPublic,
        /// <summary>
        /// 授权
        /// </summary>
        CBHBNetLoanAuthTrs
    }

    public class ClientType
    {
        public static string PC = "1";
        public static string WEB = "2";
    }

    public class TxnType
    {
        public static string Add = "1";
        public static string Update = "2";
    }

    public class AccountTyp
    {
        public static string CommonUser = "1";
        public static string GuaranteeUser = "2";
    }


    public enum AuthTyp
    {
        /// <summary>
        /// 投资授权
        /// </summary>
        Invest,
        /// <summary>
        /// 缴费授权
        /// </summary>
        Payment,
        /// <summary>
        /// 还款授权
        /// </summary>
        Repayment
    }
}