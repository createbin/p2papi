using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.ReturnModels
{
    public class RBHRevoke: RBHBaseModel
    {
        public RBHRevokeBody SvcBody { get; set; }
    }

    public class RBHRevokeBody
    {
        public string transId { get; set; }
    }
}
