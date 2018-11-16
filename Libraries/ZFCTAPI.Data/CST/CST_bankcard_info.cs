using System;
using Dapper.Contrib.Extensions;

namespace ZFCTAPI.Data.CST
{
    /// <summary>
    /// 数据表CST_bankcard_info的数据库实体类
    /// </summary>
    [Table("CST_bankcard_info")]
    public partial class CST_bankcard_info : BaseEntity
    {
        /// </summary>	
        public int? mon_account_id { get; set; }
        /// <summary>
        /// 绑卡时间
        /// </summary>
        public DateTime bank_datetime { get; set; }

        /// <summary>
        ///  开户银行账号 
        /// </summary>	
        public string bank_no { get; set; }

        /// <summary>
        ///  开户银行代号 
        /// </summary>	
        public string bank_code { get; set; }

        /// <summary>
        ///  开户银行省份 
        /// </summary>	
        public string bank_prov { get; set; }

        /// <summary>
        ///  开户银行地区 
        /// </summary>	
        public string bank_area { get; set; }

        /// <summary>
        ///  1：是 0：否
        /// </summary>	
        public int? bank_isdefalut { get; set; }
        /// <summary>
        /// 渤海银行返回的银行卡号
        /// </summary>
        public string EncBankNo { get; set; }

        /// <summary>
        /// 渤海银行返回的银行卡号
        /// </summary>
        public bool? IsBoHai { get; set; }
    }
}
