using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.SubmitModels
{
    public class SBHAutoInvestAuth:SBHBaseModel
    {
        public SBHAutoInvestAuthBody SvcBody { get; set; }
    }

    public class SBHAutoInvestAuthBody
    {
        /// <summary>
        /// 客户端类型
        /// N
        /// 不填写默认为1
        /// 1：电脑客户端
        /// 2：移动客户端
        /// </summary>
        public string clientType { get; set; }
        /// <summary>
        /// 账户存管平台客户号
        /// Y
        /// 用户唯一标识
        /// </summary>
        public string plaCustId { get; set; }
        /// <summary>
        /// 1授權 2解授权
        /// </summary>
        public string txnTyp { get; set; }
        /// <summary>
        /// 账户类型
        /// </summary>
        public string transTyp { get; set; }
        /// <summary>
        /// 页面返回url
        /// Y
        /// </summary>
        public string pageReturnUrl { get; set; }
    }
}
