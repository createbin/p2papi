using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Mvc;
using ZFCTAPI.Core;
using ZFCTAPI.Core.Configuration;
using ZFCTAPI.Core.Enums;
using ZFCTAPI.Core.Helpers;
using ZFCTAPI.Core.Infrastructure;
using ZFCTAPI.Core.Provider;
using ZFCTAPI.Data.ApiModels.ReturnModels;
using ZFCTAPI.Data.ApiModels.SubmitModels;
using ZFCTAPI.Data.BoHai.SubmitModels;
using ZFCTAPI.Data.CST;
using ZFCTAPI.Data.Interest;
using ZFCTAPI.Data.Promotion;
using ZFCTAPI.Data.PRO;
using ZFCTAPI.Services.BoHai;
using ZFCTAPI.Services.Interest;
using ZFCTAPI.Services.InvestInfo;
using ZFCTAPI.Services.LoanInfo;
using ZFCTAPI.Services.Messages;
using ZFCTAPI.Services.Popular;
using ZFCTAPI.Services.Promotion;
using ZFCTAPI.Services.RiskAssessment;
using ZFCTAPI.Services.Transaction;
using ZFCTAPI.WebApi.Validates;
using ZFCTAPI.Services.UserInfo;
using ZFCTAPI.WebApi.RequestAttribute;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ZFCTAPI.WebApi.Controllers
{
    /// <summary>
    /// 投资相关api
    /// </summary>
    [Route("api/[controller]/[action]")]
    [RequestLog]
    public class InvestController : Controller
    {
        private static IInvestInfoService _investInfoService;
        private static ICstRedInfoService _redInfoService;
        private static IPopEnvelopeRedService _envelopeRedService;
        private static IInterestService _interestService;
        private readonly IAccountInfoService _accountInfoService;
        private readonly ILoanInfoService _loanInfoService;
        private readonly IBHAccountService _bhAccountService;
        private readonly IUserInvestTypeService _userInvestTypeService;
        private readonly ICstTransactionService _transactionService;
        private readonly IMessageNoticeService _msgNoticeService;
        private readonly IBHUserService _bhUserService;
        private readonly ICustomerService _customerService;
        private readonly IInvitationActivitie _inviteService;

        public InvestController(IInvestInfoService investInfoService,
            ICstRedInfoService redInfoService,
            IPopEnvelopeRedService envelopeRedService,
            IInterestService interestService,
            IAccountInfoService accountInfoService,
            ILoanInfoService loanInfoService,
            IBHAccountService bhAccountService,
            IUserInvestTypeService userInvestTypeService,
            ICstTransactionService transactionService,
            IMessageNoticeService msgNoticeService,
            IBHUserService bhUserService,
            ICustomerService customerService,
            IInvitationActivitie inviteService)
        {
            _investInfoService = investInfoService;
            _redInfoService = redInfoService;
            _envelopeRedService = envelopeRedService;
            _interestService = interestService;
            _accountInfoService = accountInfoService;
            _loanInfoService = loanInfoService;
            _bhAccountService = bhAccountService;
            _userInvestTypeService = userInvestTypeService;
            _transactionService = transactionService;
            _msgNoticeService = msgNoticeService;
            _bhUserService = bhUserService;
            _customerService = customerService;
            _inviteService = inviteService;
        }

        private static object InvestInfoAction = new object();

        /// <summary>
        /// 是否为新手
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<bool, string> IsNewHand([FromBody]BaseSubmitModel model)
        {
            //返回模型
            var retModel = new ReturnModel<bool, string>();

            #region 校验签名

            var signResult = VerifyBase.Sign<BaseSubmitModel>(model);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = false;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名

            #region 验Token

            CST_user_info userInfo;
            var tokenResult = VerifyBase.Token(model.Token, out userInfo);
            if (tokenResult == ReturnCode.TokenEorr)
            {
                retModel.Message = "登录过期！";
                retModel.ReturnCode = (int)ReturnCode.TokenEorr;
                retModel.ReturnData = false;
                retModel.Token = model.Token;
                retModel.Signature = "";
                return retModel;
            }

            #endregion 验Token

            try
            {
                int userid = userInfo.Id;   //当前登陆用户ID
                int investCount = _investInfoService.CountInvest(userid);
                retModel.Message = "成功";
                retModel.ReturnCode = (int)ReturnCode.Success;
                retModel.ReturnData = investCount <= 0;
                retModel.Token = model.Token;
                retModel.Signature = "";
                return retModel;
            }
            catch
            {
                retModel.Message = "发生错误";
                retModel.ReturnCode = (int)ReturnCode.DataFormatError;
                retModel.ReturnData = false;
                retModel.Token = model.Token;
                retModel.Signature = "";
                return retModel;
            }
        }

        /// <summary>
        /// 可用红包列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<List<RLoanRedPackages>, string> LoanRedPackage([FromBody]SLoanRedPackage model)
        {
            //返回模型
            var retModel = new ReturnModel<List<RLoanRedPackages>, string>();

            #region 校验签名

            var signResult = VerifyBase.Sign<SLoanRedPackage>(model);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名

            #region 校验token

            CST_user_info userInfo;
            var tokenResult = VerifyBase.Token(model.Token, out userInfo);
            if (tokenResult == ReturnCode.TokenEorr)
            {
                retModel.Message = "登录过期！";
                retModel.ReturnCode = (int)ReturnCode.TokenEorr;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                retModel.Signature = "";
                return retModel;
            }

            #endregion 校验token

            var userId = userInfo.cst_customer_id.Value;
            var redInfo = _envelopeRedService.GetRedEnvelopeLists(userId, model.LoanId, model.InvestMoney);
            var result = new List<RLoanRedPackages>();
            var resultCan = new RLoanRedPackages();
            var redInfoCan = redInfo.Where(p => p.IsCanUse).ToList();
            resultCan.LoanId = model.LoanId;
            resultCan.State = true;
            if (redInfoCan.Any())
            {
                var redPackage = redInfoCan.Select(red => new RRedPackage
                {
                    EndDate = red.ExpiryDate != null ? red.ExpiryDate.Value.ToString("yyyy-MM-dd") : "无有效期",
                    RedId = red.Id,
                    Condition = red.Introduction == "" ? "无使用限制" : red.Introduction,
                    RedMoney = red.Amount,
                    RedPackageState = 1,
                    RedName = red.Name
                }).ToList();
                resultCan.Count = redPackage.Count();
                resultCan.RedPackages = redPackage;
            }
            result.Add(resultCan);
            var redInfoNot = redInfo.Where(p => !p.IsCanUse).ToList();
            var resultNot = new RLoanRedPackages { LoanId = model.LoanId };
            if (redInfoNot.Any())
            {
                var redPackage = redInfoNot.Select(red => new RRedPackage
                {
                    EndDate = red.ExpiryDate != null ? red.ExpiryDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : "无有效期",
                    RedId = red.Id,
                    Condition = red.Introduction == "" ? "无使用限制" : red.Introduction,
                    RedMoney = red.Amount,
                    RedPackageState = 1,
                    RedName = red.Name
                }).ToList();
                resultNot.Count = redPackage.Count();
                resultNot.RedPackages = redPackage;
                resultNot.State = false;
            }
            result.Add(resultNot);
            retModel.Message = "查询成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.ReturnData = result;
            retModel.Signature = "";
            retModel.Token = model.Token;
            return retModel;
        }

        /// <summary>
        /// 获取最匹配红包
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RRedPackage, string> BestLoanRedPackage([FromBody]SLoanRedPackage model)
        {
            //返回模型
            var retModel = new ReturnModel<RRedPackage, string>();

            #region 校验签名

            var signResult = VerifyBase.Sign<SLoanRedPackage>(model);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名

            #region 校验token

            CST_user_info userInfo;
            var tokenResult = VerifyBase.Token(model.Token, out userInfo);
            if (tokenResult == ReturnCode.TokenEorr)
            {
                retModel.Message = "登录过期！";
                retModel.ReturnCode = (int)ReturnCode.TokenEorr;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                retModel.Signature = "";
                return retModel;
            }

            #endregion 校验token

            var redList = _envelopeRedService.GetRedEnvelopeIsAvailable(GebruiksType.Invest,
                (int)userInfo.cst_customer_id, model.LoanId, model.InvestMoney);
            var result = new RRedPackage();
            if (redList != null && redList.Any())
            {
                var resultRed = redList.OrderByDescending(p => p.Amount).First();
                result.EndDate = resultRed.ExpiryDate != null
                    ? resultRed.ExpiryDate.Value.ToString("yyyy-MM-dd")
                    : "无有效期";
                result.RedId = resultRed.Id;
                result.Condition = "";
                result.RedMoney = resultRed.Amount;
                result.RedPackageState = 1;
                result.RedName = resultRed.Name;
            }
            retModel.Message = "查询成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.ReturnData = result;
            retModel.Signature = "";
            retModel.Token = model.Token;
            return retModel;
        }

        /// <summary>
        /// 获取可用红包数量
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<int, string> AvaliableRedCount([FromBody]BaseSubmitModel model)
        {
            var retModel = new ReturnModel<int, string>();

            #region 校验签名

            var signResult = VerifyBase.Sign<BaseSubmitModel>(model);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = 0;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名

            #region 验Token

            var tokenResult = VerifyBase.Token(model.Token, out CST_user_info userInfo);
            if (tokenResult == ReturnCode.TokenEorr)
            {
                retModel.Message = "登录过期！";
                retModel.ReturnCode = (int)ReturnCode.TokenEorr;
                retModel.ReturnData = 0;
                retModel.Token = model.Token;
                retModel.Signature = "";
                return retModel;
            }

            #endregion 验Token

            var avialiableCount = _redInfoService.AvaliableRedCount(userInfo.cst_customer_id.Value);
            retModel.Message = "查询成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.ReturnData = avialiableCount;
            retModel.Token = model.Token;
            return retModel;
        }

        /// <summary>
        /// 投资收益
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<string, string> InvestIncome([FromBody]SInvestIncome model)
        {
            var retModel = new ReturnModel<string, string>();

            #region 校验签名

            var signResult = VerifyBase.Sign<SInvestIncome>(model);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名

            var ent = new CalculateCreditorRequest
            {
                RepaymentType = InterestProvider.GetInterest(model.RepaymentType),
                LoanAmount = Convert.ToDecimal(model.InvestMoney),
                LoanRate = Convert.ToDecimal(model.LoanRate),
                RepaymentPeriods = Convert.ToInt32(model.DeadLine),
                LoanDurTime = Convert.ToInt32(model.DeadLine),
                InterestBearing = Convert.ToInt32(model.InType),
                SettlementWay = Convert.ToInt32(DataDictionary.settlementway_Fixed),
                ManagementFee = Convert.ToDecimal(0.000001),
                ChargeWay = Convert.ToInt32(DataDictionary.chargeway_Proportion),
                BillDay = Convert.ToInt32(model.BillDay)
            };
            if (model.RepaymentType == "Interests.WithBenefitClear")
            {
                ent.SettlementWay = Convert.ToInt32(DataDictionary.settlementway_NotFixed);
            }

            var intrest = _interestService.GenerateCreditor(ent);

            //利息
            var interestMoney = intrest.ProLoanPlans.Sum(s => s.pro_pay_rate);
            retModel.Message = "查询成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.ReturnData = interestMoney == null ? "0.00" : interestMoney.Value.ToString("0.00");
            retModel.Token = model.Token;
            return retModel;
        }

        /// <summary>
        /// 投资标
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RToInvestResult, string> InvestLoan([FromBody]SInvestLoan model)
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

            //获取用户开户信息
            var accountInfo = _accountInfoService.GetAccountInfoByUserId(userId: cstUserInfo.Id);
            if (accountInfo == null||accountInfo.JieSuan==false||accountInfo.BoHai==false)
            {
                retModel.Message = "用户尚未完成开户或绑卡";
                retModel.ReturnCode = (int)ReturnCode.UnOpenAccount;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            if (!_bhUserService.IsAuth(accountInfo.cst_plaCustId, AuthTyp.Invest))
            {
                retModel.Message = "用户尚未完成授权";
                retModel.ReturnCode = (int)ReturnCode.UnOpenAccount;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            var loaninfo = _loanInfoService.Find(model.LoanId);
            if (loaninfo == null)
            {
                retModel.Message = "标的信息不存在";
                retModel.ReturnCode = (int)ReturnCode.DataFormatError;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            var investMoney = model.Money;
            //投资金额必须为100的整数倍
            if (investMoney % 100 != 0)
            {
                retModel.Message = "投资金额必须是100的整数倍";
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.ReturnData = null;
                retModel.Signature = "";
                retModel.Token = model.Token;
                return retModel;
            }
            //判断新手标
            if (loaninfo.pro_is_new)
            {
                var fisrtInvest = _investInfoService.ValidateFirstInvest(cstUserInfo.Id);
                if (fisrtInvest==false)
                {
                    retModel.Message = "已投资过新手标，不能再投资";
                    retModel.ReturnCode = (int)ReturnCode.DataEorr;
                    retModel.ReturnData = null;
                    retModel.Signature = "";
                    retModel.Token = model.Token;
                    return retModel;
                }
            }

            if (loaninfo.pro_loan_state == DataDictionary.projectstate_StayRelease || loaninfo.pro_loan_state == DataDictionary.projectstate_FullScalePending
                || loaninfo.pro_loan_state == DataDictionary.projectstate_Settled || loaninfo.pro_loan_state == DataDictionary.projectstate_StayTransfer
                || loaninfo.pro_loan_state == DataDictionary.projectstate_FlowStandard || loaninfo.pro_loan_state == DataDictionary.projectstate_StayPlatformaudit)
            {
                retModel.Message = "此项目状态已变更，无法投资";
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.ReturnData = null;
                retModel.Signature = "";
                retModel.Token = model.Token;
                return retModel;
            }

            if (loaninfo.pro_is_use)
            {
                retModel.Message = "数据正在处理中,请稍后再试！";
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.ReturnData = null;
                retModel.Signature = "";
                retModel.Token = model.Token;
                return retModel;
            }
            else
            {
                loaninfo.pro_is_use = true;
                _loanInfoService.Update(loaninfo);
                var result = new RToInvestResult();
                try
                {
                    result = InvestLoanOperate(cstUserInfo, accountInfo, loaninfo, investMoney,model.RequestSource, model.RedId);
                }
                catch(Exception ex)
                {
                    LogsHelper.WriteLog(ex.Message);
                    loaninfo.pro_is_use = false;
                    _loanInfoService.Update(loaninfo);
                    retModel.Message = "发生错误";
                    retModel.ReturnCode = (int)ReturnCode.DataEorr;
                    retModel.ReturnData = result;
                    retModel.Signature = "";
                    retModel.Token = model.Token;
                    return retModel;
                }
                loaninfo.pro_is_use = false;
                _loanInfoService.Update(loaninfo);
                #region 处理投标结果

                if (result.ErrorCode == "0000")
                {
                    retModel.Message = "成功";
                    retModel.ReturnCode = (int)ReturnCode.Success;
                }
                else
                {
                    retModel.Message =result.ErrorInfo;
                    retModel.ReturnCode = (int)ReturnCode.DataEorr;
                }

                retModel.ReturnData = result;
                retModel.Signature = "";
                retModel.Token = model.Token;
                return retModel;
                #endregion
            }
        }

        private RToInvestResult InvestLoanOperate(CST_user_info userInfo,CST_account_info accountInfo,PRO_loan_info loanInfo,decimal investMoney,int requestSource,int redId=0)
        {
            var redInfo=new CST_red_info();
            var result = new RToInvestResult();
            var redMoney = 0.00m;
            #region 是否大于投标日期

            if (loanInfo.pro_invest_startTime > DateTime.Now)
            {
                result.ErrorInfo = "未到招标开始日期，不能投资！";
                return result;
            }
            #endregion
            #region 投资的金额不能大于可投金额
            if (loanInfo.pro_surplus_money < investMoney)
            {
                result.ErrorInfo = "投资的金额不能大于可投金额！";
                return result;
            }
            #endregion

            #region 自己融资的项目不允许投资
            if (loanInfo.pro_add_emp == userInfo.Id)
            {
                result.ErrorInfo = "自己融资项目不可以投资！";
                return result;
            }
            #endregion
            #region 是否允许复投
            if (loanInfo.pro_is_much == false)
            {
                var investinfo = _investInfoService.UserInvestInfos(userInfo.Id).Where(p => p.pro_loan_id == loanInfo.Id).ToList();
                if (investinfo.Count > 0)
                {
                    result.ErrorInfo = "该项目已经投资，不能重复投资!";
                    return result;
                }
            }
            #endregion
            #region 最小投资金额限制
            if (loanInfo.pro_min_invest_money != null && investMoney < loanInfo.pro_min_invest_money)
            {
                if (loanInfo.pro_surplus_money<loanInfo.pro_min_invest_money)
                {//可投
                }
                else
                {
                    result.ErrorInfo = "投资金额不能小于最小投资金额" + Math.Round((decimal)loanInfo.pro_min_invest_money, 2) + "元！";
                    return result;
                }

            }
            #endregion

            #region 最大投资金额限制
            if (loanInfo.pro_max_invest_money != null && investMoney > loanInfo.pro_max_invest_money)
            {
                result.ErrorInfo = "投资金额不能大于最大投资金额" + Math.Round((decimal)loanInfo.pro_max_invest_money, 2) + "元！";
                return result;
            }
            #endregion

            #region 红包多次限制
            if (loanInfo.pro_much_red == false && redId > 0)
            {
                var redInvestInfo = _investInfoService.UserInvestWithRed(loanInfo.Id,userInfo.Id);

                if (redInvestInfo.Count > 0)
                {
                    result.ErrorInfo = "此项目你已使用过红包，不可多次使用！";
                    return result;
                }
            }
            #endregion

            #region 红包限制
            if (redId != 0)
            {
                redInfo = _redInfoService.Find(redId);
                redMoney = redInfo.cst_red_money.Value;
                if (redInfo.cst_red_employ)
                {
                    result.ErrorInfo = "该红包已使用,无法继续投资！";
                    return result;
                }
            }

            if (loanInfo.pro_max_red != null)
            {
                decimal usedRedMoney = _redInfoService.UserInvestWithRed(loanInfo.Id,userInfo.Id).Sum(p=>(decimal)p.cst_red_money.GetValueOrDefault());
                if (loanInfo.pro_max_red < usedRedMoney+redMoney)
                {
                    result.ErrorInfo = "该项目累计使用红包金额不能超过" + loanInfo.pro_max_red + "元";
                    return result;
                }

            }

            if (investMoney <= redMoney)
            {
                result.ErrorInfo = "红包金额必须小于投资金额！";
                return result;
            }
            if (loanInfo.pro_surplus_money <= redMoney)
            {
                result.ErrorInfo = "使用红包总金额不能多于可投金额，现可投金额为：" + loanInfo.pro_surplus_money;
                return result;
            }
            #endregion

            //查询用户信息
            //调用渤海接口
            var accountBalance = _bhAccountService.AccountQueryAccBalance(new SBHAccountQueryAccBalance
            {
                SvcBody = new SBHAccountQueryAccBalanceBody
                {
                    platformUid = accountInfo.invest_platform_id
                }
            });
            if (accountBalance == null || Convert.ToDecimal(accountBalance.SvcBody.withdrawAmount) < investMoney)
            {
                result.ErrorInfo = "可用余额不足，不能投资";
                return result;
            }
            else
            {
                var guid = Guid.NewGuid();
                var orderNo = CommonHelper.GetMchntTxnSsn();
                var investitems = new PRO_invest_info();
                using (TransactionScope ts = new TransactionScope())
                {
                    #region 生成投资记录，更新项目可投金额（项目行锁，更新可投金额字段，事务处理）
                    investitems.pro_loan_id = loanInfo.Id;
                    investitems.pro_invest_emp = userInfo.Id;
                    investitems.pro_invest_money = investMoney;
                    investitems.pro_credit_money = investMoney;
                    investitems.pro_invest_date = DateTime.Now;
                    investitems.pro_frozen_state = false;
                    investitems.pro_transfer_state = false;
                    investitems.pro_invest_type = DataDictionary.investType_normal;
                    investitems.InvestSource = requestSource;
                    investitems.pro_invest_guid = guid;
                    investitems.is_onlyPhone = false;
                    investitems.is_invest_succ = false;
                    //实际支付金额，是用户投资金额-红包金额，是实际需要从用户账户扣除的金额
                    investitems.pro_creditPay_money = investMoney;
                    //添加用户当前的理财经理
                    investitems.WealthManagerId = userInfo.cst_recommend_userId;
                    //var investType = _userInvestTypeService.GetUserTypes(userInfo.Id);

                    //添加用户风险类型 by gaochao 2018 02 28
                    //investitems.pro_invester_risktype = investType.cst_invest_type;
                    investitems.pro_order_no = orderNo;
                    _investInfoService.Add(investitems);


                    ts.Complete();
                    #endregion
                }
                var investResult = _bhAccountService.ClaimsPurchase(new SBHClaimsPurchase
                {
                    SvcBody = new SBHClaimsPurchaseBody
                    {
                        projectCode = loanInfo.Id.ToString(),
                        projectType = ProjectType.Creditor,
                        purchaseNumber = orderNo,
                        investorPlatformCode = accountInfo.invest_platform_id,
                        platformAgent = "0",
                        purchaseMoney = investMoney.ToString("0.00"),
                        purchaseIP = ZfctWebEngineToConfiguration.GetIPAddress()
                    }
                });
                result.ErrorCode = investResult.RspSvcHeader.returnCode;
                result.ErrorInfo = investResult.RspSvcHeader.returnMsg;
                investitems = _investInfoService.GetInvestInfoByInvestGuid(guid);
                if (investResult.RspSvcHeader.returnCode == JSReturnCode.Success)
                {
                    result.ErrorInfo = "投资成功";
                    result.ErrorCode = "0000";

                    #region 投资成功后才对标的进行更新处理

                    //更新项目可投金额
                    loanInfo.pro_surplus_money -= investMoney;
                    loanInfo.pro_loan_speed =
                        decimal.Round(
                            (decimal) (((loanInfo.pro_loan_money - loanInfo.pro_surplus_money) /
                                        loanInfo.pro_loan_money) * 100), 2);
                    if (redId != 0)
                    {
                        redInfo.cst_red_investId = investitems.Id;
                        _redInfoService.Update(redInfo);
                    }
                    lock (InvestInfoAction)
                    {
                        _loanInfoService.Update(loanInfo);
                    }

                    #endregion

                    //处理标的数据
                    HandleInvest(investResult.SvcBody.transId, investResult.SvcBody.freezeId, investitems, loanInfo, userInfo);
                }
                else
                {
                    investitems.is_invest_succ = false;
                    investitems.pro_delsign = true;
                    _investInfoService.Update(investitems);
                }
                return result;
            }
        }

        #region InvestLoan

        private void HandleInvest(string bhNo, string fzNo, PRO_invest_info investInfo, PRO_loan_info loanInfo,CST_user_info userInfo)
        {
            //获取标的所有投标信息
            var loanList = _investInfoService.GetLoanSuccessInevst(loanInfo.Id);
            var sumInvestMoney = loanList.Sum(p => p.pro_invest_money) + investInfo.pro_invest_money;
            if (loanInfo.pro_loan_money == sumInvestMoney)
            {
                loanInfo.pro_loan_state = DataDictionary.projectstate_FullScalePending;
                loanInfo.pro_full_date = DateTime.Now.Date;
                #region 发送短信给运营人员通知已经满标
                //获取运营人员手机号
                var phoneNumber = EngineContext.Current.Resolve<CommonConfig>().ManagerPhone;
                if (string.IsNullOrEmpty(phoneNumber))
                {
                    phoneNumber = EngineContext.Current.Resolve<CommonConfig>().ManagerPhone;
                }
                var content = loanInfo.pro_loan_no + "已满标，请尽快查看";
                _msgNoticeService.SendMsgNotice(phoneNumber, content);
                #endregion
                _loanInfoService.Update(loanInfo);
            }
            investInfo.pro_order_no +="|"+bhNo ;
            investInfo.pro_fro_orderno = fzNo;
            investInfo.pro_frozen_state = true;
            investInfo.is_invest_succ = true;
            _investInfoService.Update(investInfo);

            #region 插入交易记录
            if (investInfo.pro_invest_type == DataDictionary.investType_normal)
            {
                var transactionDescription = "正常冻结" + loanInfo.pro_loan_no + "项目的投资金额";
                var trans = new CST_transaction_info
                {
                    pro_user_id = investInfo.pro_invest_emp,
                    pro_loan_id = loanInfo.Id,
                    pro_transaction_type = DataDictionary.transactiontype_InvestFrazon,
                    pro_transaction_money=investInfo.pro_invest_money,
                    pro_transaction_remarks="投资成功",
                    pro_transaction_time=DateTime.Now,
                    pro_transaction_no = investInfo.pro_order_no.Split('|')[0],
                    pro_platforms_fee=0.00m,
                    pro_guarantee_fee=0.00m,
                    pro_frozen_money = investInfo.pro_invest_money,
                    pro_transaction_status= DataDictionary.transactionstatus_success,
                    pro_description = transactionDescription
                };
                _transactionService.Add(trans);
            }
            #endregion

            var redInfo = _redInfoService.InvestUseRed(investId:investInfo.Id);
            #region 跟新红包信息
            if (redInfo!=null)
            {
                redInfo.cst_red_employ = true;
                _redInfoService.Update(redInfo);
            }
            #endregion

            #region 处理活动信息
            //获取活动
            try
            {
                var customerInfo = _customerService.Find(userInfo.cst_customer_id.Value);
                if (customerInfo != null && customerInfo.cst_parent_id != null)
                {
                    //判断距离注册日期是否是30天之内
                    if ((DateTime.Now - userInfo.cst_add_date).Value.Days <= 30)
                    {
                        var registerDate = userInfo.cst_add_date.Value;
                        var activity = _inviteService.InvitationActivity(registerDate.Year, registerDate.Month);
                        if (activity != null)
                        {
                            //获取收益率
                            var rate = activity.Rewards / 100;
                            var benifit = new tbInvitationActivitiesstat
                            {
                                InvestorId = userInfo.Id,
                                InvestmentAmount = investInfo.pro_invest_money.Value,
                                loanId = investInfo.pro_loan_id.Value,
                                BeneficiaryId = customerInfo.cst_parent_id.Value,
                                ActivelinkId = activity.Id,
                                IsDel = false,
                                CreateTime = DateTime.Now,
                                Year = registerDate.Year,
                                Months = registerDate.Month
                            };
                            var benifitMoney = benifit.InvestmentAmount * rate;
                            benifit.BeneficiaryAmount = Math.Round(benifitMoney.Value, 2, MidpointRounding.AwayFromZero);
                            _inviteService.Add(benifit);
                        }
                    }
                }
            }
            catch
            {
                // ignored 
            }

            #endregion
        }
        #endregion

    }
}