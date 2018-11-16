using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.SubmitModels
{
    public class SBHOpenChargeAccount:SBHBaseModel
    {
        public SBHOpenChargeAccount()
        {
            SvcBody=new SBHOpenChargeAccountBody();
        }

        public SBHOpenChargeAccountBody SvcBody { get; set; }
    }

    public class SBHOpenChargeAccountBody
    {
        /// <summary>
        /// 客户端类型 1：电脑客户端2：移动客户端
        /// </summary>
        public string clientType { get; set; }

        /// <summary>
        /// 投融资户
        /// </summary>
        //public string platformUidInvestment { get; set; }
        /// <summary>
        /// 融资户
        /// </summary>
        //public string platformUidFinance { get; set; }
        
        
        public string platformUid { get; set; }

        /// <summary>
        /// 交易类别 1-新开 2-修改（修改户名和清算行号）
        /// </summary>
        public string txnTyp { get; set; }
        /// <summary>
        /// 账户类型 1-普通对公户 2-担保户
        /// </summary>
        public string accountTyp { get; set; }
        /// <summary>
        /// 对公账号
        /// </summary>
        public string accountNo { get; set; }
        /// <summary>
        /// 对公账户户名
        /// </summary>
        public string accountName { get; set; }
        /// <summary>
        /// 清算行号
        /// </summary>
        public string accountBk { get; set; }
        /// <summary>
        /// 页面返回url
        /// </summary>
        public string pageReturnUrl { get; set; }
    }
}
