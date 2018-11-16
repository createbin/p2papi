using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using ZFCTAPI.Core;
using ZFCTAPI.Core.Configuration;
using ZFCTAPI.Core.DbContext;
using ZFCTAPI.Core.Enums;
using ZFCTAPI.Core.Helpers;
using ZFCTAPI.Data.ApiModels.ReturnModels;
using ZFCTAPI.Data.ApiModels.SubmitModels;
using ZFCTAPI.Data.BoHai.SubmitModels;
using ZFCTAPI.Data.CST;
using ZFCTAPI.Data.PRO;
using ZFCTAPI.Data.Records;
using ZFCTAPI.Data.Repayment;
using ZFCTAPI.Services.BoHai;
using ZFCTAPI.Services.InvestInfo;
using ZFCTAPI.Services.LoanInfo;
using ZFCTAPI.Services.Popular;
using ZFCTAPI.Services.Sys;
using ZFCTAPI.Services.Transaction;
using ZFCTAPI.Services.Transfer;
using ZFCTAPI.Services.UserInfo;

namespace ZFCTAPI.Services.Repayment
{
    /// <summary>
    /// 资金划转
    /// </summary>
    public interface ICapitalTransferService
    {
        /// <summary>
        /// 满标处理成功
        /// </summary>
        /// <param name="loanInfo"></param>
        void ReleaseSuccess(string merBillNo, string msg);

        /// <summary>
        /// 满标处理失败
        /// </summary>
        /// <param name="merBillNo"></param>
        void ReleaseFail(string merBillNo, string msg);

        /// <summary>
        /// 流标处理成功
        /// </summary>
        /// <param name="loanInfo"></param>
        /// <param name="msg">成功描述</param>
        void CancelSuccess(string merBillNo,string msg);

        /// <summary>
        /// 流标处理失败
        /// </summary>
        /// <param name="merBillNo"></param>
        /// <param name="msg">失败描述</param>
        void CancelFail(string merBillNo,string msg);

        /// <summary>
        /// 还款成功处理
        /// </summary>
        /// <param name="loanPlan"></param>
        /// <returns></returns>
        bool RepaymentSuccess(string merBillNo,string msg);

        /// <summary>
        /// 还款失败处理
        /// </summary>
        /// <param name="loanPlan"></param>
        /// <returns></returns>
        bool RepaymentFail(string merBillNo,string msg);

        /// <summary>
        /// 解除单个投资用户金额
        /// </summary>
        /// <param name="merBillNo"></param>
        /// <returns></returns>
        bool CancelInvest(string merBillNo);
    }

    public class CapitalTransferService : ICapitalTransferService
    {
        #region fields

        private readonly IInvestInfoService _iinvestInfoService;
        private readonly ILoanInfoService _iloanInfoService;
        private readonly IProLoanFlowService _proLoanFlowService;
        private readonly ILoanPlanService _iloanPlanService;
        private readonly IInvesterPlanService _iinvesterPlanService;
        private readonly IPayRecordService _payRecordService;
        private readonly IRedFunctionService _redFunctionService;
        private readonly IPopEnvelopeRedService _popEnvelopeRedService;
        private readonly ILendRecordService _lendRecordService;
        private readonly ICstUserInfoService _cstUserInfoService;
        private readonly ICstRedInfoService _cstRedInfoService;
        private readonly IInvestInfoService _investInfoService;
        private readonly IProIntentCheck _proIntentCheck;

        private readonly IDbContext _dbContext;
        private readonly ItbWechatService _itbwechatservice;
        private readonly IAccountInfoService _iaccountInfoService;
        private readonly IBHRepaymentService _ibhrepaymentService;
        private readonly BoHaiApiConfig _boHaiApiConfig;

        private readonly ICompanyInfoService _companyInfoService;

        private readonly ICstTransactionService _cstTransactionService;

        private static object lockObj = new object();

