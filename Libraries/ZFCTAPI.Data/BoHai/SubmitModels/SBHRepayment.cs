using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.SubmitModels
{
    #region 还款明细接口请求模型

    public class SBHRepaymentExecuteEx : SBHBaseModel
    {
        public SBHRepaymentExecuteExModel SvcBody { get; set; }
    }

    public class SBHRepaymentExecuteExModel
    {
        /// <summary>
        /// 项目编号
        /// </summary>
        public string projectCode { get; set; }

        /// <summary>
        /// 是否代偿
        /// 参照码值.【是否代偿】
        /// </summary>
        public string compensatoryFlag { get; set; }

        /// <summary>
        /// 还款人平台用户编码
        /// 是否代偿=担保人线上代偿时，填担保人用户编码。
        /// </summary>
        public string repaymentPlatformCode { get; set; }

        /// <summary>
        /// 还款计划编码
        /// 预留，本期不使用
        /// </summary>
        public string repaymentPlanCode { get; set; }

        /// <summary>
        /// 还款期数
        /// 分期还款总期数，若不分期填写0
        /// </summary>
        public string numberOfPayments { get; set; }

        /// <summary>
        /// 还款期号
        /// 分期还款当前期数，若不分期填写0
        /// </summary>
        public string period { get; set; }

        /// <summary>
        /// 实际还款时间(格式：hhmmss)
        /// </summary>
        public string actualRepaymentTime { get; set; }

        /// <summary>
        /// 归平台罚息合计
        /// 按借款总额逾期计算的归平台罚息（渤海银行不支持罚息，填写0）
        /// </summary>
        public string sumPenaltyInterestToPlatform { get; set; } = "0";

        /// <summary>
        /// 是否完成还款
        /// 分期还款时填写，不填默认完成还款 0：未完成还款
        /// </summary>
        public string complete { get; set; }

        /// <summary>
        /// 本期还款明细是否上送完毕
        /// 默认为否 ,1是
        /// </summary>
        public string batchFlag { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string fileName { get; set; }

        public string extension { get; set; }
    }

    #endregion

    #region 还款明细TXT文本模型

    /// <summary>
    /// 还款生成TXT文本模型
    /// </summary>
    public class SBHRepaymentFile
    {
        public SBHRepaymentFile()
        {
            InvestPersons = new List<SBHRepaymentFilePerson>();
        }

        /// <summary>
        /// char_set
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
        /// 交易金额（借款人还款总金额，包含明细汇总本金+明细汇总收益+商户收借款人手续费）
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
        /// 账户存管平台借款人ID号或对公账号
        /// (个人用户还款取账户存管平台ID号，公司还款取对公账号)
        /// </summary>
        public string BorrCustId { get; set; }

        /// <summary>
        /// 商户保留域
        /// </summary>
        public string MerPriv { get; set; }

        /// <summary>
        /// 总笔数
        /// </summary>
        public int TotalNum { get; set; }

        /// <summary>
        /// 投资人
        /// </summary>
        public List<SBHRepaymentFilePerson> InvestPersons { get; set; }
    }

    public class SBHRepaymentFilePerson
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
        /// 本金金额
        /// </summary>
        public decimal TransAmt { get; set; }

        /// <summary>
        /// 利息收益=（应还利息+罚息）
        /// </summary>
        public string Interest { get; set; }

        /// <summary>
        /// 投资管理费
        /// </summary>
        public decimal Inves_fee { get; set; }
    }

    #endregion
}
