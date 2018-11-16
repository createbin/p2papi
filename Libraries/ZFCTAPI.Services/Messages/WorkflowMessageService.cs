using System;
using System.Collections.Generic;
using System.Linq;
using ZFCTAPI.Core.Configuration.DataBase;
using ZFCTAPI.Data.Message;
using ZFCTAPI.Data.PRO;
using ZFCTAPI.Services.Localization;
using ZFCTAPI.Services.Settings;
using ZFCTAPI.Services.Sys;

namespace ZFCTAPI.Services.Messages
{
    public partial class WorkflowMessageService : IWorkflowMessageService
    {
        #region Fields

        private readonly IMessageTemplateService _messageTemplateService;
        private readonly IQueuedEmailService _queuedEmailService;
        private readonly ITokenizer _tokenizer;
        private readonly IEmailAccountService _emailAccountService;
        private readonly IMessageTokenProvider _messageTokenProvider;
        private readonly ISettingService _settingService;
        private readonly IStoreService _storeService;

        #endregion Fields

        #region Ctor

        public WorkflowMessageService(IMessageTemplateService messageTemplateService,
            IQueuedEmailService queuedEmailService,
            ITokenizer tokenizer,
            IEmailAccountService emailAccountService,
            IMessageTokenProvider messageTokenProvider,
            ISettingService settingService,
            IStoreService storeService)
        {
            _messageTemplateService = messageTemplateService;
            _queuedEmailService = queuedEmailService;
            _tokenizer = tokenizer;
            _emailAccountService = emailAccountService;
            _messageTokenProvider = messageTokenProvider;
            _settingService = settingService;
            _storeService = storeService;
        }

        #endregion Ctor

        #region Utilities

        protected virtual bool SendNotification(MessageTemplate messageTemplate,
            EmailAccount emailAccount, int languageId, IEnumerable<Token> tokens,
            string toEmailAddress, string toName,
            string attachmentFilePath = null, string attachmentFileName = null,
            string replyToEmailAddress = null, string replyToName = null)
        {
            if (string.IsNullOrWhiteSpace(toEmailAddress))
                return false;

            //retrieve localized message template data
            var bcc = messageTemplate.GetLocalized((mt) => mt.BccEmailAddresses, languageId);
            var subject = messageTemplate.GetLocalized((mt) => mt.Subject, languageId);
            var body = messageTemplate.GetLocalized((mt) => mt.Body, languageId);

            //Replace subject and body tokens
            var subjectReplaced = _tokenizer.Replace(subject, tokens, false);
            var bodyReplaced = _tokenizer.Replace(body, tokens, true);

            _queuedEmailService.Add(new QueuedEmail()
            {
                Priority = 5,
                From = emailAccount.Email,
                FromName = emailAccount.DisplayName,
                To = toEmailAddress,
                ToName = toName,
                ReplyTo = replyToEmailAddress,
                ReplyToName = replyToName,
                CC = string.Empty,
                Bcc = bcc,
                Subject = subjectReplaced,
                Body = bodyReplaced,
                AttachmentFilePath = attachmentFilePath,
                AttachmentFileName = attachmentFileName,
                CreatedOnUtc = DateTime.UtcNow,
                EmailAccountId = emailAccount.Id
            });
            return true;
        }

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

        protected virtual EmailAccount GetEmailAccountOfMessageTemplate(MessageTemplate messageTemplate, int languageId)
        {
            var _emailAccountSettings = _settingService.LoadSetting<EmailAccountSettings>();
            var emailAccounId = messageTemplate.GetLocalized(mt => mt.EmailAccountId, languageId);
            var emailAccount = _emailAccountService.Find(emailAccounId);
            if (emailAccount == null)
                emailAccount = _emailAccountService.Find(_emailAccountSettings.DefaultEmailAccountId);
            if (emailAccount == null)
                emailAccount = _emailAccountService.GetAll().FirstOrDefault();
            return emailAccount;
        }

        #endregion Utilities

        #region Methods

        /// <summary>
        /// Sends an email validation message to a customer
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual bool SendCustomerEmailValidationMessage(P2PInfo p2pInfo, string validateCode, int languageId)
        {
            if (p2pInfo == null)
                throw new ArgumentNullException("p2pInfo");

            var store = _storeService.GetAll().FirstOrDefault();
            if (store == null)
                return false;

            var messageTemplate = GetActiveMessageTemplate("Customer.EmailValidationMessage", store.Id);
            if (messageTemplate == null)
                return false;

            //email account
            var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

            //tokens
            var tokens = new List<Token>();
            _messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);
            _messageTokenProvider.AddValidateTokens(tokens, validateCode);

            //event notification
            //_eventPublisher.MessageTokensAdded(messageTemplate, tokens);

            var toEmail = p2pInfo.UserInfo.cst_user_email;
            var toName = p2pInfo.UserInfo.cst_user_name;
            return SendNotification(messageTemplate, emailAccount,
                languageId, tokens,
                toEmail, toName);
        }

        #endregion Methods
    }
}