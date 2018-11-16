using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ZFCTAPI.Data.SYS;

namespace ZFCTAPI.Data.CST
{
    /// <summary>
	/// 数据表CST_user_company的数据库实体类
	/// </summary>
	[Table("CST_user_company")]
	public partial class CST_user_company : BaseEntity
    {
        #region 关联数据
        /// <summary>
        /// 用户信息表
        /// </summary>
        public virtual CST_user_info UserInfo { get; set; }

        /// <summary>
        /// 单位性质
        /// </summary>
        public virtual SYS_data_dictionary CompanyType { get; set; }

        public virtual SYS_data_dictionary CompanyIndustry { get; set; }

        public virtual SYS_data_dictionary CompanyWorkYear { get; set; }

        #endregion

        #region 基本信息
        /// <summary>
        ///  主键ID 
        /// </summary>	
        //public int Id { get; set; }

        /// <summary>
        ///  外键表：CST_user_info 
        /// </summary>	
        public int? cst_user_id { get; set; }

        /// <summary>
        ///  单位名称 
        /// </summary>	
        public string cst_company_name { get; set; }

        /// <summary>
        ///  1：政府机关 2：国有企业 3：台（港、澳）资企业 4：合资企业 5：个体户 6：事业性单位 7：私营企业 
        /// </summary>	
        public int? cst_company_type { get; set; }

        /// <summary>
        ///  1：农、林、牧、渔业2：制造业
        ///3：电力、燃气及水的生产和供应业
        ///4：建筑业5：交通运输、仓储和邮政业
        ///6：信息传输、计算机服务和软件业
        ///7：批发和零售业8：金融业9：房地产业
        ///10：采矿业11：租赁和商务服务业
        ///12：科学研究、技术服务和地质勘查业
        ///13：水利、环境和公共设施管理业
        ///14：居民服务和其他服务业
        ///15：教育16：卫生、社会保障和社会福利业
        ///17：文化、体育和娱乐业18：公共管理和社会组织
        ///19：国际组织 
        /// </summary>	
        public int? cst_company_industry { get; set; }

        /// <summary>
        ///  1：一年以内 2：一年以上 3：二年以上 4：三年以上 5：四年以上 6：五年以上 7 六年以上 
        /// </summary>	
        public int? cst_work_year { get; set; }

        /// <summary>
        ///  职位 
        /// </summary>	
        public string cst_user_position { get; set; }

        /// <summary>
        ///  工作级别 
        /// </summary>	
        public string cst_user_level { get; set; }

        /// <summary>
        ///  工作电话 
        /// </summary>	
        public string cst_work_phone { get; set; }

        /// <summary>
        ///  工作时间 
        /// </summary>	
        public string cst_work_period { get; set; }

        /// <summary>
        ///  公司网站 
        /// </summary>	
        public string cst_company_site { get; set; }

        /// <summary>
        ///  公司地址 
        /// </summary>	
        public string cst_company_address { get; set; }

        /// <summary>
        ///  备注 
        /// </summary>	
        public string cst_company_mark { get; set; }
        #endregion
    }
}
