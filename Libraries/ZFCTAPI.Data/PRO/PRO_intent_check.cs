using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ZFCTAPI.Data.CST;
using ZFCTAPI.Data.GOV;
using ZFCTAPI.Data.SYS;

namespace ZFCTAPI.Data.PRO
{
    /// <summary>
    /// 数据表PRO_intent_check的数据库实体类
    /// </summary>
    [Table("PRO_intent_check")]
    public partial class PRO_intent_check : BaseEntity
    {
        #region 基本信息

        /// <summary>
        ///  主键ID 
        /// </summary>	
        //public int Id { get; set; }

        /// <summary>
        ///  项目借款ID 
        /// </summary>	
        public int? pro_loan_id { get; set; }

        /// <summary>
        ///  审核机构ID
        /// </summary>	
        public int? pro_gov_id { get; set; }

        /// <summary>
        ///  审核环节(关联字典表) 
        /// </summary>	
        public int? pro_step_id { get; set; }
        /// <summary>
        /// 上一环节
        /// </summary>
        public int? pro_up_link { get; set; }

        /// <summary>
        ///  审核日期
        /// </summary>	
        public DateTime? pro_check_date { get; set; }

        /// <summary>
        ///  审核人
        /// </summary>	
        public int? pro_check_emp { get; set; }

        /// <summary>
        ///  审核状态
        /// </summary>	
        public int? pro_check_state { get; set; }

        /// <summary>
        ///  发送状态
        /// </summary>	
        public int? pro_send_state { get; set; }
        /// <summary>
        ///  审核意见
        /// </summary>	
        public string pro_check_idear { get; set; }


        #endregion
    }
}
