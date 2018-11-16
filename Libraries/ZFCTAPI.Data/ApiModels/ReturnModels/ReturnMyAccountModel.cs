using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZFCTAPI.Data.ApiModels.ReturnModels
{
    /// <summary>
    /// PC融资户统计
    /// </summary>
    public class RPCAccountStatistics
    {
        /// <summary>
        /// 总资产（不包括冻结金额）
        /// </summary>
        public decimal AccountMoney { get; set; }

        /// <summary>
        /// 累计收益
        /// </summary>
        public decimal CumulativeIncome { get; set; }

        /// <summary>
        /// 待收本金
        /// </summary>
        public decimal WaitReceivePrincipal { get; set; }

        /// <summary>
        /// 今日待收
        /// </summary>
        public decimal TodayWaitReceive { get; set; }

        /// <summary>
        /// 投资数量
        /// </summary>
        public int InvestCount { get; set; }

        /// <summary>
        /// 投资金额
        /// </summary>
        public decimal InvestMoney { get; set; }

        /// <summary>
        /// 投标中投资数量
        /// </summary>
        public decimal InvestBiddingCount { get; set; }

        /// <summary>
        /// 还款中投资数量
        /// </summary>
        public decimal InvesRepayCount { get; set; }

        /// <summary>
        /// 已结清投资数量
        /// </summary>
        public decimal InvesSettledtCount { get; set; }

        /// <summary>
        /// 可转让债权数量
        /// </summary>
        public int TransferCanCount { get; set; }

        /// <summary>
        /// 已转入债权
        /// </summary>
        public int TransferInCount { get; set; }

        /// <summary>
        /// 转出中债权
        /// </summary>
        public int TransferWaitCount { get; set; }

        /// <summary>
        /// 已转出债权
        /// </summary>
        public int TransferOutCount { get; set; }

        /// <summary>
        /// 可用红包金额
        /// </summary>
        public decimal RedMoney { get; set; }

        /// <summary>
        /// 是否借款
        /// </summary>
        public bool HasLoan { get; set; }

        /// <summary>
        /// 是否开户
        /// </summary>
        public bool IsOpenAccount { get; set; }

        /// <summary>
        /// 是否开户
        /// </summary>
        public bool IsOldAccount { get; set; }
    }

    /// <summary>
    /// 融资账户统计
    /// </summary>
    public class RFinancingAccount
    {
        /// <summary>
        /// 下期还款日期
        /// </summary>
        public string NextPayDate { get; set; } = "无";

        /// <summary>
        /// 下期还款总金额
        /// </summary>
        public decimal NextPayMoney { get; set; }

        /// <summary>
        /// 代还总额
        /// </summary>
        public decimal WaitPayAllMoney { get; set; }

        /// <summary>
        /// 剩余天数
        /// </summary>
        public int SurplusDays { get; set; }

        /// <summary>
        /// 满标中已结清数量
        /// </summary>
        public int FullCount { get; set; }

        /// <summary>
        /// 还款中数量
        /// </summary>
        public int RepaymentCount { get; set; }

        /// <summary>
        /// 已结清数量
        /// </summary>
        public int ClearedCount { get; set; }

        /// <summary>
        /// 募集中数量
        /// </summary>
        public int BiddingCount { get; set; }

        /// <summary>
        /// 借款总数
        /// </summary>
        public int LoanCount { get; set; }

        /// <summary>
        /// 借款总金额
        /// </summary>
        public decimal LoanMoney { get; set; }

    }

    /// <summary>
    /// APP账户统计
    /// </summary>
    public class RAPPAccountStatistics
    {
        /// <summary>
        /// 总资产（不包括冻结金额）
        /// </summary>
        public decimal AccountMoney { get; set; }

        /// <summary>
        /// 累计收益
        /// </summary>
        public decimal CumulativeIncome { get; set; }

        /// <summary>
        /// 待收收益
        /// </summary>
        public decimal WaitReceiveIncome { get; set; }

        /// <summary>
        /// 待收罚息
        /// </summary>
        public decimal WaitReceiveOverRate { get; set; }

        /// <summary>
        /// 红包数量
        /// </summary>
        public int RedCount { get; set; }

        /// <summary>
        /// 最近还款日期
        /// </summary>
        public string NextRePayDay { get; set; }

        /// <summary>
        /// 邀请奖励
        /// </summary>
        public decimal Reward { get; set; }

        /// <summary>
        /// 是否有投资
        /// </summary>
        public bool HasLoan { get; set; }

        /// <summary>
        /// 是否开户
        /// </summary>
        public bool IsOpenAccount { get; set; }

        /// <summary>
        /// 是否老用户
        /// </summary>
        public bool IsOldAccount { get; set; }
    }

    /// <summary>
    /// 我的业务统计
    /// </summary>
    public class RBusinessStatistics {
        /// <summary>
        /// 投资总数
        /// </summary>
        public int InvestCount { get; set; }

        /// <summary>
        /// 借款总数
        /// </summary>
        public int LoanCount { get; set; }

        /// <summary>
        /// 债权转让总数
        /// </summary>
        public int TransferCount { get; set; }

        /// <summary>
        /// 可用红包总数
        /// </summary>
        public int CanUserRedCount { get; set; }
    }

    /// <summary>
    /// 用户借款数量统计
    /// </summary>
    public class RLoanCountStatistics
    {
        /// <summary>
        /// 投标中
        /// </summary>
        public int BidingCount { get; set; }

        /// <summary>
        /// 满标中
        /// </summary>
        public int FullCount { get; set; }

        /// <summary>
        /// 已结清
        /// </summary>
        public int SettledCount { get; set; }

        /// <summary>
        /// 待还款
        /// </summary>
        public int RepaymentCount { get; set; }
    }

    /// <summary>
    /// 用户投资数量统计
    /// </summary>
    public class RInvestCountStatistics
    {
        /// <summary>
        /// 投标中
        /// </summary>
        public int BiddingCount { get; set; }

        /// <summary>
        /// 已结清
        /// </summary>
        public int ClearedCount { get; set; }

        /// <summary>
        /// 还款中
        /// </summary>
        public int RepaymentCount { get; set; }
    }

    /// <summary>
    /// 用户红包数量统计
    /// </summary>
    public class RRedEnvelopesCountStatistics
    {
        /// <summary>
        /// 已过期
        /// </summary>
        public int ExprisedCount { get; set; }

        /// <summary>
        /// 已使用
        /// </summary>
        public int UsedCount { get; set; }

        /// <summary>
        /// 待使用
        /// </summary>
        public int WaitUseCount { get; set; }
    }

    /// <summary>
    /// 用户债权数量统计
    /// </summary>
    public class RTransferCountStatistics
    {
        /// <summary>
        /// 可转让
        /// </summary>
        public int CantransCount { get; set; }

        /// <summary>
        /// 已转让
        /// </summary>
        public int TransferedCount { get; set; }

        /// <summary>
        /// 转让中
        /// </summary>
        public int TransferingCount { get; set; }

        /// <summary>
        /// 已结清
        /// </summary>
        public int TransInvestCount { get; set; }
    }

    /// <summary>
    /// 用户业务数量统计
    /// </summary>
    public class RBusinessCountStatistics
    {
        /// <summary>
        /// 可用红包
        /// </summary>
        public int CanUseRedCount { get; set; }

        /// <summary>
        /// 借款总数量
        /// </summary>
        public int LoanCount { get; set; }

        /// <summary>
        /// 投资总数量
        /// </summary>
        public int InvestCount { get; set; }

        /// <summary>
        /// 债权总数量
        /// </summary>
        public int TransferCount { get; set; }
    }

    /// <summary>
    /// 渤海账户信息
    /// </summary>
    public class RBHAccountInfo
    {
        /// <summary>
        /// 可用余额
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 可提现金额
        /// </summary>
        public decimal WithdrawAmount { get; set; }

        /// <summary>
        /// 冻结金额
        /// </summary>
        public decimal FreezeAmout { get; set; }
    }

    /// <summary>
    /// 用户借款账户统计
    /// </summary>
    public class RLoanAccountStatistics
    {
        /// <summary>
        /// 已还本金
        /// </summary>
        public decimal RepayPrincipal { get; set; }

        /// <summary>
        /// 已还利息
        /// </summary>
        public decimal RepayRate { get; set; }

        /// <summary>
        /// 已还服务费
        /// </summary>
        public decimal RepayServiceFee { get; set; }
    }

    /// <summary>
    /// 用户投资账户统计
    /// </summary>
    public class RInvestAccountStatistics
    {
        /// <summary>
        /// 累计投资
        /// </summary>
        public decimal InvestMoney { get; set; }

        /// <summary>
        /// 待收本金
        /// </summary>
        public decimal WaitRepayPrincipal { get; set; }

        /// <summary>
        /// 待收收益
        /// </summary>
        public decimal WaitRepayEarnings { get; set; }

        /// <summary>
        /// 已收本金
        /// </summary>
        public decimal RepayPrincipal { get; set; }

        /// <summary>
        /// 已收收益
        /// </summary>
        public decimal RepayEarnings { get; set; }
    }

    /// <summary>
    /// 用户渤海开户信息
    /// </summary>
    public class RRealInfo
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
        /// 银行卡代号
        /// </summary>
        public string BankCode { get; set; }

        /// <summary>
        /// 银行卡号码
        /// </summary>
        public string BankCardNo { get; set; }

        /// <summary>
        /// 性别 1：男 2：女
        /// </summary>
        public string Sex { get; set; }

        /// <summary>
        /// 是否结算中心开户 0否 1是
        /// </summary>
        public string Jiesuan { get; set; }

        /// <summary>
        /// 是否渤海开户 0否 1是
        /// </summary>
        public string Bohai { get; set; }
    }

    /// <summary>
    /// 用户交易记录
    /// </summary>
    public class RTradingModel
    {
        //主键id
        public int id;

        //交易类型
        public string TradingType { get; set; }

        //交易金额
        public decimal TradingMoney { get; set; }

        //交易时间
        public string TradingDate { get; set; }

        //交易状态
        public string TradingStatus { get; set; }

        //交易流水号
        public string TradingOrderNo { get; set; }

        //项目名称
        public string TradingName { get; set; }

        //交易后账户余额
        public decimal TrandingAccountMoney { get; set; }

        //交易后融资账户余额
        public decimal FTrandingAccountMoney { get; set; }
        /// <summary>
        /// 手续费金额
        /// </summary>
        public string FeeAmt { get; set; }
        /// <summary>
        /// 失败原因
        /// </summary>
        public string FailedReson { get; set; }
    }

    /// <summary>
    /// 投资收益
    /// </summary>
    public class RInvestEarnings
    {
        /// <summary>
        /// 用户真实姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 30天收益
        /// </summary>
        public decimal ThridDayEarnings { get; set; }

        /// <summary>
        /// 累计收益
        /// </summary>
        public decimal AccumulativeEarnings { get; set; }
    }

    /// <summary>
    /// 用户银行卡信息
    /// </summary>
    public class RUserBankInfo
    {
        /// <summary>
        /// 用户是否结算开户
        /// </summary>
        public bool IsJieSuan { get; set; } = false;

        /// <summary>
        /// 用户是否渤海开户
        /// </summary>
        public bool IsBoHai { get; set; } = false;

        /// <summary>
        /// 银行卡信息
        /// </summary>
        public IEnumerable<RBankInfo> BankInfos { get; set; }
    }

    /// <summary>
    /// 银行卡信息
    /// </summary>
    public class RBankInfo {
        /// <summary>
        /// 卡号
        /// </summary>
        public string CardNumber { get; set; }

        /// <summary>
        /// 银行代号
        /// </summary>
        public string BankCode { get; set; }

        /// <summary>
        /// 银行图片链接 
        /// </summary>
        public string BankUrl { get; set; }

        /// <summary>
        /// 银行名称
        /// </summary>
        public string BankName { get; set; }

        public string BankBack { get; set; }
    }
    /// <summary>
    /// 用户第三方认证信息
    /// </summary>
    public class RUserThirdPartInfo
    {
        public string RealName { get; set; }="";

        public string IdCard { get; set; }="";

        public string PhoneNo { get; set; } = "";

        public string BankCode { get; set; } = "";

        public string BankName { get; set; } = "";

        public string BankNo { get; set; } = "";

        /// <summary>
        /// 0 失败 1成功
        /// </summary>
        public string JieSuan { get; set; } = "0";

        /// <summary>
        /// 
        /// </summary>
        public string BoHai { get; set; } = "0";

        /// <summary>
        /// 结算返回码
        /// </summary>
        public string JieSuanCode { get; set; } = "";

        /// <summary>
        /// 结算返回信息
        /// </summary>
        public string JieSuanMsg { get; set; } = "";
        /// <summary>
        /// 渤海返回碼
        /// </summary>
        public string BoHaiCode { get; set; } = "";
        /// <summary>
        /// 渤海返回信息
        /// </summary>
        public string BohaiMsg { get; set; } = "";
        /// <summary>
        /// 是否有曾经绑卡的操作
        /// </summary>
        public string OnceBohai { get; set; } = "0";
        /// <summary>
        /// 是否又曾经开户操作
        /// </summary>
        public string OnceJieSuan { get; set; } = "0";

        /// <summary>
        /// 是否授权
        /// </summary>
        public string IsAuth { get; set; } = "0";
        /// <summary>
        /// 渤海绑定的手机号
        /// </summary>
        public string AccountPhone { get; set; } = "";
        /// <summary>
        /// 账户类型 1=投资户；2=融资户
        /// </summary>
        public int? BusinessProperty { get; set; }
    }
    /// <summary>
    /// 返回用户授权信息
    /// </summary>
    public class RUserAuthInfo
    {
        public string IsAuth { get; set; } = "0";
        public List<RAuthInfo> AuthInfos { get; set; }=new List<RAuthInfo>();
    }
    /// <summary>
    /// 
    /// </summary>
    public class RAuthInfo
    {
        public string AuthType { get; set; }

        public string AuthCode { get; set; }
        /// <summary>
        /// 0:未授权 1：已授权 2：已过期
        /// </summary>
        public string AuthState { get; set; }

        public string AuthStart { get; set; }

        public string AuthEnd { get; set; }
        /// <summary>
        /// 授权金额
        /// </summary>
        public string AuthMoney { get; set;}
    }

    public class RVerifyInfo
    {
        public string Type { get; set; }

        public bool IsExit { get; set; }
    }

    public class RUserInfos
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 用户账户号
        /// </summary>
        public string PlanCustId { get; set; }
        /// <summary>
        /// 用户客户名
        /// </summary>
        public string PlanCustName { get; set; }
        /// <summary>
        /// 用户手机号
        /// </summary>
        public string PlanCustPhone { get; set; }
        /// <summary>
        /// 投资id
        /// </summary>
        public string InvestId { get; set; }
    }

    public class RUserAuth
    {
        public string IsAuth { get; set; }
        /// <summary>
        /// 授权金额
        /// </summary>
        public string AuthMoney { get; set; }
    }
}