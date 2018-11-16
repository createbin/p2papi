using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.PRO
{
    /// <summary>
    /// 合同队列
    /// </summary>
    [Table("Pro_Contract")]
    public class Pro_Contract : BaseEntity
    {
        /// <summary>
        /// 合同类型
        /// 1 借款人合同 2 投资人合同
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 投资人id
        /// </summary>
        public int? InvestId { get; set; }

        /// <summary>
        /// 标id
        /// </summary>
        public int LoanId { get; set; }

        /// <summary>
        /// 生成状态
        /// 0尚未生成 1生成成功 2生成中 3 生成失败 4重新生成
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 合同生成时间
        /// </summary>
        public DateTime? BuildTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// 合同路径
        /// </summary>
        public string ContractPath { get; set; }

        /// <summary>
        /// 生成结果
        /// </summary>
        public string BuildReuslt { get; set; }

        /// <summary>
        /// 尝试次数
        /// </summary>
        public int BuildTries { get; set; }

    }
}
