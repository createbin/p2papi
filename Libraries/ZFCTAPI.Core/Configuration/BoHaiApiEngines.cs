using System;
using ZFCTAPI.Core.Infrastructure;

namespace ZFCTAPI.Core.Configuration
{
    public class BoHaiApiEngines
    {
        private static BoHaiApiConfig _zfctApiConfig;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public static BoHaiApiConfig Instance()
        {
            if (_zfctApiConfig == null)
            {
                Initialize();
            }
            return _zfctApiConfig;
        }

        /// <summary>
        /// 解析json
        /// </summary>
        public static void Initialize()
        {
            _zfctApiConfig = null;
            try
            {
                _zfctApiConfig = EngineContext.Current.Resolve<BoHaiApiConfig>();
            }
            catch
            {
                // ignored
            }
        }
    }
}
