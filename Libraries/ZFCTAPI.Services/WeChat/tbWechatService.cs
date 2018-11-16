using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZFCTAPI.Core;
using ZFCTAPI.Core.Configuration;
using ZFCTAPI.Core.Helpers;
using ZFCTAPI.Core.Infrastructure;
using ZFCTAPI.Data.ApiModels.ReturnModels;
using ZFCTAPI.Data.CST;
using ZFCTAPI.Data.Pages;
using ZFCTAPI.Data.SYS;
using ZFCTAPI.Data.WeChat;
using ZFCTAPI.Services.BoHai;
using ZFCTAPI.Services.InvestInfo;
using ZFCTAPI.Services.LoanInfo;
using ZFCTAPI.Services.Messages;
using ZFCTAPI.Services.Repositorys;
using ZFCTAPI.Services.UserInfo;
using ZFCTAPI.Services.WeChat;

namespace ZFCTAPI.Services.Sys
{
    public interface ItbWechatService : IRepository<tbWechat>
    {
        /// <summary>
        /// 微信js签名
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        JssdkInfoReturn GetSignature(string url);

        /// <summary>
        /// 发送充值消息
        /// </summary>
        void SendReChargeNotice(CST_user_info userInfo, IDictionary<string, MessageData> data);

        /// <summary>
        /// 发送提现消息
        /// </summary>
        void SendWithdrawalsNotice(CST_user_info userInfo, IDictionary<string, MessageData> data);

        /// <summary>
        /// 发送还款消息
        /// </summary>
        /// <param name="userInfos"></param>
        void SendPaymentNotice(int loanPlanId);
    }

    public class tbWechatService : Repository<tbWechat>, ItbWechatService
    {
        private readonly IWeChatAlternately _weChatAlternately;
        private readonly WeChatConfig _weChatConfig;
        private readonly ISYSUserWechatService _sYSUserWechatService;
        private readonly IAccountInfoService _accountInfoService;
        private readonly IWeChatPushTemplateService _weChatPushTemplateService;
        private readonly IWeChatPushSwitchService _weChatPushSwitchService;
        private readonly IQueueWeChatMsgService _queueWeChatMsgService;

        public tbWechatService(IWeChatAlternately weChatAlternately, 
            WeChatConfig weChatConfig,
            ISYSUserWechatService sYSUserWechatService,
            IAccountInfoService accountInfoService,
            IWeChatPushTemplateService weChatPushTemplateService,
            IWeChatPushSwitchService weChatPushSwitchService,
            IQueueWeChatMsgService queueWeChatMsgService)
        {
            _weChatAlternately = weChatAlternately;
            _weChatConfig = weChatConfig;
            _sYSUserWechatService = sYSUserWechatService;
            _accountInfoService = accountInfoService;
            _weChatPushTemplateService = weChatPushTemplateService;
            _weChatPushSwitchService = weChatPushSwitchService;
            _queueWeChatMsgService = queueWeChatMsgService;

        }

        public JssdkInfoReturn GetSignature(string url)
        {
            var tbWeChat = _Conn.QueryFirstOrDefault($"select * from tbWeChat where AppID = '{_weChatConfig.WeixinAppId}'");

            long timeStamp = CommonHelper.GenerateTimeStamp();
            //验证access_token是否过期
            if (Convert.ToInt64(tbWeChat.expires_in) < timeStamp)
            {
                var accessTokenReturn = _weChatAlternately.GetAccessToken();
                tbWeChat.access_token = accessTokenReturn.access_token;
                tbWeChat.expires_in = (accessTokenReturn.expires_in + timeStamp).ToString();
                Update(tbWeChat);
            }

            //验证jsapi_ticket是否过期
            if (Convert.ToInt64(tbWeChat.jsapi_ticket_expires_in) < timeStamp)
            {
                var jsapi_ticketReturn = _weChatAlternately.GetJsApiTicket(tbWeChat.access_token);
                tbWeChat.jsapi_ticket = jsapi_ticketReturn.ticket;
                tbWeChat.jsapi_ticket_expires_in = (jsapi_ticketReturn.expires_in + timeStamp).ToString();
                Update(tbWeChat);
            }

            string jsapi_ticket = tbWeChat.jsapi_ticket;
            string noncestr = CommonHelper.CreateNonceStr(36);
            string timestamp = timeStamp.ToString();
            string str = "jsapi_ticket=" + jsapi_ticket + "&noncestr=" + noncestr + "&timestamp=" + timestamp + "&url=" + url;
            string signature = CommonHelper.SHA1(str);

            JssdkInfoReturn jssdkInfoReturn = new JssdkInfoReturn()
            {
                jsapi_ticket = jsapi_ticket,
                noncestr = noncestr,
                timestamp = timestamp,
                url = url,
                signature = signature,
                appId = tbWeChat.AppID
            };

            return jssdkInfoReturn;
        }

