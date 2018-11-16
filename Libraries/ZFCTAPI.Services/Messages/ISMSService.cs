using System.Collections.Generic;
using ZFCTAPI.Data.Customers;
using ZFCTAPI.Data.PRO;

namespace ZFCTAPI.Services.Messages
{
    /// <summary>
    /// 短信发送业务方法
    /// </summary>
    public partial interface ISMSService
    {
        /// <summary>
        /// 短信验证码
        /// </summary>
        /// <param name="validateCode"></param>
        /// <returns></returns>
        bool SendValidateCode(string validateCode, string phone, int languageId = 1, string num = "");

        /// <summary>
        /// 发送自定义消息通知
        /// </summary>
        /// <param name="validateCode"></param>
        /// <returns></returns>
        bool SendMsgNotice(string phone, string msg);

        /// <summary>
        /// 提现通知
        /// </summary>
        /// <param name="withdrawalMessage"></param>
        /// <returns></returns>
        bool SendWithdrawalMsg(string money, string phone, int languageId = 1);
    }
}