using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using System.Web;
using Newtonsoft.Json;
using ZFCTAPI.Core;
using ZFCTAPI.Core.Configuration;
using ZFCTAPI.Core.Enums;
using ZFCTAPI.Core.Helpers;
using ZFCTAPI.Data.ApiModels.ReturnModels;
using ZFCTAPI.Data.BoHai.ReturnModels;
using ZFCTAPI.Data.BoHai.SubmitModels;
using ZFCTAPI.Data.CST;
using ZFCTAPI.Data.RabbitMQ;
using ZFCTAPI.Services.RabbitMQ;
using ZFCTAPI.Services.SYS;
using ZFCTAPI.Services.Transaction;
using ZFCTAPI.Services.UserInfo;

namespace ZFCTAPI.Services.BoHai
{
    public interface IBHUserService
    {
        string UserAdd(SBHUserAdd model, string userType);

        string UserCancel(SBHUserCancel model);

        /// <summary>
        /// 渤海实名认证
        /// </summary>
        /// <param name="model"></param>
        /// <param name="openType"></param>
        RBHRealNameWeb RealNameWeb(SBHRealNameWeb model, UserOpenType openType);

        /// <summary>
        /// 修改绑定银行卡
        /// </summary>
        /// <param name="model"></param>
        RBHBindCardWebBody BindCardWeb(SBHBindCardWeb model);

        /// <summary>
        /// 修改绑定手机号
        /// </summary>
        /// <param name="model"></param>
        RBHBindMobileNoBody BindMobileNo(SBHBindMobileNo model);

        /// <summary>
        /// 修改交易密码
        /// </summary>
        /// <param name="model"></param>
        RBHBindMobileNoBody BindPass(SBHBindPass model);

        /// <summary>
        /// 营销活动（被删除的接口）
        /// </summary>
        /// <param name="model"></param>
        void MarketingCampaigns(SBHMarketingCampaigns model);

        /// <summary>
        /// 发红包
        /// </summary>
        /// <param name="model"></param>
        RBHExperBonus ExperBonus(SBHExperBonus model);

        /// <summary>
        /// 红包转账
        /// </summary>
        /// <param name="model"></param>
        RBHCampaignTransfer CampaignTransfer(SBHCampaignTransfer model);

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="model"></param>
        string SendUapMsg(SBHSendUapMsg model);

        /// <summary>
        /// 用户信息查询
        /// </summary>
        /// <param name="model"></param>
        RBHQueryUserInfoBody QueryUserInfo(SBHQueryUserInfo model);

        /// <summary>
        /// 商户账户查询
        /// </summary>
        /// <param name="model"></param>
        RBHQueryMerchantAcctsBody QueryMerchantAccts(SBHBaseModel model);

        /// <summary>
        /// 更改为开设对公账户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        RBHOpenChargeAccountWeb OpenChargeAccount(SBHOpenChargeAccount model);

        /// <summary>
        /// 对公开户结果查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        RBHQueryChargeAccountResult QueryChargeAccountResult(SBHQueryChargeAccountResult model);

        /// <summary>
        /// 大额充值账号查询
        /// </summary>
        /// <param name="model"></param>
        RBHQueryChargeAccountBody QueryChargeAccount(SBHQueryChargeAccount model);

        /// <summary>
        /// 大额充值记录查询
        /// </summary>
        /// <param name="model"></param>
        RBHQueryChargeDetailBody QueryChargeDetail(SBHQueryChargeDetail model);

        /// <summary>
        /// 商户像用户转账
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        string MeUserTransfer(SBHExperBonus model);

        /// <summary>
        /// 商户账户充值
        /// </summary>
        /// <param name="model"></param>
        RBHMercRecharge MercRecharge(SBHMercRecharge model);
        /// <summary>
        /// 商户账户提现
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        RBHMercWithdraw MercWithdraw(SBHMercWithdraw model);

        /// <summary>
        /// 交易状态查询
        /// </summary>
        /// <param name="model"></param>
        RBHQueryTransStatBody QueryTransStat(SBHQueryTransStat model);



        /// <summary>
        /// 用户授权信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        RBHQueryAuthInf AuthResult(SBHQueryAuthInf model);

        /// <summary>
        /// 用户授权
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        RBHAutoInvestAuthWebBody AutoInvestAuth(SBHAutoInvestAuth model);
        /// <summary>
        /// 是否授权
        /// </summary>
        /// <param name="pluCustId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        bool IsAuth(string pluCustId, AuthTyp type);
        /// <summary>
        /// 用户授权带金额
        /// </summary>
        /// <param name="pluCustId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        RUserAuth AuthInfo(string pluCustId, AuthTyp type);
        /// <summary>
        /// 商户交易记录查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        string QueryMerchantTrans(SBHQueryMerchantTrans model);

        string ToUserMoney(SBHExperBonus model, string investId);
    }

