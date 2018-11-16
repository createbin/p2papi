using System;
using System.Collections.Generic;
using System.Text;
using ZFCTAPI.Data.BoHai.SubmitModels;

namespace ZFCTAPI.Data.BoHai.TransferData
{
    public class TransferLoanStock : SBHBaseModel
    {
        public LoanRequestModel SvcBody { get; set; }
    }
    /// <summary>
    /// TODO:标的数据迁移请求 Model
    /// </summary>
    public class LoanRequestModel
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
        /// 版本号
        /// </summary>
        public string version_no { get; set; } = "2.0";
        /// <summary>
        /// 消息类型
        /// </summary>
        public string biz_type { get; set; } = "FileLoanTransfer";
        /// <summary>
        /// 签名方式
        /// </summary>
        public string sign_type { get; set; } = "RSA";
        /// <summary>
        /// 商户流水号
        /// </summary>
        public string MerBillNo { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 标的 ID
        /// </summary>
        public string BorrowId { get; set; }
        /// <summary>
        /// 标的金额
        /// </summary>
        public string BorrowerAmt { get; set; }
        /// <summary>
        /// 后台通知 url
        /// </summary>
        public string BgRetUrl { get; set; }
        /// <summary>
        /// 商户保留域
        /// </summary>
        public string MerPriv { get; set; }
        /// <summary>
        /// 签名数据
        /// </summary>
        public string mac { get; set; }
    }


    public class RequestModel
    {
        public string batchNo { get; set; }
        public string partner_id { get; set; }
        public string FileName { get; set; }
        public string BorrowId { get; set; }
        public int BorrowerAmt { get; set; }
    }
}
