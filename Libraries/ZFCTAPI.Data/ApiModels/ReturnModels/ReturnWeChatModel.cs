using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.ApiModels.ReturnModels
{
    /// <summary>
    /// 微信分享签名
    /// </summary>
    public class RJssdkInfo
    {
        public decimal reward_month { get; set; }
        public decimal reward_all { get; set; }
        public string invitation_code { get; set; }
        public string jsapi_ticket { get; set; }
        public string noncestr { get; set; }
        public string timestamp { get; set; }
        public string url { get; set; }
        public string signature { get; set; }
        public string appId { get; set; }
    }

    /// <summary>
    /// 微信推送配置
    /// </summary>
    public class RWxPushConfig
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public bool Enable { get; set; }
    }
}