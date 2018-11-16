using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.SubmitModels
{
    public class SBHExperBonus : SBHBaseModel
    {
        public SBHExperBonus()
        {
            SvcBody=new SBHExperBonusBody();
        }

        public SBHExperBonusBody SvcBody { get; set; }
    }

    public class SBHExperBonusBody
    {
        /// <summary>
        /// 营销活动编号
        /// </summary>
        public string campaignCode { get; set; }
        /// <summary>
        /// 营销活动信息
        /// </summary>
        public string campaignInfo { get; set; }
        /// <summary>
        /// 用户平台编码
        /// </summary>
        public string userPlatformCode { get; set; }
        /// <summary>
        /// 红包金额
        /// </summary>
        public string campaignMoney { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string orderNo { get; set; }
        /// <summary>
        /// 消息扩展
        /// </summary>
        public string extension { get; set; }
    }
}
