using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZFCTAPI.Data.ApiModels.ReturnModels;
using ZFCTAPI.Data.PRO;
using ZFCTAPI.Data.CST;
using System.Transactions;
using ZFCTAPI.Core;
using ZFCTAPI.Core.Enums;
using ZFCTAPI.Services.Repayment;
using ZFCTAPI.Data.Repayment;
using ZFCTAPI.Services.LoanInfo;
using ZFCTAPI.Services.UserInfo;
using ZFCTAPI.Services.InvestInfo;
using ZFCTAPI.Services.BoHai;
using ZFCTAPI.Data.BoHai.SubmitModels;
using ZFCTAPI.Services.Helpers;
using ZFCTAPI.Core.Helpers;
using ZFCTAPI.Core.Configuration;
using ZFCTAPI.Data.ApiModels.SubmitModels;
using Newtonsoft.Json;
using ZFCTAPI.Services.Popular;
using ZFCTAPI.Data.MultiTable;
using ZFCTAPI.Services.Transaction;
using ZFCTAPI.WebApi.RequestAttribute;
using Microsoft.IdentityModel.Logging;
using ZFCTAPI.WebApi.Validates;
using ZFCTAPI.Core.Caching;

namespace ZFCTAPI.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [RequestLog]
    public class RepaymentController : Controller
    {
        #region ctor fields

        private readonly ICacheManager _cacheManager;
        private readonly IPayRecordService _ipayRecordService;
        private readonly ILoanPlanService _iloanPlanService;
        private readonly ILoanInfoService _iloanInfoService;
        private readonly IAccountInfoService _iaccountInfoService;
        private readonly IInvesterPlanService _iinvesterPlanService;
        private readonly IBHAccountService _ibhaccountService;
        private readonly IInvestInfoService _iinvestInfoService;
        private readonly ITransInfoService _itransInfoService;
        private readonly IBHRepaymentService _ibhrepaymentService;
        private readonly ILoanTextHelper _iloanTextHelper;
        private readonly BoHaiApiConfig _boHaiApiConfig;
        private readonly ICstUserInfoService _cstUserInfoService;
        private readonly ICstRedInfoService _cstRedInfoService;
        private readonly IBHUserService _iBHUserService;
        private readonly ICompanyInfoService _companyInfoService;
        private readonly ICstTransactionService _cstTransactionService;
        private readonly ICapitalTransferService _capitalTransferService;

        private static object lockObj = new object();

        public RepaymentController(IPayRecordService ipayRecordService,
            ILoanPlanService iloanPlanService,
            ILoanInfoService iloanInfoService,
            IAccountInfoService iaccountInfoService,
            IInvesterPlanService iinvesterPlanService,
            IBHAccountService ibhaccountService,
            IInvestInfoService iinvestInfoService,
            ITransInfoService itransInfoService,
            IBHRepaymentService ibhrepaymentService,
            ILoanTextHelper iloanTextHelper,
            BoHaiApiConfig boHaiApiConfig,
            ICstUserInfoService cstUserInfoService,
            ICstRedInfoService cstRedInfoService,
            IBHUserService iBHUserService,
            ICapitalTransferService capitalTransferService,
            ICstTransactionService cstTransactionService,
            ICompanyInfoService companyInfoService,
            ICacheManager cacheManager
            )//
        {
            _ipayRecordService = ipayRecordService;
            _iloanPlanService = iloanPlanService;
            _iloanInfoService = iloanInfoService;
            _iaccountInfoService = iaccountInfoService;
            _iinvesterPlanService = iinvesterPlanService;
            _ibhaccountService = ibhaccountService;
            _iinvestInfoService = iinvestInfoService;
            _itransInfoService = itransInfoService;
            _ibhrepaymentService = ibhrepaymentService;
            _iloanTextHelper = iloanTextHelper;
            _boHaiApiConfig = boHaiApiConfig;
            _cstUserInfoService = cstUserInfoService;
            _cstRedInfoService = cstRedInfoService;
            _iBHUserService = iBHUserService;
            _capitalTransferService = capitalTransferService;
            _companyInfoService = companyInfoService;
            _cstTransactionService = cstTransactionService;
            _cstTransactionService = cstTransactionService;
            _cacheManager = cacheManager;
        }

        #endregion

        [HttpPost]
        public ReturnModel<string, string> Test()
        {
            //var redList = _cstRedInfoService.LoanRedList(708);
            //var str = "20180704151451431992|20180704151718508327";
            //    _capitalTransferService.CancelSuccess(str,"");
            //_capitalTransferService.RepaymentSuccess("20180511091756945837", "");
            return new ReturnModel<string, string>();
        }

        #region 还款

        /// <summary>
        /// 还款接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<bool, string> RepaymentPlanData([FromBody]SRepaymentSubmitModel model)
        {
            ReturnModel<bool, string> rModel = new ReturnModel<bool, string>();

            #region 验签

            var verifyResult = VerifyBase.SignAndToken<SRepaymentSubmitModel>(model, out CST_user_info userInfo);
            if (verifyResult != ReturnCode.Success)
            {
                rModel.ReturnCode = (int)verifyResult;
                rModel.Message = "签名验证失败";
                rModel.ReturnData = false;
                return rModel;
            }

            #endregion

            var existPhone = _cacheManager.Get<String>(userInfo.cst_user_phone);
            if (string.IsNullOrEmpty(existPhone) || !existPhone.Equals(model.VerCode, StringComparison.OrdinalIgnoreCase))
            {
                rModel.ReturnCode = (int)verifyResult;
                rModel.Message = "手机验证码错误或过期";
                rModel.ReturnData = false;
                return rModel;
            }

            try
            {
                lock (lockObj)
                {
                    rModel = Repayment(model.LoanPlanId, model.IsGuar);
                }
            }
            catch (Exception e)
            {
                LogsHelper.WriteLog("还款报错,ID" + model.LoanPlanId + ",报错时间:" + DateTime.Now + ",错误信息" + e.ToString());
            }

            return rModel;
        }

        /// <summary>
        /// 还款处理
        /// </summary>
        /// <param name="planId">还款计划表ID</param>
        /// <param name="isGuar">是否是担保人还款</param>
        /// <returns></returns>
        private ReturnModel<bool, string> Repayment(int planId, bool isGuar)
        {
            var instId = BoHaiApiEngineToConfiguration.InstId(); //银行分配给平台或者结算系统的唯一标识(800025000010003)
            ReturnModel<bool, string> rModel = new ReturnModel<bool, string>();
            string tip = "";//提示信息
            bool isJX = true;
            PRO_loan_plan loanPlan = _iloanPlanService.Find(planId);
            PRO_loan_info loanInfo = null;//标的信息
            CST_account_info accountInfo = null;//借款人账号
            CompleteUserAccount userInfo = null;//借款人详细信息
            CST_account_info guaraccountInfo = null;//担保人账号

            #region 验证标的，借款人账户信息

            //还款计划验证
            if (loanPlan == null)
            {
                tip = "还款计划不存在";
                isJX = false;
            }
            else
            {
                if (loanPlan.pro_is_clear)
                {
                    tip = "还款计划状态已还清";
                    isJX = false;
                }
                if (loanPlan.pro_is_use)
                {
                    tip = "还款计划正在处理，请稍后再试";
                    isJX = false;
                }
                var loanPlanList = _iloanPlanService.GetLoanPlansByCondition(loanPlan.pro_loan_id.Value).Where(p => p.Id < loanPlan.Id && p.pro_is_clear == false).ToList();//判断上一期还款计划是否全部已经结束
                if (loanPlanList != null && loanPlanList.Any())
                {
                    tip = "上一期还款计划尚未还款！";
                    isJX = false;
                }
            }
            //标的信息验证
            if (isJX)
            {
                loanInfo = _iloanInfoService.Find(loanPlan.pro_loan_id.Value);
                if (loanInfo == null)
                {
                    tip = "标的信息不存在";
                    isJX = false;
                }
                if (!loanInfo.Bohai)
                {
                    tip = "标的信息正确";
                    isJX = false;
                }
            }
            //账户信息验证
            if (isJX)
            {
                //借款人账户
                userInfo = _cstUserInfoService.GetCompleteUserAccount(loanInfo.pro_add_emp);
                accountInfo = userInfo?.AccountInfo;
                if (accountInfo == null || !accountInfo.JieSuan || !accountInfo.BoHai)
                {
                    tip = accountInfo == null ? "借款人账户信息错误" : "借款人未开户";
                    isJX = false;
                }
                //担保人账户
                if (isGuar)
                {
                    if (loanInfo.pro_loan_guar_company == null)
                    {
                        tip = "担保人账户信息不存在";
                        isJX = false;
                    }
                    else
                    {
                        guaraccountInfo = _iaccountInfoService.GetAccountInfoByCompanyId(loanInfo.pro_loan_guar_company.Value);
                        if (guaraccountInfo == null || !guaraccountInfo.JieSuan || !guaraccountInfo.BoHai)
                        {
                            tip = "担保人账户不存在或未开户";
                            isJX = false;
                        }
                    }
                }
            }
            if (!isJX)
            {
                rModel.Message = tip;
                rModel.ReturnData = false;
                rModel.ReturnCode = loanInfo!=null&&!loanInfo.Bohai ? (int)ReturnCode.NoBohaiData : (int)ReturnCode.DataEorr;
                return rModel;
            }

            #endregion

            #region 计算利息

            //查询投资人信息
            var investPersons = _ipayRecordService.GetInvesterByPlanId(planId);
            var repaymentPersons = CalculateInvesterMoney(investPersons, loanInfo, loanPlan, out decimal zfctOverRate);
            //平台服务费(应还平台服务费 = 本期应还平台服务费（还款计划表） - 本期实还平台服务费（还款计划表）+应还平台罚息 )
            decimal zfctServiceFee = (decimal)((loanPlan.pro_pay_service_fee == null ? 0 : loanPlan.pro_pay_service_fee) - (loanPlan.pro_collect_service_fee == null ? 0 : loanPlan.pro_collect_service_fee)) + zfctOverRate;
            //还款总金额 = 平台手续费 +平台罚息+ 明细汇总本金 + 明细汇总收益
            decimal totalRepaymentMoney = zfctServiceFee + repaymentPersons.Sum(p => p.TransAmt + Convert.ToDecimal(p.Interest));

            #endregion

            #region 授权金额校验

            //判断借款人 账号余额 >= 还款金额
            //如果是个人还款查询个人账户，否则查询当保人账户
            var platformUid = !isGuar ? accountInfo.cst_plaCustId : guaraccountInfo.cst_plaCustId;
            var repayAuth = _iBHUserService.AuthInfo(platformUid, AuthTyp.Repayment);
            if (repayAuth.IsAuth == "0")
            {
                rModel.Message = !isGuar ? "借款账户未还款授权或还款授权过期" : "担保账户未还款授权或还款授权过期";
                rModel.ReturnData = false;
                rModel.ReturnCode = (int)ReturnCode.TokenEorr;
                return rModel;
            }
            if (repayAuth.IsAuth == "1" && decimal.Parse(repayAuth.AuthMoney) < totalRepaymentMoney)
            {
                rModel.Message = !isGuar ? "借款账户还款授权金额不足" : "担保账户还款授权金额不足";
                rModel.ReturnData = false;
                rModel.ReturnCode = (int)ReturnCode.TokenEorr;
                return rModel;
            }

            var payAuth = _iBHUserService.AuthInfo(platformUid, AuthTyp.Payment);
            if (payAuth.IsAuth == "0")
            {
                rModel.Message = !isGuar ? "借款账户未缴费授权或还缴费授权过期" : "担保账户未缴费授权或缴费授权过期";
                rModel.ReturnData = false;
                rModel.ReturnCode = (int)ReturnCode.TokenEorr;
                return rModel;
            }
            if (payAuth.IsAuth == "1" && decimal.Parse(payAuth.AuthMoney) < zfctServiceFee)
            {
                rModel.Message = !isGuar ? "借款账户缴费授权金额不足" : "担保账户缴费授权金额不足";
                rModel.ReturnData = false;
                rModel.ReturnCode = (int)ReturnCode.TokenEorr;
                return rModel;
            }

            #endregion

            #region 账户余额校验

            var investPlatformUid = !isGuar ? accountInfo.financing_platform_id : guaraccountInfo.financing_platform_id;//accountInfo.invest_platform_id;

            var bhAccountInfo = _ibhaccountService.AccountQueryAccBalance(new SBHAccountQueryAccBalance
            {
                SvcBody = new SBHAccountQueryAccBalanceBody
                {
                    platformUid = investPlatformUid
                }
            });
            if (bhAccountInfo != null && bhAccountInfo.RspSvcHeader.returnCode.Equals(JSReturnCode.Success))
            {
                if (Convert.ToDecimal(bhAccountInfo.SvcBody.totalAmount) < totalRepaymentMoney)
                {
                    rModel.Message = "融资户余额不足";
                    rModel.ReturnData = false;
                    rModel.ReturnCode = (int)ReturnCode.TokenEorr;
                    return rModel;
                }
            }
            else
            {
                rModel.Message = "查询余额出错";
                rModel.ReturnData = false;
                rModel.ReturnCode = (int)ReturnCode.TokenEorr;
                return rModel;
            }

            #endregion

            #region 上传文件

            var tranSeqNo = CommonHelper.GetMchntTxnSsn();//商户流水号
            var fileFtpName = instId + "_" + DateTime.Now.ToString("yyyyMMdd") + "_FileRepayment_" + tranSeqNo + ".txt";//TXT文件文件名

            //还款文件
            var txtForFtpModel = new SBHRepaymentFile()
            {
                char_set = "00",
                partner_id = instId,
                MerBillNo = tranSeqNo,
                TransAmt = totalRepaymentMoney,
                FeeAmt = zfctServiceFee,
                BorrowId = loanInfo.Id.ToString(),
                BorrowerAmt = loanInfo.pro_loan_money.Value,
                MerPriv = "",
                TotalNum = repaymentPersons.Count,
                InvestPersons = repaymentPersons,
                BorrCustId = accountInfo.cst_plaCustId
            };

            //将"元"转成"分"
            txtForFtpModel.TransAmt = decimal.Round(txtForFtpModel.TransAmt * 100, 0);
            txtForFtpModel.FeeAmt = decimal.Round(txtForFtpModel.FeeAmt * 100, 0);
            txtForFtpModel.BorrowerAmt = decimal.Round(txtForFtpModel.BorrowerAmt * 100, 0);
            foreach (var item in txtForFtpModel.InvestPersons)
            {
                item.TransAmt = decimal.Round(item.TransAmt * 100, 0);
                item.Interest = decimal.Round(Convert.ToDecimal(item.Interest) * 100, 0).ToString();
            };

            //上传文件
            var fileUploadResult = _iloanTextHelper.FileRelease(txtForFtpModel, fileFtpName);
            if (fileUploadResult.Result != UploadResult.Success)
            {
                rModel.Message = "还款文件上传失败";
                rModel.ReturnData = false;
                rModel.ReturnCode = (int)ReturnCode.TokenEorr;
                return rModel;
            }


            #endregion

            #region 还款明细接口

            SBHRepaymentExecuteEx bhModel = new SBHRepaymentExecuteEx()
            {
                SvcBody = new SBHRepaymentExecuteExModel()
                {
                    projectCode = loanInfo.Id.ToString(),
                    repaymentPlanCode = "",
                    numberOfPayments = loanInfo.pro_loan_period > 1 ? loanInfo.pro_loan_period.ToString() : "0",
                    period = loanInfo.pro_loan_period > 1 ? loanPlan.pro_loan_period.ToString() : "0",
                    actualRepaymentTime = DateTime.Now.ToString("hhmmss"),
                    sumPenaltyInterestToPlatform = "0",
                    complete = loanPlan.pro_loan_period.Value != loanInfo.pro_loan_period ? "0" : "",
                    batchFlag = "1",
                    fileName = fileFtpName,
                    extension = "",
                    compensatoryFlag = !isGuar ? "1" : "3",
                    repaymentPlatformCode = !isGuar ? "" : platformUid
                }
            };
            bhModel.ReqSvcHeader.tranSeqNo = tranSeqNo;

            //请求结算中心还款明细接口
            var bhReturnResult = _ibhrepaymentService.RepaymentExecute(bhModel);
            if (bhReturnResult.RspSvcHeader.returnCode != JSReturnCode.Success.ToString())
            {
                rModel.Message = bhReturnResult.RspSvcHeader.returnMsg;
                rModel.ReturnData = false;
                rModel.ReturnCode = (int)ReturnCode.TokenEorr;
                return rModel;
            }

            #endregion

            #region 借款人还款后，撤销债转标

            /*
            var tranInfos = _itransInfoService.GetTransferList(loanInfo.Id, false);
            if (tranInfos != null && tranInfos.Count() > 0)
            {
                var tempTranInfos = tranInfos.Where(p => p.investInfo.pro_delsign == false && p.pro_transfer_state != DataDictionary.transferstatus_HasTransfer).ToList();
                foreach (var tranInfo in tempTranInfos)
                {
                    tranInfo.pro_transfer_state = DataDictionary.transferstatus_HasFlowStandard;
                    tranInfo.pro_missing_date = DateTime.Now.Date;
                    _itransInfoService.Update(tranInfo);
                }
            }
            */
            #endregion

            #region 更新还款计划表，回款计划表

            //更新借款人还款计划表
            loanPlan.tran_seq_no = tranSeqNo;
            loanPlan.pro_is_use = true;//正在还款

            loanPlan.pro_collect_money = loanPlan.pro_pay_money;
            loanPlan.pro_collect_service_fee = zfctServiceFee;
            loanPlan.pro_collect_guar_fee = 0;
            loanPlan.pro_collect_total = totalRepaymentMoney;
            loanPlan.pro_collect_date = DateTime.Now;
            loanPlan.pro_collect_rate = loanPlan.pro_pay_rate;
            loanPlan.pro_collect_over_rate = loanPlan.pro_pay_over_rate == null ? 0 : loanPlan.pro_pay_over_rate.Value;
            loanPlan.pro_pay_type = isGuar ? DataDictionary.RepaymentType_PlatformDaihuan : DataDictionary.repaymenstate_Normal;
            loanPlan.CreDt = DateTime.Now;

            _iloanPlanService.Update(loanPlan);


            //更新投资人回款计划表

            foreach (var item in investPersons)
            {
                var investPlan = _iinvesterPlanService.Find(item.InvestPlanId);
                var temp = repaymentPersons.First(t => t.ID == item.ID.ToString());

                //保存记录，将'分'转成元
                investPlan.pro_pay_over_rate = Convert.ToDecimal(temp.Interest) / 100 - item.Pro_pay_rate;
                investPlan.pro_collect_money = temp.TransAmt / 100;
                investPlan.pro_collect_rate = item.Pro_pay_rate;
                investPlan.pro_collect_over_rate = Convert.ToDecimal(temp.Interest) / 100 - item.Pro_pay_rate;
                investPlan.pro_collect_total = (Convert.ToDecimal(temp.Interest) + temp.TransAmt) / 100;

                _iinvesterPlanService.Update(investPlan);
            }

            #endregion

            rModel.ReturnCode = (int)ReturnCode.Success;
            rModel.ReturnData = true;
            rModel.Message = "还款提交成功，请稍后查看";
            return rModel;
        }

        /// <summary>
        /// 计算罚息
        /// </summary>
        /// <param name="investPersons"></param>
        /// <param name="loanInfo"></param>
        /// <param name="loanPlan"></param>
        /// <param name="zfctOverRate">应还平台罚息</param>
        /// <returns></returns>
        private List<SBHRepaymentFilePerson> CalculateInvesterMoney(List<ReturnRepaymentModel> investPersons, PRO_loan_info loanInfo, PRO_loan_plan loanPlan, out decimal zfctOverRate)
        {
            List<SBHRepaymentFilePerson> rList = new List<SBHRepaymentFilePerson>();
            zfctOverRate = 0;

            decimal decGenIntSing = 0.00m;      //应还利息（单个）= 本期应还利息（收回计划表）- 本期已还利息（收回计划表）
            decimal decInvAmount = 0.00m;       //应还本金（单个）
            decimal decAmountPro = 0.00m;       //投资金额占比（单个） = 投资金额（单个） / 借款金额
            decimal decOveIntSingle = 0.00m;    //罚息（单个）= 罚息（总）* 投资金额占比
            decimal decOveIntSingleExp = 0.00m; //应还平台罚息
            decimal decGenPriSing = 0.00m;      //应还本金（单个）= 本期应还本金（收回计划表）- 本期已还本金（收回计划表）
            decimal decOveIntTotal = 0.00m;     //罚息（总）= 应还本金(总) * 罚息利率 * 逾期天数 + 剩余罚金（总）
            var totalOveInt = 0.00m;//累计已还罚息
            decimal decGenTotSing = 0.00m;      //应还总额（单个）= 罚息（单个）+ 应还本金（单个）+ 应还利息（单个）
            //应还利息（总）= 本期应还利息（还款计划表） - 本期实还利息（还款计划表）
            var decGenInterest = (decimal)((loanPlan.pro_pay_rate == null ? 0 : loanPlan.pro_pay_rate) - (loanPlan.pro_collect_rate == null ? 0 : loanPlan.pro_collect_rate));

            for (int i = 0; i < investPersons.Count; i++)
            {
                //应还利息（单个）= 本期应还利息（收回计划表）- 本期已还利息（收回计划表）
                decGenIntSing = (decimal)investPersons[i].Pro_pay_rate - (decimal)(investPersons[i].Pro_collect_rate == null ? 0 : investPersons[i].Pro_collect_rate);
                //应还本金（单个）
                decInvAmount = (decimal)(investPersons[i].Pro_pay_money == null ? 0 : investPersons[i].Pro_pay_money);
                //投资金额占比（单个） = 实还利息（单个） / 实还利息（总）
                decAmountPro = decGenIntSing / (decGenInterest == 0 ? 1 : decGenInterest);
                //罚息（单个）= 罚息（总）* 投资金额占比
                decOveIntSingle = Convert.ToDecimal((decOveIntTotal * decAmountPro).ToString("0.00"));

                decOveIntSingleExp = decOveIntSingle * Convert.ToDecimal(investPersons[i].pro_experience_money) / (decimal)investPersons[i].Pro_invest_money;
                //应还本金（单个）= 本期应还本金（收回计划表）- 本期已还本金（收回计划表）
                decGenPriSing = (decimal)investPersons[i].Pro_pay_money - (decimal)(investPersons[i].Pro_collect_money == null ? 0 : investPersons[i].Pro_collect_money);
                if (decOveIntTotal > 0 && i != (investPersons.Count - 1))
                {
                    totalOveInt += decOveIntSingle;
                }
                if (i == investPersons.Count - 1)
                {
                    decOveIntSingle = Convert.ToDecimal((decOveIntTotal - totalOveInt).ToString("0.00"));
                }
                //还款特殊处理20171009 罚息归0
                if (loanInfo.Id == 448)
                {
                    decOveIntSingle = 0;
                }
                //应还总额（单个）= 罚息（单个）+ 应还本金（单个）+ 应还利息（单个）
                decGenTotSing = decOveIntSingle + decGenPriSing + decGenIntSing - decOveIntSingleExp;

                rList.Add(new SBHRepaymentFilePerson()
                {
                    ID = investPersons[i].ID.ToString(),
                    PlaCustId = investPersons[i].cst_plaCustId,
                    TransAmt = investPersons[i].Pro_pay_money == null ? 0 : investPersons[i].Pro_pay_money.Value,
                    Interest = (decOveIntSingle + investPersons[i].Pro_pay_rate).ToString(),//decGenTotSing.ToString(),
                    Inves_fee = 0
                });
                zfctOverRate = zfctOverRate + decOveIntSingleExp;
            }

            return rList;
        }

        #endregion

        #region 满标

        /// <summary>
        /// 募集结果上报
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<bool, string> RaiseLoanResult([FromBody]SRaiseLoanSubmitModel model)
        {
            var rModel = new ReturnModel<bool, string>();

            #region 验签

            var verifyResult = VerifyBase.Sign<SRaiseLoanSubmitModel>(model);
            if (verifyResult != ReturnCode.Success)
            {
                rModel.ReturnCode = (int)verifyResult;
                rModel.Message = "签名验证失败";
                return rModel;
            }

            #endregion

            try
            {
                lock (lockObj)
                {
                    rModel = RaiseLoan(model.LoanId);
                }
            }
            catch (Exception e)
            {
                LogsHelper.WriteLog("满标报错,ID" + model.LoanId + ",报错时间:" + DateTime.Now + ",错误信息" + e.ToString());
            }

            return rModel;
        }


        /// <summary>
        /// 满标处理
        /// </summary>
        /// <param name="loanId">标的ID</param>
        /// <returns></returns>
        private ReturnModel<bool, string> RaiseLoan(int loanId)
        {
            var rModel = new ReturnModel<bool, string>();
            var instId = BoHaiApiEngineToConfiguration.InstId(); //商户号
            var merBillNo = CommonHelper.GetMchntTxnSsn();//流水号
            var raiseLoanTxtFileName = instId + "_" + DateTime.Now.ToString("yyyyMMdd") + "_FileRelease_" + merBillNo + ".txt";//满文件名称

            var loanInfo = _iloanInfoService.Find(loanId);
            if (loanInfo == null)
            {
                rModel.Message = "标的信息不存在";
                rModel.ReturnData = false;
                rModel.ReturnCode = (int)ReturnCode.DataEorr;
                return rModel;
            }
            if (loanInfo.pro_loan_state != DataDictionary.projectstate_StayTransfer)
            {
                rModel.Message = "标的状态不正确";
                rModel.ReturnData = false;
                rModel.ReturnCode = (int)ReturnCode.DataEorr;
                return rModel;
            }
            if (loanInfo.pro_is_use)
            {
                rModel.Message = "银行正在处理，请稍等";
                rModel.ReturnData = false;
                rModel.ReturnCode = (int)ReturnCode.DataEorr;
                return rModel;
            }

            var userInfo = _cstUserInfoService.GetCompleteUserAccount(loanInfo.pro_add_emp);
            var accountInfo = userInfo?.AccountInfo;//借款人信息

            if (accountInfo == null || !accountInfo.BoHai || !accountInfo.JieSuan)
            {
                rModel.Message = "借款人信息错误";
                rModel.ReturnData = false;
                rModel.ReturnCode = (int)ReturnCode.DataEorr;
                return rModel;
            }

            #region 红包验证

            //统计标的使用的红包列表
            var loanRedList = _cstRedInfoService.LoanRedList(loanInfo.Id);
            if (loanRedList != null && loanRedList.Any())
            {
                //标的红包使用总金额
                var redMoney = loanRedList.Sum(r => r.cst_red_money);
                if (redMoney != null && redMoney.Value > 0)
                {
                    //商户账户余额查询
                    var merchantInfo = _iBHUserService.QueryMerchantAccts(new SBHBaseModel());
                    if (merchantInfo != null)
                    {
                        var account = merchantInfo.items.Where(m => m.acTyp == "820").FirstOrDefault();
                        if (account == null || Convert.ToDecimal(account.avlBal) < redMoney)
                        {
                            rModel.Message = "营销账户余额不足";
                            rModel.ReturnData = false;
                            rModel.ReturnCode = (int)ReturnCode.DataEorr;
                            return rModel;
                        }
                    }
                    else
                    {
                        rModel.Message = "查询营销账户余额错误";
                        rModel.ReturnData = false;
                        rModel.ReturnCode = (int)ReturnCode.DataEorr;
                        return rModel;
                    }
                }
            }
            #endregion

            #region 缴费授权

            var payAuth = _iBHUserService.AuthInfo(accountInfo.cst_plaCustId, AuthTyp.Payment);
            if (payAuth.IsAuth == "0")
            {
                rModel.Message = "借款人没有进行缴费授权,或缴费授权过期";
                rModel.ReturnData = false;
                rModel.ReturnCode = (int)ReturnCode.DataEorr;
                return rModel;
            }
            if (payAuth.IsAuth == "1" && decimal.Parse(payAuth.AuthMoney) < loanInfo.pro_this_procedurefee.GetValueOrDefault())
            {
                rModel.Message = "借款人缴费授权金额不足";
                rModel.ReturnData = false;
                rModel.ReturnCode = (int)ReturnCode.DataEorr;
                return rModel;
            }

            #endregion

            #region 修改起息日
            /*
             * (渤海银行不支持起息日修改)
            SBHUpdateProjectRateDate updateProjectModel = new SBHUpdateProjectRateDate()
            {
                SvcBody = new SBHUpdateProjectRateDateModel()
                {
                    projectCode = "9",
                    valueDate = "20180120",
                    purchaseNumber= "20180117112734494834"
                }
            };

            var updateProjectResult = _ibhrepaymentService.UpdateProjectRateDate(updateProjectModel);
            if (updateProjectResult.RspSvcHeader.returnCode != BHReturnCode.Success.ToString())
            {
                rModel.Message = updateProjectResult.RspSvcHeader.returnMsg;
                rModel.ReturnData = false;
                rModel.ReturnCode = (int)ReturnCode.TokenEorr;
                return rModel;
            
            */
            #endregion

            #region 上传文件

            //获取标的投资人列表
            var investPerson = _ipayRecordService.GetInvestPersons(loanInfo.Id);
            var investDetail = investPerson.Select(p =>
            {
                InvestDetail detail = new InvestDetail();
                detail.ID = p.ID.ToString();
                detail.PlaCustId = p.cst_plaCustId;
                detail.TransAmt = p.Pro_invest_money;
                detail.FreezeId = p.Pro_fro_orderno;
                return detail;
            }).ToList();

            #region 判断满标

            if (investDetail.Sum(i => i.TransAmt) != loanInfo.pro_loan_money)
            {
                rModel.Message = "用户投资总额与标的金额不等";
                rModel.ReturnData = false;
                rModel.ReturnCode = (int)ReturnCode.DataEorr;
                return rModel;
            }

            #endregion

            var zfctFeeMoney = loanInfo.pro_this_procedurefee.GetValueOrDefault();
            var totalMoney = investDetail.Sum(item => item.TransAmt);
            //生成TXT文档模型
            var raiseLoanTxt = new SBHRaiseLoanResultFile()
            {
                char_set = "00",
                partner_id = instId,
                MerBillNo = merBillNo,
                TransAmt = totalMoney,
                FeeAmt = zfctFeeMoney,
                BorrowId = loanInfo.Id.ToString(),
                BorrowerAmt = loanInfo.pro_loan_money.Value,
                BorrCustId = accountInfo.cst_plaCustId,
                ReleaseType = "0",
                MerPriv = "",
                TotalNum = investDetail.Count,
                InvestDetails = investDetail
            };
            //将“元”转换成“分”
            raiseLoanTxt.FeeAmt = decimal.Round(raiseLoanTxt.FeeAmt * 100, 0);
            raiseLoanTxt.TransAmt = decimal.Round(raiseLoanTxt.TransAmt * 100, 0);
            raiseLoanTxt.BorrowerAmt = decimal.Round(raiseLoanTxt.BorrowerAmt * 100, 0);
            foreach (var item in raiseLoanTxt.InvestDetails)
            {
                item.TransAmt = decimal.Round(item.TransAmt * 100, 0);
            }
            //上传TXT文件
            var fileUploadResult = _iloanTextHelper.RaiseLoanResultFile(raiseLoanTxt, raiseLoanTxtFileName);
            if (fileUploadResult == null || fileUploadResult.Result != UploadResult.Success)
            {
                rModel.Message = "上传募集结果文件失败";
                rModel.ReturnData = false;
                rModel.ReturnCode = (int)ReturnCode.TokenEorr;
                return rModel;
            }

            #endregion

            #region 募集结果上报

            var model = new SBHRaiseLoanResult()
            {
                SvcBody = new SBHRaiseLoanResultModel()
                {
                    projectCode = loanInfo.Id.ToString(),
                    projectType = ProjectType.Creditor,
                    repaymentType = "1",
                    fileName = raiseLoanTxtFileName
                }
            };
            model.ReqSvcHeader.tranSeqNo = merBillNo;

            var bhResult = _ibhrepaymentService.RaiseResultNotice(model);
            if (bhResult == null || bhResult.RspSvcHeader.returnCode != JSReturnCode.Success.ToString())
            {
                rModel.Message = bhResult.RspSvcHeader.returnMsg;
                rModel.ReturnData = false;
                rModel.ReturnCode = (int)ReturnCode.TokenEorr;
                return rModel;
            }

            #endregion

            #region 保存流水号

            loanInfo.tran_seq_no = merBillNo;
            loanInfo.pro_is_use = true;
            loanInfo.Type = true;
            loanInfo.CreDt = DateTime.Now;
            loanInfo.pro_loan_state = DataDictionary.bank_state_using;

            _iloanInfoService.Update(loanInfo);

            #endregion

            rModel.Message = "满标处理已提交，请稍后查看";
            rModel.ReturnData = true;
            rModel.ReturnCode = (int)ReturnCode.Success;

            return rModel;
        }

        #endregion

        #region 流标

        /// <summary>
        /// 流标(如果不存在投资人，直接流标；
        /// 存在投资用户,先解冻投资人投资金额,
        /// 异步回调成功之后再流标)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<bool, string> RevokeLoan([FromBody]SRevokeSubmitModel model)
        {
            var rModel = new ReturnModel<bool, string>();

            #region 验签

            var verifyResult = VerifyBase.Sign<SRevokeSubmitModel>(model);
            if (verifyResult != ReturnCode.Success)
            {
                rModel.ReturnCode = (int)verifyResult;
                rModel.Message = "签名验证失败";
                return rModel;
            }

            #endregion

            var loanInfo = _iloanInfoService.Find(model.LoanId);
            if (loanInfo == null)
            {
                rModel.Message = "标的信息不存在";
                rModel.ReturnData = true;
                rModel.ReturnCode = (int)ReturnCode.DataFormatError;
            }
            if (loanInfo.pro_is_use)
            {
                rModel.Message = "银行正在处理";
                rModel.ReturnData = false;
                rModel.ReturnCode = (int)ReturnCode.DataEorr;
                return rModel;
            }

            var investPerson = _ipayRecordService.GetInvestPersons(loanInfo.Id);
            if ((investPerson == null || !investPerson.Any())
                && loanInfo.pro_loan_state != DataDictionary.bank_state_cancel_failed)
            {
                rModel = RevokeLoan(loanInfo, model.IP);
            }
            else
            {
                rModel = RevokeLoan(loanInfo, investPerson);
            }

            return rModel;
        }

        /// <summary>
        /// 流标处理（有投资用户）
        /// </summary>
        /// <param name="loanInfo">项目信息</param>
        /// <param name="investers">投资人列表</param>
        /// <returns>IP地址</returns>
        private ReturnModel<bool, string> RevokeLoan(PRO_loan_info loanInfo, List<InvestPerson> investers)
        {
            ReturnModel<bool, string> rModel = new ReturnModel<bool, string>();
            var instId = BoHaiApiEngineToConfiguration.InstId(); //商户号
            var merBillNo = CommonHelper.GetMchntTxnSsn();//流水号

            //投资人金额未解冻
            if (loanInfo.pro_loan_state != DataDictionary.bank_state_cancel_failed)
            {
                #region 上传投资人文件

                if (investers.Any(i => !i.pro_order_no.Contains("|")))
                {
                    rModel.Message = "投资人存管平台流水号不存在";
                    rModel.ReturnCode = (int)ReturnCode.DataEorr;
                    return rModel;
                }

                var ftpFileName = instId + "_" + DateTime.Now.ToString("yyyyMMdd") + "_BatInvestCancle_" + merBillNo + ".txt";
                var investDetail = investers.Select(p =>
                {
                    BatInvestCancleFileDetail detail = new BatInvestCancleFileDetail();
                    detail.ID = p.ID.ToString();
                    detail.OldTransId = p.pro_order_no.Split('|')[1];
                    detail.PlaCustId = p.cst_plaCustId;
                    detail.TransAmt = decimal.Round(p.Pro_invest_money * 100, 0);//转成'分'
                    detail.FreezeId = p.Pro_fro_orderno;
                    return detail;
                }).ToList();

                var ftpFileModel = new SBHBatInvestCancleFile()
                {
                    char_set = "00",
                    partner_id = instId,
                    BatchNo = loanInfo.Id.ToString(),
                    TransDate = DateTime.Now.ToString("yyyyMMdd"),
                    TotalNum = investDetail.Count,
                    InvestCancleDetail = investDetail
                };

                try
                {
                    //上传TXT文件
                    var fileUploadResult = _iloanTextHelper.RaiseLoanFailFile(ftpFileModel, ftpFileName);
                    LogsHelper.WriteLog("流标文件成功");
                    if (fileUploadResult.Result != UploadResult.Success)
                    {
                        rModel.Message = "上传撤销文件失败";
                        rModel.ReturnData = false;
                        rModel.ReturnCode = (int)ReturnCode.TokenEorr;
                        return rModel;
                    }
                }
                catch (Exception e)
                {
                    LogsHelper.WriteLog("流标文件失败" + e.ToString());
                }

                #endregion

                #region 解冻投资金额

                var requestModel = new SBHBatInvestCancle()
                {
                    SvcBody = new SBHBatInvestCancleModel()
                    {
                        batchNo = loanInfo.Id.ToString(),
                        fileName = ftpFileName,
                        remark = ""
                    }
                };
                requestModel.ReqSvcHeader.tranSeqNo = merBillNo;

                LogsHelper.WriteLog("申购接口开始");
                //请求撤销所有申购接口
                var concleResult = _ibhrepaymentService.BatInvestCancle(requestModel);

                LogsHelper.WriteLog("申购接口成功");

                if (concleResult.RspSvcHeader.returnCode != JSReturnCode.Success.ToString()
                    && concleResult.RspSvcHeader.returnCode != "success")
                {
                    rModel.Message = concleResult.RspSvcHeader.returnMsg;
                    rModel.ReturnData = false;
                    rModel.ReturnCode = (int)ReturnCode.TokenEorr;
                    return rModel;
                }

                #endregion

                loanInfo.pro_loan_state = DataDictionary.bank_state_using;
                loanInfo.tran_seq_no = requestModel.ReqSvcHeader.tranSeqNo;
                loanInfo.Type = false;
                loanInfo.pro_is_use = true;
                _iloanInfoService.Update(loanInfo);

                rModel.Message = "流标处理已提交，请稍后查看";
                rModel.ReturnData = true;
                rModel.ReturnCode = (int)ReturnCode.Success;
            }
            //投资金额已解冻，但是流标失败，后台再发起流标操作流标
            else
            {
                #region 募集结果上报

                var model = new SBHRaiseLoanResult()
                {
                    SvcBody = new SBHRaiseLoanResultModel()
                    {
                        projectCode = loanInfo.Id.ToString(),
                        projectType = ProjectType.Creditor,
                        repaymentType = "0"
                    }
                };

                var bhRaiseResult = _ibhrepaymentService.RaiseResultNotice(model);
                if (bhRaiseResult.RspSvcHeader.returnCode != JSReturnCode.Success.ToString())
                {
                    rModel.Message = bhRaiseResult.RspSvcHeader.returnMsg;
                    rModel.ReturnData = false;
                    rModel.ReturnCode = (int)ReturnCode.TokenEorr;
                    return rModel;
                }

                #endregion

                loanInfo.pro_is_use = false;
                loanInfo.pro_loan_state = DataDictionary.projectstate_FlowStandard;
                loanInfo.tran_seq_no = loanInfo.tran_seq_no + "|" + model.ReqSvcHeader.tranSeqNo;

                rModel.Message = "流标成功";
                rModel.ReturnData = true;
                rModel.ReturnCode = (int)ReturnCode.Success;
            }

            return rModel;
        }

        /// <summary>
        /// 流标处理(没有投资用户)
        /// </summary>
        /// <param name="loanInfo">项目信息</param>
        /// <param name="ip">IP地址</param>
        /// <returns></returns>
        private ReturnModel<bool, string> RevokeLoan(PRO_loan_info loanInfo, string ip)
        {
            var rModel = new ReturnModel<bool, string>();

            var bhModel = new SBHRevoke
            {
                SvcBody = new SBHRevokeModel
                {
                    projectCode = loanInfo.Id.ToString(),
                    projectType = ProjectType.Creditor,
                    revokeType = "01",
                    cancelIP = ip
                }
            };

            var result = _ibhrepaymentService.Revoke(bhModel);
            if (result != null && result.RspSvcHeader.returnCode == JSReturnCode.Success.ToString())
            {
                loanInfo.pro_loan_state = DataDictionary.projectstate_FlowStandard;
                loanInfo.tran_seq_no = bhModel.ReqSvcHeader.tranSeqNo;
                _iloanInfoService.Update(loanInfo);
                rModel.ReturnData = true;
                rModel.Message = "流标成功";
            }
            else
            {
                rModel.ReturnData = false;
                rModel.Message = result.RspSvcHeader.returnMsg;
            }

            return rModel;
        }

        #endregion

        #region 解冻单个投资用户

        [HttpPost]
        public ReturnModel<bool, string> ReleaseInvester()
        {
            var rModel = new ReturnModel<bool, string>();

            int investeId = 5384;

            var investeInfo = _ipayRecordService.GetInvesteInfo(investeId);
            if (investeInfo != null)
            {
                var instId = BoHaiApiEngineToConfiguration.InstId(); //商户号
                var merBillNo = CommonHelper.GetMchntTxnSsn();//流水号
                var ftpFileName = instId + "_" + DateTime.Now.ToString("yyyyMMdd") + "_BatInvestCancle_" + merBillNo + ".txt";
                var investDetail = new BatInvestCancleFileDetail
                {
                    ID = "1",
                    OldTransId = investeInfo.pro_order_no.Split('|')[1],
                    PlaCustId = investeInfo.cst_plaCustId,
                    TransAmt = decimal.Round(investeInfo.Pro_invest_money * 100, 0),
                    FreezeId = investeInfo.pro_order_no
                };

                var ftpFileModel = new SBHBatInvestCancleFile()
                {
                    char_set = "00",
                    partner_id = instId,
                    BatchNo = investeInfo.LoanId.ToString(),
                    TransDate = DateTime.Now.ToString("yyyyMMdd"),
                    TotalNum = 1,
                    InvestCancleDetail = new List<BatInvestCancleFileDetail> { investDetail }
                };

                LogsHelper.WriteLog(JsonConvert.SerializeObject(ftpFileModel));
                //上传TXT文件
                var fileUploadResult = _iloanTextHelper.RaiseLoanFailFile(ftpFileModel, ftpFileName);
                LogsHelper.WriteLog(JsonConvert.SerializeObject(fileUploadResult));

                if (fileUploadResult.Result != UploadResult.Success)
                {
                    rModel.Message = "上传撤销文件失败";
                    rModel.ReturnData = false;
                    rModel.ReturnCode = (int)ReturnCode.TokenEorr;
                    return rModel;
                }

                var requestModel = new SBHBatInvestCancle()
                {
                    SvcBody = new SBHBatInvestCancleModel()
                    {
                        batchNo = investeInfo.LoanId.ToString(),
                        fileName = ftpFileName,
                        remark = ""
                    }
                };
                requestModel.ReqSvcHeader.tranSeqNo = merBillNo;

                LogsHelper.WriteLog(JsonConvert.SerializeObject(requestModel));
                //请求撤销所有申购接口
                var concleResult = _ibhrepaymentService.BatInvestCancle(requestModel);
                LogsHelper.WriteLog(JsonConvert.SerializeObject(concleResult));
                if (concleResult.RspSvcHeader.returnCode != JSReturnCode.Success.ToString()
                    && concleResult.RspSvcHeader.returnCode != "success")
                {
                    LogsHelper.WriteLog("提交失败");
                }

                var investeRecord = _iinvestInfoService.Find(investeId);
                investeRecord.tran_seq_no = merBillNo;
                _iinvestInfoService.Update(investeRecord);
            }

            return rModel;
        }

        #endregion
    }
}