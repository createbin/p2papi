using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Rewrite;
using Newtonsoft.Json;
using ZFCTAPI.Core.Helpers;
using ZFCTAPI.Core.Infrastructure;
using ZFCTAPI.Data.ApiModels.SubmitModels;
using ZFCTAPI.Data.Logs;
using ZFCTAPI.Services.Logs;
using ZFCTAPI.Services.RabbitMQ;

namespace ZFCTAPI.WebApi.RequestAttribute
{
    [AttributeUsage(AttributeTargets.All,AllowMultiple = true)]
    public class RequestLogAttribute: ActionFilterAttribute
    {
        private  string guid="";
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.Method.Equals("POST"))
            {
                JsonSerializerSettings settings = new JsonSerializerSettings { Converters = new[] { new JsonsHelper() } };
                var postJson = "";
                foreach (var postItem in filterContext.ActionArguments)
                {
                    if (string.IsNullOrEmpty(postJson))
                    {
                        postJson += JsonConvert.SerializeObject(postItem.Value, settings);
                    }
                }
                if (!string.IsNullOrEmpty(postJson))
                {
                    guid= Guid.NewGuid().ToString("N");
                    var baseModel = JsonConvert.DeserializeObject<BaseSubmitModel>(postJson);
                    if (!string.IsNullOrEmpty(filterContext.ActionDescriptor.Id))
                    {
                        var logSevice = EngineContext.Current.Resolve<IInterfaceLogService>();
                        var requestLog = new InterfaceRequestLog
                        {
                            Attributes = 2,
                            CreateTime = DateTime.Now,
                            FacilityType = baseModel.FacilityType,
                            InterfaceName = filterContext.HttpContext.Request.Path.Value,
                            IpAddress = baseModel.IPAddress,
                            RequestSource = baseModel.RequestSource.ToString(),
                            Timestamps = baseModel.TimeStamp.ToString(),
                            Token = baseModel.Token,
                            RequestContent = postJson,
                            UniquelyIdentifies = guid
                        };
                        try
                        {
                            logSevice.Add(requestLog);

                            var rabbitMQService = EngineContext.Current.Resolve<IRabbitMQEvent>();
                            rabbitMQService.Publish(requestLog);
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                }
            }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.HttpContext.Request.Method.Equals("POST"))
            {
                var result = filterContext.Result as Microsoft.AspNetCore.Mvc.ObjectResult;
                if (result?.Value != null)
                {
                    var jsonResult = JsonConvert.SerializeObject(result.Value);
                    if (!string.IsNullOrEmpty(jsonResult)&&!string.IsNullOrEmpty(filterContext.ActionDescriptor.Id))
                    {
                        var logSevice = EngineContext.Current.Resolve<IInterfaceLogService>();
                        var requestModel = logSevice.GetByUnique(guid);
                        if (requestModel != null)
                        {
                            requestModel.ResponseContent = jsonResult;
                            try
                            {
                                logSevice.Update(requestModel);
                                var rabbitMQService = EngineContext.Current.Resolve<IRabbitMQEvent>();
                                rabbitMQService.Publish(requestModel);
                            }
                            catch
                            {
                                // ignored
                            }
                        }
                    }
                }
            }

        }
    }
}
