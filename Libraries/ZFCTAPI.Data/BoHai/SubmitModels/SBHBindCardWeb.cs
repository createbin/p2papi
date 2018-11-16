using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.SubmitModels
{
    public class SBHBindCardWeb : SBHBaseModel
    {
        public SBHBindCardWeb()
        {
            SvcBody = new SBHBindCardWebBody();
        }

        public SBHBindCardWebBody SvcBody { get; set; }
    }

    public class SBHBindCardWebBody
    {
        /// <summary>
        /// 客户端类型
        /// N
        /// 不填写默认为1
        /// 1：电脑客户端
        /// 2：移动客户端
        /// </summary>
        public string clientType { get; set; }

        /// <summary>
        /// 用户投资户平台编码
        /// </summary>
        //public string platformUidInvestment { get; set; }

        /// <summary>
        /// 用户融资户平台编码
        /// </summary>
        //public string platformUidFinance { get; set; }


        //用户平台编码
        public string platformUid { get; set; }

        /// <summary>
        /// 账户存管平台客户号
        /// </summary>
        public string plaCustId { get; set; }

        /// <summary>
        /// 页面返回url
        /// </summary>
        public string pageReturnUrl { get; set; }
    }
}