        public void SendReChargeNotice(CST_user_info userInfo, IDictionary<string, MessageData> data) {
            if (userInfo == null)
                return;
            //是否绑定微信
            var WeChatUserInfo = _sYSUserWechatService.GetByCustomerId(userInfo.cst_customer_id.ToString());
            if (WeChatUserInfo == null || string.IsNullOrEmpty(WeChatUserInfo.cst_user_uopenid))
                return;

            //获取模板
            var template = _weChatPushTemplateService.GetByType("WeChatPush.RechargeSuccess");
            if (template == null || string.IsNullOrEmpty(template.template_id))
                return;

            var mySwitch = _weChatPushSwitchService.GetByUserIdAndType(userInfo.cst_customer_id.ToString(), template.type);
            if (mySwitch != null)//是否设置开启
            {
                if (!mySwitch.enable)
                {
                    return;
                }
            }
            else if (!template.default_enabled)//是否默认开启
            {
                return;
            }

            //拼装请求模型
            var userRealInfo = _accountInfoService.GetRealNameInfo(userInfo.Id);
            var body = new WeChatTemplatecsMsg {
                  template_id = template.template_id,
                touser = WeChatUserInfo.cst_user_uopenid
            };
          
            var realname = "客户";
            var sex = "";
            if (userRealInfo != null)
            {
                if (!userRealInfo.cst_user_sex)
                    sex = "女士";
                else
                    sex = "先生";
                realname = userRealInfo.cst_user_realname;
            }

            data.Add("first", new MessageData
            {
                value = string.Format("尊敬的{0}{1}:\n\n您有一条交易信息", realname, sex)
            });
            data.Add("remark", new MessageData
            {
                value = template.remark.Replace("<br>", "\n")
            });
            body.data = data;
            if (!string.IsNullOrEmpty(template.url))
                body.url = template.url;

            if (!string.IsNullOrEmpty(template.appid))
            {
                body.miniprogram = new WeChatMiniProgram
                {
                    appid = template.appid,
                    pagepath = template.pagepath
                };
            }

            //插入发送队列
            _queueWeChatMsgService.Add(new Data.Message.QueueWeChatMsg
            {
                ToUserId = userInfo.Id,
                MessageId = template.Id,
                Body = JsonConvert.SerializeObject(body),
                CreatedOnUtc = DateTime.Now,
                SentTries = 0,
                SendResult = false
            });
           
        }

        public void SendWithdrawalsNotice(CST_user_info userInfo, IDictionary<string, MessageData> data) {
            if (userInfo == null)
                return;
            //是否绑定微信
            var WeChatUserInfo = _sYSUserWechatService.GetByCustomerId(userInfo.cst_customer_id.ToString());
            if (WeChatUserInfo == null || string.IsNullOrEmpty(WeChatUserInfo.cst_user_uopenid))
                return;

            //获取模板
            var template = _weChatPushTemplateService.GetByType("WeChatPush.WithdrawalsSuccess");
            if (template == null || string.IsNullOrEmpty(template.template_id))
                return;

            //是否开启推送
            var mySwitch = _weChatPushSwitchService.GetByUserIdAndType(userInfo.cst_customer_id.ToString(), template.type);
            if (mySwitch != null) {
                if (!mySwitch.enable)
                {
                    return;
                }
            }
            else if (!template.default_enabled)
            {
                return;
            }

            //拼装请求模型
            var userRealInfo = _accountInfoService.GetRealNameInfo(userInfo.Id);
            var body = new WeChatTemplatecsMsg
            {
                template_id = template.template_id,
                touser = WeChatUserInfo.cst_user_uopenid
            };

            var realname = "客户";
            var sex = "";
            if (userRealInfo != null)
            {
                if (!userRealInfo.cst_user_sex)
                    sex = "女士";
                else
                    sex = "先生";
                realname = userRealInfo.cst_user_realname;
            }

            data.Add("first", new MessageData
            {
                value = string.Format("尊敬的{0}{1}:\n\n您有一条交易信息", realname, sex)
            });
            data.Add("remark", new MessageData
            {
                value = template.remark.Replace("<br>", "\n")
            });
            body.data = data;
            if (!string.IsNullOrEmpty(template.url))
                body.url = template.url;

            if (!string.IsNullOrEmpty(template.appid))
            {
                body.miniprogram = new WeChatMiniProgram
                {
                    appid = template.appid,
                    pagepath = template.pagepath
                };
            }

            //插入发送队列
            _queueWeChatMsgService.Add(new Data.Message.QueueWeChatMsg
            {
                ToUserId = userInfo.Id,
                MessageId = template.Id,
                Body = JsonConvert.SerializeObject(body),
                CreatedOnUtc = DateTime.Now,
                SentTries = 0,
                SendResult = false
            });

        }

