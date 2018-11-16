using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZFCTAPI.Core;
using ZFCTAPI.Core.Enums;
using ZFCTAPI.Core.Provider;
using ZFCTAPI.Data.ApiModels.ReturnModels;
using ZFCTAPI.Data.ApiModels.SubmitModels;
using ZFCTAPI.Data.CST;
using ZFCTAPI.Data.PRO;
using ZFCTAPI.Services.InvestInfo;
using ZFCTAPI.Services.LoanInfo;
using ZFCTAPI.Services.Sys;
using ZFCTAPI.Services.UserInfo;
using ZFCTAPI.WebApi.RequestAttribute;
using ZFCTAPI.WebApi.Validates;

namespace ZFCTAPI.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [RequestLog]
    public class TransferController
    {
        private static ITransInfoService _transInfoService;
        private static ILoanInfoService _loanInfoService;
        private static IInvesterPlanService _investerPlanService;
        private static ILoanPlanService _loanPlanService;
        private static IInvestInfoService _investInfoService;
        private static ITransApplyService _transApplyService;
        private static IAccountInfoService _accountInfoService;
        private static ICstUserInfoService _userInfoService;
        private static IProTranferApplyService _transferApplyService;

        public TransferController(ITransInfoService transInfoService,
            ILoanInfoService loanInfoService,
            IInvesterPlanService investerPlanService,
            ILoanPlanService loanPlanService,
            IInvestInfoService investInfoService,
            ITransApplyService transApplyService,
            IAccountInfoService accountInfoService,
            ICstUserInfoService userInfoService,
            IProTranferApplyService transferApplyService)
        {
            _transInfoService = transInfoService;
            _loanInfoService = loanInfoService;
            _investerPlanService = investerPlanService;
            _loanPlanService = loanPlanService;
            _investInfoService = investInfoService;
            _transApplyService = transApplyService;
            _accountInfoService = accountInfoService;
            _userInfoService = userInfoService;
            _transferApplyService = transferApplyService;
        }

        /// <summary>
        /// 可投债权转让列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<ReturnPageData<RTansferAbstract>, string> TransferCanInvestList([FromBody]BasePageModel model)
        {
            var retModel = new ReturnModel<ReturnPageData<RTansferAbstract>, string>();
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

            var transList = new List<RTansferAbstract>();
            var transPage = _transInfoService.TransInfoPage(1,model.Page, model.PageSize);
            if (transPage.Items.Any())
            {
                transList.AddRange(transPage.Items.Select(GetTransferAbstract));
            }

            var rePage = new ReturnPageData<RTansferAbstract>
            {
                PagingData = transList,
                Total = transPage.TotalNum,
                TotalPageCount = transPage.TotalPageCount
            };
            retModel.Message = "查询成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.ReturnData = rePage;
            retModel.Token = model.Token;
            return retModel;
        }

        /// <summary>
        /// 债权转让详情
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RTansferDetail, string> TransferDetail([FromBody]STransferDetail model)
        {
            var retModel = new ReturnModel<RTansferDetail, string>();
            #region 校验签名
            var signResult = VerifyBase.Sign<STransferDetail>(model);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            #endregion

            var infos = _transApplyService.Find(model.TransferId);

            var result = new RTansferDetail();
            if (infos != null)
            {
                result = GetTransferDetail(infos);
            }

            retModel.Message = "查询成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.ReturnData = result;
            retModel.Token = model.Token;
            return retModel;
        }

        /// <summary>
        /// 债券转让投资人
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RLoanInvester, string> TransferInvester([FromBody]STransferDetail model)
        {
            var retModel = new ReturnModel<RLoanInvester, string>();
            #region 校验签名
            var signResult = VerifyBase.Sign<STransferDetail>(model);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            #endregion
            var result = new RLoanInvester();
            var invester = _investInfoService.GetInvestListByLoanId(model.TransferId, DataDictionary.investType_transfer);
            if (invester != null && invester.Any())
            {
                foreach (var invest in invester)
                {
                    var accountInfo = _accountInfoService.GetAccountInfoByUserId(invest.pro_invest_emp.Value);
                    result.Invester = accountInfo.act_legal_name.Substring(0, 1) + "***";
                    result.InvestMoney = invest.pro_invest_money.Value;
                    result.InvestTime = invest.pro_invest_date.Value;
                    result.InvestState = "投资成功";
                }
            }
            retModel.Message = "查询成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.ReturnData = result;
            retModel.Token = model.Token;
            return retModel;
        }

        /// <summary>
        /// 债权转让收益
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<string, string> InvestIncome([FromBody]STransferDetail model)
        {
            //返回模型
            var retModel = new ReturnModel<string, string>();
            #region 校验签名
            var signResult = VerifyBase.Sign<STransferDetail>(model);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            #endregion
            #region 验Token

            var tokenResult = VerifyBase.Token(model.Token, out CST_user_info _);
            if (tokenResult == ReturnCode.TokenEorr)
            {
                retModel.Message = "登录过期";
                retModel.ReturnCode = (int)ReturnCode.TokenEorr;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                retModel.Signature = "";
                return retModel;
            }
            #endregion

            var transinfo = _transferApplyService.Find(model.TransferId);
            //获取未完成的还款计划

            var investPlan =
                _investerPlanService.GetListByCondition(false,transinfo.pro_invest_id.Value);
            var allMoney = investPlan.Sum(p => p.pro_pay_total) ?? 0.0m;
            var result = allMoney - transinfo.pro_transfer_money.Value;
            retModel.Message = "查询成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.ReturnData =result.ToString();
            retModel.Token = model.Token;
            retModel.Signature = "";
            return retModel;
        }
        /// <summary>
        /// 投资债转
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RToInvestResult, string> InvestTransfer([FromBody]SInvestTransfer model)
        {
            var retModel = new ReturnModel<RToInvestResult, string>();

            #region 校验签名

            var signResult = VerifyBase.SignAndToken<BaseSubmitModel>(model, out CST_user_info cstUserInfo);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名


            var transferInfo = _transferApplyService.Find(model.TransferId);
            if (transferInfo.pro_is_use)
            {
                retModel.Message = "数据正在处理中,请稍后再试";
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.ReturnData = null;
                retModel.Signature = "";
                retModel.Token = "";
                return retModel;
            }
            else
            {
                //transferInfo.pro_is_use = true;
                //_transferApplyService.Update(transferInfo);
                //var result = InvestTransferOperate(model, retModel, user, investMoney, loanId, ref pResultInvest, transferInfo);
                //transferInfo.pro_is_use = false;
                //_transferApplyService.Update(transferInfo);

                //return result;
            }
            return null;
        }

        #region 数据转换
        private RTansferAbstract GetTransferAbstract(PRO_transfer_apply model)
        {
            var loanInfo = _loanInfoService.Find(model.pro_loan_id.Value);
            var result = new RTansferAbstract
            {
                LoanId = loanInfo.Id,
                LoanName = loanInfo.pro_loan_use,
                RemainPeriod = model.pro_transfer_period.Value,
                RepaymentType = InterestProvider.LoadInterestProviderGetByFriendlyName(loanInfo.pro_pay_type),
                RepeymentTypeCode = loanInfo.pro_pay_type,
                SurplusMoney = model.pro_transfer_money.Value,
                TransferId = model.Id,
                TransferStateId = model.pro_transfer_state.Value,
                TransferState = GetLoanState(model.pro_transfer_state.Value),
                ProductType = _loanInfoService.GetProductInfo(loanInfo.pro_prod_typeId.Value).pdo_product_name
            };
            result.YearRate = TransferRate(model);
            result.RemainDay = RemainDay(model);
            var notPayed = _loanPlanService.GetLoanPlansByCondition(loanInfo.Id).OrderBy(p => p.pro_pay_date)
                .FirstOrDefault(p => !p.pro_is_clear);
            result.NextPayDay = notPayed == null ? "已结清" : notPayed.pro_pay_date.Value.ToString("yyyy-MM-dd");
            return result;

        }
        /// <summary>
        /// 获取标记状态
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private string GetLoanState(int code)
        {
            if (code == DataDictionary.transferstatus_HasThrough)
            {
                return "立即认购";
            }
            else if (code == DataDictionary.transferstatus_FullScalePending)
            {
                return "满标待审";
            }
            else if (code == DataDictionary.transferstatus_StayTransfer)
            {
                return "待划转";
            }
            else if (code == DataDictionary.transferstatus_HasTransfer)
            {
                return "已划转";
            }
            return "";
        }
        /// <summary>
        /// 债权转让利率
        /// </summary>
        /// <param name="model">PRO_transfer_apply</param>
        /// <returns></returns>
        private decimal TransferRate(PRO_transfer_apply model)
        {
            var transinfo = model;
            //获取未完成的还款计划
            var investPlan =
                _investerPlanService.GetListByCondition(false,model.pro_invest_id.Value);

            if (investPlan.Count == 0)
            {
                return model.pro_transfer_rate.Value;
            }
            var allMoney = investPlan.Sum(p => p.pro_pay_total) ?? 0.0m;
            var lastPayday = investPlan.OrderByDescending(p => p.pro_pay_date).First().pro_pay_date;
            var numerator = (allMoney - transinfo.pro_transfer_money);
            var numerator2 = numerator * 360;
            var remainDay = (lastPayday - DateTime.Now).Value.Days;
            var denominator = transinfo.pro_transfer_money * remainDay;
            var result = (numerator2 / denominator) * 100;
            var rate = Math.Round(result.Value, 2, MidpointRounding.AwayFromZero);
            return rate;
        }
        /// <summary>
        /// 剩余天数
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private int RemainDay(PRO_transfer_apply model)
        {
            var lastPlan = _loanPlanService.GetLastLoanPlan(model.pro_loan_id.Value);
            //获取未完成的还款计划
            if (model.pro_transfer_state.Value == (int)DataDictionary.transferstatus_HasThrough)
            {
                //获取最后一期的还款计划
                if (lastPlan != null)
                {
                    return (lastPlan.pro_pay_date.Value - DateTime.Now).Days;
                }
            }
            else//已经被投资的债权转让
            {
                //获取投标信息
                if (model.pro_transfer_state.Value == (int)DataDictionary.transferstatus_FullScalePending ||
                    model.pro_transfer_state.Value == DataDictionary.transferstatus_StayTransfer)
                {
                    //获取最后一期的还款计划
                    //投资未成功
                    var investInfo = _investInfoService.GetInvestInfoByCondition(null,null,transferId:model.Id);
                    if (investInfo != null && lastPlan != null)
                    {
                        return (lastPlan.pro_pay_date.Value - investInfo.pro_invest_date.Value).Days;
                    }
                }
                else if (model.pro_transfer_state.Value == (int)DataDictionary.transferstatus_HasTransfer)
                {
                    //投资成功
                    var investInfo = _investInfoService.GetInvestInfoByCondition(true,false,transferId:model.Id);
                    if (investInfo != null && lastPlan != null)
                    {
                        return (lastPlan.pro_pay_date.Value - investInfo.pro_invest_date.Value).Days;
                    }
                }
            }
            return 0;
        }
        /// <summary>
        /// 标的下个还款日
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private string NextPayDay(PRO_transfer_apply model)
        {
            var loanPlan = _loanPlanService.GetLoanPlansByCondition(model.pro_loan_id.Value);
            //刨除平台代还带情况下
            var notPayed = loanPlan.OrderBy(p => p.pro_pay_date).FirstOrDefault(p => !p.pro_is_clear&&p.pro_pay_type!= DataDictionary.RepaymentType_PlatformDaihuan);
            return notPayed == null ? "已结清" : notPayed.pro_pay_date.Value.ToString("yyyy-MM-dd");
        }
        /// <summary>
        /// 获取债转详情
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private RTansferDetail GetTransferDetail(PRO_transfer_apply model)
        {
            var loanInfo = _loanInfoService.Find(model.pro_loan_id.Value);
            var accoutInfo = _accountInfoService.GetAccountInfoByUserId(model.pro_user_id.Value);
            var userInfo = _userInfoService.Find(model.pro_user_id.Value);
            var result = new RTansferDetail
            {
                LoanId = loanInfo.Id,
                LoanName = loanInfo.pro_loan_use,
                RemainPeriod = model.pro_transfer_period.Value,
                RepaymentType = InterestProvider.LoadInterestProviderGetByFriendlyName(loanInfo.pro_pay_type),
                RepeymentTypeCode = loanInfo.pro_pay_type,
                SurplusMoney = model.pro_transfer_money.Value,
                TransferId = model.Id,
                TransferStateId = model.pro_transfer_state.Value,

                TransferState = GetLoanState(model.pro_transfer_state.Value),
                ProductType = _loanInfoService.GetProductInfo(loanInfo.pro_prod_typeId.Value).pdo_product_name,
                TransferName = accoutInfo.act_legal_name.Substring(0, 1) + "***",
                LoanRate = loanInfo.pro_loan_rate.Value
            };
            var cardNum = accoutInfo.act_user_card;
            result.TransferUserCardId = cardNum.Substring(0, 4) + "***" + cardNum.Substring(cardNum.Length - 4);
            result.TransferUserName = userInfo.cst_user_name.Substring(0, 3) + "******";
            result.TransferUserPhone = userInfo.cst_user_phone.Substring(0, 3) + "***";
            result.InvestMoney = model.investInfo.pro_invest_money.Value;
            result.Period = loanInfo.pro_loan_period.Value;
            result.NextPayDay = NextPayDay(model);
            result.YearRate = TransferRate(model);
            result.RemainDay = RemainDay(model);

            return result;

        }
        #endregion

        #region 投资债权转让

        private RToInvestResult InvestTransferOperate(SInvestTransfer model,PRO_transfer_apply transferInfo)
        {

            return null;
        }

        #endregion
    }
}
