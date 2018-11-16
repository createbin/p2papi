using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZFCTAPI.Core;
using ZFCTAPI.Core.Helpers;
using ZFCTAPI.Core.Provider;
using ZFCTAPI.Data.Interest;
using ZFCTAPI.Data.PRO;
using ZFCTAPI.Services.Transaction;

namespace ZFCTAPI.Services.Repayment
{
    public interface IInterestProvider
    {
        /// <summary>
        /// 获取计息方式对应的值
        /// </summary>
        /// <param name="repayment"></param>
        /// <returns></returns>
        string GetInterest(string repayment);

        /// <summary>
        /// 获取所有计息方式
        /// </summary>
        /// <returns></returns>
        List<InterestType> LoadAllInterestProviders();


        /// <summary>
        /// 根据systemName查询FriendlyName
        /// </summary>
        /// <param name="systemName">systemName</param>
        /// <returns>FriendlyName</returns>
        string LoadInterestProviderGetByFriendlyName(string systemName);

        /// <summary>
        /// 生成债权关系计划表
        /// </summary>
        /// <param name="calculateCreditorRequest">需要的参数</param>
        /// <returns></returns>
        GenerateCreditorResult GenerateCreditor(CalculateCreditorRequest calculateCreditorRequest);

        /// <summary>
        /// 更新指定交易流水 对应的待收金额
        /// </summary>
        /// <param name="decLoanMoney">借款金额</param>
        /// <param name="investList">投资列表</param>
        /// <param name="loanPlanList">还款计划列表</param>
        /// <param name="dic_transaction">交易人列表</param>
        /// <param name="dic_ReceiveMoney">待收金额列表</param>
        void UpdateReceiveMoney(decimal decLoanMoney, List<PRO_invest_info> investList, List<PRO_loan_plan> loanPlanList
            , Dictionary<int, string> dic_transaction, Dictionary<int, decimal> dic_ReceiveMoney);

        /// <summary>
        /// 更新借款的交易记录(账户总额、账户余额)
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="userType">用户类型</param>
        /// <param name="pro_loan_id">借款项目Id</param>
        /// <param name="transactionType">划转类型</param>
        /// <param name="transactionNo">流水号</param>
        /// <param name="redMoney">红包金额</param>
        void UpdateTransactionInfo(int userId, int? userType, int pro_loan_id, int transactionType, string transactionNo, decimal redMoney);
    }

    public class InterestProvider : IInterestProvider
    {
        public int gRetainedDigit = 2;      //保留位数
        private int gOverDays = 32;          //超过多少天算一个月
        private string gTip = "生成成功！";  //默认操作提示
        private string gIncomeMethods = "2"; //付息模式：1.预收；2.后收
        private readonly ICstTransactionService _cstTransactionService;

        public InterestProvider(ICstTransactionService cstTransactionService)
        {
            _cstTransactionService = cstTransactionService;
        }

        #region methods

        public GenerateCreditorResult GenerateCreditor(CalculateCreditorRequest calculateCreditorRequest)
        {
            var flag = true;
            var strTip = "生成成功！";
            var repayment = calculateCreditorRequest.RepaymentType; //还款方式(1：等额本息 2：按月还息到期还本 3：等额本金)
            var strSWFS = "1";//算尾方式(0.算头不算尾；1.算头算尾)
            var period = ""; //期限类型(计算周期)(1：年 2：月 3：日)
            if (calculateCreditorRequest.InterestBearing == DataDictionary.deadlinetype_Day)
                period = "3";
            else if (calculateCreditorRequest.InterestBearing == DataDictionary.deadlinetype_Month)
                period = "2";
            var borrowBalance = calculateCreditorRequest.LoanAmount;                 //借款金额
            var interestRate = calculateCreditorRequest.LoanRate;                    //利率（年利率）
            var loanPeriod = calculateCreditorRequest.LoanDurTime;                 //借款期限
            var startDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));   //借款开始日（放款日期）
            var endDate = GetEndDateByStartDate(period, startDate, loanPeriod); //借款截止日（等于 放款日期+借款期限）
            var repaymentTimes = repayment == "3" ? 1 : calculateCreditorRequest.RepaymentPeriods;//还款期数（1.借款期限不满一个月只能生产1期。2.多余1个月比如5个月，月数除以期数必须能整除。）
            var settleType = "";//结算方式(1：固定还款日； 2：非固定还款日)(1：固定还款日（1.只要是放款日期小于等于15号，下月1号还款。2.只要是放款日期大于15号，下月16号还款。）；2：非固定还款日（放款日期。）)
            var fixedRepayment = ""; //固定还款日期
            if (calculateCreditorRequest.SettlementWay == DataDictionary.settlementway_Fixed)
            {
                if (calculateCreditorRequest.BillDay >= 10)
                {
                    fixedRepayment = "-" + calculateCreditorRequest.BillDay;
                }
                else
                {
                    fixedRepayment = "-0" + calculateCreditorRequest.BillDay;
                }

                settleType = "1";
            }
            else if (calculateCreditorRequest.SettlementWay == DataDictionary.settlementway_NotFixed)
            {
                settleType = "2";
            }
            var firstRepayment = DateTime.Now;//首次还款日
            var intGlfXmlx = calculateCreditorRequest.ProjectTypes;//项目类型(1.个人信用贷；2.汽车抵押贷；3.房产抵押贷；4.企业经营贷)
            var monthRepayment = 0;//借款月数

