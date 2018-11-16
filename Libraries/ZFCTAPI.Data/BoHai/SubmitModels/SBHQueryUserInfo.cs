using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.SubmitModels
{
    public class SBHQueryUserInfo : SBHBaseModel
    {
        public SBHQueryUserInfo()
        {
            SvcBody=new SBHQueryUserInfoBody();
        }

        public SBHQueryUserInfoBody SvcBody { get; set; }
    }

    public class SBHQueryUserInfoBody
    {
        /// <summary>
        /// 账户存管平台客户号
        /// </summary>
        public string plaCustId { get; set; }

        /// <summary>
        /// 银行预留手机号码
        /// </summary>
        public string mblNo { get; set; }
    }
}
