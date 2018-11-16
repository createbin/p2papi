using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.ReturnModels
{
    public class RBHBindPass:RBHBaseModel
    {
        /// <summary>
        /// 字符集  只能取以下枚举值 00-GBK 默认 00-GBK
        /// </summary>
        public string char_set { get; set; }
        /// <summary>
        /// 商户号 账户存管平台分配给网贷公司 
        /// </summary>
        public string partner_id { get; set; }
        /// <summary>
        /// 版本号 
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
        /// 开户类型  1-新用户 2-老用户平台绑定 3-老用户（只有证件信息）绑定
        /// </summary>
        public string PlaCustId { get; set; }
        ///<summary>
        /// 页面返回 url 
        /// </summary>
        public string PageReturnUrl { get; set; }
        /// <summary>
        /// 后台通知 url 
        /// </summary>
        public string BgRetUrl { get; set; }
        /// <summary>
        /// 商户保留域 
        /// </summary>
        public string MerPriv { get; set; }
        /// <summary>
        ///签名值
        /// </summary>
        public string mac { get; set; }
    }
}
