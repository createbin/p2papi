using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ZFCTAPI.Data.POP
{
    [Table("POP_envelope_red")]
    public class POP_envelope_red:BaseEntity
    {
        /// <summary>
        /// pop_red_name
        /// </summary>	
        public string pop_red_name { get; set; }

        /// <summary>
        ///  红包类型（定向红包、系统红包、活动红包、产品红包） 
        /// </summary>	
        public string pop_red_type { get; set; }

        /// <summary>
        ///  系统红包类型 
        /// </summary>	
        public string pop_red_systemType { get; set; }

        /// <summary>
        ///  红包面值类型 
        /// </summary>	
        public string pop_red_faceValue { get; set; }

        /// <summary>
        ///  红包面值最大值 
        /// </summary>	
        public decimal? pop_red_faceValueMax { get; set; }

        /// <summary>
        ///  红包面值最小值 
        /// </summary>	
        public decimal? pop_red_faceValueMin { get; set; }

        /// <summary>
        ///  红包领取后有效期类型 
        /// </summary>	
        public string pop_red_expiryDateType { get; set; }

        /// <summary>
        ///  红包领取后有效期类型（无限制） 
        /// </summary>	
        public string pop_red_expiryDateUnlimited { get; set; }

        /// <summary>
        ///  红包领取后有效期类型（相对有效期） 
        /// </summary>	
        public int pop_red_expiryDateRelatively { get; set; }

        /// <summary>
        ///  红包领取后有效期类型（绝对有效期） 
        /// </summary>	
        public DateTime? pop_red_expiryDateAbsolute { get; set; }

        /// <summary>
        ///  使用限制 
        /// </summary>	
        public string pop_red_useAstrictType { get; set; }

        /// <summary>
        ///  发行预算 
        /// </summary>	
        public int? pop_red_distributionBudget { get; set; }

        /// <summary>
        ///  发行数量 
        /// </summary>	
        public int? pop_red_distributionNnumber { get; set; }

        /// <summary>
        ///  限领控制类型 
        /// </summary>	
        public string pop_red_controlAstrictType { get; set; }

        /// <summary>
        ///  领取次数 
        /// </summary>	
        public int? pop_red_getNumber { get; set; }

        /// <summary>
        ///  发行时间（开始） 
        /// </summary>	
        public DateTime? pop_red_distributionStartDate { get; set; }

        /// <summary>
        ///  发行时间（结束） 
        /// </summary>	
        public DateTime? pop_red_distributionStartEnd { get; set; }

        /// <summary>
        ///  使用说明 
        /// </summary>	
        public string pop_red_usingDictionary { get; set; }

        /// <summary>
        ///  红包LOGO 
        /// </summary>	
        public string pop_red_logo { get; set; }

        /// <summary>
        ///  发放提醒 
        /// </summary>	
        public string pop_red_grantRemind { get; set; }

        /// <summary>
        ///  发放提醒内容 
        /// </summary>	
        public string pop_red_grantRemindContent { get; set; }

        /// <summary>
        /// 是否发放
        /// </summary>
        public bool pop_red_grant { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime? pop_red_addTime { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool pop_red_IsDelete { get; set; }

        /// <summary>
        /// 是否可兑换
        /// </summary>
        public bool pop_red_isExchange { get; set; }

        /// <summary>
        /// 兑换消耗积分
        /// </summary>
        public decimal? pop_red_consumptionIntegral { get; set; }

        /// <summary>
        /// 是否可重复兑换
        /// </summary>
        public bool pop_red_isRepeatExchange { get; set; }

        /// <summary>
        /// 产品红包类型
        /// </summary>
        public string pop_red_productType { get; set; }
    }
}
