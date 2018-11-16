using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Core.Enums
{
    /// <summary>
    /// 使用类型
    /// </summary>
    public enum GebruiksType
    {
        /// <summary>
        /// 不限制
        /// </summary>
        Unlimited,

        /// <summary>
        /// 现金
        /// </summary>
        Cash,

        /// <summary>
        /// 投资
        /// </summary>
        Invest,

        /// <summary>
        /// 还款
        /// </summary>
        Repayment,

        /// <summary>
        /// P2P3.0API接口
        /// </summary>
        Api3
    }

    /// <summary>
    /// 发放类型
    /// </summary>
    public enum GrantType
    {
        /// <summary>
        /// 注册用户
        /// </summary>
        RegisteredUsers,

        /// <summary>
        /// 手机认证
        /// </summary>
        PhoneCertification,

        /// <summary>
        /// 实名认证
        /// </summary>
        RealNameCertification,

        /// <summary>
        /// 绑定银行卡
        /// </summary>
        BindingBankCard,

        /// <summary>
        /// 会员升级
        /// </summary>
        MemberUpgrade,

        /// <summary>
        /// 充值
        /// </summary>
        Recharge,

        /// <summary>
        /// 首次充值
        /// </summary>
        FirstRecharge,

        /// <summary>
        /// 投资
        /// </summary>
        Investment,

        /// <summary>
        /// 首次投资
        /// </summary>
        FirstInvestment,

        /// <summary>
        /// 邀请好友
        /// </summary>
        Invite,

        /// <summary>
        /// 会员生日
        /// </summary>
        MemberBirthday,

        /// <summary>
        /// 借款
        /// </summary>
        Borrowing,

        /// <summary>
        /// 首次借款
        /// </summary>
        FirstBorrowing,

        /// <summary>
        /// 还款
        /// </summary>
        Repayment,

        /// <summary>
        /// 首次还款
        /// </summary>
        FirstRepayment,

        /// <summary>
        /// 提前还款
        /// </summary>
        Prepayment,

        /// <summary>
        /// 产品
        /// </summary>
        Product,

        /// <summary>
        /// 积分兑换
        /// </summary>
        IntegralExchange,

        /// <summary>
        /// 签到
        /// </summary>
        CheckIn,

        /// <summary>
        /// 转盘抽奖 一等奖
        /// </summary>
        FirstPrize,

        /// <summary>
        /// 转盘抽奖 二等奖
        /// </summary>
        SecondPrize,

        /// <summary>
        /// 转盘抽奖 三等奖
        /// </summary>
        ThirdPrize,

        /// <summary>
        /// 答题
        /// </summary>
        Answer
    }

    public enum POPRelation
    {
        MoreThan,
        LessThan,
        Equal
    }

    public enum POPValueType
    {
        /// <summary>
        /// 固定值
        /// </summary>
        FixedValue,

        /// <summary>
        /// 随机值
        /// </summary>
        RadomValue,

        /// <summary>
        /// 阶梯值
        /// </summary>
        StepValue,
    }

    public enum RuleName
    {
        /// <summary>
        /// 额度
        /// </summary>
        Limit,

        /// <summary>
        /// 排名
        /// </summary>
        Ranking,
    }

    /// <summary>
    /// 红包类型
    /// </summary>
    public enum RedEnvelopeType
    {
        /// <summary>
        /// 定向
        /// </summary>
        Orientation,

        /// <summary>
        /// 系统
        /// </summary>
        System,

        /// <summary>
        /// 产品
        /// </summary>
        Product
    }

    /// <summary>
    /// 红包领取后有效期
    /// </summary>
    public enum RedEnvelopeExpiryDate
    {
        /// <summary>
        /// 无限制
        /// </summary>
        Unlimited,

        /// <summary>
        /// 相对
        /// </summary>
        Relatively,

        /// <summary>
        /// 绝对
        /// </summary>
        Absolute
    }

    public enum RedEnvelopeCash
    {
        /// <summary>
        /// 注册
        /// </summary>
        Registered,

        /// <summary>
        /// 投资
        /// </summary>
        Investment,

        /// <summary>
        /// 投资至少多少元
        /// </summary>
        AtLeastAmount,

        /// <summary>
        /// 投资至少多少期数
        /// </summary>
        AtLeastIssue
    }

    /// <summary>
    /// 用户选择类型Or机构选择类型
    /// </summary>
    public enum CustormOrGovChoiceType
    {
        /// <summary>
        /// 所有用户
        /// </summary>
        CustormAll,

        /// <summary>
        /// 前台注册
        /// </summary>
        Registered,

        /// <summary>
        /// 前台已开户用户
        /// </summary>
        OpenAccount,

        /// <summary>
        /// 前台已开户已有单位用户
        /// </summary>
        PayCompany,

        /// <summary>
        /// 后台管理员
        /// </summary>
        Admin,

        /// <summary>
        /// 后台注册
        /// </summary>
        FrontRegistered,

        /// <summary>
        /// 后台注册不包含任何角色
        /// </summary>
        FrontRegisteredNotInclude,

        /// <summary>
        /// 所有机构
        /// </summary>
        GovAll,

        /// <summary>
        /// 小贷
        /// </summary>
        Loan,

        /// <summary>
        /// 担保
        /// </summary>
        Guar,

        /// <summary>
        /// 理财加盟商
        /// </summary>
        Financial,

        /// <summary>
        /// 小贷和担保
        /// </summary>
        LoanAndGuar,

        /// <summary>
        /// 理财加盟商（非机构）
        /// </summary>
        Alb,

        /// <summary>
        /// 加盟商员工
        /// </summary>
        AlbEmp,

        /// <summary>
        /// 理财经理
        /// </summary>
        Licai,

        /// <summary>
        /// 理财经理员工
        /// </summary>
        LicaiEmp
    }

    /// <summary>
    /// 积分扣除规则
    /// </summary>
    public enum DeductionType
    {
        /// <summary>
        /// 抽奖
        /// </summary>
        Lotery
    }

    /// <summary>
    /// 红包统计类型
    /// </summary>
    public enum RedStatisticsType
    {
        /// <summary>
        /// 红包总数量
        /// </summary>
        RedCount,

        /// <summary>
        /// 红包总额
        /// </summary>
        RedMoney,

        /// <summary>
        /// 待使用数量
        /// </summary>
        WaitUseCount,

        /// <summary>
        /// 待使用总额
        /// </summary>
        WaitUseMoney,

        /// 已使用数量
        /// </summary>
        UsedCount,

        /// <summary>
        /// 已使用总额
        /// </summary>
        UsedMoney,

        /// <summary>
        /// 已过期数量
        /// </summary>
        ExpiredCount,

        /// <summary>
        /// 已过期总额
        /// </summary>
        ExpiredMoney
    }

    public enum RedEnvelopesResult
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success=0,
        /// <summary>
        /// 商户营销账户余额不足
        /// </summary>
        InsufficientBalance=1,
        /// <summary>
        /// 实时红包错误
        /// </summary>
        ExperBonusError =2,
        /// <summary>
        /// 红包转让错误
        /// </summary>
        TransferRedError=3,
        /// <summary>
        /// 就是错误
        /// </summary>
        Error=4,
        /// <summary>
        /// 部分红包发放失败
        /// </summary>
        PartError=5
    }
}