    public class BHUserService : IBHUserService
    {
        private readonly IRabbitMQEvent _rabbitMQEvent;
        private readonly ICstUserInfoService _userInfoService;
        private readonly IAccountInfoService _accountInfoService;
        private readonly ICompanyInfoService _companyInfoService;
        private readonly ICstTransactionService _transactionService;
        public readonly string requestAddress = BoHaiApiEngineToConfiguration.JSAddress();
        public readonly string bohaiAddress = BoHaiApiEngineToConfiguration.BHAddress();
        private readonly IWorkContext workContext;

        public BHUserService(IRabbitMQEvent rabbitMqEvent,
            ICstUserInfoService userInfoService,
            IAccountInfoService accountInfoService,
            ICompanyInfoService companyInfoService,
            IWorkContext workContext,
            ICstTransactionService transactionService)
        {
            _rabbitMQEvent = rabbitMqEvent;
            _userInfoService = userInfoService;
            _accountInfoService = accountInfoService;
            _companyInfoService = companyInfoService;
            this.workContext = workContext;
            _transactionService = transactionService;
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userType"></param>
        public string UserAdd(SBHUserAdd model, string userType)
        {
            model.serviceName = InterfaceName.p_UserAddBH.ToString();
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            var post = JsonConvert.SerializeObject(model, Formatting.Indented, jSetting);

            #region 记录发送日志

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.InputParameters.ToString(),
                ITF_req_parameters = post,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_UserAddBH.ToString()
            });

            #endregion 记录发送日志

            var result = HttpClientHelper.PostAsync(requestAddress, post).Result.Content.ReadAsStringAsync().Result;

