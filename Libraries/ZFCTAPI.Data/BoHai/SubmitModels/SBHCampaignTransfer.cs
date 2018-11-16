using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.SubmitModels
{
    public class SBHCampaignTransfer : SBHBaseModel
    {
        public SBHCampaignTransfer()
        {
            SvcBody =new SBHCampaignTransferBody();
        }

        public SBHCampaignTransferBody SvcBody { get; set; }
    }

    public class SBHCampaignTransferBody
    {
        /// <summary>
        /// 用户平台编码
        /// </summary>
        public string platformUid { get; set; }
        /// <summary>
        /// 交易金额
        /// </summary>
        public string amount { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string comment { get; set; }
    }
}
