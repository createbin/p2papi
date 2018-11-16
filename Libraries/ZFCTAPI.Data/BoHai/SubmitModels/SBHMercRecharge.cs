using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.SubmitModels
{
    public class SBHMercRecharge:SBHBaseModel
    {
        public SBHMercRecharge()
        {
            SvcBody=new SBHMercRechargeBody();
        }

        public SBHMercRechargeBody SvcBody { get; set; }
    }

    public class SBHMercRechargeBody
    {
        /// <summary>
        /// 账户类型
        /// </summary>
        public string accountType { get; set; }
        /// <summary>
        /// 交易金额
        /// </summary>
        public string amount { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 附言
        /// </summary>
        public string internalNote { get; set; }
        /// <summary>
        /// 摘要
        /// </summary>
        public string publicNote { get; set; }
    }
}