            #region 记录输出日志

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.OutputParameters.ToString(),
                ITF_ret_parameters = result,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_UserAddBH.ToString()
            });

            #endregion 记录输出日志

            var jsReturn = JsonConvert.DeserializeObject<RBHBaseModel>(result);
            #region 处理渤海结果

            if (jsReturn.RspSvcHeader.returnCode == JSReturnCode.Success.ToString())
            {
                return "已提交渤海处理";
            }

            return "注册失败：" + jsReturn.RspSvcHeader.returnMsg;

            #endregion 处理渤海结果
        }

        /// <summary>
        /// 用户注销
        /// </summary>
        /// <param name="model"></param>
        public string UserCancel(SBHUserCancel model)
        {
            model.serviceName = InterfaceName.p_UserCancel.ToString();
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            var postData = JsonConvert.SerializeObject(model, Formatting.Indented, jSetting);

            #region 记录发送日志

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.InputParameters.ToString(),
                ITF_req_parameters = postData,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_UserCancel.ToString()
            });

            #endregion 记录发送日志

            var result = HttpClientHelper.PostAsync(requestAddress, postData).Result.Content.ReadAsStringAsync().Result;

            #region 记录输出日志

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.OutputParameters.ToString(),
                ITF_ret_parameters = result,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_UserCancel.ToString()
            });

            #endregion 记录输出日志

            var jsReturn = JsonConvert.DeserializeObject<RBHBaseModel>(result);

            #region 操作返回信息

            if (jsReturn.RspSvcHeader.returnCode == JSReturnCode.Success.ToString())
            {
                //注册中心注册成功
                var userInfo = _userInfoService.GetUserInfo(id: Convert.ToInt32(model.SvcBody.platformUid));
                if (userInfo != null)
                {
                    if (userInfo.cst_account_id != null)
                    {
                        var accountInfo = _accountInfoService.GetAccountInfoByUserId(id: userInfo.cst_account_id.Value);
                        accountInfo.JieSuan = false;
                        _accountInfoService.Update(accountInfo);
                    }
                    return "用户结算中心注销成功";
                }
            }
            return "注销失败：" + jsReturn.RspSvcHeader.returnMsg;

            #endregion 操作返回信息
        }

        /// <summary>
        /// 用户渤海实名绑卡
        /// </summary>
        /// <param name="model"></param>
        /// <param name="openType"></param>
        public RBHRealNameWeb RealNameWeb(SBHRealNameWeb model, UserOpenType openType)
        {
            model.serviceName = InterfaceName.p_RealNameWeb.ToString();
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            var postData = JsonConvert.SerializeObject(model, Formatting.Indented, jSetting);

            #region 记录发送日志

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.InputParameters.ToString(),
                ITF_req_parameters = postData,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_RealNameWeb.ToString()
            });

            #endregion 记录发送日志

            var result = HttpClientHelper.PostAsync(requestAddress, postData).Result.Content.ReadAsStringAsync().Result;

            #region 记录输出日志

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.OutputParameters.ToString(),
                ITF_ret_parameters = result,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_RealNameWeb.ToString()
            });

            #endregion 记录输出日志
            var jsRenturn = JsonConvert.DeserializeObject<RBHRealNameWeb>(result);
            return jsRenturn;
        }

        /// <summary>
        /// 换绑卡
        /// </summary>
        public RBHBindCardWebBody BindCardWeb(SBHBindCardWeb model)
        {
            model.serviceName = InterfaceName.p_BindCardWeb.ToString();
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            var firstPost = JsonConvert.SerializeObject(model, Formatting.Indented, jSetting);

            #region 记录发送日志

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.InputParameters.ToString(),
                ITF_req_parameters = firstPost,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_BindCardWeb.ToString()
            });

            #endregion 记录发送日志

            var result = HttpClientHelper.PostAsync(requestAddress, firstPost).Result.Content.ReadAsStringAsync().Result;

            #region 记录输出日志

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.OutputParameters.ToString(),
                ITF_ret_parameters = result,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_BindCardWeb.ToString()
            });

            #endregion 记录输出日志

            var jsReturn = JsonConvert.DeserializeObject<RBHBindCardWeb>(result);
            if (jsReturn.RspSvcHeader.returnCode != JSReturnCode.Success.ToString())
                return null;
            return jsReturn.SvcBody;
        }

        /// <summary>
        /// 修改手机号
        /// </summary>
        /// <param name="model"></param>
        public RBHBindMobileNoBody BindMobileNo(SBHBindMobileNo model)
        {
            model.serviceName = InterfaceName.p_BindMobileNo.ToString();
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            var firstPost = JsonConvert.SerializeObject(model, Formatting.Indented, jSetting);

            #region 记录发送日志

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.InputParameters.ToString(),
                ITF_req_parameters = firstPost,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_BindMobileNo.ToString()
            });

            #endregion 记录发送日志

            var result = HttpClientHelper.PostAsync(requestAddress, firstPost).Result.Content.ReadAsStringAsync().Result;

            #region 记录输出日志

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.OutputParameters.ToString(),
                ITF_ret_parameters = result,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_BindCardWeb.ToString()
            });

            #endregion 记录输出日志

            var jsRenturn = JsonConvert.DeserializeObject<RBHBindMobileNo>(result);
            return jsRenturn.SvcBody;
        }

        /// <summary>
        /// 修改支付密码
        /// </summary>
        /// <param name="model"></param>
        public RBHBindMobileNoBody BindPass(SBHBindPass model)
        {
            model.serviceName = InterfaceName.p_BindPass.ToString();
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            var firstPost = JsonConvert.SerializeObject(model, Formatting.Indented, jSetting);

            #region 记录发送日志

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.InputParameters.ToString(),
                ITF_req_parameters =  firstPost,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_BindPass.ToString()
            });

            #endregion 记录发送日志

            var result = HttpClientHelper.PostAsync(requestAddress, firstPost).Result.Content.ReadAsStringAsync().Result;

            #region 记录输出日志

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.OutputParameters.ToString(),
                ITF_ret_parameters = result,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_BindPass.ToString()
            });

            #endregion 记录输出日志

            var jsRenturn = JsonConvert.DeserializeObject<RBHBindMobileNo>(result);

            return jsRenturn.SvcBody;

            #region post提交

            //if (jsRenturn.RspSvcHeader.returnCode != JSReturnCode.Success.ToString())
            //{
            //    //返回错误信息
            //    return null;
            //}

            /*
            //返回正确跳转渤海
            var post = new HttpClientPageHelper
            {
                AcceptCharset = "GBK",
                FormName = "bindpass",
                Url = "http://www.baidu.com/",
                Method = "POST"
            };
            post.Add("char_set", jsRenturn.SvcBody.char_set);
            post.Add("partner_id", jsRenturn.SvcBody.partner_id);
            post.Add("version_no", jsRenturn.SvcBody.version_no);
            post.Add("biz_type", jsRenturn.SvcBody.biz_type);
            post.Add("sign_type", jsRenturn.SvcBody.sign_type);
            post.Add("MerBillNo", jsRenturn.SvcBody.MerBillNo);
            post.Add("PlaCustId", jsRenturn.SvcBody.PlaCustId);
            post.Add("PageReturnUrl", jsRenturn.SvcBody.PageReturnUrl);
            post.Add("BgRetUrl", jsRenturn.SvcBody.BgRetUrl);
            post.Add("mac", jsRenturn.SvcBody.mac);

            #region 插入接口post数据

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.PageRequestParameters.ToString(),
                ITF_req_parameters = CommonHelper.NameValueCollectionToString(post.Params),
                ITF_Info_addtime = DateTime.Now,
                cmdid = "BindPass"
            });

            #endregion 插入接口post数据

            await post.Post();*/

            #endregion post提交
        }

        /// <summary>
        /// 营销活动(被删除掉的接口)
        /// </summary>
        public void MarketingCampaigns(SBHMarketingCampaigns model)
        {
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            var post = JsonConvert.SerializeObject(model, Formatting.Indented, jSetting);

            var result = HttpClientHelper.PostAsync(requestAddress, post).Result.Content.ReadAsStringAsync().Result;
        }

        public RBHExperBonus ExperBonus(SBHExperBonus model)
        {
            model.serviceName = InterfaceName.p_experBonus.ToString();
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            var post = JsonConvert.SerializeObject(model, Formatting.Indented, jSetting);

            #region 记录发送日志

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.InputParameters.ToString(),
                ITF_req_parameters = post,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_experBonus.ToString()
            });

            #endregion 记录发送日志

            var result = HttpClientHelper.PostAsync(requestAddress, post).Result.Content.ReadAsStringAsync().Result;

            #region 记录输出日志

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.OutputParameters.ToString(),
                ITF_ret_parameters = result,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_experBonus.ToString()
            });

            #endregion 记录输出日志

            var jsReturn = JsonConvert.DeserializeObject<RBHExperBonus>(result);
            return jsReturn;
        }

        /// <summary>
        /// 红包转账
        /// </summary>
        /// <param name="model"></param>
        public RBHCampaignTransfer CampaignTransfer(SBHCampaignTransfer model)
        {
            model.serviceName = InterfaceName.p_CampaignTransfer.ToString();
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            var post = JsonConvert.SerializeObject(model, Formatting.Indented, jSetting);

            #region 记录发送日志

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.InputParameters.ToString(),
                ITF_req_parameters = post,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_CampaignTransfer.ToString()
            });

            #endregion 记录发送日志

            var result = HttpClientHelper.PostAsync(requestAddress, post).Result.Content.ReadAsStringAsync().Result;

            #region 记录输出日志

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.OutputParameters.ToString(),
                ITF_ret_parameters = result,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_CampaignTransfer.ToString()
            });

            #endregion 记录输出日志

            var jsReturn = JsonConvert.DeserializeObject<RBHCampaignTransfer>(result);
            return jsReturn;
        }

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="model"></param>
        public string SendUapMsg(SBHSendUapMsg model)
        {
            model.serviceName = InterfaceName.p_sendUapMsg.ToString();
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            var postData = JsonConvert.SerializeObject(model, Formatting.Indented, jSetting);

            #region 记录发送日志

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.InputParameters.ToString(),
                ITF_req_parameters = postData,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_sendUapMsg.ToString()
            });

            #endregion 记录发送日志

            var result = HttpClientHelper.PostAsync(requestAddress, postData).Result.Content.ReadAsStringAsync().Result;

            #region 记录输出日志

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.OutputParameters.ToString(),
                ITF_ret_parameters = result,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_sendUapMsg.ToString()
            });

            #endregion 记录输出日志

            var jsRenturn = JsonConvert.DeserializeObject<RBHSendUapMsg>(result);

            if (jsRenturn.SvcBody != null)
                return jsRenturn.SvcBody.rtnCod;
            return string.Empty;
        }

        /// <summary>
        /// 用户信息查询
        /// </summary>
        /// <param name="model"></param>
        public RBHQueryUserInfoBody QueryUserInfo(SBHQueryUserInfo model)
        {
            model.serviceName = InterfaceName.p_QueryUserInf.ToString();
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            var postData = JsonConvert.SerializeObject(model, Formatting.Indented, jSetting);

            #region 记录发送日志

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.InputParameters.ToString(),
                ITF_req_parameters = postData,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_QueryUserInf.ToString()
            });

            #endregion 记录发送日志

            var result = HttpClientHelper.PostAsync(requestAddress, postData).Result.Content.ReadAsStringAsync().Result;

            #region 记录输出日志

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.OutputParameters.ToString(),
                ITF_ret_parameters = result,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_QueryUserInf.ToString()
            });

            #endregion 记录输出日志

            var jsRenturn = JsonConvert.DeserializeObject<RBHQueryUserInfo>(result);
            if (jsRenturn.RspSvcHeader.returnCode == JSReturnCode.Success)
            {
                return jsRenturn.SvcBody;
            }
            return null;
        }

        /// <summary>
        /// 商户账户查询
        /// </summary>
        /// <param name="model"></param>
        public RBHQueryMerchantAcctsBody QueryMerchantAccts(SBHBaseModel model)
        {
            model.serviceName = InterfaceName.p_QueryMerchantAccts.ToString();
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            var postData = JsonConvert.SerializeObject(model, Formatting.Indented, jSetting);

            #region 记录发送日志

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.InputParameters.ToString(),
                ITF_req_parameters = postData,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_QueryMerchantAccts.ToString()
            });

            #endregion 记录发送日志

            var result = HttpClientHelper.PostAsync(requestAddress, postData).Result.Content.ReadAsStringAsync().Result;

            #region 记录输出日志

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.OutputParameters.ToString(),
                ITF_ret_parameters = result,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_QueryMerchantAccts.ToString()
            });

            #endregion 记录输出日志

            var jsReturn = JsonConvert.DeserializeObject<RBHQueryMerchantAccts>(result);

            #region 操作返回信息

            if (jsReturn.RspSvcHeader.returnCode == JSReturnCode.Success.ToString())
            {
                //返回完整商户账户信息
                return jsReturn.SvcBody;
            }
            return null;

            #endregion 操作返回信息
        }
        /// <summary>
        /// 商户账户提现
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public RBHMercWithdraw MercWithdraw(SBHMercWithdraw model)
        {
            model.serviceName = InterfaceName.p_MercWithdraw.ToString();
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            var postData = JsonConvert.SerializeObject(model, Formatting.Indented, jSetting);

            #region 记录发送日志

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.InputParameters.ToString(),
                ITF_req_parameters = postData,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_MercWithdraw.ToString()
            });

            #endregion 记录发送日志

            var result = HttpClientHelper.PostAsync(requestAddress, postData).Result.Content.ReadAsStringAsync().Result;

            #region 记录输出日志

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.OutputParameters.ToString(),
                ITF_ret_parameters = result,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_MercWithdraw.ToString()
            });

            #endregion 记录输出日志

            var jsReturn = JsonConvert.DeserializeObject<RBHMercWithdraw>(result);

            #region 操作返回信息
            return jsReturn;

            #endregion 操作返回信息
        }

        /// <summary>
        /// 交易状态查询
        /// </summary>
        /// <param name="model"></param>
        public RBHQueryTransStatBody QueryTransStat(SBHQueryTransStat model)
        {
            model.serviceName = InterfaceName.p_QueryTransStat.ToString();
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            var postData = JsonConvert.SerializeObject(model, Formatting.Indented, jSetting);

            #region 记录发送日志

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.InputParameters.ToString(),
                ITF_req_parameters = postData,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_QueryTransStat.ToString()
            });

            #endregion 记录发送日志

            var result = HttpClientHelper.PostAsync(requestAddress, postData).Result.Content.ReadAsStringAsync().Result;

            #region 记录输出日志

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.OutputParameters.ToString(),
                ITF_ret_parameters = result,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_QueryTransStat.ToString()
            });

            #endregion 记录输出日志

            var jsReturn = JsonConvert.DeserializeObject<RBHQueryTransStat>(result);
            if (jsReturn.RspSvcHeader.returnCode == JSReturnCode.Success)
            {
                return jsReturn.SvcBody;
            }
            return null;
        }



        /// <summary>
        /// 商户账户交易查询
        /// </summary>
        /// <param name="model"></param>
        public string QueryMerchantTrans(SBHQueryMerchantTrans model)
        {
            model.serviceName = InterfaceName.p_QueryMerchantTrans.ToString();
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            var postData = JsonConvert.SerializeObject(model, Formatting.Indented, jSetting);

            #region 记录发送日志

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.InputParameters.ToString(),
                ITF_req_parameters = postData,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_QueryMerchantTrans.ToString()
            });

            #endregion 记录发送日志

            var result = HttpClientHelper.PostAsync(requestAddress, postData).Result.Content.ReadAsStringAsync().Result;

            #region 记录输出日志

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.OutputParameters.ToString(),
                ITF_ret_parameters = result,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_QueryMerchantTrans.ToString()
            });

            #endregion 记录输出日志

            var jsRenturn = JsonConvert.DeserializeObject<RBHQueryMerchantTrans>(result);
            if (jsRenturn.RspSvcHeader.returnCode == JSReturnCode.Success.ToString())
            {
                return jsRenturn.SvcBody.fileName;
            }
            else
            {
                return "";
            }
        }

        public string ToUserMoney(SBHExperBonus model,string investId)
        {
            var redInfo = ExperBonus(model);
            if (redInfo.RspSvcHeader.returnCode == JSReturnCode.Success.ToString())
            {
                //保存红包流水号
                var redTrans = new CST_transaction_info
                {
                    TrxId = model.SvcBody.campaignCode,
                    DepoBankSeq = redInfo.SvcBody.transId,
                    pro_transaction_money = Convert.ToDecimal(model.SvcBody.campaignMoney),
                    pro_transaction_no = model.ReqSvcHeader.tranSeqNo,
                    pro_transaction_time = DateTime.Now,
                    pro_transaction_type = DataDictionary.transactiontype_CostReturn,
                    pro_user_id = Convert.ToInt32(CommonHelper.SurplusPlatFormId(investId)),
                    pro_user_type = 9,
                    pro_transaction_remarks = "发放红包",
                    pro_transaction_status = DataDictionary.transactionstatus_success,
                    MarketingInformation = model.SvcBody.campaignInfo
                };
                _transactionService.Add(redTrans);
                //如果红包发放成功直接发放至用户账户
                var redModel = new SBHCampaignTransfer
                {
                    SvcBody =
                    {
                        platformUid = investId,
                        amount = model.SvcBody.campaignMoney,
                        comment = model.SvcBody.extension
                    }
                };
                var result = CampaignTransfer(redModel);
                if (result.RspSvcHeader.returnCode == JSReturnCode.Success.ToString())
                {
                    //成功红包记录添加
                    var redTrans2 = new CST_transaction_info
                    {
                        TrxId = model.SvcBody.campaignCode,
                        pro_transaction_money = Convert.ToDecimal(model.SvcBody.campaignMoney),
                        pro_transaction_no = redModel.ReqSvcHeader.tranSeqNo,
                        pro_transaction_time = DateTime.Now,
                        pro_transaction_type = DataDictionary.transactiontype_CostReturn,
                        pro_user_id = Convert.ToInt32(CommonHelper.SurplusPlatFormId(investId)),
                        pro_user_type = 9,
                        pro_transaction_remarks = "费用退回",
                        pro_transaction_status = DataDictionary.transactionstatus_success,
                        MarketingInformation = model.SvcBody.campaignInfo
                    };
                    _transactionService.Add(redTrans2);
                    return "费用退回成功";
                }
                else
                {
                    //成功红包记录添加
                    var redTrans2 = new CST_transaction_info
                    {
                        TrxId = model.SvcBody.campaignCode,
                        pro_transaction_money = Convert.ToDecimal(model.SvcBody.campaignMoney),
                        pro_transaction_no = redModel.ReqSvcHeader.tranSeqNo,
                        pro_transaction_time = DateTime.Now,
                        pro_transaction_type = DataDictionary.transactiontype_CostReturn,
                        pro_user_id = Convert.ToInt32(CommonHelper.SurplusPlatFormId(investId)),
                        pro_user_type = 9,
                        pro_transaction_remarks = result.RspSvcHeader.BusinessMsg,
                        pro_transaction_status = DataDictionary.transactionstatus_failed,
                        MarketingInformation = model.SvcBody.campaignInfo
                    };
                    _transactionService.Add(redTrans2);
                    return "费用退回失败但红包发送成功";
                }
            }
            else
            {
                return "发送红包失败";
            }
        }

        public RBHMercRecharge MercRecharge(SBHMercRecharge model)
        {
            model.serviceName = InterfaceName.p_MercRecharge.ToString();
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            var postData = JsonConvert.SerializeObject(model, Formatting.Indented, jSetting);

            #region 记录发送日志

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.InputParameters.ToString(),
                ITF_req_parameters = postData,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_MercRecharge.ToString()
            });

            #endregion 记录发送日志

            var result = HttpClientHelper.PostAsync(requestAddress, postData).Result.Content.ReadAsStringAsync().Result;

            #region 记录输出日志

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.OutputParameters.ToString(),
                ITF_ret_parameters = result,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_MercRecharge.ToString()
            });

            #endregion 记录输出日志

            var jsReturn = JsonConvert.DeserializeObject<RBHMercRecharge>(result);
            return jsReturn;
        }

        public RBHQueryChargeAccountResult QueryChargeAccountResult(SBHQueryChargeAccountResult model)
        {
            model.serviceName = InterfaceName.p_QueryChargeAccountResult.ToString();
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            var postData = JsonConvert.SerializeObject(model, Formatting.Indented, jSetting);

            #region 记录发送日志

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.InputParameters.ToString(),
                ITF_req_parameters = postData,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_QueryChargeAccountResult.ToString()
            });

            #endregion 记录发送日志

            var result = HttpClientHelper.PostAsync(requestAddress, postData).Result.Content.ReadAsStringAsync().Result;

            #region 记录输出日志

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.OutputParameters.ToString(),
                ITF_ret_parameters = result,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_QueryChargeAccountResult.ToString()
            });

            #endregion 记录输出日志

            var jsReturn = JsonConvert.DeserializeObject<RBHQueryChargeAccountResult>(result);
            return jsReturn;
        }

        /// <summary>
        /// 开设大额充值户（废弃接口）
        /// </summary>
        /// <param name="model"></param>
        //public string OpenChargeAccount(SBHOpenChargeAccount model)
        //{
        //    model.serviceName = InterfaceName.p_OpenChargeAccount.ToString();
        //    var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
        //    var postData = JsonConvert.SerializeObject(model, Formatting.Indented, jSetting);

        //    #region 记录发送日志

        //    _rabbitMQEvent.Publish(new SYS_Interface_Info
        //    {
        //        ITF_info_type = LogsEnum.InputParameters.ToString(),
        //        ITF_req_parameters = postData,
        //        ITF_Info_addtime = DateTime.Now,
        //        cmdid = InterfaceName.p_OpenChargeAccount.ToString()
        //    });

        //    #endregion 记录发送日志

        //    var result = HttpClientHelper.PostAsync(requestAddress, postData).Result.Content.ReadAsStringAsync().Result;

        //    #region 记录输出日志

        //    _rabbitMQEvent.Publish(new SYS_Interface_Info
        //    {
        //        ITF_info_type = LogsEnum.OutputParameters.ToString(),
        //        ITF_ret_parameters = result,
        //        ITF_Info_addtime = DateTime.Now,
        //        cmdid = InterfaceName.p_OpenChargeAccount.ToString()
        //    });

        //    #endregion 记录输出日志

        //    var jsReturn = JsonConvert.DeserializeObject<RBHOpenChargeAccount>(result);

        //    #region 操作返回信息

        //    if (jsReturn.RspSvcHeader.returnCode == JSReturnCode.Success.ToString())
        //    {
        //        //注册中心大额账户开设成功
        //        var chargeAccount = _companyInfoService.GetChargeAccount(accountName: jsReturn.SvcBody.accountName);
        //        if (chargeAccount != null)
        //        {
        //            chargeAccount.Success = true;
        //            chargeAccount.ChargeAccountNo = jsReturn.SvcBody.chargeAccount;
        //            _companyInfoService.UpdateChargeAccount(chargeAccount);
        //            return "开设大额充值户成功";
        //        }
        //        return "未找到匹配的大额充值户";
        //    }

        //    #endregion 操作返回信息

        //    return "开设大额充值户失败：" + jsReturn.RspSvcHeader.BusinessMsg;
        //}

        /// <summary>
        /// 大额充值账号查询
        /// </summary>
        /// <param name="model"></param>
        public RBHQueryChargeAccountBody QueryChargeAccount(SBHQueryChargeAccount model)
        {
            model.serviceName = InterfaceName.p_QueryChargeAccount.ToString();
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            var postData = JsonConvert.SerializeObject(model, Formatting.Indented, jSetting);

            #region 记录发送日志

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.InputParameters.ToString(),
                ITF_req_parameters = postData,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_QueryChargeAccount.ToString()
            });

            #endregion 记录发送日志

            var result = HttpClientHelper.PostAsync(requestAddress, postData).Result.Content.ReadAsStringAsync().Result;

            #region 记录输出日志

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.OutputParameters.ToString(),
                ITF_ret_parameters = result,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_QueryChargeAccount.ToString()
            });

            #endregion 记录输出日志

            var jsReturn = JsonConvert.DeserializeObject<RBHQueryChargeAccount>(result);

            #region 操作返回信息

            if (jsReturn.RspSvcHeader.returnCode == JSReturnCode.Success.ToString())
            {
                //查询大额充值户信息成功
                return jsReturn.SvcBody;
            }

            #endregion 操作返回信息

            return null;
        }

        /// <summary>
        /// 大额充值记录查询
        /// </summary>
        /// <param name="model"></param>
        public RBHQueryChargeDetailBody QueryChargeDetail(SBHQueryChargeDetail model)
        {
            model.serviceName = InterfaceName.p_QueryChargeDetail.ToString();
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            var postData = JsonConvert.SerializeObject(model, Formatting.Indented, jSetting);

            #region 记录发送日志

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.InputParameters.ToString(),
                ITF_req_parameters = postData,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_QueryChargeDetail.ToString()
            });

            #endregion 记录发送日志

            var result = HttpClientHelper.PostAsync(requestAddress, postData).Result.Content.ReadAsStringAsync().Result;

            #region 记录输出日志

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.OutputParameters.ToString(),
                ITF_ret_parameters = result,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_QueryChargeDetail.ToString()
            });

            #endregion 记录输出日志

            var jsReturn = JsonConvert.DeserializeObject<RBHQueryChargeDetail>(result);

            #region 操作返回信息

            if (jsReturn.RspSvcHeader.returnCode == JSReturnCode.Success.ToString())
            {
                //查询大额充值户信息成功
                return jsReturn.SvcBody;
            }

            #endregion 操作返回信息

            return null;
        }

        public string MeUserTransfer(SBHExperBonus model)
        {
            throw new NotImplementedException();
        }

        public RBHOpenChargeAccountWeb OpenChargeAccount(SBHOpenChargeAccount model)
        {
            model.serviceName = InterfaceName.p_OpenChargeAccount.ToString();
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            var firstPost = JsonConvert.SerializeObject(model, Formatting.Indented, jSetting);

            #region 记录发送日志

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.InputParameters.ToString(),
                ITF_req_parameters = firstPost,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_OpenChargeAccount.ToString()
            });

            #endregion 记录发送日志

            var result = HttpClientHelper.PostAsync(requestAddress, firstPost).Result.Content.ReadAsStringAsync().Result;

            #region 记录输出日志

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.OutputParameters.ToString(),
                ITF_ret_parameters = result,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_OpenChargeAccount.ToString()
            });

            #endregion 记录输出日志

            var jsReturn = JsonConvert.DeserializeObject<RBHOpenChargeAccountWeb>(result);
            return jsReturn;
            //if (jsReturn.RspSvcHeader.returnCode != JSReturnCode.Success.ToString())
            //    return null;
            //return jsReturn.SvcBody;
        }
        /// <summary>
        /// 授权
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public RBHAutoInvestAuthWebBody AutoInvestAuth(SBHAutoInvestAuth model)
        {
            model.serviceName = InterfaceName.p_AutoInvestAuth.ToString();
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            var firstPost = JsonConvert.SerializeObject(model, Formatting.Indented, jSetting);

            #region 记录发送日志

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.InputParameters.ToString(),
                ITF_req_parameters = firstPost,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
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
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_AutoInvestAuth.ToString()
            });

            #endregion 记录输出日志

            var jsReturn = JsonConvert.DeserializeObject<RBHAutoInvestAuthWeb>(result);
            if (jsReturn.RspSvcHeader.returnCode != JSReturnCode.Success.ToString())
                return null;
            return jsReturn.SvcBody;
        }


        public RBHQueryAuthInf AuthResult(SBHQueryAuthInf model)
        {
            model.serviceName = InterfaceName.p_QueryAuthInf.ToString();
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            var firstPost = JsonConvert.SerializeObject(model, Formatting.Indented, jSetting);

            #region 记录发送日志

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.InputParameters.ToString(),
                ITF_req_parameters = firstPost,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_QueryAuthInf.ToString()
            });

            #endregion 记录发送日志

            var result = HttpClientHelper.PostAsync(requestAddress, firstPost).Result.Content.ReadAsStringAsync().Result;

            #region 记录输出日志

            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.OutputParameters.ToString(),
                ITF_ret_parameters = result,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = InterfaceName.p_QueryAuthInf.ToString()
            });

            #endregion 记录输出日志

            var jsReturn = JsonConvert.DeserializeObject<RBHQueryAuthInf>(result);
            if (jsReturn.RspSvcHeader.returnCode != JSReturnCode.Success.ToString())
                return null;
            return jsReturn;
        }

        public bool IsAuth(string pluCustId, AuthTyp type)
        {
            var requestModel = new SBHQueryAuthInf { SvcBody = { plaCustId = pluCustId } };
            var result = AuthResult(requestModel);
            if (result == null || !result.RspSvcHeader.returnCode.Equals(JSReturnCode.Success))
            {
                return false;
            }
            switch (type)
            {
                case AuthTyp.Invest:
                    var authInfo = result.SvcBody.items.FirstOrDefault(p => p.auth_typ == "11");
                    if (authInfo != null)
                    {
                        if (CommonHelper.HandleStringTime(authInfo.end_dt) > DateTime.Now)
                            return true;
                    }
                    break;
                case AuthTyp.Payment:
                    var authInfo2 = result.SvcBody.items.FirstOrDefault(p => p.auth_typ == "59");
                    if (authInfo2 != null)
                    {
                        if (CommonHelper.HandleStringTime(authInfo2.end_dt) > DateTime.Now)
                            return true;
                    }
                    break;
                case AuthTyp.Repayment:
                    var authInfo3 = result.SvcBody.items.FirstOrDefault(p => p.auth_typ == "60");
                    if (authInfo3 != null)
                    {
                        if (CommonHelper.HandleStringTime(authInfo3.end_dt) > DateTime.Now)
                            return true;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            return false;
        }

        public RUserAuth AuthInfo(string pluCustId, AuthTyp type)
        {
            var returnResult = new RUserAuth();
            var requestModel = new SBHQueryAuthInf { SvcBody = { plaCustId = pluCustId } };
            var result = AuthResult(requestModel);
            if (result == null || !result.RspSvcHeader.returnCode.Equals(JSReturnCode.Success))
            {
                returnResult.IsAuth = "0";
                return returnResult;
            }
            switch (type)
            {
                case AuthTyp.Invest:
                    var authInfo = result.SvcBody.items.FirstOrDefault(p => p.auth_typ == "11");
                    if (authInfo != null)
                    {
                        if (CommonHelper.HandleStringTime(authInfo.end_dt) > DateTime.Now)
                        {
                            returnResult.AuthMoney = (Convert.ToDecimal(authInfo.auth_amt) / 100).ToString("0.00");
                            returnResult.IsAuth = "1";
                            return returnResult;
                        }
                    }
                    break;
                case AuthTyp.Payment:
                    var authInfo2 = result.SvcBody.items.FirstOrDefault(p => p.auth_typ == "59");
                    if (authInfo2 != null)
                    {
                        if (CommonHelper.HandleStringTime(authInfo2.end_dt) > DateTime.Now)
                        {
                            returnResult.AuthMoney = (Convert.ToDecimal(authInfo2.auth_amt) / 100).ToString("0.00");
                            returnResult.IsAuth = "1";
                            return returnResult;
                        }
                    }
                    break;
                case AuthTyp.Repayment:
                    var authInfo3 = result.SvcBody.items.FirstOrDefault(p => p.auth_typ == "60");
                    if (authInfo3 != null)
                    {
                        if (CommonHelper.HandleStringTime(authInfo3.end_dt) > DateTime.Now)
                        {
                            returnResult.AuthMoney = (Convert.ToDecimal(authInfo3.auth_amt) / 100).ToString("0.00");
                            returnResult.IsAuth = "1";
                            return returnResult;
                        }
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            returnResult.IsAuth = "0";
            return returnResult;
        }
    }
}