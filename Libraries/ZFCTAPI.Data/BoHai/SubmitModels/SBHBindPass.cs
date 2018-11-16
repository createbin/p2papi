using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.SubmitModels
{
    public class SBHBindPass : SBHBaseModel
    {
        public SBHBindPassBody SvcBody { get; set; }
    }

    public class SBHBindPassBody
    {
        /// <summary>
        /// 客户端类型(1：电脑客户端 2：移动客户端)
        /// </summary>
        public string clientType { get; set; }
        /// <summary>
        /// 账户存管平台客户号
        /// </summary>
        public string plaCustId { get; set; }
        /// <summary>
        /// 页面返回url
        /// </summary>
        public string pageReturnUrl { get; set; }
        /// <summary>
        /// 手机号(clientType为2时，且业务为找回密码时必填)
        /// </summary>
        public string mobileNo { get; set; }

        /// <summary>
        /// 账户类型(1-个人 2-对公)
        /// </summary>
        public string accountTyp { get; set; }

        /// <summary>
        /// 找回描述(clientType为2时，且业务为对公用户找回密码时必填)
        /// </summary>
        public string txDesc { get; set; }

        /// <summary>
        /// 账户类型 1 投资户  2 融资户
        /// </summary>
        public string transTyp { get; set; }
    }
}
