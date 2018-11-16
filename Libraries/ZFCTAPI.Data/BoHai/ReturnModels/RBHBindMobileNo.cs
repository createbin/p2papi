using System;
using System.Collections.Generic;
using System.Text;
using ZFCTAPI.Data.ApiModels.SubmitModels;

namespace ZFCTAPI.Data.BoHai.ReturnModels
{
    public class RBHBindMobileNo : RBHBaseModel
    {
        public RBHBindMobileNoBody SvcBody { get; set; }
    }

    public class RBHBindMobileNoBody
    {
        /// <summary>
        ///字符集
        /// </summary>
        public string char_set { get; set; }
        /// <summary>
        /// 商户号
        /// </summary>
        public string partner_id { get; set; }
        /// <summary>
        ///版本号
        /// </summary>
        public string version_no { get; set; }
        /// <summary>
        ///消息类型
        /// </summary>
        public string biz_type { get; set; }
        /// <summary>
        /// 签名方式 
        /// </summary>
        public string sign_type { get; set; }
        /// <summary>
        /// 商户流水号
        /// </summary>
        public string MerBillNo { get; set; }
        /// <summary>
        ///账户存管平台客户号
        /// </summary>
        public string PlaCustId { get; set; }
        /// <summary>
        ///新手机号
        /// </summary>
        public string MobileNo { get; set; }
        /// <summary>
        /// 页面返回 url
        /// </summary>
        public string PageReturnUrl { get; set; }
        /// <summary>
        /// 后台通知 url
        /// </summary>
        public string BgRetUrl { get; set; }
        /// <summary>
        /// 账户类型	1 投资户  2 融资户
        /// </summary>
        public string TransTyp { get; set; }       
        /// <summary>
        ///商户保留域
        /// </summary>
        public string MerPriv { get; set; }
        /// <summary>
        ///签名值
        /// </summary>
        public string mac { get; set; }
        /// <summary>
        /// 移动端页面跳转密文
        /// </summary>
        public string NetLoanInfo { get; set; }
        
    }
}
