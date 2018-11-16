using System;
using Dapper.Contrib.Extensions;


namespace ZFCTAPI.Data.CST
{
    /// <summary>
    /// 数据表CST_red_info的数据库实体类
    /// </summary>
    [Table("CST_red_info")]
    public partial class CST_red_info : BaseEntity
    {
        /// <summary>
        ///  使用金额 
        /// </summary>	
        public decimal? cst_red_money { get; set; }

        /// <summary>
        ///  外键表：Customer 
        /// </summary>	
        public int? cst_user_id { get; set; }

        /// <summary>
        ///  红包ID 
        /// </summary>	
        public int? cst_red_id { get; set; }

        /// <summary>
        ///  使用规则ID 
        /// </summary>	
        public int? cst_red_useId { get; set; }

        /// <summary>
        ///  发放日期 
        /// </summary>	
        public DateTime? cst_create_date { get; set; }

        /// <summary>
        ///  是否已使用 
        /// </summary>	
        public bool cst_red_employ { get; set; }

        /// <summary>
        ///  使用有效期（开始） 
        /// </summary>	
        public DateTime? cst_red_startDate { get; set; }

        /// <summary>
        ///  使用有效期（结束） 
        /// </summary>	
        public DateTime? cst_red_endDate { get; set; }

        /// <summary>
        ///  剩余金额 
        /// </summary>	
        public decimal? cst_red_surplusAmt { get; set; }

        /// <summary>
        ///  可用次数 
        /// </summary>	
        public int? cst_red_number { get; set; }

        /// <summary>
        ///  是否可兑换 
        /// </summary>	
        public bool cst_red_exc { get; set; }

        /// <summary>
        /// 使用红包投资id
        /// </summary>
        public int? cst_red_investId { get; set; }

        /// <summary>
        ///  是否已划转
        /// </summary>	
        public bool cst_isTransfer { get; set; }

        /// <summary>
        /// 被邀请用户Id
        /// </summary>
        public int? cst_red_beInvited { get; set; }


        /// <summary>
        /// 用户红包是否领取 默认为已经领取 2016-09-18 calabash add
        /// </summary>
        public bool cst_red_recive { get; set; }

        /// <summary>
        /// 被取消的红包投资id 2016-12-01 calabash add
        /// </summary>
        public int? cst_cancel_investId { get; set; }
        /// <summary>
        /// 红包是否已经过期提醒 2017-01-19 calabash add
        /// </summary>
        public bool cst_is_remind { get; set; }
    }
}
