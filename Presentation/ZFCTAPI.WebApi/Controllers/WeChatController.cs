using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZFCTAPI.Data.ApiModels.ReturnModels;
using ZFCTAPI.WebApi.Validates;
using ZFCTAPI.Core.Enums;
using ZFCTAPI.Data.ApiModels.SubmitModels;
using ZFCTAPI.Services.WeChat;
using ZFCTAPI.Data.WeChat;
using ZFCTAPI.Services.Sys;
using ZFCTAPI.Data.SYS;
using ZFCTAPI.Data.CST;
using ZFCTAPI.Services.UserInfo;
using ZFCTAPI.Services.Promotion;
using ZFCTAPI.WebApi.RequestAttribute;

namespace ZFCTAPI.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [RequestLog]
    public class WeChatController : Controller
    {
        private readonly IWeChatAlternately _weChatAlternately;
        private readonly ISYSUserWechatService _iSYSUserWechatService;
        private readonly ItbWechatService _tbWechatService;
        private readonly ICustomerService _customerService;
        private readonly IInvitationActivitie _invitationActivitie;
        private readonly IWeChatPushSwitchService _weChatPushSwitchService;
        private readonly IWeChatPushTemplateService _weChatPushTemplateService;

        public WeChatController(IWeChatAlternately weChatAlternately,
            ISYSUserWechatService iSYSUserWechatService,
            ItbWechatService tbWechatService,
            ICustomerService customerService,
            IInvitationActivitie invitationActivitie,
            IWeChatPushSwitchService weChatPushSwitchService,
            IWeChatPushTemplateService weChatPushTemplateService)
        {
            _weChatAlternately = weChatAlternately;
            _iSYSUserWechatService = iSYSUserWechatService;
            _tbWechatService = tbWechatService;
            _customerService = customerService;
            _invitationActivitie = invitationActivitie;
            _weChatPushSwitchService = weChatPushSwitchService;
            _weChatPushTemplateService = weChatPushTemplateService;
        }

        /// <summary>
        /// 微信授权
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RToPage, string> AuthorizedUrl([FromBody]SAuthorizedUrlModel model)
        {
            var retModel = new ReturnModel<RToPage, string>();

            #region 校验签名

            var signResult = VerifyBase.Sign<SAuthorizedUrlModel>(model);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名

            var returnModel = new RToPage
            {
                Url = _weChatAlternately.Oauth2_Code(model.RedirctUrl)
            };

            retModel.Message = "成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.ReturnData = returnModel;
            retModel.Token = model.Token;
            return retModel;
        }

        /// <summary>
        /// 微信绑定
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<bool, string> WeChatBindUser([FromBody]SAuthorizedCodeModel model)
        {
            var retModel = new ReturnModel<bool, string>();

            #region 校验签名

            var signResult = VerifyBase.SignAndToken<SAuthorizedCodeModel>(model, out CST_user_info userInfo);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = false;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名

            if (!string.IsNullOrEmpty(model.Code))
            {
                var oauth2_AccessTokenReturn = _weChatAlternately.Oauth2_AccessToken(model.Code);
                if (oauth2_AccessTokenReturn != null)
                {
                    oauth2_AccessTokenReturn = new Oauth2_AccessTokenReturn()
                    {
                        access_token = oauth2_AccessTokenReturn.access_token,
                        openid = oauth2_AccessTokenReturn.openid
                    };
                    var oauth2_UserInfoReturn = _weChatAlternately.Oauth2_UserInfo(oauth2_AccessTokenReturn.access_token, oauth2_AccessTokenReturn.openid);
                    if (oauth2_UserInfoReturn != null)
                    {
                        //添加微信用户
                        var weChatUser = _iSYSUserWechatService.GetByOpenId(oauth2_UserInfoReturn.openid);
                        if (weChatUser == null)
                        {
                            _iSYSUserWechatService.Add(new SYS_User_Wechat
                            {
                                cst_user_name = oauth2_UserInfoReturn.nickname,
                                cst_user_uopenid = oauth2_UserInfoReturn.openid,
                                cst_user_unionid = oauth2_UserInfoReturn.unionid,
                                cst_user_weurl = oauth2_UserInfoReturn.headimgurl
                            });
                        }

                        //重新关联
                        var weChatUser1 = _iSYSUserWechatService.GetByOpenId(oauth2_AccessTokenReturn.openid);
                        var weChatUser2 = _iSYSUserWechatService.GetByCustomerId(userInfo.cst_customer_id.ToString());
                        if (weChatUser1 != null && weChatUser1.cst_customer_id != userInfo.cst_customer_id)
                        {
                            //清除原先关联
                            if (weChatUser2 != null)
                            {
                                weChatUser2.cst_customer_id = null;
                                _iSYSUserWechatService.Update(weChatUser2);
                            }
                            weChatUser1.cst_customer_id = userInfo.cst_customer_id;
                            _iSYSUserWechatService.Update(weChatUser1);
                        }
                    }
                }
            }

            retModel.Message = "成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.ReturnData = true;
            retModel.Token = model.Token;
            return retModel;
        }

        /// <summary>
        /// 邀请奖励
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RJssdkInfo, string> WxShare([FromBody]SWxShare model)
        {
            var retModel = new ReturnModel<RJssdkInfo, string>();

            #region 校验签名

            var signResult = VerifyBase.SignAndToken<SWxShare>(model, out CST_user_info userInfo);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            //var jssdkInfoReturn = _tbWechatService.GetSignature(model.Url) ?? new JssdkInfoReturn();
            var jssdkInfoReturn = new JssdkInfoReturn();
            #endregion 校验签名

            var returnModel = new RJssdkInfo
            {
                jsapi_ticket = jssdkInfoReturn.jsapi_ticket,
                noncestr = jssdkInfoReturn.noncestr,
                timestamp = jssdkInfoReturn.timestamp,
                url = jssdkInfoReturn.url,
                signature = jssdkInfoReturn.signature,
                appId = jssdkInfoReturn.appId
            };
            returnModel.invitation_code = _customerService.Find(userInfo.cst_customer_id.Value).cst_invitation_code;
            var rewards = _invitationActivitie.GetListsByBeneficiaryId(userInfo.cst_customer_id.Value);
            returnModel.reward_month = rewards.Where(p => p.Year == DateTime.Now.Year && p.Months == DateTime.Now.Month).Sum(p => p.BeneficiaryAmount);
            returnModel.reward_all = rewards.Sum(p => p.BeneficiaryAmount);
            retModel.Message = "查询成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.ReturnData = returnModel;
            retModel.Token = model.Token;
            return retModel;
        }

        /// <summary>
        /// 提交微信推送配置
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<bool, string> PostWxPushConfig([FromBody]SWxPushConfigModel model)
        {
            var retModel = new ReturnModel<bool, string>();

            #region 校验签名

            var signResult = VerifyBase.SignAndToken<SWxPushConfigModel>(model, out CST_user_info userInfo);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = false;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名

            try
            {
                foreach (var configModel in model.Config)
                {
                    var config = _weChatPushSwitchService.GetByUserIdAndType(userInfo.cst_customer_id.ToString(), configModel.Type);
                    if (config == null)
                    {
                        _weChatPushSwitchService.Add(new WeChatPushSwitch
                        {
                            cst_customer_Id = userInfo.cst_customer_id.Value,
                            template_type = configModel.Type,
                            enable = configModel.Enable
                        });
                    }
                    else
                    {
                        config.enable = configModel.Enable;
                        _weChatPushSwitchService.Update(config);
                    }
                }
            }
            catch
            {
                retModel.ReturnData = false;
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.Message = "失败";
                retModel.Token = model.Token;
                return retModel;
            }

            retModel.Message = "成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.ReturnData = true;
            retModel.Token = model.Token;
            return retModel;
        }

        /// <summary>
        /// 获取微信推送配置
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<ReturnPageData<RWxPushConfig>, string> GetWxPushConfig([FromBody]BaseSubmitModel model)
        {
            var retModel = new ReturnModel<ReturnPageData<RWxPushConfig>, string>();

            #region 校验签名

            var signResult = VerifyBase.SignAndToken<BaseSubmitModel>(model, out CST_user_info userInfo);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名

            var configs = _weChatPushTemplateService.GetAll().Where(p => p.enabled == true).Select(p =>
            {
                var configModel = new RWxPushConfig();
                configModel.Name = p.name;
                configModel.Type = p.type;
                configModel.Enable = p.default_enabled;
                var config = _weChatPushSwitchService.GetByUserIdAndType(userInfo.cst_customer_id.ToString(), p.type);
                if (config != null)
                {
                    configModel.Enable = config.enable;
                }
                return configModel;
            });

            var returnModel = new ReturnPageData<RWxPushConfig>();
            returnModel.PagingData = configs;

            retModel.Message = "成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.ReturnData = returnModel;
            retModel.Token = model.Token;
            return retModel;
        }
    }
}