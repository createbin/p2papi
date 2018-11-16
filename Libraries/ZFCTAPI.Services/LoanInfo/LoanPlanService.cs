using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using ZFCTAPI.Data.PRO;
using ZFCTAPI.Services.Repositorys;
using ZFCTAPI.Data.LoanInfo;
using ZFCTAPI.Core.Enums;
using ZFCTAPI.Services.Settings;
using ZFCTAPI.Core.Configuration.DataBase;
using ZFCTAPI.Data.Pages;
using ZFCTAPI.Data.ApiModels.ReturnModels;
using ZFCTAPI.Data.Interest;
using ZFCTAPI.Core;
using ZFCTAPI.Services.Repayment;
using ZFCTAPI.Services.InvestInfo;
using ZFCTAPI.Core.Helpers;
using System.Data;

namespace ZFCTAPI.Services.LoanInfo
{
    public interface ILoanPlanService : IRepository<PRO_loan_plan>
    {
        PRO_loan_plan GetLastLoanPlan(int loanId);

        /// <summary>
        /// 根据流水号查找
        /// </summary>
        /// <param name="merBillNo"></param>
        /// <returns></returns>
        PRO_loan_plan GetLoanByMerBillNo(string merBillNo);

        /// <summary>
        /// 根据流水号查询
        /// </summary>
        /// <param name="merBillNo"></param>
        /// <returns></returns>
        PRO_loan_plan GetLastLoanPlan(string merBillNo);

        List<PRO_loan_plan> GetLoanPlansByCondition(int loanId = 0);

        #region 用户

        /// <summary>
        /// 计算项目罚息
        /// </summary>
        /// <returns></returns>
        decimal CalculationOverRate(CalculationLoanPlanOverRateRequest request);

        /// <summary>
        /// 计算用户借款还款计划罚息
        /// </summary>
        /// <returns></returns>
        decimal CalculationOverRate(int id);

        /// <summary>
        /// 用户借款还款计划统计
        /// </summary>
        /// <param name="id">用户编号</param>
        /// <param name="statisticsItem">统计项目</param>
        /// <returns></returns>
        LoanPlanStatisticsModel UserLoanPlanStatistics(int id, List<LoanPlanStatisticsType> statisticsItem);

        /// <summary>
        /// 根据用户编号获得还款计划
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns></returns>
        PRO_loan_plan GetLatelyLoanPlansByUserId(int userId);

        /// <summary>
        /// 已还清的还款计划
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PageDataView<RMyLoanPlanModel> UserLoanPlanClear(int id, int page = 1, int pageSize = 5);

        /// <summary>
        /// 未还清的还款计划
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PageDataView<RMyLoanPlanModel> UserLoanPlanWaitClear(int id, int page = 1, int pageSize = 5);

        /// <summary>
        /// 借款还款计划
        /// </summary>
        /// <param name="id">项目编号</param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="isClear"></param>
        /// <returns></returns>
        PageDataView<RMyLoanPlanModel> LoanPayPlan(int id, int page = 1, int pageSize = 5, int isClear = 0);

        #endregion 用户

        /// <summary>
        /// 生成还款计划表
        /// </summary>
        /// <param name="loanId">贷款id</param>
        /// <param name="period">还款期数</param>
        /// <param name="tips">提示信息</param>
        /// <param name="isAdd">是否保存到数据库</param>
        /// <returns></returns>
        List<PRO_loan_plan> CreateRepaymentSchedule(PRO_loan_info loanInfo, int period, ref string tips, bool isAdd = false, decimal serviceFee = 0, IDbTransaction transaction=null);

        /// <summary>
        /// 判断是否是最后一期还款计划
        /// </summary>
        /// <param name="loanId"></param>
        /// <returns></returns>
        bool IsLastLoanPlan(int loanId, int loanPlanId);

        /// <summary>
        /// 判断用户是否是第一次还款
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        bool IsFirstLoanPlan(int userId);

