using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using ZFCTAPI.Core.Configuration;
using ZFCTAPI.Core.Enums;
using ZFCTAPI.Core.Helpers;
using ZFCTAPI.Data.BoHai.ReturnModels;
using ZFCTAPI.Data.BoHai.SubmitModels;
using ZFCTAPI.Data.BoHai.TransferData;
using ZFCTAPI.Data.RabbitMQ;
using ZFCTAPI.Services.RabbitMQ;

namespace ZFCTAPI.Services.BatchImport
{
    public interface IBatchImportService
    {
        RBHRegisterApplication ExistUserRegister(TransferUserRegister model);
        LoanReturnModelApplication ExistLoanTransfer(TransferLoanStock rModel);

    }

    public class BatchImportService:IBatchImportService
    {
        private readonly IRabbitMQEvent _rabbitMQEvent;
        public readonly string requestAddress = BoHaiApiEngineToConfiguration.JSAddress();

        public  BatchImportService(IRabbitMQEvent rabbitMqEvent)
        {
            _rabbitMQEvent = rabbitMqEvent;
        }

        /// <summary>
        /// 请求存量用户数据迁移接口
        /// </summary>
        /// <param name="model">请求Model</param>
        /// <returns></returns>
        public RBHRegisterApplication ExistUserRegister(TransferUserRegister model)
        {
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            var firstPost = JsonConvert.SerializeObject(model, Formatting.Indented, jSetting);

            #region 记录发送日志

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.InputParameters.ToString(),
                ITF_req_parameters = firstPost,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser ="",
                cmdid = InterfaceName.p_AutoInvestAuth.ToString()
            });

            #endregion 记录发送日志

            var result = HttpClientHelper.PostAsync(requestAddress, firstPost).Result.Content.ReadAsStringAsync().Result;

            #region 记录输出日志

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.OutputParameters.ToString(),
                ITF_ret_parameters = result,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = "",
                cmdid = InterfaceName.p_AutoInvestAuth.ToString()
            });

            #endregion 记录输出日志

            var jsReturn = JsonConvert.DeserializeObject<RBHRegisterApplication>(result);
            if (jsReturn.RspSvcHeader.returnMsg != BHReturnCode.Success.ToString())
                return null;
            return jsReturn;
        }

        /// <summary>
        /// 请求存量标的数据迁移接口
        /// </summary>
        /// <param name="rModel">请求参数Model</param>
        /// <returns></returns>
        public LoanReturnModelApplication ExistLoanTransfer(TransferLoanStock rModel)
        {
            var jSetting = new JsonSerializerSettings {
                NullValueHandling = NullValueHandling.Ignore
            };
            var firstPost = JsonConvert.SerializeObject(rModel, Formatting.Indented, jSetting);
            PublishMQ(firstPost, LogsEnum.InputParameters.ToString(), InterfaceName.p_ExistLoanTransfer.ToString());// 记录发送日志
            var result = HttpClientHelper.PostAsync(requestAddress, firstPost).Result.Content.ReadAsStringAsync().Result;
            PublishMQ(result, LogsEnum.OutputParameters.ToString(), InterfaceName.p_ExistLoanTransfer.ToString());//记录输出日志
            var jsReturn = JsonConvert.DeserializeObject<LoanReturnModelApplication>(result);
            //if (jsReturn.RspSvcHeader.returnCode != BHReturnCode.Success.ToString())
            //    return null;
            return jsReturn;
        }

        /// <summary>
        /// 推送MQ消息
        /// </summary>
        /// <param name="content">需要推送的内容</param>
        /// <param name="type">消息类别</param>
        private void PublishMQ(string content,string rtype,string mqtype)
        {
            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = rtype,
                ITF_ret_parameters = content,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = "",
                cmdid = mqtype
            });
        }
    }
}
