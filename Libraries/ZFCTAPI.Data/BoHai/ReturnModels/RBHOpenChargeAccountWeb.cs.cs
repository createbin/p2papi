using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.ReturnModels
{
    public class RBHOpenChargeAccountWeb:RBHBaseModel
    {
        public RBHOpenChargeAccountWeb()
        {
            SvcBody=new RBHOpenChargeAccountWebBody();
        }

        public RBHOpenChargeAccountWebBody SvcBody { get; set; }
    }

    public class RBHOpenChargeAccountWebBody
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

        public string TxnTyp { get; set; }

        public string AccountTyp { get; set; }

        public string AccountNo { get; set; }

        public string AccountName { get; set; }

        public string AccountBk { get; set; }

        /// <summary>
        /// 页面返回 url 
        /// </summary>
        public string PageReturnUrl { get; set; }
        /// <summary>
        /// 后台通知 url 
        /// </summary>
        public string BgRetUrl { get; set; }
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
