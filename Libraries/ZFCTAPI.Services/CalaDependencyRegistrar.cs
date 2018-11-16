using Autofac;
using Microsoft.Extensions.DependencyInjection;
using ZFCTAPI.Core.Infrastructure;
using ZFCTAPI.Core.Infrastructure.DependencyManagement;
using ZFCTAPI.Services.BoHai;
using ZFCTAPI.Services.Helpers;
using ZFCTAPI.Services.Interest;
using ZFCTAPI.Services.LoanInfo;
using ZFCTAPI.Services.Logs;
using ZFCTAPI.Services.Sys;
using ZFCTAPI.Services.UserInfo;

namespace ZFCTAPI.Services
{
    public class CalaDependencyRegistrar : IDependencyRegistrar
    {
        public void Register(ContainerBuilder builder, ITypeFinder typeFinder, IServiceCollection services)
        {
            services.Add(new ServiceDescriptor(serviceType: typeof(ILoanInfoService), implementationType: typeof(LoanInfoService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(ITransInfoService), implementationType: typeof(TransInfoService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(ILoanPlanService), implementationType: typeof(LoanPlanService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(IUploadInfoService), implementationType: typeof(UploadInfoService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(IAccountInfoService), implementationType: typeof(AccountInfoService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(IBHUserService), implementationType: typeof(BHUserService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(IInterestService), implementationType: typeof(InterestService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(ITransApplyService), implementationType: typeof(TransApplyService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(ILoanTextHelper), implementationType: typeof(LoanTextHelper), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType:typeof(IBoHaiReconciliationService),implementationType:typeof(BoHaiReconciliationService),lifetime:ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(ICompanyInfoService), implementationType: typeof(CompanyInfoService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(IInterfaceLogService), implementationType: typeof(InterfaceLogService), lifetime: ServiceLifetime.Transient));
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets order of this dependency registrar implementation
        /// </summary>
        public int Order => 2;
    }
}
