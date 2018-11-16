using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZFCTAPI.Core.Enums
{
    public enum PasswordFormat : int
    {
        Clear = 0,
        Hashed = 1,
        Encrypted = 2
    }
    public enum UserSource
    {
        PC = 1,
        WeChat = 2,
        App = 3,
        Android = 4,
        IOS = 5
    }

    public enum OperateType
    {
        LoginOut = 0,
        LoginIn = 1
    }

    /// <summary>
    /// Represents the customer registration type fortatting enumeration
    /// </summary>
    public enum UserRegistrationType : int
    {
        /// <summary>
        /// Standard account creation
        /// </summary>
        Standard = 1,
        /// <summary>
        /// Email validation is required after registration
        /// </summary>
        EmailValidation = 2,
        /// <summary>
        /// A customer should be approved by administrator
        /// </summary>
        AdminApproval = 3,
        /// <summary>
        /// Registration is disabled
        /// </summary>
        Disabled = 4,
    }

    /// <summary>
    /// Represents the customer login result enumeration
    /// </summary>
    public enum CustomerLoginResults : int
    {
        /// <summary>
        /// Login successful
        /// </summary>
        Successful = 1,
        /// <summary>
        /// Customer dies not exist (email or username)
        /// </summary>
        CustomerNotExist = 2,
        /// <summary>
        /// Wrong password
        /// </summary>
        WrongPassword = 3,
        /// <summary>
        /// Account have not been activated
        /// </summary>
        NotActive = 4,
        /// <summary>
        /// Customer has been deleted 
        /// </summary>
        Deleted = 5,
        /// <summary>
        /// Customer not registered 
        /// </summary>
        NotRegistered = 6,
    }

    public class UserType
    {
        public static string PersonalUser = "1";
        public static string BusinessUser = "2";
    }

    public enum LoanType
    {
        NewHand=1,
        Recommand=2,
        Transfer=3
    }

    #region 意见反馈

    public enum FeedbackStateEnum
    {
        /// <summary>
        /// 未处理
        /// </summary>
        Handing = 0,
        /// <summary>
        /// 已处理
        /// </summary>
        Handed = 1
    }

    #endregion

    public enum UploadResult
    {
        Success=0,
        Fail=1
    }


    public enum UserAttributes
    {
        /// <summary>
        /// 投资户
        /// </summary>
        Invester=1,
        /// <summary>
        /// 融资户
        /// </summary>
        Financer=2,
        /// <summary>
        /// 投融资户
        /// </summary>
        AllIn=3
    }

    #region 危险交易类型
    public enum SuspiciousTransactionType
    {
        /// <summary>
        /// 大额充值
        /// </summary>
        [Description("大额充值")]
        LargeRecharge = 1,
        /// <summary>
        /// 大额提现
        /// </summary>
        [Description("大额提现")]
        LargeWithdraw = 2,
        /// <summary>
        /// 重复资金交易
        /// </summary>
        [Description("重复资金交易")]
        RepeatTrade = 3
    }
    #endregion


    public enum ReturnPageType
    {
        OpenAccount=1,
        BindCard=2,
        Recharge=3,
        Withdraw=4,
        BindMobileNo =5,
        BindPass=6,
        AutoAuth=7,
        RechargeFailed = 8,
        WithdrawFailed = 9
    }
}
