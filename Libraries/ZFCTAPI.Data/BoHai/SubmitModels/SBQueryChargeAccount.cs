using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.SubmitModels
{
    public class SBQueryChargeAccount : SBHBaseModel
    {
        public SBQueryChargeAccount()
        {
            SvcBody = new SBQueryChargeAccountModel();
        }

        public SBQueryChargeAccountModel SvcBody { get; set; }
    }

    public class SBQueryChargeAccountModel
    {
        /// <summary>
        /// 用户平台编码
        /// </summary>
        public string platformUid { get; set; }
    }
}
