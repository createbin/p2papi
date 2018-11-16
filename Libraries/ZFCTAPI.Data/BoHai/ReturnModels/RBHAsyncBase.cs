using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.ReturnModels
{
    public class RBHAsyncBase
    {
        /// <summary>
        /// 消息类型
        /// DrawingsResult
        /// </summary>
        public string bizType { get; set; }
        /// <summary>
        /// 字符集  只能取以下枚举值 00-GBK 默认 00-GBK
        /// </summary>
        public string respCode { get; set; }
        /// <summary>
        /// 商户号 账户存管平台分配给网贷公司 
        /// </summary>
        public string respDesc { get; set; }
        /// <summary>
        /// 版本号 
        /// </summary>
        public string merBillNo { get; set; }
        /// <summary>
        /// 账户存管平台客户号 
        /// </summary>
        public string plaCustId { get; set; }

        public string rpCode { get; set; }

        public string rpDesc { get; set; }

        
    }
}
