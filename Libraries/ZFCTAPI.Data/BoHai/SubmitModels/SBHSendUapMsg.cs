using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.SubmitModels
{
    public class SBHSendUapMsg : SBHBaseModel
    {
        public SBHSendUapMsgBody SvcBody { get; set; }
    }

    public class SBHSendUapMsgBody
    {
        public string mobileNo { get; set; }
    }
}
