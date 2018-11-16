using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Core.Enums
{
    /// <summary>
    /// 返回码
    /// </summary>
    public enum ReturnCode
    {
        Success = 200,//成功
        DataFormatError = 201,//数据格式错误
        SignatureFailure = 203,//签名失败
        LoginError = 204,//登录失败 用户名或密码错误
        SendMess = 205,//验证码发送失败
        DataEorr = 206,//提交数据错误
        RegisterEorr = 207, //注册失败
        SelectEorr = 208, //搜索失败
        UnOpenAccount = 209,//没有开户
        NoDate = 210,//未到指定日期
        AmountTooLarge = 211,//投资金额过大
        TokenEorr = 212,//验Token失败
        InvestMyLoan = 213,//自己项目不可以投资
        AlreadyInvest = 214,//改项目已经投资不能重复投资
        BalanceEorr = 215,//余额不足 不能投资
        InvestEorr = 216, //投资失败
        EnableRed = 217,//红包为开启
        QueryEorr = 218,//查询失败
        ApplyMore = 219,//达到融资上限
        Processing = 220,//处理中
        RechargeEorr = 221,//充值失败
        WithdrawalEorr = 222,//提现失败
        NotFoundBank = 223,//获取银行卡信息失败
        BankCardAlreadyExists = 224,//银行卡已存在
        BindBankCardEorr = 225,//绑定银行卡失败
        ReadyOpenAccount = 226,//已经开户无需开户
        UnCompletelyData = 227,//资料不全 请补全资料
        UnRealName = 228,//没有实名认证
        UnPhone = 229,//没有认证手机
        UnEmail = 230,//没有认证邮箱
        AlreadyRealName = 231,//已经实名认证
        RealNameProcessing = 232,//实名认证处理中
        EmailIsHave = 233,//邮箱已存在
        AlreadyCheckEmail = 234,//邮箱已经认证 
        ChangePasswordEorr = 235,//密码修改失败
        CheckCodeEorr = 236,//验证码输入错误
        UnBankcard = 237,//没有银行卡
        Exceptions = 238,//程序异常
        NotExamine = 239, //项目未审核
        OtherExceptions = 240,//第三方接口异常
        OpenAccounted = 241, //开户过
        OverMaxproerid = 242, //超过期限
        OverMinProerid = 243,//低于期限
        TransferMoneyErroer = 244, //转让金额填写错误
        TransferFaild = 245,
        InvestTradePwd = 246,//交易密码错误
        TimestampError = 247,//时间戳过期        
        NoBohaiData = 248,//时间戳过期
        SystemUpgrade = 999 //系统升级维护
    }
}
