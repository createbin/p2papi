using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ZFCTAPI.Core;
using ZFCTAPI.Core.Caching;
using ZFCTAPI.Core.Configuration;
using ZFCTAPI.Core.Enums;
using ZFCTAPI.Core.Helpers;
using ZFCTAPI.Data.ApiModels.ReturnModels;
using ZFCTAPI.Data.ApiModels.SubmitModels;
using ZFCTAPI.Data.BankCards;
using ZFCTAPI.Data.BoHai.ReturnModels;
using ZFCTAPI.Data.BoHai.SubmitModels;
using ZFCTAPI.Data.CST;
using ZFCTAPI.Services.BoHai;
using ZFCTAPI.Services.InvestInfo;
using ZFCTAPI.Services.LoanInfo;
using ZFCTAPI.Services.Popular;
using ZFCTAPI.Services.RiskAssessment;
using ZFCTAPI.Services.Sys;
using ZFCTAPI.Services.Transaction;
using ZFCTAPI.Services.UserInfo;
using ZFCTAPI.WebApi.RequestAttribute;
using ZFCTAPI.WebApi.Validates;
using ZFCTAPI.Services.Promotion;

namespace ZFCTAPI.WebApi.Controllers
{
    /// <summary>
    /// 个人中心 我的账户
    /// </summary>
    [Route("api/[controller]/[action]")]
    [RequestLog]
    public class MyAccountController : Controller
    {
        #region Field

        private readonly IInvestInfoService _investInfoService;
        private readonly IInvesterPlanService _investerPlanService;
        private readonly ILoanInfoService _loanInfoService;
        private readonly ILoanPlanService _loanPlanService;
        private readonly ITransInfoService _transInfoService;
        private readonly ICstRedInfoService _cstRedInfoService;
        private readonly IBHAccountService _iBHAccountService;
        private readonly IAccountInfoService _accountInfoService;
        private readonly ICstUserInfoService _userInfoService;
        private readonly IBHUserService _bhUserService;
        private readonly ZfctWebConfig _zfctWebConfig;
        private readonly FastDFSConfig _fastDFSConfig;
        private readonly ICstTransactionService _cstTransactionService;
        private readonly ISYSDataDictionaryService _iSYSDataDictionaryService;
        private readonly IUserInvestTypeService _userInvestTypeService;
        private readonly ICustomerService _customerService;
        private readonly ICacheManager _cacheManager;
        private readonly IPro_ContractService _pro_ContractService;
        private readonly ICompanyInfoService _companyInfoService;
        private readonly IInvitationActivitie _invitationActivitie;
        private readonly IFeeService _feeService;

        #endregion Field

        #region Ctor

        public MyAccountController(IInvestInfoService investInfoService,
            IInvesterPlanService investerPlanService,
            ILoanPlanService loanPlanService,
            ILoanInfoService loanInfoService,
            ITransInfoService transInfoService,
            ICstRedInfoService cstRedInfoService,
            IBHAccountService iBHAccountService,
            IAccountInfoService accountInfoService,
            ICstUserInfoService userInfoService,
            IBHUserService bhUserService,
            ZfctWebConfig zfctWebConfig,
            FastDFSConfig fastDFSConfig,
            ICstTransactionService cstTransactionService,
            ISYSDataDictionaryService iSYSDataDictionaryService,
            IUserInvestTypeService userInvestTypeService,
            ICacheManager cacheManager,
            ICustomerService customerService,
            IPro_ContractService pro_ContractService,
            ICompanyInfoService companyInfoService,
            IInvitationActivitie invitationActivitie,
            IFeeService feeService)
        {
            _investerPlanService = investerPlanService;
            _investInfoService = investInfoService;
            _loanPlanService = loanPlanService;
            _loanInfoService = loanInfoService;
            _transInfoService = transInfoService;
            _cstRedInfoService = cstRedInfoService;
            _iBHAccountService = iBHAccountService;
            _accountInfoService = accountInfoService;
            _userInfoService = userInfoService;
            _bhUserService = bhUserService;
            _zfctWebConfig = zfctWebConfig;
            _fastDFSConfig = fastDFSConfig;
            _cstTransactionService = cstTransactionService;
            _iSYSDataDictionaryService = iSYSDataDictionaryService;
            _userInvestTypeService = userInvestTypeService;
            _cacheManager = cacheManager;
            _customerService = customerService;
            _pro_ContractService = pro_ContractService;
            _companyInfoService = companyInfoService;
            _invitationActivitie = invitationActivitie;
            _feeService = feeService;
        }

        #endregion Ctor

        #region Func

        private DateTime IdCardToBirth(string identityCard)
        {
            var birthday = "";
            //处理18位的身份证号码从号码中得到生日和性别代码
            if (identityCard.Length == 18)
            {
                birthday = identityCard.Substring(6, 4) + "-" + identityCard.Substring(10, 2) + "-" +
                           identityCard.Substring(12, 2);
            }
            //处理15位的身份证号码从号码中得到生日和性别代码
            if (identityCard.Length == 15)
            {
                birthday = "19" + identityCard.Substring(6, 2) + "-" + identityCard.Substring(8, 2) + "-" +
                           identityCard.Substring(10, 2);
            }
            return Convert.ToDateTime(birthday);
        }

        /// <summary>
        /// 银行卡图片
        /// </summary>
        /// <param name="BankCode"></param>
        /// <returns></returns>
        private BankCard GetBankInfo(string BankCode)
        {
            var bankinfo = new BankCard();

            switch (BankCode)
            {
                case "BOC": bankinfo.BankCardName = "中国银行"; bankinfo.BankCardLogo = "/images/banklogo/banklogo1.png"; bankinfo.BankCardBack = "/images/bankBack/zhongguoyinhang.png"; break;
                case "ICBC": bankinfo.BankCardName = "中国工商银行"; bankinfo.BankCardLogo = "/images/banklogo/banklogo2.png"; bankinfo.BankCardBack = "/images/bankBack/gongshangyinhang.png"; break;
                case "ABC": bankinfo.BankCardName = "中国农业银行"; bankinfo.BankCardLogo = "/images/banklogo/banklogo3.png"; bankinfo.BankCardBack = "/images/bankBack/nongyeyinhang.png"; break;
                case "BOCOM": bankinfo.BankCardName = "交通银行"; bankinfo.BankCardLogo = "/images/banklogo/banklogo4.png"; bankinfo.BankCardBack = "/images/bankBack/jiaotongyinhang.png"; break;
                case "GDB": bankinfo.BankCardName = "广发银行"; bankinfo.BankCardLogo = "/images/banklogo/banklogo5.png"; bankinfo.BankCardBack = "/images/bankBack/guangfayinhang.png"; break;
                case "SDB": bankinfo.BankCardName = "深发银行"; bankinfo.BankCardLogo = "/images/banklogo/banklogo6.png"; bankinfo.BankCardBack = "/images/bankBack/zhongguoyinhang.png"; break;
                case "CCB": bankinfo.BankCardName = "中国建设银行"; bankinfo.BankCardLogo = "/images/banklogo/banklogo7.png"; bankinfo.BankCardBack = "/images/bankBack/jiansheyinhang.png"; break;
                case "SPDB": bankinfo.BankCardName = "浦发银行"; bankinfo.BankCardLogo = "/images/banklogo/banklogo8.png"; bankinfo.BankCardBack = "/images/bankBack/zhongguoyinhang.png"; break;
                case "9": bankinfo.BankCardName = "浙江泰隆商业银行"; bankinfo.BankCardLogo = "/images/banklogo/banklogo9.png"; bankinfo.BankCardBack = "/images/bankBack/zhongguoyinhang.png"; break;
                case "CMB": bankinfo.BankCardName = "招商银行"; bankinfo.BankCardLogo = "/images/banklogo/banklogo10.png"; bankinfo.BankCardBack = "/images/bankBack/zhaoshangyinhang.png"; break;
                case "PSBC": bankinfo.BankCardName = "中国邮政储蓄银行"; bankinfo.BankCardLogo = "/images/banklogo/banklogo11.png"; bankinfo.BankCardBack = "/images/bankBack/mingshengyinhang.png"; break;
                case "CMBC": bankinfo.BankCardName = "中国民生银行"; bankinfo.BankCardLogo = "/images/banklogo/banklogo12.png"; bankinfo.BankCardBack = "/images/bankBack/mingshengyinhang.png"; break;
                case "CIB": bankinfo.BankCardName = "兴业银行"; bankinfo.BankCardLogo = "/images/banklogo/banklogo13.png"; bankinfo.BankCardBack = "/images/bankBack/xingyeyinhang.png"; break;
                case "15": bankinfo.BankCardName = "东莞银行"; bankinfo.BankCardLogo = "/images/banklogo/banklogo14.png"; bankinfo.BankCardBack = "/images/bankBack/xingyeyinhang.png"; break;
                case "CITIC": bankinfo.BankCardName = "中信银行"; bankinfo.BankCardLogo = "/images/banklogo/banklogo15.png"; bankinfo.BankCardBack = "/images/bankBack/pinganyinhang.png"; break;
                case "HXB": bankinfo.BankCardName = "华夏银行"; bankinfo.BankCardLogo = "/images/banklogo/banklogo16.png"; bankinfo.BankCardBack = "/images/bankBack/pinganyinhang.png"; break;
                case "CEB": bankinfo.BankCardName = "中国光大银行"; bankinfo.BankCardLogo = "/images/banklogo/banklogo17.png"; bankinfo.BankCardBack = "/images/bankBack/guangdayinhang.png"; break;
                case "BCCB": bankinfo.BankCardName = "北京银行"; bankinfo.BankCardLogo = "/images/banklogo/banklogo18.png"; bankinfo.BankCardBack = "/images/bankBack/bohaiyinhang.png"; break;
                case "BOS": bankinfo.BankCardName = "上海银行"; bankinfo.BankCardLogo = "/images/banklogo/banklogo19.png"; bankinfo.BankCardBack = "/images/bankBack/shanghaipudongfazhanyinhang.png"; break;
                case "22": bankinfo.BankCardName = "天津银行"; bankinfo.BankCardLogo = "/images/banklogo/banklogo20.png"; bankinfo.BankCardBack = "/images/bankBack/bohaiyinhang.png"; break;
                case "23": bankinfo.BankCardName = "大连银行"; bankinfo.BankCardLogo = "/images/banklogo/banklogo21.png"; bankinfo.BankCardBack = "/images/bankBack/bohaiyinhang.png"; break;
                case "HZCB": bankinfo.BankCardName = "杭州银行"; bankinfo.BankCardLogo = "/images/banklogo/banklogo22.png"; bankinfo.BankCardBack = "/images/bankBack/bohaiyinhang.png"; break;
                case "25": bankinfo.BankCardName = "宁波银行"; bankinfo.BankCardLogo = "/images/banklogo/banklogo23.png"; bankinfo.BankCardBack = "/images/bankBack/bohaiyinhang.png"; break;
                case "26": bankinfo.BankCardName = "厦门银行"; bankinfo.BankCardLogo = "/images/banklogo/banklogo24.png"; bankinfo.BankCardBack = "/images/bankBack/bohaiyinhang.png"; break;
                case "27": bankinfo.BankCardName = "广州银行"; bankinfo.BankCardLogo = "/images/banklogo/banklogo25.png"; bankinfo.BankCardBack = "/images/bankBack/bohaiyinhang.png"; break;
                case "PINGAN": bankinfo.BankCardName = "平安银行"; bankinfo.BankCardLogo = "/images/banklogo/banklogo26.png"; bankinfo.BankCardBack = "/images/bankBack/pinganyinhang.png"; break;
                case "CZB": bankinfo.BankCardName = "浙商银行"; bankinfo.BankCardLogo = "/images/banklogo/banklogo27.png"; bankinfo.BankCardBack = "/images/bankBack/pinganyinhang.png"; break;
                case "SRCB": bankinfo.BankCardName = "上海农村商业银行"; bankinfo.BankCardLogo = "/images/banklogo/banklogo28.png"; bankinfo.BankCardBack = "/images/bankBack/pinganyinhang.png"; break;
                case "31": bankinfo.BankCardName = "重庆银行"; bankinfo.BankCardLogo = "/images/banklogo/banklogo29.png"; bankinfo.BankCardBack = "/images/bankBack/pinganyinhang.png"; break;
                case "32": bankinfo.BankCardName = "江苏银行"; bankinfo.BankCardLogo = "/images/banklogo/banklogo30.png"; bankinfo.BankCardBack = "/images/bankBack/pinganyinhang.png"; break;
                case "CBHB": bankinfo.BankCardName = "渤海银行"; bankinfo.BankCardLogo = "/images/banklogo/banklogo31.png"; bankinfo.BankCardBack = "/images/bankBack/bohaiyinhang.png"; break;
                    default: bankinfo.BankCardName = "未识别银行"; bankinfo.BankCardLogo = "/images/banklogo/banklogo32.png"; bankinfo.BankCardBack = "/images/bankBack/bank_bg.png"; break;
            }

            return bankinfo;
        }

