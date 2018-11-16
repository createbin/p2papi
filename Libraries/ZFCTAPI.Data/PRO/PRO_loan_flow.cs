using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using ZFCTAPI.Data.CST;
using ZFCTAPI.Data.SYS;

namespace ZFCTAPI.Data.PRO
{
    /// <summary>
    /// 数据表PRO_loan_flow的数据库实体类
    /// </summary>
    [Table("PRO_loan_flow")]
    public partial class PRO_loan_flow : BaseEntity
    {
        #region 基本信息
        /// <summary>
        ///  主键ID 
        /// </summary>	
        //public int Id { get; set; }

        /// <summary>
        ///  外键表：PRO_loan_apply外键表：PRO_transfer_apply 
        /// </summary>	
        public int? pro_loan_id { get; set; }

        /// <summary>
        ///  环节名称 
        /// </summary>	
        public int? pro_step_id { get; set; }

        /// <summary>
        ///  操作人  外键表：CST_user_info 
        /// </summary>	
        public int? pro_operate_emp { get; set; }

        /// <summary>
        ///  操作日期 
        /// </summary>	
        public DateTime? pro_operate_date { get; set; }

        /// <summary>
        ///  转让申请ID
        /// </summary>	
        public int? pro_transfer_id { get; set; }

        /// <summary>
        ///  项目状态 
        /// </summary>	
        public int? pro_loan_stateId { get; set; }

        /// <summary>
        ///  备注 
        /// </summary>	
        public string pro_flow_mark { get; set; }
        #endregion

    }
}
