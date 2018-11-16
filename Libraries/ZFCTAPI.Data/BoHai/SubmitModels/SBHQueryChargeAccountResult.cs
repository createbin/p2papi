using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.SubmitModels
{
    public class SBHQueryChargeAccountResult: SBHBaseModel
    {
        public SBHQueryChargeAccountResult()
        {
            SvcBody = new SBHQueryChargeAccountResultBody();
        }

        public SBHQueryChargeAccountResultBody SvcBody { get; set; }
    }


    public class SBHQueryChargeAccountResultBody
    {
        public string accountNo { get; set; }
        public string accountTyp { get; set; }
    }
}
