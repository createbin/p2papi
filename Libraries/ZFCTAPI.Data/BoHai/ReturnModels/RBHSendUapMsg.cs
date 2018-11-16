using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.ReturnModels
{
    public class RBHSendUapMsg : RBHBaseModel
    {
        public RBHSendUapMsgBody SvcBody { get; set; }
    }

    public class RBHSendUapMsgBody
    {
        public string rtnCod { get; set; }
    }
}
