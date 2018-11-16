using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;
using ZFCTAPI.Core.Configuration;
using ZFCTAPI.Core.Helpers;
using ZFCTAPI.WebApi.Infrastructure.Extensions;

namespace ZFCTAPI.WebApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("App_Data/rabbitmq.settings.json", optional: false)
                .AddJsonFile("App_Data/zfct.web.settings.json", optional: false)
                .AddJsonFile("App_Data/bohai.api.settings.json", optional: false)
                .AddJsonFile("App_Data/wechat.settings.json", optional: false);

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// （startup中必须提供的方法）
        /// </summary>
        /// <param name="services">当前容器中各服务的配置集合配置后保证服务在应用程序中可用</param>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.ApplicationServices(Configuration);
            return services.ConfigureApplicationServices(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app">被应用于构建应用程序的请求管道</param>
        /// <param name="env">提供了当前环境， 程序根文件</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
                app.UseDeveloperExceptionPage();
            //}
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions { 
                ServeUnknownFileTypes = true ,
                ContentTypeProvider = new FileExtensionContentTypeProvider( new Dictionary< string, string> { { ".apk", "application/vnd.android.package-archive"}, { ".nupkg", "application/zip"} })
            });
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseSwagger();
            app.UseSwaggerUI(p =>
            {
                p.SwaggerEndpoint("/swagger/v0.0.1/swagger.json", "ZFCT API V0.0.1");
            });
        }
    }
}