using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.ReturnModels
{
    public class RBHQueryMerchantTrans:RBHBaseModel
    {
        public RBHQueryMerchantTransBody SvcBody { get; set; }
    }

    public class RBHQueryMerchantTransBody
    {
        public string fileName { get; set; }
    }
}
