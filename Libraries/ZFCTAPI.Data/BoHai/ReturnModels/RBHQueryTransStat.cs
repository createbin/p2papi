using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.ReturnModels
{
    public class RBHQueryTransStat : RBHBaseModel
    {
        public RBHQueryTransStatBody SvcBody { get; set; }
    }

    public class RBHQueryTransStatBody
    {
        /// <summary>
        /// 交易状态
        /// </summary>
        public string transStat { get; set; }
        /// <summary>
        /// 冻结编号
        /// </summary>
        public string freezeId { get; set; }
        /// <summary>
        /// 失败原因
        /// </summary>
        public string falRsn { get; set; }

    }
}
