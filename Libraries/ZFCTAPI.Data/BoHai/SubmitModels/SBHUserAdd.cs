using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.SubmitModels
{
    public class SBHUserAdd:SBHBaseModel
    {
        public SBHUserAdd()
        {
            SvcBody=new SBHUserAddBody();
        }

        public SBHUserAddBody SvcBody { get; set; }
    }

    public class SBHUserAddBody
    {
        /// <summary>
        /// 个人用户时传身份证号，企业用户时传组织机构代码，企业用户三证合一时此字段传三证号
        /// </summary>
        public string idcard { get; set; }
        /// <summary>
        /// 详见码表-证件类型
        /// </summary>
        public string identityType { get; set; }
        /// <summary>
        /// 详见码表-开户性质
        /// </summary>
        public string businessType { get; set; }
        ///// <summary>
        ///// Y
        ///// 开户类型
        ///// 1 投资户 2 融资户（如开户性质为企业，则账户类型必填2）
        ///// </summary>
        public string businessProperty { get; set; }
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
        /// 如果开户性质为企业，则必填：1-普通对公户 2-担保户
        /// </summary>
        public string accountTyp { get; set; }

        /// <summary>
        /// 个人用户时为个人姓名，企业用户时为企业联系人姓名
        /// </summary>
        public string truename { get; set; }
        /// <summary>
        /// 个人用户时为个人手机号，企业用户时为企业联系人手机号
        /// </summary>
        public string phonenum { get; set; }
        /// <summary>
        /// 登记人
        /// </summary>
        public string booker { get; set; }
        /// <summary>
        /// 开户地点
        /// </summary>
        public string openPlace { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string sex { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string email { get; set; }
        /// <summary>
        /// 固话
        /// </summary>
        public string tel { get; set; }
        /// <summary>
        /// 传真
        /// </summary>
        public string fax { get; set; }
        /// <summary>
        /// 联系地址
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string comment { get; set; }
        /// <summary>
        /// 税务登记号开户性质为企业用户时，且无三证号时必输*
        /// </summary>
        public string taxId { get; set; }
        /// <summary>
        /// 企业名称开户性质为企业用户时输入
        /// </summary>
        public string companyName { get; set; }
        /// <summary>
        /// 企业简称开户性质为企业用户时输入
        /// </summary>
        public string companyShortName { get; set; }
        /// <summary>
        /// 法人代表姓名开户性质为企业用户时必输*
        /// </summary>
        public string corperationName { get; set; }
        /// <summary>
        /// 法人代表英文名开户性质为企业用户时输入
        /// </summary>
        public string corperationEnName { get; set; }
        /// <summary>
        /// 法人代表证件类型开户性质为企业用户时必输*
        /// </summary>
        public string corperationIdentityType { get; set; }
        /// <summary>
        /// 法人代表证件号开户性质为企业用户时必输*
        /// </summary>
        public string corperationIdcard { get; set; }
        /// <summary>
        /// 银行编码开户性质为企业用户时输入
        /// </summary>
        public string bankCode { get; set; }
        /// <summary>
        /// 开户名称开户性质为企业用户时必输*
        /// </summary>
        public string accountName { get; set; }
        /// <summary>
        /// 银行账号开户性质为企业用户时输入
        /// </summary>
        public string bankAccount { get; set; }
        /// <summary>
        /// 营业执照号开户性质为企业用户时必输且无三证号时必输*
        /// </summary>
        public string licenseCode { get; set; }
        /// <summary>
        /// 营业执照年检时间开户性质为企业用户时输入，格式yyyy-MM-dd
        /// </summary>
        public string licenseDetectionTime { get; set; }
        /// <summary>
        /// 营业执照年检时间0：不允许；1：允许。开户性质为企业用户时必输*
        /// </summary>
        public string companyInvestmentPermit { get; set; }

    }
}
