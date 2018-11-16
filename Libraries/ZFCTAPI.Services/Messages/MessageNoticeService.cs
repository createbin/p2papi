using System;
using System.Collections.Generic;
using ZFCTAPI.Core.Configuration.DataBase;
using ZFCTAPI.Data.Customers;
using ZFCTAPI.Data.PRO;
using ZFCTAPI.Services.Messages;
using ZFCTAPI.Services.Settings;

namespace ZFCTAPI.Services.Messages
{
    public class MessageNoticeService : IMessageNoticeService
    {
        #region Fields

        private readonly ISMSService _smsMessageService;
        private readonly IWorkflowMessageService _emailMessageService;

        #endregion Fields

        public MessageNoticeService(ISMSService smsMessageService,
            ISettingService settingService,
            IWorkflowMessageService emailMessageService)
        {
            _smsMessageService = smsMessageService;
            _emailMessageService = emailMessageService;
        }

        public bool SendValidateCode(string validateCode, string phone, int languageId = 1, string num = "")
        {
            return _smsMessageService.SendValidateCode(validateCode, phone, languageId, num);
        }

        public bool SendEmailActiveMsg(P2PInfo p2pInfo, string validateCode, int languageId = 1)
        {
            return _emailMessageService.SendCustomerEmailValidationMessage(p2pInfo, validateCode, languageId);
        }

        public bool SendMsgNotice(string phone, string msg)
        {
            return _smsMessageService.SendMsgNotice(phone, msg);
        }

        public bool SendWithdrawalMsg(string money, string phone, int languageId = 1)
        {
            return _smsMessageService.SendWithdrawalMsg(money, phone, languageId);
        }
    }
}