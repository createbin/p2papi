using System;
using System.Collections.Generic;
using System.Text;
using Dapper.Contrib.Extensions;
using ZFCTAPI.Data.CST;
using ZFCTAPI.Data.SYS;

namespace ZFCTAPI.Data.PRO
{
    /// <summary>
    /// 数据表PRO_transfer_check的数据库实体类
    /// </summary>
    [Table("PRO_transfer_check")]
    public partial class PRO_transfer_check : BaseEntity
    {
        #region 关联数据
        public virtual PRO_transfer_apply ProTransferApply { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>
        public virtual SYS_data_dictionary CheckState { get; set; }
        #endregion

        #region 基本信息
        /// <summary>
        ///  审核表主键 
        /// </summary>	
        //public int Id { get; set; }

        /// <summary>
        ///  转让申请Id 
        /// </summary>	
        public int? pro_transfer_id { get; set; }

        /// <summary>
        ///  1：转让审核 2：转让满标审核 
        /// </summary>	
        public int? pro_check_step { get; set; }

        /// <summary>
        ///  审核日期 
        /// </summary>	
        public DateTime? pro_check_date { get; set; }

        /// <summary>
        ///  审核人 
        /// </summary>	
        public int? pro_check_emp { get; set; }

        public virtual CST_user_info checkEmp { get; set; }

        /// <summary>
        ///  审核意见 
        /// </summary>	
        public string pro_check_idear { get; set; }

        /// <summary>
        ///  审核状态 
        /// </summary>	
        public int? pro_check_state { get; set; }
        #endregion

    }
}
