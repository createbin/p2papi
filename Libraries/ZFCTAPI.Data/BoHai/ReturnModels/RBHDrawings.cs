using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.ReturnModels
{
    /// <summary>
    /// 提现
    /// </summary>
    public class RBHDrawings : RBHBaseModel
    {
        public RBHDrawingsBody SvcBody { get; set; }
    }

    public class RBHDrawingsBody
    {
        /// <summary>
        /// 字符集
        /// 只能取以下枚举值 00-GBK 默认 00-GBK
        /// </summary>
        public string char_set { get; set; }

        /// <summary>
        /// 商户号
        /// 账户存管平台分配给网贷公司
        /// </summary>
        public string partner_id { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string version_no { get; set; }

        /// <summary>
        /// 消息类型
        /// RealNameWeb
        /// </summary>
        public string biz_type { get; set; }

        /// <summary>
        /// 签名方式
        /// RSA
        /// </summary>
        public string sign_type { get; set; }

        /// <summary>
        /// 商户流水号
        /// 商户生成， 标识交易的唯一性
        /// </summary>
        public string MerBillNo { get; set; }

        /// <summary>
        /// 账户存管平台客户号
        /// 用户唯一标识
        /// </summary>
        public string PlaCustId { get; set; }

        /// <summary>
        /// 交易金额
        /// 交易金额中包含商户手续费收入金额（不含营销金额）
        /// </summary>
        public string TransAmt { get; set; }

        /// <summary>
        /// 手续费模式
        /// 0 不收取， 1 商户收取用户手续费
        /// </summary>
        public string FeeType { get; set; }

        /// <summary>
        /// 商户手续费收入
        /// 商户收取用户手续费， FeeType 为 1 时不可空， FeeType 为 0 时上送 0
        /// </summary>
        public string MerFeeAmt { get; set; }

        /// <summary>
        /// 页面返回 url
        /// 浏览器返回地址
        /// </summary>
        public string PageReturnUrl { get; set; }

        /// <summary>
        /// 后台通知 url
        /// 后台返回地址
        /// </summary>
        public string BgRetUrl { get; set; }

        /// <summary>
        /// 商户保留域
        /// </summary>
        public string MerPriv { get; set; }

        /// <summary>
        /// 模式标识
        /// 0  T+1模式（默认方式）
        /// 1  T+0模式
        /// </summary>
        public string FastFlag { get; set; }

        /// <summary>
        /// 提现账户
        /// 1  投资户
        /// 2  融资户
        /// </summary>
        public string TransTyp { get; set; }

        /// <summary>
        /// 签名值
        /// </summary>
        public string mac { get; set; }

        /// <summary>
        ///移动端页面跳转密文
        ///N
        ///如果客户端类型为“移动客户端”，则仅返回此密文。
        ///页面跳转调用方式：银行URL? Transid = CBHBNetLoanWithdraw & NetLoanInfo = XXX
        /// </summary>
        public string NetLoanInfo { get; set; }
    }
}