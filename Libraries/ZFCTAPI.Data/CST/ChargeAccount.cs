using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Text;
using Dapper.Contrib.Extensions;

namespace ZFCTAPI.Data.CST
{
    [Table("ChargeAccount")]
    public class ChargeAccount: BaseEntity
    {
        /// <summary>
        /// 对公账号类别
        /// </summary>
        public string AccountType { get; set; }
        /// <summary>
        /// 对公账号
        /// </summary>
        public string AccountNo { get; set; }
        /// <summary>
        /// 对公账户户名
        /// </summary>
        public string AccountName { get; set; }
        /// <summary>
        /// 清算行号
        /// </summary>
        public string AccountBk { get; set; }
        /// <summary>
        /// 大额充值账号
        /// </summary>
        public string ChargeAccountNo { get; set; }
        /// <summary>
        /// 公司id
        /// </summary>
        public int CompanyId { get; set; }

        public bool Success { get; set; }
        /// <summary>
        /// 实名状态
        /// </summary>
        public bool RealNameState { get; set; }
        /// <summary>
        /// 实名打款金额
        /// </summary>
        public decimal CertificationMoney { get; set; }
        /// <summary>
        /// 商户请求流水号
        /// </summary>
        public string MerBillNo { get; set; }
    }
}
