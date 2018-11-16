using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.POP
{
    public class POP_red_userule:BaseEntity
    {
        /// <summary>
        ///  红包ID 
        /// </summary>	
        public int? pop_envelope_id { get; set; }

        /// <summary>
        ///  规则名称 
        /// </summary>	
        public string pop_red_name { get; set; }

        /// <summary>
        ///  规则开始条件额度 
        /// </summary>	
        public decimal? pop_red_term { get; set; }

        /// <summary>
        ///  类型（使用范围，其它限制） 
        /// </summary>	
        public string pop_type { get; set; }
    }
}