        public CapitalTransferService(IInvestInfoService iinvestInfoService,
            ILoanInfoService iloanInfoService,
            IProLoanFlowService proLoanFlowService,
            ILoanPlanService iloanPlanService,
            IInvesterPlanService iinvesterPlanService,
            IPayRecordService payRecordService,
            IRedFunctionService redFunctionService,
            IPopEnvelopeRedService popEnvelopeRedService,
            ILendRecordService lendRecordService,
            ICstUserInfoService cstUserInfoService,
            IInvestInfoService investInfoService,
            IProIntentCheck proIntentCheck,
            ICstRedInfoService cstRedInfoService,
            IDbContext dbContext,
            ItbWechatService itbwechatservice,
            IAccountInfoService iaccountInfoService,
            BoHaiApiConfig boHaiApiConfig,
            IBHRepaymentService ibhrepaymentService,
            ICompanyInfoService companyInfoService,
            ICstTransactionService cstTransactionService)
        {
            _iinvestInfoService = iinvestInfoService;
            _iloanInfoService = iloanInfoService;
            _proLoanFlowService = proLoanFlowService;
            _iloanPlanService = iloanPlanService;
            _iinvesterPlanService = iinvesterPlanService;
            _payRecordService = payRecordService;
            _redFunctionService = redFunctionService;
            _popEnvelopeRedService = popEnvelopeRedService;
            _lendRecordService = lendRecordService;
            _cstUserInfoService = cstUserInfoService;
            _cstRedInfoService = cstRedInfoService;
            _investInfoService = investInfoService;
            _proIntentCheck = proIntentCheck;
            _dbContext = dbContext;
            _itbwechatservice = itbwechatservice;
            _iaccountInfoService = iaccountInfoService;
            _boHaiApiConfig = boHaiApiConfig;
            _ibhrepaymentService = ibhrepaymentService;
            _companyInfoService = companyInfoService;
            _cstTransactionService = cstTransactionService;
        }

        #endregion

