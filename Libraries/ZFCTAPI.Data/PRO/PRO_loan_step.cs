using System.ComponentModel.DataAnnotations.Schema;
using ZFCTAPI.Data.CST;
using ZFCTAPI.Data.SYS;

namespace ZFCTAPI.Data.PRO
{
    /// <summary>
    /// 数据表PRO_loan_step的数据库实体类
    /// </summary>
    [Table("PRO_loan_step")]
    public partial class PRO_loan_step : BaseEntity
    {
        public virtual PRO_loan_info LoanInfo { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public virtual SYS_data_dictionary StepStatus { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public virtual CST_user_info StatusUser { get; set; }
        /// <summary>
        /// 项目
        /// </summary>	
        public int? pro_step_loanId { get; set; }

        /// <summary>
        /// 当前步奏
        /// </summary>	
        public int? pro_this_step { get; set; }

        /// <summary>
        /// 下一步奏
        /// </summary>	
        public int? pro_next_step { get; set; }

        /// <summary>
        /// 上一步奏
        /// </summary>	
        public int? pro_up_step { get; set; }

        /// <summary>
        /// 机构id
        /// </summary>
        public int? pro_step_govId { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>
        public int? pro_step_status { get; set; }

        /// <summary>
        /// 审核人
        /// </summary>
        public int? pro_step_userid { get; set; }
    }
}
