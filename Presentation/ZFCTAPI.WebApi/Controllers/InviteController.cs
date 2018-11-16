using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZFCTAPI.Core.Configuration;
using ZFCTAPI.Data.ApiModels.ReturnModels;
using ZFCTAPI.Data.ApiModels.SubmitModels;
using ZFCTAPI.Services.UserInfo;
using ZFCTAPI.Data.CST;
using ZFCTAPI.Services.Promotion;
using ZFCTAPI.Core.Enums;
using ZFCTAPI.Core.Infrastructure;
using ZFCTAPI.WebApi.RequestAttribute;
using ZFCTAPI.WebApi.Validates;

namespace ZFCTAPI.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [RequestLog]
    public class InviteController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly ICstUserInfoService _cstUserInfoService;
        private readonly IInvitationActivitie _invitationActivitie;
        private readonly ICstRealNameCheckService _cstRealNameCheckService;
        private readonly IAccountInfoService _accountInfoService;

        public InviteController(ICustomerService customerService,
            ICstUserInfoService cstUserInfoService,
            IInvitationActivitie invitationActivitie,
            ICstRealNameCheckService cstRealNameCheckService,
            IAccountInfoService accountInfoService)
        {
            this._customerService = customerService;
            this._cstUserInfoService = cstUserInfoService;
            this._invitationActivitie = invitationActivitie;
            this._cstRealNameCheckService = cstRealNameCheckService;
            _accountInfoService = accountInfoService;
        }

        /// <summary>
        /// 邀请活动
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<InviteStatistics,string> InviteStatistics([FromBody]BaseSubmitModel model)
        {
            ReturnModel<InviteStatistics, string> rModel = new ReturnModel<InviteStatistics, string>();

            var resultSign = VerifyBase.SignAndToken<BasePageModel>(model, out CST_user_info cstUserInfo);
            if (!resultSign.Equals(ReturnCode.Success))
            {
                rModel.ReturnCode = (int)resultSign;
                rModel.Message = "签名验证失败";
                return rModel;
            }

            var returnData = new InviteStatistics();
            var userInfo = _customerService.GetCustomerByCondition(null, cstUserInfo.cst_user_phone);
            var userInfos = _customerService.GetCustomerByParentId(cstUserInfo.cst_customer_id.Value);

            returnData.InviteCount = userInfos == null ? 0 : userInfos.Count;
            returnData.InviteCode = userInfo.cst_invitation_code;
            returnData.InviteUrl = EngineContext.Current.Resolve<ZfctWebConfig>().PcUrl+ "customer/Register?yqm=" + returnData.InviteCode;

            var month = DateTime.Now.Month;
            var year = DateTime.Now.Year;
            returnData.MonthReward = 0.0m;
            returnData.AllReward = 0.0m;
            returnData.MonthInviter = userInfos==null?0:userInfos.Where(p => p.CreatedOnUtc.Month == month && p.CreatedOnUtc.Year == year).Count();
            var benifits = _invitationActivitie.GetListsByBeneficiaryId(cstUserInfo.cst_customer_id.Value);
            if (benifits.Any())
            {
                returnData.MonthReward = benifits.Where(p => p.Months == month && p.Year == year).Sum(s => s.BeneficiaryAmount);
                returnData.AllReward = benifits.Sum(s => s.BeneficiaryAmount);
            }

            rModel.Message = "查询成功";
            rModel.ReturnCode = (int)ReturnCode.Success;
            rModel.ReturnData = returnData;
            rModel.Signature = "";
            rModel.Token = model.Token;
            return rModel;
        }

        /// <summary>
        /// 活动邀请列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<InviteMonthAbstract,string> InviteMonthAbstract([FromBody]BaseSubmitModel model)
        {
            ReturnModel<InviteMonthAbstract, string> rModel = new ReturnModel<InviteMonthAbstract, string>();
            InviteMonthAbstract returnData = new InviteMonthAbstract();

            var resultSign = VerifyBase.SignAndToken<BasePageModel>(model, out CST_user_info cstUserInfo);
            if (!resultSign.Equals(ReturnCode.Success))
            {
                rModel.ReturnCode = (int)resultSign;
                rModel.Message = "签名验证失败";
                return rModel;
            }

            var monthResult = _invitationActivitie.GetGroupByMonth(cstUserInfo.cst_customer_id.Value);
            var benifits = _invitationActivitie.GetListsByBeneficiaryId(cstUserInfo.cst_customer_id.Value);

            if (monthResult.Any())
            {
                returnData.Count = monthResult.Count;
                foreach (var gm in monthResult)
                {
                    var info = new RewardOverview();
                    info.MonthReward = 0.0m;
                    if (benifits.Any(p => p.Months == gm.Month && p.Year == gm.Year))
                    {
                        info.MonthReward = benifits.Where(p => p.Months == gm.Month && p.Year == gm.Year).Sum(s => s.BeneficiaryAmount);
                    }

                    info.Month = gm.Month;
                    info.Year = gm.Year;
                    info.MonthInvite = gm.Count;
                    info.RegisterCount = gm.Count;
                    info.InvestCount = _invitationActivitie.GetInvestCount(cstUserInfo.cst_customer_id.Value, gm.Year,gm.Month);
                    info.CertificationCount = _invitationActivitie.GetOpenAccountCount(cstUserInfo.cst_customer_id.Value, gm.Year, gm.Month);
                    returnData.RewardOverviews.Add(info);
                }
            }

            returnData.RewardOverviews = returnData.RewardOverviews.OrderByDescending(p => p.Year).ThenByDescending(p => p.Month).ToList();
            rModel.Message = "查询成功";
            rModel.ReturnCode = (int)ReturnCode.Success;
            rModel.ReturnData = returnData;
            rModel.Signature = "";
            rModel.Token = model.Token;

            return rModel;
        }

        /// <summary>
        /// 活动邀请详情
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<InviteMonthDetail,string> InviteMonthDetails([FromBody]SMonthInvester model)
        {
            ReturnModel<InviteMonthDetail, string> rModel = new ReturnModel<InviteMonthDetail, string>();
            InviteMonthDetail returnData = new InviteMonthDetail();

            var resultSign = VerifyBase.SignAndToken<SMonthInvester>(model, out CST_user_info cstUserInfo);
            if (!resultSign.Equals(ReturnCode.Success))
            {
                rModel.ReturnCode = (int)resultSign;
                rModel.Message = "签名验证失败";
                return rModel;
            }

            var inviter = _invitationActivitie.GetCstUserInfos(cstUserInfo.cst_customer_id.Value, model.Year, model.Month);
            if(inviter!=null&& inviter.Any())
            {
                foreach(var info in inviter)
                {
                    var investerDetail = new InvesterDetail();
                    investerDetail.HeadPicUrl = info.cst_user_pic;
                    investerDetail.CertificationTime = "";
                    if (info.cst_account_id != null)
                    {
                        var temp = _accountInfoService.GetAccountInfoByUserId(userId:info.Id);
                        if (temp != null&&temp.JieSuan)
                        {
                            investerDetail.CertificationTime = temp.JieSuanTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                    }
                    investerDetail.RegisterTime = info.cst_add_date.Value.ToString("yyyy-MM-dd HH:mm:ss");
                    var investInfo = _invitationActivitie.GetGroupByInvestDate(cstUserInfo.cst_customer_id.Value, model.Month, model.Year, info.Id);
                    if (investInfo.Any())
                    {
                        investerDetail.InvestTime = investInfo.OrderBy(p => p.InvestDate).First().InvestDate.ToString("yyyy-MM-dd HH:mm:ss");
                        investerDetail.InvestMoney = investInfo.Sum(p => p.Money);
                    }
                    investerDetail.UserName = info.cst_user_name;
                    returnData.InvesterDetails.Add(investerDetail);
                    returnData.Count = returnData.InvesterDetails.Count;
                }
            }

            rModel.Message = "查询成功";
            rModel.ReturnCode = (int)ReturnCode.Success;
            rModel.ReturnData = returnData;
            rModel.Signature = "";
            rModel.Token = model.Token;
            return rModel;
        }
    }
}