        /// <summary>
        /// 企业户最近七天代还款的金额
        /// </summary>
        /// <param name="id">企业户id</param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PageDataView<RMyLoanPlanModel> LastDateWaitClear(int id, int page = 1, int pageSize = 5);
        /// <summary>
        /// 还款中的标的信息
        /// </summary>
        /// <param name="id">借款企业户的id</param>
        /// <returns></returns>
        RMyRepayLoans MyRepayLoans(int id);
        /// <summary>
        /// 担保的还款
        /// </summary>
        /// <param name="loanNo">标的编号</param>
        /// <param name="companyName">企业名称</param>
        /// <param name="gurId">担保人id</param>
        /// <returns></returns>
        RMyRepayLoan GurRepayLoans(string loanNo,string companyName,int gurId);
        /// <summary>
        /// 部分已结清的标的信息
        /// </summary>
        /// <param name="id">借款企业户的id</param>
        /// <returns></returns>
        RMyRepayLoans MyRepayedLoans(int id);

        List<PRO_loan_plan> GetLoanPlansByLoaner(int loanerId);
        /// <summary>
        /// 代还的已完成
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PageDataView<RGurClearedPlan> GurClearedPlans(int id, int page, int pageSize = 5);
    }

    public class LoanPlanService : Repository<PRO_loan_plan>, ILoanPlanService
    {
        private readonly ISettingService _settingService;
        private readonly IInterestProvider _interestProvider;
        private readonly ILoanInfoService _iloanInfoService;
        private readonly IInvestInfoService _iinvestInfoService;

        public LoanPlanService(ISettingService settingService,
            IInterestProvider interestProvider,
            ILoanInfoService iloanInfoService,
            IInvestInfoService iinvestInfoService)
        {
            _settingService = settingService;
            _interestProvider = interestProvider;
            _iloanInfoService = iloanInfoService;
            _iinvestInfoService = iinvestInfoService;
        }

        public PRO_loan_plan GetLastLoanPlan(int loanId)
        {
            var sqlStence = "select * from PRO_loan_plan where pro_loan_id=" + loanId;
            return _Conn.QueryFirstOrDefault<PRO_loan_plan>(sqlStence);
        }

        public List<PRO_loan_plan> GetLoanPlansByCondition(int loanId = 0)
        {
            #region 查询条件

            var sqlStence = "1=1";
            if (loanId != 0)
            {
                sqlStence += $" and pro_loan_id='{loanId}'";
            }

            #endregion 查询条件

            var builder = new StringBuilder();
            builder.Append("select * from PRO_loan_plan");
            builder.Append(" where " + sqlStence);
            var result = _Conn.Query<PRO_loan_plan>(builder.ToString()).ToList();
            return result;
        }

        #region 用户

        public LoanPlanStatisticsModel UserLoanPlanStatistics(int id, List<LoanPlanStatisticsType> statisticsItem)
        {
            if (statisticsItem.Count == 0)
                return new LoanPlanStatisticsModel();

            #region 拼装统计字段

            var listsqlOutParameters = new List<string>();

            if (statisticsItem.Contains(LoanPlanStatisticsType.WaitRepayPrincipal))
                listsqlOutParameters.Add("sum(case when a.pro_is_clear = 0 then a.pro_pay_money end) as WaitRepayPrincipal");

            if (statisticsItem.Contains(LoanPlanStatisticsType.TodayWaitRepay))
                listsqlOutParameters.Add("sum(case when a.pro_is_clear = 0 and DateDiff(dd,a.pro_pay_date,getdate())>=0 then a.pro_pay_money + a.pro_pay_rate end) as TodayWaitRepay");

            if (statisticsItem.Contains(LoanPlanStatisticsType.WaitRepayRate))
                listsqlOutParameters.Add("sum(case when a.pro_is_clear = 0  then a.pro_pay_rate end) as WaitRepayRate");

            if (statisticsItem.Contains(LoanPlanStatisticsType.WaitRepayServiceFee))
                listsqlOutParameters.Add("sum(case when a.pro_is_clear = 0 then a.pro_pay_service_fee end) as WaitRepayServiceFee");

            if (statisticsItem.Contains(LoanPlanStatisticsType.WaitRepayTotal))
                listsqlOutParameters.Add("sum(case when a.pro_is_clear = 0 then a.pro_pay_service_fee+a.pro_pay_rate+a.pro_pay_money end) as WaitRepayTotal");

            if (statisticsItem.Contains(LoanPlanStatisticsType.RepayPrincipal))
                listsqlOutParameters.Add("sum(case when a.pro_is_clear = 1  then a.pro_collect_money end) as RepayPrincipal");

            if (statisticsItem.Contains(LoanPlanStatisticsType.RepayRate))
                listsqlOutParameters.Add("sum(case when a.pro_is_clear = 1  then a.pro_collect_rate end) as RepayRate");

            if (statisticsItem.Contains(LoanPlanStatisticsType.RepayServiceFee))
                listsqlOutParameters.Add("sum(case when a.pro_is_clear = 1 then a.pro_collect_service_fee end) as RepayServiceFee");

            #endregion 拼装统计字段

            #region 拼装sql

            var sqlTableParameters = new StringBuilder(" from");
            sqlTableParameters.Append(" PRO_loan_plan a");
            sqlTableParameters.Append(" left join PRO_loan_info b on a.pro_loan_id = b.Id");

            var sqlWhereParameters = new StringBuilder(" where");
            sqlWhereParameters.Append($" b.pro_add_emp = {id}");
            sqlWhereParameters.Append(" and b.pro_delsign = 0");

            var sqlString = new StringBuilder("select ");
            sqlString.Append(string.Join(",", listsqlOutParameters));
            sqlString.Append(sqlTableParameters);
            sqlString.Append(sqlWhereParameters);

            #endregion 拼装sql

            return _Conn.QueryFirstOrDefault<LoanPlanStatisticsModel>(sqlString.ToString());
        }

