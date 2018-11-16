using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ZFCTAPI.Data.CST
{
    [Table("v_personal_account")]
    public partial class AccountImportModel
    {
        /// <summary>
        /// 记录号
        /// </summary>
        public string RECORD_ID { get; set; }
        /// <summary>
        /// 平台编码
        /// </summary>
        public string PLATFORM_CODE { get; set; }
        /// <summary>
        /// 平台用户编号
        /// </summary>
        public string PLATFORM_UID { get; set; }
        /// <summary>
        /// 开户性质
        /// </summary>
        public string BUSINESS_TYPE { get; set; }
        /// <summary>
        /// 证件类型
        /// </summary>
        public string IDENTITY_TYPE { get; set; }
        /// <summary>
        /// 证件号
        /// </summary>
        public string IDCARD { get; set; }
        /// <summary>
        /// 开户类别
        /// </summary>
        public string OPEN_TYPE { get; set; }
        /// <summary>
        /// 开户地点
        /// </summary>
        public string TRUENAME { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string OPEN_PLACE { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string PHONENUM { get; set; }
        /// <summary>
        /// 个人邮箱
        /// </summary>
        public string EMAIL { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string SEX { get; set; }
        /// <summary>
        /// 固话
        /// </summary>
        public string TEL { get; set; }
        /// <summary>
        /// 个人联系地址
        /// </summary>
        public string HOME_ADDRESS { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string act_account_no { get; set; }
        /// <summary>
        /// 银行卡号
        /// </summary>
        public string BANKCARD { get; set; }
        /// <summary>
        /// 银行代号
        /// </summary>
        public string BANKCODE { get; set; }
    }
}
