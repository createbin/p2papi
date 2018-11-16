using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.POP
{
    /// <summary>
    /// 数据表POP_envelope_rule的数据库实体类
    /// </summary>
    public partial class POP_envelope_rule : BaseEntity
    {
        /// <summary>
        /// 红包ID
        /// </summary>	
        public int? pop_envelope_id { get; set; }

        /// <summary>
        ///  代表当前规则属于那个条件 
        /// </summary>	
        public string pop_envelope_name { get; set; }

        /// <summary>
        ///  大于、等于、小于 
        /// </summary>	
        public string pop_envelope_rel { get; set; }

        /// <summary>
        ///  规则开始条件额度 
        /// </summary>	
        public decimal? pop_envelope_term { get; set; }

        /// <summary>
        ///  红包额度 
        /// </summary>	
        public decimal? pop_envelope_Amount { get; set; }
    }
}