        public decimal CalculationOverRate(int id)
        {
            string sqlString = $@"select
a.pro_sett_over_rate as SurplusOverRate,
a.pro_pay_date as PayDate,
a.pro_pay_money as PayMoney,
a.pro_pay_rate as PayRate,
a.pro_collect_date as CollectDate
from PRO_loan_plan a
left join PRO_loan_info b on a.pro_loan_id = b.Id
where b.pro_add_emp = {id} and a.pro_is_clear = 0 and b.pro_delsign = 0 and DateDiff(dd,a.pro_pay_date,getdate())>0";

            var query = _Conn.Query<CalculationLoanPlanOverRateRequest>(sqlString);

            if (query == null)
                return 0.00m;

            return Math.Round(query.Sum(p =>
            {
                return CalculationOverRate(new CalculationLoanPlanOverRateRequest
                {
                    SurplusOverRate = p.SurplusOverRate,
                    CollectDate = p.CollectDate,
                    PayDate = p.PayDate,
                    PayMoney = p.PayMoney,
                    PayRate = p.PayRate
                });
            }), 2);
        }

        public decimal CalculationOverRate(CalculationLoanPlanOverRateRequest request)
        {
            bool isJX = true;
            var _projectSettings = _settingService.LoadSetting<ProjectSettings>();

            #region 相关变量

            int intDaysOverdue = 0;             //逾期天数 = 当前日期 -  应还日期（如果借款人没有还过款））
            decimal decOverRate = 0.00m;        //罚息利率
            decimal decOveIntTotal = 0.00m;     //罚息= 剩余本金 * 罚息利率 * 逾期天数 + 剩余罚金

            #endregion 相关变量

            #region 计算前判断、变量赋值

            #region 系统参数表

            /*罚息利率=罚息月利率/100/30*/
            decOverRate = _projectSettings.sys_over_rate / 100 / 30;

            #endregion 系统参数表

            #region 应还日期

            if (request.PayDate == null)
            {
                isJX = false;
            }
            else
            {
                //逾期天数 = 当前日期 - 应还日期
                //calabash 计算逾期天数应该考虑到 如果还款人的提前还款日 是小于逾期日的那么计算逾期日应该已逾期日为准
                intDaysOverdue = request.SurplusOverRate == 0.00m ? (DateTime.Now - (DateTime)request.PayDate).Days : (DateTime.Now - (DateTime)(request.CollectDate ?? request.PayDate)).Days;
                if (intDaysOverdue < 0)
                {
                    intDaysOverdue = 0;
                }
            }

            #endregion 应还日期

            #endregion 计算前判断、变量赋值

            #region 罚息计算

            if (isJX)
            {
                //罚息 = 还款金额 * 罚息利率 * 逾期天数 + 剩余罚金

                decOveIntTotal = (request.PayMoney + request.PayRate) * decOverRate * intDaysOverdue + request.SurplusOverRate;
            }

            #endregion 罚息计算

            return Math.Round(decOveIntTotal, 2);
        }

