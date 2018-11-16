using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.SubmitModels
{
    public class SBHQueryChargeAccount:SBHBaseModel
    {
        public SBHQueryChargeAccount()
        {
            SvcBody=new SBHQueryChargeAccountBody();
        }

        public SBHQueryChargeAccountBody SvcBody { get; set; }
    }

    public class SBHQueryChargeAccountBody
    {
        /// <summary>
        /// 用户平台编码
        /// </summary>
        public string platformUid { get; set; }
    }
}
