using System.Collections.Generic;
using ZFCTAPI.Data.Customers;
using ZFCTAPI.Data.PRO;

namespace ZFCTAPI.Services.Messages
{
    public partial interface IMessageNoticeService
    {
        /// <summary>
        /// 短信验证码
        /// </summary>
        /// <param name="validateCode"></param>
        /// <returns></returns>
        bool SendValidateCode(string validateCode, string phone, int languageId = 1, string num = "");

        /// <summary>
        /// 邮箱验证码
        /// </summary>
        /// <param name="p2pInfo"></param>
        /// <param name="validateCode"></param>
        /// <param name="languageId"></param>
        bool SendEmailActiveMsg(P2PInfo p2pInfo, string validateCode, int languageId = 1);

        /// <summary>
        /// 发送自定义消息通知
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="msg"></param>
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