        public PageDataView<RMyLoanPlanModel> UserLoanPlanClear(int id, int page = 1, int pageSize = 5)
        {
            var criteria = new PageCriteria();
            criteria.Condition += $" b.pro_add_emp = {id} ";
            criteria.Condition += " and b.pro_delsign = 0 ";
            criteria.Condition += $" and a.pro_is_clear =1 ";
            var innerTable = " PRO_loan_plan a left join  PRO_loan_info b on a.pro_loan_id = b.Id";
            var selectSql = new StringBuilder();
            selectSql.Append(" a.Id,");
            selectSql.Append(" a.pro_pay_date as PayDate,");
            selectSql.Append(" a.pro_pay_money as PayMoney,");
            selectSql.Append(" a.pro_pay_rate as PayRate,");
            selectSql.Append(" (a.pro_pay_rate+a.pro_pay_money) as PayPrincipal,");
            selectSql.Append(" a.pro_loan_period as Period,");
            selectSql.Append(" b.pro_loan_rate as Interest,");
            selectSql.Append(" b.pro_loan_use as LoanName,");
            selectSql.Append(" a.pro_collect_date as CollectDate,");
            selectSql.Append(" a.pro_pay_service_fee as PayServiceFee,");
            selectSql.Append(" a.pro_pay_type as CollectType,");
            selectSql.Append(" a.pro_is_clear as IsClear,");
            selectSql.Append(" a.pro_pay_type as PayState");
            criteria.CurrentPage = page;
            criteria.PageSize = pageSize;
            criteria.Fields = selectSql.ToString();
            criteria.PrimaryKey = "a.Id";
            criteria.TableName = innerTable;
            criteria.Sort = "a.pro_collect_date desc";
            var pageData = GetPageData<RMyLoanPlanModel>(criteria);
            return pageData;
        }

        public PageDataView<RMyLoanPlanModel> UserLoanPlanWaitClear(int id, int page = 1, int pageSize = 5)
        {
            var criteria = new PageCriteria();
            criteria.Condition += $" b.pro_add_emp = {id} ";
            criteria.Condition += " and b.pro_delsign = 0 ";
            criteria.Condition += $" and a.pro_is_clear =0 ";
            var innerTable = " PRO_loan_plan a left join  PRO_loan_info b on a.pro_loan_id = b.Id";
            var selectSql = new StringBuilder();
            selectSql.Append(" a.Id,");
            selectSql.Append(" a.pro_pay_date as PayDate,");
            selectSql.Append(" a.pro_pay_money as PayMoney,");
            selectSql.Append(" a.pro_pay_rate as PayRate,");
            selectSql.Append(" (a.pro_pay_rate+a.pro_pay_money) as PayPrincipal,");
            selectSql.Append(" a.pro_loan_period as Period,");
            selectSql.Append(" b.pro_loan_rate as Interest,");
            selectSql.Append(" b.pro_loan_use as LoanName,");
            selectSql.Append(" a.pro_pay_service_fee as PayServiceFee,");
            selectSql.Append(" a.pro_pay_type as CollectType,");
            selectSql.Append(" a.pro_is_clear as IsClear,");
            selectSql.Append(" a.pro_is_use as IsUsing,");
            selectSql.Append(" a.pro_pay_type as PayState");
            criteria.CurrentPage = page;
            criteria.PageSize = pageSize;
            criteria.Fields = selectSql.ToString();
            criteria.PrimaryKey = "a.Id";
            criteria.TableName = innerTable;
            criteria.Sort = "a.pro_pay_date asc";
            var pageData = GetPageData<RMyLoanPlanModel>(criteria);
            return pageData;
        }

