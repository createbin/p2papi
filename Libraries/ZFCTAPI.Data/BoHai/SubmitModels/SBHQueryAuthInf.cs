using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.SubmitModels
{
    public class SBHQueryAuthInf : SBHBaseModel
    {
        public SBHQueryAuthInf()
        {
            SvcBody = new SBHQueryAuthInfBody();
        }

        public SBHQueryAuthInfBody SvcBody { get; set; }
    }

    public class SBHQueryAuthInfBody
    {
        public string plaCustId { get; set; }
    }
}
