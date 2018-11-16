using System;
using System.Collections.Generic;
using System.Text;
using ZFCTAPI.Core.Infrastructure;

namespace ZFCTAPI.Core.Configuration
{
    public class ZfctWebEngine
    {
        private static ZfctWebConfig _zfctWebConfig;

        public static ZfctWebConfig Instance()
        {
            if (_zfctWebConfig == null)
            {
                Initialize();
            }
            return _zfctWebConfig;
        }

        /// <summary>
        /// 解析json
        /// </summary>
        public static void Initialize()
        {
            _zfctWebConfig = EngineContext.Current.Resolve<ZfctWebConfig>();
        }
    }
}
