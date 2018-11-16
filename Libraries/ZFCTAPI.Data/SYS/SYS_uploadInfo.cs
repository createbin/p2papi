using System;
using System.Collections.Generic;
using System.Text;
using Dapper.Contrib.Extensions;

namespace ZFCTAPI.Data.SYS
{
    /// <summary>
    /// 数据表SYS_uploadInfo的数据库实体类
    /// </summary>
    [Table("SYS_uploadInfo")]
    public partial class SYS_uploadInfo : BaseEntity
    {
        /// <summary>
        ///  模块Id 
        /// </summary>	
        public int? file_ClassId { get; set; }

        /// <summary>
        ///  与数据字典表关联 ，附件类型（身份证 、房产证之类） 
        /// </summary>	
        public int? file_type { get; set; }

        /// <summary>
        ///  模块名称 
        /// </summary>	
        public string file_ClassName { get; set; }

        /// <summary>
        ///  文件名称 
        /// </summary>	
        public string file_Name { get; set; }

        /// <summary>
        ///  文件原名 
        /// </summary>	
        public string file_formerly_name { get; set; }

        /// <summary>
        ///  文件路径 
        /// </summary>	
        public string file_Path { get; set; }

        /// <summary>
        ///  文件上传时间 
        /// </summary>	
        public DateTime? file_CreateTime { get; set; }
    }
}
