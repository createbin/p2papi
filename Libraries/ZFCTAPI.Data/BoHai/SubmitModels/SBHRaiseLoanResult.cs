using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.SubmitModels
{
    #region 修改项目起息日

    /// <summary>
    /// 修改项目起息日
    /// </summary>
    public class SBHUpdateProjectRateDate : SBHBaseModel
    {
        public SBHUpdateProjectRateDateModel SvcBody { get; set; }
    }

    

    public class SBHUpdateProjectRateDateModel
    {
        /// <summary>
        /// 项目编码
        /// </summary>
        public string projectCode { get; set; }

        /// <summary>
        /// 起息日
        /// </summary>
        public string valueDate { get; set; }

        /// <summary>
        /// 申购单号
        /// </summary>
        public string purchaseNumber { get; set; }
    }

    #endregion

    #region 流标实体类

    public class SBHRevoke : SBHBaseModel
    {
        public SBHRevokeModel SvcBody { get; set; }
    }

    public class SBHRevokeModel
    {
        /// <summary>
        /// 项目编号
        /// </summary>
        public string projectCode { get; set; }

        /// <summary>
        /// 项目类型
        /// </summary>
        public string projectType { get; set; }

        /// <summary>
        /// 撤销申请类型
        /// </summary>
        public string revokeType { get; set; }

        /// <summary>
        /// 申请单号
        /// </summary>
        public string applyCode { get; set; }

        /// <summary>
        /// 撤销 IP
        /// </summary>
        public string cancelIP { get; set; }

        /// <summary>
        /// 短信验证码 
        /// </summary>
        public string mobileCode { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string comment { get; set; }

        /// <summary>
        /// 消息扩展
        /// </summary>
        public string extension { get; set; }
    }

    #endregion

    #region 募集结果上报接口实体类

    /// <summary>
    /// 募集结果上报接口实体类
    /// </summary>
    public class SBHRaiseLoanResult: SBHBaseModel
    {
        public SBHRaiseLoanResultModel SvcBody { get; set; }
    }

    public class SBHRaiseLoanResultModel
    {
        /// <summary>
        /// 项目编号
        /// </summary>
        public string projectCode { get; set; }

        /// <summary>
        /// 项目类型
        /// </summary>
        public string projectType { get; set; }

        /// <summary>
        /// 募集结果(1：募集成功；0：募集失败（流标）)
        /// </summary>
        public string repaymentType { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string fileName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string comment { get; set; }

        /// <summary>
        /// 消息扩展
        /// </summary>
        public string extension { get; set; }

        /// <summary>
        /// 合同编号(如果evidFlag=1，则必填)
        /// </summary>
        public string contractNo { get; set; }
    }

    #endregion

    #region 募集结果上报TXT文件实体类

    /// <summary>
    /// 募集结果上报TXT文件实体类
    /// </summary>
    public class SBHRaiseLoanResultFile
    {
        public SBHRaiseLoanResultFile()
        {
            InvestDetails = new List<InvestDetail>();
        }

        /// <summary>
        /// 字符集
        /// </summary>
        public string char_set { get; set; }

        /// <summary>
        /// 商户号
        /// </summary>
        public string partner_id { get; set; }

        /// <summary>
        /// 商户流水号
        /// </summary>
        public string MerBillNo { get; set; }

        /// <summary>
        /// 交易金额
        /// 交易本金=明细汇总金额（实际到账金额为扣除手续费金额）
        /// </summary>
        public decimal TransAmt { get; set; }

        /// <summary>
        /// 手续费
        /// </summary>
        public decimal FeeAmt { get; set; }

        /// <summary>
        /// 标的ID
        /// </summary>
        public string BorrowId { get; set; }

        /// <summary>
        /// 标的金额
        /// </summary>
        public decimal BorrowerAmt { get; set; }

        /// <summary>
        /// 账户存管平台借款人ID号(用户唯一标识或对公账号)
        /// </summary>
        public string BorrCustId { get; set; }

        /// <summary>
        /// 放款方式(0-普通放款,1-加急放款)
        /// </summary>
        public string ReleaseType { get; set; }

        /// <summary>
        /// 商户保留域
        /// </summary>
        public string MerPriv { get; set; }

        /// <summary>
        /// 总笔数
        /// </summary>
        public int TotalNum { get; set; }

        /// <summary>
        /// 投资明细
        /// </summary>
        public List<InvestDetail> InvestDetails { get; set; }
    }

    public class InvestDetail
    {
        /// <summary>
        /// 序号
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 账户存管平台客户号
        /// </summary>
        public string PlaCustId { get; set; }

        /// <summary>
        /// 交易金额(用户投资总金额)
        /// </summary>
        public decimal TransAmt { get; set; }

        /// <summary>
        /// 冻结编号
        /// </summary>
        public string FreezeId { get; set; }
    }

    #endregion

    #region 还款充值

    public class SBPayBackRecharge : SBHBaseModel
    {
        public PayBackRechargeModel SvcBody { get; set; }
    }

    public class PayBackRechargeModel
    {
        /// <summary>
        /// 用户平台编码
        /// </summary>
        public string platformUid { get; set; }

        /// <summary>
        /// 项目编号
        /// </summary>
        public string projectCode { get; set; }

        /// <summary>
        /// 入账金额
        /// </summary>
        public string amount { get; set; }
    }

    #endregion

    #region 投资合同

    public class SBContractFileUpload : SBHBaseModel
    {
        public SBContractFileUploadModel SvcBody { get; set; }
    }

    public class SBContractFileUploadModel
    {
        /// <summary>
        /// 项目编号
        /// </summary>
        public string projectCode { get; set; }
        /// <summary>
        /// 项目类型(参照【码值】.项目类型)
        /// </summary>
        public string projectType { get; set; }
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

    #endregion
}
