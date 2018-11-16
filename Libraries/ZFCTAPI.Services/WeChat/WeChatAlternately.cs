using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using ZFCTAPI.Core.Configuration;
using ZFCTAPI.Core.Helpers;
using ZFCTAPI.Data.WeChat;

namespace ZFCTAPI.Services.WeChat
{
    public interface IWeChatAlternately
    {
        string Oauth2_Code(string redirectUrl);

        Oauth2_AccessTokenReturn Oauth2_AccessToken(string code);

        Oauth2_UserInfoReturn Oauth2_UserInfo(string access_token, string openId);

        AccessTokenReturn GetAccessToken();

        jsapi_ticketReturn GetJsApiTicket(string access_token);
    }

    public class WeChatAlternately : IWeChatAlternately
    {
        private readonly WeChatConfig _weChatConfig;

        public WeChatAlternately(WeChatConfig weChatConfig)
        {
            _weChatConfig = weChatConfig;
        }

        public string Oauth2_Code(string redirectUrl)
        {
            return "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + _weChatConfig.WeixinAppId + "&redirect_uri=" + WebUtility.UrlEncode(redirectUrl) + "&response_type=code&scope=snsapi_userinfo&state=STATE#wechat_redirect";
        }

        /// <summary>
        /// 第二步 根据Code获取用户的access_token
        /// </summary>
        /// <param name="code"></param>
        public Oauth2_AccessTokenReturn Oauth2_AccessToken(string code)
        {
            //WriteLog("----------------------------------------------------------------------------------------------------");
            //WriteLog("【日志类型】：第二步 根据Code获取用户的access_token");
            //WriteLog("【开始时间】：" + DateTime.Now.ToString());
            //WriteLog("【日志内容】：" + JsonConvert.SerializeObject(oauth2_AccessTokenReturn));

            var oauth2_AccessTokenReturn = JsonConvert.DeserializeObject<Oauth2_AccessTokenReturn>(HttpClientHelper.GetAsync($"https://api.weixin.qq.com/sns/oauth2/access_token?appid={_weChatConfig.WeixinAppId}&secret={_weChatConfig.WeixinAppSecret}&code=code&grant_type=authorization_code"));
            return oauth2_AccessTokenReturn;
        }

        /// <summary>
        /// 第三步 根据用户的access_token、openid请求微信OAuth获取用户信息
        /// </summary>
        /// <param name="userAccessToken"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        public Oauth2_UserInfoReturn Oauth2_UserInfo(string access_token, string openId)
        {
            //WriteLog("----------------------------------------------------------------------------------------------------");
            //WriteLog("【日志类型】：第三步 根据用户的access_token、openid请求微信OAuth获取用户信息");
            //WriteLog("【开始时间】：" + DateTime.Now.ToString());
            //WriteLog("【请求地址】：" + "https://api.weixin.qq.com/sns/userinfo?access_token=" + access_token + "&openid=" + openId + "&lang=zh_CN");
            //WriteLog("【日志内容】：" + JsonConvert.SerializeObject(oauth2_UserInfoReturn));

            var oauth2_UserInfoReturn = JsonConvert.DeserializeObject<Oauth2_UserInfoReturn>(HttpClientHelper.GetAsync($"https://api.weixin.qq.com/sns/userinfo?access_token={access_token}&openid={openId}&lang=zh_CN"));
            return oauth2_UserInfoReturn;
        }

        /// <summary>
        /// 获取access_token
        /// </summary>
        /// <returns>access_token</returns>
        public AccessTokenReturn GetAccessToken()
        {
            var accessTokenApi = JsonConvert.DeserializeObject<AccessTokenReturn>(HttpClientHelper.GetAsync($"https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={_weChatConfig.WeixinAppId}&secret={_weChatConfig.WeixinAppSecret}"));
            return accessTokenApi;
        }

        /// <summary>
        /// 获取jsapi_ticket
        /// </summary>
        /// <returns>jsapi_ticket</returns>
        public jsapi_ticketReturn GetJsApiTicket(string access_token)
        {
            jsapi_ticketReturn jsapi_ticketReturn = JsonConvert.DeserializeObject<jsapi_ticketReturn>(HttpClientHelper.GetAsync($"https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={access_token}&type=jsapi"));
            return jsapi_ticketReturn;
        }
    }
}