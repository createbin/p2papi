using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ZFCTAPI.Data.SYS;

namespace ZFCTAPI.Data.CST
{
    /// <summary>
    /// 数据表CST_realname_check的数据库实体类
    /// </summary>
    [Table("CST_realname_check")]
    public partial class CST_realname_check : BaseEntity
    {
        #region 数据字典相关
        /// <summary>
        /// 审核状态
        /// </summary>
        public virtual SYS_data_dictionary CheckState { get; set; }

        /// <summary>
        /// 实名认证表
        /// </summary>
        public virtual CST_realname_prove RealnameProve { get; set; }
        #endregion

        #region 基本信息
        /// <summary>
        ///  审核Id 
        /// </summary>	
        //public int Id { get; set; }

        /// <summary>
        ///  实名认证主键Id 
        /// </summary>	
        public int? cst_prove_id { get; set; }

        /// <summary>
        ///  审核人Id 
        /// </summary>	
        public int? cst_check_emp { get; set; }

        /// <summary>
        ///  审核日期 
        /// </summary>	
        public DateTime? cst_check_date { get; set; }

        /// <summary>
        ///  审核意见 
        /// </summary>	
        public string cst_check_idear { get; set; }

        /// <summary>
        ///  审核状态 
        /// </summary>	
        public int? cst_check_state { get; set; }
        #endregion

    }

}
