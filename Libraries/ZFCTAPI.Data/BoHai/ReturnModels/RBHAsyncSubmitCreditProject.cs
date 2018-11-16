using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.ReturnModels
{
    /// <summary>
    /// 新增、修改项目异步回调
    /// </summary>
    public class RBHAsyncSubmitCreditProject
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        public string bizType { get; set; }

        /// <summary>
        /// 应答返回码
        /// </summary>
        public string respCode { get; set; }

        /// <summary>
        /// 应答返回码描述信息
        /// </summary>
        public string respDesc { get; set; }

        /// <summary>
        /// 商户流水号 
        /// </summary>
        public string merBillNo { get; set; }

        /// <summary>
        /// 项目编码
        /// </summary>
        public string projectCode { get; set; }

    }
}
