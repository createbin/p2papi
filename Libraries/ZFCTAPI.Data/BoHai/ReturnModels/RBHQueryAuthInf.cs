using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.ReturnModels
{
    public class RBHQueryAuthInf: RBHBaseModel
    {
        public RBHQueryAuthInfBody SvcBody { get; set; }=new RBHQueryAuthInfBody();
    }

    public class RBHQueryAuthInfBody
    {
        public List<RBHAuthResult> items { get; set; } = new List<RBHAuthResult>();
    }
}
