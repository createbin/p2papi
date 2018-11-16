using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.ApiModels.SubmitModels
{
    /// <summary>
    /// 微信授权
    /// </summary>
    public class SAuthorizedUrlModel : BaseSubmitModel
    {
        /// <summary>
        /// 通知页面
        /// </summary>
        public string RedirctUrl { get; set; }
    }

    /// <summary>
    /// 微信用户绑定
    /// </summary>
    public class SAuthorizedCodeModel : BaseSubmitModel
    {
        /// <summary>
        /// 授权码
        /// </summary>
        public string Code { get; set; }
    }

    /// <summary>
    /// 微信分享
    /// </summary>
    public class SWxShare : BaseSubmitModel
    {
        /// <summary>
        /// url
        /// </summary>
        public string Url { get; set; }
    }

    /// <summary>
    /// 提交微信推送配置
    /// </summary>
    public class SWxPushConfigModel : BaseSubmitModel
    {
        public List<PushConfig> Config { get; set; }
    }

    public class PushConfig
    {
        public string Type { get; set; }
        public bool Enable { get; set; }
    }
}