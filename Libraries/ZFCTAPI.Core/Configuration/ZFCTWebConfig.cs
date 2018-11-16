using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Core.Configuration
{
    public class ZfctWebConfig
    {
        public string Name { get; set; }

        public string Desc { get; set; }

        public string Code { get; set; }

        public string LocalUrl { get; set; }

        public string PcUrl { get; set; }

        public string WeChatUrl { get; set; }

        public List<WebUrl> WebUrl { get; set; }
        /// <summary>
        /// 设置本机IP地址
        /// </summary>
        public string IPAddress { get; set; }
    }

    public class WebUrl
    {
        public string Name { get; set; }

        public string ReturnUrl { get; set; }

        public string PcReturnUrl { get; set; }

        public string WeChatReturnUrl { get; set; }

        public string NoticeUrl { get; set; }
    }
}