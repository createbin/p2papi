using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.SubmitModels
{
    #region 批量申购撤销

    public class SBHBatInvestCancle: SBHBaseModel
    {
        public SBHBatInvestCancleModel SvcBody { get; set; }
    }

    public class SBHBatInvestCancleModel
    {
        /// <summary>
        /// 批次号
        /// </summary>
        public string batchNo { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string fileName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
    }

    #endregion

    #region 批量申购撤销文件

   
    /// <summary>
    /// 批量申购撤销文件名命名规范
    /// </summary>
    public class SBHBatInvestCancleFile
    {
        public SBHBatInvestCancleFile()
        {
            InvestCancleDetail = new List<BatInvestCancleFileDetail>();
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
        /// 批次号
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// 交易日期(yyyyMMdd)
        /// </summary>
        public string TransDate { get; set; }

        /// <summary>
        /// 总笔数
        /// </summary>
        public int TotalNum { get; set; }

        /// <summary>
        /// 明细
        /// </summary>
        public List<BatInvestCancleFileDetail> InvestCancleDetail { get; set; }
    }

    public class BatInvestCancleFileDetail
    {
        /// <summary>
        /// 序号
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 原账户存管平台流水
        /// </summary>
        public string OldTransId { get; set; }

        /// <summary>
        /// 账户存管平台客户号
        /// </summary>
        public string PlaCustId { get; set; }

        /// <summary>
        /// 原申购金额
        /// </summary>
        public decimal TransAmt { get; set; }

        /// <summary>
        /// 冻结编号
        /// </summary>
        public string FreezeId { get; set; }
    }

    #endregion
}
