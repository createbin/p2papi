using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.RiskAssessment
{
    [Table("RiskVersion")]
    public class RiskVersion : BaseEntity
    {
        /// <summary>
        /// 版本号
        /// </summary>
        public int VersionNo { get; set; }

        /// <summary>
        /// 创建人编号
        /// </summary>
        public int MyProperty { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTimeKind CreateDate { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
