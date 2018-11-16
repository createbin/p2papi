using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.SubmitModels
{
    /// <summary>
    /// 可用余额
    /// </summary>
    public class SBHAccountQueryAccBalance : SBHBaseModel
    {
        public SBHAccountQueryAccBalanceBody SvcBody { get; set; }
    }

    public class SBHAccountQueryAccBalanceBody
    {

        /// <summary>
        /// 用户平台编码
        /// Y
        /// </summary>
        public string platformUid { get; set; }

        /// <summary>
        /// 协议类型
        /// N
        /// 预留字段，不填写默认为1。
        /// </summary>
        public string agreementType { get; set; }

        /// <summary>
        /// 协议编号
        /// N
        /// 预留字段。
        /// </summary>
        public string agreementNo { get; set; }

        /// <summary>
        /// 存管账号
        /// N
        /// 浙商银行专用：非必填项。
        /// </summary>
        public string accNo { get; set; }

        /// <summary>
        /// 备注.
        /// C
        /// 浙商银行专用：上送999999，则返回通用可用余额、通用可提现余额和可转移金额（可转移金额在返回报文的extension中体现）
        /// </summary>
        public string remark { get; set; }
    }
}