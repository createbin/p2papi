using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.RiskAssessment
{
    [Table("RiskAnswer")]
    public class RiskAnswer : BaseEntity
    {
        /// <summary>
        /// 问题编号
        /// </summary>
        public int QId { get; set; }

        /// <summary>
        /// 答案描述
        /// </summary>
        public string Describe { get; set; }

        /// <summary>
        /// 分值
        /// </summary>
        public decimal Score { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDel { get; set; }

        /// <summary>
        /// 创建人编号
        /// </summary>
        public int Creater { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 修改人编号
        /// </summary>
        public int Modifier { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyDate { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
