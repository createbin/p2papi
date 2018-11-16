using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ZFCTAPI.Core;
using ZFCTAPI.Core.Enums;
using ZFCTAPI.Core.Helpers;
using ZFCTAPI.Core.Provider;
using ZFCTAPI.Data.ApiModels.ReturnModels;
using ZFCTAPI.Data.ApiModels.SubmitModels;
using ZFCTAPI.Data.BoHai.ReturnModels;
using ZFCTAPI.Data.BoHai.SubmitModels;
using ZFCTAPI.Data.CST;
using ZFCTAPI.Data.PRO;
using ZFCTAPI.Services.BoHai;
using ZFCTAPI.Services.InvestInfo;
using ZFCTAPI.Services.LoanInfo;
using ZFCTAPI.Services.Popular;
using ZFCTAPI.Services.Sys;
using ZFCTAPI.Services.Transaction;
using ZFCTAPI.Services.UserInfo;
using ZFCTAPI.WebApi.RequestAttribute;
using ZFCTAPI.WebApi.Validates;

namespace ZFCTAPI.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [RequestLog]
    public class LoanController : Controller
    {
        private static ILoanInfoService _loanInfoService;
        private static IUploadInfoService _uploadInfoService;
        private static IInvestInfoService _investInfoService;
        private static IAccountInfoService _accountInfoService;
        private static ILoanPlanService _loanPlanService;
        private static IBHUserService _bhUserService;
        private static ICstTransactionService _transactionService;
        private static IPopEnvelopeRedService _redService;
        private static IBHAccountService _bhAccountService;
        private static ICompanyInfoService _companyInfoService;
        private static ISYSDataDictionaryService _dataDictionaryService;
        private static ICstUserInfoService _userInfoService;

        public LoanController(ILoanInfoService loanInfoService,
            IUploadInfoService uploadInfoService,
            IInvestInfoService investInfoService,
            IAccountInfoService accountInfoService,
            ILoanPlanService loanPlanService,
            IBHUserService bhUserService,
            ICstTransactionService transactionService,
            IPopEnvelopeRedService redService,
            IBHAccountService iBHAccountService,
            ICompanyInfoService companyInfoService,
            ISYSDataDictionaryService dataDictionaryService,
            ICstUserInfoService userInfoService)
        {
            _loanInfoService = loanInfoService;
            _uploadInfoService = uploadInfoService;
            _investInfoService = investInfoService;
            _accountInfoService = accountInfoService;
            _loanPlanService = loanPlanService;
            _bhUserService = bhUserService;
            _transactionService = transactionService;
            _redService = redService;
            _bhAccountService = iBHAccountService;
            _companyInfoService = companyInfoService;
            _dataDictionaryService = dataDictionaryService;
            _userInfoService = userInfoService;
        }



        /// <summary>
        /// 新手标获取分页数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<ReturnPageData<RLoanAbstract>, string> NewHandList([FromBody]BasePageModel model)
        {
            var retModel =new ReturnModel<ReturnPageData<RLoanAbstract>, string>();
            #region 校验签名
            var signResult = VerifyBase.Sign<BasePageModel>(model);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            #endregion

            var newHandList = new List<RLoanAbstract>();
            var newHandPage = _loanInfoService.LoanInfoPage(LoanType.NewHand,model.Page,model.PageSize);
            if (newHandPage.Items.Any())
            {
                newHandList.AddRange(newHandPage.Items.Select(GetLoanAbstract));
            }

            #region 返回数据
            var rePage = new ReturnPageData<RLoanAbstract>
            {
                PagingData = newHandList,
                Total = newHandPage.TotalNum,
                TotalPageCount = newHandPage.TotalPageCount
            };
            retModel.Message = "查询成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.ReturnData = rePage;
            retModel.Token = model.Token;
            #endregion

            return retModel;

        }
        /// <summary>
        /// 推荐标获取分页数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<ReturnPageData<RLoanAbstractLable>, string> LoanList([FromBody]BasePageModel model)
        {
            var retModel = new ReturnModel<ReturnPageData<RLoanAbstractLable>, string>();
            #region 校验签名
            var signResult = VerifyBase.Sign<BasePageModel>(model);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            #endregion

            var loanList = new List<RLoanAbstractLable>();
            var loanPage = _loanInfoService.LoanInfoPage(LoanType.Recommand, model.Page, model.PageSize);
            if (loanPage.Items.Any())
            {
                loanList.AddRange(loanPage.Items.Select(GetLoanAbstractLable));
            }

            var rePage = new ReturnPageData<RLoanAbstractLable>
            {
                PagingData = loanList,
                Total = loanPage.TotalNum,
                TotalPageCount = loanPage.TotalPageCount
            };
            retModel.Message = "查询成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.ReturnData = rePage;
            retModel.Token = model.Token;
            return retModel;

        }
        /// <summary>
        /// 标的详情
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RLoanDetail, string> LoanDetail([FromBody]SLoanDetail model)
        {
            model.ApiName = "标的详情";
            var retModel = new ReturnModel<RLoanDetail, string>();
            #region 校验签名
            var signResult = VerifyBase.Sign<SLoanDetail>(model);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            #endregion
            var loanInfo = _loanInfoService.Find(model.LoanId);
            var loanResult = new RLoanDetail();
            if (loanInfo != null)
            {
                loanResult = GetLoanDetail(loanInfo);
            }

            retModel.Message = "查询成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.ReturnData = loanResult;
            retModel.Token = model.Token;
            return retModel;
        }
        /// <summary>
        /// 借款人信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RLoanerInfo, string> LoanerInfo([FromBody]SLoanDetail model)
        {
            var retModel = new ReturnModel<RLoanerInfo, string>();
            #region 校验签名
            var signResult = VerifyBase.Sign<SLoanDetail>(model);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            var loanInfo = _loanInfoService.Find(model.LoanId);
            var loaner = new RLoanerInfo();
            if (loanInfo != null)
            {
                loaner.LoanId = model.LoanId;
                loaner.RepaymentResource = loanInfo.pro_loan_Sourcerepayment;
                loaner.LoanerIntro = loanInfo.pro_loan_introl;
                var loanAnnex = _uploadInfoService.GetUploadInfoAll(model.LoanId, DataDictionary.filmsdatatype_Project).Select(u =>
                    {
                        var annexM = new RLoanAnnex
                        {
                            AnnexId = u.Id,
                            AnnexName = u.file_Name,
                            AnnexTime =u.file_CreateTime == null ? "" : ((DateTime)u.file_CreateTime).ToString("yyyy-MM-dd"),
                            AnnexURl = FastDFSHelper.GetApiImgPath(u.file_Path)
                        };
                        return annexM;
                    });
                var loanAnnexes = loanAnnex as RLoanAnnex[] ?? loanAnnex.ToArray();
                if (loanAnnexes.Any())
                {
                    loaner.Annexes = loanAnnexes.ToList();
                }
            }
            retModel.Message = "查询成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.ReturnData = loaner;
            retModel.Token = model.Token;
            return retModel;
            #endregion
        }
        /// <summary>
        /// 标的投资人
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<ReturnPageData<RLoanInvester>, string> LoanInvester([FromBody]SLoanInvester model)
        {
            var retModel = new ReturnModel<ReturnPageData<RLoanInvester>, string>();
            #region 校验签名
            var signResult = VerifyBase.Sign<SLoanDetail>(model);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            #endregion
            var lInveter=new List<RLoanInvester>();
            var invester = _investInfoService.LoanInvesterPage(model.LoanId, model.Page, model.PageSize);
            if (invester.Items.Any())
            {
                foreach (var invest in invester.Items)
                {
                    var accoutInfo = _accountInfoService.GetAccountInfoByUserId(invest.pro_invest_emp.Value);
                    var info = new RLoanInvester();
                    if (accoutInfo.act_legal_name==null)
                    {
                        info.Invester = "企业用户";
                    }
                    else
                    {
                        info.Invester = accoutInfo.act_legal_name.Substring(0, 1) + "***";
                    }
                    info.InvestMoney = invest.pro_invest_money.Value;
                    info.InvestTime = invest.pro_invest_date.Value;
                    info.InvestTimeString = invest.pro_invest_date.Value.ToString("yyyy-MM-dd HH:mm:ss");
                    info.InvestState = "投资成功";
                    lInveter.Add(info);
                }
            }

            var rePage = new ReturnPageData<RLoanInvester>
            {
                PagingData = lInveter,
                Total = invester.TotalNum,
                TotalPageCount = invester.TotalPageCount
            };
            retModel.Message = "查询成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.ReturnData = rePage;
            retModel.Token = model.Token;
            return retModel;
        }
        /// <summary>
        /// 还款计划
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RRepaymentPlan, string> LoaneRepaymentPlan([FromBody]SLoanDetail model)
        {
            var retModel = new ReturnModel<RRepaymentPlan, string>();
            #region 校验签名
            var signResult = VerifyBase.Sign<SLoanDetail>(model);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            #endregion
            var result = new RRepaymentPlan
            {
                LoanId = model.LoanId,
                Repaymented = 0,
                Repaymenting = 0,
                RepaymentedMoney = 0,
                RepaymentingMoney = 0
            };
            var loanInfo = _loanInfoService.Find(model.LoanId);
            if (loanInfo.pro_loan_state == DataDictionary.projectstate_FullScalePending || loanInfo.pro_loan_state == DataDictionary.projectstate_Tender || loanInfo.pro_loan_state == DataDictionary.projectstate_StayTransfer)
            {
                retModel.Message = "查询成功";
                retModel.ReturnCode = (int)ReturnCode.Success;
                retModel.ReturnData = result;
                retModel.Token = model.Token;
                return retModel;
            }
            else
            {
                var loanPlan = _loanPlanService.GetLoanPlansByCondition(model.LoanId);
                if (loanPlan.Any())
                {
                    foreach (var loan in loanPlan)
                    {
                        var plan = new RRepaymentPlanInfo();
                        plan.Time = loan.pro_loan_period.Value;
                        plan.Principal = loan.pro_pay_money.Value;
                        plan.Interest = loan.pro_pay_rate.Value;
                        plan.RepaymentTime = loan.pro_pay_date.Value.ToString("yyyy-MM-dd");
                        plan.AllMoney = loan.pro_pay_total.Value;
                        if (loan.pro_is_clear)
                        {
                            plan.RepaymenyState = "已结清";
                        }
                        else if (loan.pro_pay_type == DataDictionary.RepaymentType_PlatformDaihuan)
                        {
                            plan.RepaymenyState = "平台代还";
                        }
                        else
                        {
                            plan.RepaymenyState = "待还款";
                        }
                        result.RepaymentPlanInfos.Add(plan);
                    }
                    var repaymented = loanPlan.Where(p => p.pro_is_clear || p.pro_pay_type == DataDictionary.RepaymentType_PlatformDaihuan).Select(p => p.pro_collect_total);
                    result.Repaymented = repaymented.Count();
                    result.Repaymenting = loanPlan.Count() - result.Repaymented;
                    result.RepaymentedMoney = repaymented.Sum().Value;
                    result.RepaymentingMoney = loanPlan.Sum(p => p.pro_pay_total).Value - result.RepaymentedMoney;
                }

                retModel.Message = "查询成功";
                retModel.ReturnCode = (int)ReturnCode.Success;
                retModel.ReturnData = result;
                retModel.Token = model.Token;
                return retModel;
            }
        }

        /// <summary>
        /// 微信端标的列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<ReturnPageData<RLoanAbstractLable>, string> WechatRecommand([FromBody]BasePageModel model)
        {
            var retModel = new ReturnModel<ReturnPageData<RLoanAbstractLable>, string>();
            #region 校验签名
            var signResult = VerifyBase.Sign<BasePageModel>(model);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            #endregion
            var listLoan = new List<PRO_loan_info>();

            #region 获取1月标1个
            var oneMonth = _loanInfoService.WechatRecommand(1);
            if (oneMonth != null)
                listLoan.Add(oneMonth);
            #endregion

            #region 获取3个月标1个
            var threeMonth = _loanInfoService.WechatRecommand(3);
            if (threeMonth != null)
                listLoan.Add(threeMonth);
            #endregion

            #region 获取6个月标1个
            var sixMonth = _loanInfoService.WechatRecommand(6);
            if (sixMonth != null)
                listLoan.Add(sixMonth);
            #endregion

            #region 获取12个月标1个
            var twelMonth = _loanInfoService.WechatRecommand(12);
            if (twelMonth != null)
                listLoan.Add(twelMonth);
            #endregion

            var loanList=new List<RLoanAbstractLable>();
            if (listLoan.Any())
            {
                loanList.AddRange(listLoan.Select(GetLoanAbstractLable));
            }
            var rePage = new ReturnPageData<RLoanAbstractLable>
            {
                PagingData = loanList,
                Total = listLoan.Count,
                TotalPageCount = 1
            };
            retModel.Message = "查询成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.ReturnData = rePage;
            retModel.Token = model.Token;
            return retModel;
        }

        /// <summary>
        /// 像用户账户转账(后台使用)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<string, string> RedEnvelopes([FromBody] SRedEnvelopeInfo model)
        {
            var retModel = new ReturnModel<string, string>();
            #region 校验签名
            var signResult = VerifyBase.Sign<BasePageModel>(model);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            #endregion

            //获取商户账户信息
            var envelopesResult = _redService.TransferRed(model);
            switch (envelopesResult)
            {
                case RedEnvelopesResult.Success:
                    retModel.Message = "成功";
                    retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                    retModel.ReturnData = "";
                    retModel.Token = model.Token;
                    return retModel;
                case RedEnvelopesResult.TransferRedError:
                    retModel.Message = "失败";
                    retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                    retModel.ReturnData = "红包转让错误";
                    retModel.Token = model.Token;
                    return retModel;
                case RedEnvelopesResult.InsufficientBalance:
                    retModel.Message = "失败";
                    retModel.ReturnCode = (int) ReturnCode.SignatureFailure;
                    retModel.ReturnData = "红包实时发放错误";
                    retModel.Token = model.Token;
                    return retModel;
                default:
                    retModel.Message = "失败";
                    retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                    retModel.ReturnData = "";
                    retModel.Token = model.Token;
                    return retModel;
            }
        }

        #region 四月新项目详情页
        [HttpPost]
        public ReturnModel<RLoanProjectDetails, string> LoanProjectDetail([FromBody]SLoanDetail model)
        {
            model.ApiName = "标的详情";
            var retModel = new ReturnModel<RLoanProjectDetails, string>();
            #region 校验签名
            var signResult = VerifyBase.Sign<SLoanDetail>(model);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            #endregion
            var loanInfo = _loanInfoService.Find(model.LoanId);
            if (loanInfo == null)
            {
                retModel.Message = "标的信息不存在";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            var result = GetProjectDetails(loanInfo);
            retModel.Message = "成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.ReturnData = result;
            retModel.Token = model.Token;
            return retModel;
        }
        /// <summary>
        /// 标的跟踪列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RTrackingDetails, string> LoanTracking([FromBody]SLoanDetail model)
        {
            model.ApiName = "标的详情";
            var retModel = new ReturnModel<RTrackingDetails, string>();
            #region 校验签名
            var signResult = VerifyBase.Sign<SLoanDetail>(model);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            #endregion

            var result = GetTrackingDetails(model.LoanId);
            retModel.Message = "成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.ReturnData = result;
            retModel.Token = model.Token;
            return retModel;
        }
        #endregion


        #region 推荐标的
        /// <summary>
        /// 推荐标的列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<ReturnPageData<RLoanAbstract>, string> RecommandLoan([FromBody]BaseSubmitModel model)
        {
            var retModel = new ReturnModel<ReturnPageData<RLoanAbstract>, string>();
            #region 校验签名
            var signResult = VerifyBase.SignAndToken<BaseSubmitModel>(model,out CST_user_info userInfo);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            #endregion
            var investLoanList =new List<RLoanAbstract>();
            var infos = new List<PRO_loan_info>();

            bool canInvestNewLoan = false;

            if (userInfo != null)//判断用户是否可以投资新手标
            {
                int investCount = _investInfoService.CountInvest(userInfo.Id);
                canInvestNewLoan = investCount <= 0;
            }
            if (canInvestNewLoan)//推荐新手标
            {
                var loans = _loanInfoService.RecommandNewHandLoan(2);
                if (loans != null && loans.Count > 0)
                    infos.AddRange(loans);
            }
            if (!canInvestNewLoan || infos.Count <= 2)
            {
                var loans = _loanInfoService.RecommandCommonLoan(2);
                if (loans != null && loans.Count > 0)
                    infos.AddRange(loans);
            }
            infos = infos.Take(2).ToList();
            if (infos.Any())
            {
                investLoanList.AddRange(infos.Select(GetLoanAbstract));
            }
            var rePage = new ReturnPageData<RLoanAbstract>
            {
                PagingData = investLoanList,
                Total = investLoanList.Count,
                TotalPageCount = 1
            };
            retModel.Message = "查询成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.ReturnData = rePage;
            retModel.Token = model.Token;
            return retModel;
        }
        #endregion

        #region 绑定数据
        private RLoanAbstract GetLoanAbstract(PRO_loan_info model)
        {
            var info = new RLoanAbstract
            {
                LoanId = model.Id,
                LoanName = model.pro_loan_use,
                YearRate = model.pro_loan_rate.Value,
                Period = model.pro_loan_period.Value,
                RepaymentType = InterestProvider.LoadInterestProviderGetByFriendlyName(model.pro_pay_type),
                RepeymentTypeCode = model.pro_pay_type,
                LoanSpeed = model.pro_loan_speed ?? 0.00m,
                SurplusMoney = model.pro_surplus_money.Value,
                IsRedPackage = !model.pro_enable_red,
                HikeRate = model.pro_loan_ratehike,
                OriginalRate = model.pro_loan_originalrate
            };
            info.IsTransfer = info.Period > 1;
            info.LoanStateId = model.pro_loan_state.Value;
            info.LoanState = GetLoanState(model.pro_loan_state.Value);
            info.LeastMoney = model.pro_min_invest_money ?? 0m;
            info.MaxMoney = model.pro_max_invest_money ?? 0m;
            return info;
        }
        private RLoanAbstractLable GetLoanAbstractLable(PRO_loan_info model)
        {
            var info = new RLoanAbstractLable
            {
                LoanId = model.Id,
                LoanName = model.pro_loan_use,
                YearRate = model.pro_loan_rate.Value,
                Period = model.pro_loan_period.Value,
                RepaymentType = InterestProvider.LoadInterestProviderGetByFriendlyName(model.pro_pay_type),
                RepeymentTypeCode = model.pro_pay_type,
                LoanSpeed = model.pro_loan_speed ?? 0.00m,
                SurplusMoney = model.pro_surplus_money.Value,
                IsRedPackage = !model.pro_enable_red,
                HikeRate = model.pro_loan_ratehike,
                OriginalRate = model.pro_loan_originalrate
            };
            info.IsTransfer = info.Period > 1;
            info.LoanStateId = model.pro_loan_state.Value;
            info.LoanState = GetLoanState(model.pro_loan_state.Value);
            info.LeastMoney = model.pro_min_invest_money ?? 0m;
            info.MaxMoney = model.pro_max_invest_money ?? 0m;
            info.PeriodDay = info.Period * 30;
            var lables = _loanInfoService.GettbloanlabelList(model.Id);
            if (lables != null && lables.Any())
            {
                var lableResult = lables.Aggregate("", (current, lableInfos) => current + (lableInfos.Title + ","));
                info.Lables = lableResult;
                var lableIcon =
                    lables.Aggregate("", (current, lableInfos) => current + (FastDFSHelper.GetApiImgPath(lableInfos.Icon) + ","));
                info.LableIcons = lableIcon;
            }
            return info;
        }
        /// <summary>
        /// 获取标记状态
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private string GetLoanState(int code)
        {
            if (code == DataDictionary.projectstate_Tender)
            {
                return "立即投资";
            }
            else if (code == DataDictionary.projectstate_FullScalePending)
            {
                return "满标待审";
            }
            else if (code == DataDictionary.projectstate_StayTransfer)
            {
                return "待划转";
            }
            else if (code == DataDictionary.projectstate_Repayment)
            {
                return "还款中";
            }
            else if (code == DataDictionary.projectstate_Settled)
            {
                return "已结清";
            }
            else if (code == DataDictionary.projectstate_Overdue)
            {
                return "还款中";
            }
            return "";
        }

        private RLoanDetail GetLoanDetail(PRO_loan_info model)
        {
            var info = new RLoanDetail
            {
                LoanId = model.Id,
                LoanName = model.pro_loan_use,
                YearRate = model.pro_loan_rate.Value,
                Period = model.pro_loan_period.Value,
                RepaymentType = InterestProvider.LoadInterestProviderGetByFriendlyName(model.pro_pay_type),
                RepeymentTypeCode = model.pro_pay_type,
                LoanSpeed = model.pro_loan_speed ?? 0.00m,
                SurplusMoney = model.pro_surplus_money.Value,
                IsRedPackage = !model.pro_enable_red,
                HikeRate = model.pro_loan_ratehike,
                OriginalRate = model.pro_loan_originalrate
            };
            info.IsTransfer = info.Period > 1;
            info.LoanStateId = model.pro_loan_state.Value;
            info.LoanState = GetLoanState(model.pro_loan_state.Value);
            info.LeastMoney = model.pro_min_invest_money ?? 0m;
            info.MaxMoney = model.pro_max_invest_money ?? 0m;
            info.LoanMoney = model.pro_loan_money.Value;
            info.MaxInvestMoney = model.pro_surplus_money.Value > model.pro_max_invest_money.Value ? model.pro_max_invest_money.Value : model.pro_surplus_money.Value;
            info.MinInvestMoney = model.pro_min_invest_money.Value;
            info.ProductType = _loanInfoService.GetProductInfo(model.pro_prod_typeId.Value).pdo_product_name;
            var lables = _loanInfoService.GettbloanlabelList(model.Id);
            if (lables != null && lables.Any())
            {
                var lableResult = lables.Aggregate("", (current, lableInfos) => current + (lableInfos.Title + ","));
                info.Lables = lableResult;
                var lableIcon =
                    lables.Aggregate("", (current, lableInfos) => current + (FastDFSHelper.GetApiImgPath(lableInfos.Icon) + ","));
                info.LableIcons = lableIcon;
            }
            info.LoanType = GetLoanType(model);
            info.PublishDate = model.pro_public_date?.AddDays(20).ToString("yyyy-MM-dd HH:mm:ss");
            info.IsCompany = (model.pro_add_emp_type == null || model.pro_add_emp != 9).ToString().ToLower();

            //标的风险类型 by gaochao 2018 0228
            switch (model.pro_risk_type.GetValueOrDefault())
            {
                case 1: info.LoanRiskType = "保守型"; break;
                case 2: info.LoanRiskType = "稳健型"; break;
                case 3: info.LoanRiskType = "激进型"; break;
                default: info.LoanRiskType = "未限制"; break;
            }
            return info;
        }

        private int GetLoanType(PRO_loan_info model)
        {
            if (model.pro_is_new)
            {
                return (int)LoanType.NewHand;
            }
            if (model.pro_loan_period >= 1 && model.pro_loan_period <= 6)
            {
                return (int)LoanType.Recommand;
            }
            if (model.pro_loan_period > 6 && model.pro_loan_period <= 12)
            {
                return (int)LoanType.Recommand;
            }
            return -1;
        }
        
        private RLoanProjectDetails GetProjectDetails(PRO_loan_info model)
        {
            var loaner = new RLoanProjectDetails();
            //项目id
            loaner.LoanId = model.Id.ToString();
            loaner.IsCo = (model.pro_add_emp_type == null||model.pro_add_emp_type != 9).ToString().ToLower();
            #region 融资信息

            var completeUser = _userInfoService.GetCompleteUserAccount(model.pro_add_emp);
            var loanProduct = _loanInfoService.GetLoanProduct(model.pro_prod_typeId);
            var financingInfo = new FinancingInformation
            {
                LoanIntro = model.pro_loan_introl,
                LoanMoney = model.pro_loan_money.ToString(),
                LoanName = model.pro_loan_no,
                LoanPeriod = model.pro_loan_period.ToString(),
                LoanRepayment = InterestProvider.LoadInterestProviderGetByFriendlyName(model.pro_pay_type),
                LoanSource = CheckIsNull(model.pro_pay_source) ? "未知" : _dataDictionaryService.Find(model.pro_pay_source.Value).sys_data_name,
                LoanType = loanProduct.pdo_product_name,
                LoanerCharacter = (model.pro_add_emp_type == null || model.pro_add_emp_type != 9) ? "法人" : "自然人",
                LoanUse = !String.IsNullOrEmpty(model.pro_loan_purpose) ? model.pro_loan_purpose : "未知",
                SafetyPrecautions = CheckIsNull(model.pro_guarantee_measures) ? "未知" : _dataDictionaryService.Find(model.pro_guarantee_measures.Value).sys_data_name
            };
            if (completeUser.LoanUserInfo.CompanyId != null)
            {
                financingInfo.LoanerName = completeUser.CstCompanyInfo.CompanyName.Substring(0, 1) + "****";
            }
            else
            {
                financingInfo.LoanerName = completeUser.AccountInfo.act_legal_name.Substring(0, 1) + "****";
            }


            loaner.Info = financingInfo;
            #endregion
            #region 还款保障

            var guarantee = new RepaymentGuarantee
            {
                Guarantee = model.pro_pay_measures ?? ""
            };
            loaner.Guar = guarantee;
            #endregion
            #region 借款人资料
            //企业
            if ((model.pro_add_emp_type == null || model.pro_add_emp_type != 9))
            {
                var coInfo = new CoInfomation
                {
                    Name = completeUser.CstCompanyInfo.CompanyName.Substring(0, 1) + "****",
                    Corporate = CheckString(completeUser.LoanUserInfo.cst_contract_person) ? "未知" : completeUser.LoanUserInfo.cst_contract_person.Substring(0, 1) + "****",
                    RegisteredCapital = CheckString(completeUser.CstCompanyInfo.RegisteredCapital.ToString()) ? "未知" : completeUser.CstCompanyInfo.RegisteredCapital.ToString(),
                    PaidCapital = CheckString(completeUser.CstCompanyInfo.PaidCapital.ToString()) ? "未知" : completeUser.CstCompanyInfo.PaidCapital.ToString(),
                    Industry = CheckIsNull(completeUser.CstCompanyInfo.Industry) ? "未知" : _dataDictionaryService.Find(completeUser.CstCompanyInfo.Industry.Value)
                        .sys_data_name,
                    BusinessLife = completeUser.CstCompanyInfo.BusinessLife == null ? "未知" : completeUser.CstCompanyInfo.BusinessLife + "年",
                    AnnualIncome = completeUser.CstCompanyInfo.YearIncome.ToString()
                };
                loaner.Coer = coInfo;

            } //个人
            else
            {
                var personal = new LoanerInfomation
                {
                    Name = completeUser.AccountInfo.act_legal_name.Substring(0, 1) + "****",
                    IdCard = completeUser.AccountInfo.act_user_card.Substring(0, 3) + "********" + completeUser.AccountInfo.act_user_card.Substring(16, 2),
                    Age = CommonHelper.GetAgeByDate(completeUser.RealnameProves.cst_user_birthdate.Value).ToString(),
                    Sex = completeUser.RealnameProves.cst_user_sex ? "男" : "女",
                    Education = CheckIsNull(completeUser.LoanUserInfo.cst_user_education) ? "未知" : _dataDictionaryService.Find(completeUser.LoanUserInfo.cst_user_education.Value)
                        .sys_data_name,

                    Income = completeUser.LoanUserInfo.cst_month_money != null ? completeUser.LoanUserInfo.cst_month_money.ToString() : "未知"
                };
                if (completeUser.UserCompanies == null)
                {
                    personal.Industry = "未知";
                    personal.WorkYear = "未知";
                }
                else
                {
                    personal.Industry = CheckIsNull(completeUser.UserCompanies.cst_company_type)
                        ? "未知"
                        : _dataDictionaryService.Find(completeUser.UserCompanies.cst_company_type.Value)
                            .sys_data_name;
                    personal.WorkYear = CheckIsNull(completeUser.UserCompanies.cst_work_year)
                        ? "未知"
                        : _dataDictionaryService.Find(completeUser.UserCompanies.cst_work_year.Value)
                            .sys_data_name;
                }
                loaner.Personal = personal;
            }

            #endregion

            #region 借款人历史借款信息
            
            var historyLoan = _loanInfoService.GetLoanUserInfo(model.Id);
            var history = new LoanerHistory();
            if (historyLoan == null)
            {
                history.Liabilities = "无负债";
                history.HistoryLoan = "0";
                history.HistoryOverDue = "0";
                history.UnclearLoan = "0";
                history.UnclearMoney = "0";
                history.OtherPlatform = "0";
            }
            else
            {
                history.Liabilities = CheckIsNull(completeUser.LoanUserInfo.cst_user_debt)
                    ? "无负债"
                    : _dataDictionaryService.Find(completeUser.LoanUserInfo.cst_user_debt.Value).sys_data_name;
                history.HistoryLoan = CheckIsNull(historyLoan.cst_loan_count) ? "0" : historyLoan.cst_loan_count.ToString();
                history.HistoryOverDue = CheckIsNull(historyLoan.cst_loan_overduecount) ? "0" : historyLoan.cst_loan_overduecount.ToString();
                history.UnclearLoan = historyLoan.cst_loan_repaycount.ToString();
                history.UnclearLoan = CheckIsNull(historyLoan.cst_loan_repaycount) ? "0" : historyLoan.cst_loan_repaycount.ToString();
                history.UnclearMoney = historyLoan.cst_loan_repaymoney == null ? "0" : historyLoan.cst_loan_repaymoney.ToString();
                history.OtherPlatform = historyLoan.cst_user_thriddebt == null ? "0" : historyLoan.cst_user_thriddebt.ToString();
            }

            loaner.History = history;
            #endregion

            #region 风险评估

            var riskInfo = new LoanRiskResult
            {
                RiskResult = model.pro_risk_result ?? ""
            };
            loaner.RiskInfo = riskInfo;

            #endregion
            var review = new ReviewProject();
            if (historyLoan != null && !string.IsNullOrEmpty(historyLoan.cst_auth_item))
            {
                var cert = historyLoan.cst_auth_item.Split(',');
                foreach (var ce in cert)
                {
                    if (!string.IsNullOrEmpty(ce.Trim()))
                        review.Reviewed.Add(_dataDictionaryService.Find(Convert.ToInt32(ce)).sys_data_name);
                }
            }
            loaner.Check = review;


            #region 项目资料
            var loanAnnex = _uploadInfoService.GetUploadInfoAll
                (model.Id, DataDictionary.filmsdatatype_Project).Select(u =>
                {
                    var annexM = new RLoanAnnex
                    {
                        AnnexId = u.Id,
                        AnnexName = u.file_Name,
                        AnnexTime =
                            u.file_CreateTime?.ToString("yyyy-MM-dd") ?? "",
                        AnnexURl = FastDFSHelper.GetApiImgPath(u.file_Path)
                    };
                    return annexM;
                });
            var loanAnnexes = loanAnnex as RLoanAnnex[] ?? loanAnnex.ToArray();
            if (loanAnnexes.Any())
            {
                loaner.Annexes = loanAnnexes.ToList();
            }
            #endregion

            return loaner;
        }

        private bool CheckIsNull(int? value)
        {
            if (value == null || value == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CheckString(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private RTrackingDetails GetTrackingDetails(int loanId)
        {
            var repayFollows = _loanInfoService.GetRepayFollows(loanId);
            var trackDetails = new RTrackingDetails();
            if (repayFollows.Any())
            {
                var followGroup = repayFollows.GroupBy(p => p.pro_loan_followdate);
                foreach (var dateFollw in followGroup)
                {
                    var followList = repayFollows.Where(p => p.pro_loan_followdate == dateFollw.Key);
                    var trackingInfo = new RepaymentTracking
                    {
                        TrackingDate = dateFollw.Key.ToString("yyyy.MM.dd")
                    };
                    followList = followList.OrderBy(p => p.pro_loan_followtype);
                    foreach (var proLoanRepayfollow in followList)
                    {
                        var track = new TrackInfo
                        {
                            Order = proLoanRepayfollow.pro_loan_followtype.ToString(),
                            TrackTitle = CommonHelper.GetEnumDescription(
                                             (RepayFollowType)proLoanRepayfollow.pro_loan_followtype.GetValueOrDefault()) ?? "未知",
                            TrackDesc = proLoanRepayfollow.pro_loan_followInfo
                        };
                        trackingInfo.TrackInfos.Add(track);
                    }
                    trackDetails.Trackings.Add(trackingInfo);
                }
            }
            trackDetails.LoanId = loanId.ToString();
            trackDetails.Count = trackDetails.Trackings.Count.ToString();
            return trackDetails;
        }

        #endregion

        /// <summary>
        /// 新增、修改项目
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<bool, string> SubmitCreditProject([FromBody] SSubmitCreditProject model) {
            var retModel = new ReturnModel<bool, string>();

            #region 校验签名
            var signResult = VerifyBase.Sign<SSubmitCreditProject>(model);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = false;
                return retModel;
            }
            #endregion
            var loanInfo = _loanInfoService.Find(model.LoanId);
            if (loanInfo != null) {
                //拼接请求参数
                var svcBody = new SBHSubmitCreditProjectBody {  projectType = ProjectType.Creditor };
                svcBody.projectCode = loanInfo.Id.ToString();
                svcBody.projectName = loanInfo.pro_loan_no;
                svcBody.repayType = LoanProvider.GetInterest(loanInfo.pro_pay_type);
                svcBody.loanAmount = loanInfo.pro_loan_money.Value.ToString("0.00");
                svcBody.loanType = LoanProvider.GetLoanType(loanInfo.pro_prod_typeId.GetValueOrDefault(), loanInfo.pro_loan_guar_company.GetValueOrDefault()); ;
                svcBody.transferFlag = "1";
                svcBody.interestPayMode = "2";
                svcBody.annualPeriod = "1";
                svcBody.APR = (loanInfo.pro_loan_rate.Value / 100).ToString();
                svcBody.startTimeOfRaising = loanInfo.pro_invest_startTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                svcBody.endTimeOfRaising = loanInfo.pro_invest_endDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss");
                if (loanInfo.pro_invest_startTime.Value <= DateTime.Now)
                    svcBody.startTimeOfRaising =DateTime.Now.AddMinutes(3).ToString("yyyy-MM-dd HH:mm:ss");
                if(loanInfo.pro_invest_endDate.GetValueOrDefault() <= loanInfo.pro_invest_startTime.Value)
                    svcBody.endTimeOfRaising = Convert.ToDateTime(svcBody.startTimeOfRaising).AddDays(7).ToString("yyyy-MM-dd HH:mm:ss");
                svcBody.valueDate = Convert.ToDateTime(svcBody.endTimeOfRaising).ToString("yyyyMMdd");

                //判断还款类型
                if (svcBody.repayType.Equals("1") || loanInfo.pro_period_type == DataDictionary.deadlinetype_Day)
                {
                    //一次还本息
                    svcBody.repayFrequency = "1";
                    if (loanInfo.pro_period_type == DataDictionary.deadlinetype_Day) {
                        svcBody.interestPayMode = "1";
                        svcBody.projectInterestPeriod = loanInfo.pro_loan_period.ToString();
                    }
                    else {
                        svcBody.projectInterestPeriod = (loanInfo.pro_loan_period * 30).ToString();
                    }
                    svcBody.principalRepayRate = "1";
                    svcBody.numberOfPayments = "1";
                }
                else
                {
                    //按月还
                    svcBody.interestType = "1";
                    svcBody.repayFrequency = "2";
                    svcBody.numberOfPayments = loanInfo.pro_loan_period.GetValueOrDefault().ToString();
                    svcBody.repayTypeComment = $"于放款后起息；按月计息；还款期数为{svcBody.numberOfPayments}期";
                }

                //测试用
                //if (loanInfo.pro_period_type == DataDictionary.deadlinetype_Day)
                //{
                //    svcBody.repayType = "2";
                //    svcBody.interestType = "1";
                //    svcBody.repayFrequency = "9";
                //    svcBody.projectInterestPeriod = "2";
                //    svcBody.numberOfPayments = "2";
                //    svcBody.repayTypeComment = $"于放款后起息；按天计息；还款期数为2期";
                //    svcBody.principalRepayRate = "0,1";
                //    svcBody.interestPayMode = "1";
                //}

                //新手标不可债转
                if (loanInfo.pro_prod_typeId.GetValueOrDefault().Equals(4))
                    svcBody.transferFlag = "0";

                //担保人信息
                if (!loanInfo.pro_loan_guar_company.GetValueOrDefault().Equals(0))
                {
                    var guarCompanyInfo = _companyInfoService.Find(loanInfo.pro_loan_guar_company.Value);
                    var guarAccountInfo = _accountInfoService.GetAccountInfoByUserId(userId: guarCompanyInfo.UserId);
                    var guarChargeAccountInfo = _companyInfoService.GetChargeAccount(companyId: loanInfo.pro_loan_guar_company.Value);
                    if (guarCompanyInfo != null && guarChargeAccountInfo != null) {
                        svcBody.guaranteeList = new List<SBHGuarantee> {
                            new SBHGuarantee{
                                guaranteeUserCode = guarAccountInfo.financing_platform_id,
                                guaranteeUserName = guarCompanyInfo.UserName,
                                guaranteeUserProperty ="2",
                                guaranteeUserIDType ="04",
                                guaranteeUserID =guarCompanyInfo.InstuCode,
                                guaranteeLRName =guarCompanyInfo.RealName,
                                guaranteeLRID = guarCompanyInfo.CorperationIdCard
                            }
                        };
                    }
                }

                //手续费
                svcBody.fundList = new List<SBHFund> {
                    new SBHFund{
                            collectDate =  loanInfo.pro_invest_endDate.Value.ToString("yyyy-MM-dd"),
                            commissionFeeMode  ="1",
                            commissionFee =(loanInfo.pro_this_guaranteefee.GetValueOrDefault() + loanInfo.pro_this_procedurefee.GetValueOrDefault()).ToString("0.00")
                        }
                };

                //借款人类型
                var userAccountInfo = _accountInfoService.GetAccountInfoByUserId(userId: loanInfo.pro_add_emp.Value);
                if (loanInfo.pro_add_emp_type.GetValueOrDefault().Equals(9)) {
                    //个人
                    svcBody.borrowerUserCode = userAccountInfo.financing_platform_id;
                    svcBody.borrowerUserName =userAccountInfo.act_user_name;
                    svcBody.borrowerUserProperty = "1";
                    svcBody.borrowerUserIDType = "01";
                    svcBody.borrowerUserID =userAccountInfo.act_user_card;
                    svcBody.borrowerPhone = userAccountInfo.act_user_phone;
                }
                else {
                    //企业
                    var userCompanyInfo = _companyInfoService.GetCompanyInfo(loanInfo.pro_add_emp.Value);
                    var chargeAccountInfo = _companyInfoService.GetChargeAccount(companyId: userCompanyInfo.Id);
                    svcBody.borrowerUserCode = userAccountInfo.financing_platform_id;
                    svcBody.borrowerUserName = userCompanyInfo.UserName;
                    svcBody.borrowerUserProperty = "2";
                    svcBody.borrowerUserIDType = string.IsNullOrEmpty(userCompanyInfo.BusiCode) ? "05":"04";
                    svcBody.borrowerUserID = userCompanyInfo.InstuCode;
                    svcBody.borrowerPhone = userAccountInfo.act_user_phone;
                    svcBody.borrowerLRName = userCompanyInfo.RealName;
                    svcBody.borrowerLRID = userCompanyInfo.CorperationIdCard;
                    svcBody.borrowerAccountNo = chargeAccountInfo.AccountNo;
                    svcBody.borrowerAccountName = chargeAccountInfo.AccountName;
                }
                svcBody.purpose = loanInfo.pro_loan_use;
                svcBody.collateralInformation =_loanInfoService.GetCollateralInformation(loanInfo.Id);

                var rBHBaseModel = _bhAccountService.SubmitCreditProject(new SBHSubmitCreditProject{SvcBody = svcBody});
                if (rBHBaseModel != null)
                {
                    if (rBHBaseModel.RspSvcHeader.returnCode == JSReturnCode.Success)
                    {
                        retModel.Message = "已发送至渤海银行处理";
                        retModel.ReturnCode = (int)ReturnCode.Success;
                        retModel.ReturnData = true;
                        return retModel;
                    }
                    else
                    {
                        retModel.Message = rBHBaseModel.RspSvcHeader.returnMsg;
                        retModel.ReturnCode = (int)ReturnCode.DataEorr;
                        retModel.ReturnData = false;
                        return retModel;
                    }
                }
            }
            retModel.Message = "发送至渤海银行失败";
            retModel.ReturnCode = (int)ReturnCode.DataEorr;
            retModel.ReturnData = false;
            return retModel;
        }


        /// <summary>
        /// 标的可投金额
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<decimal, string> LoanAvaliableMoney(SLoanDetail model)
        {
            var retModel = new ReturnModel<decimal, string>();
            #region 校验签名
            var signResult = VerifyBase.Sign<SLoanDetail>(model);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = 0.0m;
                retModel.Token = model.Token;
                return retModel;
            }
            #endregion

            var result = _loanInfoService.Find(model.LoanId).pro_surplus_money.Value;
            retModel.Message = "查询成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.ReturnData = result;
            retModel.Token = model.Token;
            return retModel;
        }
    }
}