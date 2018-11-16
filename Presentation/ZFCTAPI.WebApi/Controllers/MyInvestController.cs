using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using ZFCTAPI.Core;
using ZFCTAPI.Core.Configuration.DataBase;
using ZFCTAPI.Core.Enums;
using ZFCTAPI.Core.Provider;
using ZFCTAPI.Data.ApiModels.ReturnModels;
using ZFCTAPI.Data.ApiModels.SubmitModels;
using ZFCTAPI.Data.CST;
using ZFCTAPI.Data.PRO;
using ZFCTAPI.Services.InvestInfo;
using ZFCTAPI.Services.LoanInfo;
using ZFCTAPI.Services.Popular;
using ZFCTAPI.Services.Settings;
using ZFCTAPI.Services.Sys;
using ZFCTAPI.Services.Transfer;
using ZFCTAPI.WebApi.RequestAttribute;
using ZFCTAPI.WebApi.Validates;


namespace ZFCTAPI.WebApi.Controllers
{
    /// <summary>
    /// 个人中心中我的投资
    /// </summary>
    [Route("api/[controller]/[action]")]
    [RequestLog]
    public class MyInvestController : Controller
    {
        private readonly IInvestInfoService _investInfoService;
        private readonly IInvesterPlanService _investerPlanService;
        private readonly ICstRedInfoService _cstRedInfoService;

        public MyInvestController(IInvestInfoService investInfoService,
            IInvesterPlanService investerPlanService,
            ICstRedInfoService cstRedInfoService)
        {
            _investInfoService = investInfoService;
            _investerPlanService = investerPlanService;
            _cstRedInfoService = cstRedInfoService;
        }

