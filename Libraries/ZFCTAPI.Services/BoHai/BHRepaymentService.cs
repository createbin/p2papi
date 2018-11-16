using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using ZFCTAPI.Core.Configuration;
using ZFCTAPI.Core.Enums;
using ZFCTAPI.Core.Helpers;
using ZFCTAPI.Data.ApiModels.SubmitModels;
using ZFCTAPI.Data.BoHai.ReturnModels;
using ZFCTAPI.Data.BoHai.SubmitModels;
using ZFCTAPI.Data.RabbitMQ;
using ZFCTAPI.Services.RabbitMQ;
using ZFCTAPI.Services.SYS;

namespace ZFCTAPI.Services.BoHai
{
    public interface IBHRepaymentService
    {
        /// <summary>
        /// 用户还款
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        RBHBaseModel RepaymentExecute(SBHRepaymentExecuteEx model);

        /// <summary>
        /// 流标(无投资人)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        RBHRevoke Revoke(SBHRevoke model);

        /// <summary>
        /// 募集结果上报
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        RBHBaseModel RaiseResultNotice(SBHRaiseLoanResult model);

        /// <summary>
        /// 批量申购撤销
        /// </summary>
        /// <returns></returns>
        RBHBaseModel BatInvestCancle(SBHBatInvestCancle model);

        /// <summary>
        /// 修改项目起息日
        /// </summary>
        /// <returns></returns>
        RBHBaseModel UpdateProjectRateDate(SBHUpdateProjectRateDate model);

        /// <summary>
        /// 还款充值
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        RBHBaseModel PayBackRecharge(SBPayBackRecharge model);

        /// <summary>
        /// 线下充值记录查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        RBQueryChargeDetail QueryChargeDetail(SBQueryChargeDetail model);

        /// <summary>
        /// 投资合同上传
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        RBHBaseModel ContractFileUpload(SBContractFileUpload model);
    }

    public class BHRepaymentService : IBHRepaymentService
    {
        private readonly BoHaiApiConfig _bohaiApiAddress;
        private readonly IRabbitMQEvent _rabbitMQEvent;
        private readonly IWorkContext workContext;

        public BHRepaymentService(BoHaiApiConfig boHaiApiConfig,
            IRabbitMQEvent rabbitMQEvent,
            IWorkContext workContext)
        {
            _bohaiApiAddress = boHaiApiConfig;
            _rabbitMQEvent = rabbitMQEvent;
            this.workContext = workContext;
        }

        public RBHBaseModel BatInvestCancle(SBHBatInvestCancle model)
        {
            model.serviceName = InterfaceName.p_BatInvestCancle.ToString();
            var requestAddress = _bohaiApiAddress.ApiAddress +"/"+ model.serviceName;
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            var post = JsonConvert.SerializeObject(model, Formatting.Indented, jSetting);

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.InputParameters.ToString(),
                ITF_req_parameters = post,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_BatInvestCancle.ToString()
            });