        #endregion Func

        #region Action

        /// <summary>
        /// PC投资账户统计
        /// </summary>
        /// <param name="model">请求模型</param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RPCAccountStatistics, string> PCAccountStatistics([FromBody]BaseSubmitModel model)
        {
            var retModel = new ReturnModel<RPCAccountStatistics, string>();

            #region 校验签名

            var signResult = VerifyBase.SignAndToken<RPCAccountStatistics>(model, out CST_user_info userInfo);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名

            #region 统计投资还款计划

            var investPlanStatisticsItems = new List<InvestPlanStatisticsType>
            {
                InvestPlanStatisticsType.CumulativeIncome,
                InvestPlanStatisticsType.TodayWaitReceive,
                InvestPlanStatisticsType.WaitReceivePrincipal,
                InvestPlanStatisticsType.WaitReceiveTotal,
                InvestPlanStatisticsType.WaitReceiveIncome
            };
            var investPlanStatistics = _investerPlanService.InvestPlanStatistics(userInfo.Id, investPlanStatisticsItems);

            //投资待收罚息
            var investWaitReceiveOverRate = _investerPlanService.CalculationOverRate(userInfo.Id);

            #endregion 统计投资还款计划

            #region 统计投资

            var investStatisticsItems = new List<InvestStatisticsType>
            {
                InvestStatisticsType.InvestCount,
                InvestStatisticsType.InvestMoney,
                InvestStatisticsType.BiddingCount,
                InvestStatisticsType.RepaymentCount,
                InvestStatisticsType.ClearedCount,
                InvestStatisticsType.TransInvestCount
            };
            var investStatistics = _investInfoService.InvestStatistics(userInfo.Id, investStatisticsItems);

            #endregion 统计投资

            //#region 统计借款还款计划

            //var loanPlanStatisticsItems = new List<LoanPlanStatisticsType>
            //{
            //    LoanPlanStatisticsType.TodayWaitRepay,
            //    LoanPlanStatisticsType.WaitRepayPrincipal,
            //    LoanPlanStatisticsType.WaitRepayRate,
            //    LoanPlanStatisticsType.WaitRepayServiceFee
            //};
            //var loanPlanStatistics = _loanPlanService.UserLoanPlanStatistics(userInfo.Id, loanPlanStatisticsItems);

            ////借款罚息
            //var loanWaitReceiveOverRate = _loanPlanService.CalculationOverRate(userInfo.Id);

            //#endregion 统计借款还款计划

            //#region 统计借款

            //var loanStatisticsItems = new List<LoanStatisticsType>
            //{
            //    LoanStatisticsType.LoanCount,
            //    LoanStatisticsType.LoanMoney,
            //    LoanStatisticsType.BiddingCount,
            //    LoanStatisticsType.FullCount,
            //    LoanStatisticsType.RepaymentCount,
            //    LoanStatisticsType.ClearedCount
            //};
            //var loanStatistics = _loanInfoService.LoanStatistics(userInfo.Id, loanStatisticsItems);

            //#endregion 统计借款


            #region 统计债权转让

            var canTransCount = _transInfoService.CanTransCount(userInfo.Id);
            var transStatistics = _transInfoService.TransferStatistics(userInfo.Id);

            #endregion 统计债权转让

            #region 红包统计

            var RedStatisticsItems = new List<RedStatisticsType>
            {
                RedStatisticsType.WaitUseCount,
                RedStatisticsType.WaitUseMoney
            };
            var redStatistics = _cstRedInfoService.UserRedStatistics(userInfo.cst_customer_id.Value, RedStatisticsItems);

            #endregion 红包统计

            var returnModel = new RPCAccountStatistics()
            {
                AccountMoney = investPlanStatistics.WaitReceiveTotal,
                CumulativeIncome = investPlanStatistics.CumulativeIncome,
                TodayWaitReceive = investPlanStatistics.TodayWaitReceive + investWaitReceiveOverRate,
                WaitReceivePrincipal = investPlanStatistics.WaitReceivePrincipal,

                InvestCount = investStatistics.InvestCount,
                InvestMoney = investStatistics.InvestMoney,
                InvestBiddingCount = investStatistics.BiddingCount,
                InvesRepayCount = investStatistics.RepaymentCount,
                InvesSettledtCount = investStatistics.ClearedCount,

                //TodayWaitRepay = loanPlanStatistics.TodayWaitRepay + loanWaitReceiveOverRate,
                //WaitRepayPrincipal = loanPlanStatistics.WaitRepayPrincipal,
                //WaitRepayRate = loanPlanStatistics.WaitRepayRate,
                //WaitRepayOverRate = loanWaitReceiveOverRate,
                //WaitRepayServiceFee = loanPlanStatistics.WaitRepayServiceFee,

                //LoanCount = loanStatistics.LoanCount,
                //LoanMoney = loanStatistics.LoanMoney,
                //LoanTenderCount = loanStatistics.BiddingCount,
                //LoanFullCount = loanStatistics.FullCount,
                //LoanRepayCount = loanStatistics.RepaymentCount,
                //LoanSettledCount = loanStatistics.ClearedCount,

                TransferCanCount = canTransCount,
                TransferWaitCount = transStatistics.TransferingCount,
                TransferOutCount = transStatistics.TransferedCount,
                TransferInCount = investStatistics.TransInvestCount,

                RedMoney = redStatistics.WaitUseMoney,
                HasLoan = _loanInfoService.HasLoan(userInfo.Id) > 0
            };

            var accountInfo = _accountInfoService.GetAccountInfoByUserId(userId: userInfo.Id);
            if (accountInfo != null && accountInfo.JieSuan && accountInfo.BoHai)
            {
                returnModel.IsOpenAccount = true;
            }
            returnModel.IsOldAccount = userInfo.cst_add_date.Value < new DateTime(2018, 7, 1);


            retModel.Message = "查询成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.ReturnData = returnModel;
            retModel.Token = model.Token;
            return retModel;
        }

        /// <summary>
        /// PC融资账户统计
        /// </summary>
        /// <param name="model">请求模型</param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RFinancingAccount, string> FinancingAccount([FromBody]BaseSubmitModel model)
        {
            var retModel = new ReturnModel<RFinancingAccount, string>();

            #region 校验签名

            var signResult = VerifyBase.SignAndToken<RFinancingAccount>(model, out CST_user_info userInfo);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名
            #region 统计借款还款计划

            var loanPlanStatisticsItems = new List<LoanPlanStatisticsType>
            {
                LoanPlanStatisticsType.WaitRepayTotal,
            };
            var loanPlanStatistics = _loanPlanService.UserLoanPlanStatistics(userInfo.Id, loanPlanStatisticsItems);

            //借款罚息
            var loanWaitReceiveOverRate = _loanPlanService.CalculationOverRate(userInfo.Id);

            #endregion 统计借款还款计划

            #region 统计借款

            var loanStatisticsItems = new List<LoanStatisticsType>
            {
                LoanStatisticsType.LoanCount,
                LoanStatisticsType.LoanMoney,
                LoanStatisticsType.BiddingCount,
                LoanStatisticsType.FullCount,
                LoanStatisticsType.RepaymentCount,
                LoanStatisticsType.ClearedCount
            };
            var loanStatistics = _loanInfoService.LoanStatistics(userInfo.Id, loanStatisticsItems);

            var latelyLoanPlan = _loanPlanService.GetLatelyLoanPlansByUserId(userInfo.Id);


            #endregion 统计借款

            var returnModel = new RFinancingAccount()
            {
                WaitPayAllMoney = loanPlanStatistics.WaitRepayTotal + loanWaitReceiveOverRate,
                BiddingCount = loanStatistics.BiddingCount,
                ClearedCount = loanStatistics.ClearedCount,
                FullCount = loanStatistics.FullCount,
                RepaymentCount = loanStatistics.RepaymentCount,
                LoanCount = loanStatistics.LoanCount,
                LoanMoney = loanStatistics.LoanMoney
            };

            if (latelyLoanPlan != null)
            {
                if (latelyLoanPlan.pro_pay_date != null) {
                    returnModel.NextPayDate = latelyLoanPlan.pro_pay_date.Value.ToString("yyyy-MM-dd");
                    returnModel.SurplusDays = (latelyLoanPlan.pro_pay_date.Value- DateTime.Now).Days;
                }
                returnModel.NextPayMoney = latelyLoanPlan.pro_pay_total.Value;
               
            }
            retModel.Message = "查询成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.ReturnData = returnModel;
            retModel.Token = model.Token;
            return retModel;
        }

        /// <summary>
        /// APP个人中心账户统计
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RAPPAccountStatistics, string> APPAccountStatistics([FromBody]BaseSubmitModel model)
        {
            var retModel = new ReturnModel<RAPPAccountStatistics, string>();

            #region 校验签名

            var signResult = VerifyBase.SignAndToken<BaseSubmitModel>(model, out CST_user_info userInfo);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名

            #region 统计投资还款计划

            var investPlanStatisticsItems = new List<InvestPlanStatisticsType>
            {
                InvestPlanStatisticsType.CumulativeIncome,
                InvestPlanStatisticsType.WaitReceiveIncome,
                InvestPlanStatisticsType.NextRePayDay,
                InvestPlanStatisticsType.WaitReceiveTotal
            };
            var investPlanStatistics = _investerPlanService.InvestPlanStatistics(userInfo.Id, investPlanStatisticsItems);

            //待收罚息
            var waitReceiveOverRate = _investerPlanService.CalculationOverRate(userInfo.Id);

            #endregion 统计投资还款计划

            #region 红包统计

            var RedStatisticsItems = new List<RedStatisticsType>
            {
                RedStatisticsType.WaitUseCount
            };
            var redStatistics = _cstRedInfoService.UserRedStatistics(userInfo.cst_customer_id.Value, RedStatisticsItems);

            #endregion 红包统计

            var rewards = _invitationActivitie.GetListsByBeneficiaryId(userInfo.cst_customer_id.Value);

            var returnModel = new RAPPAccountStatistics()
            {
                AccountMoney = investPlanStatistics.WaitReceiveTotal,
                CumulativeIncome = investPlanStatistics.CumulativeIncome,
                WaitReceiveIncome = investPlanStatistics.WaitReceiveIncome,
                WaitReceiveOverRate = waitReceiveOverRate,
                RedCount = redStatistics.WaitUseCount,
                NextRePayDay = investPlanStatistics.NextRePayDay == null ? "暂无" : investPlanStatistics.NextRePayDay.Value.ToString("yyyy-MM-dd"),
                Reward = rewards.Sum(p => p.BeneficiaryAmount),
                HasLoan = _loanInfoService.HasLoan(userInfo.Id) > 0
            };

            var accountInfo = _accountInfoService.GetAccountInfoByUserId(userId: userInfo.Id);
            if (accountInfo != null && accountInfo.JieSuan && accountInfo.BoHai)
            {
                returnModel.IsOpenAccount = true;
            }
            returnModel.IsOldAccount = userInfo.cst_add_date.Value < new DateTime(2018, 7, 1);


            retModel.Message = "查询成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.ReturnData = returnModel;
            retModel.Token = model.Token;
            return retModel;
        }

