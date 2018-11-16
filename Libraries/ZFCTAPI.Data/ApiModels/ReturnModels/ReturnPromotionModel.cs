using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.ApiModels.ReturnModels
{
    
    public class RNewsPageModel
    {
        public RNewsPageModel()
        {
            NewsPage = new ReturnPageData<NewsDetail>();
        }
        /// <summary>
        /// 类别ID
        /// </summary>
        public int CategoryId { get; set; }
        /// <summary>
        /// 类别名称
        /// </summary>
        public string CategoryName { get; set; }

        public ReturnPageData<NewsDetail> NewsPage { get; set; }
    }

    public class RNewsCountModel
    {
        public RNewsCountModel()
        {
            NewsList = new List<NewsDetail>();
        }
        /// <summary>
        /// 类别ID
        /// </summary>
        public int CategoryId { get; set; }
        /// <summary>
        /// 类别名称
        /// </summary>
        public string CategoryName { get; set; }

        public int Count { get; set; }

        public IList<NewsDetail> NewsList { get; set; }
    }

    /// <summary>
    /// 新闻详情
    /// </summary>
    public class NewsDetail
    {
        /// <summary>
        /// 新闻ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 简单描述
        /// </summary>
        public string Short { get; set; }
        /// <summary>
        /// 图片链接
        /// </summary>
        public string ImageUrl { get; set; }
        /// <summary>
        /// 跳转链接
        /// </summary>
        public string SkipUrl { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        public string Details { get; set; }
    }

    /// <summary>
    /// 分页获取广告
    /// </summary>
    public class RAdvertisementPageModel
    {
        /// <summary>
        /// 类别ID
        /// </summary>
        public int CategoryId { get; set; }
        /// <summary>
        /// 类别名称
        /// </summary>
        public string CategoryName { get; set; }

        public ReturnPageData<AdvertisementDetail> AdvertisementList { get; set; }
    }

    /// <summary>
    /// 按条获取广告
    /// </summary>
    public class RAdvertisementCountModel
    {
        public RAdvertisementCountModel()
        {
            AdvertisementList = new List<AdvertisementDetail>();
        }
        /// <summary>
        /// 类别ID
        /// </summary>
        public int CategoryId { get; set; }
        /// <summary>
        /// 类别名称
        /// </summary>
        public string CategoryName { get; set; }

        public int Count { get; set; }

        public IList<AdvertisementDetail> AdvertisementList { get; set; }
    }

    public class AdvertisementDetail
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int? AdvertisPositionId { get; set; }

        public string ImageUrl { get; set; }

        public string SkipUrl { get; set; }

        public int? Sort { get; set; }

        public string ProjectType { get; set; }

        public int? JumpPosition { get; set; }

        public string JumpInfo { get; set; }

        public int? State { get; set; }

        public DateTime? StartinTime { get; set; }

        public DateTime? EndTime { get; set; }

        public DateTime? CreateDate { get; set; }
    }

    /// <summary>
    /// 活动排名
    /// </summary>
    public class RActivityRankModel
    {
        /// <summary>
        /// 名次
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 投资金额
        /// </summary>
        public decimal InvestMoney { get; set; }
    }

    /// <summary>
    /// 投资记录
    /// </summary>
    public class RInvestRecordsModel
    {
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 投资金额
        /// </summary>
        public decimal InvestMoney { get; set; }
        /// <summary>
        /// 标的期数
        /// </summary>
        public int Period { get; set; }
        /// <summary>
        /// 投资时间
        /// </summary>
        public DateTime InvestDate { get; set; }
    }

    /// <summary>
    /// 活动期间用户投资奖励
    /// </summary>
    public class RInvestRewardModel
    {
        /// <summary>
        /// 投资金额
        /// </summary>
        public decimal InvestMoney { get; set; }
        /// <summary>
        /// 奖励金额
        /// </summary>
        public decimal RewardMoney { get; set; }
        /// <summary>
        /// 红包奖励
        /// </summary>
        public decimal RedRewardMoney { get; set; }
    }
}