        /// <summary>
        /// 投标中
        /// </summary>
        [HttpPost]
        public ReturnModel<ReturnPageData<RBiddingInvest>, string> MyBiddingInvest([FromBody]BasePageModel model)
        {
            var reModel = new ReturnModel<ReturnPageData<RBiddingInvest>, string>();

            #region 验签

            var resultSign = VerifyBase.SignAndToken<BasePageModel>(model, out int userId);
            if (!resultSign.Equals(ReturnCode.Success))
            {
                reModel.ReturnCode = (int)resultSign;
                reModel.Message = "签名验证失败";
                return reModel;
            }

            #endregion 验签
            try
            {
                var pageData = _investInfoService.BiddingLoanPage(userId, model.Page, model.PageSize);
                var rePage = new ReturnPageData<RBiddingInvest>
                {
                    PagingData = pageData.Items.Select(p => {
                        p.LoanStateDesc = LoanProvider.GetLoanState(p.LoanState);
                        p.LoanPeriodDesc = LoanProvider.GetLoanPeriod(p.LoanPeriod, p.LoanPeriodType);
                        p.ExpectedRevenue = _investerPlanService.getloanPlanRate(p.InvestMoney, p.LoanRate, p.LoanPeriod, LoanProvider.GetLoanRepayPeriod(p.LoanRepayType, p.LoanPeriodType, p.LoanPeriod));
                        p.LoanRepayType = InterestProvider.LoadInterestProviderGetByFriendlyName(p.LoanRepayType);
                        p.UseRedMoney = (_cstRedInfoService.InvestUseRed(p.InvestId)?.cst_red_money).GetValueOrDefault();
                        return p;
                    }).ToList(),
                    Total = pageData.TotalNum,
                    TotalPageCount = pageData.TotalPageCount
                };
                var investStatisticsItems = new List<InvestStatisticsType>
                {
                    InvestStatisticsType.InvestMoney,
                    InvestStatisticsType.InvestCount
                };
                var investStatistics = _investInfoService.InvestStatistics(userId, investStatisticsItems);
                rePage.Extra1 = investStatistics.InvestMoney.ToString("0.00");
                rePage.Extra2 = investStatistics.InvestCount.ToString();
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
        public ReturnModel<ReturnPageData<RRepaymentInvest>, string> MyRepaymentInvest([FromBody]BasePageModel model)
        {
            var reModel = new ReturnModel<ReturnPageData<RRepaymentInvest>, string>();

            #region 验签

            var resultSign = VerifyBase.SignAndToken<BasePageModel>(model, out int userId);
            if (!resultSign.Equals(ReturnCode.Success))
            {
                reModel.ReturnCode = (int)resultSign;
                reModel.Message = "签名验证失败";
                return reModel;
            }

            #endregion 验签

            try
            {
                var pageData = _investInfoService.RepaymentLoanPage(userId, model.Page, model.PageSize);
                var rePage = new ReturnPageData<RRepaymentInvest>
                {
                    PagingData = pageData.Items.Select(p => {
                        p.LoanStateDesc = LoanProvider.GetLoanState(p.LoanState);
                        return p;
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
        public ReturnModel<ReturnPageData<RClearedInvest>, string> MyClearedInvest([FromBody]BasePageModel model)
        {
            var reModel = new ReturnModel<ReturnPageData<RClearedInvest>, string>();

            #region 验签

            var resultSign = VerifyBase.SignAndToken<BasePageModel>(model, out int userId);
            if (!resultSign.Equals(ReturnCode.Success))
            {
                reModel.ReturnCode = (int)resultSign;
                reModel.Message = "签名验证失败";
                return reModel;
            }

            #endregion 验签

            try
            {
                var pageData = _investInfoService.ClearedLoanPage(userId, model.Page, model.PageSize);
                var rePage = new ReturnPageData<RClearedInvest>
                {
                    PagingData = pageData.Items.Select(p => {
                        p.LoanStateDesc = LoanProvider.GetLoanState(p.LoanState);
                        p.LoanPeriodDesc = LoanProvider.GetLoanPeriod(p.LoanPeriod, p.LoanPeriodType);
                        p.ExpectedRevenue = _investerPlanService.getloanPlanRate(p.LoanMoney, p.LoanRate, p.LoanPeriod, LoanProvider.GetLoanRepayPeriod(p.LoanRepayType,p.LoanPeriodType,p.LoanPeriod));
                        p.LoanRepayType = InterestProvider.LoadInterestProviderGetByFriendlyName(p.LoanRepayType);
                        p.UseRedMoney = (_cstRedInfoService.InvestUseRed(p.InvestId)?.cst_red_money).GetValueOrDefault();
                        return p;
                    }).ToList(),
                    Total = pageData.TotalNum,
                    TotalPageCount = pageData.TotalPageCount
                };
                var investPlanStatisticsItems = new List<InvestPlanStatisticsType>
                {
                    InvestPlanStatisticsType.CumulativeIncome,
                    InvestPlanStatisticsType.ReceivedPrincipal
                };
                var investPlanStatistics = _investerPlanService.InvestPlanStatistics(userId, investPlanStatisticsItems);
                rePage.Extra1 = (investPlanStatistics.CumulativeIncome + investPlanStatistics.ReceivedPrincipal).ToString("0.00");


                var investStatisticsItems = new List<InvestStatisticsType>
                {
                    InvestStatisticsType.ClearedCount,
                };
                var investStatistics = _investInfoService.InvestStatistics(userId, investStatisticsItems);

                rePage.Extra2 = investStatistics.ClearedCount.ToString();

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
        /// App还款中
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<ReturnPageData<RAPPRepaymentInvest>, string> MyAppRepaymentInvest([FromBody]BasePageModel model)
        {
            var reModel = new ReturnModel<ReturnPageData<RAPPRepaymentInvest>, string>();
            #region 验签

            var resultSign = VerifyBase.SignAndToken<BasePageModel>(model, out int userId);
            if (!resultSign.Equals(ReturnCode.Success))
            {
                reModel.ReturnCode = (int)resultSign;
                reModel.Message = "签名验证失败";
                return reModel;
            }

            #endregion 验签

            try
            {
                var pageData = _investInfoService.AppRepaymentLoanPage(userId, model.Page, model.PageSize);
                var rePage = new ReturnPageData<RAPPRepaymentInvest>
                {
                    PagingData = pageData.Items.Select(p => {
                        p.LoanStateDesc = "已结清";
                        p.LoanPeriodDesc = LoanProvider.GetLoanPeriod(p.LoanPeriod, p.LoanPeriodType);
                        p.ExpectedRevenue = _investerPlanService.getloanPlanRate(p.LoanMoney, p.LoanRate, p.LoanPeriod, LoanProvider.GetLoanRepayPeriod(p.LoanRepayType, p.LoanPeriodType, p.LoanPeriod));
                        p.LoanRepayType = InterestProvider.LoadInterestProviderGetByFriendlyName(p.LoanRepayType);
                        p.UseRedMoney = (_cstRedInfoService.InvestUseRed(p.InvestId)?.cst_red_money).GetValueOrDefault();
                        return p;
                    }).ToList(),
                    Total = pageData.TotalNum,
                    TotalPageCount = pageData.TotalPageCount
                };
                var investStatisticsItems = new List<InvestStatisticsType>
                {
                    InvestStatisticsType.RepaymentMoney,
                    InvestStatisticsType.RepaymentCount
                };
                var investStatistics = _investInfoService.InvestStatistics(userId, investStatisticsItems);
                rePage.Extra1 = investStatistics.RepaymentMoney.ToString("0.00");
                rePage.Extra2 = investStatistics.RepaymentCount.ToString();
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
        /// 待收明细
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<ReturnPageData<RInvestPlanStatistics>, string> InvestPlanDetail([FromBody]SInvestPlanDetail model)
        {
            var retModel = new ReturnModel<ReturnPageData<RInvestPlanStatistics>, string>();

            #region 校验签名

            var signResult = VerifyBase.SignAndToken<SInvestPlanDetail>(model, out int userId);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名

            //待收
            var returnModel = new ReturnPageData<RInvestPlanStatistics>();
            if (model.Type.Equals("1"))
            {
                var investPlanDetail = _investerPlanService.GetDetailListByUserId(userId, 0);
                //投资罚息
                //var investWaitReceiveOverRate = _investerPlanService.CalculationOverRate(userId);

                var data = investPlanDetail.OrderBy(p => p.PayDate).GroupBy(p => p.PayDate.Value.Date).Select(p =>
                {
                    var rInvestPlanStatistics = new RInvestPlanStatistics();
                    rInvestPlanStatistics.PayDate = p.First().PayDate.Value;
                    rInvestPlanStatistics.TotalCollection = p.Sum(x => x.PayTotal);
                    rInvestPlanStatistics.InvestPlans = p.Select(x =>
                    {
                        var rInvestPlanDetail = new RInvestPlanDetail();
                        rInvestPlanDetail.LoanName = x.LoanName;
                        rInvestPlanDetail.TotalCollection = x.PayTotal;
                        rInvestPlanDetail.LoanRepayType = InterestProvider.LoadInterestProviderGetByFriendlyName(x.RepayType);
                        return rInvestPlanDetail;
                    }).ToList();
                    return rInvestPlanStatistics;
                });
                returnModel.PagingData = data.Skip((model.Page - 1) * model.PageSize).Take(model.PageSize);
                returnModel.Total = data.Count();
                returnModel.Extra1 = investPlanDetail.Sum(p => p.PayTotal).ToString("0.00");
                returnModel.Extra2 = investPlanDetail.Where(p => p.PayDate.GetValueOrDefault().Year == DateTime.Now.Year && p.PayDate.GetValueOrDefault().Month == DateTime.Now.Month).Sum(p => p.PayTotal).ToString("0.00");
            }
            //已收
            if (model.Type.Equals("2"))
            {
                var investPlanDetail = _investerPlanService.GetDetailListByUserId(userId, 1);
                var data = investPlanDetail.Where(p=>p.CollectTotal.GetValueOrDefault()>0).OrderByDescending(p => p.CollectDate).GroupBy(p => p.CollectDate.Value.Date).Select(p =>
                {
                    var rInvestPlanStatistics = new RInvestPlanStatistics();
                    rInvestPlanStatistics.PayDate = p.First().PayDate.Value;
                    rInvestPlanStatistics.TotalCollection = p.Sum(x => x.CollectTotal.GetValueOrDefault());
                    rInvestPlanStatistics.InvestPlans = p.Select(x =>
                    {
                        var rInvestPlanDetail = new RInvestPlanDetail();
                        rInvestPlanDetail.LoanName = x.LoanName;
                        rInvestPlanDetail.TotalCollection = x.CollectTotal.GetValueOrDefault();
                        rInvestPlanDetail.LoanRepayType = InterestProvider.LoadInterestProviderGetByFriendlyName(x.RepayType);
                        return rInvestPlanDetail;
                    }).ToList();
                    return rInvestPlanStatistics;
                });
                returnModel.PagingData = data.Skip((model.Page - 1) * model.PageSize).Take(model.PageSize);
                returnModel.Total = data.Count();
                returnModel.Extra1 = investPlanDetail.Sum(p => p.CollectTotal).GetValueOrDefault().ToString("0.00");
                returnModel.Extra2 = investPlanDetail.Where(p=>p.CollectDate.GetValueOrDefault().Year == DateTime.Now.Year && p.CollectDate.GetValueOrDefault().Month == DateTime.Now.Month).Sum(p => p.CollectTotal).GetValueOrDefault().ToString("0.00");
            }

            retModel.Message = "查询成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.ReturnData = returnModel;
            retModel.Token = model.Token;
            return retModel;
        }


        /// <summary>
        /// 投资还款计划
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RInvestRepayPlan, string> InvestRepayPlan([FromBody]SInvestRepayPlan model)
        {
            var retModel = new ReturnModel<RInvestRepayPlan, string>();

            #region 校验签名

            var signResult = VerifyBase.SignAndToken<SInvestPlanDetail>(model, out int userId);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名

           
            var investPlans =   _investerPlanService.GetListByCondition(null,model.InvestId);
            var returnModel = new RInvestRepayPlan()
            {
                WaitRepayMoeny = investPlans.Where(p => p.pro_is_clear == false).Sum(p => p.pro_pay_total).GetValueOrDefault(),
                RepayMoeny = investPlans.Where(p => p.pro_is_clear == true).Sum(p => p.pro_collect_total).GetValueOrDefault(),
                InvestPlans = investPlans.Select(p => {
                    var item = new RInvestRepayPlanDetail();
                    if (p.pro_is_clear == true)
                    {
                        item.RepayDate = p.pro_collect_date.Value;
                        item.RepayMoeny = p.pro_collect_money.Value;
                        item.RepayRate = p.pro_collect_rate.GetValueOrDefault() + p.pro_collect_over_rate.GetValueOrDefault();
                        item.RepayStateDesc = "已结清";
                    }
                    else {
                        item.RepayDate = p.pro_pay_date.Value;
                        item.RepayMoeny = p.pro_pay_money.Value;
                        item.RepayRate = p.pro_pay_rate.GetValueOrDefault();
                        item.RepayStateDesc = "未结清";
                    }
                    return item;
                })
            };
            retModel.Message = "查询成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.ReturnData = returnModel;
            retModel.Token = model.Token;
            return retModel;
        }


    }
}