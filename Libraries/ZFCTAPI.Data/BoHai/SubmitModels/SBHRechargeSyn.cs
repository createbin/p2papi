using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.SubmitModels
{
    /// <summary>
    /// 线下充值余额同步
    /// </summary>
    public class SBHRechargeSyn : SBHBaseModel
    {
        public SBHRechargeSynBody SvcBody { get; set; }
    }

    public class SBHRechargeSynBody
    {

        /// <summary>
        /// 用户平台编码
        /// Y
        /// </summary>
        public string platformUid { get; set; }
    }
}
