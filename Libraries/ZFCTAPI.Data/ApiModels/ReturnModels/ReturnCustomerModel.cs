using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.ApiModels.ReturnModels
{
    public class RRegisterModel
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 推荐人
        /// </summary>
        public string RecommendCodeName { get; set; }
    }

    public class RLoginModel
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///  密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string PicUrl { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 手势密码
        /// </summary>
        public string GestureCipher { get; set; }

        public bool IsCompanyLogin { get; set; }

        /// <summary>
        /// 是否为担保户
        /// </summary>
        public string IsGur { get; set; } = "0";
    }


    public class RLoginReturnModel
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        ///  密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string picUrl { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }

        public string gestureCipher { get; set; }
    }

    /// <summary>
    /// 用户基础信息
    /// </summary>
    public class RUseBaseInfoModel
    {
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 开户账号
        /// </summary>
        public string AccountNo { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 身份证号码
        /// </summary>
        public string UserCard { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 绑定银行卡
        /// </summary>
        public string BankCard { get; set; }
        /// <summary>
        /// 渤海开户
        /// </summary>
        public bool BoHai { get; set; }
        /// <summary>
        /// 结算开户
        /// </summary>
        public bool JieSuan { get; set; }
        /// <summary>
        /// 渤海返回信息
        /// </summary>
        public string BoHaiMsg { get; set; }
        /// <summary>
        /// 结算返回信息
        /// </summary>
        public string JieSuanMsg { get; set; }
        /// <summary>
        /// 渤海返回码
        /// </summary>
        public string BoHaiCode { get; set; }
        /// <summary>
        /// 结算返回码
        /// </summary>
        public string JieSuanCode { get; set; }

        /// <summary>
        /// 渤海开户号
        /// </summary>
        public string PersonalChargeAccount { get; set; }

    }

    public class RUserState
    {
        public string JieSuan { get; set; }

        public string BoHai { get; set; }

        public string Auth { get; set; }

        public string Risk { get; set; }
        /// <summary>
        /// 授权金额
        /// </summary>
        public string AuthMoney { get; set; } = "0.00";
    }
}