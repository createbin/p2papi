using System;
using System.Linq;

namespace ZFCTAPI.Core.Configuration
{
    public class ApiEngineToConfiguration
    {
        public static string GetAppSettingsUrl(string key)
        {
            var coinfo = BoHaiApiEngines.Instance();
            var action = coinfo.ApiAddress;
            var result = BoHaiApiEngines.Instance().Interfaces.First(p => p.Name == key).ActionUrl;
            return action + result;
        }

        public static string GetBoHaiApi()
        {
            BoHaiApiConfig coinfo = null;
            try
            {
                coinfo = BoHaiApiEngines.Instance();
            }
            catch
            {
                //ignore
            }
            return coinfo == null ? "http://180.96.49.211:58081/" : coinfo.Url;
        }
    }
}
