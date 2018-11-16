using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.ReturnModels
{
    public class RBHRepayment
    {
        /// <summary>
        /// 消息类型(放款通知：FileRelease,还款通知：FileRepayment)
        /// </summary>
        public string bizType { get; set; }

        /// <summary>
        /// 商户流水号
        /// </summary>
        public string merBillNo { get; set; }

        /// <summary>
        /// 应答返回码
        /// </summary>
        public string respCode { get; set; }

        /// <summary>
        /// 应答返回码描述信息
        /// </summary>
        public string respDesc { get; set; }

        /// <summary>
        /// 消息扩展
        /// </summary>
        public string extension { get; set; }
    }
}
