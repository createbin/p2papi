using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Transactions;
using ZFCTAPI.Core;
using ZFCTAPI.Core.Configuration.DataBase;
using ZFCTAPI.Core.Enums;
using ZFCTAPI.Data.ApiModels.ReturnModels;
using ZFCTAPI.Data.ApiModels.SubmitModels;
using ZFCTAPI.Data.PRO;
using ZFCTAPI.Services.InvestInfo;
using ZFCTAPI.Services.LoanInfo;
using ZFCTAPI.Services.Settings;
using ZFCTAPI.Services.Sys;
using ZFCTAPI.Services.Transfer;
using ZFCTAPI.WebApi.RequestAttribute;
using ZFCTAPI.WebApi.Validates;

namespace ZFCTAPI.WebApi.Controllers
{
    /// <summary>
    /// 个人中心-我的债权转让
    /// </summary>
    [Route("api/[controller]/[action]")]
    [RequestLog]
    public class MyTransferController : Controller
    {
        private readonly IInvestInfoService _investInfoService;
        private readonly IInvesterPlanService _investerPlanService;
        private readonly ILoanPlanService _loanPlanService;
        private readonly ILoanInfoService _loanInfoService;
        private readonly IProTranferApplyService _proTranferApplyService;
        private readonly ISettingService _settingService;
        private readonly IProLoanFlowService _proLoanFlowService;
        private readonly ITransInfoService _transInfoService;
        private readonly ISYSDataDictionaryService _sYSDataDictionaryService;

        private static object _lock = new object();

        public MyTransferController(IInvestInfoService investInfoService,
            IInvesterPlanService investerPlanService,
            ILoanPlanService loanPlanService,
            ILoanInfoService loanInfoService,
            IProTranferApplyService proTranferApplyService,
            ISettingService settingService,
            IProLoanFlowService proLoanFlowService,
            ITransInfoService transInfoService,
            ISYSDataDictionaryService sYSDataDictionaryService)
        {
            _investInfoService = investInfoService;
            _investerPlanService = investerPlanService;
            _loanPlanService = loanPlanService;
            _loanInfoService = loanInfoService;
            _proTranferApplyService = proTranferApplyService;
            _settingService = settingService;
            _proLoanFlowService = proLoanFlowService;
            _transInfoService = transInfoService;
            _sYSDataDictionaryService = sYSDataDictionaryService;
        }

