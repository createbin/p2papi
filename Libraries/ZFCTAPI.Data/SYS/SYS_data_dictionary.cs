using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ZFCTAPI.Data.SYS
{
    /// <summary>
    /// 数据表SYS_data_dictionary的数据库实体类
    /// </summary>
    [Table("SYS_data_dictionary")]
    public partial class SYS_data_dictionary : BaseEntity
    {
        #region 递归
        /// <summary>
        /// 父级
        /// </summary>
        public virtual SYS_data_dictionary Parent { get; set; }

        private ICollection<SYS_data_dictionary> _children;

        public virtual ICollection<SYS_data_dictionary> Children
        {
            get => _children ?? (_children = new List<SYS_data_dictionary>());
            protected set => _children = value;
        }
        #endregion

        #region 基本信息
        /// <summary>
        ///  名称 
        /// </summary>	
        public string sys_data_name { get; set; }

        /// <summary>
        ///  父级编号 
        /// </summary>	
        public int? sys_parent_code { get; set; }

        /// <summary>
        ///  编号 
        /// </summary>	
        public string sys_data_code { get; set; }

        /// <summary>
        ///  排序 
        /// </summary>	
        public int? sys_data_sort { get; set; }

        /// <summary>
        ///  备注 
        /// </summary>	
        public string sys_data_remarks { get; set; }

        #endregion
    }
}
