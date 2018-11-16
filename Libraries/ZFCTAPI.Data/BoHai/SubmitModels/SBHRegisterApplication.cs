using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.SubmitModels
{

    public class TransferUserRegister: SBHBaseModel
    {
        public SBHRegisterApplication SvcBody { get; set; }
    }
    /// <summary>
    /// 批量用户注册请求实体
    /// </summary>
    public class SBHRegisterApplication
    {
        /// <summary>
        /// 字符集
        /// </summary>
        public string char_set { get; set; } = "00";
        /// <summary>
        /// 商户号
        /// </summary>
        public string partner_id { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public string version_no { get; set; } = "2.0";
        /// <summary>
        /// 消息类型
        /// </summary>
        public string biz_type { get; set; } = "existUserRegister";
        /// <summary>
        /// 签名方式
        /// </summary>
        public string sign_type { get; set; } = "RSA";
        /// <summary>
        /// 商户流水号
        /// </summary>
        public string MerBillNo { get; set; }
        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNo { get; set; }
        /// <summary>
        /// 后台通知 url
        /// </summary>
        public string BgRetUrl { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string MerPriv { get; set; }

        /// <summary>
        /// 签名数据
        /// </summary>
        public string mac { get; set; }


    }
}
