using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.ReturnModels
{
    public class RBHAsyncAutoInvestAuthWeb
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

        public string rpCode { get; set; }

        public string rpDesc { get; set; }
        /// <summary>
        /// 授权列表
        /// </summary>
        public List<RBHAuthResult> items { get; set; } = new List<RBHAuthResult>();
    }


    public class RBHAuthResult
    {
        /// <summary>
        /// 授权类型
        /// </summary>
        public string auth_typ { get; set; }
        /// <summary>
        /// 有效开始日
        /// </summary>
        public string start_dt { get; set; }
        /// <summary>
        /// 有效结束日
        /// </summary>
        public string end_dt { get; set; }
        /// <summary>
        /// 授权金额
        /// </summary>
        public string auth_amt { get; set; }
    }
}
