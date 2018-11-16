using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.ApiModels.SubmitModels
{
    public class SPromotionPageModel: BasePageModel
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string Code { get;set;}
    }

    public class SPromotionCountModel: BaseSubmitModel
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 条数
        /// </summary>
        public int Count { get; set; }
    }

    public class SFeedbackModel: BaseSubmitModel
    {
        /// <summary>
        /// 反馈内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 来源
        /// </summary>
        public int Source { get; set; }
    }

    public class SManagizeModel: BasePageModel
    {
        public int Year { get; set; }

        public int Count { get; set; }
        /// <summary>
        /// 企业内刊1,运营报告2
        /// </summary>
        public int Category { get; set; }
    }

    public class SDetailModel: BaseSubmitModel
    {

        public int Id { get; set; }
    }

    /// <summary>
    /// 推广活动
    /// </summary>
    public class SActivityModel: BaseSubmitModel
    {
        /// <summary>
        /// 活动ID
        /// </summary>
        public int ActivityId { get; set; }

        /// <summary>
        /// 条数
        /// </summary>
        public int? Count { get; set; }
    }

    /// <summary>
    /// 最新投资记录
    /// </summary>
    public class SInvestRecordsModel: BaseSubmitModel
    {
        public int Count { get; set; }
    }

    public class SPromotionDetailModel: BaseSubmitModel
    {
        public int Id { get; set; }
    }
}