        /// <summary>
        /// 满标处理
        /// </summary>
        /// <param name="loanInfo"></param>
        public void ReleaseSuccess(string merBillNo, string msg)
        {
            lock (lockObj)
            {
                bool isSuccess = true;
                //标的信息
                var loanInfo = _iloanInfoService.GetLoanInfoByMerBillNo(merBillNo);
                //判断满标状态
                if (loanInfo == null || loanInfo.Type == null || !loanInfo.Type.Value)
                {
                    LogsHelper.WriteLog("标的信息不存在，或标的状态不正取。流水号：" + merBillNo);
                    return;
                }
                //投资记录
                var listInvest = _iinvestInfoService.GetInvestListByLoanId(loanInfo.Id, null);
                //还款计划
                var loanPlans = _iloanPlanService.GetLoanPlansByCondition(loanInfo.Id);
                //回款计划
                var investPlans = _iinvesterPlanService.GetInvesterPlanByLoanId(loanInfo.Id);
                //是否是第一次借款
                var isfirstApply = _iloanInfoService.GetProLoanInfoListByUserId(loanInfo.pro_add_emp.Value);
                var transOptionH = new TransactionOptions();
                transOptionH.IsolationLevel = IsolationLevel.ReadCommitted;
                //using (var scope = _dbContext.Conn.BeginTransaction())
                //{
                var tip = "";

                try
                {
                    #region 更新标的信息

                    loanInfo.pro_is_use = false;
                    loanInfo.pro_loan_state = DataDictionary.projectstate_Repayment;
                    loanInfo.pro_invest_contract_status = 8;
                    //开始日期(开始日期即放款日期)
                    loanInfo.pro_start_date = (loanInfo.pro_start_date == null ? Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")) : loanInfo.pro_start_date);
                    loanInfo.pro_end_date = (loanInfo.pro_end_date == null ? _iloanInfoService.GetEndDateByStartDate((int)loanInfo.pro_period_type, (DateTime)loanInfo.pro_start_date, Convert.ToInt32(loanInfo.pro_loan_period)) : loanInfo.pro_end_date);
                    loanInfo.bank_resp_desc = msg;
                    _iloanInfoService.Update(loanInfo);

                    #endregion

                    #region 更新投资计划表

                    foreach (var item in listInvest)
                    {
                        item.pro_frozen_state = false;
                        item.pro_transfer_state = true;
                        _iinvestInfoService.Update(item);
                    }

                    #endregion

                    #region 生成还款计划表

                    if (loanPlans == null || !loanPlans.Any())
                    {
                        loanPlans = _iloanPlanService.CreateRepaymentSchedule(loanInfo, (int)loanInfo.pro_loan_period, ref tip, true, loanInfo.pro_this_servicefee ?? 0);
                        if (loanPlans == null || !loanPlans.Any())
                        {
                            LogsHelper.WriteLog("生成还款计划表错误。流水号：" + merBillNo);
                            return;
                        }
                    }

                    #endregion

                    #region 生成回款计划表

                    if (investPlans == null || !investPlans.Any())
                    {
                        investPlans = _iinvesterPlanService.RecoveryPlanTable(loanInfo, listInvest, loanPlans, ref tip, true);
                    }

                    #endregion

                    #region 红包划转

                    _popEnvelopeRedService.TransferLoanRed(loanInfo);

                    #endregion

                    #region 投资人红包奖励

                    foreach (var invester in listInvest)
                    {
                        var customerId = _cstUserInfoService.Find((int)invester.pro_invest_emp).cst_customer_id.Value;
                        if (_iinvestInfoService.ValidateFirstInvest((int)invester.pro_invest_emp))
                        {
                            //首次投资红包奖励
                            _redFunctionService.GrantRedEnvelope(GrantType.FirstInvestment, customerId, loanInfo.Id, invester.pro_invest_money.Value);
                        }
                        _redFunctionService.GrantRedEnvelope(GrantType.Product, customerId, loanInfo.Id, invester.pro_invest_money.Value);
                        _redFunctionService.GrantRedEnvelope(GrantType.Investment, customerId, loanInfo.Id, invester.pro_invest_money.Value);
                    }

                    #endregion

                    #region 借款人红包奖励

                    var cstInfo = _cstUserInfoService.Find(loanInfo.pro_add_emp.Value);
                    int loanCustId = cstInfo.cst_customer_id.Value;

                    if (isfirstApply == null || isfirstApply.Count == 0)
                    {
                        //首次借款红包奖励
                        _redFunctionService.GrantRedEnvelope(GrantType.FirstBorrowing, loanCustId, loanInfo.Id, (decimal)loanInfo.pro_loan_money);
                    }
                    _redFunctionService.GrantRedEnvelope(GrantType.Borrowing, loanCustId, loanInfo.Id, (decimal)loanInfo.pro_loan_money);

                    #endregion

                    #region 添加日志

                    var record = new PRO_lend_record();
                    record.len_loan_id = loanInfo.Id;
                    record.len_pay_fee = 0;//应收手续费
                    record.len_pay_guar_fee = 0;//应收担保费
                    record.len_collect_fee = 0;
                    record.len_collect_guar_fee = 0;
                    record.len_lend_money = 0;//实际划转金额，计算
                    record.len_lend_date = DateTime.Now.Date;
                    record.len_user_id = loanInfo.pro_add_emp;
                    record.len_loan_type = DataDictionary.investType_normal;

                    _lendRecordService.Add(record);

                    #endregion

                    #region 添加操作日志

                    _proLoanFlowService.Add(new PRO_loan_flow()
                    {
                        pro_loan_id = loanInfo.Id,
                        pro_loan_stateId = DataDictionary.projectstate_Repayment,
                        pro_step_id = DataDictionary.LoanStepName_CreditTransferred,
                        pro_flow_mark = "满标划转",
                        pro_operate_emp = 1,//划转后台用户
                        pro_operate_date = DateTime.Now
                    });

                    #endregion

                    #region 满标划转日志

                    _cstTransactionService.Add(new CST_transaction_info
                    {
                        pro_user_id = loanInfo.pro_add_emp,
                        pro_user_type = loanInfo.pro_add_emp_type,
                        pro_loan_id = loanInfo.Id,
                        pro_transaction_time = loanInfo.CreDt,
                        pro_transaction_no = merBillNo,
                        pro_transaction_remarks = "满标划转",
                        pro_complete_time = DateTime.Now,
                        pro_transaction_status = DataDictionary.transactionstatus_success,
                        pro_transaction_type = DataDictionary.transactiontype_InvestTransfer,
                        pro_platforms_fee = loanInfo.pro_this_procedurefee,
                        pro_transaction_money= loanInfo.pro_loan_money.Value- loanInfo.pro_this_procedurefee.GetValueOrDefault()
                    });

                    _cstTransactionService.Add(new CST_transaction_info
                    {
                        pro_user_id = loanInfo.pro_add_emp,
                        pro_user_type = loanInfo.pro_add_emp_type,
                        pro_loan_id = loanInfo.Id,
                        pro_transaction_time = loanInfo.CreDt,
                        pro_transaction_no = merBillNo,
                        pro_transaction_remarks = "满标划转手续费",
                        pro_complete_time = DateTime.Now,
                        pro_transaction_status = DataDictionary.transactionstatus_success,
                        pro_transaction_type = DataDictionary.feetype_CounterFee,
                        pro_platforms_fee = loanInfo.pro_this_procedurefee,
                        pro_transaction_money = loanInfo.pro_this_procedurefee
                    });

                    #endregion

                    //scope.Commit();
                }
                catch (Exception e)
                {
                    isSuccess = false;
                    if (!string.IsNullOrEmpty(tip))
                    {
                        LogsHelper.WriteLog(tip + ":流水号：" + merBillNo);
                    }
                    LogsHelper.WriteLog("满标异步处理出错" + e.ToString() + "。流水号：" + merBillNo);
                    //scope.Rollback();
                }
                //}

                if (isSuccess)
                {
                    PostReleaseDataToMerchantControl(merBillNo, true);
                }
            }
        }


        /// <summary>
        /// 流标处理
        /// </summary>
        /// <param name="loanInfo"></param>
        public void CancelSuccess(string merBillNo,string msg)
        {
            lock (lockObj)
            {
                bool isSuccess = true;
                var loanInfo = _iloanInfoService.GetLoanInfoByMerBillNo(merBillNo);
                if(loanInfo==null||(loanInfo.pro_loan_state!= DataDictionary.bank_state_using))
                {
                    LogsHelper.WriteLog("流标异步回调标的信息错误：" + loanInfo == null ? "标的信息不存在" : "标的状态错误");
                    return;
                }
                var investerList = _investInfoService.GetInvestListByLoanId(loanInfo.Id, null);
                var redList = _cstRedInfoService.LoanRedList(loanInfo.Id);

                //解冻投资金额成功修改状态
                using(var scope = _dbContext.Conn.BeginTransaction())
                {
                    try
                    {
                        if (investerList != null&& investerList.Any())
                        {
                            foreach (var invester in investerList)
                            {
                                invester.pro_delsign = true;
                                _investInfoService.Update(invester, scope);
                            }
                        }
                        if(redList!=null&& redList.Any())
                        {
                            foreach (var red in redList)
                            {
                                red.cst_red_investId = null;
                                red.cst_red_employ = false;
                                _cstRedInfoService.Update(red, scope);
                            }
                        }
                        loanInfo.pro_loan_state = DataDictionary.bank_state_unfreeze;
                        loanInfo.pro_is_use = false;
                        _iloanInfoService.Update(loanInfo, scope);
                        scope.Commit();
                    }
                    catch (Exception e)
                    {
                        isSuccess = false;
                        scope.Rollback();
                        LogsHelper.WriteLog("报错时间："+DateTime.Now+"，流水号:"+merBillNo+"流标异步报错"+e.ToString());
                    }
                }

                if (isSuccess)
                {
                    //撤销标的
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
                    if (bhRaiseResult != null && bhRaiseResult.RspSvcHeader.returnCode == JSReturnCode.Success)
                    {
                        loanInfo.pro_loan_state = DataDictionary.projectstate_FlowStandard;
                        loanInfo.tran_seq_no = loanInfo.tran_seq_no + "|" + model.ReqSvcHeader.tranSeqNo;
                    }
                    else
                    {
                        loanInfo.pro_loan_state = DataDictionary.bank_state_cancel_failed;
                    }
                    //更新项目信息
                    _iloanInfoService.Update(loanInfo);
                }
            }
        }

        /// <summary>
        /// 还款成功
        /// </summary>
        /// <param name="loanPlan"></param>
        /// <returns></returns>
        public bool RepaymentSuccess(string merBillNo,string msg)
        {
            lock (lockObj)
            {
                bool isSuccess = true;

                var loanPlan = _iloanPlanService.GetLastLoanPlan(merBillNo);
                var loanInfo = _iloanInfoService.GetProReleaseById(loanPlan.pro_loan_id.Value);

                var transOptionH = new TransactionOptions();
                transOptionH.IsolationLevel = IsolationLevel.ReadCommitted;
                //using (var scope = _dbContext.Conn.BeginTransaction())
                //{
                try
                {
                    #region 更新 PRO_loan_plan

                    loanPlan.pro_is_clear = true;
                    loanPlan.pro_is_use = false;
                    loanPlan.pro_collect_date = DateTime.Now;
                    loanPlan.bank_resp_desc = msg;
                    _iloanPlanService.Update(loanPlan);

                    #endregion 更新 PRO_loan_plan

                    #region 更新 PRO_loan_info

                    if (_iloanPlanService.IsLastLoanPlan(loanInfo.Id, loanPlan.Id))
                    {
                        //全部还完
                        loanInfo.pro_loan_state = DataDictionary.projectstate_Settled;
                        _iloanInfoService.Update(loanInfo);
                    }

                    #endregion 更新 PRO_loan_info

                    #region 更新 PRO_invest_info

                    //var investInfo = new PRO_invest_info();
                    //_iinvestInfoService.Update(investInfo);

                    #endregion 更新 PRO_invest_info

                    #region 更新 PRO_invester_plan

                    var investerPlans = _iinvesterPlanService.GetListByPlanId(loanPlan.Id);
                    if (investerPlans != null && investerPlans.Any())
                    {
                        foreach (var investerPlan in investerPlans)
                        {
                            investerPlan.pro_is_clear = true;
                            investerPlan.pro_collect_date = DateTime.Now;
                            _iinvesterPlanService.Update(investerPlan);
                        }
                    }

                    #endregion 更新 PRO_invester_plan

                    #region 红包添加

                    var customerId = _cstUserInfoService.Find((int)loanInfo.pro_add_emp).cst_customer_id.Value;
                    if (_iloanPlanService.IsFirstLoanPlan((int)loanInfo.pro_add_emp))
                    {
                        //第一次还款送红包
                        _redFunctionService.GrantRedEnvelope(GrantType.FirstRepayment, customerId, loanInfo.Id, (decimal)loanPlan.pro_collect_money);
                    }
                    if (loanPlan.pro_pay_date > DateTime.Now)
                    {
                        //提前还款送红包
                        _redFunctionService.GrantRedEnvelope(GrantType.Prepayment, customerId, loanInfo.Id, (decimal)loanPlan.pro_collect_money);
                    }
                    //还款送红包
                    _redFunctionService.GrantRedEnvelope(GrantType.Repayment, customerId, loanInfo.Id, (decimal)loanPlan.pro_collect_money);

                    #endregion

                    #region 添加还款记录 PRO_pay_record

                    var payRecord = new PRO_pay_record()
                    {
                        pro_loan_id = loanPlan.pro_loan_id,//项目申请表主键
                        pro_pay_date = DateTime.Now,//还款日期
                        pro_pay_money = loanPlan.pro_collect_money,//归还本金
                        pro_pay_rate = loanPlan.pro_collect_rate,//归还利息
                        pro_pay_over_rate = loanPlan.pro_collect_over_rate,//归还罚金
                        pro_pay_service_fee = loanPlan.pro_collect_service_fee,//服务费待定
                        pro_pay_guar_fee = 0.00m,
                        pro_sett_over_rate = loanPlan.pro_sett_over_rate,//剩余罚金
                        pro_pay_period = loanPlan.pro_loan_period,//归还期数
                        pro_pay_type = (int)DataDictionary.repaymenstate_Normal//还款状态(1：正常还款 2：平台代还 3：强制还款)
                    };
                    _payRecordService.Add(payRecord);

                    #endregion 添加还款记录 PRO_pay_record

                    #region 还款记录

                    _cstTransactionService.Add(new CST_transaction_info
                    {
                        pro_user_id=loanInfo.pro_add_emp,
                        pro_user_type=loanInfo.pro_add_emp_type,
                        pro_loan_id=loanInfo.Id,
                        pro_transaction_time=loanPlan.CreDt,
                        pro_transaction_no=merBillNo,
                        pro_transaction_remarks="还款成功",
                        pro_complete_time=DateTime.Now,
                        pro_transaction_status= DataDictionary.transactionstatus_success,
                        pro_transaction_type= DataDictionary.transactiontype_repayment,
                        pro_platforms_fee =loanPlan.pro_collect_service_fee.GetValueOrDefault(),
                        pro_transaction_money=loanPlan.pro_collect_total.GetValueOrDefault()
                    });

                    _cstTransactionService.Add(new CST_transaction_info
                    {
                        pro_user_id = loanInfo.pro_add_emp,
                        pro_user_type = loanInfo.pro_add_emp_type,
                        pro_loan_id = loanInfo.Id,
                        pro_transaction_time = loanInfo.CreDt,
                        pro_transaction_no = merBillNo,
                        pro_transaction_remarks = "还款手续费",
                        pro_complete_time = DateTime.Now,
                        pro_transaction_status = DataDictionary.transactionstatus_success,
                        pro_transaction_type = DataDictionary.feetype_CounterFee,
                        pro_platforms_fee = loanPlan.pro_collect_service_fee.GetValueOrDefault(),
                        pro_transaction_money = loanPlan.pro_collect_service_fee.GetValueOrDefault()
                    });

                    #endregion
                }
                catch (Exception e)
                {
                    isSuccess = false;
                    LogsHelper.WriteLog("还款出错" + e.ToString() + ",流水号：" + merBillNo);
                    return false;
                }

                if (isSuccess)
                {
                    PostRepaymentDataToMerchantControl(merBillNo, true);
                }
            }
            return true;
        }

        public bool RepaymentFail(string merBillNo,string msg)
        {
            var loanPlan = _iloanPlanService.GetLastLoanPlan(merBillNo);

            if (loanPlan == null)
                return false;
            
            //商户控台数据
            
            //loanPlan.pro_collect_rate = 0;
            //loanPlan.pro_collect_over_rate = 0;
            //loanPlan.pro_collect_total = 0;
            loanPlan.pro_is_use = false;
            loanPlan.pro_collect_date = null;
            loanPlan.bank_resp_desc = msg;
            _iloanPlanService.Update(loanPlan);

            PostRepaymentDataToMerchantControl(merBillNo, false);
            return true;
        }

        public void ReleaseFail(string merBillNo, string msg)
        {
            var loanInfo = _iloanInfoService.GetLoanInfoByMerBillNo(merBillNo);
            if (loanInfo == null)
                return;

            loanInfo.pro_is_use = false;
            loanInfo.bank_resp_desc = msg;
            loanInfo.pro_loan_state = DataDictionary.projectstate_StayTransfer;

            _iloanInfoService.Update(loanInfo);

            PostReleaseDataToMerchantControl(merBillNo, false);
        }

        #region private

        /// <summary>
        /// 将放款数据传商户控制台
        /// </summary>
        /// <param name="merBillNo">流水号</param>
        /// <param name="success">放款是否成功</param>
        private void PostReleaseDataToMerchantControl(string merBillNo, bool success)
        {
            //标的信息
            var loanInfo = _iloanInfoService.GetLoanInfoByMerBillNo(merBillNo);
            var isCompany = loanInfo.pro_add_emp_type.GetValueOrDefault() == 10;
            //借款人信息
            var userInfo = _cstUserInfoService.GetCompleteUserAccount(loanInfo.pro_add_emp);
            //投资人列表
            var invester = _payRecordService.GetInvestPersons(loanInfo.Id);
            var accountInfo = userInfo.AccountInfo;
            var chargeAccount = new ChargeAccount();

            if (loanInfo.pro_add_emp_type.GetValueOrDefault() == 10)
            {
                chargeAccount = _companyInfoService.GetChargeAccount(companyId: userInfo.CstCompanyInfo.Id);
            }

            var investorLendings = invester.Select(item =>
            {
                InvestorLendingRecord record = new InvestorLendingRecord();
                record.MerBillNo = merBillNo;
                record.BorrowId = loanInfo.Id;
                record.CreDt = loanInfo.CreDt == null ? DateTime.Now : loanInfo.CreDt.Value;
                record.CreditPlaCustId = accountInfo.cst_plaCustId;
                record.CreditName = isCompany? chargeAccount.AccountName: accountInfo.act_legal_name;
                record.OutgoingPlaCustId = item.cst_plaCustId;
                record.OutgoingName = item.cst_plaCustName;
                record.TransAmt = item.Pro_invest_money;
                record.FreezeId = item.Pro_fro_orderno;
                record.CreateTime = DateTime.Now;
                return record;
            }).ToList();

            var lenderLending = new LenderLendingRecord
            {
                BorrowId = loanInfo.Id,
                MerBillNo = merBillNo,
                BorrowNo = loanInfo.pro_loan_no,
                BorrowerAmt = loanInfo.pro_loan_money.Value,
                ReleaseType = "0",
                BorrowPlaCustId = accountInfo.cst_plaCustId,
                BorrowCustName = isCompany ? chargeAccount.AccountName : accountInfo.act_legal_name,
                BorrowFee = loanInfo.pro_this_procedurefee == null ? "0" : loanInfo.pro_this_procedurefee.Value.ToString(),
                CreateTime = DateTime.Now,
                Success = success,
                Attribute = isCompany ? 2 : 1,
                InvesterCount = investorLendings == null ? 0 : investorLendings.Count,
                BankDesc=loanInfo.bank_resp_desc
            };

            var subModel = new SubmitRaiseLoanModel
            {
                LenderLending = lenderLending,
                InvestorLendings = investorLendings
            };

            var url = _boHaiApiConfig.MerchantControlAddress + _boHaiApiConfig.Interfaces.First(i => i.Name == "MerchantControlRaiseLoan").ActionUrl;
            var json = JsonConvert.SerializeObject(subModel);

            try
            {
                var result = HttpClientHelper.PostAsync(url, json).Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception e)
            {
                LogsHelper.WriteLog("满标异步商户控制台数据保存出错" + e.ToString() + "。流水号：" + merBillNo + ", 保存数据：" + JsonConvert.SerializeObject(subModel));
            }
        }

        /// <summary>
        /// 将还款数据传商户控制台
        /// </summary>
        private void PostRepaymentDataToMerchantControl(string merBillNo, bool success)
        {
            //还款人是否为公司
            bool isCompanyRep = false;
            //还款计划
            var loanPlan = _iloanPlanService.GetLastLoanPlan(merBillNo);
            //标的信息
            var loanInfo = _iloanInfoService.GetProReleaseById(loanPlan.pro_loan_id.Value);
            //还款人信息
            CST_account_info accountInfo = null;
            ChargeAccount chargeAccount = null;

            //担保人还款
            if(loanPlan.pro_pay_type== DataDictionary.RepaymentType_PlatformDaihuan)
            {
                isCompanyRep = true;
                accountInfo = _iaccountInfoService.GetAccountInfoByCompanyId(loanInfo.pro_loan_guar_company.Value);
                chargeAccount = _companyInfoService.GetChargeAccount(companyId: loanInfo.pro_loan_guar_company.Value);
            }
            else
            {
                var userInfo = _cstUserInfoService.GetCompleteUserAccount(loanInfo.pro_add_emp);
                //如果借款人为企业户，商户控台用户名称保存公司名称
                if (loanInfo.pro_add_emp_type.GetValueOrDefault() == 10)
                {
                    isCompanyRep = true;
                    accountInfo = _iaccountInfoService.GetAccountInfoByCompanyId(userInfo.CstCompanyInfo.Id);
                    chargeAccount = _companyInfoService.GetChargeAccount(companyId: userInfo.CstCompanyInfo.Id);
                }
                else
                {
                    accountInfo = userInfo.AccountInfo;
                }
            }

            //投资人信息
            var investerList = _payRecordService.GetInvesterByPlanId(loanPlan.Id,true);
            var investers = investerList.Select(item =>
            {
                var record = new InvestorRepaymentRecord();
                var temp = _iinvesterPlanService.Find(item.InvestPlanId);
                record.MerBillNo = merBillNo;
                record.BorrowId = loanInfo.Id;
                record.BorrowNo = loanInfo.pro_loan_no;
                record.BorrowPeriod = loanPlan.pro_loan_period.Value;
                record.CreDt = loanPlan.CreDt == null ? DateTime.Now : loanPlan.CreDt.Value;
                record.CreditPlaCustId = item.cst_plaCustId;
                record.CreditName = item.cst_plaCustName;
                record.OutgoingPlaCustId = accountInfo.cst_plaCustId;
                record.OutgoingName = isCompanyRep? chargeAccount.AccountName: accountInfo.act_legal_name;
                record.TransAmt = loanPlan.pro_pay_money == null ? 0 : loanPlan.pro_pay_money.Value;
                record.Interest = temp.pro_collect_rate == null ? "0" : temp.pro_collect_rate.Value.ToString();
                record.PenaltyInterest = temp.pro_collect_over_rate == null ? "0" : temp.pro_collect_over_rate.Value.ToString();
                record.CreateTime = DateTime.Now;
                return record;
            }).ToList();

            var lender = new LenderRepaymentRecord
            {
                BorrowId = loanInfo.Id,
                MerBillNo = merBillNo,
                BorrowNo = loanInfo.pro_loan_no,
                BorrowPeriod = loanPlan.pro_loan_period.Value,
                TransAmt = loanPlan.pro_collect_total.Value,
                FeeAmt = loanPlan.pro_collect_service_fee.Value,
                BorrowPlaCustId = accountInfo.cst_plaCustId,
                BorrowCustName = isCompanyRep? chargeAccount.AccountName : accountInfo.act_legal_name,
                Attribute = isCompanyRep ? 2 : 1,
                Success = success,
                CreateTime = DateTime.Now,
                BankDesc = loanInfo.bank_resp_desc,
                InvesterCount= investers==null?0: investers.Count
            };

            var subModel = new SubmitRepaymentModel
            {
                Lender = lender,
                Investors = investers
            };

            var url = _boHaiApiConfig.MerchantControlAddress + _boHaiApiConfig.Interfaces.First(i => i.Name == "MerchantControlRepayment").ActionUrl;
            var json = JsonConvert.SerializeObject(subModel);
            try
            {
                var result = HttpClientHelper.PostAsync(url, json).Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception e)
            {
                LogsHelper.WriteLog("满标异步商户控制台数据保存出错" + e.ToString() + "。流水号：" + merBillNo+",保存数据："+JsonConvert.SerializeObject(subModel));
            }
        }

        public void CancelFail(string merBillNo,string msg)
        {
            var loanInfo = _iloanInfoService.GetLoanInfoByMerBillNo(merBillNo);
            loanInfo.pro_is_use = false;
            loanInfo.bank_resp_desc = msg;
            _iloanInfoService.Update(loanInfo);
        }

        public bool CancelInvest(string merBillNo)
        {
            var investInfo = _iinvestInfoService.GetInvestInfoByMerBillNo(merBillNo);
            //更新投资标
            investInfo.pro_delsign = true;
            investInfo.is_invest_succ = false;
            _iinvestInfoService.Update(investInfo);

            //更新红包标
            var redInfo = _cstRedInfoService.GetRedInfoByInvestId(investInfo.Id);
            if (redInfo != null && redInfo.Id > 0)
            {
                redInfo.cst_red_investId = null;
                redInfo.cst_cancel_investId = investInfo.Id;
                _cstRedInfoService.Update(redInfo);
            }

            //更新标的信息表
            var loanInfo = _iloanInfoService.Find(investInfo.pro_loan_id.Value);
            loanInfo.pro_surplus_money += investInfo.pro_invest_money;
            loanInfo.pro_loan_speed = decimal.Round((decimal)(((loanInfo.pro_loan_money - loanInfo.pro_surplus_money) / loanInfo.pro_loan_money) * 100), 2);
            if (loanInfo.pro_loan_state == DataDictionary.projectstate_FullScalePending)
            {
                loanInfo.pro_loan_state = DataDictionary.projectstate_Tender;
            }
            _iloanInfoService.Update(loanInfo);
            return true;
        }

        #endregion
    }
}
