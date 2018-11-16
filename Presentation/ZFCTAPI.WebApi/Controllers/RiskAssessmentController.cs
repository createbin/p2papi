using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using ZFCTAPI.Core.Enums;
using ZFCTAPI.Data.ApiModels.ReturnModels;
using ZFCTAPI.Data.ApiModels.SubmitModels;
using ZFCTAPI.Data.CST;
using ZFCTAPI.Data.RiskAssessment;
using ZFCTAPI.Services.RiskAssessment;
using ZFCTAPI.WebApi.RequestAttribute;
using ZFCTAPI.WebApi.Validates;

namespace ZFCTAPI.WebApi.Controllers
{
    /// <summary>
    /// 风险测评
    /// </summary>
    [Route("api/[controller]/[action]")]
    [RequestLog]
    public class RiskAssessmentController : Controller
    {
        private readonly IRiskAssessmentService _riskAssessmentService;
        private readonly IUserInvestTypeService _userInvestTypeService;
        private readonly IRiskByUserService _riskByUserService;

        public RiskAssessmentController(IRiskAssessmentService riskAssessmentService,
            IUserInvestTypeService userInvestTypeService,
            IRiskByUserService riskByUserService)
        {
            this._riskAssessmentService = riskAssessmentService;
            this._userInvestTypeService = userInvestTypeService;
            this._riskByUserService = riskByUserService;
        }

        /// <summary>
        /// 获得问卷
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<List<RiskQuestionnaireModel>,string> RiskQuestionnaire([FromBody]BaseSubmitModel model)
        {
            var rModel = new ReturnModel<List<RiskQuestionnaireModel>, string>();
            var version = _riskAssessmentService.GetMaxVersion();
            var returnData = _riskAssessmentService.GetQuestionsByVersion(version).Select(p =>
            {
                var item = new RiskQuestionnaireModel();
                item.Qid = p.Id;
                item.Description = p.Describe;
                item.Answer = _riskAssessmentService.GetRiskAnswer(p.Id).OrderBy(s => s.Sort).Select(s =>
                {
                    var answerItem = new RiskQuestionnaireAnswerModel();
                    answerItem.Aid = s.Id;
                    answerItem.Description = s.Describe;
                    answerItem.Integral = s.Score;
                    return answerItem;
                }).ToList();
                return item;
            });

            rModel.ReturnData = returnData.ToList();
            rModel.Message = "查询成功！";
            rModel.ReturnCode = (int)ReturnCode.Success;
            rModel.Token = model.Token;
            return rModel;
        }

        /// <summary>
        /// 获取用户投资类型
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<string, string> UserInvestType([FromBody]BaseSubmitModel model)
        {
            var rModel = new ReturnModel<string, string>();

            var resultSign = VerifyBase.SignAndToken<BasePageModel>(model, out CST_user_info cstUserInfo);
            if (!resultSign.Equals(ReturnCode.Success))
            {
                rModel.ReturnCode = (int)resultSign;
                rModel.Message = "签名验证失败";
                return rModel;
            }

            var investType = _userInvestTypeService.GetUserTypes(cstUserInfo.Id);
            if (investType != null)
            {
                if (investType.cst_invest_type.Equals(1))
                    rModel.ReturnData = "保守型";
                if (investType.cst_invest_type.Equals(2))
                    rModel.ReturnData = "稳健型";
                if (investType.cst_invest_type.Equals(3))
                    rModel.ReturnData = "激进型";
            }

            rModel.ReturnCode = (int)ReturnCode.Success;
            rModel.Token = model.Token;
            rModel.Message = "查询成功";
            return rModel;
        }

        /// <summary>
        /// 提交用户问卷积分
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<bool, string> PostQuestionScore([FromBody]SQuestionScore model)
        {
            var rModel = new ReturnModel<bool, string>();

            var resultSign = VerifyBase.SignAndToken<BasePageModel>(model, out CST_user_info cstUserInfo);
            if (!resultSign.Equals(ReturnCode.Success))
            {
                rModel.ReturnCode = (int)resultSign;
                rModel.Message = "签名验证失败";
                return rModel;
            }

            var investtype = 0;
            if (model.score >= 10 && model.score <= 40)
                investtype = 1;
            else if (model.score > 40 && model.score <= 70)
                investtype = 2;
            else if (model.score > 70 && model.score <= 100)
                investtype = 3;
            else
            {
                rModel.ReturnData = false;
                rModel.ReturnCode = (int)ReturnCode.DataEorr;
                rModel.Token = model.Token;
                rModel.Message = "数据错误！";
                return rModel;
            }
            //更新用户所属投资类型
            _userInvestTypeService.DeleteInvesttypeByUserId(cstUserInfo.Id);
            //保存用户所属投资类型
            _userInvestTypeService.Add(new CST_user_investtype()
            {
                cst_user_id = cstUserInfo.Id,
                cst_invest_score = model.score,
                cst_invest_type = investtype,
                cst_create_time = DateTime.Now,
                cst_update_time = DateTime.Now
            });
            var investType = _userInvestTypeService.GetUserTypes(cstUserInfo.Id);
            //保存答案
            if (investType != null)
            {
                var answerList = new List<RiskByUser>();
                foreach (var item in model.answers)
                {
                    answerList.Add(new RiskByUser
                    {
                        AId = item.Aid,
                        CreateDate = DateTime.Now,
                        Score = item.Score,
                        UserId = investType.cst_user_id,
                        UserTypeId = investType.Id
                    });
                }
                if (answerList.Count > 0)
                {
                    _riskByUserService.Add(answerList);
                }
            }

            rModel.ReturnData = true;
            rModel.ReturnCode = (int)ReturnCode.Success;
            rModel.Token = model.Token;
            rModel.Message = "保存成功！";
            return rModel;
        }
    }
}