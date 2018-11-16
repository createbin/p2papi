using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Core.Configuration
{
    public class FtpConfig
    {
        /// <summary>
        /// ftp地址
        /// </summary>
        public string FtpAddress { get; set; }
        /// <summary>
        /// ftp用户
        /// </summary>
        public string FtpUser { get; set; }
        /// <summary>
        /// ftp服务器密码
        /// </summary>
        public string FtpPassword { get; set; }
    }
}