        /// <summary>
        /// 业务统计
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RBusinessStatistics, string> BusinessStatistics([FromBody]BaseSubmitModel model)
        {
            var retModel = new ReturnModel<RBusinessStatistics, string>();

            #region 校验签名

            var signResult = VerifyBase.SignAndToken<BaseSubmitModel>(model, out CST_user_info userInfo);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            #region 统计投资

            var investStatisticsItems = new List<InvestStatisticsType>
            {
                InvestStatisticsType.InvestCount,
                InvestStatisticsType.TransInvestCount
            };
            var investStatistics = _investInfoService.InvestStatistics(userInfo.Id, investStatisticsItems);

            #endregion 统计投资

            #region 红包统计

            var RedStatisticsItems = new List<RedStatisticsType>
            {
                RedStatisticsType.WaitUseCount
            };
            var redStatistics = _cstRedInfoService.UserRedStatistics(userInfo.cst_customer_id.Value, RedStatisticsItems);

            #endregion 红包统计

            #region 统计债权转让

            var canTransCount = _transInfoService.CanTransCount(userInfo.Id);
            var transStatistics = _transInfoService.TransferStatistics(userInfo.Id);

            #endregion 统计债权转让

            #region 统计借款
            var loanStatisticsItems = new List<LoanStatisticsType>
            {
                LoanStatisticsType.LoanCount
            };
            var loanStatistics = _loanInfoService.LoanStatistics(userInfo.Id, loanStatisticsItems);
            #endregion


            var returnModel = new RBusinessStatistics()
            {
                InvestCount = investStatistics.InvestCount,
                LoanCount = loanStatistics.LoanCount,
                TransferCount = canTransCount + transStatistics.TransferedCount + transStatistics.TransferingCount + investStatistics.TransInvestCount,
                CanUserRedCount = redStatistics.WaitUseCount
            };

            retModel.Message = "查询成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.ReturnData = returnModel;
            retModel.Token = model.Token;
            return retModel;

            #endregion 校验签名
        }

        /// <summary>
        /// 渤海账户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RBHAccountInfo, string> RBHAccountInfo([FromBody]SJsQueryChargeAccount model)
        {
            var retModel = new ReturnModel<RBHAccountInfo, string>();

            #region 校验签名

            var signResult = VerifyBase.SignAndToken<BaseSubmitModel>(model, out int userId);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名

            var accountInfo = _accountInfoService.GetAccountInfoByUserId(userId: userId);

            if (accountInfo != null && accountInfo.BoHai && accountInfo.JieSuan)
            {
                //调用渤海接口
                var rBHAccountQueryAccBalance = _iBHAccountService.AccountQueryAccBalance(new SBHAccountQueryAccBalance
                {
                    SvcBody = new SBHAccountQueryAccBalanceBody
                    {
                        platformUid = model.UserAttribute == 1 ? accountInfo.invest_platform_id : accountInfo.financing_platform_id
                    }
                }) ?? new RBHAccountQueryAccBalance();
                var returnModel = new RBHAccountInfo();
                if (rBHAccountQueryAccBalance.RspSvcHeader.returnCode.Equals(JSReturnCode.Success))
                {
                    returnModel.TotalAmount = decimal.Parse(rBHAccountQueryAccBalance.SvcBody.totalAmount);
                    returnModel.FreezeAmout = decimal.Parse(rBHAccountQueryAccBalance.SvcBody.freezeAmout);
                    returnModel.WithdrawAmount = decimal.Parse(rBHAccountQueryAccBalance.SvcBody.withdrawAmount ?? "0.00");
                    retModel.Message = "成功";
                    retModel.ReturnCode = (int)ReturnCode.Success;
                    retModel.ReturnData = returnModel;
                }
                else
                {
                    retModel.Message = rBHAccountQueryAccBalance.RspSvcHeader.returnMsg;
                    retModel.ReturnCode = (int)ReturnCode.DataEorr;
                    retModel.Message = "成功";
                    retModel.ReturnData = returnModel;
                }
                retModel.Token = model.Token;
                return retModel;
            }

            retModel.Message = "用户尚未完成开户或绑卡";
            retModel.ReturnCode = (int)ReturnCode.UnOpenAccount;
            retModel.Token = model.Token;
            return retModel;
        }

        /// <summary>
        /// 用户账户充值
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RToPage, string> BHAccountRecharge([FromBody]SBHAccountRecharge model)
        {
            var retModel = new ReturnModel<RToPage, string>();

            #region 校验签名

            var signResult = VerifyBase.SignAndToken<SBHAccountRecharge>(model, out int userId);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名

            var accountInfo = _accountInfoService.GetAccountInfoByUserId(userId: userId);

            if (accountInfo != null && accountInfo.BoHai && accountInfo.JieSuan)
            {
                //插入交易记录表
                var orderNo = CommonHelper.GetMchntTxnSsn();
                _cstTransactionService.Add(new CST_transaction_info
                {
                    pro_transaction_money = Convert.ToDecimal(model.Money),
                    pro_transaction_no = orderNo,
                    pro_transaction_time = DateTime.Now,
                    pro_transaction_type = DataDictionary.transactiontype_Recharge,
                    pro_user_id = accountInfo.act_user_id,
                    pro_user_type = accountInfo.act_user_type,
                    pro_transaction_status = DataDictionary.transactionstatus_processing,
                    pro_transaction_remarks = "充值",
                    pro_transaction_resource = model.RequestSource
                });

                var returnModel = new RToPage
                {
                    Url = $"{_zfctWebConfig.LocalUrl}Page/WebRecharge?token={System.Net.WebUtility.UrlEncode(model.Token)}&money={model.Money}&orderno={orderNo}&userattribute={model.UserAttribute}"
                };
                retModel.Message = "成功";
                retModel.ReturnCode = (int)ReturnCode.Success;
                retModel.ReturnData = returnModel;
                retModel.Token = model.Token;
                return retModel;
            }



            retModel.Message = "用户尚未完成开户或绑卡";
            retModel.ReturnCode = (int)ReturnCode.UnOpenAccount;
            retModel.Token = model.Token;
            return retModel;
        }

        /// <summary>
        /// 用户账户提现
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RToPage, string> BHAccountWithdraw([FromBody]SBHAccountWithdraw model)
        {
            var retModel = new ReturnModel<RToPage, string>();

            #region 校验签名

            var signResult = VerifyBase.SignAndToken<SBHAccountWithdraw>(model, out int userId);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名

            var accountInfo = _accountInfoService.GetAccountInfoByUserId(userId: userId);

            if (accountInfo != null && accountInfo.BoHai && accountInfo.JieSuan)
            {
                //插入交易记录表
                var fee = _feeService.CalcWithdrawFee(accountInfo.act_user_id.Value, model.Money,out bool isFirst);
                if (isFirst) fee = 0.00m;
                var orderNo = CommonHelper.GetMchntTxnSsn();
                _cstTransactionService.Add(new CST_transaction_info
                {
                    pro_transaction_money = model.Money,
                    pro_transaction_no = orderNo,
                    pro_transaction_time = DateTime.Now,
                    pro_transaction_type = DataDictionary.transactiontype_Withdrawals,
                    pro_user_id = accountInfo.act_user_id,
                    pro_user_type = accountInfo.act_user_type,
                    pro_transaction_status = DataDictionary.transactionstatus_processing,
                    pro_transaction_remarks = "提现",
                    pro_transaction_resource = model.RequestSource,
                    pro_platforms_fee = fee
                });

                var returnModel = new RToPage
                {
                    Url = $"{_zfctWebConfig.LocalUrl}Page/Drawings?token={System.Net.WebUtility.UrlEncode(model.Token)}&money={model.Money}&orderno={orderNo}&userattribute={model.UserAttribute}"
                };

                retModel.Message = "成功";
                retModel.ReturnCode = (int)ReturnCode.Success;
                retModel.ReturnData = returnModel;
                retModel.Token = model.Token;
                return retModel;
            }

            retModel.Message = "用户尚未完成开户或绑卡";
            retModel.ReturnCode = (int)ReturnCode.UnOpenAccount;
            retModel.Token = model.Token;
            return retModel;
        }

        /// <summary>
        /// 计算用户提现手续费
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<decimal, string> CalcWithdrawFee([FromBody]SBHAccountWithdraw model)
        {
            var retModel = new ReturnModel<decimal, string>();

            #region 校验签名

            var signResult = VerifyBase.SignAndToken<SBHAccountWithdraw>(model, out int userId);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = 0.00m;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名

            var accountInfo = _accountInfoService.GetAccountInfoByUserId(userId: userId);

            if (accountInfo != null && accountInfo.BoHai && accountInfo.JieSuan)
            {
                var fee = _feeService.CalcWithdrawFee(accountInfo.act_user_id.Value, model.Money, out bool isFrist);
                //插入交易记录表
                retModel.Message = isFrist.ToString();
                retModel.ReturnCode = (int)ReturnCode.Success;
                retModel.ReturnData = fee;
                retModel.Token = model.Token;
                return retModel;
            }

            retModel.Message = "用户尚未完成开户或绑卡";
            retModel.ReturnCode = (int)ReturnCode.UnOpenAccount;
            retModel.Token = model.Token;
            retModel.ReturnData = 0.00m;
            return retModel;
        }

        /// <summary>
        /// 结算中心开户渤海
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RRealInfo, string> UserRealInfo([FromBody]BaseSubmitModel model)
        {
            var retModel = new ReturnModel<RRealInfo, string>();

            #region 校验签名

            var signResult = VerifyBase.SignAndToken<BaseSubmitModel>(model, out CST_user_info userInfo);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名


            var realInfo = new RRealInfo();
            if (userInfo.cst_account_id != null)
            {
                //获取用户账户信息
                var accountInfo = _accountInfoService.Find(userInfo.cst_account_id.Value);
                //如果用户在结算中心没有注册只返回身份信息
                if (!accountInfo.JieSuan)
                {
                    var proveInfo = _accountInfoService.GetRealNameInfo(userInfo.cst_account_id.Value);
                    realInfo.Sex = proveInfo.cst_user_sex ? "1" : "2";
                    realInfo.IdCard = proveInfo.cst_card_num;
                    realInfo.Bohai = "0";
                    realInfo.Jiesuan = "0";
                }
                else if (accountInfo.JieSuan && !accountInfo.BoHai)//用户注册结算中心但是没有绑卡
                {
                    //返回完整信息
                    var proveInfo = _accountInfoService.GetRealNameInfo(userInfo.cst_account_id.Value);
                    realInfo.Sex = proveInfo.cst_user_sex ? "1" : "2";
                    realInfo.IdCard = proveInfo.cst_card_num;
                    realInfo.Bohai = "1";
                    realInfo.Jiesuan = "0";
                    var bankInfo = _accountInfoService.GetBankInfo(userInfo.cst_account_id.Value);
                    //如果用户没有绑卡不做处理
                    if (bankInfo != null)
                    {
                        realInfo.BankCode = bankInfo.bank_code;
                        realInfo.BankCardNo = bankInfo.bank_no;
                    }
                }
                else if (accountInfo.JieSuan && accountInfo.BoHai)////用户注册结算中心用户已经绑卡直接返回所有信息
                {
                    realInfo.Bohai = "1";
                    realInfo.Jiesuan = "1";
                    var bankInfo = _accountInfoService.GetBankInfo(userInfo.cst_account_id.Value);
                    //返回加密后的银行卡号以及开户行代码
                    realInfo.BankCode = bankInfo.bank_code;
                    realInfo.BankCardNo = bankInfo.EncBankNo;
                }
            }
            retModel.Message = "查询成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.ReturnData = realInfo;
            retModel.Token = model.Token;
            return retModel;
        }

