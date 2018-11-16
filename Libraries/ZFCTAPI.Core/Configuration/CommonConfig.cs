using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Core.Configuration
{
    public class CommonConfig
    {
        /// <summary>
        /// 是否为测试环境,true表示测试环境
        /// </summary>
        public string IsTest { get; set; }
        /// <summary>
        /// 是验证签名
        /// </summary>
        public string IsVerify { get; set; }
        /// <summary>
        /// 管理员手机号
        /// </summary>
        public string ManagerPhone { get; set; }
        /// <summary>
        /// 系统升级维护中，不允许登录
        /// </summary>
        public string SystemUpgrade { get; set; }
    }
}
