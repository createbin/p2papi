using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.ReturnModels
{
    public class RBHQueryUserInfo : RBHBaseModel
    {
        public RBHQueryUserInfoBody SvcBody { get; set; }
    }

    public class RBHQueryUserInfoBody
    {
        /// <summary>
        /// 证件类型
        /// </summary>
        public string identType { get; set; }

        /// <summary>
        /// 证件号码
        /// </summary>
        public string identNo { get; set; }

        /// <summary>
        /// 账户存管平台客户号
        /// </summary>
        public string plaCustId { get; set; }

        /// <summary>
        /// 绑卡信息
        /// </summary>
        public List<RBHQueryUserBankInfo> items { get; set; }

    }

    public class RBHQueryUserBankInfo{

        /// <summary>
        /// 1 投资户 2融资户
        /// </summary>
        public string capTyp { get; set; }

        /// <summary>
        /// 开户行3
        /// N
        /// </summary>
        public string capCorg { get; set; }
        /// <summary>
        /// 卡号
        /// N
        /// </summary>
        public string capCrdNo { get; set; }
        /// <summary>
        /// 绑定手机号
        /// N
        /// </summary>
        public string mblNo { get; set; }
        /// <summary>
        /// 姓名
        /// N
        /// </summary>
        public string usrNm { get; set; }
    }
}
