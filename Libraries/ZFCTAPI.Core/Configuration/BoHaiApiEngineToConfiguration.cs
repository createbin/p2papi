using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Core.Configuration
{
    public class BoHaiApiEngineToConfiguration
    {
        /// <summary>
        /// 结算中心地址
        /// </summary>
        /// <returns></returns>
        public static string JSAddress()
        {
            var result = string.Empty;
            try
            {
                result = BoHaiApiEngines.Instance().ApiAddress.ToString();
            }
            catch
            {
                //ignore
            }

            if (string.IsNullOrEmpty(result))
            {
                result = "http://180.96.49.211:58081/";
            }
            return result;
        }
        /// <summary>
        /// 渤海地址
        /// </summary>
        /// <returns></returns>
        public static string BHAddress()
        {
            var result = string.Empty;
            try
            {
                result = BoHaiApiEngines.Instance().BoHaiAddress.ToString();
            }
            catch
            {
                //ignore
            }

            if (string.IsNullOrEmpty(result))
            {
                result = "http://221.239.93.141:9080/bhdep/hipos/payTransaction";
            }
            return result;
        }
        /// <summary>
        /// 渤海手机端地址
        /// </summary>
        /// <returns></returns>
        public static string BHMobileAddress()
        {
            var result = string.Empty;
            try
            {
                result = BoHaiApiEngines.Instance().BoHaiMobileAddress.ToString();
            }
            catch
            {
                //ignore
            }

            if (string.IsNullOrEmpty(result))
            {
                result = "http://mtest.cbhb.com.cn/pmobile227/static/index.html";
            }
            return result;
        }

        /// <summary>
        /// 清算系统分配给互金平台的系统编码
        /// </summary>
        /// <returns></returns>
        public static string ConsumerId()
        {
            var result = string.Empty;
            try
            {
                result = BoHaiApiEngines.Instance().ConsumerId.ToString();
            }
            catch 
            {
                //ignore
            }
            if (string.IsNullOrEmpty(result))
            {
                result = "3201021";
            }
            return result;
        }
        /// <summary>
        /// 银行分配给平台或者结算系统的唯一标识
        /// </summary>
        /// <returns></returns>
        public static string InstId()
        {
            var result = string.Empty;
            try
            {
                result = BoHaiApiEngines.Instance().InstId.ToString();
            }
            catch
            {
                //ignore
            }
            if (string.IsNullOrEmpty(result))
            {
                result = "800025000010003";
            }
            return result;
        }
        /// <summary>
        /// 银行分配给平台或者结算系统的唯一标识
        /// </summary>
        /// <returns></returns>
        public static string MerId()
        {
            var result = string.Empty;
            try
            {
                result = BoHaiApiEngines.Instance().MerId.ToString();
            }
            catch
            {
                //ignore
            }
            if (string.IsNullOrEmpty(result))
            {
                result = "800080290000001";
            }
            return result;
        }
    }
}
