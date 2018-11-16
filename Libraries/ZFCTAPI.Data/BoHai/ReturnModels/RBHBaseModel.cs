using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.ReturnModels
{
    public class RBHBaseModel
    {
        public RBHBaseModelHeader RspSvcHeader { get; set; }
    }

    public class RBHBaseModelHeader
    {
        /// <summary>
        /// 平台交易日期，YYYYMMDD
        /// </summary>
        public string tranDate { get; set; }
        /// <summary>
        /// 平台交易时间，单位毫秒，样式：hhmmssSSS
        /// </summary>
        public string tranTime { get; set; }
        /// <summary>
        /// 正常：00000000；失败：11111111；异常：见BIP错误码表
        /// </summary>
        public string returnCode { get; set; }
        /// <summary>
        /// 对应服务返回码的信息内容
        /// </summary>
        public string returnMsg { get; set; }
        /// <summary>
        /// 返回审核状态。异步时，需调用统一平台消息接口查询
        /// </summary>
        public string BusinessCode { get; set; }
        /// <summary>
        /// 对应服务返回码的信息内容
        /// </summary>
        public string BusinessMsg { get; set; }
        /// <summary>
        /// 结算系统生成的全局流水号
        /// </summary>
        public string globalSeqNo { get; set; }
        /// <summary>
        /// 预留字段
        /// </summary>
        public string extension { get; set; }
    }
}
