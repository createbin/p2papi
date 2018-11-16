using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.SubmitModels
{
    public class SBHUserCancel:SBHBaseModel
    {
        public SBHUserCancel()
        {
            SvcBody=new SBHUserCancelBody();
        }

        public SBHUserCancelBody SvcBody { get; set; }
    }

    public class SBHUserCancelBody
    {
        public string platformUid { get; set; }

        public string extension { get; set; }
    }
}
