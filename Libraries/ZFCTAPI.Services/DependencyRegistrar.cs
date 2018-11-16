using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using ZFCTAPI.Core.Caching;
using ZFCTAPI.Core.DbContext;
using ZFCTAPI.Core.Helpers;
using ZFCTAPI.Core.Infrastructure;
using ZFCTAPI.Core.Infrastructure.DependencyManagement;
using ZFCTAPI.Services.BoHai;
using ZFCTAPI.Services.InvestInfo;
using ZFCTAPI.Services.LoanInfo;
using ZFCTAPI.Services.Popular;
using ZFCTAPI.Services.Repositorys;
using ZFCTAPI.Services.Security;
using ZFCTAPI.Services.UserInfo;
using ZFCTAPI.Services.Promotion;
using ZFCTAPI.Services.Settings;
using Autofac.Core;
using System.Reflection;
using ZFCTAPI.Core.Configuration;
using Autofac.Builder;
using ZFCTAPI.Services.BatchImport;
using ZFCTAPI.Services.Repayment;
using ZFCTAPI.Services.RiskAssessment;
using ZFCTAPI.Services.Transfer;
using ZFCTAPI.Services.Sys;
using ZFCTAPI.Services.Interest;
using ZFCTAPI.Services.Helpers;
using ZFCTAPI.Services.Transaction;
using ZFCTAPI.Services.Messages;
using ZFCTAPI.Services.Views;
using ZFCTAPI.Services.WeChat;

namespace ZFCTAPI.Services
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public void Register(ContainerBuilder builder, ITypeFinder typeFinder, IServiceCollection services)
        {
            //services
            builder.RegisterType<DapperDbContext>().As<IDbContext>().InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();

            services.Add(new ServiceDescriptor(serviceType: typeof(ICacheManager), implementationType: typeof(MemoryCacheManager), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(IEncryptionService), implementationType: typeof(EncryptionService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(ICustomerService), implementationType: typeof(CustomerService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(ICstUserInfoService), implementationType: typeof(CstUserInfoService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(ILoanInfoService), implementationType: typeof(LoanInfoService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(ICstRedInfoService), implementationType: typeof(CstRedInfoService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(IPopEnvelopeRedService), implementationType: typeof(PopEnvelopeRedService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(IPopEnvelopeRuleService), implementationType: typeof(PopEnvelopeRuleService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(IRedFunctionService), implementationType: typeof(RedFunctionService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(IInvestInfoService), implementationType: typeof(InvestInfoService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(IInvesterPlanService), implementationType: typeof(InvesterPlanService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(IBoHaiService), implementationType: typeof(BoHaiService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(IInternalMagazineService), implementationType: typeof(InternalMagazineService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(IFeedbackService), implementationType: typeof(FeedbackService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(IAdvertisPositionService), implementationType: typeof(AdvertisPositionService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(IAdvertisementService), implementationType: typeof(AdvertisementService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(INewsCategoryService), implementationType: typeof(NewsCategoryService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(INewsService), implementationType: typeof(NewsService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(ISettingService), implementationType: typeof(SettingService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(IActivityStatsService), implementationType: typeof(ActivityStatsService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(IPayRecordService), implementationType: typeof(PayRecordService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(IBHAccountService), implementationType: typeof(BHAccountService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(ICstRealNameCheckService), implementationType: typeof(CstRealNameCheckService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(IInvitationActivitie), implementationType: typeof(InvitationActivitie), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(IRiskAssessmentService), implementationType: typeof(RiskAssessmentService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(IRiskByUserService), implementationType: typeof(RiskByUserService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(IUserInvestTypeService), implementationType: typeof(UserInvestTypeService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(IProLoanFlowService), implementationType: typeof(ProLoanFlowService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(IBHRepaymentService), implementationType: typeof(BHRepaymentService), lifetime: ServiceLifetime.Transient));

            services.Add(new ServiceDescriptor(serviceType: typeof(IInterestProvider), implementationType: typeof(InterestProvider), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(ICapitalTransferService), implementationType: typeof(CapitalTransferService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(ILendRecordService), implementationType: typeof(LendRecordService), lifetime: ServiceLifetime.Transient));

            services.Add(new ServiceDescriptor(serviceType: typeof(IProIntentCheck), implementationType: typeof(ProIntentCheck), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(ILoginLogInfoService), implementationType: typeof(LoginLogInfoService), lifetime: ServiceLifetime.Transient));


            services.Add(new ServiceDescriptor(serviceType: typeof(IBatchImportService), implementationType: typeof(BatchImportService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(IAccountBatchImportService), implementationType: typeof(AccountBatchImportService), lifetime: ServiceLifetime.Transient));
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets order of this dependency registrar implementation
        /// </summary>
        public int Order => 1;
    }
}