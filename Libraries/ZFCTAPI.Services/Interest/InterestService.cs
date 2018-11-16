using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using ZFCTAPI.Core;
using ZFCTAPI.Data.Interest;
using ZFCTAPI.Data.PRO;

namespace ZFCTAPI.Services.Interest
{
    public interface IInterestService
    {
        /// <summary>
        /// 生成债权关系计划表
        /// </summary>
        /// <param name="calculateCreditorRequest">需要的参数</param>
        /// <returns></returns>
        GenerateCreditorResult GenerateCreditor(CalculateCreditorRequest calculateCreditorRequest);
    }
    public class  InterestService:IInterestService
    {

        public int DecimalPlace = 2;      //保留位数
        private const int OverDays = 32; //超过多少天算一个月
        private readonly string _gTip = "生成成功！";  //默认操作提示
        private string _gIncomeMethods = "2"; //付息模式：1.预收；2.后收
        public GenerateCreditorResult GenerateCreditor(CalculateCreditorRequest calculateCreditorRequest)
        {
            #region 生成借款人还款计划表
            var flag = true;
            var strTip = _gTip;   //提示信息
            #region 相关变量
            //还款方式(1：等额本息 2：按月还息到期还本 3：等额本金)
            var repayment = calculateCreditorRequest.RepaymentType;
            //算尾方式(0.算头不算尾；1.算头算尾)
            var strSWFS = "1";
            //期限类型(计算周期)(1：年 2：月 3：日)
            var period = "";
            if (calculateCreditorRequest.InterestBearing == DataDictionary.deadlinetype_Day)
            {
                period = "3";
            }
            else if (calculateCreditorRequest.InterestBearing == DataDictionary.deadlinetype_Month)
            {
                period = "2";
            }
            //借款金额
            var borrowBalance = calculateCreditorRequest.LoanAmount;                 
            //利率（年利率）
            var interestRate = calculateCreditorRequest.LoanRate;                    
            //借款期限
            var loanPeriod = calculateCreditorRequest.LoanDurTime;                 
            //借款开始日（放款日期）
            var startDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));   
            //借款截止日（等于 放款日期+借款期限）
            var endDate = GetEndDateByStartDate(period, startDate, loanPeriod);
            //还款期数（1.借款期限不满一个月只能生产1期。2.多余1个月比如5个月，月数除以期数必须能整除。）
            var repaymentTimes = repayment == "3" ? 1 : calculateCreditorRequest.RepaymentPeriods;
            //结算方式(1：固定还款日； 2：非固定还款日)(1：固定还款日（1.只要是放款日期小于等于15号，下月1号还款。2.只要是放款日期大于15号，下月16号还款。）；2：非固定还款日（放款日期。）)
            var settleType = "";
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
            #endregion

            #region 相关变量赋值
            #region 首次还款日

            switch (settleType)
            {
                case "1":
                    firstRepayment = Convert.ToDateTime(startDate.AddMonths(1).ToString("yyyy-MM") + fixedRepayment);
                    break;
                case "2":
                    firstRepayment = startDate.AddMonths(1);
                    break;
                default:
                    strTip = "结算方式不正确！";
                    flag = false;
                    break;
            }

            #endregion

