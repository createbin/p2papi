using System;
using System.Collections.Generic;
using System.Text;
using ZFCTAPI.Data.CST;

namespace ZFCTAPI.Data.ApiModels.SubmitModels
{
    #region 参数类
    public class BatchAccountData
    {
        /// <summary>
        /// 开始条数
        /// </summary>
        public string start { get; set; }
        /// <summary>
        /// 截止条数
        /// </summary>
        public int end { get; set; }
        /// <summary>
        /// 项目编号
        /// </summary>
        public int loanid { get; set; }
    }
    #endregion

    #region 文本文档需要的类型

    /// <summary>
    /// TODO：注册申请文件文件汇总Model
    /// </summary>
    public class ExistUserRegisterModel
    {
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
        /// 交易日期
        /// </summary>
        public string TransDate { get; set; } = DateTime.Now.ToString("yyyyMMdd");
        /// <summary>
        /// 总数
        /// </summary>
        public string TotalNum { get; set; }

        public List<AccountImportModel> AccountImportModels { get; set; }
    }


    #endregion
}
