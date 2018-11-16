using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZFCTAPI.Core.Configuration;
using ZFCTAPI.Core.Infrastructure;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;
using ZFCTAPI.Services.RabbitMQ;
using RabbitMQ.Client;
using ZFCTAPI.Services.Scheduling;
using ZFCTAPI.Services.Scheduling.Tasks;
using ZFCTAPI.WebApi.RequestAttribute;

namespace ZFCTAPI.WebApi.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {

        public static void ApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwagger();
            var rabbitMqConfig = services.ConfigureStartupConfig<RabbitMQConfig>(configuration.GetSection("RabbitMQ"));
            //注册MQ
            services.AddRabbitMQ(rabbitMqConfig);
        }

        /// <summary>
        /// Add services to the application and configure service provider
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration root of the application</param>
        /// <returns>Configured service provider</returns>
        public static IServiceProvider ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            //add hosting configuration parameters
            services.ConfigureStartupConfig<ConnectionsConfig>(configuration.GetSection("Connections"));
            services.ConfigureStartupConfig<BoHaiApiConfig>(configuration.GetSection("CoInfo"));
            services.ConfigureStartupConfig<ZfctWebConfig>(configuration.GetSection("ZfctWeb"));
            services.ConfigureStartupConfig<FastDFSConfig>(configuration.GetSection("FastDFS"));
            services.ConfigureStartupConfig<FtpConfig>(configuration.GetSection("FtpServer"));
            services.ConfigureStartupConfig<CommonConfig>(configuration.GetSection("Common"));
            services.ConfigureStartupConfig<WeChatConfig>(configuration.GetSection("WeChat"));

            //add accessor to HttpContext
            services.AddHttpContextAccessor();
            //注册自动执行任务
            services.AddTasks();
            //create, initialize and configure the engine

            //注册过滤器
            //services.AddSingleton<RequestLogAttribute>();
            var engine = EngineContext.Create();
            engine.Initialize(services);
            var serviceProvider = engine.ConfigureServices(services, configuration);

            return serviceProvider;
        }

        /// <summary>
        /// Create, bind and register as service the specified configuration parameters
        /// </summary>
        /// <typeparam name="TConfig">Configuration parameters</typeparam>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Set of key/value application configuration properties</param>
        /// <returns>Instance of configuration parameters</returns>
        public static TConfig ConfigureStartupConfig<TConfig>(this IServiceCollection services, IConfiguration configuration) where TConfig : class, new()
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            //create instance of config
            var config = new TConfig();

            //bind it to the appropriate section of configuration
            configuration.Bind(config);

            //and register it as a service
            services.AddSingleton(config);

            services.AddProjectMvc();

            return config;
        }

        /// <summary>
        /// Register HttpContextAccessor
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddHttpContextAccessor(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        public static IMvcBuilder AddProjectMvc(this IServiceCollection services)
        {
            var mvcBuilder = services.AddMvc();
            return mvcBuilder;
        }

        public static void AddTasks(this IServiceCollection services)
        {
            // Add scheduled tasks & scheduler
            services.AddSingleton<IScheduledTask, DownloadBoHaiFileTask>();
            services.AddScheduler((sender, args) =>
            {
                Console.Write(args.Exception.Message);
                args.SetObserved();
            });
        }

        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(p =>
            {
                p.SwaggerDoc("v0.0.1", new Info
                {
                    Version = "v0.0.1",
                    Title = "中房创投-渤海银行-Api-Document",
                    Description = "中房创投和渤海银行交互以及给予中房创投PC,Wechat,Android,Ios的接口项目",
                    Contact = new Contact { Name = "Calabash", Email = "924347693@qq.com", Url = "" }
                });
                p.DescribeAllParametersInCamelCase();
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "ZfctApi.xml");
                p.IncludeXmlComments(xmlPath);
            });
        }

        /// <summary>
        /// 注册MQ
        /// </summary>
        /// <param name="services"></param>
        /// <param name="rabbitMQConfig"></param>
        public static void AddRabbitMQ(this IServiceCollection services, RabbitMQConfig rabbitMQConfig)
        {
            services.AddSingleton<IRabbitMQSubscriptionsManager, RabbitMQSubscriptionsManager>();
            services.AddSingleton<IRabbitMQEvent, RabbitMQEvent>();
            services.AddSingleton<IRabbitMQPersistentConnection>(provider =>
            {
                var HostName = rabbitMQConfig.Connection.IP;
                var rabbitUserName = rabbitMQConfig.Connection.UserName;
                var rabbitPassword = rabbitMQConfig.Connection.PassWord;
                var factory = new ConnectionFactory()
                {
                    HostName = HostName,
                    UserName = rabbitUserName,
                    Password = rabbitPassword
                };
                return new RabbitMQPersistentConnection(factory);
            });
        }
    }
}