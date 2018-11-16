using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ZFCTAPI.Data.SYS;

namespace ZFCTAPI.Data.PRO
{
    /// <summary>
    /// 数据表PRO_financial_product的数据库实体类
    /// </summary>
    [Table("PRO_financial_product")]
    public partial class PRO_financial_product : BaseEntity
    {
        #region 关联数据
        private ICollection<PRO_loan_info> _loanInfos;

        public virtual ICollection<PRO_loan_info> LoanInfos
        {
            get => _loanInfos ?? (_loanInfos = new List<PRO_loan_info>());
            protected set => _loanInfos = value;
        }

        /// <summary>
        /// 结算方式
        /// </summary>
        public virtual SYS_data_dictionary InvestWay { get; set; }

        #endregion

        #region 基本信息

        /// <summary>
        ///  主键ID 
        /// </summary>	
        //public int Id { get; set; }

        /// <summary>
        ///  理财产品名称 
        /// </summary>	
        public string pro_prod_name { get; set; }

        /// <summary>
        ///  产品期限（最小） 
        /// </summary>	
        public int? pro_min_term { get; set; }

        /// <summary>
        ///  产品期限（最大） 
        /// </summary>	
        public int? pro_max_term { get; set; }

        /// <summary>
        ///  产品利率（最小） 
        /// </summary>	
        public decimal? pro_min_rate { get; set; }

        /// <summary>
        ///  产品利率（最大） 
        /// </summary>	
        public decimal? pro_max_rate { get; set; }

        /// <summary>
        ///  还款方式 
        /// </summary>	
        public string pro_prod_repayment { get; set; }

        /// <summary>
        ///  投资方式（数据字典配置） 
        /// </summary>	
        public int? pro_invest_way { get; set; }

        /// <summary>
        ///  产品介绍 
        /// </summary>	
        public string pro_prod_introduce { get; set; }

        /// <summary>
        ///  产品logo 
        /// </summary>	
        public string pro_prod_logo { get; set; }

        /// <summary>
        ///  产品简介 
        /// </summary>	
        public string pro_prod_profile { get; set; }

        #endregion
    }
}
