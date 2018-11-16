using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.SubmitModels
{
    public class SBBCloseAccount : SBHBaseModel
    {
        public SBBCloseAccountBody SvcBody { get; set; }
    }

    public class SBBCloseAccountBody
    {
        /// <summary>
        /// 客户端类型
        /// 不填写默认为1
        /// 1：电脑客户端
        /// 2：移动客户端
        /// </summary>
        public string clientType { get; set; }
        /// <summary>
        /// 用户平台编码
        /// </summary>
        public string platformUid { get; set; }
        /// <summary>
        /// 账户存管平台客户号
        /// </summary>
        public string plaCustId { get; set; }
        /// <summary>
        /// 页面返回url
        /// </summary>
        public string pageReturnUrl { get; set; }
    }
}
