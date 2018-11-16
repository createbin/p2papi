using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.SubmitModels
{
    /// <summary>
    /// 申购
    /// </summary>
    public class SBHClaimsPurchase : SBHBaseModel
    {
        public SBHClaimsPurchaseBody SvcBody { get; set; }
    }

    public class SBHClaimsPurchaseBody
    {
        /// <summary>
        /// 项目编码
        /// Y
        /// 互金平台上报的项目编码
        /// </summary>
        public string projectCode { get; set; }

        /// <summary>
        /// 项目类型
        /// Y
        /// 参照【码值】.项目类型
        /// </summary>
        public string projectType { get; set; }

        /// <summary>
        /// 申购单号
        /// Y
        /// </summary>
        public string purchaseNumber { get; set; }

        /// <summary>
        /// 投资人平台编码
        /// Y
        /// 互金平台上报的投资人用户编码
        /// </summary>
        public string investorPlatformCode { get; set; }

        /// <summary>
        /// 平台代理标志
        /// Y
        /// 1：平台代理申购；0：非代理、投资人自行申购
        /// 如用户已授权自动投标，则平台可依据授权条件代理申购，此时申购无需验密。
        /// </summary>
        public string platformAgent { get; set; }

        /// <summary>
        /// 申购金额
        /// C
        /// 申购总金额。投资人实际使用的自有资金金额=申购金额-红包金额-平台投资优惠
        /// </summary>
        public string purchaseMoney { get; set; }

        /// <summary>
        /// 红包金额
        /// N
        /// </summary>
        public string campaignMoney { get; set; }

        /// <summary>
        /// 平台投资优惠
        /// Y
        /// 平台给予投资优惠金额；此优惠金额将从平台营销户扣除。
        /// </summary>
        public string discount { get; set; }

        /// <summary>
        /// 申购单价
        /// C
        /// 互金平台上报的项目编码
        /// </summary>
        public string purchasePrice { get; set; }

        /// <summary>
        /// 申购份额
        /// Y
        /// 份额为空时，申购金额必填；份额不为空时，申购金额、单价任填其一。
        /// </summary>
        public string purchaseShare { get; set; }

        /// <summary>
        /// 起息日
        /// C
        /// 项目起息日，YYYYMMDD；
        ///若项目[起息方式] 为按投资起息，则必须指定起息日
        /// </summary>
        public string valueDate { get; set; }

        /// <summary>
        /// 申购有效日期
        /// N
        /// 样式：YYYYMMDD,预留字段。
        /// </summary>
        public string purchaseTerm { get; set; }

        /// <summary>
        /// 申购 IP
        /// Y
        /// </summary>
        public string purchaseIP { get; set; }

        /// <summary>
        /// 短信验证码
        /// N
        /// 根据具体接入银行的验证模式填写短信验证码或者不填。（渤海银行：非授权用户必输）
        /// </summary>
        public string mobileCode { get; set; }

        /// <summary>
        /// 密码
        /// N
        /// 渤海银行专用：非授权用户必输
        /// </summary>
        public string payPassWord { get; set; }

        /// <summary>
        /// 手续费
        /// N
        /// 预留字段
        /// </summary>
        public string purchaseFee { get; set; }
    }
}