        public PageDataView<RMyLoanPlanModel> LoanPayPlan(int id, int page = 1, int pageSize = 5,int isClear=0)
        {
            var criteria = new PageCriteria();
            criteria.Condition += $" b.Id = {id} ";
            criteria.Condition += " and b.pro_delsign = 0 ";
            criteria.Condition += $" and a.pro_is_clear ={isClear}";
            var innerTable = " PRO_loan_plan a right join  PRO_loan_info b on a.pro_loan_id = b.Id";
            var selectSql = new StringBuilder();
            selectSql.Append(" a.Id,");
            selectSql.Append(" a.pro_pay_date as PayDate,");
            selectSql.Append(" a.pro_pay_money as PayMoney,");
            selectSql.Append(" a.pro_pay_rate as PayRate,");
            selectSql.Append(" (a.pro_pay_rate+a.pro_pay_money) as PayPrincipal,");
            selectSql.Append(" a.pro_loan_period as Period,");
            selectSql.Append(" b.pro_loan_rate as Interest,");
            selectSql.Append(" b.pro_loan_use as LoanName,");
            selectSql.Append(" a.pro_pay_service_fee as PayServiceFee,");
            selectSql.Append(" a.pro_pay_type as CollectType,");
            selectSql.Append(" a.pro_is_clear as IsClear,");
            selectSql.Append(" a.pro_is_use as IsUsing,");
            selectSql.Append(" a.pro_pay_type as PayState");
            
            if (isClear == 1)
            {
                selectSql.Append(", a.pro_collect_date as CollectDate");
            }
            criteria.CurrentPage = page;
            criteria.PageSize = pageSize;
            criteria.Fields = selectSql.ToString();
            criteria.PrimaryKey = "a.Id";
            criteria.Sort = "a.pro_pay_date asc";
            criteria.TableName = innerTable;
            var pageData = GetPageData<RMyLoanPlanModel>(criteria);
            return pageData;
        }

        public PRO_loan_plan GetLastLoanPlan(string merBillNo)
        {
            var sqlStence = "select * from PRO_loan_plan where tran_seq_no='" + merBillNo+"'";
            return _Conn.QueryFirstOrDefault<PRO_loan_plan>(sqlStence);
        }

        public PRO_loan_plan GetLoanByMerBillNo(string merBillNo)
        {
            var sqlStence = "select * from PRO_loan_plan where tran_seq_no='" + merBillNo+",";
            return _Conn.QueryFirstOrDefault<PRO_loan_plan>(sqlStence);
        }

        public List<PRO_loan_plan> CreateRepaymentSchedule(PRO_loan_info loanInfo, int period, ref string tips, bool isAdd = false, decimal serviceFee = 0,IDbTransaction transaction = null)
        {
            List<PRO_loan_plan> listPlan = new List<PRO_loan_plan>();
            //if (serviceFee == 0)
            //{
            //    //尝试获取配置费率中的服务费
            //    List<SYS_fee_config> listFee = _feeConfigService.GetListByEntity(new SYS_fee_config() { sys_fee_type = DataDictionary.feetype_Managementexpense }).ToList();
            //    _proLoanInfoService.MoneyServiceFee(loanId, ref serviceFee, listFee);
            //}


            //获取管理费
            if (serviceFee >=0)
            {
                CalculateCreditorRequest ent = new CalculateCreditorRequest();
                ent.LoanAmount = Convert.ToDecimal(loanInfo.pro_loan_money);
                ent.LoanRate = Convert.ToDecimal(loanInfo.pro_loan_rate);
                ent.LoanDurTime = Convert.ToInt32(loanInfo.pro_loan_period);
                ent.InterestBearing = Convert.ToInt32(loanInfo.pro_period_type);
                ent.RepaymentType = _interestProvider.GetInterest(loanInfo.pro_pay_type);
                ent.RepaymentPeriods = period;

                if (loanInfo.pro_collect_type == DataDictionary.settlementway_Fixed)
                {
                    if (loanInfo.pro_collect_date == null || loanInfo.pro_collect_date == 0)
                    {
                        //提示信息 
                        tips = "当前项目未输入固定还款日。";
                        return listPlan;
                    }
                    else { ent.BillDay = Convert.ToInt32(loanInfo.pro_collect_date); }

                }

                ent.SettlementWay = Convert.ToInt32(loanInfo.pro_collect_type);
                ent.ManagementFee = Convert.ToDecimal(serviceFee);
                ent.ChargeWay = Convert.ToInt32(DataDictionary.chargeway_Pen);
                GenerateCreditorResult result = _interestProvider.GenerateCreditor(ent);

                if (result.Succeed)
                {
                    listPlan = result.ProLoanPlans;

                    //获取投资记录
                    List<PRO_invest_info> ListInvest = _iinvestInfoService.GetInvestListByLoanId(loanInfo.Id,null);
                    // 非收益复投金额 / 总金额
                    decimal percentage = ListInvest.Where(p => !p.pro_is_repeat).Sum(p => p.pro_invest_money).GetValueOrDefault() / ListInvest.Sum(p => p.pro_invest_money).GetValueOrDefault();

                    for (int i = 0; i < listPlan.Count(); i++)
                    {
                        //是否最后一期
                        if (i + 1 == listPlan.Count())
                        {
                            decimal dec = ListInvest.Where(p => p.pro_is_repeat).Sum(p => p.pro_invest_money).GetValueOrDefault();
                            if (percentage != 1)
                            {
                                listPlan[i].pro_pay_rate = (listPlan[i].pro_pay_rate * percentage) + getloanPlanRate(dec, (decimal)loanInfo.pro_loan_rate, (int)loanInfo.pro_loan_period, i + 1);
                            }
                            else
                            {
                                listPlan[i].pro_pay_rate = listPlan[i].pro_pay_rate;
                            }
                        }
                        else
                        {

                            PRO_loan_plan plan = listPlan[i];
                            listPlan[i].pro_pay_rate = listPlan[i].pro_pay_rate * percentage;
                        }
                        //解决平台服务费bug
                        if ((listPlan[i].pro_pay_money + listPlan[i].pro_pay_rate) > 0)
                        {
                            //重新赋值
                            listPlan[i].pro_pay_total = listPlan[i].pro_pay_money + listPlan[i].pro_pay_rate + listPlan[i].pro_pay_service_fee;
                        }
                        else
                        {
                            listPlan[i].pro_pay_total = listPlan[i].pro_pay_money + listPlan[i].pro_pay_rate;
                        }
                    }

                    listPlan = listPlan.Where(p => p.pro_pay_total > 0).ToList();


                    if (isAdd)
                    {
                        foreach (PRO_loan_plan item in listPlan)
                        {
                            item.pro_loan_id = loanInfo.Id;
                            item.pro_is_clear = item.pro_pay_total > 0 ? false : true;// 若本期无可还金额 则置为已结清
                            Add(item, transaction);
                        }
                    }
                }
                else
                {
                    //提示信息 
                    tips = result.Msg;
                }
            }
            else
            {
                //提示信息 
                tips = "请先配置服务费！";
            }

            return listPlan;
        }

