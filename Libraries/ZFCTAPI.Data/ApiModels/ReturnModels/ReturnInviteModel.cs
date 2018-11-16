using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.ApiModels.ReturnModels
{
    /// <summary>
    /// 投资统计
    /// </summary>
    public class InviteStatistics
    {
        /// <summary>
        /// 累计收益
        /// </summary>
        public decimal AllReward { get; set; }
        /// <summary>
        /// 月收益
        /// </summary>
        public decimal MonthReward { get; set; }

        public int MonthInviter { get; set; }

        /// <summary>
        /// 邀请码
        /// </summary>
        public string InviteCode { get; set; }
        /// <summary>
        /// 邀请链接
        /// </summary>
        public string InviteUrl { get; set; }
        /// <summary>
        /// 邀请数量
        /// </summary>
        public int InviteCount { get; set; }
    }

    /// <summary>
    /// 月邀请概览
    /// </summary>
    public class InviteMonthAbstract
    {
        public InviteMonthAbstract()
        {
            RewardOverviews = new List<RewardOverview>();
        }

        public int Count { get; set; }

        public List<RewardOverview> RewardOverviews { get; set; }
    }

    public class RewardOverview
    {
        /// <summary>
        /// 年份
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 月份
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// 当月奖励金额
        /// </summary>
        public decimal MonthReward { get; set; }
        /// <summary>
        /// 当月邀请数量
        /// </summary>
        public int MonthInvite { get; set; }
        /// <summary>
        /// 当月注册数量
        /// </summary>
        public int RegisterCount { get; set; }
        /// <summary>
        /// 当月认证数量
        /// </summary>
        public int CertificationCount { get; set; }
        /// <summary>
        /// 当月投资数量
        /// </summary>
        public int InvestCount { get; set; }
    }

    /// <summary>
    /// 每月投资人详情
    /// </summary>
    public class InviteMonthDetail
    {
        public InviteMonthDetail()
        {
            InvesterDetails = new List<InvesterDetail>();
        }

        public int Count { get; set; }

        public List<InvesterDetail> InvesterDetails { get; set; }
    }

    public class InvesterDetail
    {
        /// <summary>
        /// 用户头像
        /// </summary>
        public string HeadPicUrl { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 投资金额
        /// </summary>
        public decimal InvestMoney { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public string RegisterTime { get; set; }
        /// <summary>
        /// 实名认证时间
        /// </summary>
        public string CertificationTime { get; set; }
        /// <summary>
        /// 投资时间
        /// </summary>
        public string InvestTime { get; set; }
    }
}
