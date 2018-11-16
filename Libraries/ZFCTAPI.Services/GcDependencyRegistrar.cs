using Autofac;
using Microsoft.Extensions.DependencyInjection;
using ZFCTAPI.Core.Infrastructure;
using ZFCTAPI.Core.Infrastructure.DependencyManagement;
using ZFCTAPI.Services.Helpers;
using ZFCTAPI.Services.Messages;
using ZFCTAPI.Services.Repayment;
using ZFCTAPI.Services.Sys;
using ZFCTAPI.Services.SYS;
using ZFCTAPI.Services.Transaction;
using ZFCTAPI.Services.WeChat;

namespace ZFCTAPI.Services
{
    public class GcDependencyRegistrar : IDependencyRegistrar
    {
        public void Register(ContainerBuilder builder, ITypeFinder typeFinder, IServiceCollection services)
        {
            //services
            services.Add(new ServiceDescriptor(serviceType: typeof(ISYSDataDictionaryService), implementationType: typeof(SYSDataDictionaryService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(ICstTransactionService), implementationType: typeof(CstTransactionService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(IEmailAccountService), implementationType: typeof(EmailAccountService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(IMessageNoticeService), implementationType: typeof(MessageNoticeService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(IMessageTemplateService), implementationType: typeof(MessageTemplateService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(IMessageTokenProvider), implementationType: typeof(MessageTokenProvider), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(ISMSService), implementationType: typeof(SMSService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(IWorkflowMessageService), implementationType: typeof(WorkflowMessageService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(IQueuedEmailService), implementationType: typeof(QueuedEmailService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(IQueuedSMSService), implementationType: typeof(QueuedSMSService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(ITokenizer), implementationType: typeof(Tokenizer), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(IStoreService), implementationType: typeof(StoreService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(IWeChatAlternately), implementationType: typeof(WeChatAlternately), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(ItbWechatService), implementationType: typeof(tbWechatService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(IWeChatPushSwitchService), implementationType: typeof(WeChatPushSwitchService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(IWeChatPushTemplateService), implementationType: typeof(WeChatPushTemplateService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(ISYSUserWechatService), implementationType: typeof(SYSUserWechatService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(IProTranferApplyService), implementationType: typeof(ProTranferApplyService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(IUserAgentHelper), implementationType: typeof(UserAgentHelper), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(IInterestProvider), implementationType: typeof(InterestProvider), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(IPro_ContractService), implementationType: typeof(Pro_ContractService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(ICST_suspicious_transactionService), implementationType: typeof(CST_suspicious_transactionService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(IQueueWeChatMsgService), implementationType: typeof(QueueWeChatMsgService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(IWorkContext), implementationType: typeof(WebWorkContext), lifetime: ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(serviceType: typeof(IFeeService), implementationType: typeof(FeeService), lifetime: ServiceLifetime.Transient));
            
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets order of this dependency registrar implementation
        /// </summary>
        public int Order => 1;
    }
}