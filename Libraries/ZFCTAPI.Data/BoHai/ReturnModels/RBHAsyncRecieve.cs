using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.ReturnModels
{
    public class RBHAsyncRecieve:RBHAsyncBase
    {
        /// <summary>
        /// 开户银行代号
        /// </summary>
        public string openBankId { get; set; }
        /// <summary>
        /// 修改后银行账号
        /// </summary>
        public string openAcctId { get; set; }
        /// <summary>
        ///证件类型
        /// </summary>
        public string identType { get; set; }
        /// <summary>
        ///证件号码
        /// </summary>
        public string identNo { get; set; }
        /// <summary>
        ///姓名
        /// </summary>
        public string usrName { get; set; }
        /// <summary>
        ///手机号
        /// </summary>
        public string mobileNo { get; set; }
        /// <summary>
        /// 手续费
        /// </summary>
        public string feeAmt { get; set; }
        /// <summary>
        ///商户保留域
        /// </summary>
        public string merPriv { get; set; }
        /// <summary>
        /// 交易金额
        /// 交易金额中包含商户手续费收入金额（不含营销金额）
        /// </summary>
        public string transAmt { get; set; }
        /// <summary>
        /// 账户存管平台流水号
        /// </summary>
        public string transId { get; set; }
        /// <summary>
        /// 商户手续费收入
        /// 交易金额中包含商户手续费收入金额（不含营销金额）
        /// </summary>
        public string merFeeAmt { get; set; }
        /// <summary>
        /// 手续费模式
        /// 商户收取用户手续费， FeeType 为 1 时不可空， FeeType 为 0 时上送 0
        /// </summary>
        public string FeeType { get; set; }
        /// <summary>
        /// 消息扩展
        /// </summary>
        public string extension { get; set; }
        /// <summary>
        /// 线下充值账号
        /// </summary>
        public string chargeAccount { get; set; }

        /// <summary>
        ///线下充值账户户名
        /// </summary>
        public string accountName { get; set; }
        /// <summary>
        /// 清算行号
        /// </summary>
        public string accountBk { get; set; }

        /// <summary>
        /// 实名打款金额
        /// </summary>
        public string chargeAmt { get; set; }

        public string platformUid { get; set; }

        /// <summary>
        /// 项目编号
        /// </summary>
        public string projectCode { get; set; }

        /// <summary>
        /// 授权信息
        /// </summary>
        public List<AuthInfo> items { get; set; }

        /// <summary>
        /// 用户账户类型
        /// </summary>
        public string TransTyp { get; set; }
    }


    public class AuthInfo
    {
        public string auth_typ { get; set; }

        public string start_dt { get; set; }

        public string end_dt { get; set; }
    }
}
