using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.TransferData
{
    /// <summary>
    /// TODO:用户存量数据迁移（汇总数据） MODEL
    /// </summary>
    public class AccountRegisterFile
    {
        /// <summary>
        /// 字符集
        /// </summary>
        public string char_set { get; set; } = "00";
        /// <summary>
        /// 商户号
        /// </summary>
        public string partner_id { get; set; }
        /// <summary>
        /// 商户流水号
        /// </summary>
        public string MerBillNo { get; set; }
        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNo { get; set; }
        /// <summary>
        /// 交易日期
        /// </summary>
        public string TransDate { get; set; }
        /// <summary>
        /// 总笔数
        /// </summary>
        public string TotalNum { get; set; }
    }

    /// <summary>
    /// TODO:用户存量数据迁移(明细) MODEL
    /// </summary>
    public class AccountRegisterDetailFile
    {
        /// <summary>
        /// 证件类型
        /// </summary>
        public string IdentType { get; set; }
        /// <summary>
        /// 证件号码
        /// </summary>
        public string IdentNo { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string UsrName { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string MobileNo { get; set; }
        /// <summary>
        /// 开户银行代号
        /// </summary>
        public string OpenBankId { get; set; }
        /// <summary>
        /// 开户银行账号
        /// </summary>
        public string OpenAcctId { get; set; }
        /// <summary>
        /// 账户类型
        /// </summary>
        public string TransTyp { get; set; }
    }
}