            var loanPlanList = new List<PRO_loan_plan>();//计划表

            #region 管理费（平台服务费）相关变量

            var decGlf = 0m;     //管理费
            var strGlfSffs = "";    //收费方式(1.比例；2.比)
            if (calculateCreditorRequest.ChargeWay == DataDictionary.chargeway_Pen)
            {
                strGlfSffs = "2";
            }
            else if (calculateCreditorRequest.ChargeWay == DataDictionary.chargeway_Proportion)
            {
                strGlfSffs = "1";
            }
            var decGlfSfz = calculateCreditorRequest.ManagementFee;  //收费值(%/元) 

            #endregion

            #region 首次还款日

            if (settleType == "1")//1：固定还款日
            {
                firstRepayment = Convert.ToDateTime(startDate.AddMonths(1).ToString("yyyy-MM") + fixedRepayment);
            }
            else if (settleType == "2")//2：非固定还款日
            {
                firstRepayment = startDate.AddMonths(1);
            }
            else
            {
                strTip = "结算方式不正确！";
                flag = false;
            }

            #endregion

            #region 费用设置表

            if (flag)
            {
                if (strGlfSffs == "1")//1：比例
                {
                    decGlf = borrowBalance * decGlfSfz / 100;
                }
                else if (strGlfSffs == "2")//2：笔
                {
                    decGlf = decGlfSfz;
                }
                else
                {
                    strTip = "收费方式不正确！";
                    flag = false;
                }
            }

            #endregion

            #region 生成计划前条件判断
            if (flag)
            {
                monthRepayment = (endDate.Year * 12 + endDate.Month) - (startDate.Year * 12 + startDate.Month);
                if (monthRepayment == 0)
                {
                    monthRepayment = 1;
                }
                if (monthRepayment == 1 && repaymentTimes != 1)
                {
                    strTip = "请输入能被借款月数(" + monthRepayment + ")整除的[还款期数]！";
                    flag = false;
                }
                else
                {
                    if (monthRepayment % repaymentTimes != 0)//借款月数 对 还款期数 求余数 余数大于0
                    {
                        strTip = "请输入能被借款月数(" + monthRepayment + ")整除的[还款期数]！";
                        flag = false;
                    }
                }
            }
            #endregion

            #region 根据还款方式生成
            if (flag)
                switch (repayment)
                {
                    case "1": //等额本息
                        loanPlanList = GetPlanSameCorpusInterest(out strTip,
                             repayment, strSWFS, period, borrowBalance, interestRate, startDate, endDate,
                             repaymentTimes, settleType, firstRepayment, intGlfXmlx, monthRepayment, decGlf);
                        break;
                    case "2": //按月还息，到期还本
                        loanPlanList = GetPlanInterestPerMonthLastCapital(out strTip,
                            repayment, strSWFS, period, borrowBalance, interestRate, startDate, endDate,
                          repaymentTimes, settleType, firstRepayment, intGlfXmlx, monthRepayment, decGlf);
                        break;

                    case "3": //等额本金
                        loanPlanList = GetPlanSameCorpusInterest(out strTip,
                            repayment, strSWFS, period, borrowBalance, interestRate, startDate, endDate,
                          repaymentTimes, settleType, firstRepayment, intGlfXmlx, monthRepayment, decGlf);
                        break;
                    default:
                        strTip = "还款方式不正确！";
                        flag = false;
                        break;
                }
            #endregion