        public void SendPaymentNotice(int loanId)
        {
            Task.Run(()=> {
                //注入
                var t_sysUserWeChatService = EngineContext.Current.Resolve<ISYSUserWechatService>();
                var t_weChatMessageTemplateService = EngineContext.Current.Resolve<IWeChatPushTemplateService>();
                var t_weChatPushSwitchService = EngineContext.Current.Resolve<IWeChatPushSwitchService>();
                var t_queueWeChatMsgService = EngineContext.Current.Resolve<IQueueWeChatMsgService>();
                var t_investPlanService = EngineContext.Current.Resolve<IInvesterPlanService>();
                var t_bhaccountService = EngineContext.Current.Resolve<IBHAccountService>();
                var t_loaninfoService = EngineContext.Current.Resolve<ILoanInfoService>();


                // 是否存在模板
                var template = t_weChatMessageTemplateService.GetByType("WeChatPush.PaymentSuccess");
                if (template == null || string.IsNullOrEmpty(template.template_id))
                    return;

                //获得所有投资户
                var investers = t_loaninfoService.GetInvesterByLoanId(loanId);
                if (investers != null && investers.Count>0) {
                    foreach (var userInfo in investers) {
                        //是否绑定微信
                        var WeChatUserInfo = _sYSUserWechatService.GetByCustomerId(userInfo.cst_customer_id.ToString());
                        if (WeChatUserInfo == null || string.IsNullOrEmpty(WeChatUserInfo.cst_user_uopenid))
                            continue;

                        //是否开启推送
                        var mySwitch = _weChatPushSwitchService.GetByUserIdAndType(userInfo.cst_customer_id.ToString(), template.type);
                        if (mySwitch != null)
                        {
                            if (!mySwitch.enable)
                            {
                                continue;
                            }
                        }
                        else if (!template.default_enabled)
                        {
                            continue;
                        }

                        //获得当日所有回款
                        var investPlans = t_investPlanService.GetCurrentDayPlan(userInfo.Id);
                        if(investPlans != null && investPlans.Count >0)
                        {
                            var body = new WeChatTemplatecsMsg
                            {
                                template_id = template.template_id,
                                touser = WeChatUserInfo.cst_user_uopenid
                            };
                            var userRealInfo = _accountInfoService.GetRealNameInfo(userInfo.Id);
                            var realname = "客户";
                            var sex = "";
                            if (userRealInfo != null)
                            {
                                if (!userRealInfo.cst_user_sex)
                                    sex = "女士";
                                else
                                    sex = "先生";
                                realname = userRealInfo.cst_user_realname?? "客户";
                            }


                            //获得余额
                            var accountInfo = _accountInfoService.GetAccountInfoByUserId(userInfo.Id);
                            if (accountInfo == null)
                                continue;
                            //调用渤海接口
                            var availableBalance = 0M;
                            var rBHAccountQueryAccBalance = t_bhaccountService.AccountQueryAccBalance(new Data.BoHai.SubmitModels.SBHAccountQueryAccBalance
                            {
                                SvcBody = new Data.BoHai.SubmitModels.SBHAccountQueryAccBalanceBody
                                {
                                    platformUid = "1"
                                }
                            });
                            if (rBHAccountQueryAccBalance.RspSvcHeader.returnCode.Equals(Core.Enums.JSReturnCode.Success))
                            {
                                availableBalance = Convert.ToDecimal((string.IsNullOrEmpty(rBHAccountQueryAccBalance.SvcBody.withdrawAmount)) ? "0" : rBHAccountQueryAccBalance.SvcBody.withdrawAmount);
                            }

                            //拼接模板
                            var data = new Dictionary<string, MessageData>();
                            data.Add("keyword1", new MessageData
                            {
                                value = "+" + string.Format("{0:N}", investPlans.Sum(p => p.pro_collect_total.GetValueOrDefault())),
                                color = "#FF0000"
                            });
                            data.Add("keyword2", new MessageData { value = investPlans.Count().ToString() });
                            data.Add("keyword3", new MessageData { value = DateTime.Now.ToString("yyyy-MM-dd") });
                            data.Add("keyword4", new MessageData
                            {
                                value = string.Format("{0:N}", availableBalance),
                                color = "#FF0000"
                            });
                            data.Add("first", new MessageData
                            {
                                value = string.Format("尊敬的{0}{1}:\n\n您今天的回款信息", realname, sex)
                            });
                            data.Add("remark", new MessageData
                            {
                                value = template.remark.Replace("<br>", "\n")
                            });
                            body.data = data;
                            if (!string.IsNullOrEmpty(template.url))
                                body.url = template.url;

                            if (!string.IsNullOrEmpty(template.appid))
                            {
                                body.miniprogram = new WeChatMiniProgram
                                {
                                    appid = template.appid,
                                    pagepath = template.pagepath
                                };
                            }

                            t_queueWeChatMsgService.Add(new Data.Message.QueueWeChatMsg
                            {
                                ToUserId = userInfo.Id,
                                MessageId = template.Id,
                                Body = JsonConvert.SerializeObject(body),
                                CreatedOnUtc = DateTime.Now,
                                SentTries = 0,
                                SendResult = false
                            });
                        }
                    }
                }
            });
        }
    }
}