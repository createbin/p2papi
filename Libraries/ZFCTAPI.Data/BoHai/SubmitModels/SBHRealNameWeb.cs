using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.SubmitModels
{
    public class SBHRealNameWeb:SBHBaseModel
    {
        public SBHRealNameWeb()
        {
            SvcBody=new SBHRealNameWebBody();
        }

        public SBHRealNameWebBody SvcBody { get; set; }
    }

    public class SBHRealNameWebBody
    {
        /// <summary>
        /// 客户端类型
        /// N
        /// 不填写默认为1
        /// 1：电脑客户端
        /// 2：移动客户端
        /// </summary>
        public string clientType { get; set; }
        ///// <summary>
        ///// 用户平台编码
        ///// </summary>
        public string platformUid { get; set; }
        /// <summary>
        /// 投资账户
        /// </summary>
        //public string platformUidInvestment { get; set; }

        /// <summary>
        /// 融资账户
        /// </summary>
        //public string platformUidFinance { get; set; }

        /// <summary>
        /// 证件类型 开户类型为 2、 3 不可空 
        /// </summary>
        public string identType { get; set; }
        /// <summary>
        /// 证件号码 开户类型为 2、 3 不可空 
        /// </summary>
        public string identNo { get; set; }
        /// <summary>
        /// 姓名 开户类型为 2、 3 不可空 
        /// </summary>
        public string usrName { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string mobileNo { get; set; }
        /// <summary>
        /// 开户银行代号
        /// </summary>
        public string openBankId { get; set; }
        /// <summary>
        ///开户银行账号
        /// </summary>
        public string openAcctId { get; set; }
        /// <summary>
        /// 页面返回url
        /// </summary>
        public string pageReturnUrl { get; set; }
    }
}
