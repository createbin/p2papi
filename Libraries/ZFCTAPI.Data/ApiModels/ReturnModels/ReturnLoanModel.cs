using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.ApiModels.ReturnModels
{
    public class RHomeLoanInfo
    {
        /// <summary>
        /// 项目id
        /// </summary>
        public int LoanId { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string LoanName { get; set; }
        /// <summary>
        /// 年华收益率
        /// </summary>
        public decimal YearRate { get; set; }
        /// <summary>
        /// 原始收益率
        /// </summary>
        public decimal? OriginalRate { get; set; }
        /// <summary>
        /// 加息利率
        /// </summary>
        public decimal? HikeRate { get; set; }
        /// <summary>
        /// 投资期限
        /// </summary>
        public int Period { get; set; }
        /// <summary>
        /// 还款方式
        /// </summary>
        public string RepaymentType { get; set; }
        /// <summary>
        /// 项目进度
        /// </summary>
        public decimal LoanSpeed { get; set; }
        /// <summary>
        /// 剩余可投金额
        /// </summary>
        public decimal SurplusMoney { get; set; }
        /// <summary>
        /// 是否可用红包
        /// </summary>
        public bool IsRedPackage { get; set; }
        /// <summary>
        /// 是否可以转让
        /// </summary>
        public bool IsTransfer { get; set; }
        /// <summary>
        /// 项目状态
        /// </summary>
        public string LoanState { get; set; }
        /// <summary>
        /// 项目状态id
        /// </summary>
        public int LoanStateId { get; set; }
        /// <summary>
        /// 还款方式
        /// </summary>
        public string RepeymentTypeCode { get; set; }
        /// <summary>
        /// 最少投资金额
        /// </summary>
        public decimal LeastMoney { get; set; }
        /// <summary>
        /// 最多允许投资金额
        /// </summary>
        public decimal MaxMoney { get; set; }
    }

    public class RLoanAbstract : RHomeLoanInfo
    {

    }
    public class RLoanAbstractLable : RHomeLoanInfo
    {
        public int PeriodDay { get; set; }

        public string Lables { get; set; }

        public string LableIcons { get; set; }
    }

    public class RLoanDetail : RLoanAbstract
    {
        public decimal LoanMoney { get; set; }

        public string ProductType { get; set; }

        public decimal MinInvestMoney { get; set; }

        public decimal MaxInvestMoney { get; set; }

        public string Lables { get; set; }

        public string LableIcons { get; set; }

        public int LoanType { get; set; }

        public string LoanTypeName { get; set; }

        public string PublishDate { get; set; }

        /// <summary>
        /// 标的风险类型 by gaochao 2018 02 28
        /// </summary>
        public string LoanRiskType { get; set; }

        public string IsCompany { get; set; }

    }

    #region 借款人信息
    public class RLoanerInfo
    {
        public RLoanerInfo()
        {
            Annexes = new List<RLoanAnnex>();
        }

        public int LoanId { get; set; }

        public string LoanerIntro { get; set; }

        public string RepaymentResource { get; set; }

        public List<RLoanAnnex> Annexes { get; set; }
    }
    public class RLoanAnnex
    {
        //影像资料id
        public int AnnexId { get; set; }
        //影像资料Name
        public string AnnexName { get; set; }
        //影像资料URl
        public string AnnexURl { get; set; }
        //上传时间
        public string AnnexTime { get; set; }
    }

    #endregion

    public class RLoanInvester
    {
        public string Invester { get; set; }

        public decimal InvestMoney { get; set; }

        public DateTime InvestTime { get; set; }

        public string InvestTimeString { get; set; }

        public string InvestState { get; set; }
    }

    public class RRepaymentPlan
    {
        public RRepaymentPlan()
        {
            RepaymentPlanInfos = new List<RRepaymentPlanInfo>();
        }

        public int LoanId { get; set; }
        /// <summary>
        /// 已还次数
        /// </summary>
        public int Repaymented { get; set; }
        /// <summary>
        /// 待还次数
        /// </summary>
        public int Repaymenting { get; set; }
        /// <summary>
        /// 已还金额
        /// </summary>
        public decimal RepaymentedMoney { get; set; }
        /// <summary>
        /// 待还金额
        /// </summary>
        public decimal RepaymentingMoney { get; set; }

        public List<RRepaymentPlanInfo> RepaymentPlanInfos { get; set; }

    }

    public class RRepaymentPlanInfo
    {
        public int Time { get; set; }

        public string RepaymentTime { get; set; }

        public decimal Principal { get; set; }

        public decimal Interest { get; set; }

        public decimal AllMoney { get; set; }

        public string RepaymenyState { get; set; }
    }


    #region 新详情页数据

    public class RLoanProjectDetails
    {
        public RLoanProjectDetails()
        {
            Info = new FinancingInformation();
            Guar = new RepaymentGuarantee();
            Personal = new LoanerInfomation();
            Coer = new CoInfomation();
            History = new LoanerHistory();
            RiskInfo = new LoanRiskResult();
            Check = new ReviewProject();
            Annexes = new List<RLoanAnnex>();
        }
        /// <summary>
        /// 标的id
        /// </summary>
        public string LoanId { get; set; }
        /// <summary>
        /// 是否为企业
        /// </summary>
        public string IsCo { get; set; }

        /// <summary>
        /// 融资信息
        /// </summary>
        public FinancingInformation Info { get; set; }
        /// <summary>
        /// 还款保障
        /// </summary>
        public RepaymentGuarantee Guar { get; set; }
        /// <summary>
        /// 个人借款人信息
        /// </summary>
        public LoanerInfomation Personal { get; set; }
        /// <summary>
        /// 企业借款人信息
        /// </summary>
        public CoInfomation Coer { get; set; }
        /// <summary>
        /// 借款人历史数据
        /// </summary>
        public LoanerHistory History { get; set; }
        /// <summary>
        /// 风险信息
        /// </summary>
        public LoanRiskResult RiskInfo { get; set; }
        /// <summary>
        /// 审查项目
        /// </summary>
        public ReviewProject Check { get; set; }

        /// <summary>
        /// 项目资料
        /// </summary>
        public List<RLoanAnnex> Annexes { get; set; }
    }
    /// <summary>
    /// 融资信息
    /// </summary>
    public class FinancingInformation
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string LoanName { get; set; }
        /// <summary>
        /// 项目简介
        /// </summary>
        public string LoanIntro { get; set; }
        /// <summary>
        /// 借款人主体性质
        /// </summary>
        public string LoanerCharacter { get; set; }
        /// <summary>
        /// 借款人姓名
        /// </summary>
        public string LoanerName { get; set; }
        /// <summary>
        /// 借款用途
        /// </summary>
        public string LoanUse { get; set; }

        /// <summary>
        /// 借款金额
        /// </summary>
        public string LoanMoney { get; set; }
        /// <summary>
        /// 借款期限
        /// </summary>
        public string LoanPeriod { get; set; }
        /// <summary>
        /// 项目类型
        /// </summary>
        public string LoanType { get; set; }
        /// <summary>
        ///还款方式
        /// </summary>
        public string LoanRepayment { get; set; }
        /// <summary>
        /// 还款来源
        /// </summary>
        public string LoanSource { get; set; }
        /// <summary>
        /// 安全保障
        /// </summary>
        public string SafetyPrecautions { get; set; }
    }

    /// <summary>
    /// 还款保障
    /// </summary>
    public class RepaymentGuarantee
    {
        /// <summary>
        /// 还款保障
        /// </summary>
        public string Guarantee { get; set; }
    }

    public class LoanerInfomation
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; } = "";
        /// <summary>
        /// 身份证
        /// </summary>
        public string IdCard { get; set; } = "";
        /// <summary>
        /// 年龄
        /// </summary>
        public string Age { get; set; } = "";
        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; } = "";
        /// <summary>
        /// 教育情况
        /// </summary>
        public string Education { get; set; } = "";
        /// <summary>
        /// 工作
        /// </summary>
        public string Industry { get; set; } = "";
        /// <summary>
        /// 工作年限
        /// </summary>
        public string WorkYear { get; set; } = "";
        /// <summary>
        /// 收入
        /// </summary>
        public string Income { get; set; } = "";
    }

    public class CoInfomation
    {
        /// <summary>
        /// 企业名称
        /// </summary>
        public string Name { get; set; } = "";
        /// <summary>
        /// 法人
        /// </summary>
        public string Corporate { get; set; } = "";
        /// <summary>
        /// 注册资本
        /// </summary>
        public string RegisteredCapital { get; set; } = "";
        /// <summary>
        /// 实缴资本
        /// </summary>
        public string PaidCapital { get; set; } = "";
        /// <summary>
        /// 所属行业
        /// </summary>
        public string Industry { get; set; } = "";
        /// <summary>
        /// 经营年限
        /// </summary>
        public string BusinessLife { get; set; } = "";
        /// <summary>
        /// 企业年收入
        /// </summary>
        public string AnnualIncome { get; set; } = "";
    }

    public class LoanerHistory
    {
        /// <summary>
        /// 负债
        /// </summary>
        public string Liabilities { get; set; }
        /// <summary>
        /// 历史借款笔数
        /// </summary>
        public string HistoryLoan { get; set; }
        /// <summary>
        /// 历史逾期
        /// </summary>
        public string HistoryOverDue { get; set; }
        public string UnclearLoan { get; set; }
        /// <summary>
        /// 未结清金额
        /// </summary>
        public string UnclearMoney { get; set; }
        /// <summary>
        /// 其他平台情况
        /// </summary>
        public string OtherPlatform { get; set; }
    }

    public class LoanRiskResult
    {
        public string RiskResult { get; set; }
    }

    public class ReviewProject
    {
        public ReviewProject()
        {
            Reviewed = new List<string>();
        }
        public List<string> Reviewed { get; set; }
    }



    public class RTrackingDetails
    {
        public string LoanId { get; set; }

        public string Count { get; set; }

        public List<RepaymentTracking> Trackings { get; set; } = new List<RepaymentTracking>();
    }

    public class RepaymentTracking
    {
        public string TrackingDate { get; set; }

        public List<TrackInfo> TrackInfos { get; set; } = new List<TrackInfo>();
    }


    public class TrackInfo
    {
        public string Order { get; set; }

        public string TrackTitle { get; set; }

        public string TrackDesc { get; set; }
    }

    #endregion
}
