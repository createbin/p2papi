using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.SYS
{
    /// <summary>
    /// 数据表SYS_area_info的数据库实体类
    /// </summary>
    public partial class SYS_area_info : BaseEntity
    {
        #region 递归
        /// <summary>
        /// 父级
        /// </summary>
        public virtual SYS_area_info Parent { get; set; }

        private ICollection<SYS_area_info> _children;

        public virtual ICollection<SYS_area_info> Children
        {
            get => _children ?? (_children = new List<SYS_area_info>());
            protected set => _children = value;
        }
        #endregion
        /// <summary>
        ///  地区名称 
        /// </summary>	
        public string sys_area_name { get; set; }
        /// <summary>
        ///  父级地区编号 
        /// </summary>	
        public int? sys_super_area { get; set; }
        /// <summary>
        ///  地区编号 
        /// </summary>	
        public string sys_area_code { get; set; }
        /// <summary>
        ///  0：省 1：市 2：县 
        /// </summary>	
        public int? sys_area_sign { get; set; }
    }
}