            var result = new GenerateCreditorResult();
            if (flag)
            {
                result.ProLoanPlans = loanPlanList;
                result.Succeed = true;
            }
            else
            {
                result.Msg = strTip;
                result.Succeed = false;
            }
            result.Msg = strTip;
            return result;
        }

        public string GetInterest(string repayment)
        {
            switch (repayment)
            {
                case "Interests.AverageCapitalPlusInterest":
                    return "1";
                case "Interests.MonthlyInterestDuePrincipal":
                    return "2";
                case "Interests.WithBenefitClear":
                    return "3";
                default:
                    return "1";
            }
        }

        public List<InterestType> LoadAllInterestProviders()
        {
            var result = new List<InterestType>
            {
                new InterestType{Code = "Interests.AverageCapitalPlusInterest",InterestName = "每月等额返还本息"},
                new InterestType{Code = "Interests.MonthlyInterestDuePrincipal",InterestName = "每月返息到期还本"},
                new InterestType{Code = "Interests.WithBenefitClear",InterestName = "一次性到期返还本息"},
            };
            return result;
        }

        public string LoadInterestProviderGetByFriendlyName(string systemName)
        {
            var result = LoadAllInterestProviders();
            var result2 = result.FirstOrDefault(p => p.Code == systemName);
            return result2 == null ? "" : result2.InterestName;
        }

        public void UpdateReceiveMoney(decimal decLoanMoney, List<PRO_invest_info> investList, List<PRO_loan_plan> loanPlanList, Dictionary<int, string> dic_transaction, Dictionary<int, decimal> dic_ReceiveMoney)
        {
            if (true)//NopVersion.CurrentVersion == "3.0.0.1"
            {
                #region 相关变量

                //应收本金 应收利息 应收总额

                decimal decInvestMoney = 0.00m;//投资金额
                decimal decInvestRatio = 0.00m;//投资占比

                decimal decMoney = 0.00m;//应收本金
                decimal decRate = 0.00m;//应收利息

                decimal temp = 0.00m;

                Dictionary<int, decimal> dic_CloneReceive = new Dictionary<int, decimal>();
                if (dic_ReceiveMoney != null && dic_ReceiveMoney.Count > 0)
                {
                    foreach (var key in dic_ReceiveMoney.Keys)
                    {
                        dic_CloneReceive.Add(key, 0.00m);
                    }
                }

                #endregion

                #region 循环还款计划表给投资人收回计划表赋值

                foreach (var item in loanPlanList)
                {
                    decimal decMoneyTotal = 0.00m;//应收回本金合计（除去最后一个投资人）
                    decimal decTotInvRate = 0.00m;//当期收回总利息（除去最后一个投资人）
                    //循环投资人
                    for (int i = 0; i < investList.Count; i++)
                    {
                        decInvestMoney = (decimal)investList[i].pro_credit_money;//投资金额

                        /* 投资占比 = 投资金额 / 借款金额 */
                        decInvestRatio = decInvestMoney / decLoanMoney;

                        if (i == investList.Count - 1)
                        {
                            /* 应收本金 = 应还本金 - 应收本金合计（最后一个投资人前） */
                            decMoney = (decimal)item.pro_pay_money - decMoneyTotal;
                            decMoney = CommonHelper.Round((Decimal)(decMoney), 2);  //应收本金（四舍五入）

                            /* 应收利息 = 投资占比 * 应还利息 */
                            decRate = (decimal)item.pro_pay_rate - decTotInvRate;
                            decRate = Decimal.Parse(CommonHelper.getInterest(decRate, 0));//应收利息
                        }
                        else
                        {
                            /* 应收本金 = 投资占比 * 应还本金 */
                            decMoney = decInvestRatio * (decimal)item.pro_pay_money;
                            decMoney = CommonHelper.Round((Decimal)(decMoney), 2);  //应收本金（四舍五入）
                            decMoneyTotal += decMoney;

                            /* 应收利息 = 投资占比 * 应还利息 */
                            decRate = decInvestRatio * (decimal)item.pro_pay_rate;
                            decRate = Decimal.Parse(CommonHelper.getInterest(decRate, 0));//应收利息
                            decTotInvRate += decRate;
                        }

                        foreach (int key in dic_CloneReceive.Keys)
                        {
                            temp = 0.00m;

                            if (key == (int)investList[i].pro_invest_emp)
                            {
                                temp = dic_ReceiveMoney[key];
                                dic_ReceiveMoney[key] = temp + decMoney + decRate;
                                break;
                            }
                        }

                    }
                }
                #endregion

                #region 更新交易记录 待收金额

                if (dic_transaction != null && dic_transaction.Count > 0 && dic_ReceiveMoney != null && dic_ReceiveMoney.Count > 0)
                {
                    foreach (var transaction in dic_transaction.Keys)
                    {
                        foreach (var receive in dic_ReceiveMoney.Keys)
                        {
                            if (transaction == receive)
                            {
                                var transactionInfo = _cstTransactionService.GetSuccessLitByOrderNo(receive, dic_transaction[transaction]);
                                if (transactionInfo != null)
                                {
                                    transactionInfo.pro_receivable_money = transactionInfo.pro_receivable_money + dic_ReceiveMoney[receive];
                                    _cstTransactionService.Update(transactionInfo);
                                }

                                break;
                            }
                        }
                    }
                }

                #endregion

            }
        }

        public void UpdateTransactionInfo(int userId, int? userType, int pro_loan_id, int transactionType, string transactionNo, decimal redMoney)
        {
            if (true)//NopVersion.CurrentVersion == "3.0.0.1" && redMoney > 0
            {
                var transactionInfo = _cstTransactionService.GetListByOrderNo(transactionNo).Where(t => t.pro_user_id == userId && t.pro_user_type == userType
                    && t.pro_loan_id == pro_loan_id && t.pro_transaction_type == transactionType && t.pro_transaction_status == DataDictionary.transactionstatus_success).FirstOrDefault();

                if (transactionInfo != null)
                {
                    transactionInfo.pro_account_money = Convert.ToDecimal(transactionInfo.pro_account_money) + redMoney;
                    transactionInfo.pro_balance_money = Convert.ToDecimal(transactionInfo.pro_balance_money) + redMoney;
                    _cstTransactionService.Update(transactionInfo);
                }
            }
        }

        #endregion

        #region privite methods

        #region 按月利息到期还本
        /// <summary>
        /// 按月利息到期还本
        /// 创建者：吕小东
        /// 创建时间：2014年9月25日13:06:30
        /// </summary>
        /// <param name="strTip">提示信息(当提示信息不为“生成成功！”时，都应弹出此提示信息。)</param>
        /// <param name="strHKFS">还款方式(1：等额本息 2：按月还息到期还本 3：等额本金)</param>
        /// <param name="strSWFS">算尾方式(0.算头不算尾；1.算头算尾) </param>
        /// <param name="strQXLX">期限类型(计算周期)(1：年 2：月 3：日)</param>
        /// <param name="decJKJE">借款金额</param>
        /// <param name="decLL">利率（年利率）</param>
        /// <param name="datJKKSR">借款开始日（放款日期）</param>
        /// <param name="datJKJZR">借款截止日（等于 放款日期+借款期限）</param>
        /// <param name="intHKQS">还款期数（1.借款期限不满一个月只能生产1期。2.多余1个月比如5个月，月数除以期数必须能整除。）</param>
        /// <param name="strJSFS">结算方式(1：固定还款日； 2：非固定还款日)(1：固定还款日（1.只要是放款日期小于等于15号，下月1号还款。2.只要是放款日期大于15号，下月16号还款。）；2：非固定还款日（放款日期。）)</param>
        /// <param name="datSCHKR">首次还款日</param>
        /// <param name="intGlfXmlx">项目类型(1.个人信用贷；2.汽车抵押贷；3.房产抵押贷；4.企业经营贷)</param>
        /// <param name="intJKYS">借款月数</param>
        /// <param name="decGlf">管理费</param>
        /// <returns>还款计划表集合对象</returns>
        private List<PRO_loan_plan> GetPlanInterestPerMonthLastCapital(out string strTip, string strHKFS,
            string strSWFS, string strQXLX, decimal decJKJE, decimal decLL,
            DateTime datJKKSR, DateTime datJKJZR, int intHKQS, string strJSFS,
            DateTime datSCHKR, int intGlfXmlx, int intJKYS, decimal decGlf)
        {
            string strTipB = gTip;

            DateTime dtLoanStartDate = (DateTime)(datJKKSR);    //放款日
            DateTime dtLoanEndDate = (DateTime)(datJKJZR);      //到期日
            Decimal decSettMoney = (Decimal)(decJKJE);          //结欠金额
            string strRegionTime = gIncomeMethods;              //付息模式：1.预收；2.后收
            DateTime dtStartDate = dtLoanStartDate;
            DateTime dtEndDate = datSCHKR;
            Decimal? Sumpro_pay_rate = 0.00m;
            if (intHKQS == 1)
            {
                strTip = strTipB;
                return GetOneTimesPlan(out strTipB,
                       strHKFS, strSWFS, strQXLX, decJKJE, decLL, datJKKSR, datJKJZR,
                          intHKQS, strJSFS, datSCHKR, intGlfXmlx, intJKYS, decGlf);
            }

            //两次还款之间的月数
            int nMonths = CommonHelper.RoundInt(GetBetweenMonths(datSCHKR, dtLoanEndDate) / (intHKQS - 1));
            List<PRO_loan_plan> planList = new List<PRO_loan_plan>();

            //预收
            if (strRegionTime == "1")
            {
                intHKQS = intHKQS + 1;
            }

            for (int ii = 1; ii < intHKQS + 1; ii++)
            {
                //是否最后一期收款
                bool bLastPirod = false;
                PRO_loan_plan objPlan = new PRO_loan_plan();

                objPlan.pro_loan_period = ii;                   //期数
                objPlan.pro_is_clear = false;                     //是否还清：1：是 0：否
                objPlan.pro_pay_type = DataDictionary.repaymenstate_Normal;          //还款状态(1：正常还款 2：平台代还 3：强制还款)

                #region  第一期
                if (ii == 1)
                {
                    dtStartDate = dtLoanStartDate;                                                  //起息日
                    dtEndDate = datSCHKR;                                                        //止息日

                    //应还利息
                    objPlan.pro_pay_rate = getRepayInterest(
                        out strTipB, decJKJE, dtStartDate, dtEndDate, strSWFS,
                        strQXLX, decLL, bLastPirod, gOverDays);

                    objPlan.pro_pay_money = 0;  //应还本金    
                }
                #endregion  第一期

                #region  最后一期
                else if (ii == intHKQS)
                {
                    bLastPirod = true;
                    dtStartDate = dtLoanStartDate;
                    dtEndDate = dtLoanEndDate;                                      //止息日
                    objPlan.pro_pay_money = decSettMoney;   //计划归还本金


                    if (strRegionTime == "1")       //预收
                    {
                        objPlan.pro_pay_rate = 0m;  //计划归还利息
                        dtStartDate = dtEndDate;    //止息日
                    }
                    else //后收
                    {
                        objPlan.pro_pay_rate = getRepayInterest(
                            out strTipB, decJKJE, dtStartDate, dtEndDate, strSWFS,
                            strQXLX, decLL, bLastPirod, gOverDays) - Sumpro_pay_rate;
                    }

                }
                #endregion

                #region  中间期
                else
                {
                    objPlan.pro_pay_money = 0;  //应还本金

                    if (strRegionTime == "1" && ii == intHKQS - 1)//预收
                    {
                        bLastPirod = true;
                        dtEndDate = dtLoanEndDate;
                        objPlan.pro_pay_rate = getRepayInterest(
                            out strTipB, decJKJE, dtStartDate, dtEndDate, strSWFS,
                            strQXLX, decLL, bLastPirod, gOverDays);
                    }
                    else//后收
                    {
                        objPlan.pro_pay_rate = getRepayInterest(
                            out strTipB, decJKJE, dtStartDate, dtEndDate, strSWFS,
                            strQXLX, decLL, bLastPirod, gOverDays);
                    }

                }
                #endregion

                objPlan.pro_pay_date = strRegionTime == "1" ? dtStartDate : dtEndDate;

                dtStartDate = dtEndDate;                                    //起息日
                dtEndDate = GetNextMonthCollectDate(datSCHKR, dtStartDate, nMonths);             //止息日

                objPlan.pro_pay_service_fee = decGlf;//应还服务费
                //应还总额 = 应还本金 + 应还利息 + 应还服务费
                objPlan.pro_pay_total = objPlan.pro_pay_money + objPlan.pro_pay_rate + objPlan.pro_pay_service_fee;
                Sumpro_pay_rate += objPlan.pro_pay_rate;
                planList.Add(objPlan);
            }

            strTip = strTipB;
            return planList;
        }
        #endregion

        #region 等额本息
        /// <summary>
        /// 等额本息
        /// 创建者：吕小东
        /// 创建时间：2014年9月25日13:06:30
        /// </summary>
        /// <param name="strTip">提示信息(当提示信息不为“生成成功！”时，都应弹出此提示信息。)</param>
        /// <param name="strHKFS">还款方式(1：等额本息 2：按月还息到期还本 3：等额本金)</param>
        /// <param name="strSWFS">算尾方式(0.算头不算尾；1.算头算尾) </param>
        /// <param name="strQXLX">期限类型(计算周期)(1：年 2：月 3：日)</param>
        /// <param name="decJKJE">借款金额</param>
        /// <param name="decLL">利率（年利率）</param>
        /// <param name="datJKKSR">借款开始日（放款日期）</param>
        /// <param name="datJKJZR">借款截止日（等于 放款日期+借款期限）</param>
        /// <param name="intHKQS">还款期数（1.借款期限不满一个月只能生产1期。2.多余1个月比如5个月，月数除以期数必须能整除。）</param>
        /// <param name="strJSFS">结算方式(1：固定还款日； 2：非固定还款日)(1：固定还款日（1.只要是放款日期小于等于15号，下月1号还款。2.只要是放款日期大于15号，下月16号还款。）；2：非固定还款日（放款日期。）)</param>
        /// <param name="datSCHKR">首次还款日</param>
        /// <param name="intGlfXmlx">项目类型(1.个人信用贷；2.汽车抵押贷；3.房产抵押贷；4.企业经营贷)</param>
        /// <param name="intJKYS">借款月数</param>
        /// <param name="decGlf">管理费</param>
        /// <returns>还款计划表集合对象</returns>
        private List<PRO_loan_plan> GetPlanSameCorpusInterest(out string strTip, string strHKFS,
            string strSWFS, string strQXLX, decimal decJKJE, decimal decLL,
            DateTime datJKKSR, DateTime datJKJZR, int intHKQS, string strJSFS,
            DateTime datSCHKR, int intGlfXmlx, int intJKYS, decimal decGlf)
        {
            string strTipB = gTip;

            DateTime dtLoanStartDate = datJKKSR;            //借款开始日
            DateTime dtLoanEndDate = datJKJZR;              //借款截止日
            Decimal decSettMoney = decJKJE;                 //借款金额
            Decimal decYearRate = decLL;                    //年利率
            Decimal decMonthRate = decYearRate / 12 / 100;  //月利率
            string strRegionTime = gIncomeMethods;          //付息模式：1.预收；2.后收

            Decimal decMoneyPerMonth = CommonHelper.Round(
                decSettMoney * decMonthRate * (decimal)Math.Pow((double)(1 + decMonthRate), intHKQS)
                    / (decimal)(Math.Pow((double)(1 + decMonthRate), intHKQS) - 1)
                    , gRetainedDigit);

            Decimal decInterestPerMonth = 0m;
            Decimal decCapitalPerMonth = 0m;
            List<PRO_loan_plan> planList = new List<PRO_loan_plan>();

            DateTime dtStartDate = dtLoanStartDate;         //起息日
            DateTime dtEndDate = datSCHKR;                  //止息日

            if (intHKQS == 1)
            {
                strTip = strTipB;
                return GetOneTimesPlan(out strTipB,
                       strHKFS, strSWFS, strQXLX, decJKJE, decLL, datJKKSR, datJKJZR,
                          intHKQS, strJSFS, datSCHKR, intGlfXmlx, intJKYS, decGlf);
            }
            //两次还款之间的月数
            int nMonths = CommonHelper.RoundInt(GetBetweenMonths(datSCHKR, dtLoanEndDate) / (intHKQS - 1));
            for (int ii = 0; ii < intHKQS; ii++)
            {
                PRO_loan_plan objPlan = new PRO_loan_plan();

                //避免最后尾数不正
                if (ii == intHKQS - 1)
                {
                    decInterestPerMonth = CommonHelper.Round(decSettMoney * decMonthRate, gRetainedDigit);
                    decCapitalPerMonth = CommonHelper.Round(decSettMoney, gRetainedDigit);
                    decSettMoney = CommonHelper.Round(0, gRetainedDigit);
                    objPlan.pro_pay_total = decCapitalPerMonth + decInterestPerMonth + decGlf;             //应还总额
                }
                else
                {
                    decInterestPerMonth = CommonHelper.Round(decSettMoney * decMonthRate, gRetainedDigit);
                    decCapitalPerMonth = CommonHelper.Round(decMoneyPerMonth - decInterestPerMonth, gRetainedDigit);
                    decSettMoney = decSettMoney - decCapitalPerMonth;
                    objPlan.pro_pay_total = decMoneyPerMonth + decGlf;                                       //应还总额
                }
                objPlan.pro_pay_money = decCapitalPerMonth;                                        //应还本金 
                objPlan.pro_pay_rate = decInterestPerMonth;                                     //应还利息
                objPlan.pro_pay_service_fee = decGlf;//应还服务费
                objPlan.pro_loan_period = ii + 1;                                                    //期数
                objPlan.pro_is_clear = false;                                                            //是否还清：1：是 0：否
                objPlan.pro_pay_type = DataDictionary.repaymenstate_Normal;          //还款状态(27：正常还款 29：平台代还 28：强制还款)

                planList.Add(objPlan);

                if (strRegionTime == "1") //预收
                {
                    objPlan.pro_pay_date = dtStartDate;
                }
                else //后收
                {
                    objPlan.pro_pay_date = dtEndDate;
                }

                dtStartDate = (DateTime)(objPlan.pro_pay_date);                                    //起息日
                if (ii == intHKQS - 1)
                {
                    dtEndDate = dtLoanEndDate;                                                      //止息日
                    objPlan.pro_pay_date = dtEndDate;
                }
                else
                {
                    dtEndDate = GetNextMonthCollectDate(datSCHKR, dtStartDate, nMonths);         //止息日
                }
            }

            strTip = strTipB;
            return planList;
        }
        #endregion

        #region 根据放款日期、借款期限、期限类型得到到期日期
        /// <summary>
        /// 方法：根据放款日期、借款期限、期限类型得到到期日期
        /// 开发者：吕小东
        /// 开发时间：2014年10月30日16:30:04
        /// </summary>
        /// <param name="strQXLX">1.期限类型(1：年 2：月 3：日)</param>
        /// <param name="datJKKSR">2.放款日期</param>
        /// <param name="loanPeriod">3.借款期限</param>
        /// <returns>到期日期</returns>
        private DateTime GetEndDateByStartDate(string strQXLX, DateTime datJKKSR, int loanPeriod)
        {
            DateTime dtEndDate = datJKKSR;
            if (strQXLX == "1")//期限类型(1：年 2：月 3：日)
            {
                dtEndDate = dtEndDate.AddYears(loanPeriod);
            }
            else if (strQXLX == "2")
            {
                dtEndDate = dtEndDate.AddMonths(loanPeriod);
            }
            else if (strQXLX == "3")
            {
                dtEndDate = dtEndDate.AddDays(loanPeriod);
            }
            return dtEndDate;
        }
        #endregion

        #region 一期还款[特例]
        /// <summary>
        /// 一期还款[特例]
        /// 创建者：吕小东
        /// 创建时间：2014年9月25日13:06:30
        /// </summary>
        /// <param name="strTip">提示信息(当提示信息不为“生成成功！”时，都应弹出此提示信息。)</param>
        /// <param name="strHKFS">还款方式(1：等额本息 2：按月还息到期还本 3：等额本金)</param>
        /// <param name="strSWFS">算尾方式(0.算头不算尾；1.算头算尾) </param>
        /// <param name="strQXLX">期限类型(计算周期)(1：年 2：月 3：日)</param>
        /// <param name="decJKJE">借款金额</param>
        /// <param name="decLL">利率（年利率）</param>
        /// <param name="datJKKSR">借款开始日（放款日期）</param>
        /// <param name="datJKJZR">借款截止日（等于 放款日期+借款期限）</param>
        /// <param name="intHKQS">还款期数（1.借款期限不满一个月只能生产1期。2.多余1个月比如5个月，月数除以期数必须能整除。）</param>
        /// <param name="strJSFS">结算方式(1：固定还款日； 2：非固定还款日)(1：固定还款日（1.只要是放款日期小于等于15号，下月1号还款。2.只要是放款日期大于15号，下月16号还款。）；2：非固定还款日（放款日期。）)</param>
        /// <param name="datSCHKR">首次还款日</param>
        /// <param name="intGlfXmlx">项目类型(1.个人信用贷；2.汽车抵押贷；3.房产抵押贷；4.企业经营贷)</param>
        /// <param name="intJKYS">借款月数</param>
        /// <param name="decGlf">管理费</param>
        /// <returns>还款计划表集合对象</returns>
        private List<PRO_loan_plan> GetOneTimesPlan(out string strTip, string strHKFS,
            string strSWFS, string strQXLX, decimal decJKJE, decimal decLL,
            DateTime datJKKSR, DateTime datJKJZR, int intHKQS, string strJSFS,
            DateTime datSCHKR, int intGlfXmlx, int intJKYS, decimal decGlf)
        {
            string strTipB = gTip;
            List<PRO_loan_plan> planList = new List<PRO_loan_plan>();
            PRO_loan_plan objPlan = new PRO_loan_plan();

            #region  一期还款

            objPlan.pro_pay_money = CommonHelper.Round((Decimal)(decJKJE), gRetainedDigit);  //应还本金
            //应还利息
            objPlan.pro_pay_rate = getRepayInterest(out strTipB,
                  decJKJE, datJKKSR, datJKJZR, strSWFS, strQXLX
                , decLL, true, gOverDays);

            objPlan.pro_pay_service_fee = decGlf;   //应还服务费(管理费)
            objPlan.pro_pay_total = objPlan.pro_pay_money + objPlan.pro_pay_rate + decGlf;//应还总额
            objPlan.pro_pay_date = datJKJZR;        //应还日期
            objPlan.pro_loan_period = 1;            //期数
            objPlan.pro_is_clear = false;               //是否已结清：(1：是 0：否)
            objPlan.pro_pay_type = DataDictionary.repaymenstate_Normal;          //还款状态(27：正常还款 29：平台代还 28：强制还款)

            #endregion

            planList.Add(objPlan);

            strTip = strTipB;
            return planList;
        }
        #endregion

        #region 计算利息
        /// <summary>
        /// 计算利息
        /// 创建者：吕小东
        /// 创建时间：2014年9月25日13:06:30
        /// </summary>
        /// <param name="strTip">1.提示信息(当提示信息不为“生成成功！”时，都应弹出此提示信息。)</param>
        /// <param name="decSettMoney">2.结欠金额（只有一期为借款金额）</param>
        /// <param name="datValueDate">3.起息日</param>
        /// <param name="datEndDate">4.止息日期</param>
        /// <param name="strSWFS">5.算尾方式(0.算头不算尾；1.算头算尾) </param>
        /// <param name="strQXLX">6.计算周期(期限类型):(1：年 2：月 3：日)</param>
        /// <param name="decLL">7.利率：年利率月利率日利率% </param>
        /// <param name="bLastReplay">8.是否最后一期收款</param>
        /// <param name="nCountDaysAsMonth">9.超过多少天算一个月</param>
        /// <returns>利息</returns>
        private Decimal getRepayInterest(out string strTip,
              Decimal decSettMoney, DateTime datValueDate, DateTime datEndDate, string strSWFS
            , string strQXLX, Decimal decLL, bool bLastReplay, int nCountDaysAsMonth)
        {
            string strTipB = gTip;
            //天数
            int nDays = DaysDiff(out strTipB, datValueDate, datEndDate, strSWFS, strQXLX, bLastReplay, nCountDaysAsMonth);
            Decimal decInterest = 0.0m;
            //利息 = 本金*日利率(年利率/360/100)*天数 
            decInterest = decSettMoney * nDays * decLL / 360 / 100;

            strTip = strTipB;
            return Decimal.Parse(CommonHelper.getInterest(decInterest, 0));
        }
        #endregion

        #region 计算天数
        /// <summary>
        /// 计算天数
        /// 创建者：吕小东
        /// 创建时间：2014年9月25日13:06:30
        /// </summary>
        /// <param name="strTip">1.提示信息(当提示信息不为“生成成功！”时，都应弹出此提示信息。)</param>
        /// <param name="datValueDate">2.起息日</param>
        /// <param name="datEndDate">3.止息日期</param>
        /// <param name="strSWFS">4.算尾方式(0.算头不算尾；1.算头算尾) </param>
        /// <param name="strQXLX">5.计算周期(期限类型):(1：年 2：月 3：日)</param>
        /// <param name="bLastReplay">6.是否最后一期收款</param>
        /// <param name="nCountDaysAsMonth">7.超过多少天算一个月</param>
        /// <returns>天数</returns>
        private int DaysDiff(out string strTip,
              DateTime datValueDate, DateTime datEndDate, string strSWFS
            , string strQXLX, bool bLastReplay, int nCountDaysAsMonth)
        {
            int nDays = 0;
            string strTipB = gTip;

            switch (strQXLX)
            {
                case "1": //计算周期：年
                case "2": //计算周期：自然月
                    //比较剩余天数是否大于规定的天数，算月加上剩余天数
                    nDays = GetMonthAndTailDays(bLastReplay, strSWFS, datValueDate, datEndDate, nCountDaysAsMonth);
                    break;
                case "3": //计算周期：日  止息日期-起息日              
                    nDays = (datEndDate - datValueDate).Days;
                    if (bLastReplay && int.Parse(strSWFS) == 1)
                    {
                        nDays++;
                    }
                    break;
                default:
                    strTipB = "期限类型不正确！";
                    break;
            }

            strTip = strTipB;
            return nDays;
        }
        #endregion

        #region 取自然月的情况的总天数
        /// <summary>
        /// 取自然月的情况的总天数
        /// 创建者：吕小东
        /// 创建时间：2014年9月25日13:06:30
        /// </summary>
        /// <param name="bLastReplay">1.是否最后一期收款</param>
        /// <param name="strSWFS">2.算尾方式(0.算头不算尾；1.算头算尾) </param>
        /// <param name="datValueDate">3.起息日</param>
        /// <param name="datEndDate">4.止息日期</param>
        /// <param name="nCountDaysAsMonth">5.超过多少天算一个月</param>
        /// <returns>自然月的总天数</returns>
        private int GetMonthAndTailDays(bool bLastReplay, string strSWFS
            , DateTime datValueDate, DateTime datEndDate, int nCountDaysAsMonth)
        {
            int nTailDays = 0;
            int nYears = datEndDate.Year - datValueDate.Year;
            int nMonths = datEndDate.Month - datValueDate.Month;
            int nDays = 0;

            //止息日期和起息日的月份差
            int nMonthDiff = nYears * 12 + nMonths;
            DateTime tempDate = datValueDate.AddMonths(nMonthDiff);


            //1:起息日 1/10；止息日期 1/25
            if (nMonthDiff == 0)
            {
                nDays = (datEndDate - datValueDate).Days;
                //最后一期收款并且算尾方式：算尾
                if (bLastReplay && int.Parse(strSWFS) == 1)
                {
                    nDays++;
                }
                //止息日期-起息日
                return getTailDays(nDays, nCountDaysAsMonth);
            }

            //2:起息日 1/10；止息日期 5/8
            if (tempDate > datEndDate)
            {
                nDays = (datEndDate - datValueDate.AddMonths(nMonthDiff - 1)).Days;
                //最后一期收款并且算尾方式：算尾
                if (bLastReplay && int.Parse(strSWFS) == 1)
                {
                    nDays++;
                }
                nTailDays = (nMonthDiff - 1) * 30
                    + getTailDays(nDays, nCountDaysAsMonth);

            }

            //3:起息日 1/10；止息日期 5/28
            else if (tempDate < datEndDate)
            {
                nDays = (datEndDate - datValueDate.AddMonths(nMonthDiff)).Days;
                //最后一期收款并且算尾方式：算尾
                if (bLastReplay && int.Parse(strSWFS) == 1)
                {
                    nDays++;
                }


                nTailDays = nMonthDiff * 30
                    + getTailDays(nDays, nCountDaysAsMonth);

            }

            //4:起息日 1/10；止息日期 5/10
            else if (tempDate == datEndDate)
            {

                nTailDays = nMonthDiff * 30;
            }

            return nTailDays;
        }
        #endregion

        #region 取自然月的尾数部分
        /// <summary>
        /// 取自然月的尾数部分
        /// 创建者：吕小东
        /// 创建时间：2014年9月25日13:06:30
        /// </summary>
        /// <param name="nDays">1.超过天数</param>
        /// <param name="nCountDaysAsMonth">2.规定天数</param>
        /// <returns>尾部天数</returns>
        private int getTailDays(int nDays, int nCountDaysAsMonth)
        {
            ////超过N天算一个月
            if (nDays >= nCountDaysAsMonth)
            {
                //直接返回一个月
                return 30;
            }
            else
            {
                //直接返回不到一个月的天数
                return nDays;
            }
        }
        #endregion

        #region 获取下次应收日期
        /// <summary>
        /// 获取下次应收日期
        /// 创建者：吕小东
        /// 创建时间：2014年9月25日13:06:30
        /// </summary>
        /// <param name="dtFirstDate">1.首次还款日</param>
        /// <param name="dtStartDate">2.起息日</param>
        /// <param name="nMonths">3.两次还款之间的月数</param>
        /// <returns>下次应收日期</returns>
        private DateTime GetNextMonthCollectDate(DateTime dtFirstDate, DateTime dtStartDate, int nMonths)
        {
            //   dtFirstDate 01/29 dtStartDate 2/28
            bool bFirstLastDate = false;
            //bool bStartLastDate = false;

            int nDaysOfFirstDate = DateTime.DaysInMonth(dtFirstDate.Year, dtFirstDate.Month);

            DateTime dtLastDateOfFirstDate = Convert.ToDateTime(dtFirstDate.Year.ToString()
             + "-" + dtFirstDate.Month.ToString() + "-" + nDaysOfFirstDate.ToString());

            if ((dtLastDateOfFirstDate - dtFirstDate).Days == 0)
                bFirstLastDate = true;

            int nDays1 = DateTime.DaysInMonth(dtStartDate.Year, dtStartDate.Month);
            DateTime dtStartDateMonthLastDate = Convert.ToDateTime(dtStartDate.Year.ToString()
                + "-" + dtStartDate.Month.ToString() + "-" + nDays1.ToString());

            DateTime dtTempDate = dtStartDate.AddMonths(nMonths);

            //if ((dtStartDateMonthLastDate - dtStartDate).Days == 0)
            //{
            //    bStartLastDate = true;
            //}

            //一个月的最后一天 01/31
            if (bFirstLastDate)
            {
                int nDays2 = DateTime.DaysInMonth(dtTempDate.Year, dtTempDate.Month);

                return Convert.ToDateTime(dtTempDate.Year.ToString()
                        + "-" + dtTempDate.Month.ToString() + "-" + nDays2.ToString());
            }
            else
            {
                int nFirstDay = dtFirstDate.Day;

                int nDays3 = DateTime.DaysInMonth(dtTempDate.Year, dtTempDate.Month);

                //1/30 2/28
                if (nDays3 <= nFirstDay)
                {
                    return dtStartDate.AddMonths(nMonths);
                }
                //1/29 2/28=>3/29
                else
                {
                    return Convert.ToDateTime(dtTempDate.Year.ToString()
                       + "-" + dtTempDate.Month.ToString() + "-" + nFirstDay.ToString());
                }
            }
        }
        #endregion

        #region 两次还款之间的月数
        /// <summary>
        /// 两次还款之间的月数
        /// 创建者：吕小东
        /// 创建时间：2014年9月25日13:06:30
        /// </summary>
        /// <param name="dtStarDate">1.起日</param>
        /// <param name="dtEndDate">2.止日</param>
        /// <returns>月数</returns>
        private Decimal GetBetweenMonths(DateTime dtStarDate, DateTime dtEndDate)
        {
            int nYears = dtEndDate.Year - dtStarDate.Year;
            int nMonths = dtEndDate.Month - dtStarDate.Month;

            //止日和起日的月份差
            int nMonthDiff = nYears * 12 + nMonths;

            return nMonthDiff + 0m;
        }

        
        #endregion

        #endregion
    }
}