            var result = HttpClientHelper.PostAsync(requestAddress, post).Result.Content.ReadAsStringAsync().Result;

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.OutputParameters.ToString(),
                ITF_req_parameters = result,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_BatInvestCancle.ToString()
            });

            try
            {
                return JsonConvert.DeserializeObject<RBHBaseModel>(result);
            }
            catch
            {
                return null;
            }
        }

        public RBHBaseModel RepaymentExecute(SBHRepaymentExecuteEx model)
        {
            model.serviceName = InterfaceName.p_repaymentExecuteEx.ToString();
            var requestAddress = _bohaiApiAddress.ApiAddress +"/"+ model.serviceName;
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            var post = JsonConvert.SerializeObject(model, Formatting.Indented, jSetting);

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.InputParameters.ToString(),
                ITF_req_parameters = post,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_repaymentExecuteEx.ToString()
            });

            var result = HttpClientHelper.PostAsync(requestAddress, post).Result.Content.ReadAsStringAsync().Result;

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.OutputParameters.ToString(),
                ITF_req_parameters = result,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_repaymentExecuteEx.ToString()
            });

            try
            {
                return JsonConvert.DeserializeObject<RBHBaseModel>(result);
            }
            catch
            {
                return null;
            }
        }

        public RBHBaseModel RaiseResultNotice(SBHRaiseLoanResult model)
        {
            model.serviceName = InterfaceName.p_raiseResultNoticeEx.ToString();
            var requestAddress = _bohaiApiAddress.ApiAddress +"/"+ model.serviceName;
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            var post = JsonConvert.SerializeObject(model, Formatting.Indented, jSetting);

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.InputParameters.ToString(),
                ITF_req_parameters = post,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_raiseResultNoticeEx.ToString()
            });

            var result = HttpClientHelper.PostAsync(requestAddress, post).Result.Content.ReadAsStringAsync().Result;

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.OutputParameters.ToString(),
                ITF_req_parameters = result,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_raiseResultNoticeEx.ToString()
            });

            try
            {
                return JsonConvert.DeserializeObject<RBHBaseModel>(result);
            }
            catch
            {
                return null;
            }
        }

        public RBHBaseModel UpdateProjectRateDate(SBHUpdateProjectRateDate model)
        {
            model.serviceName = InterfaceName.p_updateProject.ToString();
            var requestAddress =  _bohaiApiAddress.ApiAddress +"/"+ model.serviceName;
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            var post = JsonConvert.SerializeObject(model, Formatting.Indented, jSetting);

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.InputParameters.ToString(),
                ITF_req_parameters = post,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_updateProject.ToString()
            });

            var result = HttpClientHelper.PostAsync(requestAddress, post).Result.Content.ReadAsStringAsync().Result;

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.OutputParameters.ToString(),
                ITF_req_parameters = result,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_updateProject.ToString()
            });

            try
            {
                return JsonConvert.DeserializeObject<RBHBaseModel>(result);
            }
            catch
            {
                return null;
            }
        }

        public RBHBaseModel PayBackRecharge(SBPayBackRecharge model)
        {
            model.serviceName = InterfaceName.p_PayBackRecharge.ToString();
            var requestAddress = _bohaiApiAddress.ApiAddress + "/" + model.serviceName;
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            var post = JsonConvert.SerializeObject(model, Formatting.Indented, jSetting);

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.InputParameters.ToString(),
                ITF_req_parameters = post,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_PayBackRecharge.ToString()
            });

            var result = HttpClientHelper.PostAsync(requestAddress, post).Result.Content.ReadAsStringAsync().Result;

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.OutputParameters.ToString(),
                ITF_req_parameters = result,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_PayBackRecharge.ToString()
            });

            try
            {
                return JsonConvert.DeserializeObject<RBHBaseModel>(result);
            }
            catch
            {
                return null;
            }
        }


        public RBQueryChargeDetail QueryChargeDetail(SBQueryChargeDetail model)
        {
            model.serviceName = InterfaceName.p_QueryChargeDetail.ToString();
            var requestAddress = _bohaiApiAddress.ApiAddress + "/" + model.serviceName;
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };

            var post = JsonConvert.SerializeObject(model, Formatting.Indented, jSetting);

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.InputParameters.ToString(),
                ITF_req_parameters = post,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_QueryChargeDetail.ToString()
            });

            var result = HttpClientHelper.PostAsync(requestAddress, post).Result.Content.ReadAsStringAsync().Result;

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.OutputParameters.ToString(),
                ITF_req_parameters = result,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_QueryChargeDetail.ToString()
            });

            try
            {
                return JsonConvert.DeserializeObject<RBQueryChargeDetail>(result);
            }
            catch
            {
                return null;
            }
        }

        public RBHRevoke Revoke(SBHRevoke model)
        {
            model.serviceName = InterfaceName.p_revoke.ToString();
            var requestAddress = _bohaiApiAddress.ApiAddress + "/" + model.serviceName;
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            var post = JsonConvert.SerializeObject(model, Formatting.Indented, jSetting);

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.InputParameters.ToString(),
                ITF_req_parameters = post,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_revoke.ToString()
            });

            var result = HttpClientHelper.PostAsync(requestAddress, post).Result.Content.ReadAsStringAsync().Result;

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.OutputParameters.ToString(),
                ITF_req_parameters = result,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_revoke.ToString()
            });

            try
            {
                return JsonConvert.DeserializeObject<RBHRevoke>(result);
            }
            catch
            {
                return null;
            }
        }

        public RBHBaseModel ContractFileUpload(SBContractFileUpload model)
        {
            model.serviceName = InterfaceName.p_ContractFileUpload.ToString();
            var requestAddress = _bohaiApiAddress.ApiAddress + "/" + model.serviceName;
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            var post = JsonConvert.SerializeObject(model, Formatting.Indented, jSetting);

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.InputParameters.ToString(),
                ITF_req_parameters = post,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_ContractFileUpload.ToString()
            });

            var result = HttpClientHelper.PostAsync(requestAddress, post).Result.Content.ReadAsStringAsync().Result;

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.OutputParameters.ToString(),
                ITF_req_parameters = result,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_ContractFileUpload.ToString()
            });

            try
            {
                return JsonConvert.DeserializeObject<RBHBaseModel>(result);
            }
            catch
            {
                return null;
            }
        }
    }
}
