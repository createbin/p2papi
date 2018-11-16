using System;
using System.Collections.Generic;
using System.Text;
using ZFCTAPI.Data.CST;

namespace ZFCTAPI.Data.MultiTable
{
    public class CompleteUserAccount
    {
        /// <summary>
        /// 用户基础信息
        /// </summary>
        public CST_user_info LoanUserInfo { get; set; }=new CST_user_info();
        /// <summary>
        /// 用户关联的企业信息
        /// </summary>
        public Cst_Company_Info CstCompanyInfo { get; set; }=new Cst_Company_Info();
        /// <summary>
        /// 个人账户信息
        /// </summary>
        public CST_account_info AccountInfo { get; set; }=new CST_account_info();
        /// <summary>
        /// 用户详细信息
        /// </summary>
        public CST_realname_prove RealnameProves { get; set; }=new CST_realname_prove();
        /// <summary>
        /// 用户公司信息
        /// </summary>
        public CST_user_company UserCompanies { get; set; }=new CST_user_company();
    }
}
