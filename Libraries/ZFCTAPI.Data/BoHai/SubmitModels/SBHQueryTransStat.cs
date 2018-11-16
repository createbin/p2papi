using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.SubmitModels
{
    public class SBHQueryTransStat : SBHBaseModel
    {
        public SBHQueryTransStat()
        {
            SvcBody=new SBHQueryTransStatBody();
        }

        public SBHQueryTransStatBody SvcBody { get; set; }
    }

    public class SBHQueryTransStatBody
    {
        /// <summary>
        /// 原交易商户流水号
        /// </summary>
        public string merBillNo { get; set; }
        /// <summary>
        /// 查询交易类型
        /// 查询各联机资金类交易明细，非资金类不可查询如实名、建标等。 可查询类型举例：“用户充值（页面方式）-WebRecharge”。
        /// </summary>
        public string queryTransType { get; set; }
    }
}
