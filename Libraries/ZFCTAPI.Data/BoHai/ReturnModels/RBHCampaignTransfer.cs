using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.ReturnModels
{
    public class RBHCampaignTransfer:RBHBaseModel
    {
        public RBHCampaignTransfer()
        {
            SvcBody = new RBHCampaignTransferBody();
        }

        public RBHCampaignTransferBody SvcBody { get; set; }
    }

    public class RBHCampaignTransferBody
    {
        /// <summary>
        /// 消息扩展
        /// </summary>
        public string extension { get; set; }
    }
}
