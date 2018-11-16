using System;
using System.Collections.Generic;
using System.Text;
using Dapper.Contrib.Extensions;

namespace ZFCTAPI.Data.CST
{
    /// <summary>
	/// 数据表CST_realname_prove的数据库实体类
	/// </summary>
	[Table("CST_realname_prove")]
    public partial class CST_realname_prove 
    {
        #region 基本信息

        /// <summary>
        /// 编号
        /// </summary>	
        public int Id { get; set; }

        /// <summary>
        ///  用户Id 
        /// </summary>	
        public int? cst_user_id { get; set; }

        /// <summary>
        ///  真实姓名 
        /// </summary>	
        public string cst_user_realname { get; set; }

        /// <summary>
        ///  性别 
        /// </summary>	
        public bool cst_user_sex { get; set; }

        /// <summary>
        ///  民族 
        /// </summary>	
        public string cst_user_nation { get; set; }

        /// <summary>
        ///  出身日期 
        /// </summary>	
        public DateTime? cst_user_birthdate { get; set; }

        /// <summary>
        ///  证件类型 
        /// </summary>	
        public int cst_card_type { get; set; }

        /// <summary>
        ///  证件号码 
        /// </summary>	
        public string cst_card_num { get; set; }

        /// <summary>
        ///  籍贯 
        /// </summary>	
        public string cst_user_native { get; set; }

        /// <summary>
        ///  身份证正面照片 
        /// </summary>	
        public string cst_card_front { get; set; }

        /// <summary>
        ///  身份证背面照片 
        /// </summary>	
        public string cst_card_behind { get; set; }

        /// <summary>
        ///  0 未审核 1 审核已通过 2 审核未通过 
        /// </summary>	
        public int? cst_realname_status { get; set; }
        #endregion
    }
}
