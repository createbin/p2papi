using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.TransferData
{
    /// <summary>
    /// TODO：标的数据迁移返回Model
    /// </summary>
    /// 
    public class LoanReturnModelApplication
    {

        public RBHLoanTransfer RspSvcHeader { get; set; }
    }

    public class LoanReturnModel
    {
        // <summary>
        /// 商户号 
        /// </summary>
        public string partner_id { get; set; }
        // <summary>
        /// 版本号 
        /// </summary>
        public string version_no { get; set; }
        // <summary>
        /// 消息类型 
        /// </summary>
        public string biz_type { get; set; }
        // <summary>
        /// 签名方式 
        /// </summary>
        public string sign_type { get; set; }
        // <summary>
        /// 商户流水号 
        /// </summary>
        public string MerBillNo { get; set; }
        // <summary>
        /// 应答返回码 
        /// </summary>
        public string RespCode { get; set; }
        // <summary>
        /// 应答返回码描述信息        
        /// </summary>
        public string RespDesc { get; set; }
        // <summary>
        /// 签名数据 
        /// </summary>
        public string mac { get; set; }
    }

    /// <summary>
    /// 获取结算ftp服务器文件结果的Model
    /// </summary>
    public class LoanFileReturnModel
    {
        /// <summary>
        /// 商户号
        /// </summary>
        public string partner_id     { get; set; }
        /// <summary>
        /// 商户流水号
        /// </summary>
        public string MerBillNo { get; set; }
        /// <summary>
        /// 标的 ID
        /// </summary>
        public string BorrowId { get; set; }
        /// <summary>
        /// 标的金额
        /// </summary>
        public string BorrowerAmt { get; set; }
        /// <summary>
        /// 借款人账户存管平台客户号
        /// </summary>
        public string BorrCustId { get; set; }
        /// <summary>
        /// 总笔数
        /// </summary>
        public string TotalNum { get; set; }

        public List<LoanFileDetailsReturnModel> LoanDetailsLsit { get; set; }
    }

    public class LoanFileDetailsReturnModel
    {
        public string id { get; set; }
    }

    public class RBHLoanTransfer
    {
        public string tranTime { get; set; }

        public string returnCode { get; set; }

        public string tranDate { get; set; }

        public string globalSeqNo { get; set; }

        public string returnMsg { get; set; }

        public string backendSysId { get; set; }

        public string RespCode { get; set; }
    }
}
