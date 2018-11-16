using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.SubmitModels
{
    /// <summary>
    /// 转让成交
    /// </summary>
    public class SBHClaimsTransferDeal : SBHBaseModel
    {
        public SBHClaimsTransferDealBody SvcBody { get; set; }
    }

    public class SBHClaimsTransferDealBody
    {
        /// <summary>
        /// 项目编号
        /// Y
        /// </summary>
        public string projectCode { get; set; }

        /// <summary>
        /// 项目类型
        /// Y
        /// 参照【码值】.项目类型
        /// </summary>
        public string projectType { get; set; }

        /// <summary>
        /// 原投资流水号
        /// Y
        /// 用户申购时，银行存管返回的流水号
        /// </summary>
        public string transferApplyNumber { get; set; }

        /// <summary>
        /// 转让成交单号
        /// Y
        /// 平台转让申请单号
        /// </summary>
        public string transferDealNumber { get; set; }

        /// <summary>
        /// 转让竞价单号
        /// N
        /// 预留字段
        /// </summary>
        public string transferBidNumber { get; set; }

        /// <summary>
        /// 转让人平台编码
        /// Y
        /// </summary>
        public string transferorPlatformCode { get; set; }

        /// <summary>
        /// 受让人平台编号
        /// Y
        /// </summary>
        public string transfereePlatformCode { get; set; }

        /// <summary>
        /// 成交本金
        /// Y
        /// 同一转让单，Σ成交本金 小于等于 转让本金
        /// </summary>
        public string dealMoney { get; set; }

        /// <summary>
        /// 成交单价
        /// Y
        /// 成交价格 = 成交单价*成交份额
        /// </summary>
        public string dealPrice { get; set; }

        /// <summary>
        /// 成交份额
        /// N
        /// 不填默认为1
        /// </summary>
        public string dealShare { get; set; }

        /// <summary>
        ///出让方手续费
        /// Y
        /// </summary>
        public string transferorFee { get; set; }

        /// <summary>
        /// 受让方手续费
        /// C
        /// 根据具体银行支持的业务判断是否填写
        /// </summary>
        public string transfereeFee { get; set; }

        /// <summary>
        /// 短信验证码
        /// Y
        /// </summary>
        public string mobileCode { get; set; }

        /// <summary>
        /// 红包抵扣金额
        /// N
        /// 预留字段，本期不使用
        /// </summary>
        public string redPackBala { get; set; }
    }
}