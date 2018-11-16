using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.SubmitModels
{
    public class SBHMarketingCampaigns : SBHBaseModel
    {
        public SBHMarketingCampaignsBody SvcBody { get; set; }
    }
    public class SBHMarketingCampaignsBody
    {
        public string campaignCode { get; set; }

        public string campaignTotalMoney { get; set; }

        public string campaignInfo { get; set; }

        public string validTime { get; set; }

        public string batchNo { get; set; }

        public string batchCount { get; set; }

        public SBHMarketingCampaignsUserList userList { get; set; }

        public string extension { get; set; }
    }

    public class SBHMarketingCampaignsUserList
    {
        public string userPlatformCode { get; set; }

        public string campaignMoney { get; set; }

        public string couponType { get; set; } = "001";

        public string orderNo { get; set; }


    }
}