            #region 费用设置表
            if (flag)
            {
                switch (strGlfSffs)
                {
                    case "1":
                        decGlf = borrowBalance * decGlfSfz / 100;
                        break;
                    case "2":
                        decGlf = decGlfSfz;
                        break;
                    default:
                        strTip = "收费方式不正确！";
                        flag = false;
                        break;
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

            return result;
        }

        #region 数据处理
        /// <summary>
        /// 根据放款日期、借款期限、期限类型得到到期日期
        /// </summary>
        /// <param name="strQxlx">1.期限类型(1：年 2：月 3：日)</param>
        /// <param name="dateJkksr">2.放款日期</param>
        /// <param name="loanPeriod">3.借款期限</param>
        /// <returns>到期日期</returns>
        private DateTime GetEndDateByStartDate(string strQxlx, DateTime dateJkksr, int loanPeriod)
        {
            var dtEndDate = dateJkksr;
            //期限类型(1：年 2：月 3：日)
            switch (strQxlx)
            {
                case "1":
                    dtEndDate = dtEndDate.AddYears(loanPeriod);
                    break;
                case "2":
                    dtEndDate = dtEndDate.AddMonths(loanPeriod);
                    break;
                case "3":
                    dtEndDate = dtEndDate.AddDays(loanPeriod);
                    break;
            }
            return dtEndDate;
        }

        /// <summary>
        /// 四舍五入处理
        /// </summary>
        /// <param name="d">1.需要处理的数据</param>
        /// <param name="i">2.位数</param>
        /// <returns>四舍五入后的值</returns>
        private static decimal Round(decimal d, int i)
        {
            if (d >= 0)
            {
                d += (decimal)(5 * Math.Pow(10, -(i + 1)));
            }
            else
            {
                d += (decimal)(-5 * Math.Pow(10, -(i + 1)));
            }
            var str = d.ToString(CultureInfo.InvariantCulture);
            var strs = str.Split('.');
            var idot = str.IndexOf('.');
            var prestr = strs[0];
            var poststr = strs[1];
            if (poststr.Length > i)
            {
                poststr = str.Substring(idot + 1, i);
            }
            var strd = prestr + "." + poststr;
            d = decimal.Parse(strd);
            return d;
        }

        /// <summary>
        /// 截取小数位
        /// </summary>
        /// <param name="decInterest">金额</param>
        /// <param name="nComma">类型</param>
        /// <returns>结果</returns>
        private static string GetInterest(decimal decInterest, int nComma)
        {
            //取整截尾，四舍五入到整，到分四舍五入
            return decInterest.ToString(nComma == 1 ? "#,###.#0" : "###.#0");
        }

        /// <summary>
        /// 计算利息
        /// </summary>
        /// <param name="strTip">1.提示信息(当提示信息不为“生成成功！”时，都应弹出此提示信息。)</param>
        /// <param name="decSettMoney">2.结欠金额（只有一期为借款金额）</param>
        /// <param name="datValueDate">3.起息日</param>
        /// <param name="datEndDate">4.止息日期</param>
        /// <param name="strSwfs">5.算尾方式(0.算头不算尾；1.算头算尾) </param>
        /// <param name="strQxlx">6.计算周期(期限类型):(1：年 2：月 3：日)</param>
        /// <param name="decLl">7.利率：年利率月利率日利率% </param>
        /// <param name="bLastReplay">8.是否最后一期收款</param>
        /// <param name="nCountDaysAsMonth">9.超过多少天算一个月</param>
        /// <returns>利息</returns>
        private decimal GetRepayInterest(out string strTip,
            decimal decSettMoney, DateTime datValueDate, DateTime datEndDate, string strSwfs
            , string strQxlx, Decimal decLl, bool bLastReplay, int nCountDaysAsMonth)
        {
            //天数
            var nDays = DaysDiff(out var strTipB, datValueDate, datEndDate, strSwfs, strQxlx, bLastReplay, nCountDaysAsMonth);
            var decInterest = 0.0m;
            //利息 = 本金*日利率(年利率/360/100)*天数 
            decInterest = decSettMoney * nDays * decLl / 360 / 100;
            strTip = strTipB;
            return decimal.Parse(GetInterest(decInterest, 0));
        }

        /// <summary>
        /// 取自然月的尾数部分
        /// </summary>
        /// <param name="nDays">1.超过天数</param>
        /// <param name="nCountDaysAsMonth">2.规定天数</param>
        /// <returns>尾部天数</returns>
        private int GetTailDays(int nDays, int nCountDaysAsMonth)
        {
            ////超过N天算一个月
            return nDays >= nCountDaysAsMonth ? 30 : nDays;
        }

        /// <summary>
        /// 取自然月的情况的总天数
        /// </summary>
        /// <param name="bLastReplay">1.是否最后一期收款</param>
        /// <param name="strSwfs">2.算尾方式(0.算头不算尾；1.算头算尾) </param>
        /// <param name="datValueDate">3.起息日</param>
        /// <param name="datEndDate">4.止息日期</param>
        /// <param name="nCountDaysAsMonth">5.超过多少天算一个月</param>
        /// <returns>自然月的总天数</returns>
        public int GetMonthAndTailDays(bool bLastReplay, string strSwfs
            , DateTime datValueDate, DateTime datEndDate, int nCountDaysAsMonth)
        {
            var nTailDays = 0;
            var nYears = datEndDate.Year - datValueDate.Year;
            var nMonths = datEndDate.Month - datValueDate.Month;
            var nDays = 0;

            //止息日期和起息日的月份差
            var nMonthDiff = nYears * 12 + nMonths;
            var tempDate = datValueDate.AddMonths(nMonthDiff);
            //1:起息日 1/10；止息日期 1/25
            if (nMonthDiff == 0)
            {
                nDays = (datEndDate - datValueDate).Days;
                //最后一期收款并且算尾方式：算尾
                if (bLastReplay && int.Parse(strSwfs) == 1)
                {
                    nDays++;
                }
                //止息日期-起息日
                return GetTailDays(nDays, nCountDaysAsMonth);
            }

            //2:起息日 1/10；止息日期 5/8
            if (tempDate > datEndDate)
            {
                nDays = (datEndDate - datValueDate.AddMonths(nMonthDiff - 1)).Days;
                //最后一期收款并且算尾方式：算尾
                if (bLastReplay && int.Parse(strSwfs) == 1)
                {
                    nDays++;
                }
                nTailDays = (nMonthDiff - 1) * 30
                    + GetTailDays(nDays, nCountDaysAsMonth);

            }
            //3:起息日 1/10；止息日期 5/28
            else if (tempDate < datEndDate)
            {
                nDays = (datEndDate - datValueDate.AddMonths(nMonthDiff)).Days;
                //最后一期收款并且算尾方式：算尾
                if (bLastReplay && int.Parse(strSwfs) == 1)
                {
                    nDays++;
                }
                nTailDays = nMonthDiff * 30
                    + GetTailDays(nDays, nCountDaysAsMonth);

            }
            //4:起息日 1/10；止息日期 5/10
            else if (tempDate == datEndDate)
            {

                nTailDays = nMonthDiff * 30;
            }

            return nTailDays;
        }

        /// <summary>
        /// 计算天数
        /// </summary>
        /// <param name="strTip">1.提示信息(当提示信息不为“生成成功！”时，都应弹出此提示信息。)</param>
        /// <param name="datValueDate">2.起息日</param>
        /// <param name="datEndDate">3.止息日期</param>
        /// <param name="strSwfs">4.算尾方式(0.算头不算尾；1.算头算尾) </param>
        /// <param name="strQxlx">5.计算周期(期限类型):(1：年 2：月 3：日)</param>
        /// <param name="bLastReplay">6.是否最后一期收款</param>
        /// <param name="nCountDaysAsMonth">7.超过多少天算一个月</param>
        /// <returns>天数</returns>
        public int DaysDiff(out string strTip,
            DateTime datValueDate, DateTime datEndDate, string strSwfs
            , string strQxlx, bool bLastReplay, int nCountDaysAsMonth)
        {
            var nDays = 0;
            var strTipB = _gTip;

            switch (strQxlx)
            {
                case "1": //计算周期：年
                case "2": //计算周期：自然月
                    //比较剩余天数是否大于规定的天数，算月加上剩余天数
                    nDays = GetMonthAndTailDays(bLastReplay, strSwfs, datValueDate, datEndDate, nCountDaysAsMonth);
                    break;
                case "3": //计算周期：日  止息日期-起息日              
                    nDays = (datEndDate - datValueDate).Days;
                    if (bLastReplay && int.Parse(strSwfs) == 1)
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

        /// <summary>
        /// 一期还款[特例]
        /// </summary>
        /// <param name="strTip">提示信息(当提示信息不为“生成成功！”时，都应弹出此提示信息。)</param>
        /// <param name="strHkfs">还款方式(1：等额本息 2：按月还息到期还本 3：等额本金)</param>
        /// <param name="strSwfs">算尾方式(0.算头不算尾；1.算头算尾) </param>
        /// <param name="strQxlx">期限类型(计算周期)(1：年 2：月 3：日)</param>
        /// <param name="decJkje">借款金额</param>
        /// <param name="decLl">利率（年利率）</param>
        /// <param name="datJkksr">借款开始日（放款日期）</param>
        /// <param name="datJkjzr">借款截止日（等于 放款日期+借款期限）</param>
        /// <param name="intHkqs">还款期数（1.借款期限不满一个月只能生产1期。2.多余1个月比如5个月，月数除以期数必须能整除。）</param>
        /// <param name="strJsfs">结算方式(1：固定还款日； 2：非固定还款日)(1：固定还款日（1.只要是放款日期小于等于15号，下月1号还款。2.只要是放款日期大于15号，下月16号还款。）；2：非固定还款日（放款日期。）)</param>
        /// <param name="datSchkr">首次还款日</param>
        /// <param name="intGlfXmlx">项目类型(1.个人信用贷；2.汽车抵押贷；3.房产抵押贷；4.企业经营贷)</param>
        /// <param name="intJkys">借款月数</param>
        /// <param name="decGlf">管理费</param>
        /// <returns>还款计划表集合对象</returns>
        public List<PRO_loan_plan> GetOneTimesPlan(out string strTip, string strHkfs,
            string strSwfs, string strQxlx, decimal decJkje, decimal decLl,
            DateTime datJkksr, DateTime datJkjzr, int intHkqs, string strJsfs,
            DateTime datSchkr, int intGlfXmlx, int intJkys, decimal decGlf)
        {
            var planList = new List<PRO_loan_plan>();
            var objPlan = new PRO_loan_plan
            {
                //应还本金
                pro_pay_money = Round((decimal)(decJkje), DecimalPlace),
                //应还利息
                pro_pay_rate = GetRepayInterest(out var strTipB,
                    decJkje, datJkksr, datJkjzr, strSwfs, strQxlx
                    , decLl, true, OverDays),
                //应还服务费(管理费)
                pro_pay_service_fee = decGlf
            };

            #region  一期还款
            objPlan.pro_pay_total = objPlan.pro_pay_money + objPlan.pro_pay_rate + decGlf;//应还总额
            objPlan.pro_pay_date = datJkjzr;        //应还日期
            objPlan.pro_loan_period = 1;            //期数
            objPlan.pro_is_clear = false;               //是否已结清：(1：是 0：否)
            objPlan.pro_pay_type = DataDictionary.repaymenstate_Normal;          //还款状态(27：正常还款 29：平台代还 28：强制还款)
            #endregion

            planList.Add(objPlan);

            strTip = strTipB;
            return planList;
        }

        /// <summary>
        /// 金额类型转换为整型
        /// </summary>
        /// <param name="d">decimal 型数据</param>
        /// <returns>int 型数据</returns>
        public static int RoundInt(decimal d)
        {
            return (int)Round(d, 0);
        }

        /// <summary>
        /// 两次还款之间的月数
        /// </summary>
        /// <param name="dtStarDate">1.起日</param>
        /// <param name="dtEndDate">2.止日</param>
        /// <returns>月数</returns>
        public static decimal GetBetweenMonths(DateTime dtStarDate, DateTime dtEndDate)
        {
            var nYears = dtEndDate.Year - dtStarDate.Year;
            var nMonths = dtEndDate.Month - dtStarDate.Month;
            //止日和起日的月份差
            var nMonthDiff = nYears * 12 + nMonths;
            return nMonthDiff + 0m;
        }

        /// <summary>
        /// 获取下次应收日期
        /// </summary>
        /// <param name="dtFirstDate">1.首次还款日</param>
        /// <param name="dtStartDate">2.起息日</param>
        /// <param name="nMonths">3.两次还款之间的月数</param>
        /// <returns>下次应收日期</returns>
        private DateTime GetNextMonthCollectDate(DateTime dtFirstDate, DateTime dtStartDate, int nMonths)
        {
            var bFirstLastDate = false;
            var nDaysOfFirstDate = DateTime.DaysInMonth(dtFirstDate.Year, dtFirstDate.Month);

            var dtLastDateOfFirstDate = Convert.ToDateTime(dtFirstDate.Year.ToString()
                                                           + "-" + dtFirstDate.Month.ToString() + "-" + nDaysOfFirstDate.ToString());

            if ((dtLastDateOfFirstDate - dtFirstDate).Days == 0)
                bFirstLastDate = true;

            var dtTempDate = dtStartDate.AddMonths(nMonths);
            if (bFirstLastDate)
            {
                var nDays2 = DateTime.DaysInMonth(dtTempDate.Year, dtTempDate.Month);

                return Convert.ToDateTime(dtTempDate.Year.ToString()
                                          + "-" + dtTempDate.Month.ToString() + "-" + nDays2.ToString());
            }
            else
            {
                var nFirstDay = dtFirstDate.Day;
                var nDays3 = DateTime.DaysInMonth(dtTempDate.Year, dtTempDate.Month);
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

        #region 等额本息
        /// <summary>
        /// 等额本息
        /// </summary>
        /// <param name="strTip">提示信息(当提示信息不为“生成成功！”时，都应弹出此提示信息。)</param>
        /// <param name="strHkfs">还款方式(1：等额本息 2：按月还息到期还本 3：等额本金)</param>
        /// <param name="strSwfs">算尾方式(0.算头不算尾；1.算头算尾) </param>
        /// <param name="strQxlx">期限类型(计算周期)(1：年 2：月 3：日)</param>
        /// <param name="decJkje">借款金额</param>
        /// <param name="decLl">利率（年利率）</param>
        /// <param name="datJkksr">借款开始日（放款日期）</param>
        /// <param name="datJkjzr">借款截止日（等于 放款日期+借款期限）</param>
        /// <param name="intHkqs">还款期数（1.借款期限不满一个月只能生产1期。2.多余1个月比如5个月，月数除以期数必须能整除。）</param>
        /// <param name="strJsfs">结算方式(1：固定还款日； 2：非固定还款日)(1：固定还款日（1.只要是放款日期小于等于15号，下月1号还款。2.只要是放款日期大于15号，下月16号还款。）；2：非固定还款日（放款日期。）)</param>
        /// <param name="datSchkr">首次还款日</param>
        /// <param name="intGlfXmlx">项目类型(1.个人信用贷；2.汽车抵押贷；3.房产抵押贷；4.企业经营贷)</param>
        /// <param name="intJkys">借款月数</param>
        /// <param name="decGlf">管理费</param>
        /// <returns>还款计划表集合对象</returns>
        public List<PRO_loan_plan> GetPlanSameCorpusInterest(out string strTip, string strHkfs,
            string strSwfs, string strQxlx, decimal decJkje, decimal decLl,
            DateTime datJkksr, DateTime datJkjzr, int intHkqs, string strJsfs,
            DateTime datSchkr, int intGlfXmlx, int intJkys, decimal decGlf)
        {
            var strTipB = _gTip;
            //借款开始日
            var dtLoanStartDate = datJkksr;
            //借款截止日
            var dtLoanEndDate = datJkjzr;
            //借款金额
            var decSettMoney = decJkje;
            //年利率
            var decYearRate = decLl;
            //月利率
            var decMonthRate = decYearRate / 12 / 100;
            //付息模式：1.预收；2.后收
            var strRegionTime = _gIncomeMethods;          
            var decMoneyPerMonth = Round(
                decSettMoney * decMonthRate * (decimal)Math.Pow((double)(1 + decMonthRate), intHkqs)
                    / (decimal)(Math.Pow((double)(1 + decMonthRate), intHkqs) - 1)
                    , DecimalPlace);

            var planList = new List<PRO_loan_plan>();

            var dtStartDate = dtLoanStartDate;         //起息日
            var dtEndDate = datSchkr;                  //止息日

            if (intHkqs == 1)
            {
                strTip = strTipB;
                return GetOneTimesPlan(out strTipB,
                       strHkfs, strSwfs, strQxlx, decJkje, decLl, datJkksr, datJkjzr,
                          intHkqs, strJsfs, datSchkr, intGlfXmlx, intJkys, decGlf);
            }
            //两次还款之间的月数
            var nMonths = RoundInt(GetBetweenMonths(datSchkr, dtLoanEndDate) / (intHkqs - 1));
            for (var ii = 0; ii < intHkqs; ii++)
            {
                var objPlan = new PRO_loan_plan();

                //避免最后尾数不正
                decimal decInterestPerMonth;
                decimal decCapitalPerMonth;
                if (ii == intHkqs - 1)
                {
                    decInterestPerMonth = Round(decSettMoney * decMonthRate, DecimalPlace);
                    decCapitalPerMonth = Round(decSettMoney, DecimalPlace);
                    decSettMoney = Round(0, DecimalPlace);
                    objPlan.pro_pay_total = decCapitalPerMonth + decInterestPerMonth + decGlf;             //应还总额
                }
                else
                {
                    decInterestPerMonth = Round(decSettMoney * decMonthRate, DecimalPlace);
                    decCapitalPerMonth = Round(decMoneyPerMonth - decInterestPerMonth, DecimalPlace);
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

                objPlan.pro_pay_date = strRegionTime == "1" ? dtStartDate : dtEndDate;

                dtStartDate = (DateTime)(objPlan.pro_pay_date);                                    //起息日
                if (ii == intHkqs - 1)
                {
                    dtEndDate = dtLoanEndDate;                                                      //止息日
                    objPlan.pro_pay_date = dtEndDate;
                }
                else
                {
                    dtEndDate = GetNextMonthCollectDate(datSchkr, dtStartDate, nMonths);         //止息日
                }
            }

            strTip = strTipB;
            return planList;
        }
        #endregion

        #region 按月利息到期还本
        /// <summary>
        /// 按月利息到期还本
        /// </summary>
        /// <param name="strTip">提示信息(当提示信息不为“生成成功！”时，都应弹出此提示信息。)</param>
        /// <param name="strHkfs">还款方式(1：等额本息 2：按月还息到期还本 3：等额本金)</param>
        /// <param name="strSwfs">算尾方式(0.算头不算尾；1.算头算尾) </param>
        /// <param name="strQxlx">期限类型(计算周期)(1：年 2：月 3：日)</param>
        /// <param name="decJkje">借款金额</param>
        /// <param name="decLl">利率（年利率）</param>
        /// <param name="datJkksr">借款开始日（放款日期）</param>
        /// <param name="datJkjzr">借款截止日（等于 放款日期+借款期限）</param>
        /// <param name="intHkqs">还款期数（1.借款期限不满一个月只能生产1期。2.多余1个月比如5个月，月数除以期数必须能整除。）</param>
        /// <param name="strJsfs">结算方式(1：固定还款日； 2：非固定还款日)(1：固定还款日（1.只要是放款日期小于等于15号，下月1号还款。2.只要是放款日期大于15号，下月16号还款。）；2：非固定还款日（放款日期。）)</param>
        /// <param name="datSchkr">首次还款日</param>
        /// <param name="intGlfXmlx">项目类型(1.个人信用贷；2.汽车抵押贷；3.房产抵押贷；4.企业经营贷)</param>
        /// <param name="intJkys">借款月数</param>
        /// <param name="decGlf">管理费</param>
        /// <returns>还款计划表集合对象</returns>
        public List<PRO_loan_plan> GetPlanInterestPerMonthLastCapital(out string strTip, string strHkfs,
            string strSwfs, string strQxlx, decimal decJkje, decimal decLl,
            DateTime datJkksr, DateTime datJkjzr, int intHkqs, string strJsfs,
            DateTime datSchkr, int intGlfXmlx, int intJkys, decimal decGlf)
        {
            var strTipB = _gTip;

            var dtLoanStartDate = datJkksr;    //放款日
            var dtLoanEndDate = datJkjzr;      //到期日
            var decSettMoney = decJkje;          //结欠金额
            var strRegionTime = _gIncomeMethods;              //付息模式：1.预收；2.后收
            var dtStartDate = dtLoanStartDate;
            var dtEndDate = datSchkr;
            decimal? Sumpro_pay_rate = 0.00m;
            if (intHkqs == 1)
            {
                strTip = strTipB;
                return GetOneTimesPlan(out strTipB,
                       strHkfs, strSwfs, strQxlx, decJkje, decLl, datJkksr, datJkjzr,
                          intHkqs, strJsfs, datSchkr, intGlfXmlx, intJkys, decGlf);
            }

            //两次还款之间的月数
            var nMonths = RoundInt(GetBetweenMonths(datSchkr, dtLoanEndDate) / (intHkqs - 1));
            var planList = new List<PRO_loan_plan>();

            //预收
            if (strRegionTime == "1")
            {
                intHkqs = intHkqs + 1;
            }

            for (var ii = 1; ii < intHkqs + 1; ii++)
            {
                //是否最后一期收款
                var objPlan = new PRO_loan_plan
                {
                    //期数
                    pro_loan_period = ii,
                    //是否还清：1：是 0：否
                    pro_is_clear = false,
                    //还款状态(1：正常还款 2：平台代还 3：强制还款)
                    pro_pay_type = DataDictionary.repaymenstate_Normal
                };

                #region  第一期
                if (ii == 1)
                {
                    dtStartDate = dtLoanStartDate;                                                  //起息日
                    dtEndDate = datSchkr;                                                        //止息日

                    //应还利息
                    objPlan.pro_pay_rate = GetRepayInterest(
                        out strTipB, decJkje, dtStartDate, dtEndDate, strSwfs,
                        strQxlx, decLl, false, OverDays);

                    objPlan.pro_pay_money = 0;  //应还本金    
                }
                #endregion  第一期

                #region  最后一期
                else if (ii == intHkqs)
                {
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
                        objPlan.pro_pay_rate = GetRepayInterest(
                            out strTipB, decJkje, dtStartDate, dtEndDate, strSwfs,
                            strQxlx, decLl, true, OverDays) - Sumpro_pay_rate;
                    }

                }
                #endregion

                #region  中间期
                else
                {
                    objPlan.pro_pay_money = 0;  //应还本金

                    if (strRegionTime == "1" && ii == intHkqs - 1)//预收
                    {
                        dtEndDate = dtLoanEndDate;
                        objPlan.pro_pay_rate = GetRepayInterest(
                            out strTipB, decJkje, dtStartDate, dtEndDate, strSwfs,
                            strQxlx, decLl, true, OverDays);
                    }
                    else//后收
                    {
                        objPlan.pro_pay_rate = GetRepayInterest(
                            out strTipB, decJkje, dtStartDate, dtEndDate, strSwfs,
                            strQxlx, decLl, false, OverDays);
                    }

                }
                #endregion

                objPlan.pro_pay_date = strRegionTime == "1" ? dtStartDate : dtEndDate;

                dtStartDate = dtEndDate;                                    //起息日
                dtEndDate = GetNextMonthCollectDate(datSchkr, dtStartDate, nMonths);             //止息日

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


    }
}
