using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZFCTAPI.Data.ApiModels.ReturnModels;
using ZFCTAPI.Data.ApiModels.SubmitModels;
using ZFCTAPI.Services.Promotion;
using ZFCTAPI.Core.Enums;
using ZFCTAPI.Data.CST;
using ZFCTAPI.WebApi.Validates;
using ZFCTAPI.Services.BoHai;
using ZFCTAPI.Data.BoHai.SubmitModels;
using ZFCTAPI.WebApi.RequestAttribute;

namespace ZFCTAPI.WebApi.Controllers
{
    /// <summary>
    /// 活动
    /// </summary>
    [Route("api/[controller]/[action]")]
    [RequestLog]
    public class ActivityController : Controller
    {
        private readonly IActivityStatsService _iactivityStatsService;
        private readonly IBHRepaymentService _ibhrepaymentService;
        public ActivityController(IActivityStatsService iactivityStatsService,
            IBHRepaymentService ibhrepaymentService)
        {
            this._iactivityStatsService = iactivityStatsService;
            _ibhrepaymentService = ibhrepaymentService;
        }


        #region 活动接口

        /// <summary>
        /// 投资排行榜
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<List<RActivityRankModel>, string> ActivityRank([FromBody]SActivityModel model)
        {
            ReturnModel<List<RActivityRankModel>, string> rModel = new ReturnModel<List<RActivityRankModel>, string>();
            var verifyResult = VerifyBase.Sign<SActivityModel>(model);
            if (verifyResult != ReturnCode.Success)
            {
                rModel.ReturnCode = (int)verifyResult;
                rModel.Message = "签名验证失败";
                return rModel;
            }
            rModel.ReturnData = _iactivityStatsService.GetActivitiesStateByActId(model.ActivityId, model.Count.Value);
            rModel.ReturnCode = (int)ReturnCode.Success;
            rModel.Message = "查询成功";
            return rModel;
        }

        /// <summary>
        /// 活动奖励
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RInvestRewardModel, string> ActivityReward([FromBody]SActivityModel model)
        {
            ReturnModel<RInvestRewardModel, string> rModel = new ReturnModel<RInvestRewardModel, string>();
            var verifyResult = VerifyBase.SignAndToken<SActivityModel>(model, out CST_user_info user);
            if (verifyResult != ReturnCode.Success)
            {
                rModel.ReturnCode = (int)verifyResult;
                rModel.Message = "签名验证失败";
                return rModel;
            }
            //获取用户活动投资金额
            var result = _iactivityStatsService.GetActivityInvest(model.ActivityId, user.Id);

            if (result != null)//根据活动(ActivityId)和用户的投资金额，返回奖励
            {
            }

            rModel.ReturnData = result;
            rModel.ReturnCode = (int)ReturnCode.Success;
            rModel.Message = "查询成功";
            return rModel;
        }

        /// <summary>
        /// 最新投资记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<List<RInvestRecordsModel>, string> InvestRecords([FromBody]SInvestRecordsModel model)
        {
            ReturnModel<List<RInvestRecordsModel>, string> rModel = new ReturnModel<List<RInvestRecordsModel>, string>();
            rModel.ReturnData = _iactivityStatsService.GetInvestRecords(model.Count);
            rModel.ReturnCode = (int)ReturnCode.Success;
            rModel.Message = "查询成功";
            return rModel;
        }

        #endregion
    }
}