using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.ApiModels.SubmitModels
{

    public class SMerchantRecharge:BaseSubmitModel
    {
        /// <summary>
        /// 2-营销账户 3-预付费账户
        /// </summary>
        public string AccountType { get; set; }
        /// <summary>
        /// 交易金额
        /// </summary>
        public string TranMoney { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }

    public class SQueryTransState : BaseSubmitModel
    {
        /// <summary>
        /// 2-营销账户 3-预付费账户
        /// </summary>
        public int TranscationId { get; set; }
    }

    public class SQueryMerchantTrans : BaseSubmitModel
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }

    public class MerchantToUserMoney : BaseSubmitModel
    {
        /// <summary>
        /// 营销活动编号
        /// </summary>
        public string CampaignCode { get; set; }
        /// <summary>
        /// 营销活动信息
        /// </summary>
        public string CampaignInfo { get; set; }

        /// <summary>
        /// 用户平台编码
        /// </summary>
        public List<int> AccountIds { get; set; }
        /// <summary>
        /// 交易金额
        /// </summary>
        public decimal CampaignMoney { get; set; }
        /// <summary>
        /// 消息扩展
        /// </summary>
        public string Extension { get; set; }
    }

    public class MerchantGetUser : BaseSubmitModel
    {
        /// <summary>
        /// 用户手机号
        /// </summary>
        public string UserPhone { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }
    }


    public class RefundRecords : BasePageModel
    {
        /// <summary>
        /// 用户手机号
        /// </summary>
        public string UserPhone { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        public string StartDate { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public string EndDate { get; set; }
    }
}
