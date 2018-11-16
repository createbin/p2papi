using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using ZFCTAPI.Data.CST;
using ZFCTAPI.Data.SYS;

namespace ZFCTAPI.Data.PRO
{
    /// <summary>
	/// 数据表PRO_invest_info的数据库实体类
	/// </summary>
    [Table("PRO_invest_info")]
    public partial class PRO_invest_info : BaseEntity
    {
        #region 基本信息

        /// <summary>
        ///  贷款ID
        /// </summary>
        public int? pro_loan_id { get; set; }
        /// <summary>
        /// guid
        /// </summary>
        public Guid? pro_invest_guid { get; set; }

        /// <summary>
        ///  转让申请ID
        /// </summary>
        public int? pro_transfer_id { get; set; }

        /// <summary>
        ///  投资人
        /// </summary>
        public int? pro_invest_emp { get; set; }

        /// <summary>
        ///  投资金额
        /// </summary>
        public decimal? pro_invest_money { get; set; }

        /// <summary>
        ///  使用体验金金额
        /// </summary>
        public decimal? pro_experience_money { get; set; }

        /// <summary>
        ///  实际项目金额
        /// </summary>
        public decimal? pro_credit_money { get; set; }

        /// <summary>
        ///  投资日期
        /// </summary>
        public DateTime? pro_invest_date { get; set; }

        /// <summary>
        ///  投标订单流水号
        /// </summary>
        public string pro_order_no { get; set; }

        /// <summary>
        ///  投标冻结订单流水号 pro_fro_orderno
        /// </summary>
        public string pro_fro_orderno { get; set; }

        /// <summary>
        ///  投标订单日期
        /// </summary>
        public DateTime? pro_order_date { get; set; }

        /// <summary>
        ///  商户专属交易唯一标识
        /// </summary>
        public string pro_transact_mark { get; set; }

        /// <summary>
        ///  冻结状态
        /// </summary>
        public bool pro_frozen_state { get; set; }

        /// <summary>
        ///  划转状态
        /// </summary>
        public bool pro_transfer_state { get; set; }

        /// <summary>
        ///  投标类型
        /// </summary>
        public int? pro_invest_type { get; set; }

        /// <summary>
        ///  删除标记
        /// </summary>
        public bool pro_delsign { get; set; }

        /// <summary>
        ///  投资时候可以使用加息卷
        /// </summary>
        public decimal? pro_reality_accrual { get; set; }

        /// <summary>
        ///  实际支付金额
        /// </summary>
        public decimal? pro_creditPay_money { get; set; }

        /// <summary>
        ///  1：从PC端投资 2：手机端投资
        /// </summary>
        public int? pro_invest_source { get; set; }

        /// <summary>
        ///  是否手机专享
        /// </summary>
        public bool is_onlyPhone { get; set; }

        /// <summary>
        ///  是否投资成功
        /// </summary>
        public bool is_invest_succ { get; set; }

        /// <summary>
        /// 是否正在使用
        /// </summary>
        public bool pro_is_use { get; set; }

        /// <summary>
        /// 是否进行收益复投
        /// </summary>
        public bool pro_is_repeat { get; set; }
        /// <summary>
        /// 投资人风险类型 gaochao 2018-02-28
        /// </summary>
        public int? pro_invester_risktype { get; set; }

        #endregion 基本信息

        #region 2016-09-08 calabash add

        /// <summary>
        /// 当前投资时理财经理所属
        /// </summary>
        public int? WealthManagerId { get; set; }

        /// <summary>
        /// 投资来源 1:pc 2：wechat 3:app
        /// </summary>
        public int? InvestSource { get; set; }

        /// <summary>
        /// 流水号
        /// </summary>
        public string tran_seq_no { get; set; }

        #endregion 2016-09-08 calabash add
    }
}