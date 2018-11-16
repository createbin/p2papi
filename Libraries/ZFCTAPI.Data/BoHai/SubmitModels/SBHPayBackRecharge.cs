using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.SubmitModels
{
    /// <summary>
    /// 还款充值
    /// </summary>
    public class SBHPayBackRecharge : SBHBaseModel
    {
        public SBHPayBackRechargeBody SvcBody { get; set; }
    }

    public class SBHPayBackRechargeBody
    {
        /// <summary>
        /// 用户平台编码
        /// Y
        /// </summary>
        public string platformUid { get; set; }

        /// <summary>
        /// 项目编号
        /// Y
        /// </summary>
        public string projectCode { get; set; }

        /// <summary>
        /// 入账金额
        /// Y
        /// </summary>
        public string amount { get; set; }
    }
}