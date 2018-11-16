using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFCTAPI.Core.Configuration
{
    public class ZfctWebEngineToConfiguration
    {
        public static string GetWebReturnUrl(string key)
        {
            var local = GetLoaclUrl();
            var result = ZfctWebEngine.Instance().WebUrl.First(p => p.Name == key).ReturnUrl;
            return local+result;
        }

        public static string GetWebNoticeUrl(string key)
        {
            var local = GetLoaclUrl();
            var result = ZfctWebEngine.Instance().WebUrl.First(p => p.Name == key).NoticeUrl;
            return local + result;
        }

        public static string GetLoaclUrl()
        {
            var result = ZfctWebEngine.Instance().LocalUrl;
            return result;
        }

        /// <summary>
        /// 获取本机IP地址报送给江苏结算
        /// </summary>
        /// <returns></returns>
        public static string GetIPAddress()
        {
            var result = ZfctWebEngine.Instance().IPAddress;
            return result;
        }
    }
}
