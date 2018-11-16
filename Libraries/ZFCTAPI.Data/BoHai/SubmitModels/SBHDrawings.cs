using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.SubmitModels
{
    /// <summary>
    /// 提现
    /// </summary>
    public class SBHDrawings : SBHBaseModel
    {
        public SBHDrawingsBody SvcBody { get; set; }
    }

    public class SBHDrawingsBody
    {
        /// <summary>
        /// 客户端类型
        /// N
        /// 不填写默认为1
        /// 1：电脑客户端
        /// 2：移动客户端
        /// </summary>
        public string clientType { get; set; }

        /// <summary>
        /// 用户平台编码
        /// Y
        /// </summary>
        public string platformUid { get; set; }

        /// <summary>
        /// 账户存管平台客户号
        /// Y
        /// 用户唯一标识
        /// </summary>
        public string plaCustId { get; set; }

        /// <summary>
        /// 交易金额
        /// Y
        /// 交易金额中包含商户手续费收入金额（不含营销金额）
        /// </summary>
        public string transAmt { get; set; }

        /// <summary>
        /// 手续费模式
        /// Y
        /// 0 不收取， 1 商户收取用户手续费
        /// </summary>
        public string feeType { get; set; }

        /// <summary>
        /// 商户手续费收入
        /// Y
        /// 商户收取用户手续费， FeeType 为 1 时不可空， FeeType 为 0 时上送 0
        /// </summary>
        public string merFeeAmt { get; set; }

        /// <summary>
        /// 页面返回url
        /// Y
        /// </summary>
        public string pageReturnUrl { get; set; }
    }
}