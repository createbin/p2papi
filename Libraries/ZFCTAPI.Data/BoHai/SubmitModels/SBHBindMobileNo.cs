using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.SubmitModels
{
    public class SBHBindMobileNo : SBHBaseModel
    {
        public SBHBindMobileNo()
        {
            SvcBody=new SBHBindMobileNoBody();
        }
        public SBHBindMobileNoBody SvcBody { get; set; }
    }

    public class SBHBindMobileNoBody
    {
        /// <summary>
        /// 客户端类型(1：电脑客户端 2：移动客户端)
        /// </summary>
        public string clientType { get; set; }
        /// <summary>
        /// 用户平台编码
        /// </summary>
        public string platformUid { get; set; }
        /// <summary>
        /// 账户存管平台客户号
        /// </summary>
        public string plaCustId { get; set; }
        /// <summary>
        /// 证件类型(clientType为2时必填)
        /// </summary>
        public string identType { get; set; }
        /// <summary>
        /// 证件号码(clientType为2时必填)
        /// </summary>
        public string identNo { get; set; }
        /// <summary>
        /// 老手机号(clientType为2时必填)
        /// </summary>
        public string oldMobileNo { get; set; }
        /// <summary>
        ///新手机号
        /// </summary>
        public string mobileNo { get; set; }
        /// <summary>
        ///页面返回url
        /// </summary>
        public string pageReturnUrl { get; set; }
        /// <summary>
        /// 用户名(clientType为2时必填 v1.5新增)
        /// </summary>
        public string usrName { get; set; }
        /// <summary>
        /// 账户类型  1 投资户  2 融资户
        /// </summary>
        public string transTyp { get; set; }
    }

    public class SBHBindMobileNoWebBody
    {
        public string char_set { get; set; }

        public string partner_id { get; set; }

        public string version_no { get; set; }

        public string biz_type { get; set; }

        public string sign_type { get; set; }

        public string MerBillNo { get; set; }

        public string PlaCustId { get; set; }

        public string MobileNo { get; set; }

        public string PageReturnUrl { get; set; }

        public string BgRetUrl { get; set; }

        public string MerPriv { get; set; }

        public string mac { get; set; }
    }
}