        /// <summary>
        /// 可转出
        /// </summary>
        [HttpPost]
        public ReturnModel<ReturnPageData<RCanTransfer>, string> CanTransfer([FromBody]BasePageModel model)
        {
            var reModel = new ReturnModel<ReturnPageData<RCanTransfer>, string>();

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
                var pageData = _transInfoService.GetCanTransfer(userId, model.Page, model.PageSize);
                var _projectSettings = _settingService.LoadSetting<ProjectSettings>();
                var rePage = new ReturnPageData<RCanTransfer>
                {
                    PagingData = pageData.Items.Select(p =>
                    {
                        var item = new RCanTransfer()
                        {
                            InvestId = p.Id
                        };
                        var loanInfo = _loanInfoService.Find(p.pro_loan_id.Value);
                        item.LoanName = loanInfo.pro_loan_use;
                        item.LoanRate = loanInfo.pro_loan_rate.Value;
                        var investApply =  _proTranferApplyService.GetInvestApply(p.Id);
                        var investerPlans = _investerPlanService.GetListByCondition(isClear: false, investId: p.Id);
                        item.NextPayData = investerPlans.Min(l => l.pro_pay_date).Value;
                        item.WaitPrincipal = investerPlans.Sum(l => l.pro_pay_total).Value;
                        item.SurplusDay = (investerPlans.Min(l => l.pro_pay_date).Value - DateTime.Now).Days;
                        if (investApply != null) {
                            item.Period = investApply.pro_transfer_period.Value;
                            item.Discount = investApply.pro_transfer_harf_rate.Value;
                            item.DiscountMoney = investApply.pro_transfer_money.Value;
                            item.CanTransferMoney = investApply.pro_idual_money.Value;
                            item.TransferId = investApply.Id;
                            item.IsApply = true;
                        }
                        else {
                            item.Period = investerPlans.Count();
                            item.CanTransferMoney = investerPlans.Sum(l=>l.pro_pay_money).Value;
                            item.IsApply = false;
                        }
                     
                        return item;
                    }),
                    Extra1 = _projectSettings.sys_transfer_rate.ToString("0.00"),
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
        /// 转出中
        /// </summary>
        [HttpPost]
        public ReturnModel<ReturnPageData<RTransfering>, string> Transfering([FromBody]BasePageModel model)
        {
            var reModel = new ReturnModel<ReturnPageData<RTransfering>, string>();

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
                var pageData = _transInfoService.GetTransfering(userId, model.Page, model.PageSize);
                var rePage = new ReturnPageData<RTransfering>
                {
                    PagingData = pageData.Items.Select(p =>
                    {
                        var loanInfo = _loanInfoService.Find(p.pro_loan_id.Value);
                        var item = new RTransfering() {
                            LoanName = loanInfo.pro_loan_use,
                            LoanRate = loanInfo.pro_loan_rate.Value,
                            Period = p.pro_transfer_period.Value,
                            DiscountMoney = p.pro_transfer_money.Value,
                            Discount = p.pro_transfer_harf_rate.Value,
                            produceFee = p.pro_transfer_fee.Value,
                            BuyMoney = 0,
                            BuySpeed = 0,
                            TransferData = p.pro_transfer_date,
                            EndData = p.pro_transfer_date.AddDays(7),
                            State = _sYSDataDictionaryService.Find(p.pro_transfer_state.Value).sys_data_name,
                            CanTransferMoney = p.pro_idual_money.Value
                        };
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
        /// 已转出
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<ReturnPageData<RTransferOut>, string> TransferOut([FromBody]BasePageModel model)
        {

            var reModel = new ReturnModel<ReturnPageData<RTransferOut>, string>();

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
                var pageData = _transInfoService.GetTransferOut(userId, model.Page, model.PageSize);
                var rePage = new ReturnPageData<RTransferOut>
                {
                    PagingData = pageData.Items.Select(p =>
                    {
                        var loanInfo = _loanInfoService.Find(p.pro_loan_id.Value);
                        var item = new RTransferOut()
                        {
                            LoanName = loanInfo.pro_loan_use,
                            LoanRate = loanInfo.pro_loan_rate.Value,
                            Period = p.pro_transfer_period.Value,
                            DiscountMoney = p.pro_transfer_money.Value,
                            Discount = p.pro_transfer_harf_rate.Value,
                            produceFee = p.pro_transfer_fee.Value,
                            BuyMoney = p.pro_transfer_money.Value,
                            BuySpeed = 1,
                            TransferData = p.pro_transfer_date,
                            EndData = p.pro_full_date.Value,
                            State = _sYSDataDictionaryService.Find(p.pro_transfer_state.Value).sys_data_name,
                            CanTransferMoney = p.pro_idual_money.Value
                        };
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
        /// 已转入
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<ReturnPageData<RTransferIn>, string> TransferIn([FromBody]BasePageModel model)
        {
            var reModel = new ReturnModel<ReturnPageData<RTransferIn>, string>();

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
                var pageData = _transInfoService.GetTransferIn(userId, model.Page, model.PageSize);
                var rePage = new ReturnPageData<RTransferIn>
                {
                    PagingData = pageData.Items.Select(p =>
                    {
                        var loanInfo = _loanInfoService.Find(p.pro_loan_id.Value);
                        var investApply = _proTranferApplyService.Find(p.pro_transfer_id.Value);
                        var item = new RTransferIn()
                        {
                            InvestId = p.Id,
                            LoanName = loanInfo.pro_loan_use,
                            LoanRate = loanInfo.pro_loan_rate.Value,
                            Period = investApply.pro_transfer_period.Value,
                            DiscountMoney = investApply.pro_transfer_money.Value,
                            Discount = investApply.pro_transfer_harf_rate.Value,
                            produceFee = investApply.pro_transfer_fee.Value,
                            BuyMoney = investApply.pro_transfer_money.Value,
                            BuySpeed = 1,
                            State = _sYSDataDictionaryService.Find(investApply.pro_transfer_state.Value).sys_data_name,
                            CanTransferMoney = investApply.pro_idual_money.Value,
                        };
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
        /// 债权转让申请
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<bool, string> DoTransferData([FromBody]SDoTransferData model)
        {
            var retModel = new ReturnModel<bool, string>();

            #region 校验签名

            var signResult = VerifyBase.SignAndToken<SDoTransferData>(model, out int userId);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = false;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名

            model.Discount = Math.Floor(model.Discount);
            var checkResult = DoTransterCheck(model.InvestId, model.Discount, model.IdualMoney, userId, out PRO_loan_info loanInfo, out PRO_invest_info investInfo, out int transferPeriod);
            if (!string.IsNullOrEmpty(checkResult))
            {
                retModel.Message = checkResult;
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.ReturnData = false;
                retModel.Token = model.Token;
                return retModel;
            }

            var transferResult = DoTranster(investInfo, model.Discount, model.IdualMoney, userId, loanInfo, transferPeriod);
            if (transferResult)
            {
                retModel.Message = "转让成功";
                retModel.ReturnCode = (int)ReturnCode.Success;
                retModel.ReturnData = true;
                retModel.Token = model.Token;
                return retModel;
            }

            retModel.Message = "转让失败";
            retModel.ReturnCode = (int)ReturnCode.DataEorr;
            retModel.ReturnData = true;
            retModel.Token = model.Token;
            return retModel;
        }

        /// <summary>
        /// 债转校验
        /// </summary>
        /// <param name="investId"></param>
        /// <param name="discount"></param>
        /// <param name="idualMoney"></param>
        /// <param name="userId"></param>
        /// <param name="loanInfo"></param>
        /// <param name="investInfo"></param>
        /// <param name="transferPeriod"></param>
        /// <returns></returns>
        private string DoTransterCheck(int investId, decimal discount, decimal idualMoney, int userId, out PRO_loan_info loanInfo, out PRO_invest_info investInfo, out int transferPeriod)
        {
            var _projectSettings = _settingService.LoadSetting<ProjectSettings>();
            string result = string.Empty;
            loanInfo = new PRO_loan_info();
            investInfo = new PRO_invest_info();
            transferPeriod = 0;

            #region 债转

            var proTransferApplys = _proTranferApplyService.GetListByUserId(userId.ToString());
            if (proTransferApplys.Where(p => !p.pro_is_del && p.pro_transfer_state == DataDictionary.transferstatus_Pendingaudit).Count() > 0)
                return "存在一笔正在申请中的债权，不可转让！";
            if (proTransferApplys.Where(p => !p.pro_is_del && (p.pro_transfer_state == DataDictionary.transferstatus_HasThrough || p.pro_transfer_state == DataDictionary.transferstatus_StayTransfer || p.pro_transfer_state == DataDictionary.transferstatus_FullScalePending)).Count() > 0)
                return "存在一笔正在转出中的债权，不可转让！";

            if (_projectSettings.sys_min_transfer_rate > discount)
                return "不能小于系统设置的最低转让折扣" + _projectSettings.sys_min_transfer_rate.ToString();
            if (_projectSettings.sys_max_transfer_rate < discount)
                return "不能大于系统设置的最高转让折扣" + _projectSettings.sys_max_transfer_rate.ToString();

            var investPlans = _investerPlanService.GetListByCondition(false, investId);
            transferPeriod = investPlans.Count();
            var investTransferApplys = _proTranferApplyService.GetListByInvestId(investId.ToString());
            var noRepayMoney = investPlans.Sum(p => p.pro_pay_money);
            var kzMoney = noRepayMoney - investTransferApplys.Sum(p => p.pro_idual_money);
            if (idualMoney > kzMoney)
                return "转让份额不能大于可转份额";
            if (noRepayMoney <= 0)
                return "当前没有可转份额";

            #endregion 债转

            #region 投资限制

            investInfo = _investInfoService.Find(investId);
            if (investInfo == null)
                return "获取投资信息异常！";
            if (!((DateTime.Now - Convert.ToDateTime(investInfo.pro_invest_date)).Days + 1 > 30))
                return "投资时间不满30个自然日！";
            if (investInfo.is_invest_succ)
                return "数据正在处理中,请稍后再试";

            #endregion 投资限制

            #region 标限制

            loanInfo = _loanInfoService.Find(investInfo.pro_loan_id.Value);
            var loanTransferApplys = _proTranferApplyService.GetListByLoanId(loanInfo.Id.ToString());
            var proStartTime = loanInfo.pro_start_date.GetValueOrDefault();
            var transType = _projectSettings.sys_transfer_type;
            var transCycle = _projectSettings.sys_transfer_cycle;
            var transCount = _projectSettings.sys_transfer_count.GetValueOrDefault();
            var transTypeName = "天";
            if (transType == DataDictionary.deadlinetype_Day)
            {
                proStartTime = proStartTime.AddDays(transCycle);
            }
            else
            {
                proStartTime = proStartTime.AddMonths(transCycle);
                transTypeName = "个月";
            }
            if (proStartTime.Date >= DateTime.Now.Date && loanTransferApplys.Count() == 0 && transCycle != 0)
                return "放款后" + transCycle + transTypeName + "内不能进行首次债权转让";
            if (loanTransferApplys.Count() >= transCount && transCount != 0)
                return "该标转让的次数已达到系统设置的转让次数" + transCount + "次,不能进行再转让";
            var loanPlans = _loanPlanService.GetLoanPlansByCondition(investInfo.pro_loan_id.Value);
            if (!((Convert.ToDateTime(loanPlans.Where(o => !o.pro_is_clear).Min(x => x.pro_pay_date)) - DateTime.Now).Days + 1 > 3))
                return "标的还款期（还息或者还本）前3个自然日不可转让！";
            if (!((DateTime.Now - Convert.ToDateTime(loanPlans.Where(o => o.pro_is_clear).Max(x => x.pro_pay_date))).Days + 1 > 3))
                return "标的还款期（还息或者还本）后3个自然日不可转让！";
            if (!(Convert.ToDateTime(loanPlans.Where(o => !o.pro_is_clear).Max(x => x.pro_pay_date)).Date > DateTime.Now.AddDays(14).Date))
                return "距离最后一次还本日期前14个自然日内不可转让！";
            if (loanPlans.Where(p => p.pro_is_use).Count() > 0)
                return "数据正在处理中,请稍后再试";

            #endregion 标限制

            return string.Empty;
        }

        /// <summary>
        /// 债转申请
        /// </summary>
        /// <returns></returns>
        private bool DoTranster(PRO_invest_info investInfo, decimal discount, decimal idualMoney, int userId, PRO_loan_info loanInfo, int transferPeriod)
        {
            var _projectSettings = _settingService.LoadSetting<ProjectSettings>();
            investInfo.pro_is_use = true;
            _investInfoService.Update(investInfo);
            using (var scope = new TransactionScope())
            {
                lock (_lock)
                {
                    try
                    {
                        PRO_transfer_apply app = new PRO_transfer_apply();
                        var transferFee = 0.0m;
                        var discountRate = discount / 100;
                        if (_projectSettings.sys_transfer_rate != 0)
                        {
                            transferFee = _projectSettings.sys_transfer_rate * idualMoney / 100;
                        }

                        app.pro_transfer_fee = transferFee;
                        decimal deductFee = idualMoney - (decimal)(idualMoney * discountRate + transferFee);

                        decimal transferRate = (loanInfo.pro_loan_rate.GetValueOrDefault() * idualMoney) / (decimal)(idualMoney * discountRate + transferFee);
                        app.pro_loan_id = loanInfo.Id;
                        app.pro_invest_id = investInfo.Id;
                        app.pro_transfer_date = DateTime.Now.Date;
                        app.pro_surplus_money = idualMoney * discountRate;
                        app.pro_idual_money = idualMoney;
                        app.pro_transfer_period = transferPeriod;
                        app.pro_transfer_rate = transferRate;
                        app.pro_transfer_money = idualMoney * discountRate;
                        app.pro_transfer_harf_rate = discountRate * 100;
                        app.pro_transfer_deduct_fee = deductFee;
                        app.pro_transfer_state = DataDictionary.transferstatus_Pendingaudit;
                        app.pro_user_id = userId;
                        app.pro_is_del = false;
                        _proTranferApplyService.Add(app);

                        var transAppList = _proTranferApplyService.GetListNodel(userId.ToString(), loanInfo.Id.ToString());
                        foreach (PRO_transfer_apply appTrans in transAppList)
                        {
                            appTrans.pro_is_del = true;
                            _proTranferApplyService.Update(appTrans);
                        }
;
                        //插入转让跟踪记录
                        _proLoanFlowService.FlowAdd(0, (int)app.Id, DataDictionary.transferstatus_Pendingaudit, DataDictionary.LoanStepName_Assignment, "转让申请", userId);

                        scope.Complete();
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                    finally
                    {
                        investInfo.pro_is_use = false;
                        _investInfoService.Update(investInfo);
                        scope.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// 债权转让申请撤回
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<bool, string> RecallTransfer([FromBody]SRecallTransfer model)
        {
            var rModel = new ReturnModel<bool, string>();

            #region 验签

            var resultsign = VerifyBase.SignAndToken<SRecallTransfer>(model, out int userId);
            if (!resultsign.Equals(ReturnCode.Success))
            {
                rModel.ReturnCode = (int)resultsign;
                rModel.Message = "签名验证失败";
                return rModel;
            }

            #endregion 验签

            PRO_transfer_apply trans = _proTranferApplyService.Find(model.Id);

            if (trans == null || ((int)trans.pro_transfer_state != DataDictionary.transferstatus_Pendingaudit))
            {
                rModel.Message = "没有可撤回的转让债权！";
                rModel.ReturnCode = (int)ReturnCode.DataEorr;
                rModel.ReturnData = false;
                return rModel;
            }

            using (var scope = new TransactionScope())
            {
                //插入转让跟踪记录
                _proLoanFlowService.FlowAdd(0, model.Id, DataDictionary.transferstatus_Pendingaudit, DataDictionary.LoanStepName_Assignment, "转让申请撤回", userId);
                trans.pro_is_del = true;
                _proTranferApplyService.Update(trans);
                scope.Complete();
            }

            rModel.Message = "撤回成功！";
            rModel.ReturnCode = (int)ReturnCode.Success;
            rModel.ReturnData = true;
            return rModel;
        }
    }
}