        /// <summary>
        /// 获取 收益复投利息
        /// </summary>
        /// <param name="investMoney">投资金额</param>
        /// <param name="rate">收益率</param>
        /// <param name="loan_period"></param>
        /// <returns></returns>
        public decimal getloanPlanRate(decimal investMoney, decimal rate, int loan_period, int planCount)
        {
            decimal endRate = 0.00m; //最终结算金额
            int loanMon = loan_period / planCount;
            for (int i = 0; i < planCount; i++)
            {
                decimal JsRate = CommonHelper.Round(investMoney * rate * loanMon / 1200, 2);  //收益 =  投资金额 * 收益率 /12
                investMoney += JsRate;                       // 投资金额= 投资金额+本期利息 
                endRate += JsRate;
            }
            return endRate;

        }

        public bool IsLastLoanPlan(int loanId ,int loanPlanId)
        {
            var sqlStr = $"SELECT * FROM PRO_loan_plan where pro_is_clear=0 and pro_loan_id={loanId} and Id!={loanPlanId}";
            return _Conn.Query<PRO_loan_plan>(sqlStr).Count() == 0;
        }

        public bool IsFirstLoanPlan(int userId)
        {
            var sqlStr = $"SELECT P.* FROM PRO_loan_info L JOIN PRO_loan_plan P ON L.Id = P.pro_loan_id WHERE P.pro_is_clear = 1 AND L.pro_add_emp = {userId}";
            return _Conn.Query<PRO_loan_plan>(sqlStr).Count() == 0;
        }



