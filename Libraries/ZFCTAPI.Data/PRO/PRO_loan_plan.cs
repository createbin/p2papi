using System;
using System.Collections.Generic;
using System.Text;
using Dapper.Contrib.Extensions;

namespace ZFCTAPI.Data.PRO
{
    /// <summary>
	/// 数据表PRO_loan_plan的数据库实体类
	/// </summary>
	[Table("PRO_loan_plan")]
    public partial class PRO_loan_plan : BaseEntity
    {
        #region 基本信息

        /// <summary>
        ///  外键表：PRO_loan_apply
        /// </summary>
        public int? pro_loan_id { get; set; }

        /// <summary>
        ///  应还本金
        /// </summary>
        public decimal? pro_pay_money { get; set; }

        /// <summary>
        ///  应还利息
        /// </summary>
        public decimal? pro_pay_rate { get; set; }

        /// <summary>
        ///  应还罚金
        /// </summary>
        public decimal? pro_pay_over_rate { get; set; }

        /// <summary>
        ///  应还平台服务费
        /// </summary>
        public decimal? pro_pay_service_fee { get; set; }

        /// <summary>
        ///  应还担保服务费
        /// </summary>
        public decimal? pro_pay_guar_fee { get; set; }

        /// <summary>
        ///  应还总额
        /// </summary>
        public decimal? pro_pay_total { get; set; }

        /// <summary>
        ///  实还本金
        /// </summary>
        public decimal? pro_collect_money { get; set; }

        /// <summary>
        ///  实还利息
        /// </summary>
        public decimal? pro_collect_rate { get; set; }

        /// <summary>
        ///  实还罚金
        /// </summary>
        public decimal? pro_collect_over_rate { get; set; }

        /// <summary>
        ///  实还平台服务费
        /// </summary>
        public decimal? pro_collect_service_fee { get; set; }

        /// <summary>
        ///  实还担保服务费
        /// </summary>
        public decimal? pro_collect_guar_fee { get; set; }

        /// <summary>
        ///  实还总额
        /// </summary>
        public decimal? pro_collect_total { get; set; }

        /// <summary>
        ///  因罚金是时时计算的，当第一次罚金未还清时，保留剩余未还罚金
        /// </summary>
        public decimal? pro_sett_over_rate { get; set; }

        /// <summary>
        ///  期数
        /// </summary>
        public int? pro_loan_period { get; set; }

        /// <summary>
        ///  1：是 0：否
        /// </summary>
        public bool pro_is_clear { get; set; }

        /// <summary>
        ///  应还日期
        /// </summary>
        public DateTime? pro_pay_date { get; set; }

        /// <summary>
        ///  实还日期
        /// </summary>
        public DateTime? pro_collect_date { get; set; }

        /// <summary>
        ///  1：正常还款 2：平台代还 3：强制还款
        /// </summary>
        public int? pro_pay_type { get; set; }

        /// <summary>
        ///  1：是 0：否
        /// </summary>
        public bool pro_replace_state { get; set; }

        /// <summary>
        /// 是否正在使用
        /// </summary>
        public bool pro_is_use { get; set; }

        #endregion 基本信息

        /// <summary>
        /// 商户流水号(2018-3-27)
        /// </summary>
        public string tran_seq_no { get; set; }

        /// <summary>
        /// 还款提交时间
        /// </summary>
        public DateTime? CreDt { get; set; }

        /// <summary>
        /// 银行返回消息
        /// </summary>
        public string bank_resp_desc { get; set; }
    }
}