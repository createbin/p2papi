using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using ZFCTAPI.Core;
using ZFCTAPI.Core.Enums;
using ZFCTAPI.Core.Provider;
using ZFCTAPI.Data.ApiModels.ReturnModels;
using ZFCTAPI.Data.ApiModels.SubmitModels;
using ZFCTAPI.Services.InvestInfo;
using ZFCTAPI.Services.LoanInfo;
using ZFCTAPI.WebApi.RequestAttribute;
using ZFCTAPI.WebApi.Validates;

namespace ZFCTAPI.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [RequestLog]
    public class MyLoanController : Controller
    {
        private readonly ILoanInfoService _loanInfoService;
        private readonly IInvesterPlanService _investerPlanService;
        private readonly ILoanPlanService _loanPlanService;

        public MyLoanController(ILoanInfoService loanInfoService,
            IInvesterPlanService investerPlanService,
            ILoanPlanService loanPlanService)
        {
            _loanInfoService = loanInfoService;
            _investerPlanService = investerPlanService;
            _loanPlanService = loanPlanService;
        }

        /// <summary>
        /// 投标中
        /// </summary>
        [HttpPost]
        public ReturnModel<ReturnPageData<RBiddingLoan>, string> MyBiddingLoan([FromBody]BasePageModel model)
        {
            var reModel = new ReturnModel<ReturnPageData<RBiddingLoan>, string>();

            #region 验签

            var userId = 0;
            var resultSign = VerifyBase.SignAndToken<BasePageModel>(model, out userId);
            if (!resultSign.Equals(ReturnCode.Success))
            {
                reModel.ReturnCode = (int)resultSign;
                reModel.Message = "签名验证失败";
                return reModel;
            }

            #endregion 验签

            try
            {
                var biddingState = new List<int> {
                    DataDictionary.projectstate_StaySend,
                    DataDictionary.projectstate_StayPlatformaudit,
                    DataDictionary.projectstate_Tender,
                    DataDictionary.projectstate_StayRelease
                };
                var pageData = _loanInfoService.GetPageByState(userId, biddingState, model.Page, model.PageSize);
                var rePage = new ReturnPageData<RBiddingLoan>
                {
                    PagingData = pageData.Items.Select(p=> {
                        var item = new RBiddingLoan()
                        {
                            LoanId = p.Id,
                            LoanName = p.pro_loan_use,
                            LoanMoney = p.pro_loan_money.Value,
                            LoanRate = p.pro_loan_rate.Value,
                            ApplyDate = p.pro_add_date.Value,
                            InvestMoney = (p.pro_loan_money - p.pro_surplus_money).GetValueOrDefault(),
                            LoanSpeed = p.pro_loan_speed.GetValueOrDefault()
                        };
                        item.LoanPeriodDesc = LoanProvider.GetLoanPeriod(p.pro_loan_period.Value, p.pro_period_type.Value);
                        item.LoanStateDesc = LoanProvider.GetLoanState(p.pro_loan_state, "招标中");
                        item.LoanInterest = _investerPlanService.getloanPlanRate(p.pro_loan_money.Value,p.pro_loan_rate.Value,p.pro_loan_period.Value, p.pro_loan_period.Value);
                        item.LoanServiceFee = p.pro_this_servicefee.GetValueOrDefault();
                        item.LoanRePayMoney = item.LoanInterest + item.LoanServiceFee * (p.pro_period_type == DataDictionary.deadlinetype_Month ? p.pro_loan_period.Value : 1);
                        return item;
                    }),
                    Total = pageData.TotalNum,
                    TotalPageCount = pageData.TotalPageCount
                };

                reModel.ReturnData = rePage;
                reModel.Message = "查询成功";
                reModel.ReturnCode = (int)ReturnCode.Success;
                reModel.Token = model.Token;
                return reModel;
            }
            catch
            {
                reModel.ReturnData = null;
                reModel.Message = "查询失败";
                reModel.ReturnCode = (int)ReturnCode.QueryEorr;
                reModel.Token = model.Token;
                return reModel;
            }
        }

        /// <summary>
        /// 满标中
        /// </summary>
        [HttpPost]
        public ReturnModel<ReturnPageData<RFullLoan>, string> MyFullLoan([FromBody]BasePageModel model)
        {
            var reModel = new ReturnModel<ReturnPageData<RFullLoan>, string>();

            #region 验签

            var userId = 0;
            var resultSign = VerifyBase.SignAndToken<BasePageModel>(model, out userId);
            if (!resultSign.Equals(ReturnCode.Success))
            {
                reModel.ReturnCode = (int)resultSign;
                reModel.Message = "签名验证失败";
                return reModel;
            }

            #endregion 验签

            try
            {
                var fullState = new List<int> {
                    DataDictionary.auditlink_Fullstandardaudit,
                    DataDictionary.projectstate_FullScalePending,
                    DataDictionary.projectstate_StayTransfer,
                    DataDictionary.bank_state_using
                };
                var pageData = _loanInfoService.GetPageByState(userId, fullState, model.Page, model.PageSize);
                var rePage = new ReturnPageData<RFullLoan>
                {
                    PagingData = pageData.Items.Select(p => {
                        var item = new RFullLoan()
                        {
                            LoanId = p.Id,
                            LoanName = p.pro_loan_use,
                            LoanMoney = p.pro_loan_money.Value,
                            LoanRate = p.pro_loan_rate.Value,
                            ApplyDate = p.pro_add_date.Value,
                            InvestMoney = (p.pro_loan_money - p.pro_surplus_money).GetValueOrDefault(),
                        };
                        item.FullDate = p.pro_full_date == null ? "" : p.pro_full_date.Value.ToString("yyyy-MM-dd");
                        item.LoanPeriodDesc = LoanProvider.GetLoanPeriod(p.pro_loan_period.Value, p.pro_period_type.Value);
                        item.LoanStateDesc = LoanProvider.GetLoanState(p.pro_loan_state, "满标待审");
                        item.LoanInterest = _investerPlanService.getloanPlanRate(p.pro_loan_money.Value, p.pro_loan_rate.Value, p.pro_loan_period.Value, p.pro_loan_period.Value);
                        item.LoanServiceFee = p.pro_this_servicefee.GetValueOrDefault() * (p.pro_period_type == DataDictionary.deadlinetype_Month ? p.pro_loan_period.Value : 1);
                        return item;
                    }),
                    Total = pageData.TotalNum,
                    TotalPageCount = pageData.TotalPageCount
                };

                reModel.ReturnData = rePage;
                reModel.Message = "查询成功";
                reModel.ReturnCode = (int)ReturnCode.Success;
                reModel.Token = model.Token;
                return reModel;
            }
            catch
            {
                reModel.ReturnData = null;
                reModel.Message = "查询失败";
                reModel.ReturnCode = (int)ReturnCode.QueryEorr;
                reModel.Token = model.Token;
                return reModel;
            }
        }

        /// <summary>
        /// 还款中
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<ReturnPageData<RRepaymentLoan>, string> MyRepaymentLoan([FromBody]BasePageModel model)
        {
            var reModel = new ReturnModel<ReturnPageData<RRepaymentLoan>, string>();
            var userId = 0;

            #region 验签

            var resultSign = VerifyBase.SignAndToken<BasePageModel>(model, out userId);
            if (!resultSign.Equals(ReturnCode.Success))
            {
                reModel.ReturnCode = (int)resultSign;
                reModel.Message = "签名验证失败";
                return reModel;
            }

            #endregion 验签

            try
            {
                var repayState = new List<int> {
                    DataDictionary.projectstate_Overdue,
                    DataDictionary.projectstate_Repayment
                };
                var pageData = _loanInfoService.GetPageByState(userId, repayState, model.Page, model.PageSize);
                var rePage = new ReturnPageData<RRepaymentLoan>
                {
                    PagingData = pageData.Items.Select(p => {
                        var item = new RRepaymentLoan()
                        {
                            LoanId = p.Id,
                            LoanName = p.pro_loan_use,
                            LoanMoney = p.pro_loan_money.Value,
                            LoanRate = p.pro_loan_rate.Value,
                            ApplyDate = p.pro_add_date.Value
                        };
                        item.LoanPeriodDesc = LoanProvider.GetLoanPeriod(p.pro_loan_period.Value, p.pro_period_type.Value);
                        item.LoanStateDesc = LoanProvider.GetLoanState(p.pro_loan_state, "还款中");
                        item.LoanInterest = _investerPlanService.getloanPlanRate(p.pro_loan_money.Value, p.pro_loan_rate.Value, p.pro_loan_period.Value, p.pro_loan_period.Value);
                        item.LoanServiceFee = p.pro_this_servicefee.GetValueOrDefault() * (p.pro_period_type == DataDictionary.deadlinetype_Month ? p.pro_loan_period.Value : 1);
                        var loanPlans = _loanPlanService.GetLoanPlansByCondition(p.Id).Where(l => l.pro_is_clear == false);
                        item.NoRepayMoney = loanPlans.FirstOrDefault().pro_pay_total.Value;
                        item.NoRepayPeriod = loanPlans.FirstOrDefault().pro_loan_period.Value;
                        item.NoRepayDate = loanPlans.FirstOrDefault().pro_pay_date.Value;
                        item.LoanOverRate = loanPlans.Sum(l => {
                            return Math.Round(loanPlans.Where(o=>o.pro_pay_date > DateTime.Now.Date).Sum(o =>
                            {
                                return _loanPlanService.CalculationOverRate(new Data.LoanInfo.CalculationLoanPlanOverRateRequest
                                {
                                    SurplusOverRate = o.pro_sett_over_rate.GetValueOrDefault(),
                                    CollectDate = o.pro_collect_date,
                                    PayDate = o.pro_pay_date,
                                    PayMoney = o.pro_pay_money.Value,
                                    PayRate = o.pro_pay_rate.Value
                                });
                            }), 2);
                        });
                        return item;
                    }),
                    Total = pageData.TotalNum,
                    TotalPageCount = pageData.TotalPageCount
                };

                reModel.ReturnData = rePage;
                reModel.Message = "查询成功";
                reModel.ReturnCode = (int)ReturnCode.Success;
                reModel.Token = model.Token;
                return reModel;
            }
            catch
            {
                reModel.ReturnData = null;
                reModel.Message = "查询失败";
                reModel.ReturnCode = (int)ReturnCode.QueryEorr;
                reModel.Token = model.Token;
                return reModel;
            }
        }

        /// <summary>
        /// 已结清
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<ReturnPageData<RClearedLoan>, string> MyClearedLoan([FromBody]BasePageModel model)
        {
            var reModel = new ReturnModel<ReturnPageData<RClearedLoan>, string>();

            #region 验签

            var userId = 0;
            var resultSign = VerifyBase.SignAndToken<BasePageModel>(model, out userId);
            if (!resultSign.Equals(ReturnCode.Success))
            {
                reModel.ReturnCode = (int)resultSign;
                reModel.Message = "签名验证失败";
                return reModel;
            }

            #endregion 验签

            try
            {
                var biddingState = new List<int> {
                  DataDictionary.projectstate_Settled
                };
                var pageData = _loanInfoService.GetPageByState(userId, biddingState, model.Page, model.PageSize);
                var rePage = new ReturnPageData<RClearedLoan>
                { 
                    PagingData = pageData.Items.Select(p => {
                        var item = new RClearedLoan()
                        {
                            LoanId = p.Id,
                            LoanName = p.pro_loan_use,
                            LoanMoney = p.pro_loan_money.Value,
                            LoanRate = p.pro_loan_rate.Value,
                            ApplyDate = p.pro_add_date.Value
                        };
                        item.LoanPeriodDesc = LoanProvider.GetLoanPeriod(p.pro_loan_period.Value, p.pro_period_type.Value);
                        item.LoanStateDesc = LoanProvider.GetLoanState(p.pro_loan_state, "已结清");
                        item.LoanInterest = _investerPlanService.getloanPlanRate(p.pro_loan_money.Value, p.pro_loan_rate.Value, p.pro_loan_period.Value, p.pro_loan_period.Value);
                        item.LoanServiceFee = p.pro_this_servicefee.GetValueOrDefault() * (p.pro_period_type == DataDictionary.deadlinetype_Month ? p.pro_loan_period.Value : 1);
                        var loanPlans = _loanPlanService.GetLoanPlansByCondition(p.Id);
                        item.ClearDate = loanPlans.Max(l => l.pro_collect_date).Value;
                        item.LoanOverRate = loanPlans.Sum(l => l.pro_collect_over_rate).GetValueOrDefault();
                        return item;
                    }),
                    Total = pageData.TotalNum,
                    TotalPageCount = pageData.TotalPageCount
                };

                reModel.ReturnData = rePage;
                reModel.Message = "查询成功";
                reModel.ReturnCode = (int)ReturnCode.Success;
                reModel.Token = model.Token;
                return reModel;
            }
            catch
            {
                reModel.ReturnData = null;
                reModel.Message = "查询失败";
                reModel.ReturnCode = (int)ReturnCode.QueryEorr;
                reModel.Token = model.Token;
                return reModel;
            }
        }

        /// <summary>
        /// 用户借款明细
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<ReturnPageData<RLoanItemDetail>, string> MyLoanDetail([FromBody]BasePageModel model)
        {
            var retModel = new ReturnModel<ReturnPageData<RLoanItemDetail>, string>();

            #region 校验签名

            var signResult = VerifyBase.SignAndToken<BasePageModel>(model, out int userId);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名

            try
            {
                var pageData = _loanInfoService.LoanItemDetail(userId, model.Page, model.PageSize);
                var rePage = new ReturnPageData<RLoanItemDetail>
                {
                    PagingData = pageData.Items,
                    Total = pageData.TotalNum,
                    TotalPageCount = pageData.TotalPageCount
                };

                retModel.ReturnData = rePage;
                retModel.Message = "查询成功";
                retModel.ReturnCode = (int)ReturnCode.Success;
                retModel.Token = model.Token;
                return retModel;
            }
            catch
            {
                retModel.ReturnData = null;
                retModel.Message ="查询失败";
                retModel.ReturnCode = (int)ReturnCode.QueryEorr;
                retModel.Token = model.Token;
                return retModel;
            }
        }

        /// <summary>
        /// 用户是否存在借款
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<bool, string> HasLoan([FromBody]BaseSubmitModel model)
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

            retModel.ReturnData = _loanInfoService.HasLoan(userId) > 0;
            retModel.Message = "查询成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.Token = model.Token;
            return retModel;
        }


        #region 企业用户使用的还款计划


        #endregion
    }
}