using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Core.Configuration
{
    /// <summary>
    /// 微信配置
    /// </summary>
    public class WeChatConfig
    {
        /// <summary>
        /// 微信APPID
        /// </summary>
        public string WeixinAppId { get; set; }

        /// <summary>
        /// 微信AppSecret
        /// </summary>
        public string WeixinAppSecret { get; set; }

        /// <summary>
        /// 微信AppToken
        /// </summary>
        public string WeixinAppToken { get; set; }
    }
}