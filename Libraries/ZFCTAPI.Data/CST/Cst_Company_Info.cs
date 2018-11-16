using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ZFCTAPI.Data.CST
{
    [Table("Cst_Company_Info")]
    public class Cst_Company_Info : BaseEntity
    {
        /// <summary>
        /// cst_user_info 外键
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 汇付回执的用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 组织机构代码
        /// </summary>
        public string InstuCode { get; set; }

        /// <summary>
        /// 营业执照编号
        /// </summary>
        public string BusiCode { get; set; }

        /// <summary>
        /// 税务登记号
        /// </summary>
        public string TaxCode { get; set; }

        /// <summary>
        /// 企业用户备案金
        /// </summary>
        public decimal GuarCorpEarnestAmt { get; set; }

        /// <summary>
        /// 汇付审核状态
        /// </summary>
        public int AuditState { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 汇付审核状态描述
        /// </summary>
        public string AuditDesc { get; set; }

        public bool IsDelete { get; set; }

        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 大额充值户
        /// </summary>
        public bool ChargeAccount { get; set; }
        /// <summary>
        /// 企业联系人手机号
        /// </summary>
        public string ContactPhone { get; set; }
        /// <summary>
        /// 企业联系人姓名
        /// </summary>
        public string ContactUser { get; set; }
        /// <summary>
        /// 法人证件号
        /// </summary>
        public string CorperationIdCard { get; set; }

        /// <summary>
        /// 注册资本
        /// </summary>
        public decimal? RegisteredCapital { get; set; }
        /// <summary>
        /// 实缴资本
        /// </summary>
        public decimal? PaidCapital { get; set; }
        /// <summary>
        /// 所属行业
        /// </summary>
        public int? Industry { get; set; }
        /// <summary>
        /// 经营年限
        /// </summary>
        public int? BusinessLife { get; set; }

        /// <summary>
        /// 年收入
        /// </summary>
        public decimal? YearIncome { get; set; }
    }
}
