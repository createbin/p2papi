using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using ZFCTAPI.Core.Enums;
using ZFCTAPI.Core.Helpers;
using ZFCTAPI.Data.ApiModels.ReturnModels;
using ZFCTAPI.Data.ApiModels.SubmitModels;
using ZFCTAPI.Data.CST;
using ZFCTAPI.Services.InvestInfo;
using ZFCTAPI.Services.Popular;
using ZFCTAPI.WebApi.RequestAttribute;
using ZFCTAPI.WebApi.Validates;

namespace ZFCTAPI.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [RequestLog]
    public class MyRedEnvelopesController : Controller
    {
        private readonly ICstRedInfoService _cstRedInfoService;
        private readonly IPopEnvelopeRedService _popEnvelopeRedService;
        private readonly IInvestInfoService _investInfoService;

        public MyRedEnvelopesController(ICstRedInfoService cstRedInfoService,
            IPopEnvelopeRedService popEnvelopeRedService,
            IInvestInfoService investInfoService)
        {
            _cstRedInfoService = cstRedInfoService;
            _popEnvelopeRedService = popEnvelopeRedService;
            _investInfoService = investInfoService;
        }

        /// <summary>
        /// 用户红包分页
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<ReturnPageData<RRedEnvelopesModel>, string> UserRedPage([FromBody]BasePageModel model)
        {
            var retModel = new ReturnModel<ReturnPageData<RRedEnvelopesModel>, string>();

            #region 校验签名
            var signResult = VerifyBase.SignAndToken<BasePageModel>(model, out CST_user_info userInfo);
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
                var pageData = _cstRedInfoService.UserRedPage(userInfo.cst_customer_id.Value, model.Page, model.PageSize);
                var rePage = new ReturnPageData<RRedEnvelopesModel>
                {
                    PagingData = pageData.Items.Select(p =>
                    {
                        var m = new RRedEnvelopesModel();
                        var redInfo = _popEnvelopeRedService.GetEnvelopeListInfo(p.cst_red_id.Value);
                        m.RedId = p.Id;
                        m.RedMoney = p.cst_red_money.Value;
                        m.RedStartDate = p.cst_red_startDate;
                        m.RedEndDate = p.cst_red_endDate;
                        m.RedName = redInfo.Name;
                        m.RedType = redInfo.Type;
                        m.RedInstructions = redInfo.EmployIntroductions.Replace("<br/>", ",");
                        if (p.cst_red_startDate == null || p.cst_red_endDate == null)
                        {
                            m.RedValidity = "/";
                        }
                        else
                        {
                            m.RedValidity = CommonHelper.FormatDate(p.cst_red_startDate, p.cst_red_endDate);
                        }

                        if (!p.cst_red_employ && (p.cst_red_endDate == null || p.cst_red_endDate.GetValueOrDefault().Date >= DateTime.Now.Date))
                        {
                            m.RedUseState = "未使用";
                        }
                        else if (p.cst_red_employ)
                        {
                            m.RedUseState = "已使用";
                            m.RedLoanId = _investInfoService.Find(p.cst_red_investId.GetValueOrDefault()).pro_loan_id;
                        }
                        else
                        {
                            m.RedUseState = "已过期";
                        }

                        return m;
                    }),
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
                retModel.Message = "查询失败";
                retModel.ReturnCode = (int)ReturnCode.QueryEorr;
                retModel.Token = model.Token;
                return retModel;
            }
        }

        /// <summary>
        /// 用户过期红包分页
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<ReturnPageData<RRedEnvelopesModel>, string> UserRedExpiredPage([FromBody]BasePageModel model)
        {
            var retModel = new ReturnModel<ReturnPageData<RRedEnvelopesModel>, string>();

            #region 校验签名

            var signResult = VerifyBase.SignAndToken<BasePageModel>(model, out CST_user_info userInfo);
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
                var pageData = _cstRedInfoService.UserRedExpiredPage(userInfo.cst_customer_id.Value, model.Page, model.PageSize);
                var rePage = new ReturnPageData<RRedEnvelopesModel>
                {
                    PagingData = pageData.Items.Select(p =>
                    {
                        var m = new RRedEnvelopesModel();
                        var redInfo = _popEnvelopeRedService.GetEnvelopeListInfo(p.cst_red_id.GetValueOrDefault());
                        m.RedId = p.Id;
                        m.RedMoney = p.cst_red_money.Value;
                        m.RedStartDate = p.cst_red_startDate;
                        m.RedEndDate = p.cst_red_endDate;
                        m.RedName = redInfo.Name;
                        m.RedType = redInfo.Type;
                        m.RedInstructions = redInfo.EmployIntroductions.Replace("<br/>", ",");
                        if (p.cst_red_startDate == null || p.cst_red_endDate == null)
                        {
                            m.RedValidity = "/";
                        }
                        else
                        {
                            m.RedValidity = CommonHelper.FormatDate(p.cst_red_startDate, p.cst_red_endDate);
                        }

                        if (!p.cst_red_employ && (p.cst_red_endDate == null || p.cst_red_endDate.GetValueOrDefault().Date <= DateTime.Now.Date))
                        {
                            m.RedUseState = "未使用";
                        }
                        else if (p.cst_red_employ)
                        {
                            m.RedUseState = "已使用";
                            m.RedLoanId = _investInfoService.Find(p.cst_red_investId.GetValueOrDefault()).pro_loan_id;
                        }
                        else
                        {
                            m.RedUseState = "已过期";
                        }

                        return m;
                    }),
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
                retModel.Message = "查询失败";
                retModel.ReturnCode = (int)ReturnCode.QueryEorr;
                retModel.Token = model.Token;
                return retModel;
            }
        }

        /// <summary>
        /// 用户已用红包分页
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<ReturnPageData<RRedEnvelopesModel>, string> UserRedUsedPage([FromBody]BasePageModel model)
        {
            var retModel = new ReturnModel<ReturnPageData<RRedEnvelopesModel>, string>();

            #region 校验签名

            var signResult = VerifyBase.SignAndToken<BasePageModel>(model, out CST_user_info userInfo);
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
                var pageData = _cstRedInfoService.UserRedUsedPage(userInfo.cst_customer_id.Value, model.Page, model.PageSize);
                var rePage = new ReturnPageData<RRedEnvelopesModel>
                {
                    PagingData = pageData.Items.Select(p =>
                    {
                        var m = new RRedEnvelopesModel();
                        var redInfo = _popEnvelopeRedService.GetEnvelopeListInfo(p.cst_red_id.GetValueOrDefault());
                        m.RedId = p.Id;
                        m.RedMoney = p.cst_red_money.Value;
                        m.RedStartDate = p.cst_red_startDate;
                        m.RedEndDate = p.cst_red_endDate;
                        m.RedName = redInfo.Name;
                        m.RedType = redInfo.Type;
                        m.RedInstructions = redInfo.EmployIntroductions.Replace("<br/>", ",");
                        if (p.cst_red_startDate == null || p.cst_red_endDate == null)
                        {
                            m.RedValidity = "/";
                        }
                        else
                        {
                            m.RedValidity = CommonHelper.FormatDate(p.cst_red_startDate, p.cst_red_endDate);
                        }

                        if (!p.cst_red_employ && (p.cst_red_endDate == null || p.cst_red_endDate.GetValueOrDefault().Date <= DateTime.Now.Date))
                        {
                            m.RedUseState = "未使用";
                        }
                        else if (p.cst_red_employ)
                        {
                            m.RedUseState = "已使用";
                            m.RedLoanId = _investInfoService.Find(p.cst_red_investId.GetValueOrDefault()).pro_loan_id;
                        }
                        else
                        {
                            m.RedUseState = "已过期";
                        }

                        return m;
                    }),
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
                retModel.Message = "查询失败";
                retModel.ReturnCode = (int)ReturnCode.QueryEorr;
                retModel.Token = model.Token;
                return retModel;
            }
        }

        /// <summary>
        /// 用户可用红包分页
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<ReturnPageData<RRedEnvelopesModel>, string> UserRedWaitUsePage([FromBody]BasePageModel model)
        {
            var retModel = new ReturnModel<ReturnPageData<RRedEnvelopesModel>, string>();

            #region 校验签名

            var signResult = VerifyBase.SignAndToken<BasePageModel>(model, out CST_user_info userInfo);
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
                var pageData = _cstRedInfoService.UserRedWaitUsePage(userInfo.cst_customer_id.Value, model.Page, model.PageSize);
                var rePage = new ReturnPageData<RRedEnvelopesModel>
                {
                    PagingData = pageData.Items.Select(p =>
                    {
                        var m = new RRedEnvelopesModel();
                        var redInfo = _popEnvelopeRedService.GetEnvelopeListInfo(p.cst_red_id.GetValueOrDefault());
                        m.RedId = p.Id;
                        m.RedMoney = p.cst_red_money.Value;
                        m.RedStartDate = p.cst_red_startDate;
                        m.RedEndDate = p.cst_red_endDate;
                        m.RedName = redInfo.Name;
                        m.RedType = redInfo.Type;
                        m.RedInstructions = redInfo.EmployIntroductions.Replace("<br/>", ",");
                        if (p.cst_red_startDate == null || p.cst_red_endDate == null)
                        {
                            m.RedValidity = "/";
                        }
                        else
                        {
                            m.RedValidity = CommonHelper.FormatDate(p.cst_red_startDate, p.cst_red_endDate);
                        }

                        if (!p.cst_red_employ && (p.cst_red_endDate == null || p.cst_red_endDate.GetValueOrDefault().Date <= DateTime.Now.Date))
                        {
                            m.RedUseState = "未使用";
                        }
                        else if (p.cst_red_employ)
                        {
                            m.RedUseState = "已使用";
                            m.RedLoanId = _investInfoService.Find(p.cst_red_investId.GetValueOrDefault()).pro_loan_id;
                        }
                        else
                        {
                            m.RedUseState = "已过期";
                        }

                        return m;
                    }),
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
                retModel.Message = "查询失败";
                retModel.ReturnCode = (int)ReturnCode.QueryEorr;
                retModel.Token = model.Token;
                return retModel;
            }
        }
    }
}