using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.ApiModels.SubmitModels
{
    /// <summary>
    /// 还款
    /// </summary>
    public class SRepaymentSubmitModel : BaseSubmitModel
    {
        /// <summary>
        /// 还款计划ID
        /// </summary>
        public int LoanPlanId { get; set; }

        /// <summary>
        /// true 担保户还款，false 借款人还款
        /// </summary>
        public bool IsGuar { get; set; }

        public string VerCode { get; set; }
    }

    /// <summary>
    /// 募集结果上报
    /// </summary>
    public class SRaiseLoanSubmitModel : BaseSubmitModel
    {
        /// <summary>
        /// 标的信息ID
        /// </summary>
        public int LoanId { get; set; }
    }

    /// <summary>
    /// 流标
    /// </summary>
    public class SRevokeSubmitModel: BaseSubmitModel
    {
        /// <summary>
        /// 标的信息ID
        /// </summary>
        public int LoanId { get; set; }
        /// <summary>
        /// 撤销 IP
        /// </summary>
        public string IP { get; set; }
    }

    /// <summary>
    /// 投资合同上传
    /// </summary>
    public class SContractFileUploadModel: BaseSubmitModel
    {
        /// <summary>
        /// 项目编号
        /// </summary>
        public string projectCode { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string fileName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string comment { get; set; }
        /// <summary>
        /// 合同编号
        /// </summary>
        public string contractNo { get; set; }
        /// <summary>
        /// 消息扩展
        /// </summary>
        public string extension { get; set; }
    }
}
