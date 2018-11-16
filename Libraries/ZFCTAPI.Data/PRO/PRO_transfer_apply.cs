using Dapper.Contrib.Extensions;
using System;

namespace ZFCTAPI.Data.PRO
{
    /// <summary>
    /// 数据表PRO_transfer_apply的数据库实体类
    /// </summary>
    [Table("PRO_transfer_apply")]
    public partial class PRO_transfer_apply : BaseEntity
    {
        #region 基本信息

        /// <summary>
        ///  项目申请标主键
        /// </summary>
        public int? pro_loan_id { get; set; }

        /// <summary>
        ///  投资申请表主键
        /// </summary>
        public int? pro_invest_id { get; set; }

        public virtual PRO_invest_info investInfo { get; set; }

        /// <summary>
        /// 转让期数
        /// </summary>
        public int? pro_transfer_period { get; set; }

        /// <summary>
        ///  转让人
        /// </summary>
        public int? pro_user_id { get; set; }

        /// <summary>
        ///  实际债权金额
        /// </summary>
        public decimal? pro_idual_money { get; set; }

        /// <summary>
        ///  转让金额
        /// </summary>
        public decimal? pro_transfer_money { get; set; }

        /// <summary>
        ///  转让后利率
        /// </summary>
        public decimal? pro_transfer_rate { get; set; }

        /// <summary>
        ///  转让费
        /// </summary>
        public decimal? pro_transfer_fee { get; set; }

        /// <summary>
        ///  转让费 (面向转让人收取)
        /// </summary>
        public decimal? pro_transfer_newfee { get; set; }

        /// <summary>
        ///  转让折让率
        /// </summary>
        public decimal? pro_transfer_harf_rate { get; set; }

        /// <summary>
        ///  转让折让费
        /// </summary>
        public decimal? pro_transfer_deduct_fee { get; set; }

        /// <summary>
        ///  转让申请日期
        /// </summary>
        public DateTime pro_transfer_date { get; set; }

        /// <summary>
        ///  转让满标日期
        /// </summary>
        public DateTime? pro_full_date { get; set; }

        /// <summary>
        ///  流标日期
        /// </summary>
        public DateTime? pro_missing_date { get; set; }

        /// <summary>
        ///  转让状态
        /// </summary>
        public int? pro_transfer_state { get; set; }

        /// <summary>
        /// 删除标记（初审未通过或流标的转让可以再次转让，将之前的转让标删除标记置为1）
        /// </summary>
        public bool pro_is_del { get; set; }

        /// <summary>
        /// 是否已经有部分认购人的钱划转了（如果有部分投资人钱已经划转成功，那么该笔转让仍然能够操作划转，但转让状态仍然为未划转）
        /// </summary>
        public bool pro_isAllTrans { get; set; }

        /// <summary>
        /// 剩余可投资金额
        /// </summary>
        public decimal? pro_surplus_money { get; set; }

        /// <summary>
        /// 是否正在使用
        /// </summary>
        public bool pro_is_use { get; set; }

        #endregion 基本信息
    }
}