        public PRO_loan_plan GetLatelyLoanPlansByUserId(int userId)
        {
            return _Conn.QueryFirstOrDefault<PRO_loan_plan>($@"SELECT top 1 B.* FROM PRO_loan_info A LEFT JOIN PRO_loan_plan B ON A.Id = B.pro_loan_id 
            WHERE A.pro_add_emp = {userId} AND A.pro_delsign = 0 AND B.pro_is_clear = 0 ORDER BY B.pro_pay_date");
        }

        #endregion 用户

        #region 企业户
        public PageDataView<RMyLoanPlanModel> LastDateWaitClear(int id, int page = 1, int pageSize = 5)
        {
            var criteria = new PageCriteria();
            criteria.Condition += $" b.pro_add_emp = {id} ";
            criteria.Condition += " and b.pro_delsign = 0 ";
            criteria.Condition += $" and a.pro_is_clear =0 ";
            criteria.Condition += "and a.pro_pay_date<DATEADD(D,30,GETDATE()) ";
            var innerTable = " PRO_loan_plan a left join PRO_loan_info b on a.pro_loan_id = b.Id";
            var selectSql = new StringBuilder();
            selectSql.Append(" a.Id,");
            selectSql.Append(" a.pro_pay_date as PayDate,");
            selectSql.Append(" a.pro_pay_money as PayMoney,");
            selectSql.Append(" a.pro_pay_rate as PayRate,");
            selectSql.Append(" (a.pro_pay_rate+a.pro_pay_money) as PayPrincipal,");
            selectSql.Append(" a.pro_loan_period as Period,");
            selectSql.Append(" b.pro_loan_rate as Interest,");
            selectSql.Append(" b.pro_loan_use as LoanName,");
            selectSql.Append(" a.pro_pay_service_fee as PayServiceFee,");
            selectSql.Append(" a.pro_pay_type as CollectType,");
            selectSql.Append(" a.pro_is_clear as IsClear,");
            selectSql.Append(" a.pro_is_use as IsUsing,");
            selectSql.Append(" a.pro_pay_type as PayState");
            criteria.CurrentPage = page;
            criteria.PageSize = pageSize;
            criteria.Fields = selectSql.ToString();
            criteria.PrimaryKey = "a.Id";
            criteria.TableName = innerTable;
            criteria.Sort = "a.pro_pay_date asc";
            var pageData = GetPageData<RMyLoanPlanModel>(criteria);
            return pageData;
        }

        public RMyRepayLoans MyRepayLoans(int id)
        {
            var stringBuilder=new StringBuilder();
            stringBuilder.Append("select a.ID as LoanId,");
            stringBuilder.Append(" a.pro_loan_no as LoanName,");
            stringBuilder.Append(" (select SUM(b.pro_pay_money+b.pro_pay_rate+b.pro_pay_service_fee+isnull(b.pro_pay_over_rate,0)) as WaitRepayMoney  from PRO_loan_plan b where b.pro_loan_id=a.Id and pro_is_clear=0) as WaitRepayMoney,");
            stringBuilder.Append(" (select COUNT(*) as WaitRepayMoney  from PRO_loan_plan b where b.pro_loan_id=a.Id and pro_is_clear=0) as WaitRepayPeriod,");
            stringBuilder.Append(" a.pro_loan_money as LoanMoney,");
            stringBuilder.Append(" a.pro_loan_period as LoanPeriod,");
            stringBuilder.Append(" a.pro_loan_rate as LoanRate,");
            stringBuilder.Append(" a.CreDt as FullDate,");
            stringBuilder.Append(" a.pro_pay_type as RepayType");
            stringBuilder.Append(" from PRO_loan_info a");
            stringBuilder.Append($" where a.pro_loan_state in (24,734) and a.pro_add_emp={id}");
            var loans = _Conn.Query<RMyRepayLoan>(stringBuilder.ToString()).ToList();
            var result = new RMyRepayLoans
            {
                Count = loans.Count,
                RepayLoans = loans
            };
            return result;
        }

        public RMyRepayLoan GurRepayLoans(string loanNo, string companyName, int gurId)
        {
            #region 查询条件

            var sqlStence = "a.pro_loan_state in (24,734) ";
            if (!string.IsNullOrEmpty(loanNo))
            {
                sqlStence += $" and pro_loan_no='{loanNo}'";
            }
            if (!string.IsNullOrEmpty(companyName))
            {
                sqlStence += $" and pro_user_name='{companyName}' ";
            }
            sqlStence += $" and pro_loan_guar_company='{gurId}'";
            #endregion 查询条件
            var builder=new StringBuilder();
            builder.Append("select a.ID as LoanId,");
            builder.Append(" a.pro_loan_no as LoanName,");
            builder.Append(" (select COUNT(*) as WaitRepayMoney  from PRO_loan_plan b where b.pro_loan_id=a.Id and pro_is_clear=0) as WaitRepayPeriod,");
            builder.Append(" a.pro_loan_money as LoanMoney,");
            builder.Append(" a.pro_loan_period as LoanPeriod,");
            builder.Append(" a.pro_loan_rate as LoanRate,");
            builder.Append(" a.pro_full_date as FullDate,");
            builder.Append(" a.pro_pay_type as RepayType");
            builder.Append(" from PRO_loan_info  a");
            builder.Append(" where "+sqlStence);
            var result = _Conn.QueryFirstOrDefault<RMyRepayLoan>(builder.ToString());
            return result;
        }

        public RMyRepayLoans MyRepayedLoans(int id)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("select a.ID as LoanId,");
            stringBuilder.Append(" a.pro_loan_no as LoanName,");
            stringBuilder.Append(" (select SUM(b.pro_pay_money+b.pro_pay_rate+b.pro_pay_service_fee+isnull(b.pro_pay_over_rate,0)) as WaitRepayMoney  from PRO_loan_plan b where b.pro_loan_id=a.Id and pro_is_clear=1) as WaitRepayMoney,");
            stringBuilder.Append(" (select COUNT(*) as WaitRepayMoney  from PRO_loan_plan b where b.pro_loan_id=552 and pro_is_clear=1) as WaitRepayPeriod,");
            stringBuilder.Append(" a.pro_loan_money as LoanMoney,");
            stringBuilder.Append(" a.pro_loan_period as LoanPeriod,");
            stringBuilder.Append(" a.pro_loan_rate as LoanRate,");
            stringBuilder.Append(" a.pro_full_date as FullDate,");
            stringBuilder.Append(" a.pro_pay_type as RepayType");
            stringBuilder.Append(" from PRO_loan_info a");
            stringBuilder.Append($" where a.pro_add_emp={id}");
            stringBuilder.Append(" and (select COUNT(*) from PRO_loan_plan b where b.pro_loan_id=a.Id and b.pro_is_clear=1)>0");
            var loans = _Conn.Query<RMyRepayLoan>(stringBuilder.ToString()).ToList();
            var result = new RMyRepayLoans
            {
                Count = loans.Count,
                RepayLoans = loans
            };
            return result;
        }

