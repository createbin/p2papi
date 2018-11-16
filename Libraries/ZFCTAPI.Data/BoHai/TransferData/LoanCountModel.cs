using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.TransferData
{
    /// <summary>
    /// TODO：标的数据迁移 标的汇总数据 Model
    /// </summary>
    public class LoanCountModel
    {// <summary>
        /// 商户号 
        /// </summary>
        public string partner_id { get; set; }
        
        // <summary>
        /// 商户流水号 
        /// </summary>
        public string MerBillNo { get; set; }
        // <summary>
        /// 标的 ID  
        /// </summary>
        public string BorrowId { get; set; }
        // <summary>
        /// 标的属性  
        /// </summary>
        public string BorrowTyp { get; set; }
        // <summary>
        /// 标的金额  
        /// </summary>
        public int BorrowerAmt { get; set; }
        // <summary>
        /// 标利息 
        /// </summary>
        public string BorrowerInterestAmt { get; set; }
        // <summary>
        /// 借款人账户存管平台客户号
        /// </summary>
        public string BorrCustId { get; set; }
        // <summary>
        /// 对公账户户名  
        /// </summary>
        public string AccountName { get; set; }
        // <summary>
        /// 担保人账号  
        /// </summary>
        public string GuaranteeNo { get; set; }
        // <summary>
        /// 募集开始日 
        /// </summary>
        public DateTime BorrowerStartDate { get; set; }
        // <summary>
        /// 集到期日 
        /// </summary>
        public DateTime BorrowerEndDate { get; set; }
        // <summary>
        /// 标到期日 
        /// </summary>
        public DateTime BorrowerRepayDate { get; set; }
        // <summary>
        /// 放款方式  默认值0
        /// </summary>
        public string ReleaseType { get; set; } = "0";
        // <summary>
        /// 投资期限类型  
        /// </summary>
        public string InvestDateType { get; set; }
        // <summary>
        /// 投资期限  
        /// </summary>
        public string InvestPeriod { get; set; }
        // <summary>
        /// 标的详细信息  
        /// </summary>
        public string BorrowerDetails { get; set; }
        // <summary>
        /// 总笔数 
        /// </summary>
        public string TotalNum { get; set; }
        // <summary>
        /// 商户保留域  
        /// </summary>
        public string MerPriv1 { get; set; }
        // <summary>
        /// 商户保留域  
        /// </summary>
        public string MerPriv2 { get; set; }
        /// <summary>
        /// 标的投资明细List
        /// </summary>
        public List<LoanInvestDetailsModel> LoanInvestDetailsModelList { get; set; }
    }
}
