using System;
using System.Collections.Generic;
using System.Linq;
using ZFCTAPI.Data.Customers;
using ZFCTAPI.Data.Message;
using ZFCTAPI.Data.PRO;
using ZFCTAPI.Services.Localization;
using ZFCTAPI.Services.Sys;

namespace ZFCTAPI.Services.Messages
{
    public class SMSService : ISMSService
    {
        #region Fields

        private readonly IQueuedSMSService _queuedSMSService;
        private readonly ITokenizer _tokenizer;
        private readonly IMessageTemplateService _messageTemplateService;
        private readonly IMessageTokenProvider _messageTokenProvider;
        private readonly IStoreService _storeService;

        #endregion Fields

        public SMSService(IQueuedSMSService queuedSMSService,
            ITokenizer tokenizer,
            IMessageTemplateService messageTemplateService,
            IMessageTokenProvider messageTokenProvider,
            IStoreService storeService)
        {
            _queuedSMSService = queuedSMSService;
            _tokenizer = tokenizer;
            _messageTemplateService = messageTemplateService;
            _messageTokenProvider = messageTokenProvider;
            _storeService = storeService;
        }

        protected virtual bool SendNotification(MessageTemplate messageTemplate,
            int languageId, IEnumerable<Token> tokens,
           string toPhone, string toName, int Priority
          )
        {
            //retrieve localized message template data
            var body = messageTemplate.GetLocalized((mt) => mt.Body, languageId);

            //Replace subject and body tokens
            var bodyReplaced = _tokenizer.Replace(body, tokens, true);

            _queuedSMSService.Add(new QueuedSMS()
            {
                Priority = Priority,
                ToName = toName,
                ToPhone = toPhone,
                Body = bodyReplaced,
                //CreatedOnUtc = DateTime.UtcNow
                CreatedOnUtc = DateTime.Now
            });
            return true;
        }

        public bool SendValidateCode(string validateCode, string phone, int languageId = 1, string num = "")
        {
            var store = _storeService.GetAll().FirstOrDefault();
            if (store == null)
                return false;

            var messageTemplate = GetActiveMessageTemplate("Validate.Access", store.Id);
            if (messageTemplate == null)
                return false;

            //tokens
            var tokens = new List<Token>();
            _messageTokenProvider.AddValidateTokens(tokens, validateCode);

            //event notification
            //_eventPublisher.MessageTokensAdded(messageTemplate, tokens);

            //短信类型（1：广告（普通速度）；2：验证码（最高速度）；3：催费（较高速度）；）
            return SendNotification(messageTemplate, languageId, tokens, phone, num, 2);
        }

        public bool SendWithdrawalMsg(string money, string phone, int languageId = 1)
        {
            var store = _storeService.GetAll().FirstOrDefault();
            if (store == null)
                return false;

            var messageTemplate = GetActiveMessageTemplate("Raise.WithdrawSuccess.Sms", store.Id);
            if (messageTemplate == null)
                return false;

            //tokens
            var tokens = new List<Token>();
            tokens.Add(new Token("time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            tokens.Add(new Token("money", money));

            //event notification
            //_eventPublisher.MessageTokensAdded(messageTemplate, tokens);


            return SendNotification(messageTemplate, languageId, tokens, phone, string.Empty, 1);
        }

        public bool SendMsgNotice(string phone, string msg)
        {
            //Replace subject and body tokens 
            var bodyReplaced = msg;;
            _queuedSMSService.Add(new QueuedSMS()
            {
                Priority = 2,
                ToName = "",
                ToPhone = phone,
                Body = bodyReplaced,
                //CreatedOnUtc = DateTime.UtcNow
                CreatedOnUtc = DateTime.Now
            });
            return true;

        }

        #region Protected Method

        protected virtual MessageTemplate GetActiveMessageTemplate(string messageTemplateName, int storeId)
        {
            var messageTemplate = _messageTemplateService.GetMessageTemplateByName(messageTemplateName, storeId);

            //no template found
            if (messageTemplate == null)
                return null;

            //ensure it's active
            var isActive = messageTemplate.IsActive;
            if (!isActive)
                return null;

            return messageTemplate;
        }

        #endregion Protected Method
    }
}