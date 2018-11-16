using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.ReturnModels
{
    public  class RBHExperBonus:RBHBaseModel
    {
        public RBHExperBonus()
        {
            SvcBody=new RBHExperBonusBody();
        }

        public RBHExperBonusBody SvcBody { get; set; }
    }

    public class RBHExperBonusBody
    {
        public string transId { get; set; }
    }
}
