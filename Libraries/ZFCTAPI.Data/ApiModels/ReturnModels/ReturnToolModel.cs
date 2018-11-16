using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.ApiModels.ReturnModels
{
    public class RHomeStatistics
    {
        /// <summary>
        /// 交易金额
        /// </summary>
        public decimal AllTranscationMoney { get; set; }
        /// <summary>
        /// 总收益
        /// </summary>
        public decimal AllProfitMoney { get; set; }
        /// <summary>
        /// 总用户
        /// </summary>
        public int AllUser { get; set; }
        /// <summary>
        /// 安全日期
        /// </summary>
        public int SecureDay { get; set; }
    }

    public class RBankInfos
    {
        public List<BankInfo> BankInfos { get; set; }=new List<BankInfo>();
    }

    public class BankInfo
    {
        public string BankCode { get; set; }

        public string BankName { get; set; }
    }

    public class RRetRetVersion
    {
        public string VersionCode { get; set; }
        public string VersionName { get; set; }
        public string DownloadUrl { get; set; }
        public string UpdateLog { get; set; }
        public string Mandatoryupdate { get; set; }
    }

}
