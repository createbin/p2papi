using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using ZFCTAPI.Core;
using ZFCTAPI.Core.Enums;
using ZFCTAPI.Core.Provider;
using ZFCTAPI.Data.ApiModels.ReturnModels;
using ZFCTAPI.Data.ApiModels.SubmitModels;
using ZFCTAPI.Data.CST;
using ZFCTAPI.Data.LoanInfo;
using ZFCTAPI.Services.LoanInfo;
using ZFCTAPI.Services.Sys;
using ZFCTAPI.WebApi.RequestAttribute;
using ZFCTAPI.WebApi.Validates;

namespace ZFCTAPI.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [RequestLog]
    public class MyLoanPlanController : Controller
    {
        private readonly ILoanPlanService _loanPlanService;
        private readonly ISYSDataDictionaryService _iSYSDataDictionaryService;
        private readonly ILoanInfoService _loanInfoService;

        public MyLoanPlanController(ILoanPlanService loanPlanService,
            ILoanInfoService loanInfoService,
            ISYSDataDictionaryService iSYSDataDictionaryService)
        {
            _loanPlanService = loanPlanService;
            _loanInfoService = loanInfoService;
            _iSYSDataDictionaryService = iSYSDataDictionaryService;
        }

        /// <summary>
        ///  用户已结清还款计划
        /// </summary>
        [HttpPost]
        public ReturnModel<ReturnPageData<RMyLoanPlanModel>, string> UserLoanPlanClear([FromBody]BasePageModel model)
        {
            var reModel = new ReturnModel<ReturnPageData<RMyLoanPlanModel>, string>();

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
                var pageData = _loanPlanService.UserLoanPlanClear(userId, model.Page, model.PageSize);
                var rePage = new ReturnPageData<RMyLoanPlanModel>
                {
                    PagingData = pageData.Items.Select(p =>
                    {
                        if (p.CollectType == DataDictionary.RepaymentType_Compulsoryrepay.ToString())
                        {
                            p.CollectType = "强制还款";
                        }
                        else if (p.CollectType == DataDictionary.repaymenstate_Normal.ToString())
                        {
                            p.CollectType = "正常还款";
                        }
                        else if (p.CollectType == DataDictionary.repaymenstate_Replace.ToString())
                        {
                            p.CollectType = "平台代还";
                        }
                        else if (p.CollectType == DataDictionary.RepaymentType_PlatformDaihuan.ToString())
                        {
                            p.CollectType = "平台代还";
                        }
                        else
                        {
                            p.CollectType = "正常还款";
                        }
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
        ///  用户未结清还款计划
        /// </summary>
        [HttpPost]
        public ReturnModel<ReturnPageData<RMyLoanPlanModel>, string> UserLoanPlanWaitClear([FromBody]BasePageModel model)
        {
            var reModel = new ReturnModel<ReturnPageData<RMyLoanPlanModel>, string>();

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
                var pageData = _loanPlanService.UserLoanPlanWaitClear(userId, model.Page, model.PageSize);
                var rePage = new ReturnPageData<RMyLoanPlanModel>
                {
                    PagingData = pageData.Items.Select(p =>
                    {
                        if (p.CollectType == DataDictionary.RepaymentType_Compulsoryrepay.ToString())
                        {
                            p.CollectType = "强制还款";
                        }
                        else if (p.CollectType == DataDictionary.repaymenstate_Normal.ToString())
                        {
                            p.CollectType = "正常还款";
                        }
                        else if (p.CollectType == DataDictionary.repaymenstate_Replace.ToString())
                        {
                            p.CollectType = "平台代还";
                        }
                        else if (p.CollectType == DataDictionary.RepaymentType_PlatformDaihuan.ToString())
                        {
                            p.CollectType = "平台代还";
                        }
                        else
                        {
                            p.CollectType = "正常还款";
                        }

                        if (p.IsUsing)
                        {
                            p.CollectType = "银行处理中";
                        }
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
        ///  项目还款计划
        /// </summary>
        [HttpPost]
        public ReturnModel<ReturnPageData<RMyLoanPlanModel>, string> LoanPayPlan([FromBody]SLoanPlan model)
        {
            var reModel = new ReturnModel<ReturnPageData<RMyLoanPlanModel>, string>();

            #region 验签

            var resultSign = VerifyBase.SignAndToken<SLoanPlan>(model, out int userId);
            if (!resultSign.Equals(ReturnCode.Success))
            {
                reModel.ReturnCode = (int)resultSign;
                reModel.Message = "签名验证失败";
                return reModel;
            }

            #endregion 验签

            try
            {
                var pageData = _loanPlanService.LoanPayPlan(model.LoanId, model.Page, model.PageSize);
                var rePage = new ReturnPageData<RMyLoanPlanModel>
                {
                    PagingData = pageData.Items.Select(p =>
                    {
                        if (p.CollectType == DataDictionary.RepaymentType_Compulsoryrepay.ToString())
                        {
                            p.CollectType = "强制还款";
                        }
                        else if (p.CollectType == DataDictionary.repaymenstate_Normal.ToString())
                        {
                            p.CollectType = "正常还款";
                        }
                        else if (p.CollectType == DataDictionary.repaymenstate_Replace.ToString())
                        {
                            p.CollectType = "平台代还";
                        }
                        else if (p.CollectType == DataDictionary.RepaymentType_PlatformDaihuan.ToString())
                        {
                            p.CollectType = "平台代还";
                        }
                        else
                        {
                            p.CollectType = "正常还款";
                        }
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
        /// 还款明细
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RRepayDetail, string> RepayDetail([FromBody]SRepayDetail model)
        {
            var retModel = new ReturnModel<RRepayDetail, string>();

            #region 校验签名

            var signResult = VerifyBase.SignAndToken<SRepayDetail>(model, out int userId);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名

            var loanPlan = _loanPlanService.Find(model.LoanPlanId);

            if (loanPlan != null)
            {
                var loanPlans = _loanPlanService.GetLoanPlansByCondition(loanPlan.pro_loan_id.Value);
                var loanInfo = _loanInfoService.Find(loanPlan.pro_loan_id.Value);
                var ClearloanPlans = loanPlans.Where(p => p.pro_is_clear == true);
                var NoClearLoanPlans = loanPlans.Where(p => p.pro_is_clear == false).OrderBy(p => p.pro_pay_date).ToList();

                var returnModel = new RRepayDetail
                {
                    id = loanPlan.Id,
                    LoanName = loanInfo.pro_loan_use,
                    LoanMoney = loanInfo.pro_loan_money.Value,
                    LoanPeriod = LoanProvider.GetLoanPeriod(loanInfo.pro_loan_period.Value, loanInfo.pro_period_type.Value),
                    Period = loanPlan.pro_loan_period.Value,
                    LoanRate = loanInfo.pro_loan_rate.Value,
                    LoanState = _iSYSDataDictionaryService.Find(loanInfo.pro_loan_state.Value).sys_data_name,
                    RepayDate = loanPlan.pro_pay_date.Value,
                    SurplusPeriod = NoClearLoanPlans.Count(),
                    SettledPeriod = ClearloanPlans.Count(),
                    SettledPrincipal = ClearloanPlans.Sum(p => p.pro_collect_money.GetValueOrDefault()),
                    Principal = loanPlan.pro_pay_money.Value,
                    SettledInterest = ClearloanPlans.Sum(p => p.pro_collect_rate.GetValueOrDefault()),
                    Interest = loanPlan.pro_pay_rate.Value,
                    SettledLateFee = ClearloanPlans.Sum(p => p.pro_collect_over_rate.GetValueOrDefault()),
                    SettledServiceFee = ClearloanPlans.Sum(p => p.pro_collect_service_fee.GetValueOrDefault()),
                    ServiceFee = loanPlan.pro_pay_service_fee.GetValueOrDefault(),
                    WaitPayMoney = loanPlans.Sum(p => p.pro_pay_total.Value),
                    IsClear = loanPlan.pro_is_clear == true ? "已还清" : "未还清",
                };

                //是否存在下一期
                if (NoClearLoanPlans.Count() >= 2)
                {
                    returnModel.NextRepayDate = NoClearLoanPlans[1].pro_pay_date.GetValueOrDefault();
                    returnModel.NextWaitPayMoney = NoClearLoanPlans[1].pro_pay_total.GetValueOrDefault();
                }

                //借款罚息
                if (loanPlan.pro_pay_date.GetValueOrDefault().Date < DateTime.Now.Date)
                {
                    //逾期费
                    returnModel.LateFee = _loanPlanService.CalculationOverRate(new Data.LoanInfo.CalculationLoanPlanOverRateRequest
                    {
                        CollectDate = loanPlan.pro_collect_date,
                        PayDate = loanPlan.pro_pay_date,
                        PayMoney = loanPlan.pro_pay_money.Value,
                        PayRate = loanPlan.pro_pay_rate.Value,
                        SurplusOverRate = loanPlan.pro_sett_over_rate.GetValueOrDefault()
                    });
                }

                // 应还总额 + 逾期费
                returnModel.CurrenyWaitPayMoney = loanPlan.pro_pay_total.Value + returnModel.LateFee;

                retModel.Message = "成功";
                retModel.ReturnCode = (int)ReturnCode.Success;
                retModel.ReturnData = returnModel;
                retModel.Token = model.Token;
                return retModel;
            }

            retModel.Message = "查询失败";
            retModel.ReturnCode = (int)ReturnCode.DataEorr;
            retModel.Token = model.Token;
            return retModel;
        }


        #region 企业用户使用的还款计划

        /// <summary>
        /// 企业户最近还款计划
        /// </summary>
        [HttpPost]
        public ReturnModel<ReturnPageData<RMyLoanPlanModel>, string> LastDateWaitClear([FromBody] BasePageModel model)
        {
            var reModel = new ReturnModel<ReturnPageData<RMyLoanPlanModel>, string>();
            #region 验签
            var resultSign = VerifyBase.SignAndToken<BasePageModel>(model, out CST_user_info userInfo);
            if (!resultSign.Equals(ReturnCode.Success))
            {
                reModel.ReturnCode = (int)resultSign;
                reModel.Message = "签名验证失败";
                return reModel;
            }
            #endregion 验签
            try
            {
                var pageData = _loanPlanService.LastDateWaitClear(userInfo.Id, model.Page, model.PageSize);
                var rePage = new ReturnPageData<RMyLoanPlanModel>
                {
                    PagingData = pageData.Items.Select(p =>
                    {
                        if (p.CollectType == DataDictionary.RepaymentType_Compulsoryrepay.ToString())
                        {
                            p.CollectType = "强制还款";
                        }
                        else if (p.CollectType == DataDictionary.repaymenstate_Normal.ToString())
                        {
                            p.CollectType = "正常还款";
                        }
                        else if (p.CollectType == DataDictionary.repaymenstate_Replace.ToString())
                        {
                            p.CollectType = "平台代还";
                        }
                        else if (p.CollectType == DataDictionary.RepaymentType_PlatformDaihuan.ToString())
                        {
                            p.CollectType = "平台代还";
                        }
                        else if (p.PayDate < DateTime.Now)
                        {
                            p.CollectType = "已逾期";
                        }
                        else
                        {
                            p.CollectType = "正常还款";
                        }

                        if (p.IsUsing)
                        {
                            p.CollectType = "银行处理中";
                        }
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
        /// 企业户还款中的数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RMyRepayLoans, string> MyRepayLoan([FromBody] BasePageModel model)
        {
            var reModel = new ReturnModel<RMyRepayLoans, string>();
            #region 验签
            var resultSign = VerifyBase.SignAndToken<BasePageModel>(model, out CST_user_info userInfo);
            if (!resultSign.Equals(ReturnCode.Success))
            {
                reModel.ReturnCode = (int)resultSign;
                reModel.Message = "签名验证失败";
                return reModel;
            }
            #endregion 验签

            try
            {
                var resultData = _loanPlanService.MyRepayLoans(userInfo.Id);
                foreach (var resultDataRepayLoan in resultData.RepayLoans)
                {
                    resultDataRepayLoan.RepayTypeName =InterestProvider.LoadInterestProviderGetByFriendlyName(resultDataRepayLoan.RepayType);
                    resultDataRepayLoan.RepayLoanPlans = MyRepayLoanPlans(resultDataRepayLoan.LoanId);
                }
                reModel.ReturnData = resultData;
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
        /// 企业担保的标的
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RMyRepayLoan, string> GuaranteeRepayLoan([FromBody] SGuaranteeLoans model)
        {
            var reModel = new ReturnModel<RMyRepayLoan, string>();
            #region 验签
            var resultSign = VerifyBase.SignAndToken<SGuaranteeLoans>(model, out CST_user_info userInfo);
            if (!resultSign.Equals(ReturnCode.Success))
            {
                reModel.ReturnCode = (int)resultSign;
                reModel.Message = "签名验证失败";
                return reModel;
            }
            #endregion 验签

            try
            {
                if (userInfo.CompanyId != null)
                {
                    var resultData = _loanPlanService.GurRepayLoans(model.LoanNo,model.Loaner,userInfo.CompanyId.Value);
                    resultData.RepayTypeName = InterestProvider.LoadInterestProviderGetByFriendlyName(resultData.RepayType);
                    resultData.RepayLoanPlans = MyRepayLoanPlans(resultData.LoanId);
                    reModel.ReturnData = resultData;
                }
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
        /// 项目未完成的还款计划带罚息
        /// </summary>
        /// <returns></returns>
        private List<RMyLoanPlanModel> MyRepayLoanPlans(int loanId,int isClear=0)
        {
            try
            {
                var pageData = _loanPlanService.LoanPayPlan(loanId,1,50,isClear);
                var rePage = new ReturnPageData<RMyLoanPlanModel>
                {
                    PagingData = pageData.Items.Select(p =>
                    {
                        if (p.CollectType == DataDictionary.RepaymentType_Compulsoryrepay.ToString())
                        {
                            p.CollectType = "强制还款";
                        }
                        else if (p.CollectType == DataDictionary.repaymenstate_Normal.ToString())
                        {
                            p.CollectType = "正常还款";
                        }
                        else if (p.CollectType == DataDictionary.repaymenstate_Replace.ToString())
                        {
                            p.CollectType = "平台代还";
                        }
                        else if (p.CollectType == DataDictionary.RepaymentType_PlatformDaihuan.ToString())
                        {
                            p.CollectType = "平台代还";
                        }
                        else
                        {
                            p.CollectType = "正常还款";
                        }
                        p.OverRateMoney = _loanPlanService.CalculationOverRate(new CalculationLoanPlanOverRateRequest
                        {
                            CollectDate = DateTime.Now,
                            PayDate = p.PayDate,
                            PayMoney = p.PayMoney,
                            PayRate = p.PayRate,
                            SurplusOverRate = 0.00m
                        });
                        
                        return p;
                    }),
                    Total = pageData.TotalNum,
                    TotalPageCount = pageData.TotalPageCount
                };
                return rePage.PagingData.ToList();
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 已结清的标的
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RMyRepayLoans, string> MyRepayedLoan([FromBody] BasePageModel model)
        {
            var reModel = new ReturnModel<RMyRepayLoans, string>();
            #region 验签
            var resultSign = VerifyBase.SignAndToken<BasePageModel>(model, out CST_user_info userInfo);
            if (!resultSign.Equals(ReturnCode.Success))
            {
                reModel.ReturnCode = (int)resultSign;
                reModel.Message = "签名验证失败";
                return reModel;
            }
            #endregion 验签

            try
            {
                var resultData = _loanPlanService.MyRepayedLoans(userInfo.Id);
                foreach (var resultDataRepayLoan in resultData.RepayLoans)
                {
                    resultDataRepayLoan.RepayTypeName = InterestProvider.LoadInterestProviderGetByFriendlyName(resultDataRepayLoan.RepayType);
                    resultDataRepayLoan.RepayLoanPlans = MyRepayLoanPlans(resultDataRepayLoan.LoanId,1);
                }
                reModel.ReturnData = resultData;
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
        /// 担保代偿的还款计划
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<ReturnPageData<RGurClearedPlan>, string> GurRepayedLoan([FromBody] BasePageModel model)
        {
            var reModel = new ReturnModel<ReturnPageData<RGurClearedPlan>, string>();
            #region 验签
            var resultSign = VerifyBase.SignAndToken<BasePageModel>(model, out CST_user_info userInfo);
            if (!resultSign.Equals(ReturnCode.Success))
            {
                reModel.ReturnCode = (int)resultSign;
                reModel.Message = "签名验证失败";
                return reModel;
            }
            #endregion 验签

            try
            {
                if (userInfo.CompanyId != null)
                {
                    var pageData = _loanPlanService.GurClearedPlans(userInfo.CompanyId.Value, model.Page, model.PageSize);
                    var resultData = new ReturnPageData<RGurClearedPlan>
                    {
                        Total = pageData.TotalNum,
                        TotalPageCount = pageData.TotalPageCount
                    };
                    reModel.ReturnData = resultData;
                    reModel.Message = "查询成功";
                    reModel.ReturnCode = (int)ReturnCode.Success;
                    reModel.Token = model.Token;
                    return reModel;
                }
                else
                {
                    reModel.ReturnData = null;
                    reModel.Message = "担保人信息不存在";
                    reModel.ReturnCode = (int)ReturnCode.QueryEorr;
                    reModel.Token = model.Token;
                    return reModel;
                }
            }
            catch(Exception ex)
            {
                reModel.ReturnData = null;
                reModel.Message =ex.Message;
                reModel.ReturnCode = (int)ReturnCode.QueryEorr;
                reModel.Token = model.Token;
                return reModel;
            }
        }
        #endregion
    }
}