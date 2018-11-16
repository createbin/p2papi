using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Core.Configuration.DataBase
{
    /// <summary>
    /// 短信发送设置  基础
    /// </summary>
    public partial class MessageNoticeSettings : ISettings
    {
        ///// <summary>
        ///// 注册是否发送短信
        ///// </summary>
        //public bool SendByRegister { set; get; }

        /// <summary>
        /// 修改密码是否发送短信
        /// </summary>
        public bool SendByChangePassWord { set; get; }

        /// <summary>
        /// 登录是否发送短信验证
        /// </summary>
        public bool SendByLogin { set; get; }

        /// <summary>
        /// 投资是否使用 短信验证
        /// </summary>
        public bool SendByInvest { set; get; }

        /// <summary>
        /// 注册成功后是否通知 短信
        /// </summary>
        public bool SendRegisterSuccessForSms { set; get; }

        /// <summary>
        /// 注册成功后是否通知  邮件
        /// </summary>
        public bool SendRegisterSuccessForEmail { set; get; }

        /// <summary>
        /// 投资成功后是否通知 短信
        /// </summary>
        public bool SendByInvestSuccessResultForSms { set; get; }

        /// <summary>
        /// 投资成功后是否通知  邮件
        /// </summary>
        public bool SendByInvestSuccessResultForEmail { set; get; }

        /// <summary>
        /// 收益是否通知 短信
        /// </summary>
        public bool SendByGetInterestForSms { set; get; }

        /// <summary>
        /// 收益是否通知  邮件
        /// </summary>
        public bool SendByGetInterestForEmail { set; get; }
    }
}