        /// <summary>
        /// 结算中心注册
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<string, string> JieSuanRegister([FromBody] SJsRegisterModel model)
        {
            var retModel = new ReturnModel<string, string>();

            #region 校验签名

            var signResult = VerifyBase.SignAndToken<SJsRegisterModel>(model, out CST_user_info userInfo);
            if (signResult == ReturnCode.SignatureFailure || userInfo.cst_user_phone == null)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名
            #region 校驗身份證

            var existAccount = _accountInfoService.GetAccountInfoByUserId(idCard: model.IdCard);
            if (existAccount != null)
            {
                retModel.Message = "身份证号已存在";
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            //校验真实姓名
            if (string.IsNullOrEmpty(model.RealName))
            {
                retModel.Message = "真实姓名不存在";
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            //校验身份证
            if (string.IsNullOrEmpty(model.IdCard))
            {
                retModel.Message = "身份证不存在";
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            var sexInfo = false;
            var birthInfo = DateTime.Now;
            try
            {
                sexInfo = !CommonHelper.GetSexByIdCard(model.IdCard);
                birthInfo= IdCardToBirth(model.IdCard);
            }
            catch
            {
                retModel.Message = "身份证不合法";
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            #endregion
            if (userInfo.cst_account_id == null)
            {
                //新建一个用户账户
                var account = new CST_account_info
                {
                    act_user_id = userInfo.Id,
                    act_user_type = 9,
                    act_legal_name = model.RealName,
                    act_user_name = model.RealName,
                    act_user_card = model.IdCard,
                    act_user_phone = userInfo.cst_user_phone,
                    act_business_property =Convert.ToInt32(model.BusinessProperty),
                    JieSuan = false,
                    BoHai = false,
                    //投资户结算平台id
                    invest_platform_id = CommonHelper.CreatePlatFormId(userInfo.Id, UserAttributes.Invester),
                    //融资户结算平台id
                    financing_platform_id = CommonHelper.CreatePlatFormId(userInfo.Id, UserAttributes.Financer)
                };
                #region account_info表主机字段不是自增处理办法
                //int accountid = _accountInfoService.AddAccountInfo(account);
                //if (accountid == -1)
                //{
                //    retModel.Message = "实名信息存储错误，请联系客服咨询。";
                //    retModel.ReturnCode = (int)ReturnCode.DataEorr;
                //    retModel.ReturnData = null;
                //    retModel.Token = model.Token;
                //    return retModel;
                //}
                //userInfo.cst_account_id = accountid;
                #endregion
                _accountInfoService.Add(account);
                var accountInfo = _accountInfoService.GetAccountInfoByUserId(userInfo.Id);
                userInfo.cst_account_id = accountInfo.Id;
                
                _userInfoService.Update(userInfo);
                //绑定实名认证表信息
                var realNameProve = new CST_realname_prove
                {
                    Id = userInfo.Id,
                    cst_user_id = userInfo.Id,
                    cst_user_realname = model.RealName,
                    cst_user_sex = sexInfo,
                    cst_user_birthdate = birthInfo,
                    cst_card_type = 44,
                    cst_card_num = model.IdCard,
                    cst_realname_status = 143
                };
                //处理18位的身份证号码从号码中得到生日
                _accountInfoService.AddRealNameProve(realNameProve);
            }
            else
            {
                //获取用户账户信息
                var accountInfo = _accountInfoService.Find(userInfo.cst_account_id.Value);
                if (accountInfo.JieSuan)
                {
                    retModel.Message = "用户已经完成开户";
                    retModel.ReturnCode = (int)ReturnCode.DataEorr;
                    retModel.ReturnData = null;
                    retModel.Token = model.Token;
                    return retModel;
                }
                //修改用户账户的绑定信息
                accountInfo.act_legal_name = model.RealName;
                accountInfo.act_user_name = model.RealName;
                accountInfo.act_user_card = model.IdCard;
                accountInfo.act_user_phone = userInfo.cst_user_phone;
                accountInfo.act_business_property = Convert.ToInt32(model.BusinessProperty);
                _accountInfoService.Update(accountInfo);

                var realNameProve = _accountInfoService.GetRealNameInfo(userInfo.Id);
                realNameProve.cst_user_realname = model.RealName;
                realNameProve.cst_user_sex = !CommonHelper.GetSexByIdCard(model.IdCard);
                realNameProve.cst_user_birthdate = IdCardToBirth(model.IdCard);
                realNameProve.cst_card_num = model.IdCard;
                //处理18位的身份证号码从号码中得到生日
                _accountInfoService.UpdateRealNameProve(realNameProve);
            }

            //构造请求注册中心数据
            var postData = new SBHUserAdd
            {
                SvcBody =
                {
                    idcard = model.IdCard,
                    identityType = IdentityType.IdCard,
                    businessType = UserType.PersonalUser,
                    //platformUidInvestment=CommonHelper.CreatePlatFormId(userInfo.Id, UserAttributes.Invester),
                    //platformUidFinance = CommonHelper.CreatePlatFormId(userInfo.Id, UserAttributes.Financer),
                    truename = model.RealName,
                    phonenum = userInfo.cst_user_phone,
                    sex = CommonHelper.GetSexByIdCard(model.IdCard)?"2":"1"
                }
            };

            if (model.BusinessProperty != "1")
            {
                model.BusinessProperty = "2";
                postData.SvcBody.platformUid = CommonHelper.CreatePlatFormId(userInfo.Id, UserAttributes.Financer);
                postData.SvcBody.businessProperty = "2";
            }
            else
            {
                postData.SvcBody.platformUid = CommonHelper.CreatePlatFormId(userInfo.Id, UserAttributes.Invester);
                postData.SvcBody.businessProperty = "1";
            }


            var result = _bhUserService.UserAdd(postData, UserType.PersonalUser);
            retModel.Message = "成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.ReturnData = result;
            retModel.Token = model.Token;
            return retModel;
        }

        /// <summary>
        /// 用户销户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RToPage, string> CloseAccount([FromBody] ColseAccount model)
        {
            var retModel = new ReturnModel<RToPage, string>();
            #region 校验签名

            var signResult = VerifyBase.SignAndToken<SBHAccountWithdraw>(model, out int userId);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            #endregion 校验签名
            var accountInfo = _accountInfoService.GetAccountInfoByUserId(userId: userId);
            if (accountInfo != null && accountInfo.BoHai && accountInfo.JieSuan)
            {
                var orderNo = CommonHelper.GetMchntTxnSsn();
                var returnModel = new RToPage
                {
                    Url = $"{_zfctWebConfig.LocalUrl}Page/CloseAccount?token={System.Net.WebUtility.UrlEncode(model.Token)}&platformUid={model.PlatformUid}&orderno={orderNo}&userattribute=1"
                };
                retModel.Message = "成功";
                retModel.ReturnCode = (int)ReturnCode.Success;
                retModel.ReturnData = returnModel;
                retModel.Token = model.Token;
                return retModel;
            }
            retModel.Message = "销户失败";
            retModel.ReturnCode = (int)ReturnCode.UnOpenAccount;
            retModel.Token = model.Token;
            return retModel;
        }

        /// <summary>
        /// 渤海开户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RToPage, string> BoHaiRealName([FromBody] SBhRealNameModel model)
        {
            var retModel = new ReturnModel<RToPage, string>();

            #region 校验签名

            var signResult = VerifyBase.SignAndToken<SBhRealNameModel>(model, out CST_user_info userInfo);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名

            if (userInfo.cst_account_id == null)
            {
                retModel.Message = "用户尚未完成开户";
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            var accountInfo = _accountInfoService.Find(userInfo.cst_account_id.Value);
            if (!accountInfo.JieSuan)
            {
                retModel.Message = "用户尚未完成开户";
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            if (accountInfo.BoHai)
            {
                retModel.Message = "用户已经绑卡";
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            var existBank = _accountInfoService.GetBankExist(model.BankCodeNo);
            if (existBank != null && existBank.mon_account_id != accountInfo.Id)
            {
                if (existBank.mon_account_id != null)
                {
                    var ownerBaner = _accountInfoService.Find(existBank.mon_account_id.Value);
                    if (ownerBaner.BoHai)
                    {
                        retModel.Message = "银行卡已被占用";
                        retModel.ReturnCode = (int)ReturnCode.DataEorr;
                        retModel.ReturnData = null;
                        retModel.Token = model.Token;
                        return retModel;
                    }
                }

            }
            //银行卡
            model.BankCodeNo = model.BankCodeNo.Trim();
            var bankCardInfo = _accountInfoService.GetBankInfo(userInfo.cst_account_id.Value);
            if (bankCardInfo == null)
            {
                //插入用户绑卡数据
                var bankInfo = new CST_bankcard_info
                {
                    mon_account_id = userInfo.cst_account_id,
                    bank_datetime = DateTime.Now,
                    bank_no = model.BankCodeNo,
                    bank_code = model.BankCode,
                    IsBoHai = true
                };
                _accountInfoService.AddBankCardInfo(bankInfo);
            }
            else
            {
                bankCardInfo.bank_no = model.BankCodeNo;
                bankCardInfo.bank_code = model.BankCode;
                _accountInfoService.UpdateBankCardInfo(bankCardInfo);
            }
            var returnUrl = new RToPage
            {
                Url = $"{_zfctWebConfig.LocalUrl}Page/UserRealName?token={System.Net.WebUtility.UrlEncode(model.Token)}"
            };
            retModel.Message = "成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.ReturnData = returnUrl;
            retModel.Token = model.Token;
            return retModel;
        }

        /// <summary>
        /// 用户是否开户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RUserState, string> UserState([FromBody] BaseSubmitModel model)
        {
            var retModel = new ReturnModel<RUserState, string>();

            #region 校验签名

            var signResult = VerifyBase.SignAndToken<BaseSubmitModel>(model, out CST_user_info userInfo);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名

            var state = new RUserState {Risk = (_userInvestTypeService.GetUserTypes(userInfo.Id) != null) ? "1" : "0"};
            if (userInfo.cst_account_id != null)
            {
                //获取用户开户信息
                var accountInfo = _accountInfoService.GetAccountInfoByUserId(userId: userInfo.Id);
                state.BoHai = accountInfo.BoHai ? "1" : "0";
                state.JieSuan = accountInfo.JieSuan ? "1" : "0";
                //查询是否投资授权
                if (accountInfo.BoHai)
                {
                    var authInfo = _bhUserService.AuthInfo(accountInfo.cst_plaCustId, AuthTyp.Invest);
                    state.Auth = authInfo.IsAuth;
                    state.AuthMoney = authInfo.AuthMoney;
                }
                retModel.Message = "成功";
                retModel.ReturnCode = (int)ReturnCode.Success;
                retModel.ReturnData = state;
                retModel.Token = model.Token;
                return retModel;
            }
            else
            {
                state.BoHai = "0";
                state.JieSuan = "0";
                state.Auth = "0";
            }

            retModel.Message = "成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.ReturnData = state;
            retModel.Token = model.Token;
            return retModel;
        }

        /// <summary>
        /// 绑定渤海银行卡
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RToPage, string> BoHaiBindCard([FromBody] BaseSubmitModel model)
        {
            var retModel = new ReturnModel<RToPage, string>();

            #region 校验签名

            var signResult = VerifyBase.SignAndToken<BaseSubmitModel>(model, out CST_user_info userInfo);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名

            //获取用户开户信息
            var accountInfo = _accountInfoService.GetAccountInfoByUserId(userId: userInfo.Id);
            if (accountInfo.JieSuan && accountInfo.BoHai)
            {
                var returnModel = new RToPage
                {
                    Url = $"{_zfctWebConfig.LocalUrl}Page/UserBindCard?token={System.Net.WebUtility.UrlEncode(model.Token)}"
                };
                retModel.Message = "成功";
                retModel.ReturnCode = (int)ReturnCode.Success;
                retModel.ReturnData = returnModel;
                retModel.Token = model.Token;
                return retModel;
            }
            retModel.Message = "用户尚未完成开户或绑卡";
            retModel.ReturnCode = (int)ReturnCode.DataEorr;
            retModel.ReturnData = null;
            retModel.Token = model.Token;
            return retModel;
        }

        /// <summary>
        /// 绑定渤海手机号
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RToPage, string> BoHaiBindMobile([FromBody] SBhBindMobile model)
        {
            var retModel = new ReturnModel<RToPage, string>();

            #region 校验签名

            var signResult = VerifyBase.SignAndToken<SBhBindMobile>(model, out CST_user_info userInfo);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名

            //获取用户开户信息
            var accountInfo = _accountInfoService.GetAccountInfoByUserId(userId: userInfo.Id);
            var rturnInfo = new RToPage();
            if (accountInfo.JieSuan && accountInfo.BoHai)
            {
                #region 校验参数

                if (!Regex.IsMatch(model.NewPhone, RegularExpression.Mobile))
                {
                    retModel.Message = "手机号码格式不正确！";
                    retModel.ReturnCode = (int)ReturnCode.RegisterEorr;
                    return retModel;
                }

                var verCode = _cacheManager.Get<String>(accountInfo.act_user_phone);
                if (verCode == null || verCode != model.VerCode)
                {
                    retModel.Message = "手机验证码过期或错误！";
                    retModel.ReturnCode = (int)ReturnCode.RegisterEorr;
                    return retModel;
                }

                if (accountInfo.act_user_phone.Equals(model.NewPhone)) {
                    retModel.Message = "新手机号与原手机号相同！";
                    retModel.ReturnCode = (int)ReturnCode.DataEorr;
                    retModel.ReturnData = rturnInfo;
                    retModel.Token = model.Token;
                    return retModel;
                }
              

                #endregion 校验参数
                rturnInfo.Url = $"{_zfctWebConfig.LocalUrl}Page/UserBindMobile?token={System.Net.WebUtility.UrlEncode(model.Token)}&newPhone={ model.NewPhone}";
                retModel.Message = "成功";
                retModel.ReturnCode = (int)ReturnCode.Success;
                retModel.ReturnData = rturnInfo;
                retModel.Token = model.Token;
                return retModel;
            }

            retModel.Message = "用户尚未完成开户或绑卡";
            retModel.ReturnCode = (int)ReturnCode.DataEorr;
            retModel.ReturnData = rturnInfo;
            retModel.Token = model.Token;
            return retModel;
        }

        /// <summary>
        /// 修改渤海交易密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RToPage, string> BoHaiBindPass([FromBody] SBoHaiBindPass model)
        {
            var retModel = new ReturnModel<RToPage, string>();

            #region 校验签名

            var signResult = VerifyBase.SignAndToken<BaseSubmitModel>(model, out CST_user_info userInfo);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名

            //获取用户开户信息
            var accountInfo = _accountInfoService.GetAccountInfoByUserId(userId: userInfo.Id);
            var rturnInfo = new RToPage();
            if (accountInfo.JieSuan && accountInfo.BoHai)
            {
                rturnInfo.Url = $"{_zfctWebConfig.LocalUrl}Page/UserBindPass?operationType={model.OperationType}&token={System.Net.WebUtility.UrlEncode(model.Token)}";
                retModel.Message = "成功";
                retModel.ReturnCode = (int)ReturnCode.Success;
                retModel.ReturnData = rturnInfo;
                retModel.Token = model.Token;
                return retModel;
            }
            rturnInfo.Url = "";
            retModel.Message = "用户尚未完成开户或绑卡";
            retModel.ReturnCode = (int)ReturnCode.DataEorr;
            retModel.ReturnData = rturnInfo;
            retModel.Token = model.Token;
            return retModel;
        }

        /// <summary>
        /// 用户借款数量统计
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RLoanCountStatistics, string> LoanCountStatistics([FromBody]BaseSubmitModel model)
        {
            var retModel = new ReturnModel<RLoanCountStatistics, string>();

            #region 校验签名

            var signResult = VerifyBase.SignAndToken<BaseSubmitModel>(model, out int userId);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名

            #region 统计借款

            var loanStatisticsItems = new List<LoanStatisticsType>
            {
                LoanStatisticsType.BiddingCount,
                LoanStatisticsType.FullCount,
                LoanStatisticsType.RepaymentCount,
                LoanStatisticsType.ClearedCount
            };
            var loanStatistics = _loanInfoService.LoanStatistics(userId, loanStatisticsItems);

            #endregion 统计借款

            var returnModel = new RLoanCountStatistics
            {
                BidingCount = loanStatistics.BiddingCount,
                FullCount = loanStatistics.FullCount,
                SettledCount = loanStatistics.ClearedCount,
                RepaymentCount = loanStatistics.RepaymentCount
            };
            retModel.Message = "查询成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.ReturnData = returnModel;
            retModel.Token = model.Token;
            return retModel;
        }

        /// <summary>
        /// 用户投资数量统计
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RInvestCountStatistics, string> InvestCountStatistics([FromBody]BaseSubmitModel model)
        {
            var retModel = new ReturnModel<RInvestCountStatistics, string>();

            #region 校验签名

            var signResult = VerifyBase.SignAndToken<BaseSubmitModel>(model, out int userId);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名

            #region 统计投资

            var investStatisticsItems = new List<InvestStatisticsType>
            {
                InvestStatisticsType.BiddingCount,
                InvestStatisticsType.RepaymentCount,
                InvestStatisticsType.ClearedCount
            };
            var investStatistics = _investInfoService.InvestStatistics(userId, investStatisticsItems);

            #endregion 统计投资

            var returnModel = new RInvestCountStatistics
            {
                BiddingCount = investStatistics.BiddingCount,
                ClearedCount = investStatistics.ClearedCount,
                RepaymentCount = investStatistics.RepaymentCount
            };
            retModel.Message = "查询成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.ReturnData = returnModel;
            retModel.Token = model.Token;
            return retModel;
        }

        /// <summary>
        /// 用户红包数量统计
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RRedEnvelopesCountStatistics, string> RedEnvelopesCountStatistics([FromBody]BaseSubmitModel model)
        {
            var retModel = new ReturnModel<RRedEnvelopesCountStatistics, string>();

            #region 校验签名

            var signResult = VerifyBase.SignAndToken<BaseSubmitModel>(model, out int userId);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名

            #region 红包统计

            var RedStatisticsItems = new List<RedStatisticsType>
            {
                RedStatisticsType.WaitUseCount,
                RedStatisticsType.ExpiredCount,
                RedStatisticsType.UsedCount
            };
            var redStatistics = _cstRedInfoService.UserRedStatistics(userId, RedStatisticsItems);

            #endregion 红包统计

            var returnModel = new RRedEnvelopesCountStatistics
            {
                ExprisedCount = redStatistics.ExpiredCount,
                UsedCount = redStatistics.UsedCount,
                WaitUseCount = redStatistics.WaitUseCount
            };
            retModel.Message = "查询成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.ReturnData = returnModel;
            retModel.Token = model.Token;
            return retModel;
        }

        /// <summary>
        /// 用户债权数量统计
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RTransferCountStatistics, string> TransferCountStatistics([FromBody]BaseSubmitModel model)
        {
            var retModel = new ReturnModel<RTransferCountStatistics, string>();

            #region 校验签名

            var signResult = VerifyBase.SignAndToken<BaseSubmitModel>(model, out int userId);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名

            #region 统计债转投资

            var investStatisticsItems = new List<InvestStatisticsType>
            {
                InvestStatisticsType.TransInvestCount
            };
            var investStatistics = _investInfoService.InvestStatistics(userId, investStatisticsItems);

            #endregion 统计债转投资

            #region 统计债权转让

            var canTransCount = _transInfoService.CanTransCount(userId);
            var transStatistics = _transInfoService.TransferStatistics(userId);

            #endregion 统计债权转让

            var returnModel = new RTransferCountStatistics
            {
                CantransCount = canTransCount,
                TransferedCount = transStatistics.TransferedCount,
                TransferingCount = transStatistics.TransferingCount,
                TransInvestCount = investStatistics.TransInvestCount
            };
            retModel.Message = "查询成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.ReturnData = returnModel;
            retModel.Token = model.Token;
            return retModel;
        }

        /// <summary>
        /// 用户借款账户统计
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RLoanAccountStatistics, string> LoanAccountStatistics([FromBody]BaseSubmitModel model)
        {
            var retModel = new ReturnModel<RLoanAccountStatistics, string>();

            #region 校验签名

            var signResult = VerifyBase.SignAndToken<BaseSubmitModel>(model, out int userId);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名

            #region 统计借款还款计划

            var loanPlanStatisticsItems = new List<LoanPlanStatisticsType>
            {
                LoanPlanStatisticsType.RepayPrincipal,
                LoanPlanStatisticsType.RepayRate,
                LoanPlanStatisticsType.RepayServiceFee
            };
            var loanPlanStatistics = _loanPlanService.UserLoanPlanStatistics(userId, loanPlanStatisticsItems);

            #endregion 统计借款还款计划

            var returnModel = new RLoanAccountStatistics
            {
                RepayPrincipal = loanPlanStatistics.RepayPrincipal,
                RepayRate = loanPlanStatistics.RepayRate,
                RepayServiceFee = loanPlanStatistics.RepayServiceFee
            };
            retModel.Message = "查询成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.ReturnData = returnModel;
            retModel.Token = model.Token;
            return retModel;
        }

        /// <summary>
        /// 用户投资账户统计
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RInvestAccountStatistics, string> InvestAccountStatistics([FromBody]BaseSubmitModel model)
        {
            var retModel = new ReturnModel<RInvestAccountStatistics, string>();

            #region 校验签名

            var signResult = VerifyBase.SignAndToken<BaseSubmitModel>(model, out int userId);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名

            #region 统计投资还款计划

            var investPlanStatisticsItems = new List<InvestPlanStatisticsType>
            {
                InvestPlanStatisticsType.WaitReceivePrincipal,
                InvestPlanStatisticsType.WaitReceiveIncome,
                InvestPlanStatisticsType.ReceivedPrincipal,
                InvestPlanStatisticsType.CumulativeIncome
            };
            var investPlanStatistics = _investerPlanService.InvestPlanStatistics(userId, investPlanStatisticsItems);

            //投资待收罚息
            var investWaitReceiveOverRate = _investerPlanService.CalculationOverRate(userId);

            #endregion 统计投资还款计划

            #region 统计投资

            var investStatisticsItems = new List<InvestStatisticsType>
            {
                InvestStatisticsType.InvestMoney
            };
            var investStatistics = _investInfoService.InvestStatistics(userId, investStatisticsItems);

            #endregion 统计投资

            var returnModel = new RInvestAccountStatistics
            {
                InvestMoney = investStatistics.InvestMoney,
                RepayEarnings = investPlanStatistics.CumulativeIncome,
                RepayPrincipal = investPlanStatistics.ReceivedPrincipal,
                // WaitRepayEarnings = investPlanStatistics.WaitReceiveIncome + investWaitReceiveOverRate,
                WaitRepayEarnings = investPlanStatistics.WaitReceiveIncome,
                WaitRepayPrincipal = investPlanStatistics.WaitReceivePrincipal
            };
            retModel.Message = "查询成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.ReturnData = returnModel;
            retModel.Token = model.Token;
            return retModel;
        }

        /// <summary>
        /// 用户交易记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<ReturnPageData<RTradingModel>, string> TradingInfo([FromBody]SAccountTrading model)
        {
            var retModel = new ReturnModel<ReturnPageData<RTradingModel>, string>();

            #region 校验签名

            var signResult = VerifyBase.SignAndToken<SAccountTrading>(model, out int userId);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名

            List<int> tradingType = new List<int>();
            bool isContain = true;
            switch (model.Type)
            {
                case 1:
                    isContain = false;
                    tradingType.Add(DataDictionary.transactiontype_FinanceTransfer);
                    tradingType.Add(DataDictionary.transactiontype_Withdrawals);
                    tradingType.Add(DataDictionary.transactiontype_Recharge); break;
                case 2:
                    tradingType.Add(DataDictionary.transactiontype_Withdrawals);
                    tradingType.Add(DataDictionary.transactiontype_Recharge); break;
                case 3:
                    tradingType.Add(DataDictionary.transactiontype_Recharge);
                    retModel.Extra1 = _cstTransactionService.AllTransactionMoney(tradingType,userId).ToString("0.00");
                    break;
                case 4:
                    tradingType.Add(DataDictionary.transactiontype_Withdrawals);
                    retModel.Extra1 = _cstTransactionService.AllTransactionMoney(tradingType, userId).ToString("0.00");
                    break;
                case 5:
                    tradingType.Add(DataDictionary.transactiontype_InvestmentCollection);
                    tradingType.Add(DataDictionary.transactiontype_ProjectFlow);
                    tradingType.Add(DataDictionary.transactiontype_FlowThaw); break;
                case 6:
                    tradingType.Add(DataDictionary.transactiontype_InvestFrazon);
                    tradingType.Add(DataDictionary.transactiontype_InvestTransfer);
                    tradingType.Add(DataDictionary.transactiontype_SubscriptionTransfer); break;
                case 7:
                    tradingType.Add(DataDictionary.transactiontype_InvestRed); break;
                case 8:
                    tradingType.Add(DataDictionary.transactiontype_FinanceTransfer); break;
                default: break;
            }
            try
            {
                var pageData = _cstTransactionService.GetListByCondition(userId, tradingType, model.Min, model.Max, model.Page, model.PageSize, isContain);
                var rePage = new ReturnPageData<RTradingModel>
                {
                    PagingData = pageData.Items.Select(p =>
                    {
                        var item = new RTradingModel()
                        {
                            id = p.Id,
                            TradingDate = p.pro_transaction_time == null ? "" : p.pro_transaction_time.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                            TradingMoney = p.pro_transaction_money.GetValueOrDefault(),
                            TradingType = p.pro_transaction_type == null ? "" : _iSYSDataDictionaryService.Find(p.pro_transaction_type.Value).sys_data_name,
                            TradingStatus = "成功"
                        };

                        if (model.Type.Equals(1))
                        {
                            item.TradingName = _loanInfoService.Find(p.pro_loan_id.GetValueOrDefault())?.pro_loan_use;
                        }
                        else
                        {
                            item.TradingOrderNo = p.pro_transaction_no;
                            item.TrandingAccountMoney = p.pro_balance_money.GetValueOrDefault();
                            item.FTrandingAccountMoney = p.pro_finance_money.GetValueOrDefault();
                        }
                        return item;
                    }),
                    Total = pageData.TotalNum,
                    TotalPageCount = pageData.TotalPageCount
                };

                retModel.ReturnData = rePage;
                retModel.Message = "成功";
                retModel.ReturnCode = (int)ReturnCode.Success;
                retModel.Token = model.Token;
                return retModel;
            }
            catch (Exception e)
            {
                retModel.ReturnData = null;
                retModel.Message = e.ToString();
                retModel.ReturnCode = (int)ReturnCode.QueryEorr;
                retModel.Token = model.Token;
                return retModel;
            }
        }

        /// <summary>
        /// 投资收益
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RInvestEarnings, string> InvestEarnings([FromBody]BaseSubmitModel model)
        {
            var retModel = new ReturnModel<RInvestEarnings, string>();

            #region 校验签名

            var signResult = VerifyBase.SignAndToken<BaseSubmitModel>(model, out int userId);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名

            var realNameInfo = _accountInfoService.GetRealNameInfo(userId);

            var investPlanStatisticsItems = new List<InvestPlanStatisticsType>
            {
                InvestPlanStatisticsType.CumulativeIncome,
                InvestPlanStatisticsType.ThridDaysIncome
            };
            var investPlanStatistics = _investerPlanService.InvestPlanStatistics(userId, investPlanStatisticsItems);

            var returnModel = new RInvestEarnings
            {
                AccumulativeEarnings = investPlanStatistics.CumulativeIncome,
                ThridDayEarnings = investPlanStatistics.ThridDaysIncome
            };
            returnModel.UserName = "客户";
            if (!string.IsNullOrEmpty(realNameInfo?.cst_user_realname))
            {
                returnModel.UserName = realNameInfo.cst_user_realname;
                if (realNameInfo.cst_user_sex)
                    returnModel.UserName += "先生";
                else
                    returnModel.UserName += "女士";
            }
            retModel.Message = "成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.ReturnData = returnModel;
            retModel.Token = model.Token;
            return retModel;
        }

        /// <summary>
        /// 用户融资转账
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<bool, string> FinanceTransfer([FromBody]SFinanceTransfer model)
        {
            var retModel = new ReturnModel<bool, string>();

            #region 校验签名

            var signResult = VerifyBase.SignAndToken<SFinanceTransfer>(model, out int userId);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = false;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名

            var accountInfo = _accountInfoService.GetAccountInfoByUserId(userId: userId);

            if (accountInfo != null && accountInfo.BoHai && accountInfo.JieSuan)
            {

                //线下充值同步
                _iBHAccountService.RechargeSyn(new SBHRechargeSyn
                {
                    SvcBody = new SBHRechargeSynBody
                    {
                        platformUid = accountInfo.invest_platform_id
                    }
                });

                var rBHAccountFinanceTransfer = _iBHAccountService.FinanceTransfer(new SBHFinanceTransfer
                {
                    SvcBody = new SBHFinanceTransferBody
                    {
                        platformUidInvestment = accountInfo.invest_platform_id,
                        platformUidFinance = accountInfo.financing_platform_id,
                        amount = model.Money.ToString("0.00")
                    }
                });

                if (rBHAccountFinanceTransfer.RspSvcHeader.returnCode.Equals(JSReturnCode.Success))
                {
                    var iAccBalance = _iBHAccountService.AccountQueryAccBalance(new SBHAccountQueryAccBalance
                    {
                        SvcBody = new SBHAccountQueryAccBalanceBody
                        {
                            platformUid = accountInfo.invest_platform_id
                        }
                    });
                    var fAccBalance = _iBHAccountService.AccountQueryAccBalance(new SBHAccountQueryAccBalance
                    {
                        SvcBody = new SBHAccountQueryAccBalanceBody
                        {
                            platformUid = accountInfo.financing_platform_id
                        }
                    });
                    var transaction = new CST_transaction_info
                    {
                        pro_transaction_money = Convert.ToDecimal(model.Money),
                        pro_transaction_no = CommonHelper.GetMchntTxnSsn(),
                        pro_transaction_time = DateTime.Now,
                        pro_transaction_type = DataDictionary.transactiontype_FinanceTransfer,
                        pro_user_id = accountInfo.act_user_id,
                        pro_user_type = accountInfo.act_user_type,
                        pro_transaction_status = DataDictionary.transactionstatus_success,
                        pro_transaction_remarks = "融资转账",
                        pro_transaction_resource = model.RequestSource,
                    };
                    if (iAccBalance.RspSvcHeader.returnCode.Equals(JSReturnCode.Success))
                    {
                        transaction.pro_balance_money = decimal.Parse(iAccBalance.SvcBody.withdrawAmount);
                    }
                    if (fAccBalance.RspSvcHeader.returnCode.Equals(JSReturnCode.Success))
                    {
                        transaction.pro_finance_money = decimal.Parse(fAccBalance.SvcBody.withdrawAmount);
                    }
                    _cstTransactionService.Add(transaction);
                    retModel.Message = "成功";
                    retModel.ReturnCode = (int)ReturnCode.Success;
                    retModel.ReturnData = true;
                }
                else
                {
                    retModel.Message = rBHAccountFinanceTransfer.RspSvcHeader.returnMsg;
                    retModel.ReturnCode = (int)ReturnCode.DataEorr;
                    retModel.ReturnData = false;
                }

                retModel.Token = model.Token;
                return retModel;
            }

            retModel.Message = "用户尚未完成开户或绑卡";
            retModel.ReturnCode = (int)ReturnCode.UnOpenAccount;
            retModel.Token = model.Token;
            return retModel;
        }

        /// <summary>
        /// 渤海用户信息查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RBHQueryUserInfoBody, string> BoHaiUserInfo([FromBody] BaseSubmitModel model)
        {
            var retModel = new ReturnModel<RBHQueryUserInfoBody, string>();

            #region 验签

            var resultSign = VerifyBase.SignAndToken<BaseSubmitModel>(model, out CST_user_info userInfo);
            if (!resultSign.Equals(ReturnCode.Success))
            {
                retModel.ReturnCode = (int)resultSign;
                retModel.Message = "签名验证失败";
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 验签

            //查询用户账户信息
            var accountInfo = _accountInfoService.GetAccountInfoByUserId(userId: userInfo.Id);
            //查询用户是否注册渤海
            if (!accountInfo.BoHai || string.IsNullOrEmpty(accountInfo.cst_plaCustId))
            {
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.Message = "用户尚未渤海注册绑卡";
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            //查询用户信息
            var umodel = new SBHQueryUserInfo
            {
                SvcBody =
                {
                    plaCustId = accountInfo.cst_plaCustId,
                    mblNo=accountInfo.act_user_phone
                }
            };
            var info = _bhUserService.QueryUserInfo(umodel);
            if (info == null)
            {
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.Message = "用户信息查询失败";
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            else
            {
                retModel.ReturnCode = (int)ReturnCode.Success;
                retModel.Message = "用户信息查询成功";
                retModel.ReturnData = info;
                retModel.Token = model.Token;
                return retModel;
            }
        }

        /// <summary>
        /// 结算用户账户注销
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<string, string> JsUserCancel([FromBody] BaseSubmitModel model)
        {
            var retModel = new ReturnModel<string, string>();

            #region 验签

            var resultSign = VerifyBase.SignAndToken<BaseSubmitModel>(model, out CST_user_info userInfo);
            if (!resultSign.Equals(ReturnCode.Success))
            {
                retModel.ReturnCode = (int)resultSign;
                retModel.Message = "签名验证失败";
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 验签

            //查询用户账户信息
            var accountInfo = _accountInfoService.GetAccountInfoByUserId(userId: userInfo.Id);
            //查询用户是否注册渤海
            if (accountInfo.BoHai || !string.IsNullOrEmpty(accountInfo.cst_plaCustId))
            {
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.Message = "用户已经渤海注册绑卡,不允许注销";
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            if (!accountInfo.JieSuan)
            {
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.Message = "用户尚未完成开户";
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            //查询用户信息
            var umodel = new SBHUserCancel
            {
                SvcBody =
                {
                    platformUid = userInfo.Id.ToString()
                }
            };
            var info = _bhUserService.UserCancel(umodel);
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.Message = info;
            retModel.ReturnData = info;
            retModel.Token = model.Token;
            return retModel;
        }

        /// <summary>
        /// 用户银行卡信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RUserBankInfo, string> UserBankInfo([FromBody] BaseSubmitModel model)
        {
            var retModel = new ReturnModel<RUserBankInfo, string>();

            #region 验签

            var resultSign = VerifyBase.SignAndToken<BaseSubmitModel>(model, out int userId);
            if (!resultSign.Equals(ReturnCode.Success))
            {
                retModel.ReturnCode = (int)resultSign;
                retModel.Message = "签名验证失败";
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 验签

            //查询用户账户信息
            var accountInfo = _accountInfoService.GetAccountInfoByUserId(userId: userId);
            var returnModel = new RUserBankInfo();
            if (accountInfo != null)
            {

                returnModel.IsJieSuan = accountInfo.JieSuan;
                returnModel.IsBoHai = accountInfo.BoHai;
                if (accountInfo.JieSuan && accountInfo.BoHai)
                {
                    var bankInfo = _accountInfoService.GetUserBankInfos(accountInfo.Id);
                    returnModel.BankInfos = bankInfo.Select(p =>
                    {
                        var item = new RBankInfo()
                        {
                            BankCode = p.bank_code,
                            CardNumber = $"{p.bank_no.Substring(0, 4)} **** **** {p.bank_no.Substring(p.bank_no.Length - 4, 4)}"
                        };
                        var bank = GetBankInfo(p.bank_code);
                        item.BankName = bank.BankCardName;
                        item.BankUrl = bank.BankCardLogo;
                        item.BankBack = bank.BankCardBack;
                        return item;
                    }).ToList();
                }
            }
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.Message = "成功";
            retModel.ReturnData = returnModel;
            retModel.Token = model.Token;
            return retModel;
        }

        /// <summary>
        /// 投资合同
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RToPage, string> DownInvestContract([FromBody]SDownInvestContract model)
        {
            var retModel = new ReturnModel<RToPage, string>();

            #region 校验签名

            var signResult = VerifyBase.SignAndToken<SDownInvestContract>(model, out int userId);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名

            var contract = _pro_ContractService.GetInvestContract(model.InvestId);
            if (contract != null)
            {
                if (contract.Status == 1)
                {
                   
                    retModel.ReturnCode = (int)ReturnCode.Success;
                    retModel.ReturnData = new RToPage {
                       Url = System.IO.Path.Combine(_fastDFSConfig.FastDFSWebAddress, contract.ContractPath)
                    };
                    return retModel;
                }
                else if (contract.Status == 0 || contract.Status == 4)
                {
                    retModel.ReturnCode = (int)ReturnCode.NoDate;
                    retModel.Message = "合同尚未生成，请稍后再试!";
                    return retModel;
                }
                else if (contract.Status == 2)
                {
                    retModel.ReturnCode = (int)ReturnCode.NoDate;
                    retModel.Message = "合同正在生成中，请稍后再试!";
                }
                else
                {
                    retModel.ReturnCode = (int)ReturnCode.NoDate;
                    retModel.Message = "合同生成失败，请联系我们！";
                }
                return retModel;
            }

            retModel.Message = "未发现合同，请联系我们！";
            retModel.ReturnCode = (int)ReturnCode.NoDate;
            retModel.Token = model.Token;
            return retModel;
        }

        /// <summary>
        /// 借款合同
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RToPage, string> DownLoanContract([FromBody]SDownLoanContract model)
        {
            var retModel = new ReturnModel<RToPage, string>();

            #region 校验签名

            var signResult = VerifyBase.SignAndToken<SDownLoanContract>(model, out int userId);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名

            var contract = _pro_ContractService.GetInvestContract(model.LoanId);
            if (contract != null)
            {
                if (contract.Status == 1)
                {

                    retModel.ReturnCode = (int)ReturnCode.Success;
                    retModel.ReturnData = new RToPage
                    {
                        Url = System.IO.Path.Combine(_fastDFSConfig.FastDFSWebAddress,contract.ContractPath)
                    };
                    return retModel;
                }
                else if (contract.Status == 0 || contract.Status == 4)
                {
                    retModel.ReturnCode = (int)ReturnCode.NoDate;
                    retModel.Message = "合同尚未生成，请稍后再试!";
                    return retModel;
                }
                else if (contract.Status == 2)
                {
                    retModel.ReturnCode = (int)ReturnCode.NoDate;
                    retModel.Message = "合同正在生成中，请稍后再试!";
                }
                else
                {
                    retModel.ReturnCode = (int)ReturnCode.NoDate;
                    retModel.Message = "合同生成失败，请联系我们！";
                }
                return retModel;
            }

            retModel.Message = "未发现合同，请联系我们！";
            retModel.ReturnCode = (int)ReturnCode.NoDate;
            retModel.Token = model.Token;
            return retModel;
        }

        /// <summary>
        /// 更新用户银行卡
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<bool, string> UpdateBankInfo([FromBody]BaseSubmitModel model)
        {
            var retModel = new ReturnModel<bool, string>();

            #region 校验签名

            var signResult = VerifyBase.SignAndToken<BaseSubmitModel>(model, out int userId);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = false;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名

            var accountInfo = _accountInfoService.GetAccountInfoByUserId(userId: userId);

            if (accountInfo != null && accountInfo.BoHai && accountInfo.JieSuan)
            {

                var queryUserInfo = _bhUserService.QueryUserInfo(new Data.BoHai.SubmitModels.SBHQueryUserInfo
                {
                    SvcBody = new Data.BoHai.SubmitModels.SBHQueryUserInfoBody
                    {
                        plaCustId = accountInfo.cst_plaCustId,
                        mblNo=accountInfo.act_user_phone
                    }
                });

                if (queryUserInfo != null)
                {
                   _accountInfoService.DeleteUserBankCard(accountInfo.Id);
                    foreach (var bankinfo in queryUserInfo.items)
                    {
                        _accountInfoService.AddBankCardInfo(new Data.CST.CST_bankcard_info
                        {
                            bank_code = bankinfo.capCorg,
                            bank_no = bankinfo.capCrdNo,
                            mon_account_id = accountInfo.Id,
                            bank_datetime = DateTime.Now,
                            IsBoHai = true
                        });
                    }
                }
                retModel.ReturnData = true;
                retModel.ReturnCode = (int)ReturnCode.Success;
                retModel.Token = model.Token;
                return retModel;
            }
            retModel.ReturnData = false;
            retModel.Message = "用户尚未完成开户或绑卡";
            retModel.ReturnCode = (int)ReturnCode.UnOpenAccount;
            retModel.Token = model.Token;
            return retModel;
        }

        #endregion Action

        #region 用户授权相关
        /// <summary>
        /// 用户发起授权
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RToPage, string> AutoInvestAuth([FromBody] SAuthType model)
        {
            var retModel = new ReturnModel<RToPage, string>();

            #region 校验签名

            var signResult = VerifyBase.SignAndToken<BaseSubmitModel>(model, out CST_user_info userInfo);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名

            if (userInfo.cst_account_id == null)
            {
                retModel.Message = "用户尚未完成开户";
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            var accountInfo = _accountInfoService.Find(userInfo.cst_account_id.Value);
            if (!accountInfo.JieSuan)
            {
                retModel.Message = "用户尚未完成开户";
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            if (!accountInfo.BoHai)
            {
                retModel.Message = "用户尚未绑卡";
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }

            var returnUrl = new RToPage
            {
                Url = $"{_zfctWebConfig.LocalUrl}Page/AutoInvestAuth?token={System.Net.WebUtility.UrlEncode(model.Token)}&authType=" +model.AuthType
            };
            retModel.Message = "成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.ReturnData = returnUrl;
            retModel.Token = model.Token;
            return retModel;
        }
        /// <summary>
        /// 查询用户授权信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RUserAuthInfo, string> AuthQuery([FromBody] BaseSubmitModel model)
        {
            var retModel = new ReturnModel<RUserAuthInfo, string>();
            var returnData = new RUserAuthInfo
            {
                AuthInfos = 
                {
                    new RAuthInfo{AuthType = "投标",AuthCode = "11",AuthEnd = "",AuthStart = "",AuthState = "0",AuthMoney = "0.00"},
                    new RAuthInfo{AuthType = "缴费",AuthCode = "59",AuthEnd = "",AuthStart = "",AuthState = "0",AuthMoney = "0.00"},
                    new RAuthInfo{AuthType = "还款",AuthCode = "60",AuthEnd = "",AuthStart = "",AuthState = "0",AuthMoney = "0.00"}
                }
            };
            #region 校验签名

            var signResult = VerifyBase.SignAndToken<BaseSubmitModel>(model, out CST_user_info userInfo);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名

            if (userInfo.cst_account_id == null)
            {
                retModel.Message = "用户尚未完成开户";
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            var accountInfo = _accountInfoService.Find(userInfo.cst_account_id.Value);
            if (!accountInfo.JieSuan)
            {
                retModel.Message = "用户尚未完成开户";
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            if (!accountInfo.BoHai)
            {
                retModel.Message = "用户尚未绑卡";
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            var requestModel = new SBHQueryAuthInf { SvcBody = { plaCustId = accountInfo.cst_plaCustId } };
            var authQuery = _bhUserService.AuthResult(requestModel);
            if (authQuery != null && authQuery.RspSvcHeader.returnCode.Equals(JSReturnCode.Success))
            {
                foreach (var auth in authQuery.SvcBody.items)
                {
                    if (auth.end_dt.Length == 8)
                    {
                        var startDate = CommonHelper.HandleStringTime(auth.start_dt);
                        var endDate = CommonHelper.HandleStringTime(auth.end_dt);
                        switch (auth.auth_typ)
                        {
                            case "11":
                                returnData.AuthInfos.First(p=>p.AuthCode=="11").AuthEnd = endDate.ToString("yyyy-MM-dd");
                                returnData.AuthInfos.First(p => p.AuthCode == "11").AuthStart = startDate.ToString("yyyy-MM-dd");
                                returnData.AuthInfos.First(p => p.AuthCode == "11").AuthState = (DateTime.Now < endDate) ? "1" : "2";
                                returnData.AuthInfos.First(p => p.AuthCode == "11").AuthMoney = (Convert.ToDecimal(auth.auth_amt)/100).ToString("0.00");
                                break;
                            case "59":
                                returnData.AuthInfos.First(p => p.AuthCode == "59").AuthEnd = endDate.ToString("yyyy-MM-dd");
                                returnData.AuthInfos.First(p => p.AuthCode == "59").AuthStart = startDate.ToString("yyyy-MM-dd");
                                returnData.AuthInfos.First(p => p.AuthCode == "59").AuthState = (DateTime.Now < endDate) ? "1" : "2";
                                returnData.AuthInfos.First(p => p.AuthCode == "59").AuthMoney = (Convert.ToDecimal(auth.auth_amt) / 100).ToString("0.00");
                                break;
                            case "60":
                                returnData.AuthInfos.First(p => p.AuthCode == "60").AuthEnd = endDate.ToString("yyyy-MM-dd");
                                returnData.AuthInfos.First(p => p.AuthCode == "60").AuthStart = startDate.ToString("yyyy-MM-dd");
                                returnData.AuthInfos.First(p => p.AuthCode == "60").AuthState = (DateTime.Now < endDate) ? "1" : "2";
                                returnData.AuthInfos.First(p => p.AuthCode == "60").AuthMoney = (Convert.ToDecimal(auth.auth_amt) / 100).ToString("0.00");
                                break;
                        }
                    }
                    returnData.IsAuth = "1";
                }
            }
            retModel.Message = "成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.ReturnData = returnData;
            retModel.Token = model.Token;
            return retModel;
        }

        /// <summary>
        /// 验证用户是否授权
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<string, string> IsAuth([FromBody]SIsAuth model)
        {
            var retModel = new ReturnModel<string, string>();

            #region 校验签名

            var signResult = VerifyBase.SignAndToken<BaseSubmitModel>(model, out CST_user_info userInfo);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = "0";
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名

            if (userInfo.cst_account_id == null)
            {
                retModel.Message = "用户尚未完成开户";
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.ReturnData = "0";
                retModel.Token = model.Token;
                return retModel;
            }
            var accountInfo = _accountInfoService.Find(userInfo.cst_account_id.Value);
            if (!accountInfo.JieSuan)
            {
                retModel.Message = "用户尚未完成开户";
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.ReturnData = "0";
                retModel.Token = model.Token;
                return retModel;
            }
            if (!accountInfo.BoHai)
            {
                retModel.Message = "用户尚未绑卡";
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.ReturnData = "0";
                retModel.Token = model.Token;
                return retModel;
            }
            var authType = model.AuthType == 1
                ? AuthTyp.Invest
                : model.AuthType == 2
                    ? AuthTyp.Payment
                    : AuthTyp.Repayment;
            var isAuth = _bhUserService.IsAuth(accountInfo.cst_plaCustId, authType);
            retModel.Message = "查询成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.ReturnData = isAuth ? "1" : "0";
            retModel.Token = model.Token;
            return retModel;
        }

        /// <summary>
        /// 用户是否授权并携带授权金额
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RUserAuth, string> AuthInfo([FromBody] SIsAuth model)
        {
            var retModel = new ReturnModel<RUserAuth, string>();
            #region 校验签名

            var signResult = VerifyBase.SignAndToken<BaseSubmitModel>(model, out CST_user_info userInfo);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名
            if (userInfo.cst_account_id == null)
            {
                retModel.Message = "用户尚未完成开户";
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            var accountInfo = _accountInfoService.Find(userInfo.cst_account_id.Value);
            if (!accountInfo.JieSuan)
            {
                retModel.Message = "用户尚未完成开户";
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            if (!accountInfo.BoHai)
            {
                retModel.Message = "用户尚未绑卡";
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            var authType = model.AuthType == 1
                ? AuthTyp.Invest
                : model.AuthType == 2
                    ? AuthTyp.Payment
                    : AuthTyp.Repayment;
            var retResult = _bhUserService.AuthInfo(accountInfo.cst_plaCustId, authType);
            retModel.Message = "查询成功";
            retModel.ReturnCode = (int)ReturnCode.DataEorr;
            retModel.ReturnData = retResult;
            retModel.Token = model.Token;
            return retModel;
        }


        /// <summary>
        ///获取用户第三方认证信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RUserThirdPartInfo, string> ThirdPartInfo([FromBody] BaseSubmitModel model)
        {
            var retModel = new ReturnModel<RUserThirdPartInfo, string>();

            #region 校验签名

            var signResult = VerifyBase.SignAndToken<BaseSubmitModel>(model, out CST_user_info userInfo);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            #endregion 校验签名
           
            if (userInfo.cst_account_id == null)
            {
                retModel.Message = "用戶尚未进行过开户操作";
                retModel.ReturnCode = (int)ReturnCode.Success;
                retModel.ReturnData = new RUserThirdPartInfo{PhoneNo = userInfo.cst_user_phone};
                retModel.Token = model.Token;
                return retModel;
            }
            var accountInfo = _accountInfoService.Find(userInfo.cst_account_id.Value);
            var thirdInfo = new RUserThirdPartInfo
            {
                RealName = accountInfo.act_legal_name,
                PhoneNo = accountInfo.act_user_phone,
                IdCard = accountInfo.act_user_card,
                JieSuan = accountInfo.JieSuan ? "1" : "0",
                JieSuanCode = accountInfo.JieSuanCode,
                JieSuanMsg = accountInfo.JieSuanMsg,
                AccountPhone = accountInfo.act_user_phone,
                OnceJieSuan = accountInfo.JieSuanCode == null ? "0" : "1",
                BoHai = accountInfo.BoHai ? "1" : "0",
                BoHaiCode = accountInfo.BhCode,
                BohaiMsg = accountInfo.BhMsg,
                BusinessProperty = accountInfo.act_business_property

            };

            var bankInfo = _accountInfoService.GetBankInfo(accountInfo.Id);
            if (bankInfo != null)
            {

                thirdInfo.BankCode = bankInfo.bank_code;
                thirdInfo.BankName = GetBankInfo(bankInfo.bank_code)?.BankCardName;
                thirdInfo.BankNo = bankInfo.bank_no;
            }
            if (!string.IsNullOrEmpty(thirdInfo?.BankCode) && thirdInfo.BoHai == "0")
            {
                thirdInfo.OnceBohai = "1";
            }
            if (thirdInfo.BoHai == "1" && thirdInfo.JieSuan == "1")
            {
                thirdInfo.IsAuth = _bhUserService.IsAuth(accountInfo.cst_plaCustId, AuthTyp.Invest) ? "1" : "0";
            }
            retModel.Message = "查询成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.ReturnData = thirdInfo;
            retModel.Token = model.Token;
            return retModel;
        }

        #endregion


        #region 线下充值记录
        /// <summary>
        /// 线下充值记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<ReturnPageData<RTradingModel>, string> OfflineRechargeRecord([FromBody] SOfflineRecharge model)
        {
            var retModel = new ReturnModel<ReturnPageData<RTradingModel>, string>();

            #region 校验签名
            var signResult = VerifyBase.SignAndToken<SOfflineRecharge>(model, out CST_user_info userInfo);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名

            if (userInfo.cst_account_id == null)
            {
                retModel.Message = "用户尚未完成开户";
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            var accountInfo = _accountInfoService.Find(userInfo.cst_account_id.Value);
            if (!accountInfo.JieSuan)
            {
                retModel.Message = "用户尚未完成开户";
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            if (!accountInfo.BoHai)
            {
                retModel.Message = "用户尚未完成绑卡";
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            //判断条件是否足

            if (model.EDate > DateTime.Now.AddDays(-2))
            {
                retModel.Message = "结束日期最大为T-2日";
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            if (CommonHelper.DateDiff(model.SDate, model.EDate) > 7)
            {
                retModel.Message = "起始日期与结束日期最大间隔为7日";
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }

            var requestModel = new SBHQueryChargeDetail
            {
                SvcBody = new SBHQueryChargeDetailBody
                {
                    accountNo = accountInfo.cst_plaCustId,
                    queryTyp = "1",
                    startDate = model.SDate.ToString("yyyyMMdd"),
                    endDate = model.EDate.ToString("yyyyMMdd"),
                    pageNo = model.Page.ToString()
                }
            };
            var result = _bhUserService.QueryChargeDetail(requestModel);
            if (result == null)
            {
                retModel.Message = "查询失败";
                retModel.ReturnCode = (int) ReturnCode.DataEorr;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            else
            {
                var returnResult = new ReturnPageData<RTradingModel>();
                if (model.Page == 1)
                {
                    returnResult.Total = Convert.ToInt32(result.totalNum);
                    returnResult.TotalPageCount = Convert.ToInt32(result.totalPage);
                }
                else
                {
                    returnResult.Total = model.Total;
                    returnResult.TotalPageCount = model.PageTotal;
                }
                var resultDatas = new List<RTradingModel>();
                foreach (var data in result.items)
                {
                    var resultData = new RTradingModel
                    {
                        TradingDate = data.acdate,
                        TradingOrderNo = data.transId,
                        TradingMoney = Convert.ToDecimal(data.transAmt),
                        FeeAmt = data.feeAmt,
                        TradingStatus = DealTransState(data.transStat),
                        FailedReson = data.falRsn
                    };
                    resultDatas.Add(resultData);
                }
                returnResult.PagingData = resultDatas;
                retModel.Message = "查询成功";
                retModel.ReturnCode = (int)ReturnCode.Success;
                retModel.ReturnData = returnResult;
                retModel.Token = model.Token;
                return retModel;
            }

        }

        /// <summary>
        /// 废弃的线下充值记录查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        private ReturnModel<ReturnPageData<RTradingModel>, string> OfflineRechargeRecord2([FromBody]BaseSubmitModel model)
        {
            var retModel = new ReturnModel<ReturnPageData<RTradingModel>, string>();

            #region 校验签名
            var signResult = VerifyBase.SignAndToken<SOfflineRecharge>(model, out CST_user_info userInfo);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名

            if (userInfo.cst_account_id == null)
            {
                retModel.Message = "用户尚未完成开户";
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            var accountInfo = _accountInfoService.Find(userInfo.cst_account_id.Value);
            if (!accountInfo.JieSuan)
            {
                retModel.Message = "用户尚未完成开户";
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            if (!accountInfo.BoHai)
            {
                retModel.Message = "用户尚未完成绑卡";
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            //判断条件是否足

            var requestModel = new SBHQueryChargeDetail
            {
                SvcBody = new SBHQueryChargeDetailBody
                {
                    accountNo = accountInfo.cst_plaCustId,
                    queryTyp = "2",
                    pageNo = "1"
                }
            };
            var result = _bhUserService.QueryChargeDetail(requestModel);
            if (result == null)
            {
                retModel.Message = "查询失败";
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            else
            {
                var returnResult = new ReturnPageData<RTradingModel>();
                returnResult.Total = Convert.ToInt32(result.totalNum);
                returnResult.TotalPageCount = Convert.ToInt32(result.totalPage);
                var resultDatas = new List<RTradingModel>();
                foreach (var data in result.items)
                {
                    var resultData = new RTradingModel
                    {
                        TradingDate = data.acdate,
                        TradingOrderNo = data.transId,
                        TradingMoney = Convert.ToDecimal(data.transAmt),
                        FeeAmt = data.feeAmt,
                        TradingStatus = DealTransState(data.transStat),
                        FailedReson = data.falRsn
                    };
                    resultDatas.Add(resultData);
                }
                returnResult.PagingData = resultDatas;
                retModel.Message = "查询成功";
                retModel.ReturnCode = (int)ReturnCode.Success;
                retModel.ReturnData = returnResult;
                retModel.Token = model.Token;
                return retModel;
            }

        }

        private string DealTransState(string state)
        {
            string result;
            switch (state)
            {
                case "S1":
                    result = "交易成功,已清分";
                    break;
                case "F1":
                    result = "交易失败,未清分";
                    break;
                case "W2":
                    result = "请求处理中";
                    break;
                case "W3":
                    result = "系统受理中";
                    break;
                case "W4":
                    result = "银行受理中";
                    break;
                case "S2":
                    result = "撤标解冻成功";
                    break;
                case "S3":
                    result = "放款解冻成功";
                    break;
                case "B1":
                    result = "部分成功,部分冻结";
                    break;
                case "R9":
                    result = "审批拒绝";
                    break;
                case "F2":
                    result = "撤标解冻失败";
                    break;
                default:
                    result = "未知状态";
                    break;
            }
            return result;
        }

        #endregion


        /// <summary>
        /// 验证企业证件信息(组织机构代码,营业执照编号,税务登记号,身份证账号)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<List<RVerifyInfo>, string> VerifyCompanyInfo([FromBody]SVerifyCompanyInfo model)
        {
            var rModel = new ReturnModel<List<RVerifyInfo>, string>();

            List<RVerifyInfo> list = new List<RVerifyInfo>();


            if (!string.IsNullOrEmpty(model.BusiCode))
            {
                var account = _companyInfoService.GetComayInfo(busiCode: model.BusiCode);
                if (account != null && account.Id > 0)
                {
                    list.Add(new RVerifyInfo
                    {
                        Type= "BusiCode",
                        IsExit=true
                    });
                }
            }
            if (!string.IsNullOrEmpty(model.InstuCode))
            {
                var account = _companyInfoService.GetComayInfo(instuCode: model.InstuCode);
                if (account != null && account.Id > 0)
                {
                    list.Add(new RVerifyInfo
                    {
                        Type = "InstuCode",
                        IsExit = true
                    });
                }
            }
            if (!string.IsNullOrEmpty(model.TaxCode))
            {
                var account = _companyInfoService.GetComayInfo(taxCode: model.TaxCode);
                if (account != null && account.Id > 0)
                {
                    list.Add(new RVerifyInfo
                    {
                        Type = "TaxCode",
                        IsExit = true
                    });
                }
            }

            rModel.ReturnData = list;
            return rModel;
        }


        #region 商戶控台使用的接口
        /// <summary>
        /// 自用不做加密处理
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<List<RUserInfos>, string> MechatUserInfos([FromBody] SMerchatUsers model)
        {
            var retModel = new ReturnModel<List<RUserInfos>, string>();
            if (!string.IsNullOrEmpty(model.UserPlaCustIds))
            {
                var result = _accountInfoService.GetMerchatUserInfos(model.UserPlaCustIds);
                retModel.ReturnData = result;
            }
            else
            {
                retModel.ReturnData = null;
            }
            retModel.Message = "查询成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            return retModel;
        }

        /// <summary>
        /// 根据条件查询用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RUserInfos, string> MerchantGetUserInfo([FromBody] MerchantGetUser model)
        {
            var retModel = new ReturnModel<RUserInfos, string>();
            var userInfo = _accountInfoService.GetAccountInfo(phone: model.UserPhone, realName: model.RealName);
            if (userInfo != null)
            {
                retModel.Message = "用户查询成功";
                retModel.ReturnCode = (int)ReturnCode.Success;
                retModel.ReturnData = userInfo;
                retModel.Token = model.Token;
                return retModel;
            }
            else
            {
                retModel.Message = "用户查询失败";
                retModel.ReturnCode = (int)ReturnCode.Success;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
        }

        #endregion

    }
}