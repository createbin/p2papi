using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.ReturnModels
{
    public class RBHAsyncRealNameWeb:RBHAsyncBase
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
        /// 保留字段先不用
        /// </summary>
        public string TransTyp { get; set; }
    }
}