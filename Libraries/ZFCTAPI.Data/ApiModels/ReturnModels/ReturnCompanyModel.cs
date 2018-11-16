using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.ApiModels.ReturnModels
{
    public class RCompanyRealInfo
    {
        /// <summary>
        /// 组织机构代码
        /// </summary>
        public string IdCard { get; set; }
        /// <summary>
        /// 企业联系人
        /// </summary>
        public string ContactUser { get; set; }
        /// <summary>
        /// 企业联系人手机号
        /// </summary>
        public string ContactPhone { get; set; }
        /// <summary>
        /// 税务登记号
        /// </summary>
        public string TaxId { get; set; }
        /// <summary>
        /// 企业名称
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 法人代表姓名
        /// </summary>
        public string CorperationName { get; set; }
        /// <summary>
        /// 法人证件号
        /// </summary>
        public string CorperationIdCard { get; set; }
        /// <summary>
        /// 营业执照号
        /// </summary>
        public string LicenseCode { get; set; }
        /// <summary>
        /// 开户名称
        /// </summary>
        public string AccountName { get; set; }
        /// <summary>
        /// 是否结算开户
        /// </summary>
        public string JieSuan { get; set; } = "0";
        /// <summary>
        /// 是否渤海开户
        /// </summary>
        public string BoHai { get; set; } = "0";
        /// <summary>
        /// 是否首次结算开户
        /// </summary>
        public string OnceJieSuan { get; set; } = "0";
        /// <summary>
        /// 是否首次渤海开户
        /// </summary>
        public string OnceBoHai { get; set; } = "0";
        /// <summary>
        /// 结算返回信息
        /// </summary>
        public string JieSuanMsg { get; set; } = "";
        /// <summary>
        /// 渤海返回信息
        /// </summary>
        public string BoHaiMsg { get; set; } = "";
        /// <summary>
        /// 类型
        /// </summary>
        public string CompanyType = "";
        /// <summary>
        /// 行外对公账号
        /// </summary>
        public string AccountNo = "";
        /// <summary>
        /// 清算行号
        /// </summary>
        public string AccountBk = "";
        /// <summary>
        /// 线下充值账号
        /// </summary>
        public string ChargeAccountNo = "";
        /// <summary>
        /// 线下充值户名
        /// </summary>
        public string ChargeAccountName = "";
        /// <summary>
        /// 实名状态
        /// </summary>
        public string RealNameState = "0";
        /// <summary>
        /// 实名认证金额
        /// </summary>
        public string RealNameMoney = "0.00";

    }

    public class RCompanyState
    {
        public bool JieSuan { get; set; }

        public bool ChargeAccount { get; set; }
    }

    public class RCompanyStatics
    {
        /// <summary>
        /// 累计融资比数
        /// </summary>
        public string AllLoanCount { get; set; }
        /// <summary>
        /// 累计融资金额
        /// </summary>
        public string AllLoanMoney { get; set; }
        /// <summary>
        /// 累计还款笔数
        /// </summary>
        public string RepayedPlanCount { get; set; }
        /// <summary>
        /// 累计还款金额
        /// </summary>
        public string RepayedPlanMoney { get; set; }
        /// <summary>
        /// 剩余待还笔数
        /// </summary>
        public string RepayingPlanCount { get; set; }
        /// <summary>
        /// 剩余待还金额
        /// </summary>
        public string RepayingPlanMoney { get; set; }
        /// <summary>
        /// 历史逾期次数
        /// </summary>
        public string OverduePlanCount { get; set; }
        /// <summary>
        /// 历史逾期金额
        /// </summary>
        public string OverduePlanMoney { get; set; }

        /// <summary>
        /// 企业户融资统计
        /// </summary>
        public List<LoanStatics> LoanStatics { get; set; }=new List<LoanStatics>();
    }

    public class LoanStatics
    {
        /// <summary>
        /// 借款月份
        /// </summary>
        public string MonthType { get; set; }
        /// <summary>
        /// 月份对应的数量
        /// </summary>
        public string MonthCount { get; set; }
        /// <summary>
        /// 月份对应的金额
        /// </summary>
        public string MonthMoney { get; set; }
    }
}