        public List<PRO_loan_plan> GetLoanPlansByLoaner(int loanerId)
        {
            var sBuilder=new StringBuilder();
            sBuilder.Append("select * from PRO_loan_plan where pro_loan_id");
            sBuilder.Append(" in (select id from PRO_loan_info where");
            sBuilder.Append($" pro_add_emp='{loanerId}' and pro_loan_state in(24,25,734) and pro_delsign=0)");
            var result = _Conn.Query<PRO_loan_plan>(sBuilder.ToString()).ToList();
            return result;
        }

        /// <summary>
        /// 担保人代偿的标的
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public PageDataView<RGurClearedPlan> GurClearedPlans(int id, int page, int pageSize = 5)
        {
            var criteria = new PageCriteria();
            criteria.Condition += $" a.pro_is_clear=1 and";
            criteria.Condition += " a.pro_pay_type=154 and";
            criteria.Condition += $" b.pro_loan_guar_company={id}";
            var innerTable = " PRO_loan_plan a inner join PRO_loan_info b on a.pro_loan_id=b.Id";
            var selectSql = new StringBuilder();
            selectSql.Append(" b.Id as LoanId,");
            selectSql.Append(" b.pro_loan_no as LoanNo,");
            selectSql.Append(" b.pro_loan_use as LoanName,");
            selectSql.Append(" a.pro_loan_period as PlanPeriod,");
            selectSql.Append(" a.pro_pay_date as PayDate,");
            selectSql.Append(" a.pro_pay_money as PayMoney,");
            selectSql.Append(" a.pro_pay_rate as PayRate,");
            selectSql.Append(" a.pro_pay_total as PayPrincipal,");
            selectSql.Append(" a.pro_pay_service_fee as PayServiceFee");
            criteria.CurrentPage = page;
            criteria.PageSize = pageSize;
            criteria.Fields = selectSql.ToString();
            criteria.PrimaryKey = "a.Id";
            criteria.TableName = innerTable;
            criteria.Sort = "a.pro_pay_date desc";
            var pageData = GetPageData<RGurClearedPlan>(criteria);
            return pageData;
        }

        #endregion
    }
}